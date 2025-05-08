using System;
using System.Reflection;
using Rascar.Toolbox.Utilities;
using UnityEditor;

namespace Rascar.Toolbox.Editor.Utilities
{
    public static class InspectorUtils
    {
        public static Type GetPropertyType(SerializedProperty property)
        {
            Type parentType = property.serializedObject.targetObject.GetType();
            Type propertyType = parentType;
            string[] parts = property.propertyPath.Split('.');

            for (int partIndex = 0; partIndex < parts.Length; partIndex++)
            {
                string member = parts[partIndex];

                if (partIndex < parts.Length - 1 && member == "Array" && parts[partIndex + 1].StartsWith("data["))
                {
                    if (ReflectionUtils.TryGetEnumerableElementType(propertyType, out Type elementType))
                    {
                        propertyType = elementType;
                    }

                    partIndex++;

                    continue;
                }

                FieldInfo foundField = null;
                Type currentType = propertyType;

                while (foundField == null && currentType != null)
                {
                    foundField = currentType.GetField(member, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    currentType = currentType.BaseType;
                }

                if (foundField == null)
                {
                    return null;
                }

                propertyType = foundField.FieldType;
            }

            return propertyType;
        }

        public static void DrawMixedValueProperty<TValue>(SerializedProperty serializedProperty, Func<SerializedProperty, TValue> drawer, Action<SerializedProperty, TValue> setter, bool showMixedValue)
        {
            using (new EditorGUI.MixedValueScope(showMixedValue))
            using (EditorGUI.ChangeCheckScope changeScope = new())
            {
                TValue newValue = drawer.Invoke(serializedProperty);

                if (changeScope.changed)
                {
                    setter.Invoke(serializedProperty, newValue);
                }
            }
        }

        public static void DrawMixedValueProperty<TValue>(SerializedProperty serializedProperty, Func<SerializedProperty, TValue> getter, Action<SerializedProperty, TValue> setter)
        {
            DrawMixedValueProperty(serializedProperty, getter, setter, serializedProperty.hasMultipleDifferentValues);
        }
    }
}
