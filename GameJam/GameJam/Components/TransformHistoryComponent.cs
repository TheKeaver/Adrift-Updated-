using Audrey;
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
        /* 
         * "currentTrailCounter" keeps track of how many trails are active for this entity,
         * This will be increased every update that the speed of the entity meets the minimum
         * requirement to have a trail drawn after it
         */
        public int currentTrailCounter;

        public TransformHistoryComponent(Vector2 startPosition, float startRotation)
        {
            // This "maxHistorySize" be a constant somewhere, ideally CVar
            maxHistorySize = 5;
            historyIndex = 0;

            positionHistory = new Vector2[maxHistorySize];
            rotationHistory = new float[maxHistorySize];
            AddToTransformHistory(startPosition, startRotation);
        }

        public void AddToTransformHistory(Vector2 position, float rotation)
        {
            positionHistory[historyIndex % maxHistorySize] = position;
            rotationHistory[historyIndex % maxHistorySize] = rotation;
            historyIndex++;
        }
    }
}
