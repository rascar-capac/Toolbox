using System;
using System.Collections;
using System.Collections.Generic;

namespace Rascar.Toolbox.Collections
{
    public class PriorityList<TKey, TValue> : IReadOnlyList<TValue>
    {
        private readonly SortedDictionary<TKey, List<TValue>> _sortedKeysToLists = new();

        public int Count { get; private set; }
        public TValue this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }

                int elementIndex = 0;

                foreach (List<TValue> list in _sortedKeysToLists.Values)
                {
                    if (index < elementIndex + list.Count)
                    {
                        return list[index - elementIndex];
                    }

                    elementIndex += list.Count;
                }

                throw new IndexOutOfRangeException();
            }
        }

        public void Prepend(TValue element, TKey priority)
        {
            List<TValue> list = GetListInSortedKeysToLists(priority);

            list.Insert(0, element);

            ++Count;
        }

        public void Append(TValue element, TKey priority)
        {
            List<TValue> list = GetListInSortedKeysToLists(priority);

            list.Add(element);

            ++Count;
        }

        private List<TValue> GetListInSortedKeysToLists(TKey key)
        {
            if (!_sortedKeysToLists.TryGetValue(key, out List<TValue> list))
            {
                list = new List<TValue>();
                _sortedKeysToLists.Add(key, list);
            }

            return list;
        }

        public bool Remove(TValue elementToRemove, TKey hint = default)
        {
            if (_sortedKeysToLists.TryGetValue(hint, out List<TValue> list) && list.Remove(elementToRemove))
            {
                --Count;

                return true;
            }

            foreach ((TKey key, List<TValue> elementList) in _sortedKeysToLists)
            {
                if (!hint.Equals(key) && elementList.Remove(elementToRemove))
                {
                    --Count;

                    return true;
                }
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            int elementIndex = 0;

            foreach ((TKey key, List<TValue> list) in _sortedKeysToLists)
            {
                if (index < elementIndex + list.Count)
                {
                    list.RemoveAt(index - elementIndex);
                    --Count;

                    return;
                }

                elementIndex += list.Count;
            }

            throw new IndexOutOfRangeException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (List<TValue> elementList in _sortedKeysToLists.Values)
            {
                foreach (TValue element in elementList)
                {
                    yield return element;
                }
            }
        }

        public bool Contains(TValue element, TKey hint = default)
        {
            if (_sortedKeysToLists.TryGetValue(hint, out List<TValue> list) && list.Contains(element))
            {
                return true;
            }

            foreach ((TKey key, List<TValue> elementList) in _sortedKeysToLists)
            {
                if (!hint.Equals(key) && elementList.Contains(element))
                {
                    return true;
                }
            }

            return false;
        }

        public int IndexOf(TValue element)
        {
            int indexOffset = 0;

            foreach (List<TValue> elementList in _sortedKeysToLists.Values)
            {
                if (elementList.IndexOf(element) != -1)
                {
                    return indexOffset + elementList.IndexOf(element);
                }

                indexOffset += elementList.Count;
            }

            return -1;
        }

        public void Clear()
        {
            _sortedKeysToLists.Clear();
        }
    }
}
