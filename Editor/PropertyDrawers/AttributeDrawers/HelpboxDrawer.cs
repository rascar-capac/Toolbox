using Rascar.Toolbox.Attributes;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.PropertyDrawers.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(HelpboxAttribute))]
    public sealed class HelpboxDrawer : DecoratorDrawer
    {
        private const float MIN_HEIGHT = 40f;
        private const float TOP_MARGIN = 8f;
        private const float BOTTOM_MARGIN = 2f;

        private float _height;

        public override void OnGUI(Rect position)
        {
            HelpboxAttribute helpBoxAttribute = (HelpboxAttribute)attribute;

            GUIContent content = new(helpBoxAttribute.Text);
            _height = GUI.skin.GetStyle("helpbox").CalcHeight(content, EditorGUIUtility.currentViewWidth);
            _height += TOP_MARGIN + BOTTOM_MARGIN;
            _height = Mathf.Max(_height, MIN_HEIGHT);

            position.y += TOP_MARGIN;
            position.height -= TOP_MARGIN + BOTTOM_MARGIN;
            EditorGUI.HelpBox(position, helpBoxAttribute.Text, (MessageType)helpBoxAttribute.Type);
        }

        public override float GetHeight()
        {
            return _height;
        }
    }
}
