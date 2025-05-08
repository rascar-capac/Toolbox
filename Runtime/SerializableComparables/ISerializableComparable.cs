namespace Rascar.Toolbox.SerializableComparables
{
    public interface ISerializableComparable<TSerializableValue, TValue>
    {
        public EComparisonOperator Operator { get; }
        public TSerializableValue Value { get; }

        public bool IsSatisfiedBy(TValue value);
    }
}
