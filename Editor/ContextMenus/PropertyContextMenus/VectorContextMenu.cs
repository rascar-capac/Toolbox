using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.ContextMenus.PropertyContextMenus
{
    /// <summary>
    /// Adds context menu actions to quickly set values of integer, floating-points and vectors values in inspector.
    /// </summary>
    public static class VectorContextMenu
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.contextualPropertyMenu += AddMenuItems;
        }

        public static void AddMenuItems(GenericMenu menu, SerializedProperty property)
        {
            if (!IsVectorProperty(property))
            {
                return;
            }

            menu.AddItem(new GUIContent("Zero"), on: false, () => SetVectorValue(property, x: 0, y: 0, z: 0, w: 0));
            menu.AddItem(new GUIContent("One"), on: false, () => SetVectorValue(property, x: 1, y: 1, z: 1, w: 1));
        }

        private static bool IsVectorProperty(SerializedProperty property)
        {
            return property.propertyType switch
            {
                SerializedPropertyType.Integer => true,
                SerializedPropertyType.Float => true,
                SerializedPropertyType.Vector2 => true,
                SerializedPropertyType.Vector3 => true,
                SerializedPropertyType.Vector4 => true,
                SerializedPropertyType.Vector2Int => true,
                SerializedPropertyType.Vector3Int => true,
                SerializedPropertyType.Quaternion => true,
                _ => false,
            };
        }

        private static void SetVectorValue(SerializedProperty property, float x = 0, float y = 0, float z = 0, float w = 0)
        {
            property.serializedObject.Update();

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                {
                    property.intValue = (int)x;

                    break;
                }
                case SerializedPropertyType.Float:
                {
                    property.floatValue = x;

                    break;
                }
                case SerializedPropertyType.Vector2:
                {
                    property.vector2Value = new Vector2(x, y);

                    break;
                }
                case SerializedPropertyType.Vector2Int:
                {
                    property.vector2IntValue = new Vector2Int((int)x, (int)y);

                    break;
                }
                case SerializedPropertyType.Vector3:
                {
                    property.vector3Value = new Vector3(x, y, z);

                    break;
                }
                case SerializedPropertyType.Vector3Int:
                {
                    property.vector3IntValue = new Vector3Int((int)x, (int)y, (int)z);

                    break;
                }
                case SerializedPropertyType.Vector4:
                {
                    property.vector4Value = new Vector4(x, y, z, w);

                    break;
                }
                case SerializedPropertyType.Quaternion:
                {
                    property.quaternionValue = Quaternion.Euler(new Vector3(x, y, z));

                    break;
                }
                default:
                {
                    break;
                }
            }

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
