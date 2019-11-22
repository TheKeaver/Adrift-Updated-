using Audrey;
using Audrey.Events;
using Events;
using GameJam.Components;
using GameJam.Events;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace GameJam.Directors
{
    public class EnemyCollisionOnShieldDirector : BaseDirector
    {
        readonly Family enemyFamily = Family.All(typeof(EnemyComponent), typeof(TransformComponent)).Exclude(typeof(ProjectileComponent), typeof(LaserBeamComponent)).Get();
        readonly Family playerShieldFamily = Family.All(typeof(PlayerShieldComponent), typeof(TransformComponent)).Get();
        public EnemyCollisionOnShieldDirector(Engine engine, ContentManager content, ProcessManager processManager):base(engine, content, processManager)
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

            if ( enemyFamily.Matches(entityA) && playerShieldFamily.Matches(entityB) )
                HandleCollisionStart(entityB, entityA);
            else if (playerShieldFamily.Matches(entityA) && enemyFamily.Matches(entityB) )
                HandleCollisionStart(entityA, entityB);
        }

        void HandleCollisionStart(Entity playerShield, Entity enemy)
        {
            Color color = Color.White;
            if(enemy.HasComponent<ColoredExplosionComponent>())
            {
                color = enemy.GetComponent<ColoredExplosionComponent>().Color;
            }
            EventManager.Instance.QueueEvent(new CreateExplosionEvent(enemy.GetComponent<TransformComponent>().Position, color));
            EventManager.Instance.QueueEvent(new IncreasePlayerScoreEvent(playerShield.GetComponent<PlayerComponent>().Player, 1));
            Engine.DestroyEntity(enemy);
        }
    }
}
