using Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;

namespace GameJam.Input
{
    class MonoGameInputEventTranslator : GameComponent, IEventListener
    {
        InputListenerComponent _inputListenerManager;
        MouseListener _mouseListener;
        Dictionary<PlayerIndex, GamePadListener> _gamePadListeners = new Dictionary<PlayerIndex, GamePadListener>();
        KeyboardListener _keyboardListener;

        public MonoGameInputEventTranslator(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            _inputListenerManager = new InputListenerComponent(Game);
            Game.Components.Add(_inputListenerManager);

            _mouseListener = new MouseListener();
            _inputListenerManager.Listeners.Add(_mouseListener);

            _mouseListener.MouseMoved += Mouse_MouseMoved;
            _mouseListener.MouseDown += Mouse_MouseDownOrUp;
            _mouseListener.MouseUp += Mouse_MouseDownOrUp;

            _keyboardListener = new KeyboardListener();
            _inputListenerManager.Listeners.Add(_keyboardListener);
            _keyboardListener.KeyPressed += Keyboard_KeyDown;
            _keyboardListener.KeyReleased += Keyboard_KeyUp;

            GamePadListener.CheckControllerConnections = true;
            GamePadListener.ControllerConnectionChanged += GamePad_ConnectionChanged;

            EventManager.Instance.RegisterListener<GamePadConnectedEvent>(this);
            EventManager.Instance.RegisterListener<GamePadDisconnectedEvent>(this);

            base.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            EventManager.Instance.UnregisterListener(this);

            base.Dispose(disposing);
        }

        public bool Handle(IEvent evt)
        {
            GamePadConnectedEvent gamePadConnectedEvent = evt as GamePadConnectedEvent;
            if (gamePadConnectedEvent != null)
            {
                AddGamePadListener(gamePadConnectedEvent.PlayerIndex);
            }
            GamePadDisconnectedEvent gamePadDisconnectedEvent = evt as GamePadDisconnectedEvent;
            if (gamePadDisconnectedEvent != null)
            {
                RemoveGamePadListener(gamePadDisconnectedEvent.PlayerIndex);
            }

            return false;
        }

        /** MONOGAME HANDLERS **/
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
            switch (e.PlayerIndex)
            {
                case PlayerIndex.One:
                case PlayerIndex.Two:
                case PlayerIndex.Three:
                case PlayerIndex.Four:
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
                    break;
                default:
                    Console.WriteLine(">4 gamepads not supported.");
                    break;
            }
        }

        private void AddGamePadListener(PlayerIndex playerIndex)
        {
            _gamePadListeners.Add(playerIndex, new GamePadListener(new GamePadListenerSettings()
            {
                PlayerIndex = playerIndex
            }));
            _inputListenerManager.Listeners.Add(_gamePadListeners[playerIndex]);
            _gamePadListeners[playerIndex].ButtonUp += GamePad_ButtonDown;
            _gamePadListeners[playerIndex].ButtonUp += GamePad_ButtonUp;
        }
        private void RemoveGamePadListener(PlayerIndex playerIndex)
        {
            _inputListenerManager.Listeners.Remove(_gamePadListeners[playerIndex]);
            _gamePadListeners.Remove(playerIndex);
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
    }
}
