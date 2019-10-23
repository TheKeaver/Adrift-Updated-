using System;
using System.Collections.Generic;
using Events;
using GameJam.Events.InputHandling;
using GameJam.Events.UI;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Content.Pipeline;

namespace GameJam.States
{
    public class UIOptionsGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        bool isOnLeftSide = true;
        int leftSideIndex = 0;

        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public UIOptionsGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);

            EventManager.Instance.RegisterListener<DisplaySettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<ControlsSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<GameSettingsButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<SpeedSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<DifficultySettingsButtonPressedEvent>(this);
        }

        void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override void Dispose()
        {
            UnregisterEvents();
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Hide()
        {
            _root.UnregisterListeners();
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            RegisterEvents();

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/OptionsMenu"));
        }

        public override void Show()
        {
            _root.RegisterListeners();
        }

        public override void Update(float dt)
        {
            ProcessManager.Update(dt);
        }

        public bool Handle(IEvent evt)
        {
            // Listen for the 4 types of button settings pressed
            // Consider buttonSelectedEvent and buttonDeselectedEvent to allow showing of right side
            DisplaySettingsButtonPressedEvent displaySBPE = evt as DisplaySettingsButtonPressedEvent;
            if (displaySBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 0;
                Console.WriteLine("displaySBPE");
                ((Button)_root.FindWidgetByID("Display")).isSelected = false;
                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = false;
                ((Button)_root.FindWidgetByID("FullScreen")).isSelected = true;
            }
            ControlsSettingsButtonPressedEvent controlsSBPE = evt as ControlsSettingsButtonPressedEvent;
            if (controlsSBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 1;
                Console.WriteLine("controlSBPE");
                ((Button)_root.FindWidgetByID("Controls")).isSelected = false;
                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = false;
                ((Button)_root.FindWidgetByID("Rotate_Left")).isSelected = true;
            }
            GameSettingsButtonPressedEvent gameSBPE = evt as GameSettingsButtonPressedEvent;
            if( gameSBPE != null )
            {
                isOnLeftSide = false;
                leftSideIndex = 2;
                Console.WriteLine("gameSBPE");
                ((Button)_root.FindWidgetByID("GameSettings")).isSelected = false;
                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = false;
                ((Button)_root.FindWidgetByID("Speed")).isSelected = true;
            }
            SpeedSettingsButtonPressedEvent speedSBPE = evt as SpeedSettingsButtonPressedEvent;
            if( speedSBPE != null )
            {
                Console.WriteLine("speedSBPE");
                // Save settings to the speed set by the player by modifying the cvars
            }
            DifficultySettingsButtonPressedEvent difficultySBPE = evt as DifficultySettingsButtonPressedEvent;
            if( difficultySBPE != null )
            {
                Console.WriteLine("difficultySBPE");
                // Save settings to the spawn rate set by the player by modifying the cvars
            }

            GamePadButtonDownEvent gpbde = evt as GamePadButtonDownEvent;
            if( gpbde != null )
            {
                if( gpbde._pressedButton == Buttons.B )
                {
                    if( isOnLeftSide == false )
                    {
                        switch( leftSideIndex )
                        {
                            case 0:
                                ((Button)_root.FindWidgetByID("Display")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = true;
                                break;
                            case 1:
                                ((Button)_root.FindWidgetByID("Controls")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = true;
                                break;
                            case 2:
                                ((Button)_root.FindWidgetByID("GameSettings")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = true;
                                break;
                        }
                    }
                    if( isOnLeftSide == true )
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                    }
                }
            }

            KeyboardKeyDownEvent kbkde = evt as KeyboardKeyDownEvent;
            if( kbkde != null )
            {
                if( kbkde._keyPressed == Keys.Escape )
                {
                    if( isOnLeftSide == false )
                    {
                        switch (leftSideIndex)
                        {
                            case 0:
                                ((Button)_root.FindWidgetByID("Display")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = true;
                                break;
                            case 1:
                                ((Button)_root.FindWidgetByID("Controls")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = true;
                                break;
                            case 2:
                                ((Button)_root.FindWidgetByID("GameSettings")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = true;
                                break;
                        }
                    }
                    if( isOnLeftSide == true )
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                    }
                }
            }
            return false;
        }
    }
}
