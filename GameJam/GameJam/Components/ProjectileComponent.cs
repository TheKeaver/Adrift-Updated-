using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class ProjectileComponent : IComponent
    {
        public int BouncesLeft;
        
        public ProjectileComponent(int totalBounces)
        {
            BouncesLeft = totalBounces;
        }
        public bool hasLeftShootingEnemy = false;
    }
}
