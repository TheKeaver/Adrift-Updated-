using Audrey;
using Audrey.Events;
using Events;
using GameJam.Components;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace GameJam.Directors
{
    public class EnemyCollisionWithShipDirector : BaseDirector
    {
        readonly Family playerShipFamily = Family.All(typeof(PlayerShipComponent), typeof(TransformComponent)).Get();
        readonly Family enemyFamily = Family.All(typeof(EnemyComponent), typeof(TransformComponent)).Exclude(typeof(LaserBeamReflectionComponent)).Get();

        public EnemyCollisionWithShipDirector(Engine engine, ContentManager content, ProcessManager processManager):base(engine, content, processManager)
        {
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
            EventManager.Instance.RegisterListener<ComponentRemovedEvent<PlayerShipComponent>>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if (evt is CollisionStartEvent)
            {
                OrderColliders(evt as CollisionStartEvent);
            }
            if(evt is ComponentRemovedEvent<PlayerShipComponent>)
            {
                HandleShipComponentRemovedEvent(evt as ComponentRemovedEvent<PlayerShipComponent>);
            }
            return false;
        }

        private void OrderColliders(CollisionStartEvent collisionStartEvent)
        {
            Entity entityA = collisionStartEvent.EntityA;
            Entity entityB = collisionStartEvent.EntityB;

            if ( enemyFamily.Matches(entityA) && playerShipFamily.Matches(entityB) )
                HandleCollisionStart(entityB, entityA);
            else if ( playerShipFamily.Matches(entityA) && enemyFamily.Matches(entityB) )
                HandleCollisionStart(entityA, entityB);
        }

        private void HandleCollisionStart(Entity entityA, Entity entityB)
        {
            if (!CVars.Get<bool>("god"))
            {
                entityA.GetComponent<PlayerShipComponent>().LifeRemaining -= 1;
                ChangeShieldColor(entityA);
            }
            if(entityA.GetComponent<PlayerShipComponent>().LifeRemaining <= 0)
            {
                Color color = Color.White;
                if (entityA.HasComponent<ColoredExplosionComponent>())
                {
                    color = entityA.GetComponent<ColoredExplosionComponent>().Color;
                }
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityA.GetComponent<TransformComponent>().Position, color));

                Engine.DestroyEntity(entityA);

                Entity responsibleEntity = entityB;
                if(entityB.HasComponent<LaserBeamComponent>())
                {
                    responsibleEntity = FindLaserBeamOwner(responsibleEntity);
                }
                EventManager.Instance.QueueEvent(new GameOverEvent(entityA.GetComponent<PlayerComponent>().Player, responsibleEntity));
                //Engine.DestroyEntity(entityA);
                return;
            } else
            {
                Color color = Color.White;
                if (entityA.HasComponent<ColoredExplosionComponent>())
                {
                    color = entityA.GetComponent<ColoredExplosionComponent>().Color;
                }
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityA.GetComponent<TransformComponent>().Position, color));
            }

            if (!entityB.HasComponent<LaserBeamComponent>())
            {
                {
                    Color color = Color.White;
                    if (entityB.HasComponent<ColoredExplosionComponent>())
                    {
                        color = entityB.GetComponent<ColoredExplosionComponent>().Color;
                    }
                    EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityB.GetComponent<TransformComponent>().Position, color, false));
                }
                Engine.DestroyEntity(entityB);
            }
        }

        private void ChangeShieldColor(Entity ship)
        {
            switch(ship.GetComponent<PlayerShipComponent>().LifeRemaining)
            {
                case 2:
                    foreach(Entity shield in ship.GetComponent<PlayerShipComponent>().ShipShields)
                    {
                        shield.GetComponent<VectorSpriteComponent>().ChangeColor(CVars.Get<Color>("color_player_shield_middle"));
                    }
                    break;
                case 1:
                    foreach (Entity shield in ship.GetComponent<PlayerShipComponent>().ShipShields)
                    {
                        shield.GetComponent<VectorSpriteComponent>().ChangeColor(CVars.Get<Color>("color_player_shield_low"));
                    }
                    break;
            }
        }

        private void HandleShipComponentRemovedEvent(ComponentRemovedEvent<PlayerShipComponent> shipComponentRemovedEvent)
        {
            foreach (Entity shield in shipComponentRemovedEvent.Component.ShipShields)
            {
                Engine.DestroyEntity(shield);
            }
        }

        private Entity FindLaserBeamOwner(Entity laserBeamEntity)
        {
            Family laserEnemyFamily = Family.All(typeof(LaserEnemyComponent)).Get();
            foreach (Entity entity in Engine.GetEntitiesFor(laserEnemyFamily))
            {
                LaserEnemyComponent laserEnemyComponent = entity.GetComponent<LaserEnemyComponent>();
                if(laserEnemyComponent.LaserBeamEntity == laserBeamEntity)
                {
                    return entity;
                }
            }
            return null;
        }
    }
}
