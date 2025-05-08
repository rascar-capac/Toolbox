using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rascar.Toolbox.Collections
{
    [Serializable]
    public class ObservableList<T> : IList<T>, IReadOnlyList<T>
    {
        [SerializeField] private List<T> _items;

        public int Count => _items.Count;
        public int Capacity { get => _items.Capacity; set => _items.Capacity = value; }

        bool ICollection<T>.IsReadOnly => false;

        public T this[int index]
        {
            get => _items[index];
            set
            {
                T oldItem = _items[index];
                _items[index] = value;

                OnItemRemoved.Invoke(oldItem);
                OnItemAdded.Invoke(value, index);
                OnItemChanged.Invoke(oldItem, value);
            }
        }

        public UnityEvent<T, int> OnItemAdded { get; private set; } = new();
        public UnityEvent<T> OnItemRemoved { get; private set; } = new();
        public UnityEvent<T, T> OnItemChanged { get; private set; } = new();
        public UnityEvent OnCleared { get; private set; } = new();

        public ObservableList()
        {
            _items = new List<T>();
        }

        public ObservableList(int capacity)
        {
            _items = new List<T>(capacity);
        }

        public void Add(T item)
        {
            _items.Add(item);

            OnItemAdded.Invoke(item, _items.Count - 1);
        }

        public bool AddUnique(T item)
        {
            if (!_items.Contains(item))
            {
                Add(item);

                return true;
            }

            return false;
        }

        public void AddRange(IReadOnlyList<T> items)
        {
            int newCount = _items.Count + items.Count;

            if (_items.Capacity < newCount)
            {
                _items.Capacity = newCount;
            }

            for (int index = 0; index < items.Count; index++)
            {
                T item = items[index];
                _items.Add(item);

                OnItemAdded.Invoke(item, _items.Count - 1);
            }
        }

        public void Clear()
        {
            for (int elementIndex = _items.Count - 1; elementIndex >= 0; elementIndex--)
            {
                // invokes OnItemRemoved
                RemoveAt(elementIndex);
            }

            OnCleared.Invoke();
        }

        public void Insert(int index, T item)
        {
            _items.Insert(index, item);

            OnItemAdded.Invoke(item, index);
        }

        public bool Remove(T item)
        {
            if (_items.Remove(item))
            {
                OnItemRemoved.Invoke(item);

                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            T item = _items[index];

            _items.RemoveAt(index);

            OnItemRemoved.Invoke(item);
        }

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Sort()
        {
            _items.Sort();
        }

        public void Sort(Comparison<T> comparison)
        {
            _items.Sort(comparison);
        }

        public void Sort(IComparer<T> comparer)
        {
            _items.Sort(comparer);
        }

        public T[] ToArray()
        {
            return _items.ToArray();
        }

        public bool Exists(Predicate<T> predicate)
        {
            return _items.Exists(predicate);
        }

        public T Find(Predicate<T> match)
        {
            return _items.Find(match);
        }

        public T FindLast(Predicate<T> match)
        {
            return _items.FindLast(match);
        }

        public List<T> FindAll(Predicate<T> match)
        {
            return _items.FindAll(match);
        }

        public int FindIndex(Predicate<T> match)
        {
            return _items.FindIndex(match);
        }

        public ReadOnlyList<T> AsReadOnly()
        {
            return _items;
        }
    }

}
