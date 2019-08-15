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

        public ProjectileBouncedEvent(Entity projectile, Vector2 coordinate)
        {
            this.projectile = projectile;
            this.coordinate = coordinate;
        }
    }
}
