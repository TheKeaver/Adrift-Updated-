using System;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class KamikazeDirector : BaseDirector
    {
        public KamikazeDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
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
            if(collisionStartEvent.entityA.HasComponent<PlayerShieldComponent>()
                && collisionStartEvent.entityB.HasComponent<KamikazeComponent>())
            {
                HandleKamikazeOnPlayer(collisionStartEvent.entityA, collisionStartEvent.entityB);
            } else if (collisionStartEvent.entityB.HasComponent<PlayerShieldComponent>()
                && collisionStartEvent.entityA.HasComponent<KamikazeComponent>())
            {
                HandleKamikazeOnPlayer(collisionStartEvent.entityB, collisionStartEvent.entityA);
            }
        }

        private void HandleKamikazeOnPlayer(Entity playerShield, Entity kamikaze)
        {
            Entity playerShip = playerShield.GetComponent<PlayerShieldComponent>().ShipEntity;
            MovementComponent movementComp = playerShip.GetComponent<MovementComponent>();
            TransformComponent shipTransformComp = playerShip.GetComponent<TransformComponent>();
            TransformComponent kamikazeTransformComp = kamikaze.GetComponent<TransformComponent>();

            Vector2 shipToKamikaze = kamikazeTransformComp.Position - shipTransformComp.Position;
            shipToKamikaze.Normalize();
            shipToKamikaze *= -1;

            Vector2 velocity = movementComp.direction;
            if (velocity.Length() > 0.5f)
            {
                velocity.Normalize();
            }
            velocity *= movementComp.speed;

            velocity += shipToKamikaze * CVars.Get<float>("kamikaze_enemy_pushback_force");

            movementComp.direction = velocity;
            movementComp.speed = velocity.Length();
        }
    }
}
