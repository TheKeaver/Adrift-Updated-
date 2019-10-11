using System;
using System.Collections.Generic;
using Events;
using GameJam.Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UI.Content.Pipeline;

namespace GameJam.UI
{
    public class Root : Widget, IParentWidget
    {
        List<Widget> _widgets = new List<Widget>();

        Dictionary<string, WeakReference<Widget>> _widgetIdDict = new Dictionary<string, WeakReference<Widget>>();

        public bool mouseMode = false; // False = GamePad mode

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

        private void OnResize(float width, float height)
        {
            WidthValue = new FixedValue(width);
            HeightValue = new FixedValue(height);

            ComputeProperties();
        }

        public override bool Handle(IEvent evt)
        {
            MouseMoveEvent mouseMoveEvent = evt as MouseMoveEvent;
            if (mouseMode == false && mouseMoveEvent != null)
            {
                mouseMode = true;
            }

            GamePadButtonDownEvent gamePadButtonDownEvent = evt as GamePadButtonDownEvent;
            if (gamePadButtonDownEvent != null && mouseMode == true)
            {
                mouseMode = false;
                return true;
            }

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
    }
}
