using Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    public class CreateExplosionEvent : IEvent
    {
        public Vector2 ExplosionLocation;
        public Color Color;
        public bool PlaySound;
        public CreateExplosionEvent(Vector2 coordinates, Color color, bool playSound = true)
        {
            ExplosionLocation = coordinates;
            Color = color;
            PlaySound = playSound;
        }
    }
}
