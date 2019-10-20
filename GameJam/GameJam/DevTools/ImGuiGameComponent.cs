using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Events;
using GameJam.Events.DevTools;
using GameJam.Events.InputHandling;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.ImGui;

namespace GameJam.DevTools
{
    public class ImGuiGameComponent : DrawableGameComponent, IEventListener
    {
        private readonly StatisticsProfiler _statisticsProfiler;

        private ImGUIRenderer _renderer;

        private string _cvarEditing = "";

        private List<string> _consoleItems = new List<string>();
        private byte[] _consoleCmdLine = new byte[500];

        public ImGuiGameComponent(Game game, StatisticsProfiler statisticsProfiler) : base(game)
        {
            _statisticsProfiler = statisticsProfiler;

            _renderer = new ImGUIRenderer(game).Initialize().RebuildFontAtlas();

            Console.SetOut(new ConsoleInterceptorWriter(_consoleItems, Console.Out));
        }

        public override void Initialize()
        {
            RegisterEvents();

            base.Initialize();
        }

        protected override void UnloadContent()
        {
            UnregisterEvents();

            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            _renderer.BeginLayout(gameTime);

            DrawCVarWindow();
            DrawPlaybackControls();
            DrawConsole();
            DrawStatistics();

            _renderer.EndLayout();

            base.Draw(gameTime);
        }

        private void RegisterEvents()
        {
            EventManager.Instance.RegisterWildcardListener(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
        }

        private void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            LogEvent(evt);

            KeyboardKeyDownEvent keyboardKeyDownEvent = evt as KeyboardKeyDownEvent;
            if (keyboardKeyDownEvent != null)
            {
                switch (keyboardKeyDownEvent.Key)
                {
                    case Keys.F1:
                        CVars.Get<bool>("debug_show_cvar_viewer") = !CVars.Get<bool>("debug_show_cvar_viewer");
                        return true;
                    case Keys.F2:
                        CVars.Get<bool>("debug_show_playback_controls") = !CVars.Get<bool>("debug_show_playback_controls");
                        return true;
                    case Keys.F3:
                        CVars.Get<bool>("debug_show_statistics") = !CVars.Get<bool>("debug_show_statistics");
                        return true;
                    case Keys.OemTilde:
                        CVars.Get<bool>("debug_show_console") = !CVars.Get<bool>("debug_show_console");
                        return true;
                }
            }

            return false;
        }

        private void DrawCVarWindow()
        {
            if (CVars.Get<bool>("debug_show_cvar_viewer"))
            {
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(500, 400), ImGuiCond.FirstUseEver);
                ImGui.Begin("CVar Viewer", ref CVars.Get<bool>("debug_show_cvar_viewer"));

                if (ImGui.Button("Save##Control"))
                {
                    CVars.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button("Load##Control"))
                {
                    CVars.SynchronizeFromFile();
                }

                string[] names = CVars.GetNames();
                Array.Sort(names);

                ImGui.Columns(3);

                ImGui.Text("CVar");
                ImGui.NextColumn();
                ImGui.Text("Value");
                ImGui.NextColumn();
                ImGui.Text("Default");
                ImGui.NextColumn();

                ImGui.Separator();

                foreach (string name in names)
                {
                    if (ImGui.Button(name))
                    {
                        _cvarEditing = name;
                    }

                    ImGui.NextColumn();

                    ICVar cvar = CVars.RawGet(name);
                    if (_cvarEditing == name)
                    {
                        switch (cvar)
                        {
                            case CVar<byte> numCVar:
                                {
                                    int num = (int)numCVar.Value;
                                    ImGui.InputInt("##" + name, ref num);
                                    numCVar.Value = (byte)num;
                                }
                                break;
                            case CVar<short> numCVar:
                                {
                                    int num = (int)numCVar.Value;
                                    ImGui.InputInt("##" + name, ref num);
                                    numCVar.Value = (short)num;
                                }
                                break;
                            case CVar<int> numCVar:
                                ImGui.InputInt("##" + name, ref numCVar.Value);
                                break;
                            case CVar<float> numCVar:
                                ImGui.InputFloat("##" + name, ref numCVar.Value);
                                break;
                            case CVar<double> numCVar:
                                ImGui.InputDouble("##" + name, ref numCVar.Value);
                                break;
                            case CVar<bool> boolCVar:
                                ImGui.Checkbox("##" + name, ref boolCVar.Value);
                                break;
                            case CVar<Color> colorCVar:
                                {
                                    System.Numerics.Vector3 color = new System.Numerics.Vector3(colorCVar.Value.R / 255.0f,
                                        colorCVar.Value.G / 255.0f,
                                        colorCVar.Value.B / 255.0f);
                                    ImGui.ColorEdit3("##" + name, ref color);
                                    colorCVar.Value.R = (byte)(color.X * 255);
                                    colorCVar.Value.G = (byte)(color.Y * 255);
                                    colorCVar.Value.B = (byte)(color.Z * 255);
                                }
                                break;
                            default:
                                {
                                    string strValue = cvar.Serialize();
                                    byte[] buff = new byte[strValue.Length + 500];
                                    Array.Copy(Encoding.UTF8.GetBytes(strValue), buff, strValue.Length);
                                    ImGui.InputText("##" + name, buff, (uint)buff.Length);
                                    cvar.Deserialize(Encoding.UTF8.GetString(buff, 0, buff.Length));
                                }
                                break;
                        }
                    }
                    else
                    {
                        ImGui.Text(cvar.Serialize());
                    }

                    ImGui.NextColumn();

                    if (ImGui.Button(cvar.SerializeDefault() + "##" + name))
                    {
                        cvar.Reset();
                    }

                    ImGui.NextColumn();
                }

                ImGui.End();
            }
        }

        private void DrawPlaybackControls()
        {
            if (CVars.Get<bool>("debug_show_playback_controls"))
            {
                ImGui.Begin("Playback Controls", ref CVars.Get<bool>("debug_show_playback_controls"), ImGuiWindowFlags.AlwaysAutoResize);

                // These events are trigger because when the game is paused,
                // events that are queued are not dispatched.

                if (ImGui.Button("Pause##Playback"))
                {
                    CVars.Get<bool>("debug_pause_game_updates") = true;
                }
                ImGui.SameLine();
                if (ImGui.Button("Resume##Playback"))
                {
                    CVars.Get<bool>("debug_pause_game_updates") = false;
                }
                ImGui.SameLine();
                if (ImGui.Button("Step##Playback"))
                {
                    EventManager.Instance.TriggerEvent(new StepGameUpdateEvent());
                }
                ImGui.SameLine();
                ImGui.SetNextItemWidth(100);
                ImGui.InputFloat("Time Scale##Playback", ref CVars.Get<float>("debug_update_time_scale"), 0.1f);

                ImGui.End();
            }
        }

        private void LogEvent(IEvent evt)
        {
            Regex regex = new Regex(CVars.Get<string>("debug_console_filter"));
            string evtName = evt.GetType().Name;
            if (!regex.Match(evtName).Success)
            {
                _consoleItems.Add(string.Format("[{0}] {1}", DateTime.Now.ToString("hh:mm:ss"), evt.GetType().Name));
                if (_consoleItems.Count > CVars.Get<int>("debug_max_console_entries"))
                {
                    _consoleItems.RemoveRange(0, _consoleItems.Count - CVars.Get<int>("debug_max_console_entries"));
                }
            }
        }

        private void DrawConsole()
        {
            if (CVars.Get<bool>("debug_show_console"))
            {
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 300), ImGuiCond.FirstUseEver);
                ImGui.Begin("Developer Console", ref CVars.Get<bool>("debug_show_console"));

                float footer_height_to_reserve = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();
                ImGui.BeginChild("ConsoleScrollingRegion",
                    new System.Numerics.Vector2(0, -footer_height_to_reserve),
                    false, ImGuiWindowFlags.AlwaysVerticalScrollbar);
                ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(4, 1));
                for (int i = 0; i < _consoleItems.Count; i++)
                {
                    ImGui.TextUnformatted(_consoleItems[i]);
                }
                if (ImGui.GetScrollY() >= ImGui.GetScrollMaxY())
                {
                    ImGui.SetScrollHereY(1.0f);
                }

                ImGui.PopStyleVar();
                ImGui.EndChild();
                ImGui.Separator();

                if (ImGui.InputText("##ConsoleInput",
                    _consoleCmdLine,
                    (uint)_consoleCmdLine.Length,
                    ImGuiInputTextFlags.EnterReturnsTrue
                        | ImGuiInputTextFlags.EnterReturnsTrue))
                {
                    string cmd = Encoding.UTF8.GetString(_consoleCmdLine).TrimEnd((Char)0);
                    ExecuteCommand(cmd);
                    for (int i = 0; i < _consoleCmdLine.Length; i++)
                    {
                        _consoleCmdLine[i] = 0;
                    }
                }

                ImGui.End();
            }
        }

        private void ExecuteCommand(string rawCmd)
        {
            string[] parts = rawCmd.Split(' ');
            string cvar = parts[0];
            if (Array.IndexOf(CVars.GetNames(), cvar) > -1) {
                string value = rawCmd.Substring(cvar.Length).Trim();
                if (value.Length > 0)
                {
                    string oldValue = CVars.RawGet(cvar).Serialize();
                    CVars.RawGet(cvar).Deserialize(value);
                    Console.WriteLine("Updated `{0}` from '{1}' to '{2}'.",
                        cvar, oldValue, value);
                } else
                {
                    CVar<bool> boolCVar = CVars.RawGet(cvar) as CVar<bool>;
                    if(boolCVar != null)
                    {
                        bool oldValue = boolCVar.Value;
                        boolCVar.Value = !oldValue;
                        Console.WriteLine("Updated `{0}` from '{1}' to '{2}'.",
                            cvar, oldValue, boolCVar.Value);
                    }
                }
            } else
            {
                Console.WriteLine("CVar `{0}` not found.", cvar);
            }
        }

        private class ConsoleInterceptorWriter : TextWriter
        {
            public override Encoding Encoding => Encoding.UTF8;

            private List<string> _consoleItems;
            private TextWriter _passThroughWriter;
            private string _building = "";

            public ConsoleInterceptorWriter(List<string> consoleItems, TextWriter passThroughWriter)
            {
                _consoleItems = consoleItems;
                _passThroughWriter = passThroughWriter;
            }

            public override void Write(char value)
            {
                if(value == '\r' || value == '\n')
                {
                    if(_building.Length > 0)
                    {
                        _consoleItems.Add(_building);
                    }
                    _building = "";
                }
                _building += value;
                _passThroughWriter.Write(value);
            }
        }

        private void DrawStatistics()
        {
            if(CVars.Get<bool>("debug_show_statistics"))
            {
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(400, 200));
                ImGui.Begin("Statistics", ref CVars.Get<bool>("debug_show_statistics"));

                ImGui.Text(string.Format("Tick dt (ms): {0}", _statisticsProfiler.TimeBetweenTicks * 1000));
                ImGui.Text(string.Format("Tick dt [average] (ms): {0}", _statisticsProfiler.AverageTimeBetweenTicks * 1000));
                ImGui.Text(string.Format("Frame dt (ms): {0}", _statisticsProfiler.TimeBetweenFrames * 1000));
                ImGui.Text(string.Format("Frame dt [average] (ms): {0}", _statisticsProfiler.AverageTimeBetweenFrames * 1000));

                ImGui.Text(string.Format("Update time (ms): {0}", _statisticsProfiler.UpdateTime * 1000));
                ImGui.Text(string.Format("Update time [average] (ms): {0}", _statisticsProfiler.AverageUpdateTime * 1000));

                ImGui.Text(string.Format("Draw time (ms): {0}", _statisticsProfiler.DrawTime * 1000));
                ImGui.Text(string.Format("Draw time [average] (ms): {0}", _statisticsProfiler.AverageDrawTime * 1000));

                ImGui.End();
            }
        }
    }
}
