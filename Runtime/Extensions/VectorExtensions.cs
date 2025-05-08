using System;
using UnityEngine;

namespace Rascar.Toolbox.Extensions
{
    public static class VectorExtensions
    {
        public static float GetSquaredMagnitude(this Vector3 vector, Vector3 otherVector)
        {
            Vector3 distanceVector = vector - otherVector;

            return distanceVector.sqrMagnitude;
        }

        public static float GetHorizontalMagnitude(this Vector3 vector, Vector3 otherVector)
        {
            Vector3 distanceVector = vector - otherVector;
            distanceVector.y = 0;

            return distanceVector.magnitude;
        }

        public static float GetSquaredHorizontalMagnitude(this Vector3 vector, Vector3 otherVector)
        {
            Vector3 distanceVector = vector - otherVector;
            distanceVector.y = 0;

            return distanceVector.sqrMagnitude;
        }

        public static float GetVerticalMagnitude(this Vector3 vector, Vector3 otherVector)
        {
            return Mathf.Abs(vector.y - otherVector.y);
        }

        public static float GetSquaredVerticalMagnitude(this Vector3 vector, Vector3 otherVector)
        {
            float yDistance = vector.y - otherVector.y;

            return yDistance * yDistance;
        }

        public static Vector2 ToVector2(this float input)
        {
            return new Vector2(input, input);
        }

        public static Vector3 ToVector3(this float input)
        {
            return new Vector3(input, input, input);
        }

        public static Vector2 ToVector2(this Vector3 vector, EAxis ignoredAxis = EAxis.Z)
        {
            return ignoredAxis switch
            {
                EAxis.X => new Vector2(vector.y, vector.z),
                EAxis.Y => new Vector2(vector.x, vector.z),
                EAxis.Z => new Vector2(vector.x, vector.y),
                _ => throw new ArgumentException($"Tried to ignore wrong axis type {ignoredAxis}"),
            };
        }

        public static Vector3 ToVector3(this Vector2 vector, EAxis ignoredAxis = EAxis.Z)
        {
            return ignoredAxis switch
            {
                EAxis.X => new Vector3(0f, vector.x, vector.y),
                EAxis.Y => new Vector3(vector.x, 0f, vector.y),
                EAxis.Z => new Vector3(vector.x, vector.y, 0f),
                _ => throw new ArgumentException($"Tried to ignore wrong axis type {ignoredAxis}"),
            };
        }

        public static Vector3 SetX(this Vector3 vector, float newX)
        {
            vector.x = newX;

            return vector;
        }

        public static Vector3 FlattenYAxis(this Vector3 vector)
        {
            return vector.SetY(0f);
        }

        public static Vector3 SetY(this Vector3 vector, float height)
        {
            vector.y = height;

            return vector;
        }

        public static Vector3 SetZ(this Vector3 vector, float newZ)
        {
            vector.z = newZ;

            return vector;
        }

        public static Vector3 AddX(this Vector3 vector, float x)
        {
            vector.x += x;

            return vector;
        }

        public static Vector3 AddY(this Vector3 vector, float y)
        {
            vector.y += y;

            return vector;
        }

        public static Vector3 AddZ(this Vector3 vector, float z)
        {
            vector.z += z;

            return vector;
        }

        public static bool ApproximatelyEquals(this Vector3 vector1, Vector3 vector2, float error)
        {
            return Vector3.SqrMagnitude(vector1 - vector2) <= error * error;
        }


        public static Vector3 MinComponents(this Vector3 vector1, Vector3 vector2)
        {
            return new Vector3
            (
                Mathf.Min(vector1.x, vector2.x),
                Mathf.Min(vector1.y, vector2.y),
                Mathf.Min(vector1.z, vector2.z)
            );
        }

        public static Vector3 MaxComponents(this Vector3 vector1, Vector3 vector2)
        {
            return new Vector3
            (
                Mathf.Max(vector1.x, vector2.x),
                Mathf.Max(vector1.y, vector2.y),
                Mathf.Max(vector1.z, vector2.z)
            );
        }

        public static Vector3 SetMagnitude(this Vector3 vector, float magnitude)
        {
            return magnitude * vector.normalized;
        }

        public static Quaternion LookAtRotation(this Vector3 vector, Vector3 upwards)
        {
            return vector.Equals(Vector3.zero) ? Quaternion.identity : Quaternion.LookRotation(vector, upwards);
        }

        public static Vector2 SetX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        public static Vector2 SetY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        public static Vector2 AddX(this Vector2 vector, float x)
        {
            vector.x += x;

            return vector;
        }

        public static Vector2 AddY(this Vector2 vector, float y)
        {
            vector.y += y;

            return vector;
        }

        public static Vector2 MinComponents(this Vector2 vector1, Vector2 vector2)
        {
            return new Vector2
            (
                Mathf.Min(vector1.x, vector2.x),
                Mathf.Min(vector1.y, vector2.y)
            );
        }

        public static Vector2 MaxComponents(this Vector2 vector1, Vector2 vector2)
        {
            return new Vector2
            (
                Mathf.Max(vector1.x, vector2.x),
                Mathf.Max(vector1.y, vector2.y)
            );
        }

        public static bool IsInRange(this Vector2 vector, Vector2 xRange, Vector2 yRange)
        {
            return vector.x >= xRange.x && vector.x <= xRange.y
                   && vector.y >= yRange.x && vector.y <= yRange.y;
        }

        public static Vector2 SetMagnitude(this Vector2 vector, float magnitude)
        {
            return magnitude * vector.normalized;
        }
    }
}

public enum EAxis
{
    X,
    Y,
    Z,
}
