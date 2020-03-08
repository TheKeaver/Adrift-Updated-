using System;
using System.Collections;
using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Events;
using GameJam.Events;
using GameJam.Events.InputHandling;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.UI
{
    public class Root : Widget, IParentWidget
    {
        private List<Widget> _widgets = new List<Widget>();

        private List<Widget> _deferredDrawQueue = new List<Widget>();

        internal Dictionary<string, WeakReference<Widget>> _widgetIdDict = new Dictionary<string, WeakReference<Widget>>();

        public bool MouseMode
        {
            get
            {
                return CVars.Get<bool>("ui_mouse_mode");
            }
            set
            {
                CVars.Get<bool>("ui_mouse_mode") = true;
            }
        }
        public bool AutoControlModeSwitching
        {
            get
            {
                return CVars.Get<bool>("ui_auto_control_mode_switching");
            }
            set
            {
                CVars.Get<bool>("ui_auto_control_mode_switching") = value;
            }
        }

        public Root(float width, float height) : base(HorizontalAlignment.Left, new FixedValue(0), VerticalAlignment.Top, new FixedValue(0), new FixedValue(width), new FixedValue(height))
        {
            OnResize(width, height);
        }

        public void RegisterListeners()
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);

            EventManager.Instance.RegisterListener<MouseMoveEvent>(this);
            EventManager.Instance.RegisterListener<MouseButtonEvent>(this);

            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
        }

        public void UnregisterListeners()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public void BuildFromPrototypes(ContentManager content, List<WidgetPrototype> prototypes)
        {
            foreach (WidgetPrototype prototype in prototypes)
            {
                Add(WidgetFactory.CreateFromPrototype(content, prototype, ref _widgetIdDict));
            }
        }

        public void Add(Widget widget)
        {
            _widgets.Add(widget);
            widget.Parent = this;
            widget.ComputeProperties();
        }

        public void Remove(Widget widget)
        {
            _widgets.Remove(widget);
        }

        public bool AssignID(string id, Widget widget)
        {
            if(_widgetIdDict.ContainsKey(id))
            {
                return false;
            }

            _widgetIdDict.Add(id, new WeakReference<Widget>(widget));

            return true;
        }

        public Widget FindWidgetByID(string id)
        {
            if(!_widgetIdDict.ContainsKey(id))
            {
                return null;
            }
            Widget widget;
            if(_widgetIdDict[id].TryGetTarget(out widget))
            {
                return widget;
            }
            return null;
        }

        public List<Widget> FindWidgetsByClass(string className)
        {
            className = className.Trim().ToLower();

            List<Widget> widgets = new List<Widget>();

            foreach(Widget widget in _widgets)
            {
                if(widget.Classes.Contains(className))
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

        public ISelectableWidget FindSelectedWidget()
        {
            foreach(Widget widget in _widgets)
            {
                ISelectableWidget selectableWidget = widget as ISelectableWidget;
                if(selectableWidget != null && selectableWidget.isSelected)
                {
                    return selectableWidget;
                }

                IParentWidget parentWidget = widget as IParentWidget;
                if(parentWidget != null)
                {
                    ISelectableWidget resultWidget = parentWidget.FindSelectedWidget();
                    if(resultWidget != null)
                    {
                        return resultWidget;
                    }
                }
            }

            return null;
        }

        public override void Render(SpriteBatch spriteBatch, FieldFontRenderer fieldFontRenderer)
        {
            if (!Hidden)
            {
                for (int i = 0; i < _widgets.Count; i++)
                {
                    _widgets[i].Render(spriteBatch, fieldFontRenderer);
                }

                for (int i = 0; i < _deferredDrawQueue.Count; i++)
                {
                    _deferredDrawQueue[i].Render(spriteBatch, fieldFontRenderer);
                }
                _deferredDrawQueue.Clear();
            }
        }

        public void RequestDeferredDraw(Widget widget)
        {
            _deferredDrawQueue.Add(widget);
        }

        private void OnResize(float width, float height)
        {
            WidthValue = new FixedValue(width);
            HeightValue = new FixedValue(height);

            ComputeProperties();
        }

        public override bool Handle(IEvent evt)
        {
            for (int i = 0; i < _widgets.Count; i++)
            {
                if(_widgets[i].Handle(evt))
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
            for(int i = 0; i < _widgets.Count; i++)
            {
                _widgets[i].ComputeProperties();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _widgets.GetEnumerator();
        }
    }
}
