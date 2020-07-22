using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class EntityFadingSystem : BaseSystem
    {
        readonly Family _fadingFamily = Family.All(typeof(VectorSpriteComponent), typeof(FadingEntityTimerComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _fadingEntities;

        public EntityFadingSystem(Engine engine) : base(engine)
        {
            _fadingEntities = engine.GetEntitiesFor(_fadingFamily);
        }

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnKill()
        {
            return;
        }

        protected override void OnTogglePause()
        {
            return;
        }

        protected override void OnUpdate(float dt)
        {
            List<Entity> toDestroy = new List<Entity>();

            foreach (Entity fadingEntity in _fadingEntities)
            {
                FadeEntity(fadingEntity, toDestroy, dt);
            }

            foreach (Entity destroyed in toDestroy)
            {
                Engine.DestroyEntity(destroyed);
            }
        }

        private void FadeEntity(Entity fader, List<Entity> destroyMe, float dt)
        {
            FadingEntityTimerComponent etfc = fader.GetComponent<FadingEntityTimerComponent>();
            //TransformComponent transform = fader.GetComponent<TransformComponent>();
            VectorSpriteComponent vsc = fader.GetComponent<VectorSpriteComponent>();

            etfc.timeRemaining.Update(dt);

            // Ignore scale modification for now. Looking to for timing and Alpha to be correct first
            //transform.SetScale(etfc.timeRemaining / CVars.Get<float>("animation_trail_fading_timer"));
            vsc.Alpha = (1 - etfc.timeRemaining.Alpha);

            if(etfc.timeRemaining.HasElapsed())
            {
                destroyMe.Add(fader);
            }
        }
    }
}
