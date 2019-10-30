using System.Collections.Generic;
using Events;
using GameJam.Directors;
using GameJam.Events;
using GameJam.UI;
using Microsoft.Xna.Framework.Graphics;
using UI.Content.Pipeline;

namespace GameJam.States
{
    public class PauseGameState : GameState, IEventListener
    {
        private SpriteBatch _spriteBatch;
        private Root _root;

        public PauseGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        protected override void OnInitialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);
            _root.BuildFromPrototypes(GameManager.Content, GameManager.Content.Load<List<WidgetPrototype>>(CVars.Get<string>("ui_pause_menu")));

            ProcessManager.Attach(new PauseDirector(null, Content, ProcessManager));

            base.OnInitialize();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnTogglePause()
        {
            base.OnTogglePause();
        }

        protected override void OnKill()
        {
            base.OnKill();
        }

        protected override void RegisterListeners()
        {
            _root.RegisterListeners();

            EventManager.Instance.RegisterListener<TogglePauseGameEvent>(this);

            base.RegisterListeners();
        }

        protected override void UnregisterListeners()
        {
            _root.UnregisterListeners();

            EventManager.Instance.UnregisterListener(this);

            base.UnregisterListeners();
        }

        public bool Handle(IEvent evt)
        {
            if(evt is TogglePauseGameEvent)
            {
                HandleUnpause();

                return true;
            }

            return false;
        }

        private void HandleUnpause()
        {
            GameManager.ProcessManager.TogglePauseAll();
            Kill();
        }
    }
}
