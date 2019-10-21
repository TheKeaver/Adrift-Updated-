using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Particles
{
    public class ParticleManager<T> : Process
    {
        Action<Particle, float> _updateParticle;
        CirculateParticleArray _particles;

        Vector2 FlipY
        {
            get
            {
                return new Vector2(1, -1);
            }
        }

		public ParticleManager(int capacity, Action<Particle, float> updateParticle)
		{
			_updateParticle = updateParticle;
			_particles = new CirculateParticleArray(capacity);

			// Initialize the array with empty particles
			for (int i = 0; i < capacity; i++)
			{
				_particles[i] = new Particle();
			}
		}

		public void CreateParticle(Texture2D texture, Vector2 position, Color color, float duration, Vector2 scale, T userInfo, float rotation = 0)
		{
			Particle particle;
			if (_particles.Count == _particles.Capacity)
			{
				// Rewrite oldest particle; list is full
				particle = _particles[0];
				_particles.Start++;
			}
			else
			{
				particle = _particles[_particles.Count];
				_particles.Count++;
			}

			// Create the particle (populate its values)
			particle.Texture = texture;
			particle.Position = position;
			particle.Rotation = rotation;
			particle.Color = color;

			particle.Duration = duration;
			particle.PercentLife = 1;
			particle.Scale = scale;
			particle.UserInfo = userInfo;
		}

		protected override void OnUpdate(float dt)
		{
			int removalCount = 0;
			for (int i = 0; i < _particles.Count; i++)
			{
				Particle particle = _particles[i];
				_updateParticle(particle, dt);
				particle.PercentLife -= 1f / particle.Duration;

				// Sift deleted particles to the end of the list
				_particles.Swap(i - removalCount, i);

				// If particle has expired, delete particle
				if (particle.PercentLife < 0)
				{
					removalCount++;
				}
			}
			_particles.Count -= removalCount;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < _particles.Count; i++)
			{
				Particle particle = _particles[i];

				Vector2 origin = new Vector2(particle.Texture.Width / 2, particle.Texture.Height / 2);
				spriteBatch.Draw(particle.Texture,
								 particle.Position,
								 null,
								 particle.Color,
								 particle.Rotation,
								 origin,
								 particle.Scale,
								0,
								0);
			}
		}

        protected override void OnInitialize()
        {
        }

        protected override void OnKill()
        {
        }

        protected override void OnTogglePause()
        {
        }

        #region NESTED CLASSES
        public class Particle
		{
			public Texture2D Texture;
			public Vector2 Position;
			public float Rotation;

			public Vector2 Scale = Vector2.One;

			public Color Color;
			public float Duration;
			public float PercentLife = 1f;
			public T UserInfo;
		}

		class CirculateParticleArray
		{
			int _start;
			public int Start
			{
				get
				{
					return _start;
				}
				set
				{
					_start = value % _list.Length;
				}
			}

			public int Count;
			public int Capacity
			{
				get
				{
					return _list.Length;
				}
			}
			Particle[] _list;

			public CirculateParticleArray(int capacity)
			{
				_list = new Particle[capacity];
			}

			public void Swap(int i, int j)
			{
				Particle tmp = _list[i];
				_list[i] = _list[j];
				_list[j] = tmp;
			}

			public Particle this[int i]
			{
				get
				{
					return _list[(_start + i) % _list.Length];
				}
				set
				{
					_list[(_start + i) % _list.Length] = value;
				}
			}
		}
		#endregion
	}
}
