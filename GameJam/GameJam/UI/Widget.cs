using System;
using System.Collections.Generic;
using Events;
using GameJam.Graphics.Text;
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
        public Root Root
        {
            get
            {
                if (Parent == null)
                    return (Root)this;
                else
                    return Parent.Root;
            }
        }

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
                        return Parent.Width;
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
                        return Parent.Height;
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

        private RelativeValue _alpha;
        public float Alpha
        {
            get
            {
                return _alpha.Percentage;
            }
            set
            {
                _alpha.Percentage = value;
                ComputeProperties();
            }
        }
        public float AbsoluteAlpha
        {
            get
            {
                return _alpha.Value;
            }
        }
        public Color TintColor
        {
            get;
            private set;
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

        public List<string> Classes
        {
            get;
            private set;
        } = new List<string>();

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
                    return Parent.Width;
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
                    return Parent.Height;
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
            _alpha = new RelativeValue(1, () =>
            {
                if(Parent == null)
                {
                    return 1;
                }
                return Parent.AbsoluteAlpha;
            });
        }

        public abstract void Draw(SpriteBatch spriteBatch, FieldFontRenderer fieldFontRenderer);

        public void ComputeProperties()
        {
            if(Parent == null)
            {
                // Root; only sets based on offset width and height
                TopLeft = Vector2.Zero;
                BottomRight = new Vector2(Width, Height);
                TintColor = Color.White;
            } else
            {
                float x = Parent.TopLeft.X;
                switch(HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        x += Horizontal;
                        break;
                    case HorizontalAlignment.Right:
                        x += Parent.Width - Horizontal - Width;
                        break;
                    case HorizontalAlignment.Center:
                        x += Parent.Width / 2 - Width / 2 + Horizontal;
                        break;
                }

                float y = Parent.TopLeft.Y;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        y += Vertical;
                        break;
                    case VerticalAlignment.Bottom:
                        y += Parent.Height - Vertical - Height;
                        break;
                    case VerticalAlignment.Center:
                        y += Parent.Height / 2 - Height / 2 + Vertical;
                        break;
                }

                TopLeft = new Vector2(x, y);
                BottomRight = new Vector2(x + Width, y + Height);
                TintColor = Color.White * AbsoluteAlpha;
            }

            OnComputeProperties();
        }

        protected virtual void OnComputeProperties()
        {
        }

        public abstract bool Handle(IEvent evt);
    }
}
