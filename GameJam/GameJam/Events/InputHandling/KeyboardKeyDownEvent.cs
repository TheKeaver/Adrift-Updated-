using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Events.InputHandling
{
    public class KeyboardKeyDownEvent : IEvent
    {
        public Keys _keyPressed;

        public KeyboardKeyDownEvent(Keys keyPressed)
        {
            _keyPressed = keyPressed;
        }
    }
}
