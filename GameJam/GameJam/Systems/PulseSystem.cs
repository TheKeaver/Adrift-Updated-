using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class PulseSystem : BaseSystem
    {
        readonly Family _pulseFamily = Family.All(typeof(PulseComponent), typeof(SpriteComponent)).Get();

        readonly ImmutableList<Entity> _pulseEntities;

        public PulseSystem(Engine engine) : base(engine)
        {
            _pulseEntities = Engine.GetEntitiesFor(_pulseFamily);
        }

        protected override void OnUpdate(float dt)
        {
            foreach(Entity pulseEntity in _pulseEntities)
            {
                PulseComponent pulseComp = pulseEntity.GetComponent<PulseComponent>();
                SpriteComponent spriteComp = pulseEntity.GetComponent<SpriteComponent>();
                pulseComp.Elapsed += dt;
                float beta = 0.5f * (float)Math.Cos(2 * MathHelper.Pi / pulseComp.Period * pulseComp.Elapsed) + 0.5f;
                spriteComp.Alpha = MathHelper.Lerp(pulseComp.AlphaMin, pulseComp.AlphaMax, beta);
            }
        }
    }
}
