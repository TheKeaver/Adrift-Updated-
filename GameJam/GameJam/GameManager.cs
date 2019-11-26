using System;
using Adrift.Content.Common.UI;
using Events;
using GameJam.DevTools;
using GameJam.Directors;
using GameJam.Events;
using GameJam.Events.DevTools;
using GameJam.Events.InputHandling;
using GameJam.Processes;
using GameJam.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;

namespace GameJam
{
    /// <summary>
    /// Main manager for the game. Contains implementation of the
    /// MonoGame game loop.
    /// </summary>
    public class GameManager : Game, IEventListener
    {
        InputListenerComponent _inputListenerManager;
        MouseListener _mouseListener;
        GamePadListener _gamePadListener;
        KeyboardListener _keyboardListener;

        public ProcessManager ProcessManager
        {
            get;
            private set;
        }

#if DEBUG
        StatisticsProfiler _statisticsProfiler;
#endif

        public GraphicsDeviceManager Graphics
        {
            get;
            private set;
        }
        
        public GameManager()
        {
            CVars.Initialize();

            ProcessManager = new ProcessManager();

            Graphics = new GraphicsDeviceManager(this);
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.PreferMultiSampling = false;
            Content.RootDirectory = "Content";

            Graphics.PreferredBackBufferWidth = CVars.Get<int>("initial_window_width");
            Graphics.PreferredBackBufferHeight = CVars.Get<int>("initial_window_height");

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

#if DEBUG
            _statisticsProfiler = new StatisticsProfiler();
#endif

            Console.WriteLine(typeof(UIWidgetsReader).AssemblyQualifiedName);
        }
        
        protected override void Initialize()
        {
            RegisterEvents();

            Window.Title = "Adrift";

            Mouse.WindowHandle = Window.Handle;
            IsMouseVisible = true;

            ProcessManager.Attach(new UIControlModeDirector());

            _inputListenerManager = new InputListenerComponent(this);
            Components.Add(_inputListenerManager);

            _mouseListener = new MouseListener();
            _inputListenerManager.Listeners.Add(_mouseListener);

            _mouseListener.MouseMoved += Mouse_MouseMoved;
            _mouseListener.MouseDown += Mouse_MouseDownOrUp;
            _mouseListener.MouseUp += Mouse_MouseDownOrUp;

            _gamePadListener = new GamePadListener();
            _inputListenerManager.Listeners.Add(_gamePadListener);
            _gamePadListener.ButtonDown += GamePad_ButtonDown;
            _gamePadListener.ButtonUp += GamePad_ButtonUp;

            _keyboardListener = new KeyboardListener();
            _inputListenerManager.Listeners.Add(_keyboardListener);
            _keyboardListener.KeyPressed += Keyboard_KeyDown;
            _keyboardListener.KeyReleased += Keyboard_KeyUp;

            GamePadListener.CheckControllerConnections = true;
            GamePadListener.ControllerConnectionChanged += GamePad_ConnectionChanged;

#if DEBUG
            Components.Add(new ImGuiGameComponent(this, _statisticsProfiler));
#endif

            ReloadDisplayOptions();

            base.Initialize();
        }

        private void ReloadDisplayOptions()
        {
            bool applyChanges = false;

            DisplayModeCollection displayModes = Graphics.GraphicsDevice.Adapter.SupportedDisplayModes;

            // Windowed/Borderless/Fullscreen
            if (CVars.Get<bool>("display_fullscreen"))
            {
                if(!Graphics.IsFullScreen)
                {
                    if(CVars.Get<int>("display_fullscreen_width") < 0 || CVars.Get<int>("display_fullscreen_height") < 0)
                    {
                        CVars.Get<int>("display_fullscreen_width") = Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                        CVars.Get<int>("display_fullscreen_height") = Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                        CVars.Save();
                    }

                    Graphics.PreferredBackBufferWidth = CVars.Get<int>("display_fullscreen_width");
                    Graphics.PreferredBackBufferHeight = CVars.Get<int>("display_fullscreen_height");

                    Graphics.IsFullScreen = true;
                    Graphics.HardwareModeSwitch = true;
                    applyChanges = true;
                }

                CVars.Get<bool>("display_borderless") = false;
                CVars.Get<bool>("display_windowed") = false;
                CVars.Save();
            } else if (CVars.Get<bool>("display_borderless"))
            {
                if (!Graphics.IsFullScreen)
                {
                    Graphics.IsFullScreen = true;
                    Graphics.HardwareModeSwitch = false;
                    applyChanges = true;
                }

                CVars.Get<bool>("display_windowed") = false;
                CVars.Save();
            } else
            {
                if (Graphics.IsFullScreen)
                {
                    Graphics.IsFullScreen = false;
                }
                Graphics.HardwareModeSwitch = false;
                applyChanges = true;

                CVars.Get<bool>("display_windowed") = false;
                CVars.Save();
            }

            // V-sync
            if(Graphics.SynchronizeWithVerticalRetrace
                != CVars.Get<bool>("display_vsync"))
            {
                Graphics.SynchronizeWithVerticalRetrace = CVars.Get<bool>("display_vsync");
                applyChanges = true;
            }

            if(applyChanges)
            {
                Graphics.ApplyChanges();
            }
        }

        private void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<StepGameUpdateEvent>(this);
            EventManager.Instance.RegisterListener<GameShutdownEvent>(this);
            EventManager.Instance.RegisterListener<ReloadDisplayOptionsEvent>(this);
        }
        private void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }
        
        protected override void LoadContent()
        {
            // Global Content

            SharedGameState sharedState = (SharedGameState)ProcessManager.Attach(new SharedGameState(this));
            ProcessManager.Attach(new UIMenuGameState(this, sharedState));
        }
        
        protected override void Update(GameTime gameTime)
        {
#if DEBUG
            _statisticsProfiler.BeginUpdate(gameTime);
#endif

            if(!CVars.Get<bool>("debug_pause_game_updates"))
            {
                Update((float)gameTime.ElapsedGameTime.TotalSeconds * CVars.Get<float>("debug_update_time_scale"));
            }

#if DEBUG
            _statisticsProfiler.EndUpdate();
#endif

            base.Update(gameTime);
        }

        private void Update(float dt)
        {
            EventManager.Instance.Dispatch();

            ProcessManager.Update(dt);
        }

        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            _statisticsProfiler.BeginDraw(gameTime);
#endif

            Graphics.GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            foreach(Process process in ProcessManager.Processes)
            {
                RenderProcess renderProcess = process as RenderProcess;
                if(renderProcess != null)
                {
                    renderProcess.Render((float)gameTime.ElapsedGameTime.TotalSeconds
                        * CVars.Get<float>("debug_update_time_scale")
                        * (CVars.Get<bool>("debug_pause_game_updates") ? 0 : 1));
                }
            }

#if DEBUG
            _statisticsProfiler.EndDraw();
#endif

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            UnregisterEvents();

            base.UnloadContent();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            EventManager.Instance.QueueEvent(new ResizeEvent(Window.ClientBounds.Width,
                                                               Window.ClientBounds.Height));
        }

        void Mouse_MouseMoved(object sender, MouseEventArgs e)
        {
            EventManager.Instance.QueueEvent(new MouseMoveEvent(new Vector2(e.PreviousState.Position.X,
                                                                           e.PreviousState.Position.Y),
                                                                new Vector2(e.Position.X,
                                                                           e.Position.Y)));
        }
        void Mouse_MouseDownOrUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                EventManager.Instance.QueueEvent(new MouseButtonEvent(e.CurrentState.LeftButton,
                                                                      new Vector2(e.Position.X,
                                                                                  e.Position.Y)));
            }
        }

        void GamePad_ConnectionChanged(object sender, GamePadEventArgs e)
        {
            // More than 2 controllers not supported
            if ((int)e.PlayerIndex < 2)
            {
                if (!e.PreviousState.IsConnected
                   && e.CurrentState.IsConnected)
                {
                    // Controller connected
                    EventManager.Instance.QueueEvent(new GamePadConnectedEvent(e.PlayerIndex));
                }
                if (e.PreviousState.IsConnected
                   && !e.CurrentState.IsConnected)
                {
                    // Controller disconnected
                    EventManager.Instance.QueueEvent(new GamePadDisconnectedEvent(e.PlayerIndex));
                }
            }
        }

        void GamePad_ButtonDown(object sender, GamePadEventArgs e)
        {
            EventManager.Instance.QueueEvent(new GamePadButtonDownEvent(e.PlayerIndex, e.Button));
        }

        private void GamePad_ButtonUp(object sender, GamePadEventArgs e)
        {
            EventManager.Instance.QueueEvent(new GamePadButtonUpEvent(e.PlayerIndex, e.Button));
        }

        void Keyboard_KeyDown(object sender, KeyboardEventArgs e)
        {
            // Workaround; when the game is debug paused, the EventManager's
            // queue isn't dispatched. Because of this, opening/closing
            // of debug windows isn't possible when the game is debug paused.
            if (CVars.Get<bool>("debug_pause_game_updates")
                && (e.Key == Keys.OemTilde
                    || e.Key == Keys.F1
                    || e.Key == Keys.F2
                    || e.Key == Keys.F3))
            {
                EventManager.Instance.TriggerEvent(new KeyboardKeyDownEvent(e.Key));
            }
            else
            {
                EventManager.Instance.QueueEvent(new KeyboardKeyDownEvent(e.Key));
            }
        }

        private void Keyboard_KeyUp(object sender, KeyboardEventArgs e)
        {
            EventManager.Instance.QueueEvent(new KeyboardKeyUpEvent(e.Key));
        }

        public bool Handle(IEvent evt)
        {
            if(evt is StepGameUpdateEvent)
            {
                Update(CVars.Get<float>("debug_game_step_period"));
            }

            if(evt is GameShutdownEvent)
            {
                Exit();
            }

            if(evt is ReloadDisplayOptionsEvent)
            {
                ReloadDisplayOptions();
            }

            return false;
        }
    }
}
