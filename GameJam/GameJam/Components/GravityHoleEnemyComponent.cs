﻿using Audrey;

namespace GameJam.Components
{
    public class GravityHoleEnemyComponent : IComponent
    {
        public int lifespan;
        public float strength;
        public float radius;

        public float ElapsedAliveTime = 0;

        public bool ScalingAnimation = true;

        public GravityHoleEnemyComponent(float radius, float strength, int lifespan)
        {
            this.radius = radius;
            this.strength = strength;
            this.lifespan = lifespan;
        }
    }
}
