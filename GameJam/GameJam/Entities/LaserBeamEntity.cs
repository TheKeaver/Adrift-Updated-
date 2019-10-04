using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public class LaserBeamEntity
    {
        public static Entity Create(Engine engine, Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new LaserBeamComponent());

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new QuadRenderShape(new Vector2(10, -10),
                    new Vector2(10, 10),
                    new Vector2(-10, 10),
                    new Vector2(-10, -10), Color.Red)
            }));

            //entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
            //    new Vector2(4, 0),
            //    new Vector2(-3, 3),
            //    new Vector2(-5, 1),
            //    new Vector2(-5, -1),
            //    new Vector2(-3, -3)
            //})));

            return entity;
        }
    }
}
