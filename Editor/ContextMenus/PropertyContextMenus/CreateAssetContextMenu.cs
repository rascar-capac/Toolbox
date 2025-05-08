using System;
using System.Reflection;
using Rascar.Toolbox.Attributes;
using Rascar.Toolbox.Editor.Dropdowns;
using Rascar.Toolbox.Editor.Utilities;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rascar.Toolbox.Editor.ContextMenus.PropertyContextMenus
{
    /// <summary>
    /// Adds a context menu option on ScriptableObject properties to easily create a new asset and reference it to that property.
    /// A dropdown menu allows to select one of the exposed type's implementations.
    /// </summary>
    // TODO: can't we do the same for any type of creatable asset, such as materials?
    public static class CreateAssetContextMenu
    {
        private const float DROPDOWN_WIDTH = 300f;

        private static SerializedObject _targetObject;
        private static SerializedProperty _targetProperty;
        private static SelectImplementationDropdown _dropdown;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.contextualPropertyMenu += AddMenuItem;
        }

        public static void AddMenuItem(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                return;
            }

            Type propertyType = InspectorUtils.GetPropertyType(property);
            Type scriptableObjectType = typeof(ScriptableObject);

            if (!scriptableObjectType.IsAssignableFrom(propertyType))
            {
                return;
            }

            _targetObject = property.serializedObject;
            _targetProperty = property;

            menu.AddItem(new GUIContent("Create asset"), on: false, () =>
            {
                if (_dropdown != null)
                {
                    _dropdown.OnImplementationSelected -= Dropdown_OnImplementationSelected;
                }

                _dropdown = new SelectImplementationDropdown(new AdvancedDropdownState(), propertyType, IsValidImplementation);
                _dropdown.Show(new Rect(Vector2.zero, new Vector2(DROPDOWN_WIDTH, 0f)));
                _dropdown.OnImplementationSelected += Dropdown_OnImplementationSelected;
            });
        }

        private static bool IsValidImplementation(Type type)
        {
            return !type.IsInterface
                && !type.IsAbstract
                && !type.ContainsGenericParameters
                && !type.IsDefined(typeof(HideInCreateAssetDropdownAttribute));
        }

        private static void GetCreateAssetMenuAttributeInfo(Type assetType, out string fileName)
        {
            fileName = string.Empty;

            foreach (CustomAttributeData attribute in assetType.CustomAttributes)
            {
                if (attribute.AttributeType != typeof(CreateAssetMenuAttribute))
                {
                    continue;
                }

                for (int argumentIndex = 0; argumentIndex < attribute.NamedArguments.Count; argumentIndex++)
                {
                    CustomAttributeNamedArgument argumentInfo = attribute.NamedArguments[argumentIndex];

                    if (argumentInfo.MemberName == "fileName")
                    {
                        fileName = (string)argumentInfo.TypedValue.Value;
                    }
                }

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    break;
                }
            }
        }

        private static void CreateAssetOfType(SerializedObject serializedObject, SerializedProperty property, Type assetType)
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (assetPath == "")
            {
                Debug.LogError("No location is selected in the project window. Please lock the inspector to be able to set the focus on a location.");

                return;
            }

            ScriptableObject asset = ScriptableObject.CreateInstance(assetType);
            string assetName = assetType.Name;

            GetCreateAssetMenuAttributeInfo(assetType, out string fileName);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                assetName = fileName;
            }

            if (!AssetDatabase.IsValidFolder(assetPath))
            {
                assetPath = assetPath[..assetPath.LastIndexOf('/')];
            }

            assetPath += $"/{assetName}";
            AddIndexInName(ref assetPath);
            assetPath += ".asset";
            AssetDatabase.CreateAsset(asset, assetPath);

            serializedObject.Update();
            property.objectReferenceValue = asset;
            serializedObject.ApplyModifiedProperties();

            if (serializedObject.targetObject is Component targetComponent)
            {
                Scene scene = targetComponent.gameObject.scene;
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }

            AssetDatabase.SaveAssets();
        }

        private static void AddIndexInName(ref string assetPath)
        {
            string newAssetPath = assetPath;
            int index = 0;

            while (!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID($"{newAssetPath}.asset", AssetPathToGUIDOptions.OnlyExistingAssets)))
            {
                newAssetPath = GetFormatedAssetName(assetPath, index);
                index++;
            }

            assetPath = newAssetPath;
        }

        private static string GetFormatedAssetName(string assetPath, int index)
        {
            return EditorSettings.gameObjectNamingScheme switch
            {
                EditorSettings.NamingScheme.SpaceParenthesis => $"{assetPath} ({index})",
                EditorSettings.NamingScheme.Dot => $"{assetPath}.{index}",
                _ => $"{assetPath}_{index}",
            };
        }

        private static void Dropdown_OnImplementationSelected(Type type)
        {
            CreateAssetOfType(_targetObject, _targetProperty, type);
            _dropdown.OnImplementationSelected -= Dropdown_OnImplementationSelected;
        }
    }
}
