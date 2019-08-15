using Audrey;
using Events;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    /// <summary>
    /// Base class for directors.
    /// </summary>
    public abstract class BaseDirector : IEventListener
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

        public abstract void RegisterEvents();

        public abstract void UnregisterEvents();
        public abstract bool Handle(IEvent evt);
    }
}
