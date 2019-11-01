using System.Collections.Generic;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events.EnemyActions;
using GameJam.Events.InputHandling;
using GameJam.Input;
using GameJam.Processes.Menu;
using GameJam.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Content.Pipeline;

namespace GameJam.States
{
    class UILobbyGameState : CommonGameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        int numberOfPlayers = 0;
        public List<Player> playersSeated;
        //public Player playerOneSeat;
        //public Player playersSeated[1];

        public UILobbyGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        protected override void OnInitialize()
        {
            playersSeated = new List<Player>(4);
            playersSeated.Add(null);
            playersSeated.Add(null);
            playersSeated.Add(null);
            playersSeated.Add(null);

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_lobby_menu"));

            RegisterEvents();
            _root.RegisterListeners();

            ProcessManager.Attach(new EntityBackgroundSpawner(SharedState.Engine));

            base.OnInitialize();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            _root.UnregisterListeners();
            UnregisterEvents();

            base.OnKill();
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

        public bool Handle(IEvent evt)
        {
            GamePadButtonDownEvent buttonPressed = evt as GamePadButtonDownEvent;
            if (buttonPressed != null)
            {
                PlayerIndex buttonPressedIndex = buttonPressed._playerIndex;
                string playerstring = "controller_" + ((int)buttonPressedIndex);
                int isPlayerSeatedIndex = CheckIfSeated(playerstring); // -1 means not found, else in 0,1,2 or 3
                // A button is pressed
                if( buttonPressed._pressedButton == Buttons.A )
                {
                    if( isPlayerSeatedIndex == -1 )
                    {
                        if (playersSeated[0] == null)
                        {
                            playersSeated[0] = new Player(playerstring, new ControllerInputMethod(buttonPressedIndex));
                            // Player one controller visibility helper
                            PlayerOne_VisibilityHelper(true);
                            numberOfPlayers += 1;
                        }
                        else if (playersSeated[1] == null)
                        {
                            playersSeated[1] = new Player(playerstring, new ControllerInputMethod(buttonPressedIndex));
                            // Player two controller visibility helper
                            PlayerTwo_VisibilityHelper(true);
                            numberOfPlayers += 1;
                        }
                        else if (playersSeated[2] == null)
                        {
                            playersSeated[2] = new Player(playerstring, new ControllerInputMethod(buttonPressedIndex));
                            // Player three controller visibility helper
                            //PlayerThree_VisibilityHelper(true);
                            //numberOfPlayers += 1;
                        }
                        else if (playersSeated[3] == null)
                        {
                            playersSeated[3] = new Player(playerstring, new ControllerInputMethod(buttonPressedIndex));
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
                        ChangeState(new UIMenuGameState(GameManager, SharedState));
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
                if (buttonPressed._pressedButton == Buttons.Start)
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
                        StartGame(players);
                    }
                }
            }

            // Keyboard Lobby Support
            KeyboardKeyDownEvent keyPressed = evt as KeyboardKeyDownEvent;
            if (keyPressed != null)
            {
                string playerstring = "keyboard_";
                int keyboardPlayerNumber = -1;

                if (keyPressed.Key == Keys.A || keyPressed.Key == Keys.D)
                {
                    keyboardPlayerNumber = 1;
                    playerstring += 1;
                }
                else if (keyPressed.Key == Keys.Left || keyPressed.Key == Keys.Right )
                {
                    keyboardPlayerNumber = 2;
                    playerstring += 2;
                }

                int isPlayerSeatedIndex = CheckIfSeated(playerstring); // -1 means not found, else in 0,1,2 or 3

                if (isPlayerSeatedIndex == -1 && keyboardPlayerNumber != -1)
                {
                    if (playersSeated[0] == null)
                    {
                        playersSeated[0] = (keyboardPlayerNumber < 2) ? 
                            new Player( playerstring, new PrimaryKeyboardInputMethod() ) :
                            new Player( playerstring, new PrimaryKeyboardInputMethod() ) ;
                        // Player one controller visibility helper
                        PlayerOne_VisibilityHelper(true);
                        numberOfPlayers += 1;
                    }
                    else if (playersSeated[1] == null)
                    {
                        playersSeated[1] = (keyboardPlayerNumber < 2) ?
                            new Player(playerstring, new PrimaryKeyboardInputMethod()) :
                            new Player(playerstring, new PrimaryKeyboardInputMethod());
                        // Player two controller visibility helper
                        PlayerTwo_VisibilityHelper(true);
                        numberOfPlayers += 1;
                    }
                    else if (playersSeated[2] == null)
                    {
                        playersSeated[2] = (keyboardPlayerNumber < 2) ?
                            new Player(playerstring, new PrimaryKeyboardInputMethod()) :
                            new Player(playerstring, new PrimaryKeyboardInputMethod());
                        // Player three controller visibility helper
                        //PlayerThree_VisibilityHelper(true);
                        //numberOfPlayers += 1;
                    }
                    else if (playersSeated[3] == null)
                    {
                        playersSeated[3] = (keyboardPlayerNumber < 2) ?
                            new Player(playerstring, new PrimaryKeyboardInputMethod()) :
                            new Player(playerstring, new PrimaryKeyboardInputMethod());
                        // Player four controller visibility helper
                        //PlayerFour_VisibilityHelper(true);
                        //numberOfPlayers += 1;
                    }
                }
                // If player is seated we can place code here to handle alternative actions such as color changing

                if (keyPressed.Key == Keys.Enter)
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

                        StartGame(players);
                    }
                }

                if (keyPressed.Key == Keys.Escape)
                {
                    // when all players empty - return to menu screen
                    if (isPlayerSeatedIndex == -1)
                    {
                        ChangeState(new UIMenuGameState(GameManager, SharedState));
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
        private int CheckIfSeated(string playerstring)
        {
            for (int i = 0; i < playersSeated.Count; i++)
            {
                if (playersSeated[i] != null)
                {
                    string temp = playersSeated[i].InputMethod.ToString();
                    if (temp.Equals(playerstring))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        private void StartGame(Player[] players)
        {
            // Explode all entities
            ImmutableList<Entity> explosionEntities = SharedState.Engine.GetEntitiesFor(Family
                .All(typeof(TransformComponent), typeof(ColoredExplosionComponent), typeof(MenuBackgroundComponent))
                .Get());
            foreach (Entity entity in explosionEntities)
            {
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                ColoredExplosionComponent coloredExplosionComp = entity.GetComponent<ColoredExplosionComponent>();
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(transformComp.Position,
                    coloredExplosionComp.Color,
                    false));
            }

            // Destroy all entities
            SharedState.Engine.DestroyEntitiesFor(Family.All(typeof(MenuBackgroundComponent)).Get());

            ChangeState(new AdriftGameState(GameManager, SharedState, players));
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
