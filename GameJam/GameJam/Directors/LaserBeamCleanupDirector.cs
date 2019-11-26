using Audrey;
using Audrey.Events;
using Events;
using GameJam.Components;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class LaserBeamCleanupDirector : BaseDirector
    {
        public LaserBeamCleanupDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override bool Handle(IEvent evt)
        {
            ComponentRemovedEvent<LaserBeamComponent> laserBeamComponentRemovedEvent = evt as ComponentRemovedEvent<LaserBeamComponent>;
            if (laserBeamComponentRemovedEvent != null)
            {
                LaserBeamComponent laserBeamComp = laserBeamComponentRemovedEvent.Component;

                // Laser beam destruction
                Entity reflectionBeamEntity = laserBeamComp.ReflectionBeamEntity;
                while(reflectionBeamEntity != null)
                {
                    Entity entityToDestroy = reflectionBeamEntity;

                    LaserBeamComponent reflectionBeamComp = entityToDestroy.GetComponent<LaserBeamComponent>();
                    if(reflectionBeamComp != null)
                    {
                        reflectionBeamEntity = reflectionBeamComp.ReflectionBeamEntity;
                    }

                    Engine.DestroyEntity(entityToDestroy);
                }
            }

            ComponentRemovedEvent<LaserEnemyComponent> laserEnemyComponentRemovedEvent = evt as ComponentRemovedEvent<LaserEnemyComponent>;
            if (laserEnemyComponentRemovedEvent != null)
            {
                LaserEnemyComponent laserEnemyComp = laserEnemyComponentRemovedEvent.Component;
                if(laserEnemyComp.LaserBeamEntity != null)
                {
                    Engine.DestroyEntity(laserEnemyComp.LaserBeamEntity);
                }
            }

            return false;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ComponentRemovedEvent<LaserBeamComponent>>(this);
            EventManager.Instance.RegisterListener<ComponentRemovedEvent<LaserEnemyComponent>>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}
