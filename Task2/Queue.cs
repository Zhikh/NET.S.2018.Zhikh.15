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
        public int Count => _size;
        #endregion

        #region Public methods
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

        public T Dequeue()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException();
            }

            T result = _data[_head];

            _data[_head] = default(T);

            _head++; ;
            _size--;

            return result;
        }

        public T Peek()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException();
            }

            return _data[_head];
        }

        public void Clear()
        {
            Array.Clear(_data, 0, _size);
            
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        public IEnumerator<T> GetEnumerator() => new Iterator(this);
        #endregion

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

            void IDisposable.Dispose() { }
        }
        #endregion
    }
}
