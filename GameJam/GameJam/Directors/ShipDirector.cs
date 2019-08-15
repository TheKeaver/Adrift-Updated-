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
    public class ShipDirector : BaseDirector
    {

        public ShipDirector(Engine engine, ContentManager content, ProcessManager processManager):base(engine, content, processManager)
        {
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
            EventManager.Instance.RegisterListener<CollisionEndEvent>(this);
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
            if (evt is CollisionEndEvent)
            {
                HandleCollisionEnd(evt as CollisionEndEvent);
            }
            return false;
        }

        void OrderColliders(CollisionStartEvent collisionStartEvent)
        {
            Entity entityA = collisionStartEvent.entityA;
            Entity entityB = collisionStartEvent.entityB;

            if (entityA.HasComponent<EnemyComponent>() && entityB.HasComponent<PlayerShipComponent>())
                HandleCollisionStart(entityB, entityA);
            else
                HandleCollisionStart(entityA, entityB);
        }

        void HandleCollisionStart(Entity entityA, Entity entityB)
        {
            if (entityA.HasComponent<PlayerShipComponent>())
            {
                if(entityB.HasComponent<EnemyComponent>())
                {
                    entityA.GetComponent<PlayerShipComponent>().lifeRemaining -= 1;
                    if(entityA.GetComponent<PlayerShipComponent>().lifeRemaining <= 0)
                    {
                        EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityA.GetComponent<TransformComponent>().Position));
                        Engine.DestroyEntity(entityA);
                        EventManager.Instance.QueueEvent(new GameOverEvent(entityA.GetComponent<PlayerShipComponent>().shipShield));
                    } else
                    {
                        EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityA.GetComponent<TransformComponent>().Position, false));
                    }

                    Engine.DestroyEntity(entityB);
                    EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityB.GetComponent<TransformComponent>().Position));
                }
                else
                {
                    Console.WriteLine("Well done, multiplayer has been enabled!");
                }
            }
        }

        void HandleCollisionEnd(CollisionEndEvent collisionEndEvent)
        {
            Console.WriteLine("Well, at least CollisionEndEvent worked.....");
        }
    }
}
