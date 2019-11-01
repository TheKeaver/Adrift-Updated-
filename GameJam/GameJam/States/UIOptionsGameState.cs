using System;
using System.Collections.Generic;
using Events;
using GameJam.Directors;
using GameJam.Events;
using GameJam.Events.InputHandling;
using GameJam.Events.Settings;
using GameJam.Events.UI;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Content.Pipeline;

namespace GameJam.States
{
    public class UIOptionsGameState : CommonGameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        bool rotateLeftBindingMode = false;
        bool rotateRightBindingMode = false;

        bool isOnLeftSide = true;
        int leftSideIndex = 0;

        public UIOptionsGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        public bool Handle(IEvent evt)
        {
            GamePadButtonDownEvent gpbde = evt as GamePadButtonDownEvent;
            if( gpbde != null )
            {
                if ( rotateLeftBindingMode == false && rotateRightBindingMode == false && gpbde._pressedButton == Buttons.B )
                {
                    _root.FindSelectedWidget().isSelected = false;
                    if ( isOnLeftSide == true )
                    {
                        ChangeState(new UIMenuGameState(GameManager, SharedState));
                    }
                    if ( isOnLeftSide == false )
                    {
                        switch( leftSideIndex )
                        {
                            case 0:
                                ((Button)_root.FindWidgetByID("Display")).isSelected = true;
                                isOnLeftSide = true;
                                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = true;
                                break;
                            case 1:
                                ((Button)_root.FindWidgetByID("Controls")).isSelected = true;
                                isOnLeftSide = true;
                                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = true;
                                break;
                            case 2:
                                ((Button)_root.FindWidgetByID("GameSettings")).isSelected = true;
                                isOnLeftSide = true;
                                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = true;
                                break;
                        }
                    }
                }
                if (this.rotateLeftBindingMode == true)
                {
                    switch (gpbde._playerIndex)
                    {
                        case PlayerIndex.One:
                            CVars.Get<int>("controller_0_rotate_left") = ((int)gpbde._pressedButton);
                            break;
                        case PlayerIndex.Two:
                            CVars.Get<int>("controller_1_rotate_left") = ((int)gpbde._pressedButton);
                            break;
                        case PlayerIndex.Three:
                            CVars.Get<int>("controller_2_rotate_left") = ((int)gpbde._pressedButton);
                            break;
                        case PlayerIndex.Four:
                            CVars.Get<int>("controller_3_rotate_left") = ((int)gpbde._pressedButton);
                            break;
                    }
                    CVars.Save();
                    this.rotateLeftBindingMode = false;
                    ((Button)_root.FindWidgetByID("Rotate_Left")).isSelected = true;
                    return true;
                }
                if (this.rotateRightBindingMode == true)
                {
                    switch (gpbde._playerIndex)
                    {
                        case PlayerIndex.One:
                            CVars.Get<int>("controller_0_rotate_right") = ((int)gpbde._pressedButton);
                            break;
                        case PlayerIndex.Two:
                            CVars.Get<int>("controller_1_rotate_right") = ((int)gpbde._pressedButton);
                            break;
                        case PlayerIndex.Three:
                            CVars.Get<int>("controller_2_rotate_right") = ((int)gpbde._pressedButton);
                            break;
                        case PlayerIndex.Four:
                            CVars.Get<int>("controller_3_rotate_right") = ((int)gpbde._pressedButton);
                            break;
                    }
                    CVars.Save();
                    this.rotateRightBindingMode = false;
                    ((Button)_root.FindWidgetByID("Rotate_Right")).isSelected = true;
                    return true;
                }
            }

            KeyboardKeyDownEvent kbkde = evt as KeyboardKeyDownEvent;
            if( kbkde != null )
            {
                if( kbkde.Key == Keys.Escape )
                {
                    if (isOnLeftSide == true)
                    {
                        ChangeState(new UIMenuGameState(GameManager, SharedState));
                    }
                    if ( isOnLeftSide == false )
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
                }
            }
            // Listen for the 3 types of button settings pressed
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
            if (gameSBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 2;
                Console.WriteLine("gameSBPE");
                ((Button)_root.FindWidgetByID("GameSettings")).isSelected = false;
                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = false;
                ((Button)_root.FindWidgetByID("Speed")).isSelected = true;
            }

            FullScreenSettingsButtonPressedEvent fullscreenSBPE = evt as FullScreenSettingsButtonPressedEvent;
            if (fullscreenSBPE != null)
            {
                Console.WriteLine("fullSBPE");
                // Set to full screen
                CVars.Get<bool>("display_windowed") = false;
                CVars.Get<bool>("display_borderless") = false;
                CVars.Get<bool>("display_fullscreen") = true;
                // TODO : Generate event to force GameManager to change to correct settings
            }
            WindowedSettingsButtonPressed windowedSBPE = evt as WindowedSettingsButtonPressed;
            if (windowedSBPE != null)
            {
                Console.WriteLine("windowedSBPE");
                // Set to windowed
                CVars.Get<bool>("display_windowed") = true;
                CVars.Get<bool>("display_borderless") = false;
                CVars.Get<bool>("display_fullscreen") = false;
                // TODO : Generate event to force GameManager to change to correct settings

            }
            BorderlessWindowButtonPressedEvent borderlessWindowSBPE = evt as BorderlessWindowButtonPressedEvent;
            if (borderlessWindowSBPE != null)
            {
                Console.WriteLine("bordelessWindowSBPE");
                // Set to borderless window
                CVars.Get<bool>("display_windowed") = false;
                CVars.Get<bool>("display_borderless") = true;
                CVars.Get<bool>("display_fullscreen") = false;
                // TODO : Generate event to force GameManager to change to correct settings
            }

            AAFXAASettingsButtonPressedEvent aafxaaSBPE = evt as AAFXAASettingsButtonPressedEvent;
            if (aafxaaSBPE != null)
            {
                Console.WriteLine("aafxaaSBPE");
                CVars.Get<bool>("graphics_fxaa") = false;
                CVars.Get<bool>("graphics_feathering").Equals(false);
            }
            AAFeatheringButtonPressedEvent aafeatherSBPE = evt as AAFeatheringButtonPressedEvent;
            if (aafeatherSBPE != null)
            {
                Console.WriteLine("aafeatherSBPE");
                CVars.Get<bool>("graphics_fxaa") = false;
                CVars.Get<bool>("graphics_feathering").Equals(true);
            }
            AAOffButtonPressedEvent aaoffSBPE = evt as AAOffButtonPressedEvent;
            if (aaoffSBPE != null)
            {
                Console.WriteLine("aaoffSBPE");
                CVars.Get<bool>("graphics_fxaa") = false;
                CVars.Get<bool>("graphics_feathering").Equals(false);
            }

            RotateLeftSettingsButtonPressedEvent rlSBPE = evt as RotateLeftSettingsButtonPressedEvent;
            if (rlSBPE != null)
            {
                Console.WriteLine("rlSBPE");
                // Rotate Left Button Clicked, enter into button binding state
                rotateLeftBindingMode = true;
                ((Button)_root.FindWidgetByID("Rotate_Left")).isSelected = false;
            }
            RotateRightSettingsButtonPressedEvent rrSBPE = evt as RotateRightSettingsButtonPressedEvent;
            if (rrSBPE != null)
            {
                Console.WriteLine("rrSBPE");
                // Rotate Right Button Clicked, enter into button binding state
                rotateRightBindingMode = true;
                ((Button)_root.FindWidgetByID("Rotate_Right")).isSelected = false;
            }

            SpeedSettingsButtonPressedEvent speedSBPE = evt as SpeedSettingsButtonPressedEvent;
            if (speedSBPE != null)
            {
                Console.WriteLine("speedSBPE");
                // Save settings to the speed set by the player by modifying the cvars
            }
            DifficultySettingsButtonPressedEvent difficultySBPE = evt as DifficultySettingsButtonPressedEvent;
            if (difficultySBPE != null)
            {
                Console.WriteLine("difficultySBPE");
                // Save settings to the spawn rate set by the player by modifying the cvars
            }

            return false;
        }
        // Select these below when making new GameState
        protected override void OnInitialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);

            LoadContent();

            base.OnInitialize();
        }

        private void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_options_menu"));
        }

        protected override void RegisterListeners()
        {
            _root.RegisterListeners(); // Root must be registered first because of "B" button event consumption

            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);

            EventManager.Instance.RegisterListener<DisplaySettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<ControlsSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<GameSettingsButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<FullScreenSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<WindowedSettingsButtonPressed>(this);
            EventManager.Instance.RegisterListener<BorderlessWindowButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<AAFXAASettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<AAFeatheringButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<AAOffButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<RotateLeftSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<RotateRightSettingsButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<SpeedSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<DifficultySettingsButtonPressedEvent>(this);

            base.RegisterListeners();
        }

        protected override void UnregisterListeners()
        {
            _root.UnregisterListeners();
            EventManager.Instance.UnregisterListener(this);

            base.UnregisterListeners();
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
            base.OnKill();
        }
    }
}
