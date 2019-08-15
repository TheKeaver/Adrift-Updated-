using System;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class EnemyBulletDirector : BaseDirector
    {
        public EnemyBulletDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override bool Handle(IEvent evt)
        {
            if(evt is CollisionStartEvent)
            {
                HandleCollisionStartEvent(evt as CollisionStartEvent);
            }
            if (evt is CollisionEndEvent)
            {
                HandleCollisionEndEvent(evt as CollisionEndEvent);
            }

            return false;
        }

        private void HandleCollisionEndEvent(CollisionEndEvent collisionEndEvent)
        {
            if (collisionEndEvent.entityA.HasComponent<EnemyComponent>()
                && !collisionEndEvent.entityA.HasComponent<ProjectileComponent>()
                && collisionEndEvent.entityB.HasComponent<ProjectileComponent>())
            {
                collisionEndEvent.entityB.GetComponent<ProjectileComponent>().hasLeftShootingEnemy = true;
            }
            if (collisionEndEvent.entityB.HasComponent<EnemyComponent>()
                && !collisionEndEvent.entityB.HasComponent<ProjectileComponent>()
                && collisionEndEvent.entityA.HasComponent<ProjectileComponent>())
            {
                collisionEndEvent.entityA.GetComponent<ProjectileComponent>().hasLeftShootingEnemy = true;
            }
        }

        private void HandleCollisionStartEvent(CollisionStartEvent collisionStartEvent)
        {
            if(collisionStartEvent.entityA.HasComponent<EnemyComponent>()
                && !collisionStartEvent.entityA.HasComponent<ProjectileComponent>()
                && collisionStartEvent.entityB.HasComponent<ProjectileComponent>())
            {
                EnemyProjectileCollision(collisionStartEvent.entityA, collisionStartEvent.entityB);
            }
            if (collisionStartEvent.entityB.HasComponent<EnemyComponent>()
                && !collisionStartEvent.entityB.HasComponent<ProjectileComponent>()
                && collisionStartEvent.entityA.HasComponent<ProjectileComponent>())
            {
                EnemyProjectileCollision(collisionStartEvent.entityB, collisionStartEvent.entityA);
            }
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
            EventManager.Instance.RegisterListener<CollisionEndEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        private void EnemyProjectileCollision(Entity enemy, Entity projectile)
        {
            if (projectile.GetComponent<ProjectileComponent>().hasLeftShootingEnemy)
            {
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(enemy.GetComponent<TransformComponent>().Position));
                Engine.DestroyEntity(enemy);
                Engine.DestroyEntity(projectile);
            }
        }
    }
}
