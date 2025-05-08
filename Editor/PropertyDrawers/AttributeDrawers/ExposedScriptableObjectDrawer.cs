using Rascar.Toolbox.Attributes;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(ExposedScriptableObjectAttribute), true)]
    public class ExposedScriptableObjectDrawer : PropertyDrawer
    {
        private const float INDENTATION_SIZE = 12f;

        private ExposedScriptableObjectAttribute ScriptableAttribute => (ExposedScriptableObjectAttribute)attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect propertyPosition = new(position);
            propertyPosition.x += INDENTATION_SIZE;
            propertyPosition.width -= INDENTATION_SIZE;
            propertyPosition.height = EditorGUIUtility.singleLineHeight;

            Rect headerPosition = propertyPosition;
            headerPosition.width = EditorGUIUtility.labelWidth - INDENTATION_SIZE;

            if (property.objectReferenceValue == null)
            {
                GUI.enabled = false;
                property.isExpanded = false;
            }

            // was using blue foldout style
            bool propertyIsExpanded = EditorGUI.Foldout(headerPosition, property.isExpanded, label, true, new GUIStyle(EditorStyles.foldout));
            GUI.enabled = true;

            if (ScriptableAttribute._itMustDisplayObjectSelector)
            {
                headerPosition.x += headerPosition.width;
                headerPosition.width = propertyPosition.width - EditorGUIUtility.labelWidth + INDENTATION_SIZE;

                EditorGUI.PropertyField(headerPosition, property, GUIContent.none, true);
            }

            if (property.objectReferenceValue == null)
            {
                return;
            }

            property.isExpanded = propertyIsExpanded;

            if (!property.isExpanded)
            {
                return;
            }

            propertyPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            SerializedObject serializedObject = new(property.objectReferenceValue);
            SerializedProperty childProperty = serializedObject.GetIterator();

            GUI.enabled = false;

            childProperty.NextVisible(true);

            do
            {
                if (childProperty.isArray)
                {
                    EditorGUI.indentLevel++;
                }

                EditorGUI.PropertyField(propertyPosition, childProperty, true);

                propertyPosition.y += EditorGUI.GetPropertyHeight(childProperty);
                propertyPosition.y += EditorGUIUtility.standardVerticalSpacing;

                GUI.enabled = true;

                if (childProperty.isArray)
                {
                    EditorGUI.indentLevel--;
                }
            }
            while (childProperty.NextVisible(false));

            serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (property.objectReferenceValue == null || !property.isExpanded)
            {
                return totalHeight;
            }

            SerializedObject serializedObject = new(property.objectReferenceValue);
            SerializedProperty childProperty = serializedObject.GetIterator();

            childProperty.NextVisible(true);

            do
            {
                totalHeight += EditorGUI.GetPropertyHeight(childProperty);
                totalHeight += EditorGUIUtility.standardVerticalSpacing;
            }
            while (childProperty.NextVisible(false));

            totalHeight += EditorGUIUtility.standardVerticalSpacing;

            return totalHeight;
        }
    }
}
