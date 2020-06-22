using Audrey;
using Events;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    /// <summary>
    /// Base class for directors.
    /// </summary>
    public abstract class BaseDirector : Process, IEventListener
    {
        protected Engine Engine
        {
            get;
            private set;
        }
        protected ContentManager Content
        {
            get;
            private set;
        }
        protected ProcessManager ProcessManager
        {
            get;
            private set;
        }

        public BaseDirector(Engine engine, ContentManager content, ProcessManager processManager)
        {
            Engine = engine;
            Content = content;
            ProcessManager = processManager;
        }

        protected abstract void RegisterEvents();

        protected abstract void UnregisterEvents();
        public abstract bool Handle(IEvent evt);

        protected override void OnInitialize()
        {
            RegisterEvents();
        }

        protected override void OnKill()
        {
            UnregisterEvents();
        }

        protected sealed override void OnUpdate(float dt)
        {
        }

        protected sealed override void OnTogglePause()
        {
            if(IsPaused)
            {
                UnregisterEvents();
            } else
            {
                RegisterEvents();
            }
        }
    }
}
