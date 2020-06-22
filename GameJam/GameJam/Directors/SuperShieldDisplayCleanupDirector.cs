using Audrey;
using Audrey.Events;
using Events;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    class SuperShieldDisplayCleanupDirector : BaseDirector
    {
        readonly Family _superShieldDisplayFamily = Family.All(typeof(SuperShieldComponent)).Get();
        readonly ImmutableList<Entity> _superShieldDisplayEntities;

        public SuperShieldDisplayCleanupDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
            _superShieldDisplayEntities = Engine.GetEntitiesFor(_superShieldDisplayFamily);
        }

        public override bool Handle(IEvent evt)
        {
            ComponentRemovedEvent<PlayerShipComponent> playerShipCompRemovedEvent = evt as ComponentRemovedEvent<PlayerShipComponent>;
            if(playerShipCompRemovedEvent != null)
            {
                for (int i = 0; i < _superShieldDisplayEntities.Count; i++)
                {
                    Entity superShieldDisplayEntity = _superShieldDisplayEntities[i];

                    SuperShieldComponent superShieldComp = superShieldDisplayEntity.GetComponent<SuperShieldComponent>();
                    if (superShieldComp.ship == playerShipCompRemovedEvent.Entity)
                    {
                        Engine.DestroyEntity(superShieldDisplayEntity);
                        i--;
                    }
                }
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ComponentRemovedEvent<PlayerShipComponent>>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}
