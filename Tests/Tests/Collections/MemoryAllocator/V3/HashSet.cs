namespace ME.ECS.Tests.MemoryAllocator.V3.Collections {

    using NUnit.Framework;
    using UnityEngine;
    using ME.ECS.Collections.MemoryAllocator;

    public class HashSet {

        [Test]
        public void Initialize() {

            var allocator = Base.GetAllocator(10);

            var list = new HashSet<Vector3>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);
            
            allocator.Dispose();

        }

        [Test]
        public void Add() {

            var allocator = Base.GetAllocator(10);

            var list = new HashSet<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Base.GetAllocator(10);

            var list = new HashSet<Vector3>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Vector3(i, i, i));
                
            }
            
            Assert.IsTrue(list.Contains(ref allocator, new Vector3(50, 50, 50)));
            
            allocator.Dispose();

        }

        [Test]
        public void Remove() {

            var allocator = Base.GetAllocator(10);

            var list = new HashSet<Vector3>(ref allocator, 10);
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

            var list = new HashSet<Vector3>(ref allocator, 10);
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