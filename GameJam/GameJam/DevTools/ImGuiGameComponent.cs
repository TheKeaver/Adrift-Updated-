using System;
using System.Text;
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
        private ImGUIRenderer _renderer;

        private string _cvarEditing = "";

        public ImGuiGameComponent(Game game) : base(game)
        {
            _renderer = new ImGUIRenderer(game).Initialize().RebuildFontAtlas();
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

            _renderer.EndLayout();

            base.Draw(gameTime);
        }

        private void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
        }

        private void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            KeyboardKeyDownEvent keyboardKeyDownEvent = evt as KeyboardKeyDownEvent;
            if (keyboardKeyDownEvent != null)
            {
                switch (keyboardKeyDownEvent.Key)
                {
                    case Keys.F1:
                        CVars.Get<bool>("debug_show_cvar_viewer") = !CVars.Get<bool>("debug_show_cvar_viewer");
                        break;
                    case Keys.F2:
                        CVars.Get<bool>("debug_show_playback_controls") = !CVars.Get<bool>("debug_show_playback_controls");
                        break;
                }
            }

            return false;
        }

        private void DrawCVarWindow()
        {
            if (CVars.Get<bool>("debug_show_cvar_viewer"))
            {
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(500, 500), ImGuiCond.FirstUseEver);
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
    }
}
