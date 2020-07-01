using Audrey;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class TransformHistoryComponent : IComponent
    {
        public Vector2[] transformHistory;
        public int historyIndex;
        public int maxHistorySize;

        public TransformHistoryComponent(Vector2 startPosition)
        {
            // Tisse should be a constant somewhere, ideally CVar
            maxHistorySize = 6;
            historyIndex = 0;

            transformHistory = new Vector2[maxHistorySize];
            AddToTransformHistory(startPosition);
        }

        public void AddToTransformHistory(Vector2 position)
        {
            transformHistory[historyIndex % maxHistorySize] = position;
            historyIndex++;
        }
    }
}
