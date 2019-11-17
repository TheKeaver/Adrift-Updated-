using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events.UI
{
    public class EnterGamePadUIModeEvent : IEvent
    {
        public PlayerIndex PlayerIndex;

        public EnterGamePadUIModeEvent(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
