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

            if (Vector2.Dot(shieldNormal, projectileDirection) < 0)
            {
                projectileDirection = getReflectionVector(projectileDirection, shieldNormal);
            }

            Vector2 directionAndMagnitude = projectileDirection * projectile.GetComponent<MovementComponent>().speed;
            Vector2 shipDirectionAndMagnitude = playerShield.GetComponent<PlayerShieldComponent>().ShipEntity.GetComponent<MovementComponent>().direction * playerShield.GetComponent<PlayerShieldComponent>().ShipEntity.GetComponent<MovementComponent>().speed;

            directionAndMagnitude = directionAndMagnitude + shipDirectionAndMagnitude;

            if (directionAndMagnitude.Length() > projectile.GetComponent<MovementComponent>().speed)
            {
                projectile.GetComponent<MovementComponent>().speed = directionAndMagnitude.Length();
            }
            directionAndMagnitude.Normalize();
            projectile.GetComponent<MovementComponent>().direction = directionAndMagnitude;

            EventManager.Instance.QueueEvent(new ProjectileBouncedEvent(projectile, projectile.GetComponent<TransformComponent>().Position));
        }

        private Vector2 getReflectionVector(Vector2 colliding, Vector2 normal)
        {
            return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
        }
    }
}
