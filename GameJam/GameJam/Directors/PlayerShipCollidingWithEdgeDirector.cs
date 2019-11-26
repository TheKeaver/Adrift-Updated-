using Audrey;
using Events;
using GameJam.Components;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class PlayerShipCollidingWithEdgeDirector : BaseDirector
    {
        readonly Family playerShipFamily = Family.All(typeof(PlayerShipComponent)).Get();
        readonly Family edgeEntityFamily = Family.All(typeof(EdgeComponent)).Get();

        public PlayerShipCollidingWithEdgeDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
        }
        public override bool Handle(IEvent evt)
        {
            if(evt is CollisionStartEvent)
            {
                HandleCollisionStartEvent(evt as CollisionStartEvent);
            }
            if(evt is CollisionEndEvent)
            {
                HandleCollisionEndEvent(evt as CollisionEndEvent);
            }
            return false;
        }

        private void HandleCollisionEndEvent(CollisionEndEvent collisionEndEvent)
        {
            if (playerShipFamily.Matches(collisionEndEvent.EntityA)
                && edgeEntityFamily.Matches(collisionEndEvent.EntityB))
            {
                HandleEnemyShipOffPlayer(collisionEndEvent.EntityA);
            }
            else if (edgeEntityFamily.Matches(collisionEndEvent.EntityA)
              && playerShipFamily.Matches(collisionEndEvent.EntityB))
            {
                HandleEnemyShipOffPlayer(collisionEndEvent.EntityB);
            }
        }

        private void HandleEnemyShipOffPlayer(Entity playerShipEntity)
        {
            playerShipEntity.GetComponent<PlayerShipComponent>().IsCollidingWithWall = false;
        }

        private void HandleCollisionStartEvent(CollisionStartEvent collisionStartEvent)
        {
            if (playerShipFamily.Matches(collisionStartEvent.EntityA)
                && edgeEntityFamily.Matches(collisionStartEvent.EntityB))
            {
                HandleEnemyShipOnPlayer(collisionStartEvent.EntityA);
            }
            else if (edgeEntityFamily.Matches(collisionStartEvent.EntityA)
              && playerShipFamily.Matches(collisionStartEvent.EntityB))
            {
                HandleEnemyShipOnPlayer(collisionStartEvent.EntityB);
            }
        }

        private void HandleEnemyShipOnPlayer(Entity playerShipEntity)
        {
            playerShipEntity.GetComponent<PlayerShipComponent>().IsCollidingWithWall = true;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CollisionStartEvent>(this);
            EventManager.Instance.RegisterListener<CollisionEndEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }
    }
}
