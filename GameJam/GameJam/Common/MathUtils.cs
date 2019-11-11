namespace GameJam.Common
{
    /// <summary>
    /// Helpful math functions not present in the .NET framework.
    /// </summary>
    public static class MathUtils
    {
        public static float InverseLerp(float a, float b, float v)
        {
            return ((v - a) / (b - a));
        }
    }
}
