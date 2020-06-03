using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class PlayerShieldEntity
    {
        public static Entity Create(Engine engine, Entity shipEntity, float angle, bool isActive)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent());
            entity.AddComponent(new PlayerShieldComponent(shipEntity, angle, CVars.Get<float>("player_shield_radius"), isActive));

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new QuadRenderShape(new Vector2(6, -1),
                    new Vector2(6, 1),
                    new Vector2(-6, 1),
                    new Vector2(-6, -1),
                    Color.White)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<VectorSpriteComponent>().ChangeColor(CVars.Get<Color>("color_player_shield_high"));
            entity.GetComponent<VectorSpriteComponent>().Hidden = !isActive;
            entity.GetComponent<TransformComponent>().SetScale(CVars.Get<float>("player_shield_size"), true);
            entity.AddComponent(new QuadTreeReferenceComponent(new QuadTreeNode(new BoundingRect())));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(6, -1),
                new Vector2(6, 1),
                new Vector2(-6, 1),
                new Vector2(-6, -1)
            })));
            entity.GetComponent<CollisionComponent>().CollisionGroup = 
                (isActive) ? Constants.Collision.COLLISION_GROUP_PLAYER : Constants.Collision.GROUP_MASK_NONE;
            entity.GetComponent<CollisionComponent>().CollisionMask = 
                (isActive) ? (byte)(Constants.Collision.GROUP_MASK_ALL & ~Constants.Collision.COLLISION_GROUP_PLAYER) :
                             (byte)(Constants.Collision.GROUP_MASK_NONE);

            return entity;
        }
    }
}
