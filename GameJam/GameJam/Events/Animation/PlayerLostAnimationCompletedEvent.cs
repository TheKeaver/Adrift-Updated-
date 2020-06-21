using Events;

namespace GameJam.Events.Animation
{
    public class PlayerLostAnimationCompletedEvent : IEvent
    {
        public PlayerLostAnimationCompletedEvent(Player player)
        {
            Player = player;
        }

        Player Player;
    }
}
