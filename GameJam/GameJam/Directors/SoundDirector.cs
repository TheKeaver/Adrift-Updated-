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
        public List<SoundEffect> sounds;

        public SoundDirector(Engine engine, ContentManager content, ProcessManager processManager) : base(engine, content, processManager)
        {
            sounds = new List<SoundEffect>();
            sounds.Add(Content.Load<SoundEffect>(Constants.Resources.SOUND_EXPLOSION));
            sounds.Add(Content.Load<SoundEffect>(Constants.Resources.SOUND_LASER_FIRED));
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<LaserFiredEvent>(this);
            EventManager.Instance.RegisterListener <CreateExplosionEvent>(this);
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
            return false;
        }

        void HandleLaserFireEvent(LaserFiredEvent evt)
        {
            sounds[1].Play();
        }

        void HandleCreateExplosionEvent(CreateExplosionEvent evt)
        {
            sounds[0].Play();
        }
    }
}
