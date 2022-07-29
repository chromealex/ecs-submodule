namespace ME.ECS.Tests.MemoryAllocator.V3.Collections {

    using NUnit.Framework;
    using UnityEngine;
    using ME.ECS.Collections.MemoryAllocator;

    public class Queue {

        [Test]
        public void Initialize() {

            var allocator = Base.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);

            allocator.Dispose();

        }

        [Test]
        public void ForEach() {

            var allocator = Base.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3(i, i, i));
                
            }

            var cnt = 0;
            var e = list.GetEnumerator(in allocator);
            while (e.MoveNext() == true) {
                Assert.IsTrue(e.Current.x >= 0 && e.Current.x < 100);
                ++cnt;
            }
            e.Dispose();
            
            Assert.IsTrue(list.Count(in allocator) == 100);
            Assert.IsTrue(cnt == 100);

            allocator.Dispose();

        }

        [Test]
        public void Enqueue() {

            var allocator = Base.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count(in allocator) == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Base.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3(i, i, i));
                
            }
            
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(list.Contains(in allocator, new Vector3(i, i, i)));
                
            }

            allocator.Dispose();

        }

        [Test]
        public void Dequeue() {

            var allocator = Base.GetAllocator(10);

            var list = new Queue<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3Int(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count(in allocator) == 100);
            Assert.IsTrue(list.Dequeue(in allocator) == new Vector3Int(0, 0, 0));
            Assert.IsTrue(list.Dequeue(in allocator) == new Vector3Int(1, 1, 1));
            Assert.IsTrue(list.Count(in allocator) == 98);

            allocator.Dispose();

        }

        [Test]
        public void Peek() {

            var allocator = Base.GetAllocator(10);

            var list = new Queue<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3Int(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count(in allocator) == 100);
            Assert.IsTrue(list.Peek(in allocator) == new Vector3Int(0, 0, 0));
            Assert.IsTrue(list.Dequeue(in allocator) == new Vector3Int(0, 0, 0));
            Assert.IsTrue(list.Count(in allocator) == 99);

            allocator.Dispose();

        }

        [Test]
        public void EnqueueDequeue() {

            var allocator = Base.GetAllocator(10);

            var list = new Queue<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3Int(i, i, i));
                
            }

            Assert.IsTrue(list.Count(in allocator) == 100);
            
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(list.Dequeue(in allocator) == new Vector3Int(i, i, i));
                
            }

            Assert.IsTrue(list.Count(in allocator) == 0);

            allocator.Dispose();

        }

        [Test]
        public void Clear() {

            var allocator = Base.GetAllocator(10);

            var list = new Queue<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Enqueue(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count(in allocator) == 100);
            list.Clear(in allocator);
            Assert.IsTrue(list.Count(in allocator) == 0);

            allocator.Dispose();

        }

    }

}