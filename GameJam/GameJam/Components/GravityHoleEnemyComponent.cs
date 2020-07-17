using Audrey;
using GameJam.Common;
using System;

namespace GameJam.Components
{
    public class GravityHoleEnemyComponent : IComponent, ICopyComponent
    {
        public int lifespan;
        public float strength;
        public float radius;

        public float ElapsedAliveTime = 0;

        public bool ScalingAnimation = true;
        public bool PingAnimation = true;

        public Timer PingTimer;

        public GravityHoleEnemyComponent(float radius, float strength, int lifespan)
        {
            this.radius = radius;
            this.strength = strength;
            this.lifespan = lifespan;

            PingTimer = new Timer(CVars.Get<float>("gravity_hole_animation_ping_period"));
            PingTimer.Update(CVars.Get<float>("gravity_hole_animation_ping_period"));
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new GravityHoleEnemyComponent(radius, strength, lifespan);
        }
    }
}
