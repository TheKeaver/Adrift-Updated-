using System;
using System.Collections.Generic;
using Events;
using GameJam.Events.UI;
using GameJam.UI;
using Microsoft.Xna.Framework.Graphics;
using UI.Content.Pipeline;

namespace GameJam.States
{
    public class UIPlaygroundGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;

        Root _root;

        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public UIPlaygroundGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<TestButtonPressedEvent>(this);
        }
        void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);

            RegisterEvents();

            ProcessManager.Attach(new IDBlinkingProcess(_root, "label_blink", 1));
        }

        public override void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/test"));
        }


        public override void Show()
        {
            _root.RegisterListeners();
        }

        public override void Hide()
        {
            _root.UnregisterListeners();
        }

        public override void Update(float dt)
        {
            ProcessManager.Update(dt);
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Dispose()
        {
            UnregisterEvents();
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
    }
}
