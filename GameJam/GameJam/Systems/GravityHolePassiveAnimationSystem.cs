using System;
using Audrey;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Systems
{
    public class GravityHolePassiveAnimationSystem : BaseSystem
    {
        private readonly Family _gravityHoleFamily = Family.All(typeof(GravityHoleEnemyComponent), typeof(TransformComponent)).Get();
        private readonly ImmutableList<Entity> _gravityHoleEntities;

        public ProcessManager ProcessManager
        {
            get;
            private set;
        }

        public ContentManager ContentManager
        {
            get;
            private set;
        }

        public GravityHolePassiveAnimationSystem(Engine engine, ProcessManager processManager, ContentManager contentManager) : base(engine)
        {
            ProcessManager = processManager;
            ContentManager = contentManager;

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
                    transformComp.SetScale(scale);
                }

                if(gravityHoleEnemyComp.PingAnimation)
                {
                    gravityHoleEnemyComp.PingTimer.Update(dt);
                    if(gravityHoleEnemyComp.PingTimer.HasElapsed())
                    {
                        gravityHoleEnemyComp.PingTimer.Reset();

                        GravityHolePingEntity.Create(Engine, ProcessManager, ContentManager,
                            transformComp.Position, gravityHoleEnemyComp.radius);
                    }
                }
            }
        }
    }
}
