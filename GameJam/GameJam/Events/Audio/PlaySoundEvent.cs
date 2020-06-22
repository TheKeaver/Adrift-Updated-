using Audrey;
using Events;
using GameJam.Audio;

namespace GameJam.Events.Audio
{
    public class PlaySoundEvent : IEvent
    {
        public string cvarName;
        public float eventVolume;
        public float pan;
        public float pitch;
        public SoundType SoundType;

        public PlaySoundEvent(string cvarName, float volume, float pan, float pitch, SoundType type)
        {
            this.cvarName = cvarName;
            this.eventVolume = volume;
            this.pan = pan;
            this.pitch = pitch;
            SoundType = type;
        }
    }
}
