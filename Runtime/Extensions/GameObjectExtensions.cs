using UnityEngine;

namespace Rascar.Toolbox.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool TryGetComponentInChildren<TComponent>(this GameObject target, out Component component, bool includeInactive = false) where TComponent : Component
        {
            component = target.GetComponentInChildren<TComponent>(includeInactive);

            return component != null;
        }

        public static bool TryGetComponentInChildren<TComponent>(this Component target, out Component component, bool includeInactive = false) where TComponent : Component
        {
            return target.gameObject.TryGetComponentInChildren<TComponent>(out component, includeInactive);
        }

        public static bool TryGetComponentInParent<TComponent>(this GameObject unityObject, out TComponent component, bool includeInactive = false)
        {
            component = unityObject.GetComponentInParent<TComponent>(includeInactive);

            return component != null;
        }

        public static bool TryGetComponentInParent<TComponent>(this Component target, out TComponent component, bool includeInactive = false)
        {
            return target.gameObject.TryGetComponentInParent(out component, includeInactive);
        }

        public static void SetLayerRecursive(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;

            foreach (Transform child in gameObject.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }
    }
}
