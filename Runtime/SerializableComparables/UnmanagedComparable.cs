using UnityEngine;

namespace Rascar.Toolbox.SerializableComparables
{
    /// <inheritdoc cref="SerializableComparable{T}"/>
    [System.Serializable]
    public struct UnmanagedComparable<TValue, TUnmanaged> : ISerializableComparable<TValue, TValue>
        where TValue : unmanaged
        where TUnmanaged : unmanaged, System.IComparable<TUnmanaged>
    {
        [SerializeField] private EComparisonOperator _operator;
        [SerializeField] private TValue _value;

        public readonly EComparisonOperator Operator => _operator;
        public readonly TValue Value => _value;

        public static implicit operator UnmanagedComparable<TValue, TUnmanaged>(TValue value) => new(EComparisonOperator.Equal, value);
        public static implicit operator UnmanagedComparable<TValue, TUnmanaged>((EComparisonOperator @operator, TValue value) tuple) => new(tuple.@operator, tuple.value);

        public UnmanagedComparable(EComparisonOperator @operator, TValue value)
        {
            _operator = @operator;
            _value = value;
        }

        public readonly bool IsSatisfiedBy(TValue value)
        {
            return SerializableComparableUtilities.CompareUnmanaged<TValue, TUnmanaged>(_operator, value, _value);
        }
    }
}
