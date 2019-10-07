using Events;

namespace Audrey.Events
{
    public class EntityCreatedEvent : IEvent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public EntityCreatedEvent(Entity entity)
        {
            Entity = entity;
        }
    }
}
