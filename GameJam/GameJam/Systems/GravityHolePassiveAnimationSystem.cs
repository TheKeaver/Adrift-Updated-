using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class GravityHolePassiveAnimationSystem : BaseSystem
    {
        private readonly Family _gravityHoleFamily = Family.All(typeof(GravityHoleEnemyComponent), typeof(TransformComponent)).Get();
        private readonly ImmutableList<Entity> _gravityHoleEntities;

        public GravityHolePassiveAnimationSystem(Engine engine) : base(engine)
        {
            _gravityHoleEntities = Engine.GetEntitiesFor(_gravityHoleFamily);
        }

        public override void Update(float dt)
        {
            foreach(Entity entity in _gravityHoleEntities)
            {
                GravityHoleEnemyComponent gravityHoleEnemyComp = entity.GetComponent<GravityHoleEnemyComponent>();
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                transformComp.Rotate(CVars.Get<float>("gravity_hole_animation_rotation_speed") * dt);

                if (gravityHoleEnemyComp.ScalingAnimation)
                {
                    gravityHoleEnemyComp.ElapsedAliveTime += dt;
                    float scaleMin = CVars.Get<float>("gravity_enemy_size") * CVars.Get<float>("gravity_hole_animation_size_multiplier_min");
                    float scaleMax = CVars.Get<float>("gravity_enemy_size") * CVars.Get<float>("gravity_hole_animation_size_multiplier_max");
                    float alpha = (float)(0.5f * Math.Cos(2 * MathHelper.Pi / CVars.Get<float>("gravity_hole_animation_size_period") * gravityHoleEnemyComp.ElapsedAliveTime) + 0.5f);
                    float scale = MathHelper.Lerp(scaleMin, scaleMax, alpha);
                    transformComp.ChangeScale(scale);
                }
            }
        }
    }
}
