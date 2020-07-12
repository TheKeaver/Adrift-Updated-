using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameJam.Systems
{
    public class TransformDebugRenderSystem
    {
        protected Engine Engine
        {
            get;
            private set;
        }

        public static readonly Vector2 FlipY = new Vector2(1, -1);
        public static readonly Vector2 HalfHalf = new Vector2(0.5f, 0.5f);

        readonly Family _transformFamily = Family.All(typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _transformEntities;

        public SpriteBatch SpriteBatch { get; }
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        public TransformDebugRenderSystem(GraphicsDevice graphics, Engine engine)
        {
            Engine = engine;
            _transformEntities = Engine.GetEntitiesFor(_transformFamily);

            SpriteBatch = new SpriteBatch(graphics);
            GraphicsDevice = graphics;
        }

        public void Draw(Camera camera, float dt, Camera debugCamera = null)
        {
            Camera drawCamera = camera;
            if (debugCamera != null)
            {
                drawCamera = debugCamera;
            }

            Matrix transformMatrix = drawCamera.GetInterpolatedTransformMatrix(1);
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.Opaque,
                SamplerState.AnisotropicClamp,
                null,
                null,
                null,
                transformMatrix);

            BoundingRect cameraRect = camera.BoundingRect;
            SpriteBatch.DrawRectangle(new Rectangle((int)cameraRect.Min.X,
                -(int)cameraRect.Min.Y,
                (int)cameraRect.Width,
                -(int)cameraRect.Height),
                Color.Orange,
                3);

            foreach (Entity entity in _transformEntities)
            {
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 position = transformComp.Position * FlipY;
                float rotation = transformComp.Rotation * -1;
                float scale = transformComp.Scale;

                SpriteBatch.DrawCircle(position, 10.0f * scale, 20, Color.Cyan);
                SpriteBatch.DrawLine(position, rotateAroundOrigin(position + new Vector2(20, 0), rotation),
                    Color.Cyan,
                    5.0f);
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
