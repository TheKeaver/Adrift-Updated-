﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace FontExtension
{
    public class FieldFont
    {
        [ContentSerializer] private readonly Dictionary<char, FieldGlyph> Glyphs;
        [ContentSerializer] private readonly string NameBackend;
        [ContentSerializer] private readonly float PxRangeBackend;
        [ContentSerializer] private readonly float TxSizeBackend;
        [ContentSerializer] private readonly List<KerningPair> KerningPairsBackend;

        public FieldFont()
        {            
        }

        public FieldFont(string name, IReadOnlyCollection<FieldGlyph> glyphs, IReadOnlyCollection<KerningPair> kerningPairs, float pxRange, float txSize)
        {
            NameBackend = name;
            PxRangeBackend = pxRange;
            TxSizeBackend = txSize;
            KerningPairsBackend = kerningPairs.ToList();

            Glyphs = new Dictionary<char, FieldGlyph>(glyphs.Count);
            foreach (var glyph in glyphs)
            {
                Glyphs.Add(glyph.Character, glyph);
            }
        }

        /// <summary>
        /// Name of the font
        /// </summary>
        public string Name => NameBackend;
        
        /// <summary>
        /// Distance field effect range in pixels
        /// </summary>
        public float PxRange => PxRangeBackend;

        /// <summary>
        /// Texture size in pixels.
        /// </summary>
        public float TxSize => TxSizeBackend;

        /// <summary>
        /// Kerning pairs available in this font
        /// </summary>
        public IReadOnlyList<KerningPair> KerningPairs => KerningPairsBackend;

        /// <summary>
        /// Characters supported by this font
        /// </summary>
        [ContentSerializerIgnore]
        public IEnumerable<char> SupportedCharacters => Glyphs.Keys;

        /// <summary>
        /// Returns the glyph for the given character, or throws an exception when the glyph is not supported by this font
        /// </summary>        
        public FieldGlyph GetGlyph(char c)
        {
            if (c == '\t' || c == '\n' || c == '\r')
            {
                return null;
            }

            if (Glyphs.TryGetValue(c, out FieldGlyph glyph))
            {
                return glyph;
            }

            throw new InvalidOperationException($"Character '{c}' not found in font {Name}. Did you forget to include it in the character ranges?");
        }

        private GlyphInfo GetInfo(char c)
        {
            FieldGlyph glyph = GetGlyph(c);
            if(glyph == null)
            {
                return null;
            }

            return new GlyphInfo(c, GetGlyph(c).Metrics);
        }
        public Vector2 MeasureString(string str, bool enableKerning = true)
        {
            Vector2 maxSize = Vector2.Zero;
            Vector2 pen = Vector2.Zero;

            GlyphInfo[] sequence = str.Select(GetInfo).ToArray();
            for(int i = 0; i < sequence.Length; i++)
            {
                GlyphInfo info = sequence[i];
                if(info == null)
                {
                    continue;
                }

                float glyphWidth = TxSize / info.Metrics.Scale;
                float glyphHeight = TxSize / info.Metrics.Scale;

                float bottom = pen.Y - info.Metrics.Translation.Y;
                float top = bottom + glyphHeight;

                if(top - bottom > maxSize.Y)
                {
                    maxSize.Y = top - bottom;
                }

                pen.X += info.Metrics.Advance;
                
                if(enableKerning && i < sequence.Length - 1)
                {
                    GlyphInfo next = sequence[i + 1];
                    if (next != null)
                    {
                        KerningPair pair = KerningPairs.FirstOrDefault(x => x.Left == info.Character && x.Right == next.Character);

                        if (pair != null)
                        {
                            pen.X += pair.Advance;
                        }
                    }
                }
            }

            maxSize.X = pen.X;

            return maxSize;
        }

        /// <summary>
        /// Used by FieldFontRenderer.
        /// </summary>
        private Dictionary<char, GlyphRenderInfo> Cache
        {
            get;
            set;
        } = new Dictionary<char, GlyphRenderInfo>();
        public GlyphRenderInfo GetRenderInfo(GraphicsDevice device, ContentManager content, char c)
        {
            if(c == '\t' || c == '\n' || c == '\r')
            {
                return null;
            }

            if (Cache.TryGetValue(c, out var value))
            {
                return value;
            }

            var unit = LoadRenderInfo(device, content, c);
            Cache.Add(c, unit);
            return unit;
        }
        private GlyphRenderInfo LoadRenderInfo(GraphicsDevice device, ContentManager content, char c)
        {
            var glyph = GetGlyph(c);
            TextureRegion2D texture;
            if (glyph.Bitmap == null)
            {
                texture = content.Load<TextureAtlas>("complete_texture_atlas")
                    .GetRegion((glyph.Path));
            }
            else
            {
                using (var stream = new MemoryStream(glyph.Bitmap))
                {
                    texture = new TextureRegion2D(Texture2D.FromStream(device, stream));
                }
            }
            // TODO: Support grabbing TextureRegion2D from texture atlas
            var unit = new GlyphRenderInfo(c, texture, glyph.Metrics);

            return unit;
        }
    }
}
