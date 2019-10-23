using System;
using Events;
using GameJam.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using GameJam.Events.InputHandling;

namespace GameJam.UI.Widgets
{
    public enum ButtonState
    {
        Released,
        Hover,
        Pressed
    }

    public class Button : Widget, IParentWidget, ISelectableWidget
    {
        public Action Action;

        private NinePatchRegion2D _releasedNinePatch;
        private NinePatchRegion2D _hoverNinePatch;
        private NinePatchRegion2D _pressedNinePatch;
        public NinePatchRegion2D ReleasedNinePatch
        {
            get
            {
                return _releasedNinePatch;
            }
            set
            {
                _releasedNinePatch = value;

                ComputeProperties();
            }
        }
        public NinePatchRegion2D HoverNinePatch
        {
            get
            {
                return _hoverNinePatch;
            }
            set
            {
                _hoverNinePatch = value;

                ComputeProperties();
            }
        }
        public NinePatchRegion2D PressedNinePatch 
        {
            get
            {
                return _pressedNinePatch;
            }
            set
            {
                _pressedNinePatch = value;

                ComputeProperties();
            }
        }

        public Panel SubPanel
        {
            get;
            private set;
        }

        public ButtonState ButtonState
        {
            get;
            private set;
        } = ButtonState.Released;

        public string aboveID { get; set; } = "";
        public string leftID { get; set; } = "";
        public string rightID { get; set; } = "";
        public string belowID { get; set; } = "";
        public bool isSelected { get; set; } = false;

        public Button(NinePatchRegion2D releasedNinePatch,
            NinePatchRegion2D hoverNinePatch,
            NinePatchRegion2D pressedNinePatch,

            HorizontalAlignment hAlign,
            AbstractValue horizontal,

            VerticalAlignment vAlign,
            AbstractValue vertical,

            AbstractValue width,
            AbstractValue height) : base(hAlign, horizontal, vAlign, vertical, width, height)
        {
            _releasedNinePatch = releasedNinePatch;
            _hoverNinePatch = hoverNinePatch;
            _pressedNinePatch = pressedNinePatch;

            SubPanel = new Panel(HorizontalAlignment.Center, new FixedValue(0),
                VerticalAlignment.Center, new FixedValue(0),
                new RelativeValue(1, () => { return Width; }),
                new RelativeValue(1, () => { return Height; }));

            SubPanel.Parent = this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Hidden)
            {
                NinePatchRegion2D ninePatch = _releasedNinePatch;
                if(ButtonState == ButtonState.Hover && Root.mouseMode == true)
                {
                    ninePatch = _hoverNinePatch;
                }
                if (this.isSelected == true && Root.mouseMode == false)
                {
                    ninePatch = _hoverNinePatch;
                }
                if(ButtonState == ButtonState.Pressed)
                {
                    ninePatch = _pressedNinePatch;
                }

                spriteBatch.Draw(ninePatch,
                    new Rectangle((int)TopLeft.X,
                        (int)TopLeft.Y,
                        (int)Width,
                        (int)Height),
                        Color.White);
                SubPanel.Draw(spriteBatch);
            }
        }

        public override bool Handle(IEvent evt)
        {
            MouseMoveEvent mouseMoveEvent = evt as MouseMoveEvent;
            if(mouseMoveEvent != null)
            {
                //this.isSelected = false;
                if (mouseMoveEvent.CurrentPosition.X > TopLeft.X
                    && mouseMoveEvent.CurrentPosition.X < BottomRight.X
                    && mouseMoveEvent.CurrentPosition.Y > TopLeft.Y
                    && mouseMoveEvent.CurrentPosition.Y < BottomRight.Y)
                {
                    if (ButtonState != ButtonState.Pressed)
                    {
                        ButtonState = ButtonState.Hover;
                    }
                }
                else
                {
                    ButtonState = ButtonState.Released;
                }
            }

            MouseButtonEvent mouseButtonEvent = evt as MouseButtonEvent;
            if (mouseButtonEvent != null)
            {
                if (mouseButtonEvent.CurrentPosition.X > TopLeft.X
                    && mouseButtonEvent.CurrentPosition.X < BottomRight.X
                    && mouseButtonEvent.CurrentPosition.Y > TopLeft.Y
                    && mouseButtonEvent.CurrentPosition.Y < BottomRight.Y)
                {
                    if (mouseButtonEvent.LeftButtonState == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        ButtonState = ButtonState.Pressed;
                    }
                    if (ButtonState == ButtonState.Pressed
                        && mouseButtonEvent.LeftButtonState == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        if (Action != null && !Hidden)
                        {
                            Action.Invoke();
                        }

                        ButtonState = ButtonState.Released;
                    }
                }
            }

            GamePadButtonDownEvent gamePadButtonDownEvent = evt as GamePadButtonDownEvent;
            if(gamePadButtonDownEvent != null && this.isSelected)
            {
                if(gamePadButtonDownEvent._pressedButton == Buttons.A)
                {
                    Action.Invoke();
                }
                // This is likely bad news
                if(gamePadButtonDownEvent._pressedButton == Buttons.B)
                {
                    this.isSelected = false;
                }
                if(gamePadButtonDownEvent._pressedButton == Buttons.DPadLeft ||
                   gamePadButtonDownEvent._pressedButton == Buttons.LeftThumbstickLeft )
                {
                    if(leftID.Length > 0)
                    {
                        this.isSelected = false;
                        ((ISelectableWidget)Root.FindWidgetByID(leftID)).isSelected = true;
                        return true;
                    }
                }
                if (gamePadButtonDownEvent._pressedButton == Buttons.DPadRight ||
                    gamePadButtonDownEvent._pressedButton == Buttons.LeftThumbstickRight )
                {
                    if (rightID.Length > 0)
                    {
                        this.isSelected = false;
                        ((ISelectableWidget)Root.FindWidgetByID(rightID)).isSelected = true;
                        return true;
                    }
                }
                if (gamePadButtonDownEvent._pressedButton == Buttons.DPadUp ||
                    gamePadButtonDownEvent._pressedButton == Buttons.LeftThumbstickUp )
                {
                    if (aboveID.Length > 0)
                    {
                        this.isSelected = false;
                        ((ISelectableWidget)Root.FindWidgetByID(aboveID)).isSelected = true;
                        return true;
                    }
                }
                if (gamePadButtonDownEvent._pressedButton == Buttons.DPadDown ||
                    gamePadButtonDownEvent._pressedButton == Buttons.LeftThumbstickDown )
                {
                    if (belowID.Length > 0)
                    {
                        this.isSelected = false;
                        ((ISelectableWidget)Root.FindWidgetByID(belowID)).isSelected = true;
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void OnComputeProperties()
        {
            SubPanel.ComputeProperties();
        }

        public void Add(Widget widget)
        {
            SubPanel.Add(widget);
            ComputeProperties();
        }

        public void Remove(Widget widget)
        {
            SubPanel.Remove(widget);
            ComputeProperties();
        }
    }
}
