using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Task2.Tests
{
    [TestFixture]
    public class QueueTests
    {
        #region TestData
        private Person[][] _sourceData = 
        {
            new Person[]
            {
                new Person { LastName = "Smbd", Name = "Smbd"},
                new Person { LastName = "Smbd1", Name = "Smbd1"}
            },
            new Person[]
            {
                new Person { LastName = "Smbd", Name = "Smbd"},
                new Person { LastName = "Smbd1", Name = "Smbd1"},
                new Person { LastName = "Smbd2", Name = "Smbd2"},
                new Person { LastName = "Smbd3", Name = "Smbd3"}
            },
            new Person[]
            {
                new Person { LastName = "Smbd", Name = "Smbd"},
                new Person { LastName = "Smbd1", Name = "Smbd1"},
                new Person { LastName = "Smbd2", Name = "Smbd2"},
                new Person { LastName = "Smbd3", Name = "Smbd3"},
                null,
                new Person { LastName = "Smbd4", Name = "Smbd4"},
                new Person { LastName = "Smbd5", Name = "Smbd5"},
                null
            },
        };

        private int[] _countData = { 1, 3, 5 };
        #endregion

        #region Exceptions
        [Test]
        public void Queue_NegativeValue_ArgumentException()
            => Assert.Catch<ArgumentException>(() => new Queue<int>(-1));

        [Test]
        public void Queue_Null_ArgumentException()
            => Assert.Catch<ArgumentException>(() => new Queue<int>(null));

        [Test]
        public void Dequeue_EmptyQueue_InvalidOperationException()
        {
            var queue = new Queue<int>();

            Assert.Catch<InvalidOperationException>(() => queue.Dequeue());
        }

        [Test]
        public void Peek_EmptyQueue_InvalidOperationException()
        {
            var queue = new Queue<int>();

            Assert.Catch<InvalidOperationException>(() => queue.Peek());
        }

        [Test]
        public void Current_EmptyQueue_InvalidOperationException()
        {
            var queue = new Queue<int>();
            var iterator = queue.GetEnumerator();
            
            Assert.Catch<InvalidOperationException>(() => { var temp = iterator.Current; });
        }

        [Test]
        public void Iterate_ChangeCollection_ArgumentException()
        {
            var queue = new Queue<int>();
            var iterator = queue.GetEnumerator();

            queue.Enqueue(10);
            queue.Enqueue(20);

            Assert.Catch<ArgumentException>(() => 
            {
                foreach (var element in queue)
                {
                    queue.Dequeue();
                }
            });
        }
        #endregion

        #region Enqueue
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }, 0, 
            ExpectedResult = new int[] { 2, 3, 4, 5, 6, 7, 8, 0} )]
        public IEnumerable<int> Enque_CycleInsertWithoutResize_CorrectResult(int[] items, int value)
        {
            var queue = new Queue<int>();

            foreach (var item in items)
            {
                queue.Enqueue(item);
            }

            queue.Dequeue();
            queue.Enqueue(value);

            return queue;
        }

        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }, 0,
            ExpectedResult = new int[] { 2, 3, 4, 5, 6, 7, 8, 0, 0 })]
        public IEnumerable<int> Enque_CycleInsertWithResize_CorrectResult(int[] items, int value)
        {
            var queue = new Queue<int>();

            foreach (var item in items)
            {
                queue.Enqueue(item);
            }

            queue.Dequeue();

            queue.Enqueue(value);
            queue.Enqueue(value);

            return queue;
        }

        [TestCase(new int[] { })]
        [TestCase(new int[] { 1 })]
        [TestCase(new int[] { 1, 2, 3, 4 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199 })]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199, int.MinValue, int.MinValue, 90, int.MaxValue })]
        public void Enqueue_IntValue_AddingValueInQueue(int[] items)
        {
            var queue = new Queue<int>();

            foreach (var item in items)
            {
                queue.Enqueue(item);
            }

            int i = 0;
            foreach (var item in queue)
            {
                Assert.AreEqual(items[i++], item);
            }
        }

        [TestCase(50)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(1000000)]
        public void Enqueue_BigAmountOfValues_AddingValueInQueue(int count)
        {
            var queue = new Queue<int>();

            int[] items = new int[count];
            for (int i = 0; i < count; i++)
            {
                items[i] = i;
                queue.Enqueue(i);
            }

            int j = 0;
            foreach (var item in queue)
            {
                Assert.AreEqual(items[j++], item);
            }
        }

        [Test]
        public void Enque_ObjectValues_AddingValueInQueue()
        {
            for (int j = 0; j < _sourceData.Length; j++)
            {
                var queue = new Queue<Person>();

                foreach (var item in _sourceData[j])
                {
                    queue.Enqueue(item);
                }

                int i = 0;
                foreach (var item in queue)
                {
                    Assert.AreEqual(_sourceData[j][i++], item);
                }
            }
        }
        #endregion

        #region Count
        [TestCase(new int[] { })]
        [TestCase(new int[] { 1 })]
        [TestCase(new int[] { 1, 2, 3, 4 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199 })]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199, int.MinValue, int.MinValue, 90, int.MaxValue })]
        public void Count_IntValueAfterEnqueue_AddingValueInQueue(params int[] items)
        {
            var queue = new Queue<int>();

            foreach (var item in items)
            {
                queue.Enqueue(item);
            }

            int expected = items.Length;
            int actual = queue.Count();
            Assert.AreEqual(expected, actual);
        }

        [TestCase(new int[] { 1 }, 1)]
        [TestCase(new int[] { 1, 2, 3, 4 }, 2)]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 3)]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199 }, 1)]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199, int.MinValue, int.MinValue, 90, int.MaxValue }, 8)]
        public void Count_IntValueAfterDequeue_ExcludeValueFromQueue(int[] items, int count)
        {
            var queue = new Queue<int>();

            foreach (var item in items)
            {
                queue.Enqueue(item);
            }

            for (int i = 0; i < count; i++)
            {
                queue.Dequeue();
            }

            int expected = items.Length - count;
            int actual = queue.Count();
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Dequeue
        [TestCase(new int[] { 1 }, 1)]
        [TestCase(new int[] { 1, 2, 3, 4 }, 2)]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 3)]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199 }, 1)]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199, int.MinValue, int.MinValue, 90, int.MaxValue }, 8)]
        public void Dequeue_IntValue_ExcludeValueFromQueue(int[] items, int count)
        {
            var queue = new Queue<int>();

            foreach (var item in items)
            {
                queue.Enqueue(item);
            }

            for (int i = 0; i < count; i++)
            {
                int expected = items[i];
                int actual = queue.Dequeue();

                Assert.AreEqual(expected, actual);
            }
        }
        
        [Test]
        public void Dequeue_ObjectValues_ExcludeValueFromQueue()
        {
            for (int j = 0; j < _sourceData.Length; j++)
            {
                var queue = new Queue<Person>();

                foreach (var item in _sourceData[j])
                {
                    queue.Enqueue(item);
                }

                for (int i = 0; i < _countData[j]; i++)
                {
                    Person expected = _sourceData[j][i];
                    Person actual = queue.Dequeue();

                    Assert.AreEqual(expected, actual);
                }
            }
        }
        #endregion

        #region Peek
        [TestCase(new int[] { 1, 2, 3, 4 }, 2)]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 3)]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199 }, 1)]
        [TestCase(new int[] { 78, 902, 43, -4, 88, -199, int.MinValue, int.MinValue, 90, int.MaxValue }, 8)]
        public void Peek_IntValue_GetHeadValueFromQueue(int[] items, int count)
        {
            var queue = new Queue<int>();

            foreach (var item in items)
            {
                queue.Enqueue(item);
            }

            for (int i = 0; i < count; i++)
            {
                queue.Dequeue();
            }

            int expected = items[count];
            int actual = queue.Peek();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Peek_ObjectValues_GetHeadValueFromQueue()
        {
            for (int j = 0; j < _sourceData.Length; j++)
            {
                var queue = new Queue<Person>();

                foreach (var item in _sourceData[j])
                {
                    queue.Enqueue(item);
                }

                for (int i = 0; i < _countData[j]; i++)
                {
                    queue.Dequeue();
                }

                Person expected = _sourceData[j][_countData[j]];
                Person actual = queue.Peek();

                Assert.AreEqual(expected, actual);
            }
        }
        #endregion
    }
}
