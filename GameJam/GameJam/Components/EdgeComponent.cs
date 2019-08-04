using Audrey;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class EdgeComponent : IComponent
    {
        public Vector2 Normal;
        public EdgeComponent(Vector2 norm)
        {
            Normal = norm;
        }
    }
}
