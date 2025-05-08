using System.Collections;
using System.Collections.Generic;

namespace Rascar.Toolbox.Collections
{
    /// <summary>
    /// Provides readonly access with garbage free enumerating and utility functions.
    /// </summary>
    public readonly struct ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        public TValue this[TKey key] => _dictionary[key];
        public int Count => _dictionary.Count;
        public Dictionary<TKey, TValue>.KeyCollection Keys => _dictionary.Keys;
        public Dictionary<TKey, TValue>.ValueCollection Values => _dictionary.Values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public static implicit operator ReadOnlyDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) => new(dictionary);

        public ReadOnlyDictionary(Dictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public bool ContainsValue(TValue value)
        {
            return _dictionary.ContainsValue(value);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
