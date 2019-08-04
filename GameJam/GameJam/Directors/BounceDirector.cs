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

            if (!(entityA.HasComponent<EdgeComponent>()) && entityB.HasComponent<EdgeComponent>())
                HandleBounceCollision(entityB, entityA);
            else if ((entityA.HasComponent<EdgeComponent>()) && !entityB.HasComponent<EdgeComponent>())
                HandleBounceCollision(entityA, entityB);
        }

        void HandleBounceCollision(Entity edge, Entity bouncer)
        {
            if(edge.HasComponent<EdgeComponent>())
            {
                if(bouncer.HasComponent<ProjectileComponent>() && bouncer.HasComponent<MovementComponent>())
                {
                    EventManager.Instance.TriggerEvent(new ProjectileBouncedEvent(bouncer, bouncer.GetComponent<TransformComponent>().Position));
                }
                if (bouncer != null && bouncer.HasComponent<MovementComponent>())
                {
                    Vector2 bounceDirection = bouncer.GetComponent<MovementComponent>().direction;
                    bouncer.GetComponent<MovementComponent>().direction = getReflectionVector(
                        bounceDirection,
                        edge.GetComponent<EdgeComponent>().Normal
                        );
                    Console.WriteLine("Collision with wall");
                }
            }
        }

        Vector2 getReflectionVector(Vector2 colliding, Vector2 normal)
        {
            return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
        }
    }
}
