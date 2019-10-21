using System;
using GameJam.Content;
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
