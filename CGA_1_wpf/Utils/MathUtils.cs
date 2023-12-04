using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.Utils
{
    public static class MathUtils
    {

        public const float PI_Deg = (float)Math.PI * 1f / 180f;
        public const float Epsilon = 1E-5f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToRad(this float angleDegree) => angleDegree * PI_Deg;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToRad(this Vector2 v) => v * PI_Deg;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float ComputeNDotL(Vector3 vertexCenter, Vector3 normal, Vector3 lightPosition) =>
            Math.Max(0, Vector3.Dot(
                Vector3.Normalize(normal),
                Vector3.Normalize(lightPosition - vertexCenter)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float value, float min = 0, float max = 1) => Math.Max(min, Math.Min(value, max));
    }
}
