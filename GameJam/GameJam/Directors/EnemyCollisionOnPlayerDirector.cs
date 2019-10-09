using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class EnemyCollisionOnPlayerDirector : BaseDirector
    {
        readonly Family playerFamily = Family.One(typeof(PlayerShieldComponent), typeof(PlayerShipComponent)).Get();
        readonly Family enemyShipFamily = Family.One(typeof(KamikazeComponent), typeof(ShootingEnemyComponent)).Exclude(typeof(ProjectileComponent)).Get();
        public EnemyCollisionOnPlayerDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
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

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
        }

        public override void UnregisterEvents()
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
