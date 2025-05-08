using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rascar.Toolbox.Extensions
{
    public static class TransformExtensions
    {
        public static void SetParent(this Transform transform, Transform parent, bool resetLocalPosition = true, bool resetLocalRotation = true, bool resetLocalScale = false)
        {
            transform.parent = parent;

            if (resetLocalPosition)
            {
                transform.localPosition = Vector3.zero;
            }

            if (resetLocalRotation)
            {
                transform.localRotation = Quaternion.identity;
            }

            if (resetLocalScale)
            {
                transform.localScale = Vector3.one;
            }
        }

        public static void SetX(this Transform transform, float x)
        {
            Vector3 newPosition = new(x, transform.position.y, transform.position.z);

            transform.position = newPosition;
        }

        public static void SetY(this Transform transform, float y)
        {
            Vector3 newPosition = new(transform.position.x, y, transform.position.z);

            transform.position = newPosition;
        }

        public static void SetZ(this Transform transform, float z)
        {
            Vector3 newPosition = new(transform.position.x, transform.position.y, z);

            transform.position = newPosition;
        }

        public static void SetLocalX(this Transform transform, float x)
        {
            Vector3 newPosition = new(x, transform.localPosition.y, transform.localPosition.z);

            transform.localPosition = newPosition;
        }

        public static void SetLocalY(this Transform transform, float y)
        {
            Vector3 newPosition = new(transform.localPosition.x, y, transform.localPosition.z);

            transform.localPosition = newPosition;
        }

        public static void SetLocalZ(this Transform transform, float z)
        {
            Vector3 newPosition = new(transform.localPosition.x, transform.localPosition.y, z);
            transform.localPosition = newPosition;
        }

        public static void CopyTransform(this Transform transform, Transform source)
        {
            transform.SetPositionAndRotation(source.position, source.rotation);
        }

        public static void CopyLocalTransform(this Transform transform, Transform source)
        {
            transform.SetLocalPositionAndRotation(source.localPosition, source.localRotation);
        }

        public static void Reset(this Transform transform)
        {
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public static void ResetLocal(this Transform transform)
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static void DestroyImmediateChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }

        public static void SetScale(this Transform transform, float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }

        public static Transform FindRecursive(this Transform transform, string name)
        {
            if (transform.name == name)
            {
                return transform;
            }

            List<Transform> recursiveList = new() { transform };

            for (int index = 0; index < recursiveList.Count; index++)
            {
                Transform parent = recursiveList[index];

                foreach (Transform child in parent)
                {
                    if (child.name == name)
                    {
                        return child;
                    }

                    if (child.childCount > 0)
                    {
                        recursiveList.Add(child);
                    }
                }
            }

            return null;
        }

        public static void PositionRelativeTo(this Transform transform, Transform targetTransform)
        {
            transform.SetPositionAndRotation(targetTransform.position + transform.localPosition, transform.localRotation);
        }
    }
}
