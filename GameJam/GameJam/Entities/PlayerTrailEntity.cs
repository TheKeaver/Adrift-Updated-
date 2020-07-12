using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public class PlayerTrailEntity
    {
        public static Entity Create(Engine engine, Entity shipEntity)
        {
            Entity entity = engine.CreateEntity();
            Vector2 offset = new Vector2(-3, 0);

            entity.AddComponent(new TransformComponent(shipEntity.GetComponent<TransformComponent>().Position + offset));
            offset = entity.GetComponent<TransformComponent>().Position;
            entity.AddComponent(new VectorSpriteComponent(
                new RenderShape[]
                {
                    // Offset already accounts for the distance to the back of the ship
                    // To make the trail connect directly to the ship simply set the
                    // 1st and 2nd rows to x = 0
                    // EX: ' offset + new Vector2(0,-1)
                    new QuadRenderShape(offset + new Vector2(0,1),
                        offset + new Vector2(0,-1),
                        offset + new Vector2(-5,-1),
                        offset + new Vector2(-5,1),
                        shipEntity.GetComponent<ColoredExplosionComponent>().Color)
                }));
            entity.AddComponent(new EntityMirroringComponent(shipEntity, true, true));

            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_NO_GLOW;
            entity.GetComponent<TransformComponent>().SetScale(CVars.Get<float>("player_ship_size"), true);

            return entity;
        }
    }
}
