using System.Collections.Generic;
using Events;
using Microsoft.Xna.Framework.Graphics;
using GameJam.Events;

namespace GameJam.UI
{
    /// <summary>
    /// UI root.
    /// </summary>
    public class Root : Widget, IEventListener
    {
        List<Widget> _widgets = new List<Widget>();

        public Root(float width, float height)
            : base(Origin.TopLeft, 0, 0, 0, 0, 0, 0, 0, (float)0)
        {
            OnResize(width, height);
        }

        public void RegisterListeners()
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);

            EventManager.Instance.RegisterListener<MouseMoveEvent>(this);
            EventManager.Instance.RegisterListener<MouseButtonEvent>(this);
        }

        public void UnregisterListeners()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public void Add(Widget widget)
        {
            _widgets.Add(widget);
            widget.Parent = this;
        }

        public void Remove(Widget widget)
        {
            _widgets.Remove(widget);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                for (int i = 0; i < _widgets.Count; i++)
                {
                    _widgets[i].Draw(spriteBatch);
                }
            }
        }

        void OnResize(float width, float height)
        {
            POffsetWidth = width;
            POffsetHeight = height;

            ComputeProperties();
        }

        public override bool Handle(IEvent evt)
        {
            for (int i = 0; i < _widgets.Count; i++)
            {
                if (_widgets[i].Handle(evt))
                {
                    return true;
                }
            }

            ResizeEvent resizeEvent = evt as ResizeEvent;
            if (resizeEvent != null)
            {
                OnResize(resizeEvent.Width, resizeEvent.Height);
            }

            return false;
        }

        protected override void OnComputeProperties()
        {
            for (int i = 0; i < _widgets.Count; i++)
            {
                _widgets[i].ComputeProperties();
            }
        }
    }
}
