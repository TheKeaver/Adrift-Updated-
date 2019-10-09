using System;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class HazardCollisionOnEnemyDirector : BaseDirector
    {
        readonly Family projectileFamily = Family.One(typeof(ProjectileComponent), typeof(LaserBeamComponent)).Get();
        readonly Family enemyFamily = Family.All(typeof(EnemyComponent)).Exclude(typeof(ProjectileComponent), typeof(LaserBeamComponent)).Get();
        public HazardCollisionOnEnemyDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override bool Handle(IEvent evt)
        {
            if(evt is CollisionStartEvent)
            {
                HandleCollisionStartEvent(evt as CollisionStartEvent);
            }

            return false;
        }

        private void HandleCollisionStartEvent(CollisionStartEvent collisionStartEvent)
        {
            if( enemyFamily.Matches(collisionStartEvent.EntityA)
                && projectileFamily.Matches(collisionStartEvent.EntityB) )
            {
                EnemyHazardCollision(collisionStartEvent.EntityA, collisionStartEvent.EntityB);
            }
            if (enemyFamily.Matches(collisionStartEvent.EntityB)
                && projectileFamily.Matches(collisionStartEvent.EntityA) )
            {
                EnemyHazardCollision(collisionStartEvent.EntityB, collisionStartEvent.EntityA);
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

        private void EnemyHazardCollision(Entity enemy, Entity hazard)
        {
            if(enemy.HasComponent<LaserEnemyComponent>())
            {
                LaserEnemyComponent laserEnemyComp = enemy.GetComponent<LaserEnemyComponent>();
                if(laserEnemyComp.LaserBeamEntity == hazard)
                {
                    return;
                }
            }
            Color color = Color.White;
            if (enemy.HasComponent<ColoredExplosionComponent>())
            {
                color = enemy.GetComponent<ColoredExplosionComponent>().Color;
            }
            EventManager.Instance.QueueEvent(new CreateExplosionEvent(enemy.GetComponent<TransformComponent>().Position, color));

            Engine.DestroyEntity(enemy);
            if (hazard.HasComponent<ProjectileComponent>())
            {
                Engine.DestroyEntity(hazard);
            }
        }
    }
}
