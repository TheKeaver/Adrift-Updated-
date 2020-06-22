using System;
using Microsoft.Xna.Framework;

namespace GameJam.Particles
{
    public struct VelocityParticleInfo
    {
        public Vector2 Velocity;
        public float LengthMultiplier;

        public static void UpdateParticle(ParticleManager<VelocityParticleInfo>.Particle particle, float dt)
        {
            Vector2 velocity = particle.UserInfo.Velocity;

            // If within tolerance, set the velocity to zero.
            // We don't have to use sqrt here, so we shouldn't
            if (Math.Abs(velocity.X) + Math.Abs(velocity.Y) < 0.1f)
            {
                velocity = Vector2.Zero;
                particle.Expired = true;
            }

            particle.Position += velocity * dt;
            particle.Rotation = (float)Math.Atan2(velocity.Y, velocity.X);

            float speed = velocity.Length();
            float alpha = Math.Min(1, speed * dt);
            alpha *= alpha;

            particle.Color.A = (byte)(255 * alpha); // This worked in LibGDX but not in MonoGame; probably won't fix

            particle.Scale.X = particle.UserInfo.LengthMultiplier * Math.Min(Math.Min(1, 0.2f * speed * dt + 0.1f), alpha);

            velocity *= (float)(Math.Pow(CVars.Get<float>("particle_explosion_decay_multiplier"), dt * 144));  // Decay multiplier was determined at 144Hz, this fixes the issue of explosion size being different on different refresh rates

            particle.UserInfo.Velocity = velocity;
        }
    }
}
