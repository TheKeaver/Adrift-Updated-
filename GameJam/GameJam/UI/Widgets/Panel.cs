using System.Collections;
using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Events;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.UI.Widgets
{
    public class Panel : Widget, IParentWidget
    {
        List<Widget> _widgets = new List<Widget>();

        public Panel(HorizontalAlignment hAlign, AbstractValue horizontal,
            VerticalAlignment vAlign, AbstractValue vertical,
            AbstractValue width, AbstractValue height)
                :base(hAlign, horizontal, vAlign, vertical, width, height)
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

        public void BuildFromPrototypes(ContentManager content, List<WidgetPrototype> prototypes)
        {
            foreach (WidgetPrototype prototype in prototypes)
            {
                Add(WidgetFactory.CreateFromPrototype(content, prototype, Root));
            }
        }

        public override void Render(SpriteBatch spriteBatch, FieldFontRenderer fieldFontRenderer)
        {
            if (!Hidden)
            {
                for (int i = 0; i < _widgets.Count; i++)
                {
                    _widgets[i].Render(spriteBatch, fieldFontRenderer);
                }
            }
        }

        public override bool Handle(IEvent evt)
        {
            if(Hidden)
            {
                return false;
            }

            for (int i = 0; i < _widgets.Count; i++)
            {
                if (_widgets[i].Handle(evt))
                {
                    return true;
                }
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

        public IEnumerator GetEnumerator()
        {
            return _widgets.GetEnumerator();
        }

        public ISelectableWidget FindSelectedWidget()
        {
            foreach (Widget widget in _widgets)
            {
                ISelectableWidget selectableWidget = widget as ISelectableWidget;
                if (selectableWidget != null && selectableWidget.isSelected)
                {
                    return selectableWidget;
                }

                IParentWidget parentWidget = widget as IParentWidget;
                if (parentWidget != null)
                {
                    ISelectableWidget resultWidget = parentWidget.FindSelectedWidget();
                    if (resultWidget != null)
                    {
                        return resultWidget;
                    }
                }
            }

            return null;
        }

        public List<Widget> FindWidgetsByClass(string className)
        {
            List<Widget> widgets = new List<Widget>();

            foreach (Widget widget in _widgets)
            {
                if (widget.Classes.Contains(className))
                {
                    widgets.Add(widget);
                }

                IParentWidget parent = widget as IParentWidget;
                if (parent != null)
                {
                    widgets.AddRange(parent.FindWidgetsByClass(className));
                }
            }

            return widgets;
        }
    }
}
