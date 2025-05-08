using System;
using UnityEditor;

namespace Rascar.Toolbox.Editor.Utilities
{
    public static class EditorPrefsUtils
    {
        public static bool TryGetEditorPrefsInt(string key, out int value)
        {
            return TryGetEditorPrefsKey(key, EditorPrefs.GetInt, out value);
        }

        public static bool TryGetEditorPrefsFloat(string key, out float value)
        {
            return TryGetEditorPrefsKey(key, EditorPrefs.GetFloat, out value);
        }

        public static bool TryGetEditorPrefsBool(string key, out bool value)
        {
            return TryGetEditorPrefsKey(key, EditorPrefs.GetBool, out value);
        }

        public static bool TryGetEditorPrefsString(string key, out string value)
        {
            return TryGetEditorPrefsKey(key, EditorPrefs.GetString, out value);
        }

        private static bool TryGetEditorPrefsKey<TKey>(string key, Func<string, TKey> action, out TKey value)
        {
            if (EditorPrefs.HasKey(key))
            {
                value = action(key);

                return true;
            }

            value = default;

            return false;
        }
    }
}
