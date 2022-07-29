namespace ME.ECS.Tests.MemoryAllocator.V3.Collections {

    using NUnit.Framework;
    using UnityEngine;
    using ME.ECS.Collections.MemoryAllocator;

    public class NativeHashSet {

        public struct Test : IEquatableAllocator<Test> {

            public float x;
            public float y;
            public float z;

            public Test(float x, float y, float z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public bool Equals(in ME.ECS.Collections.V3.MemoryAllocator allocator, Test obj) {
                return this.x == obj.x &&
                    this.y == obj.y &&
                    this.z == obj.z;
            }

            public int GetHash(in ME.ECS.Collections.V3.MemoryAllocator allocator) {
                return this.x.GetHashCode() ^ this.y.GetHashCode() ^ this.z.GetHashCode();
            }

        }

        [Test]
        public void Initialize() {

            var allocator = Base.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            Assert.IsTrue(list.isCreated);
            list.Dispose(ref allocator);
            
            allocator.Dispose();

        }

        [Test]
        public void ForEach() {

            var allocator = Base.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
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
        public void Add() {

            var allocator = Base.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count(in allocator) == 100);

            allocator.Dispose();

        }

        [Test]
        public void Contains() {

            var allocator = Base.GetAllocator(10);

            var cnt = 2;
            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < cnt; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
            }
            
            for (int i = 0; i < cnt; ++i) {

                Assert.IsTrue(list.Contains(in allocator, new Test(i, i, i)));
                
            }
            
            allocator.Dispose();

        }

        [Test]
        public void Remove() {

            var allocator = Base.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count(in allocator) == 100);
            Assert.IsTrue(list.Remove(ref allocator, new Test(50, 50, 50)));
            Assert.IsFalse(list.Remove(ref allocator, new Test(50, 50, 50)));
            Assert.IsTrue(list.Count(in allocator) == 99);

            allocator.Dispose();

        }

        [Test]
        public void Clear() {

            var allocator = Base.GetAllocator(10);

            var list = new NativeHashSet<Test>(ref allocator, 10);
            for (int i = 0; i < 100; ++i) {

                list.Add(ref allocator, new Test(i, i, i));
                
            }
            
            Assert.IsTrue(list.Count(in allocator) == 100);
            list.Clear(in allocator);
            Assert.IsTrue(list.Count(in allocator) == 0);

            allocator.Dispose();

        }

    }

}