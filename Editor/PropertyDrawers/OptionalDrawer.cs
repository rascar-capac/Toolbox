using Rascar.Toolbox.Serialization;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    public sealed class OptionalDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            string labelString = label.text;

            SerializedProperty enabledProperty = property.FindPropertyRelative("_enabled");
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");

            Rect enabledPropertyRect = position;
            enabledPropertyRect.height = EditorGUI.GetPropertyHeight(enabledProperty);
            enabledPropertyRect.width = enabledPropertyRect.height;

            EditorGUI.PropertyField(enabledPropertyRect, enabledProperty, GUIContent.none);

            position.x = enabledPropertyRect.x + enabledPropertyRect.width + 15f;
            position.width -= enabledPropertyRect.width + 15f;

            EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
            EditorGUI.PropertyField(position, valueProperty, new GUIContent(labelString), true);
            EditorGUI.EndDisabledGroup();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative("_value");

            return EditorGUI.GetPropertyHeight(valueProperty);
        }
    }
}
