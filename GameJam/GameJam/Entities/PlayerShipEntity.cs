using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Entities
{
    public static class PlayerShipEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position)
        {
            return Create(engine, texture, position, Color.White);
        }

        public static Entity Create(Engine engine, Texture2D texture, Vector2 position, Color color)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.GetComponent<TransformComponent>().ChangeScale(5, true);

            entity.AddComponent(new MovementComponent());
            entity.AddComponent(new PlayerShipComponent(CVars.Get<int>("player_ship_max_health")));
            entity.AddComponent(new BounceComponent());
            entity.AddComponent(new CollisionComponent(new BoundingRect(0, 0, Constants.ObjectBounds.PLAYER_SHIP_BOUNDS.X, Constants.ObjectBounds.PLAYER_SHIP_BOUNDS.Y)));

            entity.GetComponent<MovementComponent>().UpdateRotationWithDirection = false;

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                // Top Right
                new QuadRenderShape(new Vector2(3, 0), new Vector2(3.5f, 0),
                    new Vector2(0, 3.5f), new Vector2(0, 3),
                    Color.White),
                // Bottom Right
                new QuadRenderShape(new Vector2(3, 0), new Vector2(0, -3),
                    new Vector2(0, -3.5f), new Vector2(3.5f, 0),
                    Color.White),
                // Top Left
                new QuadRenderShape(new Vector2(0, 3), new Vector2(0, 3.5f),
                    new Vector2(-3.5f, 0), new Vector2(-3, 0),
                    Color.White),
                // Bottom Left
                new QuadRenderShape(new Vector2(0, -3.5f), new Vector2(0, -3),
                    new Vector2(-3, 0), new Vector2(-3.5f, 0),
                    Color.White),
                // Tails
                new QuadRenderShape(new Vector2(0, 3), new Vector2(0, 3.5f),
                    new Vector2(-3.3f, -2f), new Vector2(-3.3f, -2.5f),
                    Color.White),
                new QuadRenderShape(new Vector2(0, -3.5f), new Vector2(0, -3),
                    new Vector2(-3.3f, 2.5f), new Vector2(-3.5f, 2),
                    Color.White),
                new QuadRenderShape(new Vector2(-3.5f, -2.5f), new Vector2(-3.3f, -2.5f),
                    new Vector2(-3.3f, 2.5f), new Vector2(-3.5f, 2.5f),
                    Color.White)
            }));

            return entity;
        }
    }
}
