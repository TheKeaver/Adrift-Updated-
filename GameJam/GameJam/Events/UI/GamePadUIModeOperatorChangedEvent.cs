using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events.UI
{
    public class GamePadUIModeOperatorChangedEvent : IEvent
    {
        public PlayerIndex PlayerIndex;

        public GamePadUIModeOperatorChangedEvent(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
