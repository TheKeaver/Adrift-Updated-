using Events;
using System;
using System.Collections.Generic;
using System.Text;

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
