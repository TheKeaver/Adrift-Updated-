using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Processes.Animations.Warp
{
    public class WarpEntityPhase2Process : AnimationProcess
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

        public Vector2 WarpTo
        {
            get;
            private set;
        }
        public Vector2 Starting
        {
            get;
            private set;
        }

        public WarpEntityPhase2Process(Engine engine, Entity entity, Vector2 warpTo, float duration) : base(duration)
        {
            Engine = engine;
            Entity = entity;
            WarpTo = warpTo;
        }

        protected override void OnInitialize()
        {
            Starting = Entity.GetComponent<TransformComponent>().Position;
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdateAnimation()
        {
            if (IsAlive && !Engine.GetEntities().Contains(Entity))
            {
                Kill();
                return;
            }

            VectorSpriteComponent vectorSpriteComp = Entity.GetComponent<VectorSpriteComponent>();
            vectorSpriteComp.Alpha = MathHelper.Lerp(0, 1, ClampedAlpha);

            TransformComponent transformComp = Entity.GetComponent<TransformComponent>();
            transformComp.Move(Vector2.Lerp(Starting, WarpTo, ClampedAlpha) - transformComp.Position);
        }
    }
}
