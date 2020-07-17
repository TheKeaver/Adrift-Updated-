using Audrey;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Components
{
    public class ProjectileComponent : IComponent, ICopyComponent
    {
        public int BouncesLeft;
        public Color Color;
        public Player LastBouncedBy = null;
        
        public ProjectileComponent(int totalBounces, Color color)
        {
            BouncesLeft = totalBounces;
            Color = color;
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new ProjectileComponent(BouncesLeft, new Color(Color.R, Color.G, Color.B));
        }
    }
}
