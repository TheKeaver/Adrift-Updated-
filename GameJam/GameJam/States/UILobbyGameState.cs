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

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
            _root.RegisterListeners(); // Root must be registered first because of "B" button event consumption

            RegisterEvents();
        }

        public override void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/LobbyMenu"));
        }

        public override void Show()
        {
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
                if ( buttonPressed._pressedButton == Buttons.A )
                {
                    // Any player presses A while no seats are occupied
                    if ( playerOneSeat == null )
                    {
                        if ( playerTwoSeat == null )
                        {
                            playerOneSeat = new Player("playerOne", new ControllerInputMethod(buttonPressed._playerIndex));
                            // Player one controller visibility helper
                            PlayerOne_VisibilityHelper(true);
                            numberOfPlayers += 1;
                        }
                        if ( playerTwoSeat != null )
                        {
                            ControllerInputMethod playerTwoControllerIM = playerTwoSeat.InputMethod as ControllerInputMethod;
                            if ( playerTwoControllerIM == null || playerTwoControllerIM.PlayerIndex != buttonPressed._playerIndex )
                            {
                                playerOneSeat = new Player("playerOne", new ControllerInputMethod(buttonPressed._playerIndex));
                                // Player one controller visibility helper
                                PlayerOne_VisibilityHelper(true);
                                numberOfPlayers += 1;
                            }
                        }
                    }
                    // One is occupied, Two is Open
                    if ( playerOneSeat != null && playerTwoSeat == null )
                    {
                        ControllerInputMethod playerOneControllerIM = playerOneSeat.InputMethod as ControllerInputMethod;
                        if ( playerOneControllerIM == null || playerOneControllerIM.PlayerIndex != buttonPressed._playerIndex )
                        {
                            playerTwoSeat = new Player("playerTwo", new ControllerInputMethod(buttonPressed._playerIndex));
                            // Player two controller visibility helper
                            PlayerTwo_VisibilityHelper(true);
                            numberOfPlayers += 1;
                        }
                    }
                }

                // B button is pressed
                if ( buttonPressed._pressedButton == Buttons.B )
                {
                    // when both players empty - return to menu screen
                    if ( playerOneSeat == null && playerTwoSeat == null )
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                        // Both players revert to default visibility (same as below)
                    }
                    // when player 1 presses B
                    if ( playerOneSeat != null || playerTwoSeat != null )
                    {
                        playerOneSeat = null;
                        playerTwoSeat = null;
                        Default_VisibilityHelper();
                        numberOfPlayers = 0;
                    }
                }

                // If start button is pressed
                if ( buttonPressed._pressedButton == Buttons.Start )
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
                        // Player one keyboard visibility helper
                        PlayerOne_VisibilityHelper(false);
                        numberOfPlayers += 1;
                    }
                }

                if ( keyPressed._keyPressed == Keys.Left || keyPressed._keyPressed == Keys.Right )
                {
                    if ( playerTwoSeat == null )
                    {
                        playerTwoSeat = new Player("playerTwo", new SecondaryKeyboardInputMethod());
                        // Player two keyboard visibility helper
                        PlayerTwo_VisibilityHelper(false);
                        numberOfPlayers += 1;
                    }
                }

                if ( keyPressed._keyPressed == Keys.Enter )
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

                if ( keyPressed._keyPressed == Keys.Escape )
                {
                    // when both players empty - return to menu screen
                    if ( playerOneSeat == null && playerTwoSeat == null )
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                        // Both players revert to default visibility (same as above)
                    }
                    if ( playerOneSeat != null || playerTwoSeat != null )
                    {
                        playerOneSeat = null;
                        playerTwoSeat = null;
                        Default_VisibilityHelper();
                        numberOfPlayers = 0;
                    }
                }
            }

            return false;
        }

        private void PlayerOne_VisibilityHelper(bool isController)
        {
            _root.FindWidgetByID("player_one_a_button").Hidden = true;
            _root.FindWidgetByID("player_one_left_bumper").Hidden = !isController;
            _root.FindWidgetByID("player_one_right_bumper").Hidden = !isController;
            _root.FindWidgetByID("player_one_a_key").Hidden = isController;
            _root.FindWidgetByID("player_one_d_key").Hidden = isController;
        }

        private void PlayerTwo_VisibilityHelper(bool isController)
        {
            _root.FindWidgetByID("player_two_a_button").Hidden = true;
            _root.FindWidgetByID("player_two_left_bumper").Hidden = !isController;
            _root.FindWidgetByID("player_two_right_bumper").Hidden = !isController;
            _root.FindWidgetByID("player_two_left_arrow_key").Hidden = isController;
            _root.FindWidgetByID("player_two_right_arrow_key").Hidden = isController;
        }

        private void Default_VisibilityHelper()
        {
            _root.FindWidgetByID("player_one_a_button").Hidden = false;
            _root.FindWidgetByID("player_one_left_bumper").Hidden = true;
            _root.FindWidgetByID("player_one_right_bumper").Hidden = true;
            _root.FindWidgetByID("player_one_a_key").Hidden = false;
            _root.FindWidgetByID("player_one_d_key").Hidden = false;

            _root.FindWidgetByID("player_two_a_button").Hidden = false;
            _root.FindWidgetByID("player_two_left_bumper").Hidden = true;
            _root.FindWidgetByID("player_two_right_bumper").Hidden = true;
            _root.FindWidgetByID("player_two_left_arrow_key").Hidden = false;
            _root.FindWidgetByID("player_two_right_arrow_key").Hidden = false;
        }
    }
}
