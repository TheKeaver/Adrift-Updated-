using Audrey;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Components
{
    public class EdgeComponent : IComponent, ICopyComponent
    {
        public Vector2 Normal;
        public EdgeComponent(Vector2 norm)
        {
            Normal = norm;
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            EdgeComponent ec = new EdgeComponent(new Vector2(Normal.X, Normal.Y));
            return ec;
        }
    }
}
