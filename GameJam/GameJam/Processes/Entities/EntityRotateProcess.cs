using System;
using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Processes.Entities
{
    public class EntityRotateProcess : AnimationProcess
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

        public float StartAngle
        {
            get;
            private set;
        }
        public float EndAngle
        {
            get;
            private set;
        }

        public Easings.Functions EasingFunction
        {
            get;
            private set;
        }

        public EntityRotateProcess(Engine engine, Entity entity, float duration, float startAngle, float endAngle, Easings.Functions easingFunction = Easings.Functions.Linear) : base(duration)
        {
            Engine = engine;
            Entity = entity;
            EasingFunction = easingFunction;

            StartAngle = startAngle;
            EndAngle = endAngle;
        }

        protected override void OnInitialize()
        {
            
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
            float targetRotation = MathUtils.LerpAngle(StartAngle, EndAngle, Easings.Interpolate(ClampedAlpha, EasingFunction));
            transformComp.Rotate(targetRotation - transformComp.Rotation);
        }
    }
}
