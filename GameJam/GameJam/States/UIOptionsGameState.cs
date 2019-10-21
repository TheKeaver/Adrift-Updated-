using System;
using System.Collections.Generic;
using Events;
using GameJam.Events.InputHandling;
using GameJam.Events.UI;
using GameJam.Input;
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
            EventManager.Instance.RegisterListener<DifficultySettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<SpeedSettingsButtonPressedEvent>(this);
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
                ((Button)_root.FindWidgetByID("Display")).isSelected = false;
                // Default selected of right side .Hidden = false;
                // Default selected of right side .isSelected = true;
            }
            ControlsSettingsButtonPressedEvent controlsSBPE = evt as ControlsSettingsButtonPressedEvent;
            if (controlsSBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 1;
                ((Button)_root.FindWidgetByID("Controls")).isSelected = false;
                // Default selected of right side .Hidden = false;
                // Default selected of right side .isSelected = true;
            }
            DifficultySettingsButtonPressedEvent difficultySBPE = evt as DifficultySettingsButtonPressedEvent;
            if (difficultySBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 2;
                ((Button)_root.FindWidgetByID("Difficulty")).isSelected = false;
                // Default selected of right side .Hidden = false;
                // Default selected of right side .isSelected = true;
            }
            SpeedSettingsButtonPressedEvent speedSBPE = evt as SpeedSettingsButtonPressedEvent;
            if (speedSBPE != null)
            {
                isOnLeftSide = false;
                leftSideIndex = 3;
                ((Button)_root.FindWidgetByID("Speed")).isSelected = false;
                // Default selected of right side .Hidden = false;
                // Default selected of right side .isSelected = true;
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
                                // Right side .Hidden = true;
                                break;
                            case 1:
                                ((Button)_root.FindWidgetByID("Controls")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                // Right side .Hidden = true;
                                break;
                            case 2:
                                ((Button)_root.FindWidgetByID("Difficulty")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                // Right side .Hidden = true;
                                break;
                            case 3:
                                ((Button)_root.FindWidgetByID("Speed")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                // Right side .Hidden = true;
                                break;
                        }
                    }
                    if ( isOnLeftSide == true )
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
                    if (isOnLeftSide == false)
                    {
                        switch (leftSideIndex)
                        {
                            case 0:
                                ((Button)_root.FindWidgetByID("Display")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                // Right side .Hidden = true;
                                break;
                            case 1:
                                ((Button)_root.FindWidgetByID("Controls")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                // Right side .Hidden = true;
                                break;
                            case 2:
                                ((Button)_root.FindWidgetByID("Difficulty")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                // Right side .Hidden = true;
                                break;
                            case 3:
                                ((Button)_root.FindWidgetByID("Speed")).isSelected = true;
                                isOnLeftSide = true;
                                // Deslection of button/slider should be handled in the button/slider class
                                // Right side .Hidden = true;
                                break;
                        }
                    }
                    if (isOnLeftSide == true)
                    {
                        GameManager.ChangeState(new UIMenuGameState(GameManager));
                    }
                }
            }
            return false;
        }
    }
}
