using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Directors
{
    public class ShipDirector : BaseDirector
    {
        readonly Family playerShipFamily = Family.All(typeof(PlayerShipComponent), typeof(TransformComponent)).Get();
        readonly Family enemyFamily = Family.All(typeof(EnemyComponent), typeof(TransformComponent)).Exclude(typeof(LaserBeamReflectionComponent)).Get();

        public ShipDirector(Engine engine, ContentManager content, ProcessManager processManager):base(engine, content, processManager)
        {
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
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
            return false;
        }

        void OrderColliders(CollisionStartEvent collisionStartEvent)
        {
            Entity entityA = collisionStartEvent.EntityA;
            Entity entityB = collisionStartEvent.EntityB;

            if ( enemyFamily.Matches(entityA) && playerShipFamily.Matches(entityB) )
                HandleCollisionStart(entityB, entityA);
            else if ( playerShipFamily.Matches(entityA) && enemyFamily.Matches(entityB) )
                HandleCollisionStart(entityA, entityB);
        }

        void HandleCollisionStart(Entity entityA, Entity entityB)
        {
            if (!CVars.Get<bool>("god"))
            {
                entityA.GetComponent<PlayerShipComponent>().LifeRemaining -= 1;
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
                EventManager.Instance.QueueEvent(new GameOverEvent(entityA.GetComponent<PlayerShipComponent>().ShipShield));
            } else
            {
                Color color = Color.White;
                if (entityA.HasComponent<ColoredExplosionComponent>())
                {
                    color = entityA.GetComponent<ColoredExplosionComponent>().Color;
                }
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityA.GetComponent<TransformComponent>().Position, color, false));
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
    }
}
