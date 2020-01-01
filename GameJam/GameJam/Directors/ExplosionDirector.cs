using System;
using Audrey;
using Events;
using GameJam.Common;
using GameJam.Events.EnemyActions;
using GameJam.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Directors
{
    public class ExplosionDirector : BaseDirector
    {
        private readonly ParticleManager<VelocityParticleInfo> _particleManager;
        private readonly MTRandom _random;
        private readonly TextureRegion2D _particleTexture;

        public ExplosionDirector(Engine engine, ContentManager content,
            ProcessManager processManager,
            ParticleManager<VelocityParticleInfo> particleManager)
            :base(engine, content, processManager)
        {
            _particleManager = particleManager;
            _random = new MTRandom();
            _particleTexture = content.Load<TextureRegion2D>("texture_particle_velocity");
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CreateExplosionEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if(evt is CreateExplosionEvent)
            {
                CreateExplosionEvent createExplosionEvent = evt as CreateExplosionEvent;
                CreateExplosionEvent(createExplosionEvent.ExplosionLocation, createExplosionEvent.Color);
            }
            return false;
        }

        private void CreateExplosionEvent(Vector2 explosionLocation, Color color)
        {
            for (int i = 0; i < CVars.Get<int>("particle_explosion_count"); i++)
            {
                float speed = CVars.Get<float>("particle_explosion_strength")
                    * _random.NextSingle(CVars.Get<float>("particle_explosion_variety_min"), CVars.Get<float>("particle_explosion_variety_max"));
                float dir = _random.NextSingle(0, MathHelper.TwoPi);

                ref VelocityParticleInfo info = ref _particleManager.CreateParticle(_particleTexture,
                                                        explosionLocation.X,
                                                        -explosionLocation.Y, // For various reasons, ParticleManager is flipped
                                                        color,
                                                        float.PositiveInfinity,
                                                        1,
                                                        0.5f);
                info.Velocity.X = (float)(speed * Math.Cos(dir));
                info.Velocity.Y = (float)(speed * Math.Sin(dir));
                info.LengthMultiplier = 1f;
            }
        }
    }
}
