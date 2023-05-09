namespace ME.ECS.Tests.LowLevel.Collections {

    using NUnit.Framework;
    using UnityEngine;
    using ME.ECS.Collections.LowLevel;
    using ME.ECS.Tests.LowLevel.Unsafe;

    public class Stack {

        [Test]
        public void Initialize() {

            var allocator = Base.GetAllocator(10);

            var list = new Stack<Vector3>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);

            allocator.Dispose();

        }

        [Test]
        public void ForEach() {

            var allocator = Base.GetAllocator(10);

            var list = new Stack<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Push(ref allocator, new Vector3(i, i, i));
                
            }

            var cnt = 0;
            var e = list.GetEnumerator(in allocator);
            while (e.MoveNext() == true) {
                Assert.IsTrue(e.Current.x >= 0 && e.Current.x < 100);
                ++cnt;
            }
            e.Dispose();
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(cnt == 100);

            allocator.Dispose();

        }

        [Test]
        public void Push() {

            var allocator = Base.GetAllocator(10);

            var list = new Stack<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Push(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Base.GetAllocator(10);

            var list = new Stack<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Push(ref allocator, new Vector3(i, i, i));
                
            }
            
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(list.Contains(in allocator, new Vector3(i, i, i)));
                
            }

            allocator.Dispose();

        }

        [Test]
        public void Pop() {

            var allocator = Base.GetAllocator(10);

            var list = new Stack<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Push(ref allocator, new Vector3Int(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.Pop(in allocator) == new Vector3Int(99, 99, 99));
            Assert.IsTrue(list.Pop(in allocator) == new Vector3Int(98, 98, 98));
            Assert.IsTrue(list.Count == 98);

            allocator.Dispose();

        }

        [Test]
        public void Peek() {

            var allocator = Base.GetAllocator(10);

            var list = new Stack<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Push(ref allocator, new Vector3Int(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.Peek(in allocator) == new Vector3Int(99, 99, 99));
            Assert.IsTrue(list.Pop(in allocator) == new Vector3Int(99, 99, 99));
            Assert.IsTrue(list.Count == 99);

            allocator.Dispose();

        }

        [Test]
        public void PushPop() {

            var allocator = Base.GetAllocator(10);

            var list = new Stack<Vector3Int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Push(ref allocator, new Vector3Int(i, i, i));
                
            }

            Assert.IsTrue(list.Count == 100);
            
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(list.Pop(in allocator) == new Vector3Int(100 - i - 1, 100 - i - 1, 100 - i - 1));
                
            }

            Assert.IsTrue(list.Count == 0);

            allocator.Dispose();

        }

        [Test]
        public void Clear() {

            var allocator = Base.GetAllocator(10);

            var list = new Stack<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Push(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            list.Clear(in allocator);
            Assert.IsTrue(list.Count == 0);

            allocator.Dispose();

        }

    }

}