﻿using System;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.UI
{
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }
    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    }

    public interface AbstractValue
    {
        float Value
        {
            get;
        }
    }
    public class FixedValue : AbstractValue
    {
        public float Value
        {
            get;
            private set;
        }

        public FixedValue(float value)
        {
            Value = value;
        }
    }
    public class RelativeValue : AbstractValue
    {
        public float Percentage
        {
            get;
            set;
        }

        public float Value
        {
            get
            {
                return Percentage * GetBaseValueFn();
            }
        }

        public Func<float> GetBaseValueFn = null;

        public RelativeValue(float percentage, Func<float> getBaseValueFn)
        {
            Percentage = percentage;
            GetBaseValueFn = getBaseValueFn;
        }
    }

    public abstract class Widget : IEventListener
    {
        WeakReference<Widget> _parent = new WeakReference<Widget>(null);
        public Widget Parent
        {
            get
            {
                Widget parent;
                if (_parent.TryGetTarget(out parent))
                {
                    return parent;
                }
                return null;
            }
            internal set
            {
                _parent = new WeakReference<Widget>(value);
                ComputeProperties();
            }
        }

        public bool Hidden = false;

        private HorizontalAlignment _hAlign = HorizontalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return _hAlign;
            }
            set
            {
                _hAlign = value;
                ComputeProperties();
            }
        }
        private AbstractValue _horizontal;
        public float Horizontal
        {
            get
            {
                return _horizontal.Value;
            }
        }
        public AbstractValue HorizontalValue
        {
            set
            {
                _horizontal = value;
                if(_horizontal is RelativeValue)
                {
                    ((RelativeValue)_horizontal).GetBaseValueFn = () =>
                    {
                        if(Parent == null)
                        {
                            return 0;
                        }
                        return Parent.Horizontal;
                    };
                }
                ComputeProperties();
            }
        }
        private VerticalAlignment _vAlign = VerticalAlignment.Center;
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return _vAlign;
            }
            set
            {
                _vAlign = value;
                ComputeProperties();
            }
        }
        private AbstractValue _vertical;
        public float Vertical
        {
            get
            {
                return _vertical.Value;
            }
        }
        public AbstractValue VerticalValue
        {
            set
            {
                _vertical = value;
                if (_vertical is RelativeValue)
                {
                    ((RelativeValue)_vertical).GetBaseValueFn = () =>
                    {
                        if (Parent == null)
                        {
                            return 0;
                        }
                        return Parent.Vertical;
                    };
                }
                ComputeProperties();
            }
        }

        private bool _maintainAspectRatio = false;
        public virtual bool MaintainAspectRatio
        {
            get
            {
                return _maintainAspectRatio;
            }
            set
            {
                _maintainAspectRatio = value;
                ComputeProperties();
            }
        }
        private float _aspectRatio;
        public virtual float AspectRatio
        {
            get
            {
                return _aspectRatio;
            }
            set
            {
                _aspectRatio = value;
                if (MaintainAspectRatio)
                {
                    ComputeProperties();
                }
            }
        }

        private AbstractValue _width;
        public float Width
        {
            get
            {
                float width = _width.Value;
                if (MaintainAspectRatio)
                {
                    float freeAspectRatio = _width.Value / _height.Value;

                    if (freeAspectRatio > AspectRatio) // Height is dominant
                    {
                        width = _height.Value * AspectRatio;
                    }
                }

                return width;
            }
        }
        public AbstractValue WidthValue
        {
            set
            {
                _width = value;
                if (_horizontal is RelativeValue)
                {
                    ((RelativeValue)_width).GetBaseValueFn = () =>
                    {
                        if (Parent == null)
                        {
                            return 0;
                        }
                        return Parent.Width;
                    };
                }
                ComputeProperties();
            }
        }
        private AbstractValue _height;
        public float Height
        {
            get
            {
                float height = _height.Value;
                if (MaintainAspectRatio)
                {
                    float freeAspectRatio = _width.Value / _height.Value;

                    if (freeAspectRatio < AspectRatio) // Width is dominant
                    {
                        height = _width.Value / AspectRatio;
                    }
                }

                return height;
            }
        }
        public AbstractValue HeightValue
        {
            set
            {
                _height = value;
                if (_height is RelativeValue)
                {
                    ((RelativeValue)_height).GetBaseValueFn = () =>
                    {
                        if (Parent == null)
                        {
                            return 0;
                        }
                        return Parent.Height;
                    };
                }
                ComputeProperties();
            }
        }

        public Vector2 TopLeft
        {
            get;
            internal set;
        }
        public Vector2 BottomRight
        {
            get;
            internal set;
        }

        public Widget(
            HorizontalAlignment hAlign,
            AbstractValue horizontal,

            VerticalAlignment vAlign,
            AbstractValue vertical,

            AbstractValue width,
            AbstractValue height)
        {
            _hAlign = hAlign;
            _horizontal = horizontal;
            _vAlign = vAlign;
            _vertical = vertical;
            _width = width;
            _height = height;

            if (_horizontal is RelativeValue)
            {
                ((RelativeValue)_horizontal).GetBaseValueFn = () =>
                {
                    if (Parent == null)
                    {
                        return 0;
                    }
                    return Parent.Horizontal;
                };
            }
            if (_vertical is RelativeValue)
            {
                ((RelativeValue)_vertical).GetBaseValueFn = () =>
                {
                    if (Parent == null)
                    {
                        return 0;
                    }
                    return Parent.Vertical;
                };
            }
            if (_width is RelativeValue)
            {
                ((RelativeValue)_width).GetBaseValueFn = () =>
                {
                    if (Parent == null)
                    {
                        return 0;
                    }
                    return Parent.Width;
                };
            }
            if (_height is RelativeValue)
            {
                ((RelativeValue)_height).GetBaseValueFn = () =>
                {
                    if (Parent == null)
                    {
                        return 0;
                    }
                    return Parent.Height;
                };
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public void ComputeProperties()
        {
            if(Parent == null)
            {
                // Root; only sets based on offset width and height
                TopLeft = Vector2.Zero;
                BottomRight = new Vector2(Width, Height);
            } else
            {
                float x = Parent.TopLeft.X;
                switch(HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        x += _horizontal.Value;
                        break;
                    case HorizontalAlignment.Right:
                        x += Parent.Width - _horizontal.Value - Width;
                        break;
                    case HorizontalAlignment.Center:
                        x += Parent.Width / 2 - Width / 2 + _horizontal.Value;
                        break;
                }

                float y = Parent.TopLeft.Y;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        y += _vertical.Value;
                        break;
                    case VerticalAlignment.Bottom:
                        y += Parent.Height - _vertical.Value - Height;
                        break;
                    case VerticalAlignment.Center:
                        y += Parent.Height / 2 - Height / 2 + _vertical.Value;
                        break;
                }

                TopLeft = new Vector2(x, y);
                BottomRight = new Vector2(x + Width, y + Height);
            }

            OnComputeProperties();
        }

        protected virtual void OnComputeProperties()
        {
        }

        public abstract bool Handle(IEvent evt);
    }
}
