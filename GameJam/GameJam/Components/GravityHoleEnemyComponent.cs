using Audrey;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class GravityHoleEnemyComponent : IComponent
    {
        public int lifespan;
        public float strength;
        public float radius;

        public GravityHoleEnemyComponent(float radius, float strength, int lifespan)
        {
            this.radius = radius;
            this.strength = strength;
            this.lifespan = lifespan;
        }
    }
}
