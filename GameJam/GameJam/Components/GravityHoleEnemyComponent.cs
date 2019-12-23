using Audrey;
using GameJam.Common;

namespace GameJam.Components
{
    public class GravityHoleEnemyComponent : IComponent
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
    }
}
