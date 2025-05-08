using System;
using System.Collections.Generic;
using System.Linq;
using Rascar.Toolbox.Extensions;

namespace Rascar.Toolbox.Utilities
{
    public static class EnumUtils
    {
        public static TEnum ParseEnum<TEnum>(string value) where TEnum : Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }

        public static IEnumerable<string> GetValueTable<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(value => value.ToString());
        }

        public static TEnum GetRandom<TEnum>() where TEnum : Enum
        {
            Array array = Enum.GetValues(typeof(TEnum));

            return (TEnum)array.GetValue(UnityEngine.Random.Range(0, array.Length));
        }

        /// <summary>
        /// Returns a random enum value between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>.
        /// </summary>
        public static TEnum GetRandom<TEnum>(TEnum minInclusive, TEnum maxInclusive) where TEnum : Enum
        {
            if (minInclusive.CompareTo(maxInclusive) > 0)
            {
                throw new ArgumentException("minInclusive must be less than or equal to maxInclusive");
            }

            Array array = Enum.GetValues(typeof(TEnum));
            Array.Sort(array);

            int minIndex = Array.IndexOf(array, minInclusive);
            int maxIndex = Array.IndexOf(array, maxInclusive);

            if (minIndex == -1 || maxIndex == -1)
            {
                throw new ArgumentException("minInclusive or maxInclusive are not valid enum values");
            }

            return (TEnum)array.GetValue(UnityEngine.Random.Range(minIndex, maxIndex + 1));
        }

        public static bool TryParse<TEnum>(string value, out TEnum parsedValue,
            bool insensitiveMatch = false, bool mustIgnoreSpacing = false) where TEnum : struct
        {
            foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
            {
                if (insensitiveMatch && value.EqualsInsensitive(enumValue.ToString(), mustIgnoreSpacing) || value == enumValue.ToString())
                {
                    parsedValue = enumValue;

                    return true;
                }
            }

            parsedValue = default;

            return false;
        }
    }
}
