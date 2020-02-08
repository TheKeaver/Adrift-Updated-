using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameJam.Components;
using System.Collections.Generic;
using System;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.TextureAtlases;

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
            DrawFieldFontEntities(transformMatrix, groupMask, dt, betweenFrameAlpha);
        }

        private void DrawSpriteBatchEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            if (_spriteEntities.Count == 0 && _fontEntities.Count == 0)
            {
                return;
            }
            if (_spriteEntities.Count > 0 || _fontEntities.Count > 0)
            {
                CheckUpdateProjections();
            }

            //SpriteBatch.Begin(SpriteSortMode.Deferred,
            //                   BlendState.AlphaBlend,
            //                   SamplerState.AnisotropicClamp,
            //                   _spriteBatchDepthStencilState,
            //                   null,
            //                   _spriteEffect,
            //                   transformMatrix);
            GraphicsDevice.DepthStencilState = _spriteDepthStencilState;
            GraphicsDevice.BlendState = _spriteBlendState;
            GraphicsDevice.SamplerStates[0] = _spriteSamplerState;
            _spriteEffect.View = transformMatrix;

            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            Texture2D currentTexture = null;

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

                float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float cos = (float)Math.Cos(rotation),
                    sin = (float)Math.Sin(rotation);

                TextureRegion2D textureRegion = spriteComp.Texture;

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
                    Color = spriteComp.Color,
                    TextureCoordinate = new Vector2(umax, vmax)
                };
                VertexPositionColorTexture v2 = new VertexPositionColorTexture()
                {
                    Position = new Vector3(RotateVector(new Vector2(position.X - spriteComp.Bounds.X * transformScale / 2, position.Y - spriteComp.Bounds.Y * transformScale / 2), cos, sin), spriteComp.Depth),
                    Color = spriteComp.Color,
                    TextureCoordinate = new Vector2(umin, vmax)
                };
                VertexPositionColorTexture v3 = new VertexPositionColorTexture()
                {
                    Position = new Vector3(RotateVector(new Vector2(position.X - spriteComp.Bounds.X * transformScale / 2, position.Y + spriteComp.Bounds.Y * transformScale / 2), cos, sin), spriteComp.Depth),
                    Color = spriteComp.Color,
                    TextureCoordinate = new Vector2(umin, vmin)
                };
                VertexPositionColorTexture v4 = new VertexPositionColorTexture()
                {
                    Position = new Vector3(RotateVector(new Vector2(position.X + spriteComp.Bounds.X * transformScale / 2, position.Y + spriteComp.Bounds.Y * transformScale / 2), cos, sin), spriteComp.Depth),
                    Color = spriteComp.Color,
                    TextureCoordinate = new Vector2(umax, vmin)
                };

                _spriteBuffer.Add(v1);
                _spriteBuffer.Add(v2);
                _spriteBuffer.Add(v3);

                _spriteBuffer.Add(v3);
                _spriteBuffer.Add(v4);
                _spriteBuffer.Add(v1);
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

                Vector2 position = transformComp.Position
                    + (transformComp.LastPosition - transformComp.Position)
                        * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float rotation = transformComp.Rotation
                    + MathHelper.WrapAngle(transformComp.LastRotation - transformComp.Rotation) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;
                float cos = (float)Math.Cos(rotation),
                    sin = (float)Math.Sin(rotation);

                float transformScale = transformComp.Scale + (transformComp.LastScale - transformComp.Scale) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

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

        private void DrawFieldFontEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            if (_fieldFontEntities.Count > 0) {
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
                        fieldFontComp.Color * fieldFontComp.Alpha,
                        transformScale,
                        fieldFontComp.EnableKerning,
                        fieldFontComp.Depth);
                }
                FieldFontRenderer.End();
            }
        }

        
        private void DrawVectorEntities(Matrix transformMatrix, byte groupMask, float dt, float betweenFrameAlpha)
        {
            int enableFrameSmoothingFlag = CVars.Get<bool>("graphics_frame_smoothing") ? 0 : 1;

            List<VertexPositionColor> _verts = new List<VertexPositionColor>();

            GraphicsDevice.DepthStencilState = _spriteDepthStencilState;
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

                Vector2 stretch = vectorSpriteComp.Stretch + (vectorSpriteComp.LastStretch - vectorSpriteComp.Stretch) * (1 - betweenFrameAlpha) * enableFrameSmoothingFlag;

                float cos = (float)Math.Cos(rotation);
                float sin = (float)Math.Sin(rotation);

                foreach (RenderShape renderShape in vectorSpriteComp.RenderShapes)
                {
                    VertexPositionColor[] verts = renderShape.ComputeVertices();
                    for(int i = verts.Length - 1; i >= 0; i--)
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
