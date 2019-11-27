using System;
using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Events;
using GameJam.Events.UI;
using GameJam.UI;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.States
{
    public class UIPlaygroundGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;

        Root _root;

        public UIPlaygroundGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        protected override void OnInitialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_test"));

            _root.AutoControlModeSwitching = true;

            // ProcessManager.Attach(new IDBlinkingProcess(_root, "label_blink", 1));

            base.OnInitialize();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            base.OnKill();
        }

        public bool Handle(IEvent evt)
        {
            if(evt is TestButtonPressedEvent)
            {
                Console.WriteLine("Test Button Pressed");
            }

            return false;
        }

        private class IDBlinkingProcess : IntervalProcess
        {
            public string ID
            {
                get;
                set;
            }

            public Root Root {
                get;
                private set;
            }

            public IDBlinkingProcess(Root root, string id, float interval) : base(interval)
            {
                Root = root;
                ID = id;
            }

            protected override void OnTick(float interval)
            {
                Root.FindWidgetByID(ID).Hidden = !Root.FindWidgetByID(ID).Hidden;
            }
        }

        void RegisterEvents()
        {
            _root.RegisterListeners();

            EventManager.Instance.RegisterListener<TestButtonPressedEvent>(this);
        }
        void UnregisterEvents()
        {
            _root.UnregisterListeners();

            EventManager.Instance.UnregisterListener(this);
        }
    }
}
