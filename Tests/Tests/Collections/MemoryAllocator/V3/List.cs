namespace ME.ECS.Tests.MemoryAllocator.V3 {

    using ME.ECS.Collections;
    using ME.ECS.Collections.V3;
    using NUnit.Framework;
    using UnityEngine;
    using ME.ECS.Collections.MemoryAllocator;

    public class List {

        [Test]
        public void Initialize() {

            var allocator = Base.GetAllocator(10);

            var list = new List<Vector3>(ref allocator, 10);
            list.Dispose(ref allocator);

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
            
            Assert.IsTrue(list.Contains(ref allocator, new Vector3(50, 50, 50)));
            
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
            list.Clear();
            Assert.IsTrue(list.Count == 0);

            allocator.Dispose();

        }

    }

}