using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Rascar.Toolbox.Collections
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        public int Count => _dictionary.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                bool hadOldValue = _dictionary.TryGetValue(key, out TValue oldValue);
                _dictionary[key] = value;

                if (hadOldValue)
                {
                    if (!EqualityComparer<TValue>.Default.Equals(value, oldValue))
                    {
                        OnKeyRemoved.Invoke(new OnKeyRemovedArgs(this, key, oldValue));
                        OnKeyAdded.Invoke(new OnKeyAddedArgs(this, key, value));
                        OnKeyChanged.Invoke(new OnKeyChangedArgs(this, key, oldValue, value));
                    }
                }
                else
                {
                    OnKeyAdded.Invoke(new OnKeyAddedArgs(this, key, value));
                }
            }
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys => _dictionary.Keys;
        public Dictionary<TKey, TValue>.ValueCollection Values => _dictionary.Values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        public UnityEvent<OnKeyAddedArgs> OnKeyAdded { get; } = new();
        public UnityEvent<OnKeyRemovedArgs> OnKeyRemoved { get; } = new();
        public UnityEvent<OnKeyChangedArgs> OnKeyChanged { get; } = new();
        public UnityEvent OnCleared { get; } = new();

        public ObservableDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public ObservableDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            OnKeyAdded.Invoke(new OnKeyAddedArgs(this, key, value));
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if (_dictionary.TryAdd(key, value))
            {
                OnKeyAdded.Invoke(new OnKeyAddedArgs(this, key, value));

                return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            foreach ((TKey key, TValue value) in _dictionary)
            {
                OnKeyRemoved.Invoke(new OnKeyRemovedArgs(this, key, value));
            }

            _dictionary.Clear();
            OnCleared.Invoke();
        }

        public int EnsureCapacity(int capacity)
        {
            return _dictionary.EnsureCapacity(capacity);
        }

        public bool Remove(TKey key)
        {
            return Remove(key, out _);
        }

        public bool Remove(TKey key, out TValue value)
        {
            if (_dictionary.Remove(key, out value))
            {
                OnKeyRemoved.Invoke(new OnKeyRemovedArgs(this, key, value));

                return true;
            }

            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _dictionary.ContainsValue(value);
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ReadOnlyDictionary<TKey, TValue> AsReadOnly()
        {
            return _dictionary;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
        {
            ICollection<KeyValuePair<TKey, TValue>> dictionaryCollection = _dictionary;

            return dictionaryCollection.Contains(keyValuePair);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ICollection<KeyValuePair<TKey, TValue>> dictionaryCollection = _dictionary;
            dictionaryCollection.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (TryGetValue(keyValuePair.Key, out TValue value) && EqualityComparer<TValue>.Default.Equals(keyValuePair.Value, value))
            {
                _dictionary.Remove(keyValuePair.Key);
                OnKeyRemoved.Invoke(new OnKeyRemovedArgs(this, keyValuePair.Key, value));

                return true;
            }

            return false;
        }

        public readonly struct OnKeyAddedArgs
        {
            public readonly ObservableDictionary<TKey, TValue> Sender { get; }
            public readonly TKey Key { get; }
            public readonly TValue Value { get; }

            public OnKeyAddedArgs(ObservableDictionary<TKey, TValue> sender, TKey key, TValue value)
            {
                Sender = sender;
                Key = key;
                Value = value;
            }
        }

        public readonly struct OnKeyRemovedArgs
        {
            public readonly ObservableDictionary<TKey, TValue> Sender { get; }
            public readonly TKey Key { get; }
            public readonly TValue Value { get; }

            public OnKeyRemovedArgs(ObservableDictionary<TKey, TValue> sender, TKey key, TValue value)
            {
                Sender = sender;
                Key = key;
                Value = value;
            }
        }

        public readonly struct OnKeyChangedArgs
        {
            public readonly ObservableDictionary<TKey, TValue> Sender { get; }
            public readonly TKey Key { get; }
            public readonly TValue OldValue { get; }
            public readonly TValue NewValue { get; }

            public OnKeyChangedArgs(ObservableDictionary<TKey, TValue> sender, TKey key, TValue oldValue, TValue newValue)
            {
                Sender = sender;
                Key = key;
                OldValue = oldValue;
                NewValue = newValue;
            }
        }
    }
}
