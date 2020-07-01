using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class PlayerShipEntity
    {
        public static Entity Create(Engine engine, Vector2 position)
        {
            return Create(engine, position, CVars.Get<Color>("color_player_ship"));
        }

        public static Entity Create(Engine engine, Vector2 position, Color color)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));

            entity.AddComponent(new MovementComponent());
            entity.AddComponent(new PlayerShipComponent(CVars.Get<int>("player_ship_max_health"), CVars.Get<float>("player_super_shield_max")));
            entity.AddComponent(new BounceComponent());

            entity.AddComponent(new CameraTrackingComponent());
            entity.AddComponent(new TransformHistoryComponent(position, 0));

            entity.GetComponent<MovementComponent>().UpdateRotationWithDirection = CVars.Get<bool>("player_rotate_in_direction_of_movement");

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(new Vector2[]{ new Vector2(3, 0),
                    new Vector2(0, 3),
                    new Vector2(-3, 0),
                    new Vector2(0, -3)
                    }, 0.2f, Color.White, PolyRenderShape.PolyCapStyle.Filled, true),
                new PolyRenderShape(new Vector2[]{ new Vector2(0, 3),
                    new Vector2(-3, -2),
                    new Vector2(-3, 2),
                    new Vector2(0, -3)
                    }, 0.13f, Color.White, PolyRenderShape.PolyCapStyle.Filled)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<VectorSpriteComponent>().ChangeColor(color);
            entity.GetComponent<TransformComponent>().SetScale(CVars.Get<float>("player_ship_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(color));
            entity.AddComponent(new QuadTreeReferenceComponent(new QuadTreeNode(new BoundingRect())));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(3, 0),
                new Vector2(0, 3),
                new Vector2(-3, 2),
                new Vector2(-3, -2),
                new Vector2(0, -3)
            })));
            entity.GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.COLLISION_GROUP_PLAYER;
            entity.GetComponent<CollisionComponent>().CollisionMask = (byte)(Constants.Collision.GROUP_MASK_ALL);

            return entity;
        }
    }
}
