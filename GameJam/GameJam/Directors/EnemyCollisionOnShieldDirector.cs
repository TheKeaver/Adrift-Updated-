using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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
            EventManager.Instance.RegisterListener<GameOverEvent>(this);
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
            if (evt is GameOverEvent)
            {
                HandleGameOver(evt as GameOverEvent);
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
            EventManager.Instance.QueueEvent(new IncreasePlayerScoreEvent(1));
            Engine.DestroyEntity(enemy);
        }

        void HandleGameOver(GameOverEvent evt)
        {
            Engine.DestroyEntity(evt.ShipShield);
        }
    }
}
