using System;
using System.Collections.Generic;

namespace Rascar.Toolbox.Serialization
{
    [Serializable]
    public struct SerializablePair<TLeft, TRight> : IEquatable<SerializablePair<TLeft, TRight>>
    {
        public TLeft _leftValue;
        public TRight _rightValue;

        public SerializablePair(TLeft leftValue, TRight rightValue)
        {
            _leftValue = leftValue;
            _rightValue = rightValue;
        }

        public readonly void Deconstruct(out TLeft leftValue, out TRight rightValue)
        {
            leftValue = _leftValue;
            rightValue = _rightValue;
        }

        public override bool Equals(object obj)
        {
            return obj is SerializablePair<TLeft, TRight> pair && Equals(pair);
        }

        public readonly bool Equals(SerializablePair<TLeft, TRight> other)
        {
            // uses EqualityComparer<TLeft>.Default.Equals rather than Equals to avoid boxing (which would create GC)
            return EqualityComparer<TLeft>.Default.Equals(_leftValue, other._leftValue) && EqualityComparer<TRight>.Default.Equals(_rightValue, other._rightValue);
        }

        public readonly override int GetHashCode()
        {
            return 31 * _leftValue.GetHashCode() + _rightValue.GetHashCode();
        }

        public readonly override string ToString()
        {
            return $"{_leftValue}, {_rightValue}";
        }
    }
}
