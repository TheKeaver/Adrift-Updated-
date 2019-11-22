using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Processes.Entities
{
    public class SpriteEntityFadeOutProcess : AnimationProcess
    {
        public Engine Engine
        {
            get;
            private set;
        }

        public Entity Entity
        {
            get;
            private set;
        }

        public Easings.Functions EasingFunction
        {
            get;
            private set;
        }

        public SpriteEntityFadeOutProcess(Engine engine, Entity entity, float duration, Easings.Functions easingFunction = Easings.Functions.Linear) : base(duration)
        {
            Engine = engine;
            Entity = entity;
            EasingFunction = easingFunction;
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnTogglePause()
        {   
        }

        protected override void OnUpdateAnimation()
        {
            if(!Engine.GetEntities().Contains(Entity) && IsAlive)
            {
                Kill();
                return;
            }

            SpriteComponent spriteComp = Entity.GetComponent<SpriteComponent>();
            if (spriteComp != null)
            {
                spriteComp.Alpha = MathHelper.Lerp(1, 0, Easings.Interpolate(ClampedAlpha, EasingFunction));
            }
            VectorSpriteComponent vectorSpriteComp = Entity.GetComponent<VectorSpriteComponent>();
            if (vectorSpriteComp != null)
            {
                vectorSpriteComp.Alpha = MathHelper.Lerp(1, 0, Easings.Interpolate(ClampedAlpha, EasingFunction));
            }
        }
    }
}
