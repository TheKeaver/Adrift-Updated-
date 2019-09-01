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
        readonly Family playerShipFamily = Family.All(typeof(PlayerShipComponent), typeof(TransformComponent)).Get();
        readonly Family enemyFamily = Family.All(typeof(EnemyComponent), typeof(TransformComponent)).Get();

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
            Entity entityA = collisionStartEvent.EntityA;
            Entity entityB = collisionStartEvent.EntityB;

            if ( enemyFamily.Matches(entityA) && playerShipFamily.Matches(entityB) )
                HandleCollisionStart(entityB, entityA);
            else if ( playerShipFamily.Matches(entityA) && enemyFamily.Matches(entityB) )
                HandleCollisionStart(entityA, entityB);
        }

        void HandleCollisionStart(Entity entityA, Entity entityB)
        {
            entityA.GetComponent<PlayerShipComponent>().LifeRemaining -= 1;
            if(entityA.GetComponent<PlayerShipComponent>().LifeRemaining <= 0)
            {
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityA.GetComponent<TransformComponent>().Position));
                Engine.DestroyEntity(entityA);
                EventManager.Instance.QueueEvent(new GameOverEvent(entityA.GetComponent<PlayerShipComponent>().ShipShield));
            } else
            {
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityA.GetComponent<TransformComponent>().Position, false));
            }

            Engine.DestroyEntity(entityB);
            EventManager.Instance.QueueEvent(new CreateExplosionEvent(entityB.GetComponent<TransformComponent>().Position));
        }

        void HandleCollisionEnd(CollisionEndEvent collisionEndEvent)
        {
            Console.WriteLine("Well, at least CollisionEndEvent worked.....");
        }
    }
}
