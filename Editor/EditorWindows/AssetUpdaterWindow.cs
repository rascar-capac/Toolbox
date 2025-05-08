using System;
using System.Collections.Generic;
using System.Linq;
using Rascar.Toolbox.Editor.Dropdowns;
using Rascar.Toolbox.Editor.Utilities;
using Rascar.Toolbox.Utilities;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.TerrainTools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rascar.Toolbox.Editor.EditorWindows
{
    public class AssetUpdaterWindow : EditorWindow
    {
        private const string UPDATE_ASSETS_MENU = "RascarToolbox/Asset Updater";
        private const string OPEN_UPDATE_WINDOW_MENU_ITEM = UPDATE_ASSETS_MENU + "/Open Update Window";
        private const string UPDATE_SELECTION_MENU_ITEM = UPDATE_ASSETS_MENU + "/Update Selected Assets";

        private static readonly IEnumerable<Type> FORBIDDEN_BASE_TYPES = new Type[]
        {
            typeof( UnityEditor.Editor ),
            typeof( EditorWindow ),
            typeof( EditorTool ),
            typeof( EndNameEditAction ),
            typeof( TerrainPaintTool<> ).Assembly.GetType( "UnityEditor.TerrainTools.ITerrainPaintTool" ),
        };

        private readonly SelectImplementationDropdown<ScriptableObject> _selectAssetTypeDropdown = new(new AdvancedDropdownState(), IsValidAssetType);
        private static readonly List<IAssetUpdater> _assetUpdaters = ReflectionUtils.CreateInstances<IAssetUpdater>().ToList();

        private Type _selectedType;
        private Rect _dropdownRect;

        private static bool IsValidAssetType(Type type)
        {
            return !type.IsGenericTypeDefinition
                && !FORBIDDEN_BASE_TYPES.Any(baseType => baseType.IsAssignableFrom(type));
        }

        private void DrawContents()
        {
            string buttonName = _selectedType == null ? "Select type" : _selectedType.ToString();

            if (GUILayout.Button(buttonName, EditorStyles.popup))
            {
                _selectAssetTypeDropdown.Show(_dropdownRect);
            }

            if (Event.current.type == EventType.Repaint)
            {
                _dropdownRect = GUILayoutUtility.GetLastRect();
            }

            using (new EditorGUI.DisabledScope(_selectedType == null))
            {
                if (GUILayout.Button("Update assets"))
                {
                    Object[] assets = ProjectUtils.GetAllInstances(_selectedType);
                    UpdateAssets(assets);
                }
            }
        }

        private static void UpdateAssets(Object[] assets)
        {
            foreach (IAssetUpdater assetUpdater in _assetUpdaters)
            {
                assetUpdater.Prepare();
            }

            for (int assetIndex = 0; assetIndex < assets.Length; assetIndex++)
            {
                Object asset = assets[assetIndex];
                string assetPath = AssetDatabase.GetAssetPath(asset);

                foreach (IAssetUpdater assetUpdater in _assetUpdaters)
                {
                    asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                    if (asset != null)
                    {
                        assetUpdater.TryUpdateAsset(asset);
                    }
                    else
                    {
                        break;
                    }
                }

                if (asset != null)
                {
                    EditorUtility.SetDirty(asset);
                }
            }

            foreach (IAssetUpdater assetUpdater in _assetUpdaters)
            {
                assetUpdater.Finish();
            }

            AssetDatabase.SaveAssets();
        }

        private void SelectImplementationPopup_OnImplementationSelected(Type selectedType)
        {
            _selectedType = selectedType;
        }

        private void OnEnable()
        {
            _selectAssetTypeDropdown.OnImplementationSelected += SelectImplementationPopup_OnImplementationSelected;
        }

        private void OnDisable()
        {
            _selectAssetTypeDropdown.OnImplementationSelected -= SelectImplementationPopup_OnImplementationSelected;
        }

        [MenuItem(OPEN_UPDATE_WINDOW_MENU_ITEM)]
        [MenuItem("Assets/" + OPEN_UPDATE_WINDOW_MENU_ITEM)]
        public static void ShowWindow()
        {
            GetWindow<AssetUpdaterWindow>();
        }

        [MenuItem(UPDATE_SELECTION_MENU_ITEM, validate = true)]
        [MenuItem("Assets/" + UPDATE_SELECTION_MENU_ITEM, validate = true)]
        private static bool ValidateUpdateSelectedAssets()
        {
            return Selection.assetGUIDs != null
                && Selection.assetGUIDs.Length > 0;
        }

        [MenuItem(UPDATE_SELECTION_MENU_ITEM, secondaryPriority = -10f)]
        [MenuItem("Assets/" + UPDATE_SELECTION_MENU_ITEM)]
        private static void UpdateSelectedAssets()
        {
            Object[] assets = Selection.assetGUIDs.Select(guid => AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();
            UpdateAssets(assets);
        }

        private void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("You need to be in editor mode for this window to work!");

                return;
            }

            DrawContents();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }

}
