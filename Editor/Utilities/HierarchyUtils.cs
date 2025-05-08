using System;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.Utilities
{
    public static class HierarchyUtils
    {
        public static GameObject CreateAndRename(string defaultName, Transform parent = null, bool activeObjectIsDefaultParent = true)
        {
            GameObject gameObject = new(defaultName);

            if (parent == null && activeObjectIsDefaultParent && Selection.activeGameObject != null)
            {
                parent = Selection.activeGameObject.transform;
            }

            if (parent != null)
            {
                gameObject.transform.parent = parent;
                gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            Undo.RegisterCreatedObjectUndo(gameObject, "Created game object");
            Rename(gameObject);

            return gameObject;
        }

        public static void Rename(GameObject gameObject)
        {
            Selection.activeGameObject = gameObject;

            EditorApplication.delayCall += () =>
            {
                Type sceneHierarchyType = Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor");
                EditorWindow.GetWindow(sceneHierarchyType).SendEvent(EditorGUIUtility.CommandEvent("Rename"));
            };
        }
    }
}
