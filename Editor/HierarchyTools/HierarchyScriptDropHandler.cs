using System;
using System.Collections.Generic;
using Rascar.Toolbox.Editor.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rascar.Toolbox.Editor.HierarchyTools
{
    /// <summary>
    /// Allows people to drag and drop scripts inside a scene to quickly create empty game objects with the dragged script attached.
    /// </summary>
    [InitializeOnLoad]
    public static class HierarchyScriptDropHandler
    {
        static HierarchyScriptDropHandler()
        {
            DragAndDrop.AddDropHandler(DropHandler);
        }

        private static DragAndDropVisualMode DropHandler(int dropTargetInstanceID, HierarchyDropFlags dropMode, Transform parentForDraggedObjects, bool perform)
        {
            if (parentForDraggedObjects != null)
            {
                return DragAndDropVisualMode.None;
            }

            if (!CanDropScriptWithInstanceID(dropTargetInstanceID, dropMode))
            {
                return DragAndDropVisualMode.None;
            }

            List<MonoScript> monoScripts = GetDraggedScripts();

            if (monoScripts.Count > 0)
            {
                if (perform)
                {
                    CreateScripts(monoScripts, dropTargetInstanceID, dropMode);
                }

                return DragAndDropVisualMode.Copy;
            }

            return DragAndDropVisualMode.None;
        }

        private static void CreateScripts(List<MonoScript> monoScripts, int dropTargetInstanceID, HierarchyDropFlags dropMode)
        {
            GetDropPosition(dropTargetInstanceID, dropMode, out Transform parent, out int siblingIndex);

            for (int index = 0; index < monoScripts.Count; index++)
            {
                MonoScript monoScript = monoScripts[index];
                GameObject gameObject = HierarchyUtils.CreateAndRename(monoScript.name, parent, activeObjectIsDefaultParent: false);
                gameObject.AddComponent(monoScript.GetClass());

                if (siblingIndex >= 0)
                {
                    gameObject.transform.SetSiblingIndex(siblingIndex);
                    siblingIndex++;
                }
            }
        }

        private static List<MonoScript> GetDraggedScripts()
        {
            List<MonoScript> monoScripts = new();

            foreach (Object unityObject in DragAndDrop.objectReferences)
            {
                if (unityObject is MonoScript monoScript)
                {
                    Type type = monoScript.GetClass();

                    if (CanCreateMonoBehaviourInstance(type))
                    {
                        monoScripts.Add(monoScript);
                    }
                }
            }

            return monoScripts;
        }

        private static bool CanDropScriptWithInstanceID(int dropTargetInstanceID, HierarchyDropFlags dropMode)
        {
            return EditorUtility.InstanceIDToObject(dropTargetInstanceID) == null
                || dropMode.HasFlag(HierarchyDropFlags.DropAbove)
                || dropMode.HasFlag(HierarchyDropFlags.DropAfterParent)
                || dropMode.HasFlag(HierarchyDropFlags.DropBetween);
        }

        public static void GetDropPosition(int dropTargetInstanceID, HierarchyDropFlags dropMode, out Transform parent, out int firstSiblingIndex)
        {
            parent = null;
            firstSiblingIndex = -1;
            GameObject dropObject = EditorUtility.InstanceIDToObject(dropTargetInstanceID) as GameObject;

            if (dropObject == null || dropMode.HasFlag(HierarchyDropFlags.SearchActive))
            {
                return;
            }

            parent = dropObject.transform.parent;
            int dropObjectIndex = dropObject.transform.GetSiblingIndex();

            if (dropMode.HasFlag(HierarchyDropFlags.DropAfterParent))
            {
                firstSiblingIndex = 0;
            }
            else if (dropMode.HasFlag(HierarchyDropFlags.DropAbove))
            {
                firstSiblingIndex = dropObjectIndex - 1;
            }
            else if (dropMode.HasFlag(HierarchyDropFlags.DropBetween))
            {
                firstSiblingIndex = dropObjectIndex + 1;
            }
        }

        private static bool CanCreateMonoBehaviourInstance(Type type)
        {
            return type != null
                && type.IsSubclassOf(typeof(MonoBehaviour))
                && !type.IsAbstract
                && !type.ContainsGenericParameters;
        }
    }
}
