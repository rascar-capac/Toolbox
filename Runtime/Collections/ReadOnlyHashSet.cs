using System.Collections;
using System.Collections.Generic;

namespace Rascar.Toolbox.Collections
{
    /// <summary>
    /// Provides readonly access with garbage free enumerating and utility functions.
    /// </summary>
    public readonly struct ReadOnlyHashSet<T> : IReadOnlyCollection<T>
    {
        private readonly HashSet<T> _hashSet;

        public int Count => _hashSet.Count;

        public static implicit operator ReadOnlyHashSet<T>(HashSet<T> hashSet) => new(hashSet);

        public ReadOnlyHashSet(HashSet<T> hashSet)
        {
            _hashSet = hashSet;
        }

        public HashSet<T>.Enumerator GetEnumerator()
        {
            return _hashSet.GetEnumerator();
        }

        public bool Contains(T item)
        {
            return _hashSet.Contains(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
