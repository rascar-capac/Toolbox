using System;
using Rascar.Toolbox.Serialization;
using Rascar.Toolbox.Utilities;

namespace Rascar.Toolbox.SerializableComparables
{
    public static class SerializableComparableUtilities
    {
        public static bool Compare<TValue>(EComparisonOperator @operator, TValue firstValue, TValue secondValue)
            where TValue : IComparable<TValue>
        {
            int comparison = firstValue.CompareTo(secondValue);

            return Compare(@operator, comparison);
        }

        public static bool Compare<TValue, TCompare>(EComparisonOperator @operator, TCompare firstValue, TValue secondValue)
            where TValue : IComparable<TCompare>
        {
            int comparison = -secondValue.CompareTo(firstValue);

            return Compare(@operator, comparison);
        }

        public static bool CompareUnmanaged<TValue, TUnmanaged>(EComparisonOperator @operator, TValue firstValue, TValue secondValue)
            where TValue : unmanaged
            where TUnmanaged : unmanaged, IComparable<TUnmanaged>
        {
            TUnmanaged firstUnmanagedValue = UnsafeUtils.AsUnmanaged<TValue, TUnmanaged>(firstValue);
            TUnmanaged secondUnmanagedValue = UnsafeUtils.AsUnmanaged<TValue, TUnmanaged>(secondValue);
            int comparison = firstUnmanagedValue.CompareTo(secondUnmanagedValue);

            return Compare(@operator, comparison);
        }

        public static bool Compare(EComparisonOperator @operator, int comparisonResult)
        {
            return @operator switch
            {
                EComparisonOperator.NotEqual => comparisonResult != 0,
                EComparisonOperator.Equal => comparisonResult == 0,
                EComparisonOperator.Lower => comparisonResult < 0,
                EComparisonOperator.LowerOrEqual => comparisonResult <= 0,
                EComparisonOperator.HigherOrEqual => comparisonResult >= 0,
                EComparisonOperator.Higher => comparisonResult > 0,
                _ => throw new NotImplementedException(),
            };
        }

        public static bool CompareOptional<TComparable, TValue>(Optional<TComparable> optionalComparable, TValue value)
            where TComparable : ISerializableComparable<TValue, TValue>
        {
            return !optionalComparable.HasValue(out TComparable comparable) || comparable.IsSatisfiedBy(value);
        }

        public static bool CompareOptional<TSerializableValue, TValue>(Optional<SerializableComparable<TSerializableValue, TValue>> optionalComparable, TValue value)
            where TSerializableValue : IComparable<TValue>
        {
            return !optionalComparable.HasValue(out SerializableComparable<TSerializableValue, TValue> comparable) || comparable.IsSatisfiedBy(value);
        }
    }
}
