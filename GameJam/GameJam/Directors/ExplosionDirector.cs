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
        private readonly ParticleManager<VelocityParticleInfo> _CPUParticleManager;
        private readonly GPUParticleManager _gpuParticleManager;
        private readonly Random _random;
        private readonly TextureRegion2D _particleTexture;

        public ExplosionDirector(Engine engine, ContentManager content,
            ProcessManager processManager,
            ParticleManager<VelocityParticleInfo> cpuParticleManager,
            GPUParticleManager gpuParticleManager)
            :base(engine, content, processManager)
        {
            _CPUParticleManager = cpuParticleManager;
            _gpuParticleManager = gpuParticleManager;
            _random = new Random();
            _particleTexture = content.Load<TextureAtlas>("complete_texture_atlas").GetRegion("texture_particle_velocity");
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
                if (CVars.Get<bool>("particle_gpu_accelerated"))
                {
                    CreateGPUExplosion(createExplosionEvent.ExplosionLocation, createExplosionEvent.Color);
                }
                else
                {
                    CreateCPUExplosion(createExplosionEvent.ExplosionLocation, createExplosionEvent.Color);
                }
            }
            return false;
        }

        private float NextRandomSingle(float min, float max)
        {
            return MathHelper.Lerp(min, max, (float) _random.NextDouble());
        }

        private void CreateCPUExplosion(Vector2 explosionLocation, Color color)
        {
            for (int i = 0; i < CVars.Get<int>("particle_explosion_count"); i++)
            {
                float speed = CVars.Get<float>("particle_explosion_strength")
                    * NextRandomSingle(CVars.Get<float>("particle_explosion_variety_min"), CVars.Get<float>("particle_explosion_variety_max"));
                float dir = NextRandomSingle(0, MathHelper.TwoPi);

                ref VelocityParticleInfo info = ref _CPUParticleManager.CreateParticle(_particleTexture,
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

        private void CreateGPUExplosion(Vector2 explosionLocation, Color color)
        {
            for (int i = 0; i < CVars.Get<int>("particle_explosion_count"); i++)
            {
                float speed = CVars.Get<float>("particle_explosion_strength")
                    * NextRandomSingle(CVars.Get<float>("particle_explosion_variety_min"), CVars.Get<float>("particle_explosion_variety_max"));
                float dir = NextRandomSingle(0, MathHelper.TwoPi);

                _gpuParticleManager.CreateParticle(explosionLocation.X,
                    explosionLocation.Y,
                    (float)(speed * Math.Cos(dir)),
                    (float)(speed * Math.Sin(dir)));
            }
        }
    }
}
