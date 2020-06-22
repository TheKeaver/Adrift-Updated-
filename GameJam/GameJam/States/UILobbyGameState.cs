using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Content;
using GameJam.Events.EnemyActions;
using GameJam.Events.InputHandling;
using GameJam.Graphics.Text;
using GameJam.Input;
using GameJam.Processes.Menu;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.States
{
    class UILobbyGameState : CommonGameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        FieldFontRenderer _fieldFontRenderer;
        Root _root;

        KeyTextureMap _keyTextureMap;
        GamePadTextureMap _gamePadTextureMap;

        public static readonly int MAX_PLAYERS = 4;

        public Player[] _playersSeated = new Player[MAX_PLAYERS];

        public int SeatedPlayerCount
        {
            get
            {
                int _count = 0;
                for (int i = 0; i < _playersSeated.Length; i++)
                {
                    if (_playersSeated[i] != null)
                    {
                        _count++;
                    }
                }
                return _count;
            }
        }

        public UILobbyGameState(GameManager gameManager,
            SharedGameState sharedState,
            Player[] players = null)
            :base(gameManager, sharedState)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
            _fieldFontRenderer = new FieldFontRenderer(Content, GameManager.GraphicsDevice);

            if (players != null)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    int idx = players[i].LobbySeatIndex > 0 && players[i].LobbySeatIndex < MAX_PLAYERS ? players[i].LobbySeatIndex : i;
                    _playersSeated[idx] = players[i];
                }
            }
        }

        protected override void OnInitialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_lobby_menu"));

            _root.AutoControlModeSwitching = false;

            _keyTextureMap = new KeyTextureMap();
            _keyTextureMap.CacheAll(Content);
            _gamePadTextureMap = new GamePadTextureMap();
            _gamePadTextureMap.CacheAll(Content);

            ProcessManager.Attach(new EntityBackgroundSpawner(SharedState.Engine, SharedState.Camera));

            UpdateUI();

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
            _root.Render(_spriteBatch, _fieldFontRenderer);

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void RegisterListeners()
        {
            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);
            // Root usually goes first, but goes last in this case. It will
            // absorb the GamePadButtonDownEvent otherwise.
            _root.RegisterListeners();
        }

        protected override void UnregisterListeners()
        {
            EventManager.Instance.UnregisterListener(this);
            _root.UnregisterListeners();
        }

        public bool Handle(IEvent evt)
        {
            GamePadButtonDownEvent buttonPressed = evt as GamePadButtonDownEvent;
            if (buttonPressed != null)
            {
                return HandleGamePadButtonDownEvent(buttonPressed);
            }

            // Keyboard Lobby Support
            KeyboardKeyDownEvent keyPressed = evt as KeyboardKeyDownEvent;
            if (keyPressed != null)
            {
                HandleKeyboardKeyDownEvent(keyPressed);

            }

            return false;
        }

        private bool HandleGamePadButtonDownEvent(GamePadButtonDownEvent gamePadButtonDownEvent)
        {
            PlayerIndex gamepadIndex = gamePadButtonDownEvent._playerIndex;
            string playerstring = "controller_" + ((int)gamepadIndex);
            int isPlayerSeatedIndex = CheckIfSeated(playerstring); // -1 means not found, else in 0,1,2 or 3
                                                                   // A button is pressed
            if (gamePadButtonDownEvent._pressedButton == Buttons.A)
            {
                if (isPlayerSeatedIndex == -1)
                {
                    // Player is not seated -- Put them in the next available
                    // seat.
                    SeatPlayer(new Player(playerstring, new ControllerInputMethod(gamepadIndex)));
                    return true;
                }
                // TODO: If player is seated we can place code here to handle alternative actions such as color changing
            }

            // B button is pressed
            if (gamePadButtonDownEvent._pressedButton == Buttons.B)
            {
                // If not seated; go back to main menu
                if (isPlayerSeatedIndex < 0)
                {
                    ChangeState(new UIMenuGameState(GameManager, SharedState));
                    return true;
                }
                // when player 1 presses B
                if (isPlayerSeatedIndex == 0)
                {
                    // Blanks all seats
                    _playersSeated = new Player[4];
                    UpdateUI();
                    return true;
                }
                // Blank only the player's seat
                _playersSeated[isPlayerSeatedIndex] = null;
                UpdateUI();
                return true;
            }

            // If start button is pressed
            if (gamePadButtonDownEvent._pressedButton == Buttons.Start)
            {
                if (SeatedPlayerCount > 0)
                {
                    StartGame(CondenseSeatedPlayersArray());
                    return true;
                }
            }

            return false;
        }

        private bool HandleKeyboardKeyDownEvent(KeyboardKeyDownEvent keyboardKeyDownEvent)
        {
            string playerString = "keyboard_";

            InputMethod inputMethod = null;
            if(keyboardKeyDownEvent._key == (Keys)CVars.Get<int>("input_keyboard_primary_rotate_counter_clockwise")
                || keyboardKeyDownEvent._key == (Keys)CVars.Get<int>("input_keyboard_primary_rotate_clockwise"))
            {
                inputMethod = new PrimaryKeyboardInputMethod();
                playerString += "primary";
            }
            if (keyboardKeyDownEvent._key == (Keys)CVars.Get<int>("input_keyboard_secondary_rotate_counter_clockwise")
                || keyboardKeyDownEvent._key == (Keys)CVars.Get<int>("input_keyboard_secondary_rotate_clockwise"))
            {
                inputMethod = new SecondaryKeyboardInputMethod();
                playerString += "secondary";
            }

            int isPlayerSeatedIndex = CheckIfSeated(playerString); // -1 means not found, else in 0,1,2 or 3

            if (isPlayerSeatedIndex == -1 && inputMethod != null)
            {
                // Player is not seated -- Put them in the next available
                // seat.
                SeatPlayer(new Player(playerString, inputMethod));
                return true;
            }
            // TODO: If player is seated we can place code here to handle alternative actions such as color changing

            if (keyboardKeyDownEvent._key == Keys.Enter)
            {
                if (SeatedPlayerCount > 0)
                {
                    StartGame(CondenseSeatedPlayersArray());
                }
            }

            if (keyboardKeyDownEvent._key == Keys.Escape)
            {
                // Remove keyboard player (if there is one)
                for(int i = _playersSeated.Length - 1; i >= 0; i--)
                {
                    if(_playersSeated[i] == null)
                    {
                        continue;
                    }
                    if(_playersSeated[i].InputMethod is PrimaryKeyboardInputMethod
                        || _playersSeated[i].InputMethod is SecondaryKeyboardInputMethod)
                    {
                        if (i == 0)
                        {
                            // Player 1 pressed Escape
                            // Blanks all seats
                            _playersSeated = new Player[4];
                            UpdateUI();
                            return true;
                        }

                        _playersSeated[i] = null;
                        UpdateUI();
                        return true;
                    }
                }

                // Player not removed
                // If not seated; go back to main menu
                if (isPlayerSeatedIndex < 0)
                {
                    ChangeState(new UIMenuGameState(GameManager, SharedState));
                    return true;
                }
            }

            return false;
        }

        private void SeatPlayer(Player player)
        {
            for(int i = 0; i < _playersSeated.Length; i++)
            {
                if(_playersSeated[i] != null)
                {
                    continue;
                }
                _playersSeated[i] = player;
                player.LobbySeatIndex = i;
                break;
            }

            UpdateUI();
        }

        // Returns index when found, else returns -1
        private int CheckIfSeated(string playerstring)
        {
            for (int i = 0; i < _playersSeated.Length; i++)
            {
                if (_playersSeated[i] != null)
                {
                    if(_playersSeated[i].Name == playerstring)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private Player[] CondenseSeatedPlayersArray()
        {
            List<Player> players = new List<Player>();
            foreach(Player player in _playersSeated)
            {
                if(player == null)
                {
                    continue;
                }
                players.Add(player);
            }
            return players.ToArray();
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

        private void UpdateUI()
        {
            UpdatePlayAndJoinInstructions();
            UpdateKeyBindingTextures();
        }
        private void UpdatePlayAndJoinInstructions()
        {
            bool primaryKeyboardInputMethodUsed = false;
            bool secondaryKeyboardInputMethodUsed = false;

            for (int i = 0; i < _playersSeated.Length; i++)
            {
                _root.FindWidgetByID(string.Format("player_{0}_join_instructions", i)).Hidden = _playersSeated[i] != null;
                _root.FindWidgetByID(string.Format("player_{0}_play_instructions", i)).Hidden = _playersSeated[i] == null;
                if(_playersSeated[i] != null)
                {
                    _root.FindWidgetsByClass(string.Format("player_{0}_controls_instruction", i)).ForEach((Widget widget) =>
                    {
                        widget.Hidden = true;
                    });

                    if (_playersSeated[i].InputMethod is ControllerInputMethod)
                    {
                        PlayerIndex controllerIndex = ((ControllerInputMethod)_playersSeated[i].InputMethod).PlayerIndex;

                        ((Image)_root.FindWidgetByID(string.Format("player_{0}_controller_counter_clockwise_texture", i))).Texture = Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion(_gamePadTextureMap[(Buttons)CVars.Get<int>(string.Format("controller_{0}_rotate_counter_clockwise", (int)controllerIndex))]);
                        ((Image)_root.FindWidgetByID(string.Format("player_{0}_controller_counter_clockwise_texture", i))).Hidden = false;
                        ((Image)_root.FindWidgetByID(string.Format("player_{0}_controller_clockwise_texture", i))).Texture = Content.Load<TextureAtlas>("complete_texture_atlas").GetRegion(_gamePadTextureMap[(Buttons)CVars.Get<int>(string.Format("controller_{0}_rotate_clockwise", (int)controllerIndex))]);
                        ((Image)_root.FindWidgetByID(string.Format("player_{0}_controller_clockwise_texture", i))).Hidden = false;
                    }
                    else if (_playersSeated[i].InputMethod is PrimaryKeyboardInputMethod)
                    {
                        _root.FindWidgetsByClass(string.Format("player_{0}_primary_keyboard_instructions", i)).ForEach((Widget widget) =>
                        {
                            widget.Hidden = false;
                        });

                        primaryKeyboardInputMethodUsed = true;
                    }
                    else
                    {
                        // SecondaryKeyboardInputMethod
                        _root.FindWidgetsByClass(string.Format("player_{0}_secondary_keyboard_instructions", i)).ForEach((Widget widget) =>
                        {
                            widget.Hidden = false;
                        });

                        secondaryKeyboardInputMethodUsed = true;
                    }
                }
            }

            _root.FindWidgetsByClass("primary_keyboard_join_instructions").ForEach((Widget widget) =>
            {
                widget.Hidden = primaryKeyboardInputMethodUsed;
            });
            _root.FindWidgetsByClass("secondary_keyboard_join_instructions").ForEach((Widget widget) =>
            {
                widget.Hidden = secondaryKeyboardInputMethodUsed;
            });
        }
        private void UpdateKeyBindingTextures()
        {
            _root.FindWidgetsByClass("primary_keyboard_counter_clockwise_texture").ForEach((Widget widget) =>
            {
                Image image = widget as Image;
                if(image != null)
                {
                    image.Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                    .GetRegion((_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_primary_rotate_counter_clockwise")]));
                }
            });
            _root.FindWidgetsByClass("primary_keyboard_clockwise_texture").ForEach((Widget widget) =>
            {
                Image image = widget as Image;
                if (image != null)
                {
                    image.Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                    .GetRegion((_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_primary_rotate_clockwise")]));
                }
            });

            _root.FindWidgetsByClass("secondary_keyboard_counter_clockwise_texture").ForEach((Widget widget) =>
            {
                Image image = widget as Image;
                if (image != null)
                {
                    image.Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                        .GetRegion((_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_secondary_rotate_counter_clockwise")]));
                }
            });
            _root.FindWidgetsByClass("secondary_keyboard_clockwise_texture").ForEach((Widget widget) =>
            {
                Image image = widget as Image;
                if (image != null)
                {
                    image.Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                        .GetRegion((_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_secondary_rotate_clockwise")]));
                }
            });
        }
    }
}
