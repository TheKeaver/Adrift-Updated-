using Events;

namespace GameJam.Events.GameLogic
{
    public class IncreasePlayerScoreEvent : IEvent
    {
        public int ScoreAddend;
        public IncreasePlayerScoreEvent(int scoreAddend)
        {
            ScoreAddend = scoreAddend;
        }
    }
}
