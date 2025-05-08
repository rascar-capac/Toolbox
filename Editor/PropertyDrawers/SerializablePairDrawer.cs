using Rascar.Toolbox.Serialization;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(SerializablePair<,>))]
    public sealed class SerializableFactionPairPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUIUtility.labelWidth = 100;

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect leftValueRect = new(position.x, position.y, 100, position.height);
            Rect rightValueRect = new(position.x + 105, position.y, 100, position.height);

            EditorGUI.PropertyField(leftValueRect, property.FindPropertyRelative("LeftValue"), GUIContent.none);
            EditorGUI.PropertyField(rightValueRect, property.FindPropertyRelative("RightValue"), GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUIUtility.labelWidth = 0;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

}
