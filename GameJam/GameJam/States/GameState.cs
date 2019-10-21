using System;
using Events;
using GameJam.Content;
using GameJam.Events;
using GameJam.Processes;

namespace GameJam
{
    /// <summary>
    /// A state of the game that can be switched between other states.
    /// </summary>
    public abstract class GameState : RenderProcess
    {
        protected GameManager GameManager
        {
            get;
            private set;
        }
        public LockingContentManager Content
        {
            get;
            private set;
        }

        public GameState(GameManager gameManager)
        {
            GameManager = gameManager;

            Content = new LockingContentManager(gameManager.Services);
            Content.RootDirectory = "Content";
        }

        protected override void OnInitialize()
        {
            // Updates the camera and post-processor with the actual screen size
            // This fixes a bug present in a build of Super Pong
            // Must be triggered after all listeners are registered
            EventManager.Instance.TriggerEvent(new ResizeEvent(GameManager.GraphicsDevice.Viewport.Width,
                                                              GameManager.GraphicsDevice.Viewport.Height));

            Content.Locked = true;
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
        }

        protected override void OnKill()
        {
            UnloadContent();
        }

        private void UnloadContent()
        {
            Content.Unload();
        }

        protected override void OnTogglePause()
        {
        }

        protected void ChangeState(GameState gameState)
        {
            SetNext(gameState);
            Kill();
        }

    }
}
