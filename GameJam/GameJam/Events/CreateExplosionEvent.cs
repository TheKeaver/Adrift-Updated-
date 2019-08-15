using Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    public class CreateExplosionEvent : IEvent
    {
        public Vector2 explosionLocation;
        public bool playSound;
        public CreateExplosionEvent(Vector2 coordinates, bool playSound = true)
        {
            explosionLocation = coordinates;
            this.playSound = playSound;
        }
    }
}
