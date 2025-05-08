using UnityEngine;

namespace Rascar.Toolbox.Extensions
{
    public static class CameraExtensions
    {
        /// <summary>
        /// Calculates the frustum intersection of the camera with a given plane.
        /// Return order is bottom left, top left, top right then bottom right.
        /// If the direction of a corner is parallel to the plane, return Vector3.negativeInfinity for this corner.
        /// </summary>
        public static FrustumPlaneIntersection IntersectFrustumWithPlane(this Camera camera, Plane plane, Vector3 offset)
        {
            Ray bottomLeft = camera.ViewportPointToRay(new Vector3(0, 0, 0));
            Ray topLeft = camera.ViewportPointToRay(new Vector3(0, 1, 0));
            Ray topRight = camera.ViewportPointToRay(new Vector3(1, 1, 0));
            Ray bottomRight = camera.ViewportPointToRay(new Vector3(1, 0, 0));
            bottomLeft.origin += offset;
            topLeft.origin += offset;
            topRight.origin += offset;
            bottomRight.origin += offset;

            FrustumPlaneIntersection intersection = new
            (
                plane.Raycast(bottomLeft, out float distance) ? bottomLeft.origin + distance * bottomLeft.direction : Vector3.negativeInfinity,
                plane.Raycast(topLeft, out distance) ? topLeft.origin + distance * topLeft.direction : Vector3.negativeInfinity,
                plane.Raycast(topRight, out distance) ? topRight.origin + distance * topRight.direction : Vector3.negativeInfinity,
                plane.Raycast(bottomRight, out distance) ? bottomRight.origin + distance * bottomRight.direction : Vector3.negativeInfinity
            );

            return intersection;
        }

        public readonly struct FrustumPlaneIntersection
        {
            public readonly Vector3 _bottomLeft;
            public readonly Vector3 _topLeft;
            public readonly Vector3 _topRight;
            public readonly Vector3 _bottomRight;

            public FrustumPlaneIntersection(Vector3 bottomLeft, Vector3 topLeft, Vector3 topRight, Vector3 bottomRight)
            {
                _bottomLeft = bottomLeft;
                _topLeft = topLeft;
                _topRight = topRight;
                _bottomRight = bottomRight;
            }

            public bool IsValid => !_bottomLeft.Equals(Vector3.negativeInfinity)
                                    && !_topLeft.Equals(Vector3.negativeInfinity)
                                    && !_topRight.Equals(Vector3.negativeInfinity)
                                    && !_bottomRight.Equals(Vector3.negativeInfinity);
        }
    }
}
