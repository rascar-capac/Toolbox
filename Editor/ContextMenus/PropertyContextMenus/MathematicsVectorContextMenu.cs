using System;
using Rascar.Toolbox.Editor.Utilities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.ContextMenus.PropertyContextMenus
{
    /// <summary>
    /// Adds context menu actions to quickly set values of <see cref="Unity.Mathematics"/> types in inspector.
    /// </summary>
    public static class MathematicsVectorContextMenu
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.contextualPropertyMenu += AddMenuItems;
        }

        public static void AddMenuItems(GenericMenu menu, SerializedProperty property)
        {
            if (!IsVectorProperty(property, out Type propertyType))
            {
                return;
            }

            menu.AddItem(new GUIContent("Zero"), on: false, () => SetVectorValue(property, propertyType, x: 0, y: 0, z: 0, w: 0));
            menu.AddItem(new GUIContent("One"), on: false, () => SetVectorValue(property, propertyType, x: 1, y: 1, z: 1, w: 1));
        }

        private static bool IsVectorProperty(SerializedProperty property, out Type propertyType)
        {
            propertyType = InspectorUtils.GetPropertyType(property);

            if (propertyType == null)
            {
                return false;
            }

            return propertyType.Equals(typeof(int2))
                || propertyType.Equals(typeof(int3))
                || propertyType.Equals(typeof(int4))
                || propertyType.Equals(typeof(float2))
                || propertyType.Equals(typeof(float3))
                || propertyType.Equals(typeof(float4));
        }

        private static void SetVectorValue(SerializedProperty property, Type propertyType, float x = 0, float y = 0, float z = 0, float w = 0)
        {
            property.serializedObject.Update();

            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                {
                    property.boxedValue = GetBoxedValue(propertyType, x, y, z, w);

                    break;
                }
                default:
                {
                    break;
                }
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        private static object GetBoxedValue(Type propertyType, float x = 0, float y = 0, float z = 0, float w = 0)
        {
            if (propertyType.Equals(typeof(int2)))
            {
                return new int2((int)x, (int)y);
            }

            if (propertyType.Equals(typeof(int3)))
            {
                return new int3((int)x, (int)y, (int)z);
            }

            if (propertyType.Equals(typeof(int4)))
            {
                return new int4((int)x, (int)y, (int)z, (int)w);
            }

            if (propertyType.Equals(typeof(float2)))
            {
                return new float2(x, y);
            }

            if (propertyType.Equals(typeof(float3)))
            {
                return new float3(x, y, z);
            }

            if (propertyType.Equals(typeof(float4)))
            {
                return new float4(x, y, z, w);
            }

            return null;
        }
    }
}
