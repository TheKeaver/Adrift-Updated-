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
            get
            {
                return GameManager.GlobalContent;
            }
        }

        public ProcessManager ProcessManager
        {
            get;
            private set;
        }

        public GameState(GameManager gameManager)
        {
            GameManager = gameManager;

            ProcessManager = new ProcessManager();
        }

        protected override void OnInitialize()
        {
            RegisterListeners();

            // Updates the camera and post-processor with the actual screen size
            // This fixes a bug present in a build of Super Pong
            // Must be triggered after all listeners are registered
            EventManager.Instance.TriggerEvent(new ResizeEvent(GameManager.GraphicsDevice.Viewport.Width,
                                                              GameManager.GraphicsDevice.Viewport.Height));

            Content.Locked = true;
        }

        protected virtual void RegisterListeners()
        {
        }
        protected virtual void UnregisterListeners()
        {
        }

        protected override void OnUpdate(float dt)
        {
            //ProcessManager.Update(dt);

            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
            ProcessManager.Update(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
        }

        protected override void OnKill()
        {
            ProcessManager.KillAll();
            UnregisterListeners();
        }

        protected override void OnTogglePause()
        {
            ProcessManager.TogglePauseAll();

            if(IsPaused)
            {
                UnregisterListeners();
            } else
            {
                RegisterListeners();
            }
        }

        protected void ChangeState(GameState gameState)
        {
            SetNext(gameState);
            Kill();
        }

    }
}
