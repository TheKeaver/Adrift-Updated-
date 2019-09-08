using System;
using System.Collections.Generic;
using Events;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.UINew.Widgets
{
    public class Panel : Widget, IParentWidget
    {
        List<Widget> _widgets = new List<Widget>();

        public Panel(HorizontalAlignment hAlign, AbstractValue horizontal, VerticalAlignment vAlign, AbstractValue vertical, AbstractValue width, AbstractValue height) : base(hAlign, horizontal, vAlign, vertical, width, height)
        {
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

        public override bool Handle(IEvent evt)
        {
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
