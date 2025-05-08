using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rascar.Toolbox.Editor.Utilities
{
    public static class ProjectUtils
    {
        public static Object[] GetAllInstances(Type type)
        {
            string[] guids = AssetDatabase.FindAssets("t:" + type.Name);
            Object[] instances = new Object[guids.Length];

            for (int guidIndex = 0; guidIndex < guids.Length; guidIndex++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[guidIndex]);
                instances[guidIndex] = AssetDatabase.LoadAssetAtPath(assetPath, type);
            }

            return instances;
        }

        public static TScriptable[] GetAllInstances<TScriptable>() where TScriptable : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(TScriptable).Name);
            TScriptable[] instances = new TScriptable[guids.Length];

            for (int guidIndex = 0; guidIndex < guids.Length; guidIndex++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[guidIndex]);
                instances[guidIndex] = AssetDatabase.LoadAssetAtPath<TScriptable>(assetPath);
            }

            return instances;
        }
    }
}
