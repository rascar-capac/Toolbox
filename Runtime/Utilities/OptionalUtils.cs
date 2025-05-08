using System;
using System.Collections.Generic;
using Rascar.Toolbox.Serialization;

namespace Rascar.Toolbox.Utilities
{
    public class OptionalUtils
    {
        public static bool OptionalEquals<TValue>(Optional<TValue> optionalValue, TValue compareValue) where TValue : IEquatable<TValue>
        {
            return !optionalValue.HasValue(out TValue value) || value.Equals(compareValue);
        }

        public static bool OptionalEquals<TValue>(Optional<TValue> optionalValue, TValue compareValue, IEqualityComparer<TValue> comparer)
        {
            return !optionalValue.HasValue(out TValue value) || comparer.Equals(value, compareValue);
        }
    }
}
