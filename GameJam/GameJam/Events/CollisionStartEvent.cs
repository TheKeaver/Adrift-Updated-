using Audrey;
using Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    public class CollisionStartEvent : IEvent
    {
        public Entity entityA;
        public Entity entityB;

        public CollisionStartEvent(Entity nttA, Entity nttB)
        {
            entityA = nttA;
            entityB = nttB;
        }
    }
}
