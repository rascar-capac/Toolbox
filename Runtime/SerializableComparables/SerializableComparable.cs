using System;
using UnityEngine;

namespace Rascar.Toolbox.SerializableComparables
{
    /// <summary>
    /// Comparable wrapper with a comparison operator that can be serialized.
    /// Use it to compare a value of type <typeparamref name="TValue"/> with the value and the comparison test (equal, greater, etc.) selected in the inspector.
    /// </summary>
    /// <typeparam name="TValue">Type of the values to compare, must implement <see cref="IComparable"/></typeparam>
    [Serializable]
    public struct SerializableComparable<TValue> : ISerializableComparable<TValue, TValue> where TValue : IComparable<TValue>
    {
        [SerializeField] private EComparisonOperator _operator;
        [SerializeField] private TValue _value;

        public readonly EComparisonOperator Operator => _operator;
        public readonly TValue Value => _value;

        public static implicit operator SerializableComparable<TValue>(TValue value) => new(EComparisonOperator.Equal, value);
        public static implicit operator SerializableComparable<TValue>((EComparisonOperator @operator, TValue value) tuple) => new(tuple.@operator, tuple.value);

        public SerializableComparable(EComparisonOperator @operator, TValue value)
        {
            _operator = @operator;
            _value = value;
        }

        public readonly bool IsSatisfiedBy(TValue value)
        {
            return SerializableComparableUtilities.Compare<TValue>(_operator, value, _value);
        }
    }

    /// <summary>
    /// <inheritdoc cref="SerializableComparable{T}"/>
    /// This version can use a serialized value with a different type, as long as it implements <see cref="IComparable"/> with <see cref="TValue"/>.
    /// </summary>
    /// <typeparam name="TSerializableValue">Type of the exposed value to compare to, must implement <see cref="IComparable"/> with <see cref="TValue"/>.</typeparam>
    /// <typeparam name="TValue">Type of the value that will be compared.</typeparam>
    [Serializable]
    public struct SerializableComparable<TSerializableValue, TValue> : ISerializableComparable<TSerializableValue, TValue>
        where TSerializableValue : IComparable<TValue>
    {
        [SerializeField] private EComparisonOperator _operator;
        [SerializeField] private TSerializableValue _value;

        public readonly EComparisonOperator Operator => _operator;
        public readonly TSerializableValue Value => _value;

        public static implicit operator SerializableComparable<TSerializableValue, TValue>(TSerializableValue value) => new(EComparisonOperator.Equal, value);
        public static implicit operator SerializableComparable<TSerializableValue, TValue>((EComparisonOperator @operator, TSerializableValue value) tuple) => new(tuple.@operator, tuple.value);

        public SerializableComparable(EComparisonOperator @operator, TSerializableValue value)
        {
            _operator = @operator;
            _value = value;
        }

        public readonly bool IsSatisfiedBy(TValue value)
        {
            return SerializableComparableUtilities.Compare(_operator, value, _value);
        }
    }
}
