using System;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class BulletBounceDirector : BaseDirector
    {
        public BulletBounceDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
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
                && collisionStartEvent.entityB.HasComponent<ProjectileComponent>())
            {
                BounceBulletOffShield(collisionStartEvent.entityA, collisionStartEvent.entityB);
            }
            if (collisionStartEvent.entityB.HasComponent<PlayerShieldComponent>()
                && collisionStartEvent.entityA.HasComponent<ProjectileComponent>())
            {
                BounceBulletOffShield(collisionStartEvent.entityB, collisionStartEvent.entityA);
            }
        }

        private void BounceBulletOffShield(Entity playerShield, Entity projectile)
        {
            Entity playerShip = playerShield.GetComponent<PlayerShieldComponent>().ShipEntity;

            TransformComponent shipTransformComp = playerShip.GetComponent<TransformComponent>();
            TransformComponent shieldTransformComp = playerShield.GetComponent<TransformComponent>();

            Vector2 shieldNormal = shieldTransformComp.Position - shipTransformComp.Position;
            shieldNormal.Normalize();

            Vector2 projectileDirection = projectile.GetComponent<MovementComponent>().direction;
            projectileDirection.Normalize();
            projectileDirection = getReflectionVector(projectileDirection, shieldNormal);
            projectile.GetComponent<MovementComponent>().direction = projectileDirection;
            projectile.GetComponent<MovementComponent>().speed +=
                playerShield.GetComponent<PlayerShieldComponent>().ShipEntity
                .GetComponent<MovementComponent>().direction.Length() +
                playerShield.GetComponent<PlayerShieldComponent>().ShipEntity
                .GetComponent<MovementComponent>().speed;

            EventManager.Instance.QueueEvent(new ProjectileBouncedEvent(projectile, projectile.GetComponent<TransformComponent>().Position));
        }

        private Vector2 getReflectionVector(Vector2 colliding, Vector2 normal)
        {
            return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
        }
    }
}
