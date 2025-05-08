using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rascar.Toolbox.Utilities
{
    public static class RandomUtils
    {
        /// <summary>
        /// Selects an item in a successive manner, each candidate item has the same chance of being selected.<br/>
        /// This function can be called multiple times in a row, it will update the necessary variables to work as expected.<br/>
        /// It can be used to select a random item that satisfies a predicate in any type of collection (e.g. <see cref="GetRandomItem"/>) or algorithm.
        /// </summary>
        /// <param name="randomValue">A value starting inside [0,1]<br />Giving the same starting value will produce the same result.</param>
        /// <param name="selectionIndex">The index of selection, this ensures that the chances to select each item are the same.</param>
        public static void SelectRandomItem<TItem>(ref float randomValue, ref int selectionIndex, ref bool hasFirstItem, ref TItem selectedItem, TItem candidateItem)
        {
            if (!hasFirstItem)
            {
                hasFirstItem = true;
                selectedItem = candidateItem;

                return;
            }

            randomValue = Mathf.Abs(randomValue) % 1f;
            float chanceOfKeepingItem = (selectionIndex + 1) / (float)(selectionIndex + 2);

            if (randomValue <= chanceOfKeepingItem)
            {
                randomValue /= chanceOfKeepingItem;
            }
            else
            {
                selectedItem = candidateItem;
                randomValue = (randomValue - chanceOfKeepingItem) / (1f - chanceOfKeepingItem);
            }

            selectionIndex++;
        }

        /// <summary>
        /// Selects an item in the list using a predicate, each item that matches the predicate has the same chance of being selected.<br/>
        /// Random value is calculated using Unity's random, to use different kind of random use the overload of this function.
        /// </summary>
        /// <param name="randomValue">A value inside [0,1]<br />Giving the same value will produce the same result.</param>
        public static bool TryGetRandomItem<TItem>(IReadOnlyList<TItem> readOnlyList, Predicate<TItem> predicate, out TItem item)
        {
            float randomValue = UnityEngine.Random.value;

            return TryGetRandomItem(readOnlyList, predicate, randomValue, out item);
        }

        /// <summary>
        /// Selects an item from a random value in the list using a predicate, each item that matches the predicate has the same chance of being selected.
        /// </summary>
        /// <param name="randomValue">A value inside [0,1]<br />Giving the same value will produce the same result.</param>
        public static bool TryGetRandomItem<TItem>(IReadOnlyList<TItem> readOnlyList, Predicate<TItem> predicate, float randomValue, out TItem item)
        {
            int selectionIndex = 0;
            bool hasFirstItem = false;
            item = default;

            for (int index = 0; index < readOnlyList.Count; index++)
            {
                TItem candidateItem = readOnlyList[index];

                if (predicate.Invoke(candidateItem))
                {
                    SelectRandomItem(ref randomValue, ref selectionIndex, ref hasFirstItem, ref item, candidateItem);
                }
            }

            return hasFirstItem;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, float, out T)"/>
        public static TItem GetRandomItem<TItem>(IReadOnlyList<TItem> readOnlyList, Predicate<TItem> predicate, float randomValue)
        {
            TryGetRandomItem(readOnlyList, predicate, randomValue, out TItem item);

            return item;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IReadOnlyList{T}, Predicate{T}, out T)"/>
        public static TItem GetRandomItem<TItem>(IReadOnlyList<TItem> readOnlyList, Predicate<TItem> predicate)
        {
            TryGetRandomItem(readOnlyList, predicate, out TItem item);

            return item;
        }

        /// <summary>
        /// Selects an item from a random value in the enumerable using a predicate, each item that matches the predicate has the same chance of being selected.
        /// </summary>
        /// <param name="randomValue">A value inside [0,1]<br />Giving the same value will produce the same result.</param>
        public static bool TryGetRandomItem<TItem>(IEnumerable<TItem> enumerable, Predicate<TItem> predicate, float randomValue, out TItem item)
        {
            int selectionIndex = 0;
            bool hasFirstItem = false;
            item = default;

            foreach (TItem candidateItem in enumerable)
            {
                if (predicate.Invoke(candidateItem))
                {
                    SelectRandomItem(ref randomValue, ref selectionIndex, ref hasFirstItem, ref item, candidateItem);
                }
            }

            return hasFirstItem;
        }

        /// <inheritdoc cref="TryGetRandomItem{T}(IEnumerable{T}, Predicate{T}, float, out T)"/>
        public static TItem GetRandomItem<TItem>(IEnumerable<TItem> enumerable, Predicate<TItem> predicate, float randomValue)
        {
            TryGetRandomItem(enumerable, predicate, randomValue, out TItem item);

            return item;
        }
    }
}
