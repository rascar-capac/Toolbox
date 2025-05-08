using System.Collections;
using System.Collections.Generic;

namespace Rascar.Toolbox.Collections
{
    /// <summary>
    /// Provides readonly access with garbage free enumerating and utility functions.
    /// </summary>
    public readonly struct ReadOnlyQueue<T> : IReadOnlyCollection<T>
    {
        private readonly Queue<T> _queue;

        public int Count => _queue.Count;

        public static implicit operator ReadOnlyQueue<T>(Queue<T> queue) => new(queue);

        public ReadOnlyQueue(Queue<T> queue)
        {
            _queue = queue;
        }

        public Queue<T>.Enumerator GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        public T Peek()
        {
            return _queue.Peek();
        }

        public bool TryPeek(out T result)
        {
            return _queue.TryPeek(out result);
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
