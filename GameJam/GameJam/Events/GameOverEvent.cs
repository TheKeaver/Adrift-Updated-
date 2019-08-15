using Audrey;
using Events;
using GameJam.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    public class GameOverEvent : IEvent
    {
        public Entity shipShield;

        public GameOverEvent(Entity shield)
        {
            shipShield = shield;
        }
    }
}
