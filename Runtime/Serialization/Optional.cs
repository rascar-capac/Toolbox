using UnityEngine;

namespace Rascar.Toolbox.Serialization
{
    [System.Serializable]
    public struct Optional<TValue>
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private TValue _value;

        public static implicit operator Optional<TValue>(TValue value) => new(enabled: true, value);

        public readonly bool Enabled => _enabled;
        public readonly TValue Value => _value;

        public Optional(bool enabled)
        {
            _enabled = enabled;
            _value = default;
        }

        public Optional(bool enabled, TValue value)
        {
            _enabled = enabled;
            _value = value;
        }

        public readonly bool HasValue(out TValue value)
        {
            value = _value;

            return _enabled;
        }

        public readonly TValue TryGetValueOrDefault(TValue defaultValue = default)
        {
            if (_enabled)
            {
                return _value;
            }

            return defaultValue;
        }
    }
}
