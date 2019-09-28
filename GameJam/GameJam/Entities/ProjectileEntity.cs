using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Entities
{
    public static class ProjectileEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position, Vector2 direction)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new ProjectileComponent(CVars.Get<int>("shooting_enemy_projectile_bounces")));
            entity.AddComponent(new BounceComponent());
            entity.AddComponent(new MovementComponent(direction, CVars.Get<float>("shooting_enemy_projectile_speed")));
            entity.AddComponent(new EnemyComponent());

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new QuadRenderShape(new Vector2(3, -1), new Vector2(3, 1),
                    new Vector2(-3, 1), new Vector2(-3, -1),
                    Color.Red)
            }));
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("projectile_size"), false);

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(3, -1),
                new Vector2(3, 1),
                new Vector2(-3, 1),
                new Vector2(-3, -1)
            })));

            return entity;
        }
    }
}
