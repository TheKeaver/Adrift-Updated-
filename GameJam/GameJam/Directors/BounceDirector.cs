using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Directors
{
    public class BounceDirector : BaseDirector
    {
        readonly Family edgeFamily = Family.All(typeof(EdgeComponent)).Get();
        readonly Family playerShieldFamily = Family.All(typeof(PlayerShieldComponent)).Get();
        readonly Family projectileFamily = Family.All(typeof(ProjectileComponent), typeof(MovementComponent)).Get();
        readonly Family playerShipFamily = Family.All(typeof(PlayerShipComponent), typeof(MovementComponent)).Get();
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
            Entity entityA = collisionStartEvent.EntityA;
            Entity entityB = collisionStartEvent.EntityB;

            if (!edgeFamily.Matches(entityA) && edgeFamily.Matches(entityB))
                HandleBounceCollision(entityB, entityA);
            else if (edgeFamily.Matches(entityA) && !edgeFamily.Matches(entityB))
                HandleBounceCollision(entityA, entityB);
            else if (projectileFamily.Matches(entityA) && playerShieldFamily.Matches(entityB))
                HandleBounceCollision(entityB, entityA);
            else if (playerShieldFamily.Matches(entityA) && projectileFamily.Matches(entityB))
                HandleBounceCollision(entityA, entityB);
            else if (playerShipFamily.Matches(entityA) && playerShipFamily.Matches(entityB))
                HandleBounceCollision(entityA, entityB);
        }

        void HandleBounceCollision(Entity reflector, Entity bouncer)
        {
            if(reflector.HasComponent<EdgeComponent>())
            {
                if(bouncer.HasComponent<ProjectileComponent>() && bouncer.HasComponent<MovementComponent>())
                {
                    EventManager.Instance.QueueEvent(new ProjectileBouncedEvent(bouncer, bouncer.GetComponent<TransformComponent>().Position, reflector));
                    bouncer.GetComponent<ProjectileComponent>().BouncesLeft -= 1;

                    if (bouncer.GetComponent<ProjectileComponent>().BouncesLeft <= 0)
                        Engine.DestroyEntity(bouncer);
                }
                if (bouncer != null && bouncer.HasComponent<MovementComponent>())
                {
                    Vector2 bounceDirection = bouncer.GetComponent<MovementComponent>().MovementVector;
                    if (bounceDirection.Length() == 0)
                    {
                        // Length is zero
                        bounceDirection = new Vector2(0, 0);
                    }
                    else
                    {
                        bounceDirection.Normalize();
                    }

                    bouncer.GetComponent<MovementComponent>().MovementVector = getReflectionVector(
                        bounceDirection, reflector.GetComponent<EdgeComponent>().Normal) 
                        * bouncer.GetComponent<MovementComponent>().MovementVector.Length();
                }
            }
            if(reflector.HasComponent<PlayerShieldComponent>())
            {
                EventManager.Instance.QueueEvent(new ProjectileBouncedEvent(bouncer, bouncer.GetComponent<TransformComponent>().Position, reflector));
                Entity playerShip = reflector.GetComponent<PlayerShieldComponent>().ShipEntity;

                TransformComponent shipTransformComp = playerShip.GetComponent<TransformComponent>();
                TransformComponent shieldTransformComp = reflector.GetComponent<TransformComponent>();

                Vector2 shieldNormal = shieldTransformComp.Position - shipTransformComp.Position;
                shieldNormal.Normalize();

                Vector2 bouncerDirection = bouncer.GetComponent<MovementComponent>().MovementVector;
                if (bouncerDirection.Length() == 0)
                {
                    // Length is zero
                    bouncerDirection = new Vector2(0, 0);
                }
                else
                {
                    bouncerDirection.Normalize();
                }

                if (Vector2.Dot(shieldNormal, bouncerDirection) < 0)
                {
                    bouncerDirection = getReflectionVector(bouncerDirection, shieldNormal);
                    bouncer.GetComponent<MovementComponent>().MovementVector = bouncerDirection * (bouncer.GetComponent<MovementComponent>().MovementVector.Length() + playerShip.GetComponent<MovementComponent>().MovementVector.Length());
                }
            }
            if (reflector.HasComponent<PlayerShipComponent>() && bouncer.HasComponent<PlayerShipComponent>() )
            {
                Vector2 ref2bounce = bouncer.GetComponent<TransformComponent>().Position - reflector.GetComponent<TransformComponent>().Position;
                Vector2 bounce2ref = reflector.GetComponent<TransformComponent>().Position - bouncer.GetComponent<TransformComponent>().Position;

                ref2bounce.Normalize();
                bounce2ref.Normalize();

                if (Vector2.Dot(ref2bounce, bounce2ref) < 0)
                {
                    ref2bounce = getReflectionVector(ref2bounce, bounce2ref);
                    reflector.GetComponent<MovementComponent>().MovementVector = ref2bounce * 
                        (bouncer.GetComponent<MovementComponent>().MovementVector.Length() + reflector.GetComponent<MovementComponent>().MovementVector.Length())/2;
                    bounce2ref = getReflectionVector(bounce2ref, ref2bounce);
                    bouncer.GetComponent<MovementComponent>().MovementVector = bounce2ref *
                        (bouncer.GetComponent<MovementComponent>().MovementVector.Length() + reflector.GetComponent<MovementComponent>().MovementVector.Length())/2;
                }
            }
        }

        Vector2 getReflectionVector(Vector2 colliding, Vector2 normal)
        {
            return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
        }
    }
}
