using System;
using Events;
using GameJam.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.UI.Widgets
{
    public enum ButtonState
    {
        Released,
        Hover,
        Pressed
    }

    public class Button : Widget, IParentWidget
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
                new FixedValue(100), // TODO: Width and Height need to be percentage-based
                new FixedValue(100));
            SubPanel.Parent = this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Hidden)
            {
                NinePatchRegion2D ninePatch = _releasedNinePatch;
                if(ButtonState == ButtonState.Hover)
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
