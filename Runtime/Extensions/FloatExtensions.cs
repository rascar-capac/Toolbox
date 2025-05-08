using System.Globalization;
using Rascar.Toolbox.Utilities;
using UnityEngine;
#if UNITY_LOCALIZATION_INSTALLED
using UnityEngine.Localization.Settings;
#endif

namespace Rascar.Toolbox.Extensions
{
    public static class FloatExtensions
    {
        public static bool ApproximatelyEquals(this float input, float other, float epsilon)
        {
            return MathUtils.Approximately(input, other, epsilon);
        }

        public static float Step(this float input, float step, float minimumValue = float.MinValue, float maximumValue = float.MaxValue)
        {
            return MathUtils.Step(input, step, minimumValue, maximumValue);
        }

        public static bool IsSameSign(this float input, float other, bool zeroIsSame = false)
        {
            return MathUtils.IsSameSign(input, other, zeroIsSame);
        }

        public static float DecimalPart(this float input)
        {
            return Mathf.Repeat(input, 1f);
        }

        public static int ToMilliseconds(this float input)
        {
            return (int)(input * 1000);
        }

        public static string GetCompactFormat(this float number)
        {
            string format;

            if (number >= 1_000_000)
            {
                float displayedNumber = number / 1_000_000f;

                return displayedNumber.GetCompactFormat() + "m";
            }

            if (number >= 10_000)
            {
                float displayedNumber = number / 1_000f;

                return displayedNumber.GetCompactFormat() + "k";
            }

            format = number % 1 == 0 ? "N0" : "N1";

#if UNITY_LOCALIZATION_INSTALLED
            return number.ToString(format, LocalizationSettings.SelectedLocale.Formatter);
#else
            return number.ToString(format, CultureInfo.CurrentCulture);
#endif
        }
    }
}
