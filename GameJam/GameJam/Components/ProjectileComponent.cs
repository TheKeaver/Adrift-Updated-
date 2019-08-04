using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class ProjectileComponent : IComponent
    {
        public int bouncesLeft;
        
        public ProjectileComponent(int totalBounces)
        {
            bouncesLeft = totalBounces;
        }
    }
}
