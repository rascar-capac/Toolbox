using Unity.Mathematics;
using UnityEngine;

namespace Rascar.Toolbox.Extensions
{
    public static class MathematicsExtensions
    {
        public static Vector2 ToVector2(this float3 vector, EAxis ignoredAxis = EAxis.Z)
        {
            return ToVector2((Vector3)vector, ignoredAxis);
        }

        public static Vector3 ToVector3(this float2 vector, EAxis ignoredAxis = EAxis.Z)
        {
            return ToVector3((Vector2)vector, ignoredAxis);
        }
    }
}
