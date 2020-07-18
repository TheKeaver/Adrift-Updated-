using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class TransformHistoryComponent : IComponent
    {
        public Vector2[] positionHistory;
        public float[] rotationHistory;
        public int historyIndex;
        public int maxHistorySize;
        public Timer updateInterval;

        public TransformHistoryComponent(Vector2 startPosition, float startRotation, int maxHistory)
        {
            maxHistorySize = maxHistory;
            updateInterval = new Timer(CVars.Get<float>("animation_trail_frequency_timer"));
            // This "maxHistorySize" be a constant somewhere, ideally CVar
            historyIndex = 0;

            positionHistory = new Vector2[maxHistorySize];
            rotationHistory = new float[maxHistorySize];
            AddToTransformHistory(startPosition, startRotation);
        }

        public Vector2 AddToTransformHistory(Vector2 position, float rotation)
        {
            positionHistory[historyIndex % maxHistorySize] = position;
            rotationHistory[historyIndex % maxHistorySize] = rotation;

            historyIndex = (historyIndex + 1 >= maxHistorySize) ? 0 : historyIndex + 1;

            return position;
        }

        public int GetLastHistoryIndex()
        {
            int ret = (historyIndex == 0) ? (maxHistorySize - 1) : (historyIndex - 1);
            return ret;
        }

        // Return the Position vector at "historyIndex +/- index"
        public Vector2 GetTransformHistoryAt(int mod)
        {
            int correctIndex = GetWrappedIndex(mod);
            return positionHistory[correctIndex];
        }

        public float GetRotationHistoryAt(int mod)
        {
            int correctIndex = GetWrappedIndex(mod);
            return rotationHistory[correctIndex];
        }

        private int GetWrappedIndex(int mod)
        {
            if(mod + historyIndex >= 0) {
                return (historyIndex + mod) % maxHistorySize;
            }

            return GetWrappedIndex(maxHistorySize + mod);
        }
    }
}
