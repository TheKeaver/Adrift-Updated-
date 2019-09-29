using System;
using Audrey;
using Events;
using GameJam.Common;
using GameJam.Entities;
using GameJam.Events;
using GameJam.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Directors
{
    public class ExplosionDirector : BaseDirector
    {
        private readonly ParticleManager<VelocityParticleInfo> _particleManager;
        private readonly MTRandom _random;
        private readonly Texture2D _particleTexture;

        public ExplosionDirector(Engine engine, ContentManager content,
            ProcessManager processManager,
            ParticleManager<VelocityParticleInfo> particleManager)
            :base(engine, content, processManager)
        {
            _particleManager = particleManager;
            _random = new MTRandom();
            _particleTexture = content.Load<Texture2D>(CVars.Get<string>("texture_particle_velocity"));
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CreateExplosionEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if(evt is CreateExplosionEvent)
            {
                CreateExplosionEvent createExplosionEvent = evt as CreateExplosionEvent;
                CreateExplosionEvent(createExplosionEvent.ExplosionLocation);
            }
            return false;
        }

        private void CreateExplosionEvent(Vector2 explosionLocation)
        {
            for (int i = 0; i < CVars.Get<int>("particle_explosion_count"); i++)
            {
                float speed = CVars.Get<float>("particle_explosion_strength")
                    * _random.NextSingle(CVars.Get<float>("particle_explosion_variety_min"), CVars.Get<float>("particle_explosion_variety_max"));
                float dir = _random.NextSingle(0, MathHelper.TwoPi);
                VelocityParticleInfo info = new VelocityParticleInfo()
                {
                    Velocity = new Vector2((float)(speed * Math.Cos(dir)), (float)(speed * Math.Sin(dir))),
                    LengthMultiplier = 1f
                };

                _particleManager.CreateParticle(_particleTexture,
                                                        explosionLocation * new Vector2(1, -1), // For various reasons, ParticleManager is flipped
                                                        Color.White,
                                                        CVars.Get<float>("particle_explosion_duration"),
                                                        new Vector2(1, 0.5f),
                                                        info);
            }
        }
    }
}
