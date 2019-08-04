using Audrey;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class CollisionSystem : BaseSystem
    {
        Family _collisionFamily = Family.All(typeof(CollisionComponent), typeof(TransformComponent)).Get();
        ImmutableList<Entity> _collisionEntities;

        public CollisionSystem(Engine engine) : base(engine)
        {
            _collisionEntities = engine.GetEntitiesFor(_collisionFamily);
        }

        public override void Update(float dt)
        {
            processCollisions();
        }

        void processCollisions()
        {
            for(int i=0; i<_collisionEntities.Count; i++)
            {
                TransformComponent transformCompA = _collisionEntities[i].GetComponent<TransformComponent>();
                CollisionComponent collisionCompA = _collisionEntities[i].GetComponent<CollisionComponent>();

                BoundingRect locationA = new BoundingRect(
                    transformCompA.Position.X - collisionCompA.BoundingBoxComponent.Width / 2,
                    transformCompA.Position.Y - collisionCompA.BoundingBoxComponent.Height / 2,
                    collisionCompA.BoundingBoxComponent.Width,
                    collisionCompA.BoundingBoxComponent.Height
                    );
                
                for(int j=i+1; j<_collisionEntities.Count; j++)
                {
                    TransformComponent transformCompB = _collisionEntities[j].GetComponent<TransformComponent>();
                    CollisionComponent collisionCompB = _collisionEntities[j].GetComponent<CollisionComponent>();

                    BoundingRect locationB = new BoundingRect(
                        transformCompB.Position.X - collisionCompB.BoundingBoxComponent.Width / 2,
                        transformCompB.Position.Y - collisionCompB.BoundingBoxComponent.Height / 2,
                        collisionCompB.BoundingBoxComponent.Width,
                        collisionCompB.BoundingBoxComponent.Height
                        );

                    if ( locationA.Intersects(locationB) && !( collisionCompA.collidingWith.Contains(_collisionEntities[j])) )
                        {
                            EventManager.Instance.QueueEvent(new CollisionStartEvent(_collisionEntities[i], _collisionEntities[j]));
                            collisionCompA.collidingWith.Add(_collisionEntities[j]);
                            collisionCompB.collidingWith.Add(_collisionEntities[i]);
                        }
                    else
                    {
                        if ( !(locationA.Intersects(locationB)) && collisionCompA.collidingWith.Contains(_collisionEntities[j]) )
                        {
                            Console.WriteLine("Normal Collision System");
                            EventManager.Instance.QueueEvent(new CollisionEndEvent(_collisionEntities[i], _collisionEntities[j]));
                            collisionCompA.collidingWith.Remove(_collisionEntities[j]);
                            collisionCompB.collidingWith.Remove(_collisionEntities[i]);
                        }
                    }
                }
            }
        }
    }
}
