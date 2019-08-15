using System;
using Audrey;
using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events
{
    public class ProjectileBouncedEvent : IEvent
    {
        public Entity projectile;
        public Vector2 coordinate;
        public Entity collidedWith;

        public ProjectileBouncedEvent(Entity projectile, Vector2 coordinate, Entity collidedWith)
        {
            this.projectile = projectile;
            this.coordinate = coordinate;
            this.collidedWith = collidedWith;
        }
    }
}
