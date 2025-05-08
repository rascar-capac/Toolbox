using System;
using System.Collections;
using System.Collections.Generic;

namespace Rascar.Toolbox.Collections
{
    /// <summary>
    /// Provides readonly access with garbage free enumerating and utility functions.
    /// </summary>
    public readonly struct ReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _list;

        public T this[int index] => _list[index];
        public int Count => _list.Count;

        public static implicit operator ReadOnlyList<T>(List<T> list) => new(list);

        public ReadOnlyList(List<T> list)
        {
            _list = list;
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public int IndexOf(T item, int index)
        {
            return _list.IndexOf(item, index);
        }

        public int IndexOf(T item, int index, int count)
        {
            return _list.IndexOf(item, index, count);
        }

        public bool Exists(Predicate<T> match)
        {
            return _list.Exists(match);
        }

        public T Find(Predicate<T> match)
        {
            return _list.Find(match);
        }

        public T FindLast(Predicate<T> match)
        {
            return _list.FindLast(match);
        }

        public int FindIndex(Predicate<T> match)
        {
            return _list.FindIndex(match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return _list.FindIndex(startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            return _list.FindIndex(startIndex, count, match);
        }

        public int FindLastIndex(Predicate<T> match)
        {
            return _list.FindLastIndex(match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return _list.FindLastIndex(startIndex, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            return _list.FindLastIndex(startIndex, count, match);
        }

        public void ForEach(Action<T> action)
        {
            _list.ForEach(action);
        }

        public bool TrueForAll(Predicate<T> match)
        {
            return _list.TrueForAll(match);
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return _list.BinarySearch(index, count, item, comparer);
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
