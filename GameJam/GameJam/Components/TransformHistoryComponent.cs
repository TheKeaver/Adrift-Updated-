using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class TransformHistoryComponent : IComponent
    {
        public Vector2[] positionHistory;
        public float[] rotationHistory;
        public int historyIndex;
        public int maxHistorySize;
        public Timer updateInterval;

        public TransformHistoryComponent(Vector2 startPosition, float startRotation, int maxHistorySize)
        {
            updateInterval = new Timer(CVars.Get<float>("animation_trail_frequency_timer"));
            // This "maxHistorySize" be a constant somewhere, ideally CVar
            historyIndex = 0;

            positionHistory = new Vector2[maxHistorySize];
            rotationHistory = new float[maxHistorySize];
            AddToTransformHistory(startPosition, startRotation);
        }

        public void AddToTransformHistory(Vector2 position, float rotation)
        {
            positionHistory[historyIndex % maxHistorySize] = position;
            rotationHistory[historyIndex % maxHistorySize] = rotation;

            historyIndex = (historyIndex + 1 >= maxHistorySize) ? 0 : historyIndex + 1;
        }

        public int GetLastHistoryIndex()
        {
            int ret = (historyIndex == 0) ? (maxHistorySize - 1) : (historyIndex - 1);
            return ret;
        }
    }
}
