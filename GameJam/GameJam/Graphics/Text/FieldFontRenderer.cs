using System.IO;
using System.Linq;
using FontExtension;
using GameJam.Components;
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
        private readonly Quad Quad;

        public FieldFontRenderer(ContentManager content, GraphicsDevice device)
        {
            Effect = content.Load<Effect>("effect_field_font");
            Device = device;

            Quad = new Quad();
        }

        public void Render(Matrix worldViewProjection, FieldFont fieldFont,
            string content, Vector2 position, float rotation)
        {
            Render(worldViewProjection, fieldFont, content, position, rotation);
        }
        public void Render(Matrix worldViewProjection, FieldFont font,
            string content, Vector2 position, float rotation, Color color,
            float scale = 1, bool enableKerning = true, bool optimizeForSmallText = true)
        {
            if (string.IsNullOrEmpty(content))
                return;

            var sequence = content.Select((char c) => {
                return font.GetRenderInfo(Device, c);
            }).ToArray();
            var textureWidth = sequence[0].Texture.Width;
            var textureHeight = sequence[0].Texture.Height;

            Matrix modelMatrix = Matrix.CreateTranslation(new Vector3(position - font.MeasureString(content, enableKerning) / 2, 0))
                * Matrix.CreateRotationZ(-rotation)
                * Matrix.CreateScale(scale);

            Effect.Parameters["WorldViewProjection"].SetValue(modelMatrix * worldViewProjection);
            Effect.Parameters["PxRange"].SetValue(font.PxRange);
            Effect.Parameters["TextureSize"].SetValue(new Vector2(textureWidth, textureHeight));
            Effect.Parameters["ForegroundColor"].SetValue(color.ToVector4());

            if (optimizeForSmallText)
            {
                Effect.CurrentTechnique = Effect.Techniques[SmallTextTechnique];
            }
            else
            {
                Effect.CurrentTechnique = Effect.Techniques[LargeTextTechnique];
            }

            var pen = Vector2.Zero;
            for (var i = 0; i < sequence.Length; i++)
            {
                var current = sequence[i];

                Effect.Parameters["GlyphTexture"].SetValue(current.Texture);
                Effect.CurrentTechnique.Passes[0].Apply();

                var glyphHeight = textureHeight * (1.0f / current.Metrics.Scale);
                var glyphWidth = textureWidth * (1.0f / current.Metrics.Scale);

                var left = pen.X - current.Metrics.Translation.X;
                var bottom = pen.Y - current.Metrics.Translation.Y;

                var right = left + glyphWidth;
                var top = bottom + glyphHeight;

                if (!char.IsWhiteSpace(current.Character))
                {
                    Quad.Render(Device, new Vector2(left, bottom), new Vector2(right, top));
                }

                pen.X += current.Metrics.Advance;

                if (enableKerning && i < sequence.Length - 1)
                {
                    var next = sequence[i + 1];

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
}
