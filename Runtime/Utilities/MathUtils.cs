using Rascar.Toolbox.Extensions;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityRandom = UnityEngine.Random;

namespace Rascar.Toolbox.Utilities
{
    public static class MathUtils
    {
        public static readonly float GOLDEN_RATIO = (1 + math.sqrt(5)) / 2;

        public static Vector3 CatmulRomInterpolation(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float percentage)
        {
            Vector3 a = 0.5f * (2f * point1);
            Vector3 b = 0.5f * (point2 - point0);
            Vector3 c = 0.5f * (2f * point0 - 5f * point1 + 4f * point2 - point3);
            Vector3 d = 0.5f * (-point0 + 3f * point1 - 3f * point2 + point3);

            Vector3 pos = a + (b * percentage) + (percentage * percentage * c) + (percentage * percentage * percentage * d);

            return pos;
        }

        public static bool IsSameSign(float left, float right, bool zeroIsSame = false)
        {
            return left * right > 0 || (zeroIsSame && left * right == 0);
        }

        public static bool IsSameSign(int left, int right, bool zeroIsSame = false)
        {
            return IsSameSign((float)left, right, zeroIsSame);
        }

        /// <summary>
        /// Returns the modulo with always a positive result:<br/>
        /// MathMod( 8, 3 ) returns 2<br/>
        /// MathMod( -8, 3 ) returns 2<br/>
        /// This can be used to get the same behaviour as Mathf.Repeat() but with integers.
        /// </summary>
        public static int MathMod(int dividend, int modulus)
        {
            return (dividend % modulus + modulus) % modulus;
        }

        public static void DegreesAngleToDirection(float angle, out float2 direction)
        {
            RadiansAngleToDirection(math.radians(angle), out direction);
        }

        public static void RadiansAngleToDirection(float angle, out float2 direction)
        {
            math.sincos(angle, out float y, out float x);
            direction = new float2(x, y);
        }

        public static void MinMax(float a, float b, out float min, out float max)
        {
            min = math.min(a, b);
            max = a + b - min;
        }

        public static void MinMax(in float2 a, in float2 b, out float2 min, out float2 max)
        {
            min = math.min(a, b);
            max = a + b - min;
        }

        public static bool Approximately(float left, float right, float epsilon)
        {
            return math.abs(left - right) <= epsilon;
        }

        public static bool SmallerOrApproximately(float left, float right, float epsilon)
        {
            return left <= right || Approximately(left, right, epsilon);
        }

        public static bool LargerOrApproximately(float left, float right, float epsilon)
        {
            return left >= right || Approximately(left, right, epsilon);
        }

        public static float Step(float input, float step, float minimumValue = float.MinValue, float maximumValue = float.MaxValue)
        {
            if (input <= minimumValue)
            {
                return minimumValue;
            }

            if (input >= maximumValue)
            {
                return maximumValue;
            }

            float steppedValue = Mathf.Round(input / step) * step;
            float clampedValue = Mathf.Clamp(steppedValue, minimumValue, maximumValue);

            return clampedValue;
        }

        public static void Perpendicular(in float2 input, out float2 result)
        {
            result = new float2(-input.y, input.x);
        }

        public static float SafeDivide(float left, float right, float safeResult = 0f)
        {
            return right == 0f ? safeResult : left / right;
        }

        public static float SqrDistanceToSegment(in float2 segmentPointA, in float2 segmentPointB, in float2 pointC)
        {
            return SqrDistanceToSegment(new float4(segmentPointA.xy, 0f, 0f), new float4(segmentPointB.xy, 0f, 0f), new float4(pointC.xy, 0f, 0f));
        }

        public static float SqrDistanceToSegment(in float3 segmentPointA, in float3 segmentPointB, in float3 pointC)
        {
            return SqrDistanceToSegment(new float4(segmentPointA.xyz, 0f), new float4(segmentPointB.xyz, 0f), new float4(pointC.xyz, 0f));
        }

        public static float SqrDistanceToSegment(in float4 segmentPointA, in float4 segmentPointB, in float4 pointC)
        {
            if (math.all(segmentPointB == segmentPointA))
            {
                return math.distancesq(pointC, segmentPointA);
            }

            float4 directionAB = segmentPointB - segmentPointA;
            float4 directionAC = pointC - segmentPointA;
            float4 directionBC = pointC - segmentPointB;
            float projectAC_AB = math.dot(directionAC, directionAB);

            if (projectAC_AB <= 0f)
            {
                return math.dot(directionAC, directionAC);
            }

            float sqrMagnitudeAB = math.dot(directionAB, directionAB);

            if (projectAC_AB >= sqrMagnitudeAB)
            {
                return math.dot(directionBC, directionBC);
            }

            return math.dot(directionAC, directionAC) - projectAC_AB * projectAC_AB / sqrMagnitudeAB;
        }

        public static bool ClosestPointOnSegment(in float2 segmentStart, in float2 segmentEnd, in float2 point, out float2 result)
        {
            ClosestPointOnSegment(new float4(segmentStart.xy, 0f), new float4(segmentEnd.xy, 0f), new float4(point.xy, 0f), out float4 result4);
            result = result4.xy;

            return true;
        }

        public static void ClosestPointOnSegment(in float3 segmentPointA, in float3 segmentPointB, in float3 pointC, out float3 result)
        {
            ClosestPointOnSegment(new float4(segmentPointA.xyz, 0f), new float4(segmentPointB.xyz, 0f), new float4(pointC.xyz, 0f), out float4 result4);
            result = result4.xyz;
        }

        public static void ClosestPointOnSegment(in float4 segmentPointA, in float4 segmentPointB, in float4 pointC, out float4 result)
        {
            if (math.all(segmentPointB == segmentPointA))
            {
                result = segmentPointA;

                return;
            }

            float4 directionAB = segmentPointB - segmentPointA;
            float4 directionAC = pointC - segmentPointA;
            float projectAC_AB = math.dot(directionAC, directionAB);

            if (projectAC_AB <= 0f)
            {
                result = segmentPointA;

                return;
            }

            float sqrMagnitudeAB = math.dot(directionAB, directionAB);

            if (projectAC_AB >= sqrMagnitudeAB)
            {
                result = segmentPointB;

                return;
            }

            result = segmentPointA + math.unlerp(0f, sqrMagnitudeAB, projectAC_AB) * directionAB;
        }

        public static bool PointIsOnSegment(in float2 segmentStart, in float2 segmentEnd, in float2 point, float epsilon)
        {
            float2 startToPoint = point - segmentStart;
            float2 startToEnd = segmentEnd - segmentStart;
            float crossProduct = startToPoint.y * startToEnd.x - startToPoint.x * startToEnd.y;

            if (math.abs(crossProduct) > epsilon)
            {
                return false;
            }

            float dotProduct = math.dot(startToPoint, startToEnd);

            if (dotProduct < 0f)
            {
                return false;
            }

            float squaredLength = math.dot(startToEnd, startToEnd);

            if (dotProduct > squaredLength)
            {
                return false;
            }

            return true;
        }

        // non-optimized
        public static bool TryGetSegmentSegmentIntersection(in float2 firstPoint1, in float2 secondPoint1,
            in float2 firstPoint2, in float2 secondPoint2, float epsilon, out float2 intersectPoint)
        {
            if (!TryGetLineLineIntersection(in firstPoint1, in secondPoint1, in firstPoint2, in secondPoint2, epsilon, out intersectPoint))
            {
                return false;
            }

            return PointIsOnSegment(in firstPoint1, in secondPoint1, in intersectPoint, epsilon)
                && PointIsOnSegment(in firstPoint2, in secondPoint2, in intersectPoint, epsilon);
        }

        public static bool TryGetDirectionSegmentIntersection(in float2 start1, in float2 direction1,
            in float2 start2, in float2 end2, float epsilon, out float2 intersectPoint)
        {
            return TryGetLineLineIntersection(in start1, start1 + direction1, in start2, end2, epsilon, out intersectPoint)
                && PointIsOnSegment(in start2, in end2, in intersectPoint, epsilon);
        }

        public static bool TryGetLineLineIntersection(in float2 start1, in float2 end1, in float2 start2, in float2 end2, float epsilon, out float2 intersectPoint)
        {
            float2 startToEnd1 = start1 - end1;
            float2 startToEnd2 = start2 - end2;
            float intersectDenominator = startToEnd1.x * startToEnd2.y - startToEnd1.y * startToEnd2.x;

            if (Approximately(intersectDenominator, 0f, epsilon))
            {
                intersectPoint = float2.zero;

                return false;
            }

            float first = start1.x * end1.y - start1.y * end1.x;
            float second = start2.x * end2.y - start2.y * end2.x;

            intersectPoint.x = (first * startToEnd2.x - second * startToEnd1.x) / intersectDenominator;
            intersectPoint.y = (first * startToEnd2.y - second * startToEnd1.y) / intersectDenominator;

            return true;
        }

        public static bool TryGetDirectionDirectionIntersection(in float2 start1, in float2 direction1, in float2 start2,
            in float2 direction2, float epsilon, out float2 intersectPoint)
        {
            return TryGetLineLineIntersection(in start1, start1 + direction1, in start2, start2 + direction2, epsilon, out intersectPoint);
        }

        public static void Mirror(in float3 direction, in float3 normal, out float3 mirror)
        {
            mirror = math.reflect(direction, math.cross(normal, new float3(0f, 1f, 0f)));
        }

        /// <summary>
        /// -1 is left, 1 is right, 0 is forward or backward.
        /// </summary>
        public static float AngleDirection(in float3 forward, in float3 targetForward, in float3 upVector)
        {
            float3 perpendicular = math.cross(forward, targetForward);
            float direction = math.dot(perpendicular, upVector);

            if (direction > 0f)
            {
                return 1f;
            }

            if (direction < 0f)
            {
                return -1f;
            }

            return 0f;
        }

        public static float GetAngleFromDirection(in float3 direction)
        {
            return math.degrees(math.atan2(direction.z, direction.x));
        }

        /// <summary>
        /// Returns whether the other transform is to the right of the original transform.
        /// </summary>
        public static bool IsToTheRight(Transform original, Transform other)
        {
            Vector3 originalObjectPosition = original.position;
            Vector3 otherObjectPosition = other.position;

            Vector3 originalObjectLocalPosition = original.InverseTransformPoint(originalObjectPosition);
            Vector3 otherObjectLocalPosition = original.InverseTransformPoint(otherObjectPosition);

            return otherObjectLocalPosition.x > originalObjectLocalPosition.x;
        }

        /// <summary>
        /// Returns whether the other position is to the right of the original position.
        /// </summary>
        public static bool IsToTheRight(in float3 originalPosition, in float3 originalRightDirection, in float3 otherPosition)
        {
            float3 directionToOther = otherPosition - originalPosition;

            return math.dot(directionToOther, originalRightDirection) >= 0;
        }

        public static float GetAngularSpeed(float movementSpeed, float turnRadius)
        {
            return math.degrees(movementSpeed / turnRadius);
        }

        public static bool PointIsInsideRectangle(in float2 position, in float2 a, in float2 b, in float2 c)
        {
            return PointIsInsideRectangle(new float4(position.xy, 0f, 0f), new float4(a.xy, 0f, 0f), new float4(b.xy, 0f, 0f), new float4(c.xy, 0f, 0f));
        }

        public static bool PointIsInsideRectangle(in float3 position, in float3 a, in float3 b, in float3 c)
        {
            return PointIsInsideRectangle(new float4(position.xyz, 0f), new float4(a.xyz, 0f), new float4(b.xyz, 0f), new float4(c.xyz, 0f));
        }

        public static bool PointIsInsideRectangle(in float4 position, in float4 a, in float4 b, in float4 c)
        {
            float4 ab = b - a;
            float4 ap = position - a;
            float4 ac = c - a;

            float dotAB = math.dot(ab, ab);
            float dotAP_AB = math.dot(ap, ab);
            float dotAC = math.dot(ac, ac);
            float dotAP_AC = math.dot(ap, ac);

            return dotAP_AB >= 0 && dotAP_AB <= dotAB
                && dotAP_AC >= 0 && dotAP_AC <= dotAC;
        }

        public static bool PointIsInsideTriangle(in float2 position, in float2 a, in float2 b, in float2 c)
        {
            float signAB = GetTriangleSignedArea(in a, in b, in position);
            float signBC = GetTriangleSignedArea(in b, in c, in position);
            float signCA = GetTriangleSignedArea(in c, in a, in position);

            bool allClockwiseMargin = signAB <= float.Epsilon && signBC <= float.Epsilon && signCA <= float.Epsilon;
            bool allAnticlockwiseMargin = signAB >= float.Epsilon && signBC >= float.Epsilon && signCA >= float.Epsilon;

            return allClockwiseMargin || allAnticlockwiseMargin;
        }

        public static float GetTriangleSignedArea(in float2 a, in float2 b, in float2 c)
        {
            return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
        }

        public static void GetRandomPointInsideTriangle(in float2 a, in float2 b, in float2 c, out float2 result)
        {
            float r1 = UnityRandom.value;
            float r2 = UnityRandom.value;
            float sqrtR1 = math.sqrt(r1);

            result = (1 - sqrtR1) * a + sqrtR1 * (1 - r2) * b + r2 * sqrtR1 * c;
        }

        public static float GetTriangleArea(in float2 a, in float2 b, in float2 c)
        {
            float ab = math.distance(b, a);
            float bc = math.distance(c, b);
            float ca = math.distance(a, c);
            float s = (ab + bc + ca) / 2f;

            return math.sqrt(s * (s - ab) * (s - bc) * (s - ca));
        }

        public static bool SegmentCircleOverlap(in float2 start, in float2 end, in float2 center, float radius)
        {
            return SqrDistanceToSegment(in start, in end, in center) <= radius * radius;
        }

        public static bool SegmentCapsuleOverlap(in float2 segmentStart, in float2 segmentEnd, in float2 capsuleStart, in float2 capsuleEnd, float radius)
        {
            return SqrDistanceToSegment(capsuleStart, capsuleEnd, segmentStart) <= radius * radius
                || SqrDistanceToSegment(capsuleStart, capsuleEnd, segmentEnd) <= radius * radius
                || SqrDistanceToSegment(segmentStart, segmentEnd, capsuleStart) <= radius * radius
                || SqrDistanceToSegment(segmentStart, segmentEnd, capsuleEnd) <= radius * radius;
        }

        public static bool RectCircleOverlap(in Rect rect, in float2 center, float radius)
        {
            return rect.Contains(center) || math.distancesq(center, math.clamp(center, rect.min, rect.max)) <= radius * radius;
        }

        public static bool RectContainsPointInclusive(in Rect rect, in float2 point)
        {
            return point.x >= rect.xMin
                && point.x <= rect.xMax
                && point.y >= rect.yMin
                && point.y <= rect.yMax;
        }

        public static bool RectSegmentOverlap(in Rect rect, in float2 segmentStart, in float2 segmentEnd)
        {
            return rect.Contains(segmentStart)
                || rect.Contains(segmentEnd)
                || (ClosestPointOnSegment(in segmentStart, in segmentEnd, rect.center, out float2 closestSegmentPoint) && rect.Contains(closestSegmentPoint));
        }

        public static bool RectCapsuleOverlap(in Rect rect, in float2 capsuleStart, in float2 capsuleEnd, float radius)
        {
            return rect.Contains(capsuleStart)
                || rect.Contains(capsuleEnd)
                || (ClosestPointOnSegment(in capsuleStart, in capsuleEnd, rect.center, out float2 closestSegmentPoint)
                    && math.distancesq(closestSegmentPoint, math.clamp(closestSegmentPoint, rect.min, rect.max)) <= radius * radius);
        }

        public static float TriangleWave(float x, float period, float min, float max)
        {
            float amplitude = max - min;
            float xOnPeriod = x / period;

            return 2f * math.abs(amplitude * (xOnPeriod - math.floor(xOnPeriod + 0.5f))) + min;
        }

        /// <inheritdoc cref="ComputeShortestSqrDistanceOnSegment( in float4, in float4, in float4, out float4 )"/>
        public static float ComputeShortestSqrDistanceOnSegment(in float2 segmentStart, in float2 segmentEnd, in float2 pointToCompare, out float2 closestPoint)
        {
            float result = ComputeShortestSqrDistanceOnSegment(new float4(segmentStart.xy, 0f, 0f), new float4(segmentEnd.xy, 0f, 0f), new float4(pointToCompare.xy, 0f, 0f), out float4 closestPoint4);
            closestPoint = closestPoint4.xy;

            return result;
        }

        /// <inheritdoc cref="ComputeShortestSqrDistanceOnSegment( in float4, in float4, in float4, out float4 )"/>
        public static float ComputeShortestSqrDistanceOnSegment(in float3 segmentStart, in float3 segmentEnd, in float3 pointToCompare, out float3 closestPoint)
        {
            float result = ComputeShortestSqrDistanceOnSegment(new float4(segmentStart.xyz, 0f), new float4(segmentEnd.xyz, 0f), new float4(pointToCompare.xyz, 0f), out float4 closestPoint4);
            closestPoint = closestPoint4.xyz;

            return result;
        }

        /// <summary>
        /// Returns the closest point on a segment from the <paramref name="pointToCompare"/>, as well as the distance between them.
        /// </summary>
        public static float ComputeShortestSqrDistanceOnSegment(in float4 segmentStart, in float4 segmentEnd, in float4 pointToCompare, out float4 closestPoint)
        {
            float4 segmentDirection = segmentEnd - segmentStart;
            float t = math.clamp(math.dot(pointToCompare - segmentStart, segmentDirection) / math.dot(segmentDirection, segmentDirection), 0f, 1f);

            closestPoint = segmentStart + t * segmentDirection;

            return math.distancesq(pointToCompare, closestPoint);
        }

        public static Vector3 GetRandomPositionInsideCircle(Vector3 center, float radius, float height = 0f)
        {
            return center + (radius * Random.insideUnitCircle).ToVector3(ignoredAxis: EAxis.Y).SetY(height);
        }

        public static void AddToAverage(ref float average, float value, int oldValueCount)
        {
            average += (value - average) / (oldValueCount + 1);
        }

        public static void RemoveFromAverage(ref float average, float value, int oldValueCount)
        {
            int newValueCount = oldValueCount - 1;
            average = newValueCount == 0 ? 0 : average + (average - value) / newValueCount;
        }

        public static void AddToAverage(ref float2 average, in float2 value, int oldValueCount)
        {
            average += (value - average) / (oldValueCount + 1);
        }

        public static void RemoveFromAverage(ref float2 average, in float2 value, int oldValueCount)
        {
            int newValueCount = oldValueCount - 1;
            average = newValueCount == 0 ? float2.zero : average + (average - value) / newValueCount;
        }

        public static void AddToAverage(ref Vector2 average, in Vector2 value, int oldValueCount)
        {
            average += (value - average) / (oldValueCount + 1);
        }

        public static void RemoveFromAverage(ref Vector2 average, in Vector2 value, int oldValueCount)
        {
            int newValueCount = oldValueCount - 1;
            average = newValueCount == 0 ? Vector2.zero : average + (average - value) / newValueCount;
        }

        public static void AddToAverage(ref float3 average, in float3 value, int oldValueCount)
        {
            average += (value - average) / (oldValueCount + 1);
        }

        public static void RemoveFromAverage(ref float3 average, in float3 value, int oldValueCount)
        {
            int newValueCount = oldValueCount - 1;
            average = newValueCount == 0 ? float3.zero : (average - value) / newValueCount;
        }

        public static void AddToAverage(ref Vector3 average, in Vector3 value, int oldValueCount)
        {
            average += (value - average) / (oldValueCount + 1);
        }

        public static void RemoveFromAverage(ref Vector3 average, in Vector3 value, int oldValueCount)
        {
            int newValueCount = oldValueCount - 1;
            average = newValueCount == 0 ? Vector3.zero : (average - value) / newValueCount;
        }
    }
}
