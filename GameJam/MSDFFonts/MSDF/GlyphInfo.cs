using System;

namespace FontExtension
{
    public sealed class GlyphInfo
    {
        public char Character
        {
            get;
        }
        public Metrics Metrics
        {
            get;
        }

        public GlyphInfo(char character, Metrics metrics)
        {
            Character = character;
            Metrics = metrics;
        }
    }
}
