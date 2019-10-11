using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events.InputHandling
{
    /// <summary>
    /// An event queued when the mouse is moved.
    /// </summary>
    public class MouseMoveEvent : IEvent
    {
        public Vector2 PreviousPosition
        {
            get;
            private set;
        }

        public Vector2 CurrentPosition
        {
            get;
            private set;
        }

        public MouseMoveEvent(Vector2 previousPosition, Vector2 currentPosition)
        {
            PreviousPosition = previousPosition;
            CurrentPosition = currentPosition;
        }
    }
}
