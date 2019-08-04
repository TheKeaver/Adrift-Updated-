using System;
using System.Collections.Generic;
using System.Text;
using Audrey;
using Events;
using GameJam.Events;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class SoundDirector : BaseDirector
    {
        SoundEffect explosionFx;
        SoundEffect laserFiredFx;
        SoundEffect projectileBouncedFx;

        public SoundDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
            explosionFx = Content.Load<SoundEffect>(Constants.Resources.SOUND_EXPLOSION);
            laserFiredFx = Content.Load<SoundEffect>(Constants.Resources.SOUND_LASER_FIRED);
            projectileBouncedFx = Content.Load<SoundEffect>(Constants.Resources.SOUND_PROJECTILE_BOUNCE);
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<LaserFiredEvent>(this);
            EventManager.Instance.RegisterListener<CreateExplosionEvent>(this);
            EventManager.Instance.RegisterListener<ProjectileBouncedEvent>(this);
        }

        public override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if (evt is LaserFiredEvent)
            {
                HandleLaserFireEvent(evt as LaserFiredEvent);
            }
            if (evt is CreateExplosionEvent)
            {
                HandleCreateExplosionEvent(evt as CreateExplosionEvent);
            }
            if(evt is ProjectileBouncedEvent)
            {
                HandleProjectileBouncedEvent(evt as ProjectileBouncedEvent);
            }
            return false;
        }

        private void HandleProjectileBouncedEvent(ProjectileBouncedEvent projectileBouncedEvent)
        {
            projectileBouncedFx.Play();
        }

        void HandleLaserFireEvent(LaserFiredEvent evt)
        {
            laserFiredFx.Play();
        }

        void HandleCreateExplosionEvent(CreateExplosionEvent evt)
        {
            explosionFx.Play();
        }
    }
}
