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
        private int _version;
        #endregion

        #region Constructors
        /// <summary>
        /// Initialize queue of length = capacity
        /// </summary>
        /// <param name="capacity"> Length of queue </param>
        public Queue(int capacity = 8)
        {
            if (capacity < 0)
            {
                throw new ArgumentException($"The {nameof(capacity)} can't be less than 0!");
            }

            _data = new T[capacity];

            _head = 0;
            _tail = 0;
            _size = 0;
            _version = 0;
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
            _version = 0;

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
                if (_tail <= _head)
                {
                    ReorderElements();
                }

                Array.Resize(ref _data, _data.Length + 4);
            }
            else if (_tail == _data.Length)
            {
                _tail = 0;
            }

            _data[_tail] = item;
            _tail++;
            _size++;
            _version++;
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
            _version++;

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
            Array.Clear(_data, 0, _data.Length);
            
            _head = 0;
            _tail = 0;
            _size = 0;
            _version = 0;
        }

        /// <summary>
        /// Get iterator
        /// </summary>
        /// <returns> Iterator </returns>
        public Enumerator GetEnumerator() => new Enumerator(this);

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
        public struct Enumerator : IEnumerator<T>
        {
            private readonly Queue<T> _collection;
            private int _currentIndex;
            private int _currentVersion;

            public Enumerator(Queue<T> collection)
            {
                this._currentIndex = -1;
                this._collection = collection;
                this._currentVersion = collection._version;
            }

            // called by foreach
            public T Current
            {
                get
                {
                    if (_currentIndex == -1 || _currentIndex == _collection.Count)
                    {
                        throw new InvalidOperationException($"It's impossible to get {nameof(Current)} for {nameof(Queue<T>)}");
                    }

                    int index = _currentIndex + _collection._head;
                    index %= _collection._data.Length;
                    return _collection._data[index];
                }
            }

            object IEnumerator.Current => Current;

            public void Reset()
            {
                _currentIndex = -1;
            }

            public bool MoveNext()
            {
                if (_currentVersion != _collection._version)
                {
                    throw new ArgumentException("Collection can't change during iteration!");
                }

                return ++_currentIndex < _collection.Count;
            }

            void IDisposable.Dispose()
            {
            }
        }
        #endregion

        #region Private methods
        private void ReorderElements()
        {
            T[] temp = new T[_data.Length];

            int i = 0;
            foreach (var element in this)
            {
                temp[i++] = element;
            }

            Array.Copy(temp, _data, temp.Length);

            _head = 0;
            _tail = temp.Length;
        }

        #endregion
    }
}
