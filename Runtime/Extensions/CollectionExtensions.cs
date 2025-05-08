using System;
using System.Collections.Generic;
using System.Linq;
using Rascar.Toolbox.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rascar.Toolbox.Extensions
{
    public static class CollectionExtensions
    {
        public static T GetItemByIndexClamped<T>(this T[] array, int index)
        {
            return array[Mathf.Clamp(index, 0, array.Length - 1)];
        }

        public static T GetRandomItem<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static T GetRandomItem<T>(this T[] array, out int index)
        {
            index = Random.Range(0, array.Length);

            return array[index];
        }

        public static T GetItemByIndexClamped<T>(this List<T> list, int index)
        {
            return list[Mathf.Clamp(index, 0, list.Count - 1)];
        }

        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static T GetRandomItem<T>(this List<T> list, out int index)
        {
            index = Random.Range(0, list.Count);

            return list[index];
        }

        public static T LastValue<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return default;
            }

            return list[list.Count - 1];
        }

        public static void Shuffle<T>(this List<T> list)
        {
            for (int index = 0; index < list.Count - 1; index++)
            {
                int indexToSwap = Random.Range(index, list.Count);
                (list[indexToSwap], list[index]) = (list[index], list[indexToSwap]);
            }
        }

        public static void MoveDown<T>(this List<T> list, int index)
        {
            if (index > 0 && list.Count > 1)
            {
                T firstItem = list[index - 1];
                list.RemoveAt(index - 1);
                list.Insert(index, firstItem);
            }
        }

        public static List<T> PickRandomly<T>(this List<T> originalList, int itemCountToPick, out List<T> remainingItems)
        {
            remainingItems = new(originalList);
            List<T> pickedItems = new();

            if (originalList == null || originalList.Count == 0)
            {
                return pickedItems;
            }

            itemCountToPick = Mathf.Min(itemCountToPick, originalList.Count);

            remainingItems.Shuffle();

            int remainingCount = remainingItems.Count - itemCountToPick;

            for (int listIndex = remainingItems.Count - 1; listIndex >= remainingCount; listIndex--)
            {
                pickedItems.Add(remainingItems[listIndex]);
                remainingItems.RemoveAt(listIndex);
            }

            return pickedItems;
        }

        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            if (index1 < 0 || index1 >= list.Count
                || index2 < 0 || index2 >= list.Count)
            {
                throw new IndexOutOfRangeException();
            }

            (list[index2], list[index1]) = (list[index1], list[index2]);
        }

        public static void RemoveFirst<TItem>(this IList<TItem> list, Predicate<TItem> predicate)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (predicate.Invoke(list[index]))
                {
                    list.RemoveAt(index);

                    return;
                }
            }
        }

        public static TOut[] ToArray<TIn, TOut>(this IList<TIn> list, Func<TIn, TOut> converter)
        {
            TOut[] array = new TOut[list.Count];

            for (int index = 0; index < list.Count; index++)
            {
                array[index] = converter(list[index]);
            }

            return array;
        }

        public static bool AddUnique<TItem>(this IList<TItem> list, TItem item)
        {
            if (list.Contains(item))
            {
                return false;
            }

            list.Add(item);

            return true;
        }

        public static void AddNotNull<TItem>(this IList<TItem> list, TItem item) where TItem : class
        {
            if (item != null)
            {
                list.Add(item);
            }
        }

        public static void RemoveItemRange<TItem>(this IList<TItem> list, IReadOnlyList<TItem> readOnlyList)
        {
            if (readOnlyList.Count == 0)
            {
                return;
            }

            for (int itemIndex = 0; itemIndex < readOnlyList.Count; itemIndex++)
            {
                list.Remove(readOnlyList[itemIndex]);
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int targetIndex = list.Count;

            while (targetIndex > 1)
            {
                int swappingIndex = Random.Range(0, targetIndex);
                targetIndex--;
                (list[targetIndex], list[swappingIndex]) = (list[swappingIndex], list[targetIndex]);
            }
        }

        public static T PickRandomElement<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Avoids the instantiation of a new collection.
        /// </summary>
        /// <param name="targetDictionary">If null, will be instantiated.</param>
        public static void CopyTo<TKey, TValue>(this Dictionary<TKey, TValue> sourceDictionary, Dictionary<TKey, TValue> targetDictionary)
        {
            if (targetDictionary == null)
            {
                targetDictionary = new Dictionary<TKey, TValue>(sourceDictionary.Count);
            }
            else
            {
                targetDictionary.Clear();
            }

            foreach ((TKey key, TValue value) in sourceDictionary)
            {
                targetDictionary[key] = value;
            }
        }

        /// <inheritdoc cref="CopyTo"/>
        public static void CopyTo<TItem>(this HashSet<TItem> hashSet, IList<TItem> list)
        {
            list.Clear();

            foreach (TItem item in hashSet)
            {
                list.Add(item);
            }
        }

        /// <inheritdoc cref="CopyTo"/>
        public static void CopyTo<TItem>(this IReadOnlyList<TItem> readOnlyList, IList<TItem> list)
        {
            list.Clear();

            for (int index = 0; index < readOnlyList.Count; index++)
            {
                TItem item = readOnlyList[index];

                list.Add(item);
            }
        }

        public static TValue TryGetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }

            return defaultValue;
        }

        public static TItem Max<TItem>(this IReadOnlyList<TItem> readOnlyList, Comparison<TItem> comparison)
        {
            if (readOnlyList.Count == 0)
            {
                return default;
            }

            TItem bestItem = readOnlyList[0];

            for (int index = 1; index < readOnlyList.Count; index++)
            {
                TItem item = readOnlyList[index];

                if (comparison.Invoke(item, bestItem) > 0)
                {
                    bestItem = item;
                }
            }

            return bestItem;
        }

        public static TItem GetRandomItem<TItem>(this IReadOnlyList<TItem> readOnlyList)
        {
            if (readOnlyList.Count == 0)
            {
                return default;
            }

            return readOnlyList[Random.Range(0, readOnlyList.Count)];
        }

        /// <inheritdoc cref="RandomUtils.TryGetRandomItem{TItem}(IReadOnlyList{TItem}, Predicate{TItem}, float, out TItem)"/>
        public static bool TryGetRandomItem<TItem>(this IReadOnlyList<TItem> readOnlyList, Predicate<TItem> predicate, float randomValue, out TItem item)
        {
            return RandomUtils.TryGetRandomItem(readOnlyList, predicate, randomValue, out item);
        }

        /// <inheritdoc cref="RandomUtils.GetRandomItem{TItem}(IReadOnlyList{TItem}, Predicate{TItem}, float)"/>
        public static TItem GetRandomItem<TItem>(this IReadOnlyList<TItem> readOnlyList, Predicate<TItem> predicate, float randomValue)
        {
            return RandomUtils.GetRandomItem(readOnlyList, predicate, randomValue);
        }

        public static bool TryGetItemAt<TItem>(this IReadOnlyList<TItem> readOnlyList, int index, out TItem item)
        {
            if (index < 0 || index >= readOnlyList.Count)
            {
                item = default;

                return false;
            }

            item = readOnlyList[index];

            return true;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any(item => true);
        }

        /// <summary>
        /// Checks if two enumerables have the same elements, whatever their order.
        /// </summary>
        public static bool CompareEnumerableAnyOrder<T>(this IEnumerable<T> firstEnumerable, IEnumerable<T> otherEnumerable)
        {
            if (firstEnumerable.Count() != otherEnumerable.Count())
            {
                return false;
            }

            // null value is never inserted in the map, they are calculated outside of it
            Dictionary<T, int> counts = new(firstEnumerable.Count());
            int nullCount = 0;

            foreach (T element in firstEnumerable)
            {
                if (element == null)
                {
                    nullCount++;

                    continue;
                }

                if (counts.TryGetValue(element, out int count))
                {
                    counts[element] = count + 1;
                }
                else
                {
                    counts[element] = 1;
                }
            }

            foreach (T element in otherEnumerable)
            {
                if (element == null)
                {
                    nullCount++;

                    continue;
                }

                if (counts.TryGetValue(element, out int count))
                {
                    counts[element] = count - 1;
                }
                else
                {
                    return false;
                }
            }

            return nullCount == 0 && counts.Values.All(count => count == 0);
        }
    }
}