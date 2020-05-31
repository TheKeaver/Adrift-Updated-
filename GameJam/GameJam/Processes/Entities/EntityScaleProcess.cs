using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Processes.Entities
{
    public class EntityScaleProcess : AnimationProcess
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

        public float StartScale
        {
            get;
            private set;
        }
        public float EndScale
        {
            get;
            private set;
        }

        public Easings.Functions EasingFunction
        {
            get;
            private set;
        }

        public EntityScaleProcess(Engine engine, Entity entity, float duration, float startScale, float endScale, Easings.Functions easingFunction = Easings.Functions.Linear) : base(duration)
        {
            Engine = engine;
            Entity = entity;
            EasingFunction = easingFunction;

            StartScale = startScale;
            EndScale = endScale;
        }

        protected override void OnInitialize()
        {
            if (!Engine.GetEntities().Contains(Entity) && IsAlive)
            {
                Kill();
                return;
            }

            TransformComponent transformComp = Entity.GetComponent<TransformComponent>();
            transformComp.SetScale(StartScale, true);
        }

        protected override void OnTogglePause()
        {
            
        }

        protected override void OnUpdateAnimation()
        {
            if (!Engine.GetEntities().Contains(Entity) && IsAlive)
            {
                Kill();
                return;
            }

            TransformComponent transformComp = Entity.GetComponent<TransformComponent>();
            transformComp.SetScale(MathHelper.Lerp(StartScale, EndScale, Easings.Interpolate(ClampedAlpha, EasingFunction)));
        }
    }
}
