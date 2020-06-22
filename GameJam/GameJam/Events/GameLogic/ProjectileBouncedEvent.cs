using Audrey;
using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events.GameLogic
{
    public class ProjectileBouncedEvent : IEvent
    {
        public Entity Projectile;
        public Vector2 Coordinate;
        public Entity CollidedWith;

        public ProjectileBouncedEvent(Entity projectile, Vector2 coordinate, Entity collidedWith)
        {
            this.Projectile = projectile;
            this.Coordinate = coordinate;
            this.CollidedWith = collidedWith;
        }
    }
}
