using Audrey;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    /// <summary>
    /// Base class for directors.
    /// </summary>
    public abstract class BaseDirector
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
    }
}
