using Audrey;

namespace GameJam.Components
{
    public class PulseComponent : IComponent
    {
        public float Period;
        public float AlphaMin;
        public float AlphaMax;
        public float Elapsed = 0;

        public PulseComponent(float period, float alphaMin, float alphaMax)
        {
            Period = period;
            AlphaMin = alphaMin;
            AlphaMax = alphaMax;
        }
    }
}
