using Microsoft.Xna.Framework.Content;

namespace FontExtension
{    
    public class FieldGlyph
    {        
        [ContentSerializer] private readonly char CharacterBackend;
        [ContentSerializer] private readonly byte[] BitmapBackend;
        [ContentSerializer] private readonly string BitmapContentPath;
        [ContentSerializer] private readonly Metrics MetricsBackend;

        public FieldGlyph()
        {
           
        }

        public FieldGlyph(char character, byte[] bitmap, Metrics metrics)
        {
            CharacterBackend = character;
            BitmapBackend = bitmap;
            MetricsBackend = metrics;
        }
        public FieldGlyph(char character, string bitmapContentPath, Metrics metrics)
        {
            CharacterBackend = character;
            BitmapContentPath = bitmapContentPath;
            MetricsBackend = metrics;
        }
        
        /// <summary>
        /// The character this glyph represents
        /// </summary>
        public char Character => this.CharacterBackend;
        /// <summary>
        /// Distance field for this character (if used)
        /// </summary>
        public byte[] Bitmap => this.BitmapBackend;
        /// <summary>
        /// Content path for this character (if used)
        /// </summary>
        public string Path => this.BitmapContentPath;
        /// <summary>
        /// Metrics for this character
        /// </summary>
        public Metrics Metrics => this.MetricsBackend;
    }
}
