using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class EnemyPushBackOnPlayerDirector : BaseDirector
    {
        readonly Family playerFamily = Family.One(typeof(PlayerShieldComponent), typeof(PlayerShipComponent)).Get();
        readonly Family enemyShipFamily = Family.One(typeof(EnemyComponent)).Exclude(typeof(ProjectileComponent), typeof(LaserBeamComponent), typeof(LaserBeamReflectionComponent)).Get();
        public EnemyPushBackOnPlayerDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
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

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        private void HandleCollisionStartEvent(CollisionStartEvent collisionStartEvent)
        {
            if( playerFamily.Matches(collisionStartEvent.EntityA)
                && enemyShipFamily.Matches(collisionStartEvent.EntityB) )
            {
                HandleEnemyShipOnPlayer(collisionStartEvent.EntityA, collisionStartEvent.EntityB);
            } else if ( enemyShipFamily.Matches(collisionStartEvent.EntityA)
                && playerFamily.Matches(collisionStartEvent.EntityB) )
            {
                HandleEnemyShipOnPlayer(collisionStartEvent.EntityB, collisionStartEvent.EntityA);
            }
        }

        private void HandleEnemyShipOnPlayer(Entity playerEntity, Entity enemyShip)
        {
            Entity player;
            if(playerEntity.HasComponent<PlayerShieldComponent>())
            {
                player = playerEntity.GetComponent<PlayerShieldComponent>().ShipEntity;
            }
            else
            {
                player = playerEntity;
            }

            if (player.GetComponent<PlayerShipComponent>().IsCollidingWithWall == true)
                return;

            MovementComponent movementComp = player.GetComponent<MovementComponent>();
            TransformComponent shipTransformComp = player.GetComponent<TransformComponent>();
            TransformComponent kamikazeTransformComp = enemyShip.GetComponent<TransformComponent>();

            Vector2 playerShipToEnemyShip = kamikazeTransformComp.Position - shipTransformComp.Position;
            playerShipToEnemyShip.Normalize();
            playerShipToEnemyShip *= -1;

            Vector2 combined = movementComp.MovementVector;

            combined += playerShipToEnemyShip * CVars.Get<float>("enemy_pushback_force");

            movementComp.MovementVector = combined;
        }
    }
}
