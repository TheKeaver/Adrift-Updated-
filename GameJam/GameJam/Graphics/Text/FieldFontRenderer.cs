using System;
using System.Collections.Generic;
using System.Linq;
using FontExtension;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Graphics.Text
{
    public sealed class FieldFontRenderer
    {
        private const string LargeTextTechnique = "LargeText";
        private const string SmallTextTechnique = "SmallText";

        private readonly Effect Effect;
        private readonly GraphicsDevice Device;
        private readonly ContentManager Content;
        private readonly Quad Quad;

        private FieldFont _currentFont;
        private Texture2D _currentTexture;
        private List<VertexPositionColorTexture> _vertices = new List<VertexPositionColorTexture>();
        private List<int> _indices = new List<int>();
        private bool _optimizeForSmallText;
        private bool _currentlyDrawing;

        public FieldFontRenderer(ContentManager content, GraphicsDevice device)
        {
            Effect = content.Load<Effect>("effect_field_font");
            Device = device;
            Content = content;

            Quad = new Quad();
        }

        public void Begin(bool optimizeForSmallText = false)
        {
            Matrix wvp;
            Matrix.CreateOrthographicOffCenter(0, Device.Viewport.Width, 0,
                Device.Viewport.Height, -10000, 10000, out wvp);
            Begin(wvp, optimizeForSmallText);

        }
        public void Begin(Matrix worldViewProjection, bool optimizeForSmallText = false)
        {
            if (_currentlyDrawing)
            {
                throw new Exception("Currently drawing.");
            }

            _currentFont = null;
            _currentTexture = null;
            _vertices.Clear();
            _optimizeForSmallText = optimizeForSmallText;

            _currentlyDrawing = true;

            Effect.Parameters["WorldViewProjection"].SetValue(worldViewProjection);

            if (optimizeForSmallText)
            {
                Effect.CurrentTechnique = Effect.Techniques[SmallTextTechnique];
            }
            else
            {
                Effect.CurrentTechnique = Effect.Techniques[LargeTextTechnique];
            }
        }
        public void End()
        {
            if (!_currentlyDrawing)
            {
                throw new Exception("Not currently drawing.");
            }

            Flush();

            _currentlyDrawing = false;

            _currentFont = null;
            _currentTexture = null;
        }
        public void Flush()
        {
            if (!_currentlyDrawing)
            {
                throw new Exception("Not currently drawing.");
            }

            if (!(_vertices.Count == 0 || _indices.Count == 0))
            {
                Effect.CurrentTechnique.Passes[0].Apply();
                Device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList,
                    _vertices.ToArray(),
                    0,
                    _vertices.Count,
                    _indices.ToArray(),
                    0,
                    _indices.Count / 3);
            }
            _vertices.Clear();
            _indices.Clear();
        }

        public void Draw(FieldFont font, string content, Vector2 position, float rotation)
        {
            Draw(font, content, position, rotation, Color.White);
        }
        public void Draw(FieldFont font, string content, Vector2 position, float rotation,
            Color color, float scale = 1, bool enableKerning = true)
        {
            if(!_currentlyDrawing)
            {
                throw new Exception("Not currently drawing. Call `Begin` first.");
            }

            if(string.IsNullOrEmpty(content))
            {
                return;
            }

            if(_currentFont != null)
            {
                if (_currentFont.PxRange != font.PxRange || _currentFont.TxSize != font.TxSize)
                {
                    // Incompatible
                    Flush();
                }
            }
            _currentFont = font;
            Effect.Parameters["PxRange"].SetValue(font.PxRange);
            Effect.Parameters["TextureSize"].SetValue(new Vector2(font.TxSize));

            GlyphRenderInfo[] sequence = content.Select((char c) => {
                return font.GetRenderInfo(Device, Content, c);
            }).ToArray();

            float cos = (float)Math.Cos(rotation);
            float sin = (float)Math.Sin(rotation);

            Vector2 pen = -font.MeasureString(content, enableKerning) / 2;
            for (int i = 0; i < sequence.Length; i++)
            {
                GlyphRenderInfo current = sequence[i];
                if(current == null)
                {
                    continue;
                }

                if(_currentTexture != null)
                {
                    if(_currentTexture != current.TextureRegion.Texture)
                    {
                        Flush();
                    }
                }
                _currentTexture = current.TextureRegion.Texture;
                Effect.Parameters["GlyphTexture"].SetValue(current.TextureRegion.Texture);

                float glyphHeight = font.TxSize * (1.0f / current.Metrics.Scale);
                float glyphWidth = font.TxSize * (1.0f / current.Metrics.Scale);

                float left = pen.X - current.Metrics.Translation.X;
                float bottom = pen.Y - current.Metrics.Translation.Y;

                float right = left + glyphWidth;
                float top = bottom + glyphHeight;

                if (!char.IsWhiteSpace(current.Character))
                {
                    Vector2 bottomLeft = new Vector2(left, bottom) * scale;
                    Vector2 topRight = new Vector2(right, top) * scale;

                    bottomLeft += position;
                    topRight += position;

                    int verticesCount = _vertices.Count;
                    _vertices.Add(new VertexPositionColorTexture
                    {
                        Position = new Vector3(RotateVector(new Vector2(topRight.X, bottomLeft.Y), cos, sin), -10000),
                        Color = color,
                        TextureCoordinate = new Vector2(1, 1)
                    });
                    _vertices.Add(new VertexPositionColorTexture
                    {
                        Position = new Vector3(RotateVector(new Vector2(bottomLeft.X, bottomLeft.Y), cos, sin), -10000),
                        Color = color,
                        TextureCoordinate = new Vector2(0, 1)
                    });
                    _vertices.Add(new VertexPositionColorTexture
                    {
                        Position = new Vector3(RotateVector(new Vector2(bottomLeft.X, topRight.Y), cos, sin), -10000),
                        Color = color,
                        TextureCoordinate = new Vector2(0, 0)
                    });
                    _vertices.Add(new VertexPositionColorTexture
                    {
                        Position = new Vector3(RotateVector(new Vector2(topRight.X, topRight.Y), cos, sin), -10000),
                        Color = color,
                        TextureCoordinate = new Vector2(1, 0)
                    });

                    _indices.Add(verticesCount); // 0
                    _indices.Add(verticesCount + 1); // 1
                    _indices.Add(verticesCount + 2); // 2
                    _indices.Add(verticesCount + 2); // 2
                    _indices.Add(verticesCount + 3); // 3
                    _indices.Add(verticesCount); // 0
                }

                pen.X += current.Metrics.Advance;

                if (enableKerning && i < sequence.Length - 1)
                {
                    var next = sequence[i + 1];
                    if (next != null)
                    {

                        var pair = font.KerningPairs.FirstOrDefault(
                            x => x.Left == current.Character && x.Right == next.Character);

                        if (pair != null)
                        {
                            pen.X += pair.Advance;
                        }
                    }
                }
            }
        }

        private Vector2 RotateVector(Vector2 vector, float cos, float sin)
        {
            return new Vector2(vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos);
        }
    }
}
