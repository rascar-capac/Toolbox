using Rascar.Toolbox.Serialization;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(IntegerRange), true)]
    [CustomPropertyDrawer(typeof(FloatRange), true)]
    public class RangeDrawer : PropertyDrawer
    {
        private readonly float _oneLineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded ? 3f * _oneLineHeight : _oneLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect localRectangle = position;
            SerializedProperty minimumValueProperty = property.FindPropertyRelative("_MinimumValue");
            SerializedProperty maximumValueProperty = property.FindPropertyRelative("_MaximumValue");

            EditorGUI.BeginChangeCheck();

            localRectangle.height = EditorGUIUtility.singleLineHeight;

            if (maximumValueProperty.propertyType == SerializedPropertyType.Integer)
            {
                property.isExpanded = EditorGUI.Foldout
                (
                    localRectangle,
                    property.isExpanded,
                    $"{label.text}   [{minimumValueProperty.intValue};{maximumValueProperty.intValue}[",
                    toggleOnLabelClick: true
                );
            }
            else
            {
                property.isExpanded = EditorGUI.Foldout
                (
                    localRectangle,
                    property.isExpanded,
                    $"{label.text}   [{minimumValueProperty.floatValue};{maximumValueProperty.floatValue}]",
                    toggleOnLabelClick: true
                );
            }

            if (property.isExpanded)
            {
                localRectangle = EditorGUI.IndentedRect(localRectangle);

                localRectangle.y += _oneLineHeight;
                EditorGUI.PropertyField(localRectangle, minimumValueProperty);

                localRectangle.y += _oneLineHeight;
                EditorGUI.PropertyField(localRectangle, maximumValueProperty);
            }

            if (EditorGUI.EndChangeCheck())
            {
                switch (minimumValueProperty.propertyType)
                {
                    case SerializedPropertyType.Float:
                    {
                        if (minimumValueProperty.floatValue > maximumValueProperty.floatValue)
                        {
                            maximumValueProperty.floatValue = minimumValueProperty.floatValue + 1f;
                        }

                        break;
                    }

                    case SerializedPropertyType.Integer:
                    {
                        if (minimumValueProperty.intValue > maximumValueProperty.intValue)
                        {
                            maximumValueProperty.intValue = minimumValueProperty.intValue + 1;
                        }

                        break;
                    }

                    default:
                    {
                        Debug.LogWarning($"{property.name} : Range parameter type {minimumValueProperty.propertyType.ToString()} not supported.");

                        break;
                    }
                }
            }
        }
    }
}
