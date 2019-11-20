using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using GameJam.Components;
using System.Collections.Generic;
using System;

namespace GameJam.Systems
{
    public class RenderSystem
    {
        protected Engine Engine
        {
            get;
            private set;
        }

        public static readonly Vector2 FlipY = new Vector2(1, -1);
        public static readonly Vector2 HalfHalf = new Vector2(0.5f, 0.5f);

        readonly Family _spriteFamily = Family.All(typeof(SpriteComponent), typeof(TransformComponent)).Get();
        readonly Family _fontFamily = Family.All(typeof(FontComponent), typeof(TransformComponent)).Get();
        readonly Family _vectorSpriteFamily = Family.All(typeof(VectorSpriteComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _spriteEntities;
        readonly ImmutableList<Entity> _fontEntities;
        readonly ImmutableList<Entity> _vectorSpriteEntities;

        public SpriteBatch SpriteBatch { get; }
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        private BasicEffect _vectorSpriteEffect;
        private Viewport _lastViewport;

        public RenderSystem(GraphicsDevice graphics, Engine engine)
        {
            Engine = engine;
            _spriteEntities = Engine.GetEntitiesFor(_spriteFamily);
            _fontEntities = Engine.GetEntitiesFor(_fontFamily);
            _vectorSpriteEntities = Engine.GetEntitiesFor(_vectorSpriteFamily);

            SpriteBatch = new SpriteBatch(graphics);
            GraphicsDevice = graphics;

            _vectorSpriteEffect = new BasicEffect(GraphicsDevice);
            _vectorSpriteEffect.AmbientLightColor = new Vector3(1, 1, 1);
            _vectorSpriteEffect.World = Matrix.Identity;
            _vectorSpriteEffect.TextureEnabled = false;
            _vectorSpriteEffect.VertexColorEnabled = true;
        }

        public void DrawEntities(float dt, float betweenFrameAlpha)
        {
            DrawEntities(Constants.Render.GROUP_MASK_ALL, dt, betweenFrameAlpha);
        }

        public void DrawEntities(byte groupMask, float dt, float betweenFrameAlpha)
        {
            DrawEntities(Matrix.Identity, groupMask, dt, betweenFrameAlpha);
        }

        public void DrawEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            DrawSpriteBatchEntities(transformMatrix, groupMask, dt, betweenFrameAlpha);
            DrawVectorEntities(transformMatrix, groupMask, dt, betweenFrameAlpha);
        }

        private void DrawSpriteBatchEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                               BlendState.AlphaBlend,
                               SamplerState.PointClamp,
                               null,
                               null,
                               null,
                               transformMatrix);

            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            foreach (Entity entity in _spriteEntities)
            {
                SpriteComponent spriteComp = entity.GetComponent<SpriteComponent>();
                if (spriteComp.Hidden
                    || (spriteComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 position = transformComp.Position
                    + (transformComp.LastPosition - transformComp.Position)
                        * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float rotation = transformComp.Rotation
                    + MathHelper.WrapAngle(transformComp.LastRotation - transformComp.Rotation) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                Vector2 scale = new Vector2(spriteComp.Bounds.X / spriteComp.Texture.Width,
                                            spriteComp.Bounds.Y / spriteComp.Texture.Height);
				float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                Vector2 origin = new Vector2(spriteComp.Texture.Bounds.Width,
                                             spriteComp.Texture.Bounds.Height) * HalfHalf;

                AnimationComponent animationComp = entity.GetComponent<AnimationComponent>();
                if (animationComp != null && animationComp.ActiveAnimationIndex > -1)
                {
                    Rectangle sourceRectangle = animationComp.Animations[animationComp.ActiveAnimationIndex].TextureRegion.Bounds;
                    scale = new Vector2(spriteComp.Bounds.X / sourceRectangle.Width,
                                            spriteComp.Bounds.Y / sourceRectangle.Height);
                    origin = new Vector2(sourceRectangle.Width,
                                             sourceRectangle.Height) * HalfHalf;

                    SpriteBatch.Draw(animationComp.Animations[animationComp.ActiveAnimationIndex].TextureRegion.Texture,
                                      position * FlipY,
                                      sourceRectangle,
                                      spriteComp.Color * spriteComp.Alpha,
                                      -rotation,
                                      origin,
                                      scale * transformScale,
                                      SpriteEffects.None,
                                      0);
                }
                else
                {
                    SpriteBatch.Draw(spriteComp.Texture,
                                      position * FlipY,
                                      null,
                                      spriteComp.Color * spriteComp.Alpha,
                                      -rotation,
                                      origin,
                                      scale * transformScale,
                                      SpriteEffects.None,
                                      0);
                }
            }

            foreach (Entity entity in _fontEntities)
            {
                FontComponent fontComp = entity.GetComponent<FontComponent>();
                if (fontComp.Hidden
                    || (fontComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 position = transformComp.Position
                    + (transformComp.LastPosition - transformComp.Position)
                        * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float rotation = transformComp.Rotation
                    + MathHelper.WrapAngle(transformComp.LastRotation - transformComp.Rotation) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                Vector2 origin = fontComp.Font.MeasureString(fontComp.Content) / 2;

                SpriteBatch.DrawString(fontComp.Font,
                                        fontComp.Content,
                                        position * FlipY,
                                        fontComp.Color,
                                        -rotation,
                                        origin,
                                        transformScale,
                                        SpriteEffects.None,
                                        0);
            }

            SpriteBatch.End();
        }

        private void SetupVectorDrawing()
        {
            Viewport viewport = GraphicsDevice.Viewport;
            if(viewport.Width != _lastViewport.Width || viewport.Height != _lastViewport.Height)
            {
                /** Based on MonoGame SpriteBatch implementation!! **/

                // Normal 3D cameras look into the -z direction (z = 1 is in front of z = 0). The
                // sprite batch layer depth is the opposite (z = 0 is in front of z = 1).
                // --> We get the correct matrix with near plane 0 and far plane -1.
                Matrix projection;
                Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height,
                    0, 0, -1, out projection);
                _vectorSpriteEffect.Projection = projection;

                _lastViewport = viewport;
            }
        }
        private void DrawVectorEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            List<VertexPositionColor> _verts = new List<VertexPositionColor>();

            foreach (Entity entity in _vectorSpriteEntities)
            {
                VectorSpriteComponent vectorSpriteComp = entity.GetComponent<VectorSpriteComponent>();
                if(vectorSpriteComp.Hidden
                    || (vectorSpriteComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 position = transformComp.Position
                    + (transformComp.LastPosition - transformComp.Position)
                        * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;
                position *= FlipY;

                float rotation = transformComp.Rotation
                    + MathHelper.WrapAngle(transformComp.LastRotation - transformComp.Rotation) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;
                rotation *= -1;

                float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float cos = (float)Math.Cos(rotation);
                float sin = (float)Math.Sin(rotation);

                foreach (RenderShape renderShape in vectorSpriteComp.RenderShapes)
                {
                    VertexPositionColor[] verts = renderShape.ComputeVertices();
                    for(int i = verts.Length - 1; i >= 0; i--)
                    {
                        VertexPositionColor vert = verts[i];
                        _verts.Add(new VertexPositionColor(new Vector3((vert.Position.X * cos + vert.Position.Y * -1.0f * -sin) * transformScale + position.X,
                            (vert.Position.X * sin + vert.Position.Y * -1.0f * cos) * transformScale + position.Y, 0), vert.Color * vectorSpriteComp.Alpha));
                    }
                }
            }

            if (_verts.Count > 0)
            {
                SetupVectorDrawing();
                _vectorSpriteEffect.View = transformMatrix;
                GraphicsDevice.BlendState = BlendState.NonPremultiplied;
                foreach (EffectPass pass in _vectorSpriteEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                        _verts.ToArray(), 0, _verts.Count / 3);
                }
            }
        }
    }
}
