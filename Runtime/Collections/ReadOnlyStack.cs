using System;
using System.Collections;
using System.Collections.Generic;

namespace Rascar.Toolbox.Collections
{
    /// <summary>
    /// Provides readonly access with garbage free enumerating and utility functions.
    /// </summary>
    public readonly struct ReadOnlyStack<T> : IReadOnlyCollection<T>
    {
        private readonly Stack<T> _stack;

        public int Count => _stack.Count;

        public static implicit operator ReadOnlyStack<T>(Stack<T> stack) => new(stack);

        public ReadOnlyStack(Stack<T> stack)
        {
            _stack = stack;
        }

        public Stack<T>.Enumerator GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        public bool Contains(T item)
        {
            return _stack.Contains(item);
        }

        public T Peek()
        {
            return _stack.Peek();
        }

        public T Peek(int indexFromTop)
        {
            if (indexFromTop < 0 || indexFromTop >= _stack.Count)
            {
                throw new IndexOutOfRangeException();
            }

            T resultItem = default;

            foreach (T item in _stack)
            {
                resultItem = item;

                if (--indexFromTop < 0)
                {
                    break;
                }
            }

            return resultItem;
        }

        public bool TryPeek(out T result)
        {
            return _stack.TryPeek(out result);
        }

        public bool TryPeek(int indexFromTop, out T resultItem)
        {
            resultItem = default;

            if (indexFromTop < 0 || indexFromTop >= _stack.Count)
            {
                return false;
            }

            foreach (T item in _stack)
            {
                resultItem = item;

                if (--indexFromTop < 0)
                {
                    break;
                }
            }

            return true;
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
