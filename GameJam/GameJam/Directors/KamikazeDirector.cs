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
            if(collisionStartEvent.EntityA.HasComponent<PlayerShieldComponent>()
                && collisionStartEvent.EntityB.HasComponent<KamikazeComponent>())
            {
                HandleKamikazeOnPlayer(collisionStartEvent.EntityA, collisionStartEvent.EntityB);
            } else if (collisionStartEvent.EntityB.HasComponent<PlayerShieldComponent>()
                && collisionStartEvent.EntityA.HasComponent<KamikazeComponent>())
            {
                HandleKamikazeOnPlayer(collisionStartEvent.EntityB, collisionStartEvent.EntityA);
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

            Vector2 combined = movementComp.MovementVector;

            combined += shipToKamikaze * Constants.GamePlay.KAMIKAZE_PUSHBACK_FORCE;

            movementComp.MovementVector = combined;
        }
    }
}
