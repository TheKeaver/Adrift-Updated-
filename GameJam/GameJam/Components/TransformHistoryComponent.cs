using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class TransformHistoryComponent : IComponent
    {
        public List<Vector2> Positions;
        public List<float> Rotations;
        public int MaxHistoryCount;
        public Timer updateInterval;

        public TransformHistoryComponent(Vector2 startPosition, float startRotation, int maxHistory)
        {
            MaxHistoryCount = maxHistory;
            updateInterval = new Timer(CVars.Get<float>("animation_trail_frequency_timer"));

            Positions = new List<Vector2>();
            Rotations = new List<float>(MaxHistoryCount);
            AddToTransformHistory(startPosition, startRotation);
        }

        public Vector2 AddToTransformHistory(Vector2 position, float rotation)
        {
            Positions.Insert(0, position);
            Rotations.Insert(0, rotation);

            if (Positions.Count > MaxHistoryCount)
            {
                Positions.RemoveRange(MaxHistoryCount, Positions.Count - MaxHistoryCount);
            }
            if (Rotations.Count > MaxHistoryCount)
            {
                Rotations.RemoveRange(MaxHistoryCount, Rotations.Count - MaxHistoryCount);
            }

            return position;
        }

        public int GetLastHistoryIndex()
        {
            // Positions.Count == Rotations.Count so this is safe
            return Positions.Count - 1;
        }
    }
}
