#if UNITY_LOCALIZATION_INSTALLED
using UnityEditor;
using UnityEditor.Localization.UI;

namespace Rascar.Toolbox.Editor.Localization
{
    public class LocalizationMenuItems
    {
        public const string MENU_ITEMS_ROOT_PATH = "RascarToolbox/Localization/";
        public const string GOOGLE_SYNC_WINDOW_PATH = MENU_ITEMS_ROOT_PATH + "GoogleSync";
        public const string LOCALIZATION_TABLES_WINDOW_PATH = MENU_ITEMS_ROOT_PATH + "Localization Tables";

        [MenuItem(GOOGLE_SYNC_WINDOW_PATH)]
        public static void OpenGoogleSyncWindow()
        {
            EditorWindow.GetWindow<GoogleSyncWindow>();
        }

        [MenuItem(LOCALIZATION_TABLES_WINDOW_PATH)]
        public static void OpenLocalizationTablesWindow()
        {
            EditorWindow.GetWindow<LocalizationTablesWindow>();
        }
    }

}
#endif