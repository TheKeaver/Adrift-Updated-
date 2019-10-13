using Events;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Events.InputHandling
{
    public class KeyboardKeyDownEvent : IEvent
    {
        public Keys Key;

        public KeyboardKeyDownEvent(Keys key)
        {
            Key = key;
        }
    }
}
