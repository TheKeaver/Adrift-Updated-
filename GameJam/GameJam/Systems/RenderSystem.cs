using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using GameJam.Components;
using System.Collections.Generic;
using System;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using GameJam.Common;

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
        readonly Family _fontFamily = Family.All(typeof(BitmapFontComponent), typeof(TransformComponent)).Get();
        readonly Family _fieldFontFamily = Family.All(typeof(FieldFontComponent), typeof(TransformComponent)).Get();
        readonly Family _vectorSpriteFamily = Family.All(typeof(VectorSpriteComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _spriteEntities;
        readonly ImmutableList<Entity> _fontEntities;
        readonly ImmutableList<Entity> _fieldFontEntities;
        readonly ImmutableList<Entity> _vectorSpriteEntities;

        public SpriteBatch SpriteBatch { get; }
        public FieldFontRenderer FieldFontRenderer { get; }
        private Matrix _fieldFontRendererProjection;
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        private BasicEffect _vectorSpriteEffect;
        private Viewport _lastViewport;

        public RenderSystem(GraphicsDevice graphics, ContentManager content, Engine engine)
        {
            Engine = engine;
            _spriteEntities = Engine.GetEntitiesFor(_spriteFamily);
            _fontEntities = Engine.GetEntitiesFor(_fontFamily);
            _fieldFontEntities = Engine.GetEntitiesFor(_fieldFontFamily);
            _vectorSpriteEntities = Engine.GetEntitiesFor(_vectorSpriteFamily);

            SpriteBatch = new SpriteBatch(graphics);
            FieldFontRenderer = new FieldFontRenderer(content, graphics);
            GraphicsDevice = graphics;

            _vectorSpriteEffect = new BasicEffect(GraphicsDevice);
            _vectorSpriteEffect.AmbientLightColor = new Vector3(1, 1, 1);
            _vectorSpriteEffect.World = Matrix.Identity;
            _vectorSpriteEffect.TextureEnabled = false;
            _vectorSpriteEffect.VertexColorEnabled = true;
        }

        public void DrawEntities(Camera camera, float dt, float betweenFrameAlpha)
        {
            DrawEntities(camera, Constants.Render.GROUP_MASK_ALL, dt, betweenFrameAlpha);
        }

        public void DrawEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha)
        {
            DrawSpriteBatchEntities(camera, groupMask, dt, betweenFrameAlpha);
            DrawVectorEntities(camera, groupMask, dt, betweenFrameAlpha);
            DrawFieldFontEntities(camera, groupMask, dt, betweenFrameAlpha);
        }

        private void DrawSpriteBatchEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha)
        {
            Matrix transformMatrix = camera.TransformMatrix;
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                               BlendState.AlphaBlend,
                               SamplerState.AnisotropicClamp,
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
                BoundingRect boundRect = new BoundingRect(transformComp.Position.X, transformComp.Position.Y,
                                                            spriteComp.Bounds.X * transformComp.Scale,
                                                            spriteComp.Bounds.Y * transformComp.Scale);

                if (!boundRect.Intersects(camera.BoundingRect))
                {
                    continue;
                }
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
                BitmapFontComponent fontComp = entity.GetComponent<BitmapFontComponent>();
                if (fontComp.Hidden
                    || (fontComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                BoundingRect boundRect = new BoundingRect(transformComp.Position.X, transformComp.Position.Y,
                                                            fontComp.Font.MeasureString(fontComp.Content).Width * transformComp.Scale,
                                                            fontComp.Font.MeasureString(fontComp.Content).Height * transformComp.Scale);

                if (!boundRect.Intersects(camera.BoundingRect))
                {
                    continue;
                }

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

        private void DrawFieldFontEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha)
        {
            Matrix transformMatrix = camera.TransformMatrix;
            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            if (_fieldFontEntities.Count > 0)
            {
                CheckUpdateProjections();
                Matrix wvp = transformMatrix * _fieldFontRendererProjection;
                FieldFontRenderer.Begin(wvp);
                foreach (Entity entity in _fieldFontEntities)
                {
                    FieldFontComponent fieldFontComp = entity.GetComponent<FieldFontComponent>();
                    if (fieldFontComp.Hidden || (fieldFontComp.RenderGroup & groupMask) == 0)
                    {
                        continue;
                    }

                    TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                    BoundingRect boundRect = new BoundingRect(transformComp.Position.X, transformComp.Position.Y,
                                                                fieldFontComp.Font.MeasureString(fieldFontComp.Content).X * transformComp.Scale,
                                                                fieldFontComp.Font.MeasureString(fieldFontComp.Content).Y * transformComp.Scale);

                    if (!boundRect.Intersects(camera.BoundingRect))
                    {
                        continue;
                    }

                    Vector2 position = transformComp.Position
                        + (transformComp.LastPosition - transformComp.Position)
                            * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                    float rotation = transformComp.Rotation
                        + MathHelper.WrapAngle(transformComp.LastRotation - transformComp.Rotation) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                    float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                    FieldFontRenderer.Draw(fieldFontComp.Font,
                        fieldFontComp.Content,
                        position,
                        rotation,
                        fieldFontComp.Color,
                        transformScale,
                        fieldFontComp.EnableKerning);
                }
                FieldFontRenderer.End();
            }
        }


        private void DrawVectorEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha)
        {
            Matrix transformMatrix = camera.TransformMatrix;
            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            List<VertexPositionColor> _verts = new List<VertexPositionColor>();

            foreach (Entity entity in _vectorSpriteEntities)
            {
                VectorSpriteComponent vectorSpriteComp = entity.GetComponent<VectorSpriteComponent>();
                if (vectorSpriteComp.Hidden
                    || (vectorSpriteComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                BoundingRect boundRect = vectorSpriteComp.GetAABB(transformComp.Scale);
                boundRect.Min += transformComp.Position;
                boundRect.Max += transformComp.Position;

                if (!boundRect.Intersects(camera.BoundingRect) && CVars.Get<bool>("debug_show_render_culling"))
                {
                    Console.WriteLine("Render culled, object off screen");
                    continue;
                }

                Vector2 position = transformComp.Position
                    + (transformComp.LastPosition - transformComp.Position)
                        * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;
                position *= FlipY;

                float rotation = transformComp.Rotation
                    + MathHelper.WrapAngle(transformComp.LastRotation - transformComp.Rotation) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;
                rotation *= -1;

                float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                Vector2 stretch = vectorSpriteComp.Stretch + (vectorSpriteComp.LastStretch - vectorSpriteComp.Stretch) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float cos = (float)Math.Cos(rotation);
                float sin = (float)Math.Sin(rotation);

                foreach (RenderShape renderShape in vectorSpriteComp.RenderShapes)
                {
                    VertexPositionColor[] verts = renderShape.ComputeVertices();
                    for (int i = verts.Length - 1; i >= 0; i--)
                    {
                        VertexPositionColor vert = verts[i];
                        _verts.Add(new VertexPositionColor(new Vector3((vert.Position.X * stretch.X * cos + vert.Position.Y * stretch.Y * -1.0f * -sin) * transformScale + position.X,
                            (vert.Position.X * stretch.X * sin + vert.Position.Y * stretch.Y * -1.0f * cos) * transformScale + position.Y, 0), new Color(vert.Color.ToVector4() * renderShape.TintColor.ToVector4() * vectorSpriteComp.Alpha)));
                    }
                }
            }

            if (_verts.Count > 0)
            {
                CheckUpdateProjections();
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

        #region PROJECTION_UPDATES
        private void CheckUpdateProjections()
        {
            Viewport viewport = GraphicsDevice.Viewport;
            if (viewport.Width != _lastViewport.Width || viewport.Height != _lastViewport.Height)
            {
                SetupFieldFontDrawing(viewport);
                SetupVectorDrawing(viewport);

                _lastViewport = viewport;
            }
        }
        private void SetupFieldFontDrawing(Viewport viewport)
        {
            /** Based on MonoGame SpriteBatch implementation!! **/

            // Normal 3D cameras look into the -z direction (z = 1 is in front of z = 0). The
            // sprite batch layer depth is the opposite (z = 0 is in front of z = 1).
            // --> We get the correct matrix with near plane 0 and far plane -1.
            Matrix.CreateOrthographicOffCenter(0, viewport.Width, 0,
                viewport.Height, -1, 1, out _fieldFontRendererProjection);
        }
        private void SetupVectorDrawing(Viewport viewport)
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
        #endregion
    }
}
