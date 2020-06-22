using Events;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Events.InputHandling
{
    public class KeyboardKeyUpEvent :IEvent
    {
        public Keys _key;

        public KeyboardKeyUpEvent(Keys key)
        {
            _key = key;
        }
    }
}
