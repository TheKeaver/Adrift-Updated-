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
            return ((v - a) / (b - a));
        }

        public static float LerpAngle(float a, float b, float alpha)
        {
            a = MathHelper.WrapAngle(a);
            b = MathHelper.WrapAngle(b);
            if (MathHelper.Distance(a, b) <= MathHelper.Pi)
            {
                return MathHelper.Lerp(a, b, alpha);
            }
            float travel = b - a;
            float travelSign = travel / (float)Math.Abs(travel);
            float inverseTravel = travelSign * MathHelper.TwoPi - travel;
            return inverseTravel * alpha + a;
        }

        public static Vector2 RotateVector(Vector2 vector, float cos, float sin)
        {
            return new Vector2(vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos);
        }
    }
}
