using System.Collections.Generic;
using Audrey;

namespace GameJam.NUI.Widgets
{
    public class ContainerWidget : Widget
    {
        private List<Widget> _widgets = new List<Widget>();

        public ContainerWidget(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
        }

        protected override void OnComputeProperties(Entity entity)
        {
            foreach(Widget widget in _widgets)
            {
                widget.ComputeProperties();
            }
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
