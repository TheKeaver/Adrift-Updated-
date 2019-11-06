using Audrey;

namespace GameJam.Systems
{
    /// <summary>
    /// Base class for systems.
    /// </summary>
    public abstract class BaseSystem : Process
    {
        protected Engine Engine {
            get;
            private set;
        }

        public BaseSystem(Engine engine)
        {
            Engine = engine;
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnKill()
        {
        }

        protected override void OnTogglePause()
        {
        }
    }
}
