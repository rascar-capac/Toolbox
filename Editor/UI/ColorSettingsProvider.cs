using Rascar.Toolbox.UI;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.UI
{
    public class ColorSettingsProvider : SettingsProvider
    {
        private const string SETTINGS_PATH = "RascarToolbox/Colors";
        private const string INITIALIZATION_MENU = "RascarToolbox/Initialize Color Settings Asset";

        private readonly SerializedObject _settings;

        public ColorSettingsProvider() : base(path: SETTINGS_PATH, scopes: SettingsScope.Project)
        {
            if (ColorSettings.TryGetSerializedSettings(out SerializedObject settings))
            {
                _settings = settings;
            }
        }

        public override void OnGUI(string searchContext)
        {
            if (_settings != null)
            {
                EditorGUILayout.PropertyField(_settings.FindProperty("_colorPalette"));
                _settings.ApplyModifiedPropertiesWithoutUndo();
            }
            else
            {
                if (GUILayout.Button("Create New Color Settings Asset"))
                {
                    ColorSettings.CreateSettings();
                }
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new ColorSettingsProvider();
        }

        [MenuItem(INITIALIZATION_MENU, validate = true)]
        private static bool ValidateInitializeColorSettingsAsset()
        {
            return ColorSettings.Asset == null;
        }

        [MenuItem(INITIALIZATION_MENU)]
        private static void InitializeColorSettingsAsset()
        {
            ColorSettings.CreateSettings();
        }
    }
}
