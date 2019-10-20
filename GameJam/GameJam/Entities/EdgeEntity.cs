using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class EdgeEntity
    {
        public static Entity Create(Engine engine, Vector2 position, Vector2 bounds, Vector2 normal)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            float hw = bounds.X / 2;
            float hh = bounds.Y / 2;
            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(hw, hh),
                new Vector2(-hw, hh),
                new Vector2(-hw, -hh),
                new Vector2(hw, -hh)
                })));
            entity.AddComponent(new EdgeComponent(normal));

            RenderShape[] temp = new RenderShape[1];
            temp[0] = new QuadRenderShape(new Vector2(hw, hh), new Vector2(-hw, hh), new Vector2(-hw, -hh), new Vector2(hw, -hh), CVars.Get<Color>("color_playfield"));
            entity.AddComponent(new VectorSpriteComponent(temp));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;

            return entity;
        }
    }
}
