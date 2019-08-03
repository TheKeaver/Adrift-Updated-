using Audrey;
using Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    public class CollisionEndEvent : IEvent
    {
        public Entity entityA;
        public Entity entityB;

        public CollisionEndEvent(Entity nttA, Entity nttB)
        {
            entityA = nttA;
            entityB = nttB;
        }
    }
}
