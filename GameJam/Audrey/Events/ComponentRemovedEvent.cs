using Events;

namespace Audrey.Events
{
    public class ComponentRemovedEvent: IEvent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public IComponent Component
        {
            get;
            private set;
        }

        public ComponentRemovedEvent(Entity entity, IComponent component)
        {
            Entity = entity;
            Component = component;
        }
    }
}
