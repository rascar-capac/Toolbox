using Rascar.Toolbox.SerializableComparables;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ISerializableComparable<,>), useForChildren: true)]
    public class SerializableComparableDrawer : PropertyDrawer
    {
        private const string OPERATOR_NAME = "_operator";
        private const string VALUE_NAME = "_value";
        private const float LABEL_EXTRA_WIDTH = 15f;
        private const float FOLDOUT_ARROW_SPACING = 15f;
        private const float OPERATOR_WIDTH = 50f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty operatorProperty = property.FindPropertyRelative(OPERATOR_NAME);
            SerializedProperty valueProperty = property.FindPropertyRelative(VALUE_NAME);

            float valueLabelWidth = EditorStyles.label.CalcSize(label).x + LABEL_EXTRA_WIDTH;
            float remainingWidth = position.width;

            Rect labelPropertyRect = position;
            labelPropertyRect.height = EditorGUIUtility.singleLineHeight;
            labelPropertyRect.width = valueLabelWidth;
            remainingWidth -= valueLabelWidth;

            Rect operatorPropertyRect = position;
            operatorPropertyRect.x = labelPropertyRect.x + valueLabelWidth;
            operatorPropertyRect.height = EditorGUIUtility.singleLineHeight;
            operatorPropertyRect.width = OPERATOR_WIDTH;
            remainingWidth -= OPERATOR_WIDTH;

            Rect valuePropertyRect = position;

            if (valueProperty.hasChildren)
            {
                labelPropertyRect.x += FOLDOUT_ARROW_SPACING;
                operatorPropertyRect.x += FOLDOUT_ARROW_SPACING;
                valuePropertyRect.x = position.x + FOLDOUT_ARROW_SPACING;
                valuePropertyRect.width = position.width;
            }
            else
            {
                valuePropertyRect.x = operatorPropertyRect.x + operatorPropertyRect.width;
                valuePropertyRect.width = remainingWidth;
            }

            EditorGUI.LabelField(labelPropertyRect, label);

            EditorGUI.PropertyField(operatorPropertyRect, operatorProperty, GUIContent.none);
            EditorGUI.PropertyField(valuePropertyRect, valueProperty, GUIContent.none, includeChildren: true);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative(VALUE_NAME);

            return EditorGUI.GetPropertyHeight(valueProperty);
        }
    }
}
