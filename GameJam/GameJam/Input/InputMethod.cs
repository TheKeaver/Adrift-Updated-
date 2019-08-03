using Microsoft.Xna.Framework;

namespace GameJam.Input
{
    /// <summary>
    /// Abstract interface for input devices (keyboards, controllers, etc.).
    /// </summary>
    public abstract class InputMethod
    {
        protected InputSnapshot _snapshot = new InputSnapshot();

        public abstract void Update(float dt);

        public InputSnapshot GetSnapshot()
        {
            return _snapshot;
        }
    }
}
