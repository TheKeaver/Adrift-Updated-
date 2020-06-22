using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events.EnemyActions
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
