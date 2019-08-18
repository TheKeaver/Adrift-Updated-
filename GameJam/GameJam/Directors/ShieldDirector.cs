using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using Microsoft.Xna.Framework.Content;
using System;

namespace GameJam.Directors
{
    public class ShieldDirector : BaseDirector
    {
        public ShieldDirector(Engine engine, ContentManager content, ProcessManager processManager):base(engine, content, processManager)
        {
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
            EventManager.Instance.RegisterListener<GameOverEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if (evt is CollisionStartEvent)
            {
                OrderColliders(evt as CollisionStartEvent);
            }
            if (evt is GameOverEvent)
            {
                HandleGameOver(evt as GameOverEvent);
            }
            return false;
        }

        void OrderColliders(CollisionStartEvent collisionStartEvent)
        {
            Entity entityA = collisionStartEvent.entityA;
            Entity entityB = collisionStartEvent.entityB;

            if (entityA.HasComponent<EnemyComponent>() && entityB.HasComponent<PlayerShieldComponent>())
                HandleCollisionStart(entityB, entityA);
            else
                HandleCollisionStart(entityA, entityB);
        }

        void HandleCollisionStart(Entity playerShield, Entity enemy)
        {
            if (playerShield.HasComponent<PlayerShieldComponent>())
            {
                if(enemy.HasComponent<EnemyComponent>() && !enemy.HasComponent<ProjectileComponent>())
                {
                    EventManager.Instance.QueueEvent(new CreateExplosionEvent(enemy.GetComponent<TransformComponent>().Position));
                    Engine.DestroyEntity(enemy);
                    //EventManager.Instance.UnregisterListener()
                }
            }
        }

        void HandleGameOver(GameOverEvent evt)
        {
            Engine.DestroyEntity(evt.shipShield);
        }
    }
}
