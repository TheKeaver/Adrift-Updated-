using System;
using GameJam.Content;

namespace GameJam
{
    /// <summary>
    /// A state of the game that can be switched between other states.
    /// </summary>
    public abstract class GameState : IDisposable
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

        public abstract void Initialize();

        public abstract void LoadContent();

        public void UnloadContent()
        {
            Content.Unload();
        }

        public abstract void Show();

        public abstract void Hide();

        public abstract void Update(float dt);

        public abstract void Draw(float dt);

        public abstract void Dispose();
    }
}
