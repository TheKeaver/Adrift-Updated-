using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class PlayerShieldEntity
    {
        public static Entity Create(Engine engine, Entity shipEntity)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent());
            entity.AddComponent(new PlayerShieldComponent(shipEntity));

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new QuadRenderShape(new Vector2(6, -1),
                    new Vector2(6, 1),
                    new Vector2(-6, 1),
                    new Vector2(-6, -1),
                    Color.White)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<VectorSpriteComponent>().ChangeColor(CVars.Get<Color>("color_player_shield_high"));
            entity.GetComponent<VectorSpriteComponent>().Depth = Constants.Render.RENDER_DEPTH_LAYER_SPRITES_GAMEPLAY;
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("player_shield_size"), true);

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(6, -1),
                new Vector2(6, 1),
                new Vector2(-6, 1),
                new Vector2(-6, -1)
            })));
            entity.GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.COLLISION_GROUP_PLAYER;
            entity.GetComponent<CollisionComponent>().CollisionMask = (byte)(Constants.Collision.GROUP_MASK_ALL & ~Constants.Collision.COLLISION_GROUP_PLAYER);

            return entity;
        }
    }
}
