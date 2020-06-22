using Events;

namespace GameJam.Events.GameLogic
{
    public class IncreasePlayerScoreEvent : IEvent
    {
        public Player Player;
        public int ScoreIncrement;

        public IncreasePlayerScoreEvent(Player player, int scoreIncrement)
        {
            Player = player;
            ScoreIncrement = scoreIncrement;
        }
    }
}
