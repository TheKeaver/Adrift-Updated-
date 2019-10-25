using System;
using Events;
using GameJam.DevTools;
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
            Graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";

            Graphics.PreferredBackBufferWidth = CVars.Get<int>("window_width");
            Graphics.PreferredBackBufferHeight = CVars.Get<int>("window_height");

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

#if DEBUG
            _statisticsProfiler = new StatisticsProfiler();
#endif
        }
        
        protected override void Initialize()
        {
            RegisterEvents();

            Window.Title = "Adrift";

            Mouse.WindowHandle = Window.Handle;
            IsMouseVisible = true;

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
            //_gamePadListener.ButtonUp += GamePad_ButtonUp;

            _keyboardListener = new KeyboardListener();
            _inputListenerManager.Listeners.Add(_keyboardListener);
            _keyboardListener.KeyPressed += Keyboard_KeyDown;
            //_keyboardListener.KeyReleased += Keyboard_KeyUp;

            GamePadListener.CheckControllerConnections = true;
            GamePadListener.ControllerConnectionChanged += GamePad_ConnectionChanged;

#if DEBUG
            Components.Add(new ImGuiGameComponent(this, _statisticsProfiler));
#endif

            base.Initialize();
        }

        private void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<StepGameUpdateEvent>(this);
            EventManager.Instance.RegisterListener<GameShutdownEvent>(this);
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

        // GamePad_ControllerButtonUp

        void Keyboard_KeyDown(object sender, KeyboardEventArgs e)
        {
            // Workaround; when the game is debug paused, the EventManager's
            // queue isn't dispatched. Because of this, opening/closing
            // of debug windows isn't possible when the game is debug paused.
            if (CVars.Get<bool>("debug_pause_game_updates")
                && (e.Key == Keys.OemTilde
                    || e.Key == Keys.F1
                    || e.Key == Keys.F2))
            {
                EventManager.Instance.TriggerEvent(new KeyboardKeyDownEvent(e.Key));
            }
            else
            {
                EventManager.Instance.QueueEvent(new KeyboardKeyDownEvent(e.Key));
            }
        }

        // Keyboard_KeyUp

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

            return false;
        }
    }
}
