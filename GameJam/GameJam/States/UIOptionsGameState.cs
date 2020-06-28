﻿using System;
using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Events;
using GameJam.Content;
using GameJam.Events;
using GameJam.Events.InputHandling;
using GameJam.Events.Settings;
using GameJam.Events.UI;
using GameJam.Graphics.Text;
using GameJam.Processes.Menu;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.States
{
    public class UIOptionsGameState : CommonGameState, IEventListener
    {
        enum BindingControl
        {
            none,
            rotate_counter_clockwise,
            rotate_clockwise,
            super_shield
        }
        enum BindingMode
        {
            none,
            gamepad,
            primary,
            secondary
        }

        SpriteBatch _spriteBatch;
        FieldFontRenderer _fieldFontRenderer;
        Root _root;

        KeyTextureMap _keyTextureMap;
        GamePadTextureMap _gamePadTextureMap;

        //PlayerIndex bindingIndex;

        List<string> supportedResolutions;
        int resolutionIndex = 0;

        BindingControl bindingControl = BindingControl.none;
        BindingMode bindingMode = BindingMode.none;

        bool isOnLeftSide = true;
        int leftSideIndex = 0;

        public UIOptionsGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
            _fieldFontRenderer = new FieldFontRenderer(Content, GameManager.GraphicsDevice);
            supportedResolutions = new List<string>();

            IEnumerator<DisplayMode> enumerator = GameManager.Graphics.GraphicsDevice.Adapter.SupportedDisplayModes.GetEnumerator();

            while(enumerator.MoveNext())
            {
                supportedResolutions.Add(enumerator.Current.ToString());
            }
        }

        public bool Handle(IEvent evt)
        {
            if(evt is EnterMouseUIModeEvent)
            {
                UpdateButtonBindingsForKeyboard();
            }
            if(evt is EnterGamePadUIModeEvent)
            {
                UpdateButtonBindingsForGamePad(((EnterGamePadUIModeEvent)evt).PlayerIndex);
            }
            if(evt is GamePadUIModeOperatorChangedEvent)
            {
                UpdateButtonBindingsForGamePad(((GamePadUIModeOperatorChangedEvent)evt).PlayerIndex);
            }

            GamePadButtonDownEvent gpbde = evt as GamePadButtonDownEvent;
            if( gpbde != null )
            {
                if ( bindingMode == BindingMode.none && gpbde._pressedButton == Buttons.B )
                {
                    _root.FindSelectedWidget().isSelected = false;
                    if (isOnLeftSide == true)
                    {
                        ChangeState(new UIMenuGameState(GameManager, SharedState));
                        return true;
                    }
                    if ( isOnLeftSide == false )
                    {
                        foreach ( DropDownPanel ddp in _root.FindWidgetsByClass("DropDown") )
                        {
                            ddp.ShowDropDown = false;
                            ddp.isSelected = false;
                        }

                        switch( leftSideIndex )
                        {
                            case 0:
                                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = true;
                                ((Button)_root.FindWidgetByID("Display")).isSelected = true;
                                isOnLeftSide = true;
                                break;
                            case 1:
                                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = true;
                                ((Button)_root.FindWidgetByID("Controls")).isSelected = true;
                                isOnLeftSide = true;
                                break;
                            case 2:
                                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = true;
                                ((Button)_root.FindWidgetByID("GameSettings")).isSelected = true;
                                isOnLeftSide = true;
                                break;
                        }
                    }
                }
                if (bindingMode == BindingMode.gamepad)
                {
                    BindingControl other1 = BindingControl.none;
                    BindingControl other2 = BindingControl.none;
                    switch (gpbde._pressedButton)
                    {
                        case Buttons.A:
                        case Buttons.B:
                        case Buttons.X:
                        case Buttons.Y:
                        case Buttons.LeftTrigger:
                        case Buttons.LeftShoulder:
                        case Buttons.LeftStick:
                        case Buttons.RightTrigger:
                        case Buttons.RightShoulder:
                        case Buttons.RightStick:
                        case Buttons.DPadUp:
                        case Buttons.DPadDown:
                        case Buttons.DPadLeft:
                        case Buttons.DPadRight:
                        case Buttons.LeftThumbstickUp:
                        case Buttons.LeftThumbstickDown:
                        case Buttons.LeftThumbstickLeft:
                        case Buttons.LeftThumbstickRight:
                        case Buttons.RightThumbstickUp:
                        case Buttons.RightThumbstickDown:
                        case Buttons.RightThumbstickLeft:
                        case Buttons.RightThumbstickRight:
                            switch (bindingControl)
                            {
                                case BindingControl.rotate_counter_clockwise:
                                    other1 = BindingControl.rotate_clockwise;
                                    other2 = BindingControl.super_shield;
                                    break;
                                case BindingControl.rotate_clockwise:
                                    other1 = BindingControl.rotate_counter_clockwise;
                                    other2 = BindingControl.super_shield;
                                    break;
                                case BindingControl.super_shield:
                                    other1 = BindingControl.rotate_counter_clockwise;
                                    other2 = BindingControl.rotate_clockwise;
                                    break;
                            }
                            // if 'other1' is assigned to the value the user attempts to bind to 'bindingControl' then the two will swap values
                            if (CVars.Get<int>(string.Format($"input_controller_{(int)gpbde._playerIndex}_{other1}")) == (int)gpbde._pressedButton)
                            {
                                CVars.Get<int>(string.Format($"input_controller_{(int)gpbde._playerIndex}_{other1}")) = CVars.Get<int>(string.Format($"input_controller_{(int)gpbde._playerIndex}_{bindingControl}"));
                            }
                            // if 'other2' is assigned to the value the user attempts to bind to 'bindingControl' then the two will swap values
                            if (CVars.Get<int>(string.Format($"input_controller_{(int)gpbde._playerIndex}_{other2}")) == (int)gpbde._pressedButton)
                            {
                                CVars.Get<int>(string.Format($"input_controller_{(int)gpbde._playerIndex}_{other2}")) = CVars.Get<int>(string.Format($"input_controller_{(int)gpbde._playerIndex}_{bindingControl}"));
                            }
                           CVars.Get<int>(string.Format($"input_controller_{(int)gpbde._playerIndex}_{bindingControl}")) = (int)gpbde._pressedButton;
                           break;
                        default:
                            return true;
                    }
                    CVars.Save();
                    UpdateButtonBindingsForGamePad(gpbde._playerIndex);
                    ((Button)_root.FindWidgetByID(string.Format($"{bindingControl}"))).isSelected = true;
                    bindingControl = BindingControl.none;
                    _root.AutoControlModeSwitching = true;
                    return true;
                }
            }

            KeyboardKeyDownEvent kbkde = evt as KeyboardKeyDownEvent;
            if( kbkde != null )
            {
                return HandleKeyboardKeyDownEvent(kbkde);
            }

            // Listen for the 3 types of button settings pressed
            // Consider buttonSelectedEvent and buttonDeselectedEvent to allow showing of right side
            DisplaySettingsButtonPressedEvent displaySBPE = evt as DisplaySettingsButtonPressedEvent;
            if (displaySBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 0;
                Console.WriteLine("displaySBPE");

                //_root.FindSelectedWidget().isSelected = false;
                ((Button)_root.FindWidgetByID("Display")).isSelected = false;
                ((DropDownPanel)_root.FindWidgetByID("Screen_Size_Settings_Dropdown")).isSelected = true;

                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = true;
                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = true;
                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = false;
            }
            ControlsSettingsButtonPressedEvent controlsSBPE = evt as ControlsSettingsButtonPressedEvent;
            if (controlsSBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 1;
                Console.WriteLine("controlSBPE");

                //_root.FindSelectedWidget().isSelected = false;
                ((Button)_root.FindWidgetByID("Controls")).isSelected = false;
                ((Button)_root.FindWidgetByID("primary_rotate_counter_clockwise")).isSelected = true;

                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = false;
                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = true;
                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = true;
            }
            GameSettingsButtonPressedEvent gameSBPE = evt as GameSettingsButtonPressedEvent;
            if (gameSBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 2;
                Console.WriteLine("gameSBPE");

                //_root.FindSelectedWidget().isSelected = false;
                ((Button)_root.FindWidgetByID("GameSettings")).isSelected = false;
                ((DropDownPanel)_root.FindWidgetByID("Speed_Dropdown")).isSelected = true;

                ((Panel)_root.FindWidgetByID("controls_options_menu_right_panel")).Hidden = true;
                ((Panel)_root.FindWidgetByID("game_options_menu_right_panel")).Hidden = false;
                ((Panel)_root.FindWidgetByID("display_options_menu_right_panel")).Hidden = true;
            }

            FullScreenSettingsButtonPressedEvent fullscreenSBPE = evt as FullScreenSettingsButtonPressedEvent;
            if (fullscreenSBPE != null)
            {
                ((Label)_root.FindWidgetByID("Screen_Size_Settings_Dropdown_Label")).Content = "Screen Size: Full Screen";
                // Set to full screen
                CVars.Get<bool>("display_windowed") = false;
                CVars.Get<bool>("display_borderless") = false;
                CVars.Get<bool>("display_fullscreen") = true;
                // Generate event to force GameManager to change to correct settings
                EventManager.Instance.QueueEvent(new ReloadDisplayOptionsEvent());
            }
            WindowedSettingsButtonPressed windowedSBPE = evt as WindowedSettingsButtonPressed;
            if (windowedSBPE != null)
            {
                ((Label)_root.FindWidgetByID("Screen_Size_Settings_Dropdown_Label")).Content = "Screen Size: Windowed";
                // Set to windowed
                CVars.Get<bool>("display_windowed") = true;
                CVars.Get<bool>("display_borderless") = false;
                CVars.Get<bool>("display_fullscreen") = false;
                // Generate event to force GameManager to change to correct settings
                EventManager.Instance.QueueEvent(new ReloadDisplayOptionsEvent());
            }
            BorderlessWindowButtonPressedEvent borderlessWindowSBPE = evt as BorderlessWindowButtonPressedEvent;
            if (borderlessWindowSBPE != null)
            {
                ((Label)_root.FindWidgetByID("Screen_Size_Settings_Dropdown_Label")).Content = "Screen Size: Borderless";
                // Set to borderless window
                CVars.Get<bool>("display_windowed") = false;
                CVars.Get<bool>("display_borderless") = true;
                CVars.Get<bool>("display_fullscreen") = false;
                // Generate event to force GameManager to change to correct settings
                EventManager.Instance.QueueEvent(new ReloadDisplayOptionsEvent());
            }
            /*
            AAFXAASettingsButtonPressedEvent aafxaaSBPE = evt as AAFXAASettingsButtonPressedEvent;
            if (aafxaaSBPE != null)
            {
                ((Label)_root.FindWidgetByID("FXAA_Settings_Dropdown_Label")).Content = "Anti-Alias: FXAA";
                CVars.Get<bool>("graphics_fxaa") = true;
                CVars.Get<bool>("graphics_feathering") = false;
                EventManager.Instance.QueueEvent(new ReloadDisplayOptionsEvent());
            }
            */
            AASMAASettingsButtonPressedEvent aasmaaSBPE = evt as AASMAASettingsButtonPressedEvent;
            if (aasmaaSBPE != null)
            {
                ((Label)_root.FindWidgetByID("AA_Settings_Dropdown_Label")).Content = "Anti-Alias: SMAA";
                CVars.Get<bool>("graphics_smaa") = true;
                CVars.Get<bool>("graphics_feathering") = false;
                EventManager.Instance.QueueEvent(new ReloadDisplayOptionsEvent());
            }
            AAFeatheringButtonPressedEvent aafeatherSBPE = evt as AAFeatheringButtonPressedEvent;
            if (aafeatherSBPE != null)
            {
                ((Label)_root.FindWidgetByID("AA_Settings_Dropdown_Label")).Content = "Anti-Alias: Feathering";
                CVars.Get<bool>("graphics_smaa") = false;
                CVars.Get<bool>("graphics_feathering") = true;
                EventManager.Instance.QueueEvent(new ReloadDisplayOptionsEvent());
            }
            AAOffButtonPressedEvent aaoffSBPE = evt as AAOffButtonPressedEvent;
            if (aaoffSBPE != null)
            {
                ((Label)_root.FindWidgetByID("AA_Settings_Dropdown_Label")).Content = "Anti-Alias: Off";
                CVars.Get<bool>("graphics_smaa") = false;
                CVars.Get<bool>("graphics_feathering") = false;
                EventManager.Instance.QueueEvent(new ReloadDisplayOptionsEvent());
            }

            RotateLeftSettingsButtonPressedEvent rlSBPE = evt as RotateLeftSettingsButtonPressedEvent;
            if (rlSBPE != null)
            {
                Console.WriteLine("rlSBPE");
                // Rotate Left Button Clicked, enter into button binding state
                bindingControl = BindingControl.rotate_counter_clockwise;
                bindingMode = (_root.MouseMode == true) ? BindingMode.primary : BindingMode.gamepad;
                /*
                 * bindingMode = BindingMode.primary;
                bindingGamepad = !_root.MouseMode;
                */
                _root.AutoControlModeSwitching = false;
                ((Button)_root.FindWidgetByID("primary_rotate_counter_clockwise")).isSelected = false;
            }
            RotateRightSettingsButtonPressedEvent rrSBPE = evt as RotateRightSettingsButtonPressedEvent;
            if (rrSBPE != null)
            {
                Console.WriteLine("rrSBPE");
                // Rotate Right Button Clicked, enter into button binding state
                bindingControl = BindingControl.rotate_clockwise;
                bindingMode = (_root.MouseMode == true) ? BindingMode.primary : BindingMode.gamepad;
                _root.AutoControlModeSwitching = false;
                ((Button)_root.FindWidgetByID("primary_rotate_clockwise")).isSelected = false;
            }
            if (evt is SuperShieldSettingsButtonPressedEvent)
            {
                Console.WriteLine("Super Shield Pressed");
                bindingControl = BindingControl.super_shield;
                bindingMode = (_root.MouseMode == true) ? BindingMode.primary : BindingMode.gamepad;
                _root.AutoControlModeSwitching = false;
                ((Button)_root.FindWidgetByID("primary_super_shield")).isSelected = false;
            }
            if (evt is SecondaryRotateLeftSettingsButtonPressedEvent)
            {
                Console.WriteLine("secondary_rlSBPE");
                // Rotate Left Button Clicked, enter into button binding state
                bindingControl = BindingControl.rotate_counter_clockwise;
                bindingMode = (_root.MouseMode == true) ? BindingMode.secondary : BindingMode.gamepad;
                _root.AutoControlModeSwitching = false;
                ((Button)_root.FindWidgetByID("secondary_rotate_counter_clockwise")).isSelected = false;
            }
            if (evt is SecondaryRotateRightSettingsButtonPressedEvent)
            {
                Console.WriteLine("secondary_rrSBPE");
                // Rotate Right Button Clicked, enter into button binding state
                bindingControl = BindingControl.rotate_clockwise;
                bindingMode = (_root.MouseMode == true) ? BindingMode.secondary : BindingMode.gamepad;
                _root.AutoControlModeSwitching = false;
                ((Button)_root.FindWidgetByID("secondary_rotate_clockwise")).isSelected = false;
            }
            if (evt is SecondarySuperShieldSettingsButtonPressedEvent)
            {
                Console.WriteLine("Super Shield Pressed");
                bindingControl = BindingControl.super_shield;
                bindingMode = (_root.MouseMode == true) ? BindingMode.secondary : BindingMode.gamepad;
                _root.AutoControlModeSwitching = false;
                ((Button)_root.FindWidgetByID("secondary_super_shield")).isSelected = false;
            }
            if (evt is ResolutionButtonPressedEvent)
            {
                ((Label)_root.FindWidgetByID("Resolution_Button_Label")).Content = SetNextResolution();
                GameManager.Graphics.IsFullScreen = false;
                GameManager.Graphics.HardwareModeSwitch = true;
                GameManager.Graphics.ApplyChanges();
                EventManager.Instance.QueueEvent(new ReloadDisplayOptionsEvent());
            }
            if(evt is VSyncButtonPressedEvent)
            {
                bool replacement = !(CVars.Get<bool>("display_vsync"));
                CVars.Get<bool>("display_vsync") = replacement;
                ((Label)_root.FindWidgetByID("VSync_Button_Label")).Content = 
                    (replacement == true) ?
                    "V-Sync: On" :
                    "V-Sync: Off";
            }

            return false;
        }
        
        private string SetNextResolution()
        {
            Console.WriteLine("Set Next Resolution Called");
            resolutionIndex = (resolutionIndex == supportedResolutions.Count-1) ? 0 : resolutionIndex+1;
            string returnString = supportedResolutions[resolutionIndex];
            
            string[] splitterList = { "{Width:", " Height:", "Format:" };
            string[] resolutionList = returnString.Split(splitterList, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in resolutionList)
            {
                Console.WriteLine(item);
            }

            Int32.TryParse(resolutionList[0], out int width);
            Int32.TryParse(resolutionList[1], out int height);

            CVars.Get<int>("display_fullscreen_width") = width;
            CVars.Get<int>("display_fullscreen_height") = height;

            return width + " x " + height;
        }
        private bool HandleKeyboardKeyDownEvent(KeyboardKeyDownEvent keyboardKeyDownEvent)
        {
            if(bindingMode == BindingMode.primary || bindingMode == BindingMode.secondary)
            {
                BindingControl other1 = BindingControl.none;
                    BindingControl other2 = BindingControl.none;
                switch(keyboardKeyDownEvent._key)
                {
                    case Keys.D0:
                    case Keys.D1:
                    case Keys.D2:
                    case Keys.D3:
                    case Keys.D4:
                    case Keys.D5:
                    case Keys.D6:
                    case Keys.D7:
                    case Keys.D8:
                    case Keys.D9:

                    case Keys.A:
                    case Keys.B:
                    case Keys.C:
                    case Keys.D:
                    case Keys.E:
                    case Keys.F:
                    case Keys.G:
                    case Keys.H:
                    case Keys.I:
                    case Keys.J:
                    case Keys.K:
                    case Keys.L:
                    case Keys.M:
                    case Keys.N:
                    case Keys.O:
                    case Keys.P:
                    case Keys.Q:
                    case Keys.R:
                    case Keys.S:
                    case Keys.T:
                    case Keys.U:
                    case Keys.V:
                    case Keys.W:
                    case Keys.X:
                    case Keys.Y:
                    case Keys.Z:

                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:

                    case Keys.OemOpenBrackets:
                    case Keys.OemCloseBrackets:
                    case Keys.OemSemicolon:
                    case Keys.OemQuotes:
                    case Keys.OemComma:
                    case Keys.OemPeriod:
                    case Keys.OemQuestion:
                    case Keys.OemBackslash:
                    case Keys.OemMinus:
                    case Keys.OemPlus:
                    case Keys.Space:
                        switch (bindingControl)
                        {
                            case BindingControl.rotate_counter_clockwise:
                                other1 = BindingControl.rotate_clockwise;
                                other2 = BindingControl.super_shield;
                                break;
                            case BindingControl.rotate_clockwise:
                                other1 = BindingControl.rotate_counter_clockwise;
                                other2 = BindingControl.super_shield;
                                break;
                            case BindingControl.super_shield:
                                other1 = BindingControl.rotate_counter_clockwise;
                                other2 = BindingControl.rotate_clockwise;
                                break;
                        }
                        // if 'other1' is assigned to the value the user attempts to bind to 'bindingControl' then the two will swap values
                        if (CVars.Get<int>(string.Format($"input_keyboard_{bindingMode}_{other1}")) == (int)keyboardKeyDownEvent._key)
                        {
                            CVars.Get<int>(string.Format($"input_keyboard_{bindingMode}_{other1}")) = CVars.Get<int>(string.Format($"input_keyboard_{bindingMode}_{bindingControl}"));
                        }
                        // if 'other2' is assigned to the value the user attempts to bind to 'bindingControl' then the two will swap values
                        if (CVars.Get<int>(string.Format($"input_keyboard_{bindingMode}_{other2}")) == (int)keyboardKeyDownEvent._key)
                        {
                            CVars.Get<int>(string.Format($"input_keyboard_{bindingMode}_{other2}")) = CVars.Get<int>(string.Format($"input_keyboard_{bindingMode}_{bindingControl}"));
                        }
                        CVars.Get<int>(string.Format($"input_keyboard_{bindingMode}_{bindingControl}")) = (int)keyboardKeyDownEvent._key;
                        CVars.Save();
                        UpdateButtonBindingsForKeyboard();
                        bindingMode = BindingMode.none;
                        bindingControl = BindingControl.none;
                        //((Button)_root.FindWidgetByID("primary_rotate_rotate_counter_clockwise")).isSelected = true;
                        _root.AutoControlModeSwitching = true;
                        return true;
                    default:
                        break;
                }
            }
            else
            {
                if (keyboardKeyDownEvent._key == Keys.Escape)
                {
                    if (isOnLeftSide)
                    {
                        ChangeState(new UIMenuGameState(GameManager, SharedState));
                    }
                    if (!isOnLeftSide)
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
                    return true;
                }
            }

            return false;
        }

        // Select these below when making new GameState
        protected override void OnInitialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);
            _root.AutoControlModeSwitching = true;

            LoadContent();

            ((Button)_root.FindWidgetByID("0.5x")).Action = () => { CVars.Get<float>("game_speed") = 0.5f; EventManager.Instance.QueueEvent(new SpeedSettingsButtonPressedEvent()); ((Label)_root.FindWidgetByID("Speed_Dropdown_Label")).Content = "Speed: 0.5x"; };
            ((Button)_root.FindWidgetByID("1.0x")).Action = () => { CVars.Get<float>("game_speed") = 1.0f; EventManager.Instance.QueueEvent(new SpeedSettingsButtonPressedEvent()); ((Label)_root.FindWidgetByID("Speed_Dropdown_Label")).Content = "Speed: 1.0x"; };
            ((Button)_root.FindWidgetByID("1.5x")).Action = () => { CVars.Get<float>("game_speed") = 1.5f; EventManager.Instance.QueueEvent(new SpeedSettingsButtonPressedEvent()); ((Label)_root.FindWidgetByID("Speed_Dropdown_Label")).Content = "Speed: 1.5x"; };
            ((Button)_root.FindWidgetByID("2.0x")).Action = () => { CVars.Get<float>("game_speed") = 2.0f; EventManager.Instance.QueueEvent(new SpeedSettingsButtonPressedEvent()); ((Label)_root.FindWidgetByID("Speed_Dropdown_Label")).Content = "Speed: 2.0x"; };

            ((Button)_root.FindWidgetByID("Easy")).Action = () => { CVars.Get<int>("game_difficulty") = 0; EventManager.Instance.QueueEvent(new DifficultySettingsButtonPressedEvent()); ((Label)_root.FindWidgetByID("Difficulty_Dropdown_Label")).Content = "Difficulty: Easy"; };
            ((Button)_root.FindWidgetByID("Normal")).Action = () => { CVars.Get<int>("game_difficulty") = 1; EventManager.Instance.QueueEvent(new DifficultySettingsButtonPressedEvent()); ((Label)_root.FindWidgetByID("Difficulty_Dropdown_Label")).Content = "Difficulty: Normal"; };
            ((Button)_root.FindWidgetByID("Hard")).Action = () => { CVars.Get<int>("game_difficulty") = 2; EventManager.Instance.QueueEvent(new DifficultySettingsButtonPressedEvent()); ((Label)_root.FindWidgetByID("Difficulty_Dropdown_Label")).Content = "Difficulty: Hard"; };

            ((Button)_root.FindWidgetByID("SMAA")).Action = () => { EventManager.Instance.QueueEvent(new AASMAASettingsButtonPressedEvent()); };
            ((Button)_root.FindWidgetByID("Feathering")).Action = () => { EventManager.Instance.QueueEvent(new AAFeatheringButtonPressedEvent()); };
            ((Button)_root.FindWidgetByID("Off")).Action = () => { EventManager.Instance.QueueEvent(new AAOffButtonPressedEvent()); };

            ((Button)_root.FindWidgetByID("FullScreen")).Action = () => { EventManager.Instance.QueueEvent(new FullScreenSettingsButtonPressedEvent()); };
            ((Button)_root.FindWidgetByID("Windowed")).Action = () => { EventManager.Instance.QueueEvent(new WindowedSettingsButtonPressed()); };
            ((Button)_root.FindWidgetByID("BorderlessWindow")).Action = () => { EventManager.Instance.QueueEvent(new BorderlessWindowButtonPressedEvent()); };

            if(CVars.Get<bool>("ui_mouse_mode"))
            {
                UpdateButtonBindingsForKeyboard();
            } else
            {
                UpdateButtonBindingsForGamePad((PlayerIndex)CVars.Get<int>("ui_gamepad_mode_current_operator"));
            }

            ProcessManager.Attach(new EntityBackgroundSpawner(SharedState.Engine, SharedState.Camera));

            base.OnInitialize();
        }

        private void LoadContent()
        {
            _keyTextureMap = new KeyTextureMap();
            _keyTextureMap.CacheAll(Content);
            _gamePadTextureMap = new GamePadTextureMap();
            _gamePadTextureMap.CacheAll(Content);

            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_options_menu"));

            // Update all values from CVars
            UpdateWidgetsFromCVars();
        }

        private void UpdateWidgetsFromCVars()
        {
            float speed = CVars.Get<float>("game_speed");
            switch(speed)
            {
                case 0:
                    ((Label)_root.FindWidgetByID("Speed_Dropdown_Label")).Content = "Speed: 0.5x";
                    break;
                case 1:
                    ((Label)_root.FindWidgetByID("Speed_Dropdown_Label")).Content = "Speed: 1.0x";
                    break;
                case 2:
                    ((Label)_root.FindWidgetByID("Speed_Dropdown_Label")).Content = "Speed: 2.0x";
                    break;
            }

            float dif = CVars.Get<int>("game_difficulty");
            switch(dif)
            {
                case 0:
                    ((Label)_root.FindWidgetByID("Difficulty_Dropdown_Label")).Content = "Difficutly: Easy";
                    break;
                case 1:
                    ((Label)_root.FindWidgetByID("Difficulty_Dropdown_Label")).Content = "Difficutly: Normal";
                    break;
                case 2:
                    ((Label)_root.FindWidgetByID("Difficulty_Dropdown_Label")).Content = "Difficutly: Hard";
                    break;
            }

            bool fxaa = CVars.Get<bool>("graphics_fxaa");
            bool feather = CVars.Get<bool>("graphics_feathering");

            if(fxaa && !feather)
                ((Label)_root.FindWidgetByID("AA_Settings_Dropdown_Label")).Content = "Anti-Alias: FXAA";
            else if(!fxaa && feather)
                ((Label)_root.FindWidgetByID("AA_Settings_Dropdown_Label")).Content = "Anti-Alias: Feathering";
            else
                ((Label)_root.FindWidgetByID("AA_Settings_Dropdown_Label")).Content = "Anti-Alias: Off";

            bool full = CVars.Get<bool>("display_fullscreen");
            bool window = CVars.Get<bool>("display_windowed");

            if(full)
                ((Label)_root.FindWidgetByID("Screen_Size_Settings_Dropdown_Label")).Content = "Full Screen";
            else if(window)
                ((Label)_root.FindWidgetByID("Screen_Size_Settings_Dropdown_Label")).Content = "Windowed";
            else
                ((Label)_root.FindWidgetByID("Screen_Size_Settings_Dropdown_Label")).Content = "Borderless Window";

            int width = CVars.Get<int>("display_fullscreen_width");
            int height = CVars.Get<int>("display_fullscreen_height");

            ((Label)_root.FindWidgetByID("Resolution_Button_Label")).Content = width + " X " + height;

            ((Label)_root.FindWidgetByID("VSync_Button_Label")).Content =
                (CVars.Get<bool>("display_vsync") == true) ?
                "V-Sync: On" :
                "V-Sync: Off";
        }

        protected override void RegisterListeners()
        {
            _root.RegisterListeners(); // Root must be registered first because of "B" button event consumption

            EventManager.Instance.RegisterListener<EnterMouseUIModeEvent>(this);
            EventManager.Instance.RegisterListener<EnterGamePadUIModeEvent>(this);
            EventManager.Instance.RegisterListener<GamePadUIModeOperatorChangedEvent>(this);

            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);

            EventManager.Instance.RegisterListener<DisplaySettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<ControlsSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<GameSettingsButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<FullScreenSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<WindowedSettingsButtonPressed>(this);
            EventManager.Instance.RegisterListener<BorderlessWindowButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<ResolutionButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<VSyncButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<AASMAASettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<AAFeatheringButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<AAOffButtonPressedEvent>(this);

            EventManager.Instance.RegisterListener<RotateLeftSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<RotateRightSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<SecondaryRotateLeftSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<SecondaryRotateRightSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<SuperShieldSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<SecondarySuperShieldSettingsButtonPressedEvent>(this);

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
            _root.Render(_spriteBatch, _fieldFontRenderer);

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            base.OnKill();
        }

        private void UpdateButtonBindingsForGamePad(PlayerIndex controllerIndex)
        {
            ((Image)_root.FindWidgetByID("primary_rotate_counter_clockwise_button_texture")).Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                .GetRegion(_gamePadTextureMap[(Buttons)CVars.Get<int>(string.Format("controller_{0}_rotate_counter_clockwise", (int)controllerIndex))]);
            ((Image)_root.FindWidgetByID("primary_clockwise_button_texture")).Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                .GetRegion(_gamePadTextureMap[(Buttons)CVars.Get<int>(string.Format("controller_{0}_rotate_clockwise", (int)controllerIndex))]);

            _root.FindWidgetByID("secondary_rotate_counter_clockwise").Hidden = true;
            _root.FindWidgetByID("secondary_rotate_clockwise").Hidden = true;
        }

        private void UpdateButtonBindingsForKeyboard()
        {
            ((Image)_root.FindWidgetByID("primary_counter_clockwise_button_texture")).Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                .GetRegion(_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_primary_rotate_counter_clockwise")]);
            ((Image)_root.FindWidgetByID("primary_clockwise_button_texture")).Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                .GetRegion(_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_primary_rotate_clockwise")]);
            ((Image)_root.FindWidgetByID("primary_super_shield_button_texture")).Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                .GetRegion(_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_primary_super_shield")]);

            ((Image)_root.FindWidgetByID("secondary_counter_clockwise_button_texture")).Texture = Content.Load<TextureAtlas>("complete_texture_atlas").
                GetRegion(_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_secondary_rotate_counter_clockwise")]);
            ((Image)_root.FindWidgetByID("secondary_clockwise_button_texture")).Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                .GetRegion(_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_secondary_rotate_clockwise")]);
            ((Image)_root.FindWidgetByID("secondary_super_shield_button_texture")).Texture = Content.Load<TextureAtlas>("complete_texture_atlas")
                .GetRegion(_keyTextureMap[(Keys)CVars.Get<int>("input_keyboard_secondary_super_shield")]);

            _root.FindWidgetByID("secondary_rotate_counter_clockwise").Hidden = false;
            _root.FindWidgetByID("secondary_rotate_clockwise").Hidden = false;
        }
    }
}
