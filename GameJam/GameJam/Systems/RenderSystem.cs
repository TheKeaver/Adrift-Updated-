﻿using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;

namespace GameJam.Systems
{
    public class RenderSystem
    {
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        public Engine Engine
        {
            get;
            private set;
        }

        public static readonly Vector2 HalfHalf = new Vector2(0.5f, 0.5f);
        public static readonly Vector2 FlipY = new Vector2(1, -1);

        readonly Family _renderEntityFamily = Family.All(typeof(TransformComponent))
            .One(typeof(SpriteComponent),
                typeof(NinePatchComponent),
                typeof(BitmapFontComponent),
                typeof(FieldFontComponent),
                typeof(VectorSpriteComponent))
            .Get();
        readonly ImmutableList<Entity> _renderEntities;

        private BasicEffect _spriteEffect;
        private BasicEffect _vectorSpriteEffect;

        private Matrix _projectionMatrix;
        private Matrix _transformMatrix;
        private Viewport _lastViewport;

        private Dictionary<Type, Action<Matrix, Entity, Vector2, float, float, float>> _componentRenderFunctionDict;

        #region SPRITE RENDER MEMBERS
        private Texture2D _currentSpriteTexture;
        private List<VertexPositionColorTexture> _spriteVerts;
        private List<int> _spriteIndices;
        #endregion

        #region VECTOR SPRITE RENDER MEMBERS
        private List<VertexPositionColor> _vectorSpriteVerts;
        private List<int> _vectorSpriteIndices;
        #endregion

        #region FIELD FONT RENDER MEMBERS
        private FieldFontRenderer _fieldFontRenderer;
        #endregion

        public RenderSystem(GraphicsDevice graphics, ContentManager content, Engine engine)
        {
            GraphicsDevice = graphics;
            Engine = engine;

            _renderEntities = Engine.GetEntitiesFor(_renderEntityFamily);

            InitEffects();

            _projectionMatrix = Matrix.Identity;
            _lastViewport = new Viewport();

            _componentRenderFunctionDict = new Dictionary<Type, Action<Matrix, Entity, Vector2, float, float, float>>();
            _componentRenderFunctionDict.Add(typeof(SpriteComponent), SpriteRenderFunction);
            _componentRenderFunctionDict.Add(typeof(BitmapFontComponent), BitmapFontRenderFunction);
            _componentRenderFunctionDict.Add(typeof(VectorSpriteComponent), VectorSpriteRenderFunction);
            _componentRenderFunctionDict.Add(typeof(FieldFontComponent), FieldFontRenderFunction);

            _currentSpriteTexture = null;
            _spriteVerts = new List<VertexPositionColorTexture>();
            _spriteIndices = new List<int>();

            _vectorSpriteVerts = new List<VertexPositionColor>();
            _vectorSpriteIndices = new List<int>();

            _fieldFontRenderer = new FieldFontRenderer(content, GraphicsDevice);
        }

        #region INITIALIZATION
        private void InitEffects()
        {
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
        #endregion

        #region PUBLIC DRAW CALLS
        public void DrawEntities(Camera camera, float dt, float betweenFrameAlpha, Camera debugCamera = null)
        {
            DrawEntities(camera, Constants.Render.GROUP_MASK_ALL, dt, betweenFrameAlpha, debugCamera);
        }
        public void DrawEntities(Camera camera, byte groupMask, float dt, float betweenFrameAlpha, Camera debugCamera = null)
        {
            int enableFrameSmoothing = CVars.Get<bool>("graphics_frame_smoothing") ? 1 : 0;
            // Allow between 0-1 when enabled, set at 1 if not enabled
            betweenFrameAlpha = betweenFrameAlpha * enableFrameSmoothing + (1 - enableFrameSmoothing);

            Matrix transformMatrix = debugCamera == null ?
                camera.GetInterpolatedTransformMatrix(betweenFrameAlpha)
                    : debugCamera.GetInterpolatedTransformMatrix(betweenFrameAlpha);
            BoundingRect cameraBoundingRect = debugCamera == null ?
                camera.BoundingRect
                    : debugCamera.BoundingRect;
            DrawRenderEntities(transformMatrix, cameraBoundingRect, groupMask, dt, betweenFrameAlpha);
        }
        #endregion

        #region DRAW RENDER ENTITIES
        private void DrawRenderEntities(Matrix cameraTransformMatrix, BoundingRect cameraBoundingRect, byte groupMask, float dt, float betweenFrameAlpha)
        {
            if(_renderEntities.Count == 0)
            {
                return;
            }
            UpdateProjectionMatrix();
            _transformMatrix = cameraTransformMatrix;
            Matrix combinedMatrix = UpdateEffectsProjection();

            BeginRender(combinedMatrix);

            foreach(Entity entity in _renderEntities)
            {
                IRenderComponent iRenderComp = GetRenderComponent(entity);
                if(iRenderComp.IsHidden()
                    || (iRenderComp.GetRenderGroup() & groupMask) == 0)
                {
                    continue;
                }

                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                // Render Culling
                BoundingRect boundingRect = iRenderComp.GetAABB(transformComp.Scale);
                boundingRect.Min += transformComp.Position;
                boundingRect.Max += transformComp.Position;

                if(!boundingRect.Intersects(cameraBoundingRect)
                    && CVars.Get<bool>("debug_show_render_culling"))
                {
                    continue;
                }

                Vector2 transformPosition;
                float transformRotation;
                float transformScale;
                transformComp.Interpolate(betweenFrameAlpha,
                    out transformPosition,
                    out transformRotation,
                    out transformScale);

                if (_componentRenderFunctionDict.ContainsKey(iRenderComp.GetType()))
                {
                    _componentRenderFunctionDict[iRenderComp.GetType()](cameraTransformMatrix,
                        entity,
                        transformPosition,
                        transformRotation,
                        transformScale,
                        betweenFrameAlpha);
                } else
                {
                    Console.WriteLine($"No render function for {iRenderComp.GetType().Name}");
                }
            }

            FlushAll();

            EndRender();
        }
        #endregion

        #region RENDER METHODS
        private void SpriteRenderFunction(Matrix cameraMatrix, Entity entity, Vector2 position, float rotation, float scale, float betweenFrameAlpha)
        {
            SpriteComponent spriteComp = entity.GetComponent<SpriteComponent>();
            if(_currentSpriteTexture == null)
            {
                _currentSpriteTexture = spriteComp.Texture.Texture;
            }
            if(_currentSpriteTexture != spriteComp.Texture.Texture)
            {
                SpriteFlush();
                _currentSpriteTexture = spriteComp.Texture.Texture;
            }

            position *= FlipY;

            float cos = (float)Math.Cos(rotation),
                sin = (float)Math.Sin(rotation);

            int preVertCount = _spriteVerts.Count;

            Rectangle texRegionSourceBounds = spriteComp.Texture.Bounds;
            float umin = texRegionSourceBounds.X / (float)spriteComp.Texture.Texture.Width,
                vmin = texRegionSourceBounds.Y / (float)spriteComp.Texture.Texture.Height,
                umax = (texRegionSourceBounds.X + texRegionSourceBounds.Width) / (float)spriteComp.Texture.Texture.Width,
                vmax = (texRegionSourceBounds.Y + texRegionSourceBounds.Height) / (float)spriteComp.Texture.Texture.Height;

            // Bottom Left
            _spriteVerts.Add(new VertexPositionColorTexture
            {
                Position = new Vector3(RotateVector(new Vector2(-spriteComp.Bounds.X / 2, -spriteComp.Bounds.Y / 2), cos, sin) * scale + position, 0),
                Color = spriteComp.Color,
                TextureCoordinate = new Vector2(umin, vmax)
            });
            // Bottom Right
            _spriteVerts.Add(new VertexPositionColorTexture
            {
                Position = new Vector3(RotateVector(new Vector2(spriteComp.Bounds.X / 2, -spriteComp.Bounds.Y / 2), cos, sin) * scale + position, 0),
                Color = spriteComp.Color,
                TextureCoordinate = new Vector2(umax, vmax)
            });
            // Top Right
            _spriteVerts.Add(new VertexPositionColorTexture
            {
                Position = new Vector3(RotateVector(new Vector2(spriteComp.Bounds.X / 2, spriteComp.Bounds.Y / 2), cos, sin) * scale + position, 0),
                Color = spriteComp.Color,
                TextureCoordinate = new Vector2(umax, vmin)
            });
            // Top Left
            _spriteVerts.Add(new VertexPositionColorTexture
            {
                Position = new Vector3(RotateVector(new Vector2(-spriteComp.Bounds.X / 2, spriteComp.Bounds.Y / 2), cos, sin) * scale + position, 0),
                Color = spriteComp.Color,
                TextureCoordinate = new Vector2(umin, vmin)
            });

            // Clockwise faces
            _spriteIndices.Add(0 + preVertCount);
            _spriteIndices.Add(1 + preVertCount);
            _spriteIndices.Add(2 + preVertCount);

            _spriteIndices.Add(0 + preVertCount);
            _spriteIndices.Add(2 + preVertCount);
            _spriteIndices.Add(3 + preVertCount);
        }
        private void BitmapFontRenderFunction(Matrix cameraMatrix, Entity entity, Vector2 position, float rotation, float scale, float betweenFrameAlpha)
        {
            BitmapFontComponent bitmapFontComp = entity.GetComponent<BitmapFontComponent>();

            float cos = (float)Math.Cos(rotation),
                sin = (float)Math.Sin(rotation);

            Vector2 bounds = bitmapFontComp.Font.MeasureString(bitmapFontComp.Content);

            BitmapFont.StringGlyphEnumerable glyphs = bitmapFontComp.Font.GetGlyphs(bitmapFontComp.Content);
            foreach(BitmapFontGlyph glyph in glyphs)
            {
                if(glyph.FontRegion == null)
                {
                    continue;
                }

                TextureRegion2D texture = glyph.FontRegion.TextureRegion;

                if(_currentSpriteTexture == null)
                {
                    _currentSpriteTexture = texture.Texture;
                }
                if(_currentSpriteTexture != texture.Texture)
                {
                    SpriteFlush();
                    _currentSpriteTexture = texture.Texture;
                }

                Rectangle texRegionSourceBounds = texture.Bounds;
                float umin = texRegionSourceBounds.X / (float)texture.Texture.Width,
                    vmin = texRegionSourceBounds.Y / (float)texture.Texture.Height,
                    umax = (texRegionSourceBounds.X + texRegionSourceBounds.Width) / (float)texture.Texture.Width,
                    vmax = (texRegionSourceBounds.Y + texRegionSourceBounds.Height) / (float)texture.Texture.Height;

                Vector2 characterOrigin = position - bounds * scale / 2 - glyph.Position * scale;

                int preVertCount = _spriteVerts.Count;


                // Bottom Left
                _spriteVerts.Add(new VertexPositionColorTexture
                {
                    Position = new Vector3(RotateVector(new Vector2(characterOrigin.X - texture.Bounds.X * scale / 2,
                        characterOrigin.Y - texture.Bounds.Y * scale / 2), cos, sin), 0),
                    Color = bitmapFontComp.Color,
                    TextureCoordinate = new Vector2(umin, vmax)
                });
                // Bottom Right
                _spriteVerts.Add(new VertexPositionColorTexture
                {
                    Position = new Vector3(RotateVector(new Vector2(characterOrigin.X + texture.Bounds.X * scale / 2,
                        characterOrigin.Y - texture.Bounds.Y * scale / 2), cos, sin), 0),
                    Color = bitmapFontComp.Color,
                    TextureCoordinate = new Vector2(umax, vmax)
                });
                // Top Right
                _spriteVerts.Add(new VertexPositionColorTexture
                {
                    Position = new Vector3(RotateVector(new Vector2(characterOrigin.X + texture.Bounds.X * scale / 2,
                        characterOrigin.Y + texture.Bounds.Y * scale / 2), cos, sin), 0),
                    Color = bitmapFontComp.Color,
                    TextureCoordinate = new Vector2(umax, vmin)
                });
                // Top Left
                _spriteVerts.Add(new VertexPositionColorTexture
                {
                    Position = new Vector3(RotateVector(new Vector2(characterOrigin.X - texture.Bounds.X * scale / 2,
                        characterOrigin.Y + texture.Bounds.Y * scale / 2), cos, sin), 0),
                    Color = bitmapFontComp.Color,
                    TextureCoordinate = new Vector2(umin, vmin)
                });

                // Clockwise faces
                _spriteIndices.Add(0 + preVertCount);
                _spriteIndices.Add(1 + preVertCount);
                _spriteIndices.Add(2 + preVertCount);

                _spriteIndices.Add(0 + preVertCount);
                _spriteIndices.Add(2 + preVertCount);
                _spriteIndices.Add(3 + preVertCount);
            }
        }
        private void VectorSpriteRenderFunction(Matrix cameraMatrix, Entity entity, Vector2 position, float rotation, float scale, float betweenFrameAlpha)
        {
            VectorSpriteComponent vectorSpriteComp = entity.GetComponent<VectorSpriteComponent>();

            position *= FlipY;
            rotation *= -1;

            float cos = (float)Math.Cos(rotation),
                sin = (float)Math.Sin(rotation);

            Vector2 stretch = vectorSpriteComp.Stretch + (vectorSpriteComp.LastStretch - vectorSpriteComp.Stretch) * (1 - betweenFrameAlpha);

            // Split into two lists
            // 1. Stores each unique vertex that will be drawn
            // 2. Stores the index into list '1' that will be drawn
            foreach (RenderShape renderShape in vectorSpriteComp.RenderShapes)
            {
                /*
                 * 1) Create local arrays to set equal to the return of the ComputedVertices() in the RenderShape()
                 * 2) Run through the for loop of indices.Count, using the corresponding "computedVerticesReturn[index]" to calculate
                 * 3) Add the local arrays to their respective overall lists that were created at the beginning of the function
                 */
                VertexPositionColor[] computedVerticesReturn;
                int[] computedIndicesReturn;

                renderShape.ComputeVertices(out computedVerticesReturn, out computedIndicesReturn);

                int vertsCountBeforeAdd = _vectorSpriteVerts.Count;

                /* 
                * Loop
                */
                for (int i = 0; i < computedVerticesReturn.Length; i++)
                {
                    VertexPositionColor vert = computedVerticesReturn[i];
                    _vectorSpriteVerts.Add(new VertexPositionColor(new Vector3((vert.Position.X * stretch.X * cos + vert.Position.Y * stretch.Y * -sin) * scale + position.X,
                        (vert.Position.X * stretch.X * sin + vert.Position.Y * stretch.Y * cos) * scale + position.Y, 0), new Color(vert.Color.ToVector4() * renderShape.TintColor.ToVector4() * vectorSpriteComp.Alpha)));
                }

                // Change indices values to change based on length of array currently
                for (int j = 0; j < computedIndicesReturn.Length; j++)
                {
                    // Vertices are added linearly to the "_verts" array, we need to increment
                    _vectorSpriteIndices.Add(computedIndicesReturn[j] + vertsCountBeforeAdd);
                }
            }
        }
        private void FieldFontRenderFunction(Matrix cameraMatrix, Entity entity, Vector2 position, float rotation, float scale, float betweenFrameAlpha)
        {
            FieldFontComponent fieldFontComp = entity.GetComponent<FieldFontComponent>();

            _fieldFontRenderer.Draw(fieldFontComp.Font,
                fieldFontComp.Content,
                position,
                rotation,
                fieldFontComp.Color,
                scale,
                fieldFontComp.EnableKerning,
                0);
        }
        #endregion

        #region FLUSH METHODS
        private void FlushAll()
        {
            SpriteFlush();
            VectorSpriteFlush();
            FieldFontFlush();
        }

        private void SpriteFlush()
        {
            if(_spriteVerts.Count > 0 && _spriteIndices.Count > 0)
            {
                _spriteEffect.Texture = _currentSpriteTexture;
                // Projection matrix is already handled at beginning of render call
                foreach(EffectPass pass in _spriteEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                        _spriteVerts.ToArray(), 0, _spriteVerts.Count,
                        _spriteIndices.ToArray(), 0,
                        _spriteIndices.Count / 3);
                }
            }

            _currentSpriteTexture = null;
            _spriteVerts.Clear();
            _spriteIndices.Clear();
        }
        private void VectorSpriteFlush()
        {
            if (_vectorSpriteVerts.Count > 0 && _vectorSpriteIndices.Count > 0)
            {
                // Projection matrix is already handled at beginning of render call
                foreach (EffectPass pass in _vectorSpriteEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                        _vectorSpriteVerts.ToArray(), 0, _vectorSpriteVerts.Count,
                        _vectorSpriteIndices.ToArray(), 0,
                        _vectorSpriteIndices.Count / 3);
                }
            }

            _vectorSpriteVerts.Clear();
            _vectorSpriteIndices.Clear();
        }
        private void FieldFontFlush()
        {
            _fieldFontRenderer.Flush();
        }
        #endregion

        private void BeginRender(Matrix cameraMatrix)
        {
            _fieldFontRenderer.Begin(cameraMatrix);
        }
        private void EndRender()
        {
            _fieldFontRenderer.End();
        }

        #region HELPER METHODS
        private IRenderComponent GetRenderComponent(Entity entity)
        {
            SpriteComponent spriteComp = entity.GetComponent<SpriteComponent>();
            if (spriteComp != null)
            {
                return spriteComp;
            }

            VectorSpriteComponent vectorSpriteComp = entity.GetComponent<VectorSpriteComponent>();
            if (vectorSpriteComp != null)
            {
                return vectorSpriteComp;
            }

            BitmapFontComponent bitmapFontComp = entity.GetComponent<BitmapFontComponent>();
            if (bitmapFontComp != null)
            {
                return bitmapFontComp;
            }

            FieldFontComponent fieldFontComp = entity.GetComponent<FieldFontComponent>();
            if (fieldFontComp != null)
            {
                return fieldFontComp;
            }

            NinePatchComponent ninePatchComp = entity.GetComponent<NinePatchComponent>();
            if (ninePatchComp != null)
            {
                return ninePatchComp;
            }

            return null;
        }

        private Vector2 RotateVector(Vector2 vector, float cos, float sin)
        {
            return new Vector2(vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos);
        }
        #endregion

        #region PROJECTION MATRIX
        private void UpdateProjectionMatrix()
        {
            Viewport viewport = GraphicsDevice.Viewport;
            if(viewport.Width != _lastViewport.Width
                || viewport.Height != _lastViewport.Height)
            {
                Matrix.CreateOrthographicOffCenter(0, viewport.Width,
                    viewport.Height, 0,
                    -float.MaxValue, float.MaxValue,
                    out _projectionMatrix);
            }
        }
        private Matrix UpdateEffectsProjection()
        {
            Matrix combinedMatrix = Matrix.Multiply(_transformMatrix, _projectionMatrix);

            _spriteEffect.Projection = combinedMatrix;
            _vectorSpriteEffect.Projection = combinedMatrix;

            return combinedMatrix;
        }
        #endregion
    }
}
