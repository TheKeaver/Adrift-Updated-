using Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    public class CreateExplosionEvent : IEvent
    {
        Vector2 explosionLocation;
        public CreateExplosionEvent(Vector2 coordinates)
        {
            explosionLocation = coordinates;
        }
    }
}
