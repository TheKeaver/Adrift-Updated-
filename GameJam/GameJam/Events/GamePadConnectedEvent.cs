using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events
{
    /// <summary>
    /// An event queued when a gamepad is connected.
    /// </summary>
    public class GamePadConnectedEvent : IEvent
    {
        public PlayerIndex PlayerIndex
        {
            get;
            private set;
        }

        public GamePadConnectedEvent(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
