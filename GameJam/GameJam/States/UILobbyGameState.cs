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
        public List<Player> playersSeated;
        //public Player playerOneSeat;
        //public Player playersSeated[1];

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
            playersSeated = new List<Player>(4);
            playersSeated.Add(null);
            playersSeated.Add(null);
            playersSeated.Add(null);
            playersSeated.Add(null);

            ProcessManager = new ProcessManager();

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
            _root.RegisterListeners(); // Root must be registered first because of "B" button event consumption

            RegisterEvents();
        }

        public override void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_lobby_menu"));
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
            if( buttonPressed != null )
            {
                PlayerIndex buttonPressedIndex = buttonPressed._playerIndex;
                String playerString = "controller_" + ((int)buttonPressedIndex);
                int isPlayerSeatedIndex = CheckIfSeated(playerString); // -1 means not found, else in 0,1,2 or 3
                // A button is pressed
                if( buttonPressed._pressedButton == Buttons.A )
                {
                    if( isPlayerSeatedIndex == -1 )
                    {
                        if (playersSeated[0] == null)
                        {
                            playersSeated[0] = new Player(playerString, new ControllerInputMethod(buttonPressedIndex));
                            // Player one controller visibility helper
                            PlayerOne_VisibilityHelper(true);
                            numberOfPlayers += 1;
                        }
                        else if (playersSeated[1] == null)
                        {
                            playersSeated[1] = new Player(playerString, new ControllerInputMethod(buttonPressedIndex));
                            // Player two controller visibility helper
                            PlayerTwo_VisibilityHelper(true);
                            numberOfPlayers += 1;
                        }
                        else if (playersSeated[2] == null)
                        {
                            playersSeated[2] = new Player(playerString, new ControllerInputMethod(buttonPressedIndex));
                            // Player three controller visibility helper
                            //PlayerThree_VisibilityHelper(true);
                            //numberOfPlayers += 1;
                        }
                        else if (playersSeated[3] == null)
                        {
                            playersSeated[3] = new Player(playerString, new ControllerInputMethod(buttonPressedIndex));
                            // Player four controller visibility helper
                            //PlayerFour_VisibilityHelper(true);
                            //numberOfPlayers += 1;
                        }
                    }
                    // If player is seated we can place code here to handle alternative actions such as color changing
                }

                // B button is pressed
                if( buttonPressed._pressedButton == Buttons.B )
                {
                    // when all players empty - return to menu screen
                    if ( isPlayerSeatedIndex == -1 )
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                    }
                    // when player 1 presses B
                    if ( isPlayerSeatedIndex != -1 )
                    {
                        // Blanks all seats
                        playersSeated = new List<Player>(4);
                        Default_VisibilityHelper();
                        numberOfPlayers = 0;
                        /* Other Option
                         * Blanks occupied seat only
                         * playerSeated[isPlayerSeatedIndex] = null;
                         * undoSeatVisibilityHelper() // not implemented */
                    }
                }

                // If start button is pressed
                if ( buttonPressed._pressedButton == Buttons.Start )
                {
                    if ( playersSeated[0] != null )
                    {
                        int playersCounter = 0;
                        Player[] players = new Player[numberOfPlayers];
                        for (int i = 0; i < playersSeated.Count; i++)
                        {
                            if (playersSeated[i] != null)
                            {
                                players[playersCounter] = playersSeated[i];
                                playersCounter++;
                            }
                        }
                        GameManager.ChangeState(new MainGameState(GameManager, players));
                    }
                }
            }

            // Keyboard Lobby Support
            KeyboardKeyDownEvent keyPressed = evt as KeyboardKeyDownEvent;
            if ( keyPressed != null )
            {
                String playerString = "keyboard_";
                int keyboardPlayerNumber = -1;

                if (keyPressed._keyPressed == Keys.A || keyPressed._keyPressed == Keys.D)
                {
                    keyboardPlayerNumber = 1;
                    playerString += 1;
                }
                else if (keyPressed._keyPressed == Keys.Left || keyPressed._keyPressed == Keys.Right )
                {
                    keyboardPlayerNumber = 2;
                    playerString += 2;
                }

                int isPlayerSeatedIndex = CheckIfSeated(playerString); // -1 means not found, else in 0,1,2 or 3

                if (isPlayerSeatedIndex == -1 && keyboardPlayerNumber != -1)
                {
                    if (playersSeated[0] == null)
                    {
                        playersSeated[0] = (keyboardPlayerNumber < 2) ? 
                            new Player( playerString, new PrimaryKeyboardInputMethod() ) :
                            new Player( playerString, new PrimaryKeyboardInputMethod() ) ;
                        // Player one controller visibility helper
                        PlayerOne_VisibilityHelper(true);
                        numberOfPlayers += 1;
                    }
                    else if (playersSeated[1] == null)
                    {
                        playersSeated[1] = (keyboardPlayerNumber < 2) ?
                            new Player(playerString, new PrimaryKeyboardInputMethod()) :
                            new Player(playerString, new PrimaryKeyboardInputMethod());
                        // Player two controller visibility helper
                        PlayerTwo_VisibilityHelper(true);
                        numberOfPlayers += 1;
                    }
                    else if (playersSeated[2] == null)
                    {
                        playersSeated[2] = (keyboardPlayerNumber < 2) ?
                            new Player(playerString, new PrimaryKeyboardInputMethod()) :
                            new Player(playerString, new PrimaryKeyboardInputMethod());
                        // Player three controller visibility helper
                        //PlayerThree_VisibilityHelper(true);
                        //numberOfPlayers += 1;
                    }
                    else if (playersSeated[3] == null)
                    {
                        playersSeated[3] = (keyboardPlayerNumber < 2) ?
                            new Player(playerString, new PrimaryKeyboardInputMethod()) :
                            new Player(playerString, new PrimaryKeyboardInputMethod());
                        // Player four controller visibility helper
                        //PlayerFour_VisibilityHelper(true);
                        //numberOfPlayers += 1;
                    }
                }
                // If player is seated we can place code here to handle alternative actions such as color changing

                if ( keyPressed._keyPressed == Keys.Enter )
                {
                    if (playersSeated[0] != null)
                    {
                        int playersCounter = 0;
                        Player[] players = new Player[numberOfPlayers];
                        for (int i = 0; i < playersSeated.Count; i++)
                        {
                            if (playersSeated[i] != null)
                            {
                                players[playersCounter] = playersSeated[i];
                                playersCounter++;
                            }
                        }
                        GameManager.ChangeState(new MainGameState(GameManager, players));
                    }
                }

                if ( keyPressed._keyPressed == Keys.Escape )
                {
                    // when all players empty - return to menu screen
                    if (isPlayerSeatedIndex == -1)
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                    }
                    // when player 1 presses B
                    if (isPlayerSeatedIndex != -1)
                    {
                        // Blanks all seats
                        playersSeated = new List<Player>(4);
                        Default_VisibilityHelper();
                        numberOfPlayers = 0;
                        /* Other Option
                         * Blanks occupied seat only
                         * playerSeated[isPlayerSeatedIndex] = null;
                         * undoSeatVisibilityHelper() // not implemented */
                    }
                }
            }

            return false;
        }

        // Returns index when found, else returns -1
        private int CheckIfSeated(String playerString)
        {
            for( int i=0; i<playersSeated.Count; i++ )
            {
                if (playersSeated[i] != null)
                {
                    String temp = playersSeated[i].InputMethod.ToString();
                    if (temp.Equals(playerString))
                    {
                        return i;
                    }
                }
            }
            return -1;
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
