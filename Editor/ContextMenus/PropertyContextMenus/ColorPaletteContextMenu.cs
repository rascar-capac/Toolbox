using System;
using Rascar.Toolbox.UI;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.ContextMenus.PropertyContextMenus
{
    public static class ColorPaletteContextMenu
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.contextualPropertyMenu += AddMenuItem;
        }

        public static void AddMenuItem(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.Color)
            {
                return;
            }

            if (property.serializedObject.targetObject.GetType() == typeof(ColorSettings))
            {
                return;
            }

            SerializedProperty propertyCopy = property.Copy();
            const string rootMenuItem = "Pick Color from Palette";

            if (ColorSettings.TryGetSettings(out ColorSettings settings))
            {
                Array colors = Enum.GetValues(typeof(EColor));

                foreach (EColor colorType in colors)
                {
                    string colorMenuItem = $"{rootMenuItem}/{colorType}";

                    menu.AddItem(new GUIContent(colorMenuItem), false, () =>
                    {
                        propertyCopy.colorValue = settings.ColorPalette.GetColor(colorType);
                        propertyCopy.serializedObject.ApplyModifiedProperties();
                    });
                }
            }
            else
            {
                menu.AddDisabledItem(new GUIContent(rootMenuItem), false);
            }
        }
    }
}
