using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Particles
{
    public class ParticleManager<T> : Process
    {
        Action<Particle, float> _updateParticle;
        CirculateParticleArray _particles;

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

		public ref T CreateParticle(TextureRegion2D texture,
            float x,
            float y,
            Color color,
            float duration,
            float scaleX,
            float scaleY,
            float rotation = 0)
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
            particle.Position.X = x;
            particle.Position.Y = y;
            particle.Rotation = rotation;
			particle.Color = color;

			particle.Duration = duration;
            particle.Elapsed = 0;
            particle.Expired = false;
            particle.Scale.X = scaleX;
            particle.Scale.Y = scaleY;

            return ref particle.UserInfo;
		}

		protected override void OnUpdate(float dt)
		{
            if(!CVars.Get<bool>("particle_enable"))
            {
				return;
            }

			int removalCount = 0;
			for (int i = 0; i < _particles.Count; i++)
			{
				Particle particle = _particles[i];
				_updateParticle(particle, dt);
                particle.Elapsed += dt;

				// Sift deleted particles to the end of the list
				_particles.Swap(i - removalCount, i);

				// If particle has expired, delete particle
				if (particle.Expired)
				{
					removalCount++;
				}
			}
			_particles.Count -= removalCount;

			GameManager.StatisticsProfiler.PushParticleCount(_particles.Count);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!CVars.Get<bool>("particle_enable"))
			{
				return;
			}

			Vector2 origin = Vector2.Zero;
            TextureRegion2D texture = null;
			for (int i = 0; i < _particles.Count; i++)
			{
				Particle particle = _particles[i];

                if(particle.Texture != texture)
                {
                    texture = _particles[i].Texture;
                    origin = new Vector2(particle.Texture.Width / 2, particle.Texture.Height / 2);
                }

				spriteBatch.Draw(particle.Texture,
					particle.Position, 
					particle.Color,
					particle.Rotation,
					origin,
					particle.Scale,
					SpriteEffects.None,
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
			public TextureRegion2D Texture = null;
			public Vector2 Position = Vector2.Zero;
			public float Rotation;

			public Vector2 Scale = Vector2.One;

			public Color Color = Color.TransparentBlack;
			public float Duration;
            public float Elapsed;
            public bool Expired;
            public T UserInfo = (T)Activator.CreateInstance(typeof(T));
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
