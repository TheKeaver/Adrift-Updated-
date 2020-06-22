using Events;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Events.InputHandling
{
    public class KeyboardKeyDownEvent : IEvent
    {
        public Keys _key;

        public KeyboardKeyDownEvent(Keys key)
        {
            _key = key;
        }
    }
}
