using Audrey;
using Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    public class OutOfAmmoEvent : IEvent
    {
        public Entity shootingEnemyOOA;
        public OutOfAmmoEvent(Entity self)
        {
            shootingEnemyOOA = self;
        }
    }
}
