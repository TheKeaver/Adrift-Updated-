using System;
using System.Collections.Generic;
using Events;
using GameJam.Events.UI;
using GameJam.UI;
using Microsoft.Xna.Framework.Graphics;
using UI.Content.Pipeline;

namespace GameJam.States
{
    class UILobbyGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public UILobbyGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        void RegisterEvents()
        {
            // Player Joins Lobby Player 1 Event
            // Player Joins Lobby Player 2 Event
            // Back To Main Menu Event
            // Player 2 Leaves Lobby Event
            // Start Game Event
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
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/LobbyMenu"));
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
            return false;
        }
    }
}
