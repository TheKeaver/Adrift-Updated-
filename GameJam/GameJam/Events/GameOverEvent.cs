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
        public Entity ShipShield;

        public GameOverEvent(Entity shield)
        {
            ShipShield = shield;
        }
    }
}
