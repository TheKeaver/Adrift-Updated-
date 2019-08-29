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
        public bool PlaySound;
        public CreateExplosionEvent(Vector2 coordinates, bool playSound = true)
        {
            ExplosionLocation = coordinates;
            PlaySound = playSound;
        }
    }
}
