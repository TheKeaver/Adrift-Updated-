using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameJam.Components;
using System.Collections.Generic;
using System;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.TextureAtlases;
using GameJam.Common;
using GameJam.Graphics;

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
        readonly Family _ninePatchFamily = Family.All(typeof(NinePatchComponent), typeof(TransformComponent)).Get();
        readonly Family _fontFamily = Family.All(typeof(BitmapFontComponent), typeof(TransformComponent)).Get();
        readonly Family _fieldFontFamily = Family.All(typeof(FieldFontComponent), typeof(TransformComponent)).Get();
        readonly Family _vectorSpriteFamily = Family.All(typeof(VectorSpriteComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _spriteEntities;
        readonly ImmutableList<Entity> _ninePatchEntities;
        readonly ImmutableList<Entity> _fontEntities;
        readonly ImmutableList<Entity> _fieldFontEntities;
        readonly ImmutableList<Entity> _vectorSpriteEntities;

        private List<VertexPositionColorTexture> _spriteBuffer = new List<VertexPositionColorTexture>();
        private DepthStencilState _spriteDepthStencilState;
        private BlendState _spriteBlendState;
        private SamplerState _spriteSamplerState;
        public FieldFontRenderer FieldFontRenderer { get; }
        private Matrix _fieldFontRendererProjection;
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }


        private BasicEffect _spriteEffect;
        private BasicEffect _vectorSpriteEffect;
        private Viewport _lastViewport;

        public RenderSystem(GraphicsDevice graphics, ContentManager content, Engine engine)
        {
            Engine = engine;
            _spriteEntities = Engine.GetEntitiesFor(_spriteFamily);
            _ninePatchEntities = Engine.GetEntitiesFor(_ninePatchFamily);
            _fontEntities = Engine.GetEntitiesFor(_fontFamily);
            _fieldFontEntities = Engine.GetEntitiesFor(_fieldFontFamily);
            _vectorSpriteEntities = Engine.GetEntitiesFor(_vectorSpriteFamily);

            _spriteDepthStencilState = DepthStencilState.Default;
            _spriteBlendState = BlendState.AlphaBlend;
            _spriteSamplerState = SamplerState.AnisotropicClamp;
            FieldFontRenderer = new FieldFontRenderer(content, graphics);
            GraphicsDevice = graphics;

            _spriteEffect = new BasicEffect(GraphicsDevice);
            _spriteEffect.AmbientLightColor = new Vector3(1, 1, 1);
            _spriteEffect.World = Matrix.Identity;
            _spriteEffect.TextureEnabled = true;
            _spriteEffect.VertexColorEnabled = true;

            _vectorSpriteEffect = new BasicEffect(GraphicsDevice);
            _vectorSpriteEffect.AmbientLightColor = new Vector3(1, 1, 1);
            _vectorSpriteEffect.World = Matrix.Identity;
            _vectorSpriteEffect.TextureEnabled = false;
            _vectorSpriteEffect.VertexColorEnabled = true;
        }

        public void DrawEntities(Camera camera, float dt, float betweenFrameAlpha, Camera debugCamera = null)
        {
            DrawEntities(camera, Constants.Render.GROUP_MASK_ALL, dt, betweenFrameAlpha, debugCamera);
        }

        public void DrawEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha, Camera debugCamera = null)
        {
            int enableFrameSmoothing = CVars.Get<bool>("graphics_frame_smoothing") ? 1 : 0;
            betweenFrameAlpha = betweenFrameAlpha * enableFrameSmoothing + (1 - enableFrameSmoothing);

            DrawSpriteBatchEntities(camera, groupMask, dt, betweenFrameAlpha, debugCamera);
            DrawVectorEntities(camera, groupMask, dt, betweenFrameAlpha, debugCamera);
            DrawFieldFontEntities(camera, groupMask, dt, betweenFrameAlpha, debugCamera);
        }

        private void DrawSpriteBatchEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha, Camera debugCamera)
        {
            if (_spriteEntities.Count == 0 && _fontEntities.Count == 0 && _ninePatchEntities.Count == 0)
            {
                return;
            }
            if (_spriteEntities.Count > 0 || _fontEntities.Count > 0 || _ninePatchEntities.Count > 0)
            {
                CheckUpdateProjections();
            }

            GraphicsDevice.DepthStencilState = _spriteDepthStencilState;
            GraphicsDevice.BlendState = _spriteBlendState;
            GraphicsDevice.SamplerStates[0] = _spriteSamplerState;
            _spriteEffect.View = transformMatrix;

            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            Texture2D currentTexture = null;

            Matrix transformMatrix = debugCamera == null ? camera.GetInterpolatedTransformMatrix(betweenFrameAlpha) : debugCamera.GetInterpolatedTransformMatrix(betweenFrameAlpha);
            SpriteBatch.Begin(SpriteSortMode.Deferred,
                               BlendState.AlphaBlend,
                               SamplerState.AnisotropicClamp,
                               null,
                               null,
                               null,
                               transformMatrix);

            foreach (Entity entity in _spriteEntities)
            {
                SpriteComponent spriteComp = entity.GetComponent<SpriteComponent>();
                if (spriteComp.Hidden
                    || (spriteComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                BoundingRect boundRect = spriteComp.GetAABB(transformComp.Scale);
                boundRect.Min += transformComp.Position;
                boundRect.Max += transformComp.Position;

                if (!boundRect.Intersects(camera.BoundingRect) && CVars.Get<bool>("debug_show_render_culling"))
                {
                    continue;
                }

                Vector2 position;
                float rotation;
                float transformScale;
                transformComp.Interpolate(betweenFrameAlpha, out position, out rotation, out transformScale);

                Vector2 scale = new Vector2(spriteComp.Bounds.X / spriteComp.Texture.Width,
                                            spriteComp.Bounds.Y / spriteComp.Texture.Height);

                Vector2 origin = new Vector2(spriteComp.Texture.Bounds.Width,
                                                spriteComp.Texture.Bounds.Height) * HalfHalf;

                AnimationComponent animationComp = entity.GetComponent<AnimationComponent>();
                if (animationComp != null && animationComp.ActiveAnimationIndex > -1)
                {
                    textureRegion = animationComp.Animations[animationComp.ActiveAnimationIndex].TextureRegion;
                }

                if(currentTexture == null)
                {
                    currentTexture = textureRegion.Texture;
                }
                if(currentTexture != textureRegion.Texture)
                {
                    FlushSpriteArray(currentTexture);
                    currentTexture = textureRegion.Texture;
                }

                Rectangle texRegionSourceBounds = textureRegion.Bounds;
                float umin = texRegionSourceBounds.X / (float)textureRegion.Texture.Width,
                    vmin = texRegionSourceBounds.Y / (float)textureRegion.Texture.Height,
                    umax = (texRegionSourceBounds.X + texRegionSourceBounds.Width) / (float)textureRegion.Texture.Width,
                    vmax = (texRegionSourceBounds.Y + texRegionSourceBounds.Height) / (float)textureRegion.Texture.Height;

                VertexPositionColorTexture v1 = new VertexPositionColorTexture()
                {
                    Position = new Vector3(RotateVector(new Vector2(position.X + spriteComp.Bounds.X * transformScale / 2, position.Y - spriteComp.Bounds.Y * transformScale / 2), cos, sin), spriteComp.Depth),
                    Color = spriteComp.Color * spriteComp.Alpha,
                    TextureCoordinate = new Vector2(umax, vmax)
                };
                VertexPositionColorTexture v2 = new VertexPositionColorTexture()
                {
                    Position = new Vector3(RotateVector(new Vector2(position.X - spriteComp.Bounds.X * transformScale / 2, position.Y - spriteComp.Bounds.Y * transformScale / 2), cos, sin), spriteComp.Depth),
                    Color = spriteComp.Color * spriteComp.Alpha,
                    TextureCoordinate = new Vector2(umin, vmax)
                };
                VertexPositionColorTexture v3 = new VertexPositionColorTexture()
                {
                    Position = new Vector3(RotateVector(new Vector2(position.X - spriteComp.Bounds.X * transformScale / 2, position.Y + spriteComp.Bounds.Y * transformScale / 2), cos, sin), spriteComp.Depth),
                    Color = spriteComp.Color * spriteComp.Alpha,
                    TextureCoordinate = new Vector2(umin, vmin)
                };
                VertexPositionColorTexture v4 = new VertexPositionColorTexture()
                {
                    Position = new Vector3(RotateVector(new Vector2(position.X + spriteComp.Bounds.X * transformScale / 2, position.Y + spriteComp.Bounds.Y * transformScale / 2), cos, sin), spriteComp.Depth),
                    Color = spriteComp.Color * spriteComp.Alpha,
                    TextureCoordinate = new Vector2(umax, vmin)
                };

                _spriteBuffer.Add(v1);
                _spriteBuffer.Add(v2);
                _spriteBuffer.Add(v3);

                _spriteBuffer.Add(v3);
                _spriteBuffer.Add(v4);
                _spriteBuffer.Add(v1);
            }

            foreach (Entity entity in _ninePatchEntities)
            {
                NinePatchComponent ninePatchComp = entity.GetComponent<NinePatchComponent>();
                if (ninePatchComp.Hidden
                    || (ninePatchComp.RenderGroup & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                Vector2 position = transformComp.Position
                    + (transformComp.LastPosition - transformComp.Position)
                        * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                // TODO: NinePatch's can't be rotated
                float rotation = 0;

                float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float cos = (float)Math.Cos(rotation),
                    sin = (float)Math.Sin(rotation);

                Rectangle[] destinationPatches = ninePatchComp.NinePatch.CreatePatches(new Rectangle((int)position.X,
                    (int)position.Y,
                    (int)(ninePatchComp.Bounds.X * transformScale),
                    (int)(ninePatchComp.Bounds.Y * transformScale)));
                Rectangle[] sourcePatches = ninePatchComp.NinePatch.SourcePatches;

                if (currentTexture == null)
                {
                    currentTexture = ninePatchComp.NinePatch.Texture;
                }
                if (currentTexture != ninePatchComp.NinePatch.Texture)
                {
                    FlushSpriteArray(currentTexture);
                    currentTexture = ninePatchComp.NinePatch.Texture;
                }

                for (int i = 0; i < sourcePatches.Length; i++)
                {
                    Rectangle texRegionSourceBounds = sourcePatches[i];
                    float umin = texRegionSourceBounds.X / (float)ninePatchComp.NinePatch.Texture.Width,
                        vmin = texRegionSourceBounds.Y / (float)ninePatchComp.NinePatch.Texture.Height,
                        umax = (texRegionSourceBounds.X + texRegionSourceBounds.Width) / (float)ninePatchComp.NinePatch.Texture.Width,
                        vmax = (texRegionSourceBounds.Y + texRegionSourceBounds.Height) / (float)ninePatchComp.NinePatch.Texture.Height;

                    Vector2 patchPosition = position - ninePatchComp.Bounds / 2 + new Vector2(destinationPatches[i].X, destinationPatches[i].Y);

                    VertexPositionColorTexture v1 = new VertexPositionColorTexture()
                    {
                        Position = new Vector3(RotateVector(new Vector2(patchPosition.X + destinationPatches[i].Width * transformScale, patchPosition.Y), cos, sin), ninePatchComp.Depth),
                        Color = ninePatchComp.Color * ninePatchComp.Alpha,
                        TextureCoordinate = new Vector2(umax, vmax)
                    };
                    VertexPositionColorTexture v2 = new VertexPositionColorTexture()
                    {
                        Position = new Vector3(RotateVector(new Vector2(patchPosition.X, patchPosition.Y), cos, sin), ninePatchComp.Depth),
                        Color = ninePatchComp.Color * ninePatchComp.Alpha,
                        TextureCoordinate = new Vector2(umin, vmax)
                    };
                    VertexPositionColorTexture v3 = new VertexPositionColorTexture()
                    {
                        Position = new Vector3(RotateVector(new Vector2(patchPosition.X, patchPosition.Y + destinationPatches[i].Height * transformScale / 1), cos, sin), ninePatchComp.Depth),
                        Color = ninePatchComp.Color * ninePatchComp.Alpha,
                        TextureCoordinate = new Vector2(umin, vmin)
                    };
                    VertexPositionColorTexture v4 = new VertexPositionColorTexture()
                    {
                        Position = new Vector3(RotateVector(new Vector2(patchPosition.X + destinationPatches[i].Width * transformScale, patchPosition.Y + destinationPatches[i].Height * transformScale / 1), cos, sin), ninePatchComp.Depth),
                        Color = ninePatchComp.Color * ninePatchComp.Alpha,
                        TextureCoordinate = new Vector2(umax, vmin)
                    };

                    _spriteBuffer.Add(v1);
                    _spriteBuffer.Add(v2);
                    _spriteBuffer.Add(v3);

                    _spriteBuffer.Add(v3);
                    _spriteBuffer.Add(v4);
                    _spriteBuffer.Add(v1);
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
                BoundingRect boundRect = new BoundingRect(transformComp.Position.X - (fontComp.Font.MeasureString(fontComp.Content).Width * transformComp.Scale /2),
                                                            transformComp.Position.Y - (fontComp.Font.MeasureString(fontComp.Content).Height * transformComp.Scale /2),
                                                            fontComp.Font.MeasureString(fontComp.Content).Width * transformComp.Scale,
                                                            fontComp.Font.MeasureString(fontComp.Content).Height * transformComp.Scale);

                if (!boundRect.Intersects(camera.BoundingRect) && CVars.Get<bool>("debug_show_render_culling"))
                {
                    continue;
                }

                Vector2 position;
                float rotation;
                float transformScale;
                transformComp.Interpolate(betweenFrameAlpha, out position, out rotation, out transformScale);

                Vector2 bounds = fontComp.Font.MeasureString(fontComp.Content);

                var glyphs = fontComp.Font.GetGlyphs(fontComp.Content);
                foreach(var glyph in glyphs)
                {
                    if(glyph.FontRegion == null)
                    {
                        continue;
                    }

                    TextureRegion2D texture = glyph.FontRegion.TextureRegion;

                    if (currentTexture == null)
                    {
                        currentTexture = texture.Texture;
                    }
                    if (currentTexture != texture.Texture)
                    {
                        FlushSpriteArray(currentTexture);
                        currentTexture = texture.Texture;
                    }

                    Rectangle texRegionSourceBounds = texture.Bounds;
                    float umin = texRegionSourceBounds.X / (float)texture.Texture.Width,
                        vmin = texRegionSourceBounds.Y / (float)texture.Texture.Height,
                        umax = (texRegionSourceBounds.X + texRegionSourceBounds.Width) / (float)texture.Texture.Width,
                        vmax = (texRegionSourceBounds.Y + texRegionSourceBounds.Height) / (float)texture.Texture.Height;

                    Vector2 characterOrigin = position - bounds * transformScale / 2 - glyph.Position * transformScale; // TODO

                    VertexPositionColorTexture v1 = new VertexPositionColorTexture()
                    {
                        Position = new Vector3(RotateVector(new Vector2(characterOrigin.X + texture.Bounds.X * transformScale / 2, characterOrigin.Y - texture.Bounds.Y * transformScale / 2), cos, sin), fontComp.Depth),
                        Color = fontComp.Color,
                        TextureCoordinate = new Vector2(umax, vmax)
                    };
                    VertexPositionColorTexture v2 = new VertexPositionColorTexture()
                    {
                        Position = new Vector3(RotateVector(new Vector2(characterOrigin.X - texture.Bounds.X * transformScale / 2, characterOrigin.Y - texture.Bounds.Y * transformScale / 2), cos, sin), fontComp.Depth),
                        Color = fontComp.Color,
                        TextureCoordinate = new Vector2(umin, vmax)
                    };
                    VertexPositionColorTexture v3 = new VertexPositionColorTexture()
                    {
                        Position = new Vector3(RotateVector(new Vector2(characterOrigin.X - texture.Bounds.X * transformScale / 2, characterOrigin.Y + texture.Bounds.Y * transformScale / 2), cos, sin), fontComp.Depth),
                        Color = fontComp.Color,
                        TextureCoordinate = new Vector2(umin, vmin)
                    };
                    VertexPositionColorTexture v4 = new VertexPositionColorTexture()
                    {
                        Position = new Vector3(RotateVector(new Vector2(characterOrigin.X + texture.Bounds.X * transformScale / 2, characterOrigin.Y + texture.Bounds.Y * transformScale / 2), cos, sin), fontComp.Depth),
                        Color = fontComp.Color,
                        TextureCoordinate = new Vector2(umax, vmin)
                    };

                    _spriteBuffer.Add(v1);
                    _spriteBuffer.Add(v2);
                    _spriteBuffer.Add(v3);

                    _spriteBuffer.Add(v3);
                    _spriteBuffer.Add(v4);
                    _spriteBuffer.Add(v1);
                }
            }

            FlushSpriteArray(currentTexture);
        }
        private void FlushSpriteArray(Texture2D currentTexture)
        {
            if(_spriteBuffer.Count == 0)
            {
                return;
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
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            _spriteEffect.Texture = currentTexture;
            foreach(EffectPass pass in _spriteEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                    _spriteBuffer.ToArray(),
                    0,
                    _spriteBuffer.Count / 3);
            }
            _spriteBuffer.Clear();
        }

        private void DrawFieldFontEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha, Camera debugCamera)
        {
            Matrix transformMatrix = debugCamera == null ? camera.GetInterpolatedTransformMatrix(betweenFrameAlpha) : debugCamera.GetInterpolatedTransformMatrix(betweenFrameAlpha);

            if (_fieldFontEntities.Count > 0)
            {
                CheckUpdateProjections();
                Matrix wvp = transformMatrix * _fieldFontRendererProjection;
                FieldFontRenderer.Begin(wvp);
                GraphicsDevice.DepthStencilState = _spriteDepthStencilState;
                foreach (Entity entity in _fieldFontEntities)
                {
                    FieldFontComponent fieldFontComp = entity.GetComponent<FieldFontComponent>();
                    if (fieldFontComp.Hidden || (fieldFontComp.RenderGroup & groupMask) == 0)
                    {
                        continue;
                    }

                    TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                    BoundingRect boundRect = new BoundingRect(transformComp.Position.X - (fieldFontComp.Font.MeasureString(fieldFontComp.Content).X * transformComp.Scale/2),
                                                                transformComp.Position.Y - (fieldFontComp.Font.MeasureString(fieldFontComp.Content).Y * transformComp.Scale/2),
                                                                fieldFontComp.Font.MeasureString(fieldFontComp.Content).X * transformComp.Scale,
                                                                fieldFontComp.Font.MeasureString(fieldFontComp.Content).Y * transformComp.Scale);

                    if (!boundRect.Intersects(camera.BoundingRect) && CVars.Get<bool>("debug_show_render_culling"))
                    {
                        continue;
                    }

                    Vector2 position;
                    float rotation;
                    float transformScale;
                    transformComp.Interpolate(betweenFrameAlpha, out position, out rotation, out transformScale);

                    FieldFontRenderer.Draw(fieldFontComp.Font,
                        fieldFontComp.Content,
                        position,
                        rotation,
                        fieldFontComp.Color * fieldFontComp.Alpha,
                        transformScale,
                        fieldFontComp.EnableKerning,
                        fieldFontComp.Depth);
                }
                FieldFontRenderer.End();
            }
        }


        private void DrawVectorEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha, Camera debugCamera)
        {
            Matrix transformMatrix = debugCamera == null ? camera.GetInterpolatedTransformMatrix(betweenFrameAlpha) : debugCamera.GetInterpolatedTransformMatrix(betweenFrameAlpha);

            List<VertexPositionColor> _verts = new List<VertexPositionColor>();

            GraphicsDevice.DepthStencilState = _spriteDepthStencilState;
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
                    continue;
                }

                Vector2 position;
                float rotation;
                float transformScale;
                transformComp.Interpolate(betweenFrameAlpha, out position, out rotation, out transformScale);

                position *= FlipY;
                rotation *= -1;

                int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;
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
                            (vert.Position.X * stretch.X * sin + vert.Position.Y * stretch.Y * -1.0f * cos) * transformScale + position.Y, vectorSpriteComp.Depth),
                            new Color(vert.Color.ToVector4() * renderShape.TintColor.ToVector4() * vectorSpriteComp.Alpha)));
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
                SetupVectorAndSpriteDrawing(viewport);

                _lastViewport = viewport;
            }
        }
        private void SetupFieldFontDrawing(Viewport viewport)
        {
            Matrix.CreateOrthographicOffCenter(0, viewport.Width, 0,
                viewport.Height, -float.MaxValue, float.MaxValue, out _fieldFontRendererProjection);
        }
        private void SetupVectorAndSpriteDrawing(Viewport viewport)
        {
            Matrix projection;
            Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height,
                0, -float.MaxValue, float.MaxValue, out projection);
            _vectorSpriteEffect.Projection = projection;
            _spriteEffect.Projection = projection;

            _lastViewport = viewport;
        }
        #endregion

        private Vector2 RotateVector(Vector2 vector, float cos, float sin)
        {
            return new Vector2(vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos);
        }
    }
}
