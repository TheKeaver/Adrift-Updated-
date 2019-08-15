using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Directors
{
    public class BounceDirector : BaseDirector
    {
        public BounceDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
        }

        public override void UnregisterEvents()
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
            Entity entityA = collisionStartEvent.entityA;
            Entity entityB = collisionStartEvent.entityB;

            if (!entityA.HasComponent<EdgeComponent>() && entityB.HasComponent<EdgeComponent>())
                HandleBounceCollision(entityB, entityA);
            else if (entityA.HasComponent<EdgeComponent>() && !entityB.HasComponent<EdgeComponent>())
                HandleBounceCollision(entityA, entityB);
            else if (!entityA.HasComponent<PlayerShieldComponent>() && entityB.HasComponent<PlayerShieldComponent>())
                HandleBounceCollision(entityB, entityA);
            else if (entityA.HasComponent<PlayerShieldComponent>() && !entityB.HasComponent<PlayerShieldComponent>())
                HandleBounceCollision(entityA, entityB);
        }

        void HandleBounceCollision(Entity reflector, Entity bouncer)
        {
            if(reflector.HasComponent<EdgeComponent>())
            {
                if(bouncer.HasComponent<ProjectileComponent>() && bouncer.HasComponent<MovementComponent>())
                {
                    EventManager.Instance.QueueEvent(new ProjectileBouncedEvent(bouncer, bouncer.GetComponent<TransformComponent>().Position, reflector));
                    bouncer.GetComponent<ProjectileComponent>().bouncesLeft -= 1;

                    if (bouncer.GetComponent<ProjectileComponent>().bouncesLeft <= 0)
                        Engine.DestroyEntity(bouncer);
                }
                if (bouncer != null && bouncer.HasComponent<MovementComponent>())
                {
                    Vector2 bounceDirection = bouncer.GetComponent<MovementComponent>().direction;
                    bouncer.GetComponent<MovementComponent>().direction = getReflectionVector(
                        bounceDirection,
                        reflector.GetComponent<EdgeComponent>().Normal
                        );
                    Console.WriteLine("Collision with wall");
                }
            }
            if(reflector.HasComponent<PlayerShieldComponent>())
            {
                if (bouncer.HasComponent<ProjectileComponent>() && bouncer.HasComponent<MovementComponent>())
                {
                    EventManager.Instance.QueueEvent(new ProjectileBouncedEvent(bouncer, bouncer.GetComponent<TransformComponent>().Position, reflector));
                    Entity playerShip = reflector.GetComponent<PlayerShieldComponent>().ShipEntity;

                    TransformComponent shipTransformComp = playerShip.GetComponent<TransformComponent>();
                    TransformComponent shieldTransformComp = reflector.GetComponent<TransformComponent>();

                    Vector2 shieldNormal = shieldTransformComp.Position - shipTransformComp.Position;
                    shieldNormal.Normalize();

                    Vector2 bouncerDirection = bouncer.GetComponent<MovementComponent>().direction;
                    bouncerDirection.Normalize();

                    if (Vector2.Dot(shieldNormal, bouncerDirection) < 0)
                    {
                        bouncerDirection = getReflectionVector(bouncerDirection, shieldNormal);
                    }

                    Vector2 directionAndMagnitude = bouncerDirection * bouncer.GetComponent<MovementComponent>().speed;
                    Vector2 shipDirectionAndMagnitude = reflector.GetComponent<PlayerShieldComponent>().ShipEntity.GetComponent<MovementComponent>().direction * reflector.GetComponent<PlayerShieldComponent>().ShipEntity.GetComponent<MovementComponent>().speed;

                    directionAndMagnitude = directionAndMagnitude + shipDirectionAndMagnitude;

                    if (directionAndMagnitude.Length() > bouncer.GetComponent<MovementComponent>().speed)
                    {
                        bouncer.GetComponent<MovementComponent>().speed = directionAndMagnitude.Length();
                    }
                    directionAndMagnitude.Normalize();
                    bouncer.GetComponent<MovementComponent>().direction = directionAndMagnitude;
                }
            }
        }

        Vector2 getReflectionVector(Vector2 colliding, Vector2 normal)
        {
            return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
        }
    }
}
