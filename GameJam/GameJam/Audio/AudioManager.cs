using Audrey;
using Events;
using GameJam.Events.Audio;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Audio
{
    public enum SoundType
    {
        SoundEffect = 0,
        Music = 1
    }

    public class AudioManager : Process, IEventListener
    {
        private List<SoundEffectInstance> _soundEffects = new List<SoundEffectInstance>();
        ContentManager content;

        public SoundEffectInstance[] CurrentSounds
        {
            get
            {
                return _soundEffects.ToArray();
            }
        }

        public AudioManager(Engine sharedEngine, ContentManager content)
        {
            IsPausable = false;
            this.content = content;
        }

        public void PauseAll()
        {
            foreach (SoundEffectInstance sound in _soundEffects )
            {
                sound.Pause();
            }
        }

        public void StopAll()
        {
            foreach (SoundEffectInstance sound in _soundEffects)
            {
                sound.Stop();
            }
        }

        public void ResumeAll()
        {
            foreach (SoundEffectInstance sound in _soundEffects)
            {
                sound.Resume();
            }
        }

        public void PlaySoundHelper(PlaySoundEvent pse)
        {
            PlaySound(pse.cvarName, pse.eventVolume, pse.pan, pse.pitch, pse.SoundType);
        }
        
        private void PlaySound(string cvarName, float eventVolume, float pan, float pitch, SoundType type)
        {
            SoundEffect sound = content.Load<SoundEffect>(CVars.Get<string>(cvarName));
            SoundEffectInstance instance = sound.CreateInstance();

            if ((int)type == 0)
                instance.Volume = CVars.Get<float>("sound_master_volume") *
                                  CVars.Get<float>("sound_effect_volume") *
                                  eventVolume;
            if ((int)type == 1)
                instance.Volume = CVars.Get<float>("sound_master_volume") *
                                  CVars.Get<float>("sound_music_volume") *
                                  eventVolume;

            instance.Pan = pan;
            instance.Pitch = pitch;
            instance.Play();
        }

        protected override void OnInitialize()
        {
            EventManager.Instance.RegisterListener<PlaySoundEvent>(this);
            EventManager.Instance.RegisterListener<PauseAllSoundsEvent>(this);
            EventManager.Instance.RegisterListener<ResumeAllSoundsEvent>(this);
            EventManager.Instance.RegisterListener<StopAllSoundsEvent>(this);
        }

        protected override void OnKill()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(float dt)
        {
            // check all expired and remove completed
            foreach (SoundEffectInstance sound in _soundEffects)
            {
                if (sound.IsDisposed)
                    _soundEffects.Remove(sound);
            }
        }

        public bool Handle(IEvent evt)
        {
            if (evt is PlaySoundEvent)
                PlaySoundHelper(evt as PlaySoundEvent);
            if (evt is PauseAllSoundsEvent)
                PauseAll();
            if (evt is ResumeAllSoundsEvent)
                ResumeAll();
            if (evt is StopAllSoundsEvent)
                StopAll();

            return false;
        }
    }
}
