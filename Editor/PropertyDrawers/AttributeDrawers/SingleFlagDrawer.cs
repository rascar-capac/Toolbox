using System;
using Rascar.Toolbox.Attributes;
using Rascar.Toolbox.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(SingleFlagAttribute))]
    public sealed class SingleFlagDrawer : PropertyDrawer
    {
        private GUIContent[] _displayTextsCache;
        private int[] _enumIndicesCache;

        private void RefreshCache(int arrayLength)
        {
            if (_displayTextsCache == null || _displayTextsCache.Length != arrayLength)
            {
                _displayTextsCache = new GUIContent[arrayLength];
            }

            if (_enumIndicesCache == null || _enumIndicesCache.Length != arrayLength)
            {
                _enumIndicesCache = new int[arrayLength];
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type propertyType = fieldInfo.FieldType;

            if (!propertyType.IsEnum)
            {
                EditorGUI.PropertyField(position, property);

                return;
            }

            Array enumArray = Enum.GetValues(propertyType);
            RefreshCache(enumArray.Length);
            int flagIndex = 0;

            foreach (object displayText in enumArray)
            {
                _displayTextsCache[flagIndex] = new GUIContent(displayText.ToString());
                _enumIndicesCache[flagIndex] = (int)displayText;
                flagIndex++;
            }

            InspectorUtils.DrawMixedValueProperty
            (
                property,
                getter: property => EditorGUI.IntPopup(position, label, property.intValue, _displayTextsCache, _enumIndicesCache),
                setter: (property, newValue) => property.intValue = newValue
            );
        }
    }

}
