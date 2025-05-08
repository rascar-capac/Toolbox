using System;
using UnityEngine;

namespace Rascar.Toolbox.UI
{
    public class ColorSettings : ScriptableObject
    {
        public const string COLOR_SETTINGS_FOLDER = "_Game/Settings";
        public const string COLOR_SETTINGS_PATH = "Assets/" + COLOR_SETTINGS_FOLDER + "/ColorSettings.asset";

        private static ColorSettings _asset;

        public static ColorSettings Asset
        {
            get
            {
                if (_asset == null)
                {
                    _asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ColorSettings>(COLOR_SETTINGS_PATH);
                }

                return _asset;
            }
        }

        [SerializeField] private ColorPaletteInfo _colorPalette;

        public ColorPaletteInfo ColorPalette => _colorPalette;

        public static bool TryGetSettings(out ColorSettings settings)
        {
            settings = Asset;

            return settings != null;
        }

#if UNITY_EDITOR
        public static ColorSettings CreateSettings()
        {
            if (Asset != null)
            {
                return _asset;
            }

            _asset = CreateInstance<ColorSettings>();

            if (!UnityEditor.AssetDatabase.IsValidFolder($"Assets/{COLOR_SETTINGS_FOLDER}"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", COLOR_SETTINGS_FOLDER);
            }

            UnityEditor.AssetDatabase.CreateAsset(_asset, COLOR_SETTINGS_PATH);
            UnityEditor.AssetDatabase.SaveAssets();

            Debug.Log($"ColorSettings asset created at {COLOR_SETTINGS_PATH}.");

            return _asset;
        }

        public static bool TryGetSerializedSettings(out UnityEditor.SerializedObject settings)
        {
            settings = null;

            if (Asset != null)
            {
                settings = new UnityEditor.SerializedObject(_asset);

                return true;
            }

            return false;
        }
#endif
    }

    [Serializable]
    public struct ColorPaletteInfo
    {
        [SerializeField] private Color _white;
        [SerializeField] private Color _black;
        [SerializeField] private Color _disabled;
        [SerializeField] private Color _full;
        [SerializeField] private Color _positive;
        [SerializeField] private Color _medium;
        [SerializeField] private Color _negative;
        [SerializeField] private Color _primaryText;
        [SerializeField] private Color _secondaryText;
        [SerializeField] private Color _emphasis;
        [SerializeField] private Color _lightEmphasis;
        [SerializeField] private Color _hover;

        public readonly Color GetColor(EColor colorType)
        {
            return colorType switch
            {
                EColor.White => _white,
                EColor.Black => _black,
                EColor.Disabled => _disabled,
                EColor.Full => _full,
                EColor.Positive => _positive,
                EColor.Medium => _medium,
                EColor.Negative => _negative,
                EColor.PrimaryText => _primaryText,
                EColor.SecondaryText => _secondaryText,
                EColor.Emphasis => _emphasis,
                EColor.LightEmphasis => _lightEmphasis,
                EColor.Hover => _hover,
                _ => throw new NotImplementedException()
            };
        }
    }

    public enum EColor
    {
        White = 1,
        Black = 2,
        Disabled = 3,
        Positive = 4,
        Medium = 5,
        Negative = 6,
        PrimaryText = 7,
        SecondaryText = 8,
        Emphasis = 9,
        Hover = 10,
        Full = 11,
        LightEmphasis = 12
    }
}
