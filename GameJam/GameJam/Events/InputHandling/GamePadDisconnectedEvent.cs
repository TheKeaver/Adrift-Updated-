using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events.InputHandling
{
    /// <summary>
    /// An event queued when a gamepad is disconnected.
    /// </summary>
    public class GamePadDisconnectedEvent : IEvent
    {
        public PlayerIndex PlayerIndex
        {
            get;
            private set;
        }

        public GamePadDisconnectedEvent(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
