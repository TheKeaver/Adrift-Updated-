using System;
using System.Collections.Generic;
using Events;
using GameJam.Events;
using GameJam.Events.InputHandling;
using GameJam.Input;
using GameJam.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Content.Pipeline;

namespace GameJam.States
{
    class UILobbyGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        int numberOfPlayers = 0;
        public Player playerOneSeat;
        public Player playerTwoSeat;

        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public UILobbyGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
        }

        void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            RegisterEvents();

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/LobbyMenu"));
        }

        public override void Show()
        {
            _root.RegisterListeners();
        }

        public override void Hide()
        {
            _root.UnregisterListeners();
        }

        public override void Update(float dt)
        {
            ProcessManager.Update(dt);
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Dispose()
        {
            UnregisterEvents();
        }

        public bool Handle(IEvent evt)
        {
            GamePadButtonDownEvent buttonPressed = evt as GamePadButtonDownEvent;
            if ( buttonPressed != null )
            {
                // A button is pressed
                if (buttonPressed._pressedButton == Buttons.A)
                {
                    // Any player presses A while no seats are occupied
                    if ( playerOneSeat == null && playerTwoSeat == null )
                    {
                        playerOneSeat = new Player("playerOne", new ControllerInputMethod(buttonPressed._playerIndex));
                        numberOfPlayers += 1;
                    }
                    // One is occupied, Two is Open
                    if ( playerOneSeat != null && playerTwoSeat == null )
                    {
                        playerTwoSeat = new Player("playerTwo", new ControllerInputMethod(buttonPressed._playerIndex));
                        numberOfPlayers += 1;
                    }
                }

                // B button is pressed
                if ( buttonPressed._pressedButton == Buttons.B )
                {
                    // when both players empty - return to menu screen
                    if ( playerOneSeat == null && playerTwoSeat == null )
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                    }
                    // when player 1 presses B
                    if (playerOneSeat != null || playerTwoSeat != null)
                    {
                        playerOneSeat = null;
                        playerTwoSeat = null;
                        numberOfPlayers = 0;
                    }
                }

                // If start button is pressed
                if (buttonPressed._pressedButton == Buttons.Start)
                {
                    if ( playerOneSeat != null )
                    {
                        Player[] players = new Player[numberOfPlayers];
                        for (int i = 0; i < numberOfPlayers; i++)
                        {
                            if (i == 0)
                                players[i] = playerOneSeat;
                            else
                                players[i] = playerTwoSeat;

                        }
                        // This line needs to instead change to Lobby Screen
                        GameManager.ChangeState(new MainGameState(GameManager, players));
                    }
                }
            }

            // Keyboard Lobby Support
            KeyboardKeyDownEvent keyPressed = evt as KeyboardKeyDownEvent;
            if ( keyPressed != null )
            {
                if ( keyPressed._keyPressed == Keys.A || keyPressed._keyPressed == Keys.D )
                {
                    // Any player presses A while no seats are occupied
                    if ( playerOneSeat == null )
                    {
                        playerOneSeat = new Player("playerOne", new PrimaryKeyboardInputMethod());
                        numberOfPlayers += 1;
                    }
                }

                if (keyPressed._keyPressed == Keys.Left || keyPressed._keyPressed == Keys.Right)
                {
                    if ( playerTwoSeat == null )
                    {
                        playerTwoSeat = new Player("playerTwo", new SecondaryKeyboardInputMethod());
                        numberOfPlayers += 1;
                    }
                }

                if (keyPressed._keyPressed == Keys.Enter)
                {
                    if ( playerOneSeat != null )
                    {
                        Player[] players = new Player[numberOfPlayers];
                        for (int i = 0; i < numberOfPlayers; i++)
                        {
                            if (i == 0)
                                players[i] = playerOneSeat;
                            else
                                players[i] = playerTwoSeat;

                        }
                        GameManager.ChangeState(new MainGameState(GameManager, players));
                    }
                }

                if (keyPressed._keyPressed == Keys.Escape)
                {
                    // when both players empty - return to menu screen
                    if ( playerOneSeat == null && playerTwoSeat == null )
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                    }
                    if ( playerOneSeat != null || playerTwoSeat != null )
                    {
                        playerOneSeat = null;
                        playerTwoSeat = null;
                        numberOfPlayers = 0;
                    }
                }
            }

            return false;
        }
    }
}
