namespace ME.ECS.Tests.MemoryAllocator.V3.Collections {

    using NUnit.Framework;
    using UnityEngine;
    using ME.ECS.Collections.MemoryAllocator;

    public class List {

        [Test]
        public void Initialize() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);

            allocator.Dispose();

        }

        [Test]
        public void Performance() {

            var allocator = Base.GetAllocator(10);

            var count = 1_000_000;
            var genericList = new System.Collections.Generic.List<int>(count);
            var list = new List<int>(ref allocator, count);
            for (int i = 0; i < count; ++i) {
                list.Add(ref allocator, i);
                genericList.Add(i);
            }

            {
                var ms = System.Diagnostics.Stopwatch.StartNew();
                var sum = 0;
                for (int i = 0; i < count; ++i) {
                    sum += genericList[i];
                }

                ms.Stop();
                Debug.Log("Generic List: " + ms.ElapsedMilliseconds + "ms");
            }
            {
                var ms = System.Diagnostics.Stopwatch.StartNew();
                var sum = 0;
                for (int i = 0; i < count; ++i) {
                    sum += list[in allocator, i];
                }

                ms.Stop();
                Debug.Log("Allocator List: " + ms.ElapsedMilliseconds + "ms");
            }

            list.Dispose(ref allocator);

            allocator.Dispose();

        }

        [Test]
        public void ForEach() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
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
        public void Add() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void AddRange() {

            var allocator = Base.GetAllocator(10);

            var source = new List<int>(ref allocator, 10);
            var target = new List<int>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                source.Add(ref allocator, i);
                
            }
            
            target.AddRange(ref allocator, source);
            Assert.IsTrue(source.Count == 100);
            Assert.IsTrue(target.Count == 100);
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(source[in allocator, i] == target[in allocator, i]);
                
            }
            
            target.AddRange(ref allocator, source);
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(target[in allocator, i] == i);
                
            }
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(target[in allocator, i + 100] == i);
                
            }
            
            Assert.IsTrue(target.Count == 200);

            allocator.Dispose();

        }

        [Test]
        public void RemoveAt() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.RemoveAt(ref allocator, 50));
            Assert.IsTrue(list.Count == 99);
            Assert.IsFalse(list.RemoveAt(ref allocator, 100));
            list.Add(ref allocator, new Vector3());
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
            }
            
            for (int i = 0; i < 100; ++i) {

                Assert.IsTrue(list.Contains(in allocator, new Vector3(i, i, i)));
                
            }

            allocator.Dispose();

        }

        [Test]
        public void RemoveAtFast() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.RemoveAtFast(ref allocator, 50));
            Assert.IsTrue(list.Count == 99);
            Assert.IsFalse(list.RemoveAtFast(ref allocator, 100));
            list.Add(ref allocator, new Vector3());
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void Remove() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            Assert.IsTrue(list.Remove(ref allocator, new Vector3(50, 50, 50)));
            Assert.IsFalse(list.Remove(ref allocator, new Vector3(50, 50, 50)));
            Assert.IsTrue(list.Count == 99);

            allocator.Dispose();

        }

        [Test]
        public void Clear() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);
            list.Clear(in allocator);
            Assert.IsTrue(list.Count == 0);

            allocator.Dispose();

        }

    }

}