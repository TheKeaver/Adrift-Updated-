using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Processes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Entities
{
    public static class ShootingEnemyEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position, ProcessManager processManager, ContentManager content)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new ShootingEnemyComponent(CVars.Get<int>("shooting_enemy_projectile_ammo")));
            entity.AddComponent(new RotationComponent(CVars.Get<float>("shooting_enemy_rotational_speed")));
            entity.AddComponent(new MovementComponent(new Vector2(0, 1), CVars.Get<float>("shooting_enemy_speed")));
            entity.AddComponent(new EnemyComponent());

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(new Vector2[]{ new Vector2(-2, -5),
                    new Vector2(-2, 5),
                    new Vector2(2, 4),
                    new Vector2(1, 1),
                    new Vector2(4, 0),
                    new Vector2(1, -1),
                    new Vector2(2, -4)
                    }, 0.3f, Color.Cyan, PolyRenderShape.PolyCapStyle.Filled, true)
            }));
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("shooting_enemy_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(Color.Cyan));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(-2, 5),
                new Vector2(2, 4),
                new Vector2(4, 0),
                new Vector2(2, -4),
                new Vector2(-2, -5)
            })));

            FireProjectileProcess fpp = new FireProjectileProcess(entity, engine, content);
            processManager.Attach(fpp);
            entity.AddComponent(new ProjectileSpawningProcessComponent(fpp));

            return entity;
        }
    }
}
