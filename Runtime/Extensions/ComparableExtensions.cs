using System;

namespace Rascar.Toolbox.Extensions
{
    public static class ComparableExtensions
    {
        public static TValue Clamp<TValue>(this TValue value, TValue min, TValue max) where TValue : IComparable
        {
            TValue clampedValue;

            if (value.CompareTo(min) < 0)
            {
                clampedValue = min;
            }
            else if (value.CompareTo(max) > 0)
            {
                clampedValue = max;
            }
            else
            {
                clampedValue = value;
            }

            return clampedValue;
        }
    }
}
