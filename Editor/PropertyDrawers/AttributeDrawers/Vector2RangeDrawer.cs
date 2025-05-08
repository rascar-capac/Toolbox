using Rascar.Toolbox.Serialization;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(Vector2Range))]
    public class Vector2RangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector2)
            {
                EditorGUI.LabelField(position, $"Use {nameof(Vector2RangeDrawer)} with {nameof(Vector2)}.");

                return;
            }

            Vector2Range vector2ComponentsRange = (Vector2Range)attribute;

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();

            Rect rect = EditorGUI.PrefixLabel(position, label);
            Rect[] rectSplitInThree = SplitRectInThree(rect, 36, 5);
            SerializedPropertyType serializedPropertyType = property.propertyType;

            if (serializedPropertyType == SerializedPropertyType.Vector2)
            {
                Vector2 vector2Value = property.vector2Value;
                float min = vector2Value.x;
                float max = vector2Value.y;

                min = EditorGUI.FloatField(rectSplitInThree[0], min);
                max = EditorGUI.FloatField(rectSplitInThree[2], max);
                EditorGUI.MinMaxSlider(rectSplitInThree[1], ref min, ref max, vector2ComponentsRange.MinValue, vector2ComponentsRange.MaxValue);

                vector2Value = new Vector2(min < max ? min : max, max);

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = vector2Value;
                }
            }
            else if (serializedPropertyType == SerializedPropertyType.Vector2Int)
            {
                Vector2Int vector2IntValue = property.vector2IntValue;
                float min = vector2IntValue.x;
                float max = vector2IntValue.y;

                min = EditorGUI.IntField(rectSplitInThree[0], (int)min);
                max = EditorGUI.IntField(rectSplitInThree[2], (int)max);
                EditorGUI.MinMaxSlider(rectSplitInThree[1], ref min, ref max, vector2ComponentsRange.MinValue, vector2ComponentsRange.MaxValue);

                vector2IntValue = new Vector2Int(Mathf.RoundToInt(min < max ? min : max), Mathf.RoundToInt(max));

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2IntValue = vector2IntValue;
                }
            }
            else
            {
                EditorGUI.HelpBox(rect, $"Use {nameof(Vector2Range)} with {nameof(Vector2)} or {nameof(Vector2Int)}.", MessageType.Error);
            }

            EditorGUI.EndProperty();
        }

        private Rect[] SplitRectInThree(Rect rect, int bordersSize, int emptySpace = 0)
        {
            Rect[] splitRects = SplitRect(rect, 3);
            int pad = (int)splitRects[0].width - bordersSize;
            int startAndEndPad = pad + emptySpace;

            splitRects[0].width = splitRects[2].width -= startAndEndPad;
            splitRects[1].width += pad * 2;
            splitRects[1].x -= pad;
            splitRects[2].x += startAndEndPad;

            return splitRects;
        }

        private Rect[] SplitRect(Rect rect, int splitNumber)
        {
            Rect[] splitRects = new Rect[splitNumber];

            for (int splitIndex = 0; splitIndex < splitNumber; ++splitIndex)
            {
                splitRects[splitIndex] = new Rect(rect.x + rect.width / splitNumber * splitIndex, rect.y, rect.width / splitNumber, rect.height);
            }

            return splitRects;
        }
    }
}
