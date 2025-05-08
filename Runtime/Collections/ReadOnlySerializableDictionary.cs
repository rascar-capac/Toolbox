using System.Collections;
using System.Collections.Generic;

namespace Rascar.Toolbox.Collections
{
    /// <summary>
    /// Provides readonly access with garbage free enumerating and utility functions.
    /// </summary>
    public readonly struct ReadOnlySerializableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly SerializableDictionary<TKey, TValue> _dictionary;

        public TValue this[TKey key] => _dictionary[key];
        public int Count => _dictionary.Count;
        public SerializableDictionary<TKey, TValue>.KeyCollection Keys => _dictionary.Keys;
        public SerializableDictionary<TKey, TValue>.ValueCollection Values => _dictionary.Values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public static implicit operator ReadOnlySerializableDictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> dictionary) => new(dictionary);

        public ReadOnlySerializableDictionary(SerializableDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public SerializableDictionary<TKey, TValue>.Enumerator GetEnumerator()
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
