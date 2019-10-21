using System;
using System.Collections.Generic;
using Events;
using GameJam.Events;
using GameJam.Events.InputHandling;
using GameJam.Events.UI;
using GameJam.UI;
using Microsoft.Xna.Framework.Graphics;
using UI.Content.Pipeline;

namespace GameJam.States
{
    class UIMenuGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public UIMenuGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<PlayGameButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<OptionsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<QuitGameButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
        }

        void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            RegisterEvents();

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/MainMenu"));
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
            if(evt is PlayGameButtonPressedEvent)
            {
                GameManager.ChangeState(new UILobbyGameState(GameManager));
            }
            if(evt is OptionsButtonPressedEvent)
            {
                Console.WriteLine("Options Pressed");
            }
            if(evt is QuitGameButtonPressedEvent)
            {
                EventManager.Instance.QueueEvent(new GameShutdownEvent());
            }
            return false;
        }
    }
}
