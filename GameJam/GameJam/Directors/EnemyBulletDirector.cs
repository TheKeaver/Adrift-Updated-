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
            if (collisionEndEvent.EntityA.HasComponent<EnemyComponent>()
                && !collisionEndEvent.EntityA.HasComponent<ProjectileComponent>()
                && collisionEndEvent.EntityB.HasComponent<ProjectileComponent>())
            {
                collisionEndEvent.EntityB.GetComponent<ProjectileComponent>().hasLeftShootingEnemy = true;
            }
            if (collisionEndEvent.EntityB.HasComponent<EnemyComponent>()
                && !collisionEndEvent.EntityB.HasComponent<ProjectileComponent>()
                && collisionEndEvent.EntityA.HasComponent<ProjectileComponent>())
            {
                collisionEndEvent.EntityA.GetComponent<ProjectileComponent>().hasLeftShootingEnemy = true;
            }
        }

        private void HandleCollisionStartEvent(CollisionStartEvent collisionStartEvent)
        {
            if(collisionStartEvent.EntityA.HasComponent<EnemyComponent>()
                && !collisionStartEvent.EntityA.HasComponent<ProjectileComponent>()
                && collisionStartEvent.EntityB.HasComponent<ProjectileComponent>())
            {
                EnemyProjectileCollision(collisionStartEvent.EntityA, collisionStartEvent.EntityB);
            }
            if (collisionStartEvent.EntityB.HasComponent<EnemyComponent>()
                && !collisionStartEvent.EntityB.HasComponent<ProjectileComponent>()
                && collisionStartEvent.EntityA.HasComponent<ProjectileComponent>())
            {
                EnemyProjectileCollision(collisionStartEvent.EntityB, collisionStartEvent.EntityA);
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
                if(enemy.HasComponent<ProjectileSpawningProcessComponent>())
                {

                }
            }
        }
    }
}
