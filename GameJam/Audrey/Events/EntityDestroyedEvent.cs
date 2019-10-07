using Events;

namespace Audrey.Events
{
    public class EntityDestroyedEvent: IEvent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public EntityDestroyedEvent(Entity entity)
        {
            Entity = entity;
        }
    }
}
