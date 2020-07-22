using System;
using Microsoft.Xna.Framework;

namespace GameJam.Common
{
    /// <summary>
    /// Helpful math functions not present in the .NET framework.
    /// </summary>
    public static class MathUtils
    {
        public static float InverseLerp(float a, float b, float v)
        {
            return (v - a) / (b - a);
        }

        public static float LerpAngle(float a, float b, float alpha)
        {
            // Reference: https://stackoverflow.com/a/14498790
            float shortestAngle = ((b - a) + MathHelper.Pi) % MathHelper.TwoPi - MathHelper.Pi;
            return a + (shortestAngle * alpha) % MathHelper.TwoPi;
        }

        public static Vector2 RotateVector(Vector2 vector, float cos, float sin)
        {
            return new Vector2(vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos);
        }
    }
}
