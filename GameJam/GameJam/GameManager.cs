using System;
using Events;
using GameJam.Debug;
using GameJam.Events;
using GameJam.Events.InputHandling;
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
    public class GameManager : Game
    {
        InputListenerComponent _inputListenerManager;
        MouseListener _mouseListener;
        GamePadListener _gamePadListener;
        KeyboardListener _keyboardListener;

        GameState _currentState;
        bool _cvarSyncButtonReleased = false;

        public GraphicsDeviceManager Graphics
        {
            get;
            private set;
        }
        
        public GameManager()
        {
            CVars.Initialize();

            this.Window.Title = "Adrift";

            Graphics = new GraphicsDeviceManager(this);
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";

            Graphics.PreferredBackBufferWidth = CVars.Get<int>("window_width");
            Graphics.PreferredBackBufferHeight = CVars.Get<int>("window_height");

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }
        
        protected override void Initialize()
        {
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
            Components.Add(new ImGuiGameComponent(this));
#endif

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Global Content

            ChangeState(new UIMenuGameState(this));
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F2) && _cvarSyncButtonReleased)
            {
                CVars.SynchronizeFromFile();
                _cvarSyncButtonReleased = false;
            } else
            {
                _cvarSyncButtonReleased = true;
            }

            EventManager.Instance.Dispatch();

            if(_currentState != null)
            {
                _currentState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Graphics.GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            if (_currentState != null)
            {
                _currentState.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Draw(gameTime);
        }

        public void ChangeState(GameState nextState)
        {
            if (_currentState != null)
            {
                _currentState.Hide();
                _currentState.UnloadContent();
                _currentState.Dispose();
            }

            _currentState = nextState;
            _currentState.Initialize();

            _currentState.LoadContent();
            _currentState.Content.Locked = true;
            _currentState.Show();
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
            EventManager.Instance.QueueEvent(new KeyboardKeyDownEvent(e.Key));
        }

        // Keyboard_KeyUp
    }
}
