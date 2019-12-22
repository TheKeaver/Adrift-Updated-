using System;
using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Systems
{
    public class CollisionDebugRenderSystem
    {
        protected Engine Engine
        {
            get;
            private set;
        }

        public static readonly Vector2 FlipY = new Vector2(1, -1);
        public static readonly Vector2 HalfHalf = new Vector2(0.5f, 0.5f);

        readonly Family _collisionFamily = Family.All(typeof(TransformComponent), typeof(CollisionComponent)).Get();
        readonly ImmutableList<Entity> _collisionEntities;

        public SpriteBatch SpriteBatch { get; }
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        public CollisionDebugRenderSystem(GraphicsDevice graphics, Engine engine)
        {
            Engine = engine;
            _collisionEntities = Engine.GetEntitiesFor(_collisionFamily);

            SpriteBatch = new SpriteBatch(graphics);
            GraphicsDevice = graphics;
        }

        public void Draw(float dt)
        {
            Draw(Matrix.Identity, dt);
        }
        public void Draw(Matrix transformMatrix, float dt)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.Opaque,
                SamplerState.PointClamp,
                null,
                null,
                null,
                transformMatrix);

            foreach (Entity entity in _collisionEntities)
            {
                CollisionComponent collisionComp = entity.GetComponent<CollisionComponent>();
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 position = transformComp.Position * FlipY;


                float cos = (float)Math.Cos(-transformComp.Rotation);
                float sin = (float)Math.Sin(-transformComp.Rotation);
                float scale = transformComp.Scale;

                foreach (CollisionShape shape in collisionComp.CollisionShapes)
                {
                    Vector2 rotatedOffset = rotateAroundOrigin(shape.Offset, cos, sin);

                    BoundingRect AABB = shape.GetAABB(cos, sin, scale);
                    AABB.Min += position;
                    AABB.Max += position;
                    SpriteBatch.DrawRectangle(new Rectangle((int)AABB.Left,
                        (int)AABB.Bottom,
                        (int)AABB.Width,
                        (int)AABB.Height),
                        Color.BlueViolet,
                        1);

                    CircleCollisionShape circleCollisionShape = shape as CircleCollisionShape;
                    if(circleCollisionShape != null)
                    {
                        SpriteBatch.DrawCircle(position + rotatedOffset * scale,
                            circleCollisionShape.Radius * scale,
                            25,
                            Color.YellowGreen);
                    }

                    PolygonCollisionShape polygonCollisionShape = shape as PolygonCollisionShape;
                    if (polygonCollisionShape != null)
                    {
                        for(int i = 0; i < polygonCollisionShape.Vertices.Length; i++)
                        {
                            Vector2 v1 = polygonCollisionShape.Vertices[i] * FlipY;
                            Vector2 v2 = polygonCollisionShape.Vertices[(i + 1) % polygonCollisionShape.Vertices.Length] * FlipY;
                            SpriteBatch.DrawLine(position + (rotatedOffset + rotateAroundOrigin(v1, cos, sin)) * scale,
                                position + (rotatedOffset + rotateAroundOrigin(v2, cos, sin)) * scale,
                                Color.YellowGreen);
                        }
                    }
                }

            }

            SpriteBatch.End();
        }

        private Vector2 rotateAroundOrigin(Vector2 p, float rotation)
        {
            float cos = (float)Math.Cos(rotation);
            float sin = (float)Math.Sin(rotation);
            return rotateAroundOrigin(p, cos, sin);
        }
        private Vector2 rotateAroundOrigin(Vector2 p, float cos, float sin)
        {
            return new Vector2(p.X * cos - p.Y * sin,
                p.X * sin + p.Y * cos);
        }
    }
}
