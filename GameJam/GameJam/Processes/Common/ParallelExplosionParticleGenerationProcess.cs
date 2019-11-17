using System;
using System.Collections.Generic;
using GameJam.Common;
using GameJam.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Processes.Common
{
    public struct ColoredExplosion
    {
        public Vector2 Position;
        public Color Color;
    }

    public class ParallelExplosionParticleGenerationProcess : ParallelProcess
    {
        private readonly ParticleManager<VelocityParticleInfo> _particleManager;
        private readonly MTRandom _random;

        private readonly Texture2D _particleTexture;

        private List<ColoredExplosion> _explosionsQueue = new List<ColoredExplosion>();

        public ParallelExplosionParticleGenerationProcess(ParticleManager<VelocityParticleInfo> particleManager,
            Texture2D particleTexture)
        {
            _particleManager = particleManager;
            _random = new MTRandom();

            _particleTexture = particleTexture;
        }

        protected override void OnInitialize()
        {
            RequestWorkInBackground = true;
        }

        protected override void OnKill()
        {
        }

        protected override void OnRun(float dt)
        {
            int explosionCount = _explosionsQueue.Count;
            int particleCount = explosionCount * CVars.Get<int>("particle_explosion_count");
            if (explosionCount > 0)
            {
                for (int i = 0; i < explosionCount; i++)
                {
                    CreateExplosion(_explosionsQueue[i].Position, CVars.Get<int>("particle_explosion_count"), _explosionsQueue[i].Color);
                }
                _explosionsQueue.RemoveRange(0, explosionCount);
            }
        }

        protected override void OnTogglePause()
        {
        }

        public void QueueExplosion(ColoredExplosion explosion)
        {
            _explosionsQueue.Add(explosion);
        }

        private void CreateExplosion(Vector2 explosionLocation, float particleCount, Color color)
        {
            for (int i = 0; i < particleCount; i++)
            {
                float speed = CVars.Get<float>("particle_explosion_strength")
                    * _random.NextSingle(CVars.Get<float>("particle_explosion_variety_min"), CVars.Get<float>("particle_explosion_variety_max"));
                float dir = _random.NextSingle(0, MathHelper.TwoPi);

                ParticleManager<VelocityParticleInfo>.Particle particle = _particleManager.CreateReservedParticle(_particleTexture,
                                                        explosionLocation.X,
                                                        -explosionLocation.Y, // For various reasons, ParticleManager is flipped
                                                        color,
                                                        float.PositiveInfinity,
                                                        1,
                                                        0.5f,
                                                        0);
                ref VelocityParticleInfo info = ref particle.UserInfo;
                info.Velocity.X = (float)(speed * Math.Cos(dir));
                info.Velocity.Y = (float)(speed * Math.Sin(dir));
                info.LengthMultiplier = 1f;

                particle.Reserved = false;
            }
        }
    }
}
