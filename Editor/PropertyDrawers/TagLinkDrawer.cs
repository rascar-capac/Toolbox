using Rascar.Toolbox.Serialization;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(TagLink), true)]
    public class TagLinkDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty tagNameProperty = property.FindPropertyRelative("_TagName");

            tagNameProperty.stringValue = EditorGUI.TagField(position, label, tagNameProperty.stringValue);
        }
    }
}
