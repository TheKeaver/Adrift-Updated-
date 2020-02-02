using System.Collections.Generic;
using Audrey;
using Events;
using GameJam.Events;

namespace GameJam.NUI
{
    public class Root : Widget, IEventListener
    {
        private List<Widget> _widgets = new List<Widget>();

        public Root(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);
        }

        protected override void OnComputeProperties(Entity entity)
        {
            foreach(Widget widget in _widgets)
            {
                widget.ComputeProperties();
            }
        }

        public bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvent = evt as ResizeEvent;
            if(resizeEvent != null)
            {
                OnResize(resizeEvent.Width, resizeEvent.Height);
            }

            return false;
        }

        private void OnResize(float width, float height)
        {
            Width = new FixedValue<float>(width);
            Height = new FixedValue<float>(height);
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
    }
}
