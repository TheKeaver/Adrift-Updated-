using System;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using GameJam.Events.InputHandling;
using GameJam.Common;

namespace GameJam.UI.Widgets
{
    public enum SliderState
    {
        Released,
        Hover,
        Pressed
    }

    public class Slider : Widget, ISelectableWidget
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

        /*public Panel SubPanel
        {
            get;
            private set;
        }*/

        public Button SliderButton
        {
            get;
            private set;
        }

        public SliderState SliderState
        {
            get;
            private set;
        } = SliderState.Released;

        public string aboveID { get; set; } = "";
        public string leftID { get; set; } = "";
        public string rightID { get; set; } = "";
        public string belowID { get; set; } = "";

        public int horizontalValue { get; set; }
        public int verticalValue { get; set; }

        public int divisions;

        public bool isVertical = false;
        public bool isHorizontal = false;
        public bool isSelected { get; set; } = false;
        public bool isDragging { get; set; } = false;

        public Slider(NinePatchRegion2D releasedNinePatch,
            NinePatchRegion2D hoverNinePatch,
            NinePatchRegion2D pressedNinePatch,

            HorizontalAlignment hAlign,
            AbstractValue horizontal,

            VerticalAlignment vAlign,
            AbstractValue vertical,

            AbstractValue width,
            AbstractValue height,
            bool isVertical,
            bool isHorizontal,
            int divisions) : base(hAlign, horizontal, vAlign, vertical, width, height)
        {
            _releasedNinePatch = releasedNinePatch;
            _hoverNinePatch = hoverNinePatch;
            _pressedNinePatch = pressedNinePatch;

            this.isVertical = isVertical;
            this.isHorizontal = isHorizontal;
            this.divisions = divisions;

            SliderButton = new Button(_releasedNinePatch,
                _hoverNinePatch,
                _pressedNinePatch,
                HorizontalAlignment.Center, new FixedValue(0),
                VerticalAlignment.Center, new FixedValue(0),
                new RelativeValue(0.2f, () => { return Width; }),
                new RelativeValue(0.3f, () => { return Height; }));

            //SubPanel.Parent = this;
            SliderButton.Parent = this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                NinePatchRegion2D ninePatch = _releasedNinePatch;
                if (SliderState == SliderState.Hover && Root.MouseMode == true)
                {
                    ninePatch = _hoverNinePatch;
                }
                if (this.isSelected == true && Root.MouseMode == false)
                {
                    ninePatch = _hoverNinePatch;
                }
                if (SliderState == SliderState.Pressed)
                {
                    ninePatch = _pressedNinePatch;
                }

                spriteBatch.Draw(ninePatch,
                    new Rectangle((int)TopLeft.X,
                    (int)(TopLeft.Y + (Height/2) - Height/20),
                    (int)Width,
                    (int)Height/10),
                    Color.Green);

                SliderButton.Draw(spriteBatch);
            }
        }

        public override bool Handle(IEvent evt)
        {
            SliderButton.Handle(evt);

            float distanceToCenterOfButtonX = ((BottomRight.X - TopLeft.X) / 2 + TopLeft.X);
            float distanceToCenterOfButtonY = ((BottomRight.Y - TopLeft.Y) / 2 + TopLeft.Y);

            MouseButtonEvent mouseButtonEvent = evt as MouseButtonEvent;
            if (mouseButtonEvent != null)
            {
                if (mouseButtonEvent.LeftButtonState == Microsoft.Xna.Framework.Input.ButtonState.Pressed && SliderButton.ButtonState == ButtonState.Pressed)
                    isDragging = true;
                if (mouseButtonEvent.LeftButtonState == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    isDragging = false;
            }

            MouseMoveEvent mouseMoveEvent = evt as MouseMoveEvent;
            if (mouseMoveEvent != null)
            {
                if (isDragging == true || SliderButton.ButtonState == ButtonState.Pressed)
                {
                    if (isHorizontal)
                    {
                        int beta = ((int)Math.Round(MathHelper.Clamp(MathUtils.InverseLerp(TopLeft.X, BottomRight.X, mouseMoveEvent.CurrentPosition.X) * divisions, 0, divisions)));
                        float posX = MathHelper.Lerp(TopLeft.X, BottomRight.X, (float)beta/divisions);
                        SliderButton.HorizontalValue = new FixedValue( posX - (BottomRight.X - TopLeft.X) / 2 - TopLeft.X);
                    }

                    if (isVertical)
                    {
                        int beta = ((int)Math.Round(MathHelper.Clamp(MathUtils.InverseLerp(TopLeft.Y, BottomRight.Y, mouseMoveEvent.CurrentPosition.Y) * divisions, 0, divisions)));
                        float posY = MathHelper.Lerp(TopLeft.Y, BottomRight.Y, (float)beta/divisions);
                        SliderButton.HorizontalValue = new FixedValue(posY - (BottomRight.Y - TopLeft.Y) / 2 - TopLeft.Y);
                    }
                }
            }

            GamePadButtonDownEvent gamePadButtonDownEvent = evt as GamePadButtonDownEvent;
            if (gamePadButtonDownEvent != null && this.isSelected)
            {
                float oneUnitOfWidth = (BottomRight.X - TopLeft.X) / divisions;
                float oneUnitOfHeight = (BottomRight.Y - TopLeft.Y) / divisions;

                // This is likely bad news
                if (gamePadButtonDownEvent._pressedButton == Buttons.B)
                {
                    this.isSelected = false;
                    Root.MouseMode = true;
                }
                if (gamePadButtonDownEvent._pressedButton == Buttons.DPadLeft ||
                   gamePadButtonDownEvent._pressedButton == Buttons.LeftThumbstickLeft)
                {
                    if (!isHorizontal)
                    {
                        this.isSelected = false;
                        ((ISelectableWidget)Root.FindWidgetByID(leftID)).isSelected = true;
                    }
                    if (isHorizontal)
                    {
                        SliderButton.HorizontalValue = new FixedValue(
                            MathHelper.Clamp(SliderButton.Horizontal + distanceToCenterOfButtonX - oneUnitOfWidth, (float)TopLeft.X, (float)BottomRight.X) -
                            distanceToCenterOfButtonX
                            );
                    }
                }
                if (gamePadButtonDownEvent._pressedButton == Buttons.DPadRight ||
                    gamePadButtonDownEvent._pressedButton == Buttons.LeftThumbstickRight)
                {
                    if (!isHorizontal)
                    {
                        this.isSelected = false;
                        ((ISelectableWidget)Root.FindWidgetByID(rightID)).isSelected = true;
                    }
                    if (isHorizontal)
                    {
                        SliderButton.HorizontalValue = new FixedValue(
                            MathHelper.Clamp(SliderButton.Horizontal + distanceToCenterOfButtonX + oneUnitOfWidth, (float)TopLeft.X, (float)BottomRight.X) -
                            distanceToCenterOfButtonX
                            );
                    }
                }
                if (gamePadButtonDownEvent._pressedButton == Buttons.DPadUp ||
                    gamePadButtonDownEvent._pressedButton == Buttons.LeftThumbstickUp)
                {
                    if (!isVertical)
                    {
                        this.isSelected = false;
                        ((ISelectableWidget)Root.FindWidgetByID(aboveID)).isSelected = true;
                    }
                    if (isVertical)
                    {
                        SliderButton.HorizontalValue = new FixedValue(
                            MathHelper.Clamp(SliderButton.Vertical + distanceToCenterOfButtonY - oneUnitOfHeight, (float)BottomRight.Y, (float)TopLeft.Y) -
                            distanceToCenterOfButtonY
                            );
                    }
                }
                if (gamePadButtonDownEvent._pressedButton == Buttons.DPadDown ||
                    gamePadButtonDownEvent._pressedButton == Buttons.LeftThumbstickDown)
                {
                    if (!isVertical)
                    {
                        this.isSelected = false;
                        ((ISelectableWidget)Root.FindWidgetByID(belowID)).isSelected = true;
                    }
                    if (isVertical)
                    {
                        SliderButton.HorizontalValue = new FixedValue(
                            MathHelper.Clamp(SliderButton.Vertical + distanceToCenterOfButtonY + oneUnitOfHeight, (float)BottomRight.Y, (float)TopLeft.Y) -
                            distanceToCenterOfButtonY
                            );
                    }
                }
            }
            horizontalValue = ((int)Math.Round(MathHelper.Clamp(MathUtils.InverseLerp(TopLeft.X, BottomRight.X, (SliderButton.Horizontal + distanceToCenterOfButtonX)) * divisions, 0, divisions)));
            //Console.WriteLine(horizontalValue);

            verticalValue = ((int)Math.Round(MathHelper.Clamp(MathUtils.InverseLerp(TopLeft.Y, BottomRight.Y, (SliderButton.Vertical + distanceToCenterOfButtonY)) * divisions, 0, divisions)));
            //Console.WriteLine(verticalValue);

            return false;
        }

        protected override void OnComputeProperties()
        {
            SliderButton.ComputeProperties();
        }
    }
}
