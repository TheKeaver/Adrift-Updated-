using Audrey;
using Events;
using GameJam.Common;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.TextureAtlases;
using System;

namespace GameJam.NUI.Widgets
{
    public class ButtonWidget : ContainerWidget, IEventListener
    {
        public enum ButtonWidgetState
        {
            Released,
            Hover,
            Pressed
        }

        NinePatchImageWidget _releasedImage;
        NinePatchImageWidget _hoverImage;
        NinePatchImageWidget _pressedImage;

        public WidgetProperty<NinePatchRegion2D> ReleasedImage
        {
            get
            {
                return Properties.GetProperty<NinePatchRegion2D>("released-image");
            }
            set
            {
                Properties.SetProperty("released-image", value);
            }
        }
        public WidgetProperty<NinePatchRegion2D> HoverImage
        {
            get
            {
                return Properties.GetProperty<NinePatchRegion2D>("hover-image");
            }
            set
            {
                Properties.SetProperty("hover-image", value);
            }
        }
        public WidgetProperty<NinePatchRegion2D> PressedImage
        {
            get
            {
                return Properties.GetProperty<NinePatchRegion2D>("pressed-image");
            }
            set
            {
                Properties.SetProperty("pressed-image", value);
            }
        }

        public ButtonWidgetState State
        {
            get;
            private set;
        } = ButtonWidgetState.Released;

        public Action Action = null;

        public ButtonWidget(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
            _releasedImage = new NinePatchImageWidget(Engine);
            _releasedImage.Width = new RelativeValue<float>(this, "width", 1.0f);
            _releasedImage.Height = new RelativeValue<float>(this, "height", 1.0f);
            Add(_releasedImage);
            _hoverImage = new NinePatchImageWidget(Engine);
            _hoverImage.Width = new RelativeValue<float>(this, "width", 1.0f);
            _hoverImage.Height = new RelativeValue<float>(this, "height", 1.0f);
            Add(_hoverImage);
            _pressedImage = new NinePatchImageWidget(Engine);
            _pressedImage.Width = new RelativeValue<float>(this, "width", 1.0f);
            _pressedImage.Height = new RelativeValue<float>(this, "height", 1.0f);
            Add(_pressedImage);

            Properties.SetPropertyAsChildPassThrough<NinePatchRegion2D>("released-image", _releasedImage, "image");
            Properties.SetPropertyAsChildPassThrough<NinePatchRegion2D>("hover-image", _hoverImage, "image");
            Properties.SetPropertyAsChildPassThrough<NinePatchRegion2D>("pressed-image", _pressedImage, "image");

            EventManager.Instance.RegisterListener<MouseMoveEvent>(this);
            EventManager.Instance.RegisterListener<MouseButtonEvent>(this);

            base.Initialize(entity);

            UpdateBasedOnState();
        }

        protected override void OnComputeProperties(Entity entity)
        {
            UpdateBasedOnState();

            base.OnComputeProperties(entity);
        }

        private void UpdateBasedOnState()
        {
            _releasedImage.Hidden = new FixedValue<bool>(true);
            _hoverImage.Hidden = new FixedValue<bool>(true);
            _pressedImage.Hidden = new FixedValue<bool>(true);

            switch (State)
            {
                case ButtonWidgetState.Pressed:
                    _pressedImage.Hidden = new FixedValue<bool>(false);
                    break;
                case ButtonWidgetState.Hover:
                    _hoverImage.Hidden = new FixedValue<bool>(false);
                    break;
                case ButtonWidgetState.Released:
                default:
                    _releasedImage.Hidden = new FixedValue<bool>(false);
                    break;
            }
        }

        public bool Handle(IEvent evt)
        {
            MouseMoveEvent mouseMoveEvent = evt as MouseMoveEvent;
            if(mouseMoveEvent != null)
            {
                return HandleMouseMoveEvent(mouseMoveEvent);
            }

            MouseButtonEvent mouseButtonEvent = evt as MouseButtonEvent;
            if(mouseButtonEvent != null)
            {
                return HandleMouseButtonEvent(mouseButtonEvent);
            }

            return false;
        }

        private bool HandleMouseMoveEvent(MouseMoveEvent mouseMoveEvent)
        {
            BoundingRect buttonRect = new BoundingRect(BottomLeft, TopRight);
            if (buttonRect.Contains(mouseMoveEvent.CurrentPosition))
            {
                if (State == ButtonWidgetState.Released)
                {
                    State = ButtonWidgetState.Hover;
                }
            } else
            {
                if(State != ButtonWidgetState.Released)
                {
                    State = ButtonWidgetState.Released;
                }
            }

            UpdateBasedOnState();

            return false;
        }

        private bool HandleMouseButtonEvent(MouseButtonEvent mouseButtonEvent)
        {
            if(State == ButtonWidgetState.Hover)
            {
                if(mouseButtonEvent.LeftButtonState == ButtonState.Pressed)
                {
                    State = ButtonWidgetState.Pressed;
                }
            } else if(State == ButtonWidgetState.Pressed)
            {
                if (mouseButtonEvent.LeftButtonState == ButtonState.Released)
                {
                    State = ButtonWidgetState.Hover;

                    Action?.Invoke();

                    UpdateBasedOnState();
                    return true;
                }
            }

            UpdateBasedOnState();

            return false;
        }
    }
}
