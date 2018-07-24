using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task2
{
    public class Queue<T> : IEnumerable<T>
    {
        #region Fields
        private T[] _data;
        private int _head;
        private int _tail;
        private int _size;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize queue of length = capacity
        /// </summary>
        /// <param name="capacity"> Length of queue </param>
        public Queue(int capacity = 4)
        {
            if (capacity < 0)
            {
                throw new ArgumentException($"The {nameof(capacity)} can't be less than 0!");
            }

            _data = new T[capacity];

            _head = 0;
            _tail = 0;
            _size = 0;
        }

        /// <summary>
        /// Initialize queue and copy items 
        /// </summary>
        /// <param name="items"> Items for corying </param>
        public Queue(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentException($"The {nameof(items)} can't be null!");
            }

            _data = new T[items.Count()];
            _size = 0;

            foreach (T item in items)
            {
                Enqueue(item);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Count of elements in queue
        /// </summary>
        public int Count => _size;
        #endregion

        #region Public methods
        /// <summary>
        /// Add item in queue
        /// </summary>
        /// <param name="item"> Item for adding </param>
        public void Enqueue(T item)
        {
            if (_size == _data.Length)
            {
                Array.Resize(ref _data, _data.Length * 2);
            }

            _data[_tail] = item;
            _tail++;
            _size++;
        }

        /// <summary>
        /// Exclude first element of queue
        /// </summary>
        /// <returns> Head value </returns>
        public T Dequeue()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException();
            }

            T result = _data[_head];

            _data[_head] = default(T);

            _head++;
            _size--;

            return result;
        }

        /// <summary>
        /// Get head value
        /// </summary>
        /// <returns> Head value </returns>
        public T Peek()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException();
            }

            return _data[_head];
        }

        /// <summary>
        /// Clear queue
        /// </summary>
        public void Clear()
        {
            Array.Clear(_data, 0, _size);
            
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        /// <summary>
        /// Get iterator
        /// </summary>
        /// <returns> Iterator </returns>
        public IEnumerator<T> GetEnumerator() => new Iterator(this);

        /// <summary>
        /// Get iterator
        /// </summary>
        /// <returns> Iterator </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Get iterator
        /// </summary>
        /// <returns> Iterator </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region Iterator
        private struct Iterator : IEnumerator<T>
        {
            private readonly Queue<T> _collection;
            private int _currentIndex;

            public Iterator(Queue<T> collection)
            {
                this._currentIndex = -1;
                this._collection = collection;
            }

            public T Current
            {
                get
                {
                    if (_currentIndex == -1 || _currentIndex == _collection.Count)
                    {
                        throw new InvalidOperationException();
                    }

                    return _collection._data[_currentIndex];
                }
            }

            object IEnumerator.Current => Current;

            public void Reset()
            {
                _currentIndex = -1;
            }

            public bool MoveNext()
            {
                return ++_currentIndex < _collection.Count;
            }

            void IDisposable.Dispose()
            {
            }
        }
        #endregion
    }
}
