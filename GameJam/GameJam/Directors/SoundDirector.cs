using Audrey;
using Events;
using GameJam.Events.Audio;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class SoundDirector : BaseDirector
    {
        /*SoundEffect explosionFx;
        SoundEffect projectileFiredFx;
        SoundEffect projectileBouncedFx;*/

        string explosionString;
        string projectileFiredString;
        string projectileBouncedString;

        public SoundDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
            /*explosionFx = Content.Load<SoundEffect>("sound_explosion");
            projectileFiredFx = Content.Load<SoundEffect>("sound_projectile_fired");
            projectileBouncedFx = Content.Load<SoundEffect>("sound_projectile_bounce");*/

            explosionString = "sound_explosion";
            projectileFiredString = "sound_projectile_fired";
            projectileBouncedString = "sound_projectile_bounce";
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ProjectileFiredEvent>(this);
            EventManager.Instance.RegisterListener<CreateExplosionEvent>(this);
            EventManager.Instance.RegisterListener<ProjectileBouncedEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if (evt is ProjectileFiredEvent)
            {
                HandleProjectileFiredEvent(evt as ProjectileFiredEvent);
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
            EventManager.Instance.QueueEvent(new PlaySoundEvent(projectileBouncedString, 1.0f, 0.0f, 0.0f, Audio.SoundType.SoundEffect));
        }

        void HandleProjectileFiredEvent(ProjectileFiredEvent evt)
        {
            EventManager.Instance.QueueEvent(new PlaySoundEvent(projectileFiredString, 1.0f, 0.0f, 0.0f, Audio.SoundType.SoundEffect));
        }

        void HandleCreateExplosionEvent(CreateExplosionEvent evt)
        {
            if (evt.PlaySound)
            {
                EventManager.Instance.QueueEvent(new PlaySoundEvent(explosionString, 1.0f, 0.0f, 0.0f, Audio.SoundType.SoundEffect));
            }
        }
    }
}
