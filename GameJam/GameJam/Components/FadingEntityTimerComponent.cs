using Audrey;
using GameJam.Common;

namespace GameJam.Components
{
    public class FadingEntityTimerComponent : IComponent
    {
        public Timer timeRemaining;

        public FadingEntityTimerComponent(float maxTimer)
        {
            timeRemaining = new Timer(maxTimer);
        }
    }
}
