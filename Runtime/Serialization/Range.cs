using UnityEngine;

namespace Rascar.Toolbox.Serialization
{
    [System.Serializable]
    public abstract class Range<TValue>
    {
        [SerializeField] protected TValue _minValue;
        [SerializeField] protected TValue _maxValue;

        public TValue MinValue => _minValue;
        public TValue MaxValue => _maxValue;

        public abstract TValue Amplitude { get; }
        public abstract TValue Center { get; }

        public abstract TValue GetRandomValue();
        public abstract bool Contains(TValue valueToCheck);
        public abstract float GetRatio(TValue valueToCheck);
    }

    [System.Serializable]
    public class IntegerRange : Range<int>
    {
        public override int Amplitude => _maxValue - _minValue;
        public override int Center => (_maxValue + _minValue) / 2;

        public IntegerRange(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public override int GetRandomValue()
        {
            return Random.Range(MinValue, MaxValue);
        }

        public override bool Contains(int valueToCheck)
        {
            return MinValue <= valueToCheck && valueToCheck < MaxValue;
        }

        public override float GetRatio(int valueToCheck)
        {
            return Mathf.InverseLerp(_minValue, _maxValue, valueToCheck);
        }
    }

    [System.Serializable]
    public class FloatRange : Range<float>
    {
        public override float Amplitude => _maxValue - _minValue;
        public override float Center => (_maxValue + _minValue) * 0.5f;

        public FloatRange(float minValue, float maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public override float GetRandomValue()
        {
            return Random.Range(MinValue, MaxValue);
        }

        public override bool Contains(float valueToCheck)
        {
            return MinValue <= valueToCheck && valueToCheck <= MaxValue;
        }

        public override float GetRatio(float valueToCheck)
        {
            return Mathf.InverseLerp(_minValue, _maxValue, valueToCheck);
        }
    }

    [System.Serializable]
    public class Vector3Range : Range<Vector3>
    {
        public override Vector3 Amplitude => _maxValue - _minValue;
        public override Vector3 Center => (_maxValue + _minValue) * 0.5f;

        public Vector3Range(Vector3 minValue, Vector3 maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public override Vector3 GetRandomValue()
        {
            return new Vector3
            (
                Random.Range(MinValue.x, MaxValue.x),
                Random.Range(MinValue.y, MaxValue.y),
                Random.Range(MinValue.z, MaxValue.z)
            );
        }

        public override bool Contains(Vector3 valueToCheck)
        {
            return MinValue.x <= valueToCheck.x
                && valueToCheck.x <= MaxValue.x
                && MinValue.y <= valueToCheck.y
                && valueToCheck.y <= MaxValue.y
                && MinValue.z <= valueToCheck.z
                && valueToCheck.z <= MaxValue.z;
        }

        public override float GetRatio(Vector3 valueToCheck)
        {
            if (Amplitude != Vector3.zero)
            {
                return Mathf.Clamp01((valueToCheck - _minValue).magnitude / Amplitude.magnitude);
            }

            return 0f;
        }
    }
}
