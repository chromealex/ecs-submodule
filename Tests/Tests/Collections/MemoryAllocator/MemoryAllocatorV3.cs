using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace ME.ECS.Tests.MemoryAllocator.V3 {

    using ME.ECS.Collections;
    using ME.ECS.Collections.V3;
    using MemPtr = System.Int64;
    
    public struct TestData {

        public byte b;
        public float test;
        public int a;
        public MemPtr ptr;

        public void Test(int a, float test) {
            this.a = a;
            this.test = test;
        }

    }

    public class Base {

        public static MemoryAllocator GetAllocator(long size, long maxSize = -1) {
            var allocator = new MemoryAllocator();
            return allocator.Initialize(10, -1);
        }

        [Test]
        public void AllocLarge() {
            using (var allocator = Base.GetAllocator(10)) {
                UnityEngine.Random.InitState(10);

                for (int i = 0; i < 10; ++i) {

                    var bytes100 = allocator.Alloc(UnityEngine.Random.Range(1, 5) * 1000000L);
                    var bytes100_2 = allocator.Alloc(UnityEngine.Random.Range(1, 5) * 1500000L);
                    var bytes100_3 = allocator.Alloc(UnityEngine.Random.Range(1, 5) * 10000000L);
                    allocator.Free(bytes100_2);
                    bytes100 = allocator.ReAlloc(bytes100, UnityEngine.Random.Range(1, 5) * 200000000L, ClearOptions.ClearMemory);
                    allocator.Free(bytes100);
                    allocator.Free(bytes100_3);

                }
            }
        }

        [Test]
        public void AllocSteps() {
            using (var allocator = Base.GetAllocator(10)) {
                for (int i = 2; i < 100; ++i) {

                    var typeId = i;

                    allocator.Alloc(typeId + typeId * 20);

                }
            }
        }

        [Test]
        public void AllocAndFree() {

            var allocator = Base.GetAllocator(10);

            var list = new System.Collections.Generic.List<MemArrayAllocator<MemPtr>>();
            for (int i = 2; i < 100; ++i) {

                var typeId = i;

                var arr = new MemArrayAllocator<MemPtr>(ref allocator, typeId + 1);
                list.Add(arr);
                
            }

            for (int i = 0; i < list.Count; ++i) {
                
                list[i].Dispose(ref allocator);
                
            }
            
            allocator.Dispose();

        }

        [Test]
        public void AllocAndFreeReverse() {

            var allocator = Base.GetAllocator(10);

            var list = new System.Collections.Generic.List<MemArrayAllocator<MemPtr>>();
            for (int i = 2; i < 100; ++i) {

                var typeId = i;

                var arr = new MemArrayAllocator<MemPtr>();
                arr.Resize(ref allocator, typeId + 1);
                list.Add(arr);
                
            }

            for (int i = list.Count - 1; i >= 0; --i) {
                
                list[i].Dispose(ref allocator);
                
            }

            allocator.Dispose();

        }

        [Test]
        public void AllocResize() {
            using (var allocator = Base.GetAllocator(10, 2000)) {
                for (int i = 2; i < 100; ++i) {

                    var bytes100 = allocator.Alloc(100);
                    var bytes100_1 = allocator.Alloc(100);

                    Assert.IsTrue(allocator.Free(bytes100));
                    Assert.IsTrue(allocator.Free(bytes100_1));

                }

                var bytes100_2 = allocator.Alloc(150);
            }
        }

        [Test]
        public void AllocResizeCache() {
            using (var allocator = Base.GetAllocator(10, 2000)) {
                var list = new System.Collections.Generic.List<MemPtr>();
                for (int i = 2; i < 10; ++i) {

                    var len = i + 1;
                    var bytes100 = allocator.Alloc(len * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<MemPtr>(), ClearOptions.UninitializedMemory);
                    list.Add(bytes100);

                }

                for (int i = 0; i < list.Count; ++i) {

                    UnityEngine.Assertions.Assert.IsTrue(allocator.Free(list[i]));

                }

                var bytes100_2 = allocator.Alloc(150);
            }
        }

        [Test]
        public void Alloc() {

            using (var allocator = Base.GetAllocator(1000, 2000)) {

                var bytes100 = allocator.Alloc(100);
                var bytes50_1 = allocator.Alloc(50);
                var bytes50_2 = allocator.Alloc(50);
                var bytes50_3 = allocator.Alloc(50);

                Assert.IsTrue(allocator.Free(bytes50_1));
                Assert.IsTrue(allocator.Free(bytes50_2));
                Assert.IsTrue(allocator.Free(bytes50_3));

                var bytes50_4 = allocator.Alloc(50);

                Assert.IsTrue(allocator.Free(bytes100));
                Assert.IsTrue(allocator.Free(bytes50_4));

            }

        }

        [Test]
        public void Free() {

            using (var allocator = Base.GetAllocator(1000, 2000)) {

                var bytes100 = allocator.Alloc(100);

                Assert.IsTrue(allocator.Free(bytes100));
                Assert.IsFalse(allocator.Free(bytes100));
                Assert.IsFalse(allocator.Free(new MemPtr()));

            }

        }

        [Test]
        public void ReAlloc() {

            using (var allocator = Base.GetAllocator(100, 2000)) {

                var bytes100 = allocator.Alloc(100);
                var bytes100_2 = allocator.Alloc(150);
                var bytes100_3 = allocator.Alloc(100);
                allocator.Free(bytes100_2);
                allocator.ReAlloc(bytes100, 200, ClearOptions.ClearMemory);

            }

        }

        [Test]
        public void CopyFrom() {

            var allocator = Base.GetAllocator(1000, 2000);
            var allocator2 = Base.GetAllocator(1000, 2000);

            var arr = new MemArrayAllocator<TestData>(ref allocator, 100);
            for (int i = 0; i < 100; ++i) {
                arr[in allocator, i] = new TestData() { a = i, test = 1.5f * i };
            }
            
            var arr2 = new MemArrayAllocator<TestData>(ref allocator2, 200);
            for (int i = 0; i < 200; ++i) {
                arr2[in allocator2, i] = new TestData() { a = 100 + i, test = 1.5f * i };
            }
            
            allocator2.CopyFrom(allocator);
            
            for (int i = 0; i < 100; ++i) {
                Assert.AreEqual(arr[in allocator, i].a, i);
                Assert.AreEqual(arr[in allocator, i].test, 1.5f * i);
                Assert.AreEqual(arr[in allocator2, i].a, arr[in allocator, i].a);
                Assert.AreEqual(arr[in allocator2, i].test, arr[in allocator, i].test);
            }

            allocator.Dispose();

        }

        [Test]
        public void LargeAlloc() {

            using (var allocator = Base.GetAllocator(10)) {

                for (int i = 0; i < 10_000; ++i) {

                    var typeId = i + 1;
                    allocator.Alloc(typeId);

                }

            }

        }

        [Test]
        public void LargeAllocNoRealloc() {
            
            using (var allocator = Base.GetAllocator(10_000 * 10_000)) {

                for (int i = 0; i < 10_000; ++i) {

                    var typeId = i + 1;
                    allocator.Alloc(typeId);

                }

            }

        }

    }

    public class Arrays {
        
        [Test]
        public void LargeAlloc() {

            var allocator = Base.GetAllocator(1000);

            allocator.Prepare(10_000 * 10_000);
            for (int i = 2; i < 10_000; ++i) {

                var typeId = i;

                var arr = new MemArrayAllocator<MemPtr>();
                arr.Resize(ref allocator, typeId + 1);
                ref var ptr = ref arr[in allocator, typeId];
                if (ptr != 0L) {
                    ptr = arr[in allocator, typeId] = allocator.Alloc<TestData>();
                }

                ref var item = ref allocator.Ref<TestData>(ptr);
                item.Test(i, 1.5f * i);
                
            }

            allocator.Dispose();

        }

        [Test]
        public void LargeAllocGC() {

            System.GC.AddMemoryPressure(10_000 * 10_000);
            for (int i = 2; i < 10_000; ++i) {

                var typeId = i;

                var arr = new TestData[0];
                System.Array.Resize(ref arr, typeId + 1);
                ref var ptr = ref arr[typeId];
                ptr.Test(i, 1.5f * i);
                
            }

        }

        [Test]
        public void MemArrayStaticAllocator() {

            var arr = new MemArray<TestData>(100, AllocatorType.Persistent);
            for (int i = 0; i < 100; ++i) {
                arr[i] = new TestData() { a = i };
            }

            for (int i = 0; i < 100; ++i) {
                Assert.AreEqual(arr[i].a, i);
            }

            arr.Dispose();

        }

        [Test]
        public void AllocArray() {

            var allocator = Base.GetAllocator(1000);

            var arr = new MemArrayAllocator<TestData>(ref allocator, 100);
            for (int i = 0; i < 100; ++i) {
                arr[in allocator, i] = new TestData() { a = i };
            }

            for (int i = 0; i < 100; ++i) {
                Assert.AreEqual(arr[in allocator, i].a, i);
            }

            arr.Dispose(ref allocator);
            allocator.Dispose();

        }

        [Test]
        public void ResizeArray() {

            var allocator = Base.GetAllocator(1000);

            var arr = new MemArrayAllocator<TestData>(ref allocator, 100);
            var pr = new MemArrayAllocatorProxy<TestData>(ref allocator, arr);
            for (int i = 0; i < 100; ++i) {
                arr[in allocator, i] = new TestData() { a = i };
            }

            Assert.IsFalse(arr.Resize(ref allocator, 50));
            Assert.IsTrue(arr.Resize(ref allocator, 200));

            for (int i = 0; i < 100; ++i) {
                Assert.AreEqual(arr[in allocator, i].a, i);
            }

            arr.Dispose(ref allocator);
            allocator.Dispose();

        }

    }
    
    public class MemoryAllocatorTests {

        [Test]
        public void Perf() {
            
            var allocator = Base.GetAllocator(100_000_000);

            var list = new MemPtr[10_000];
            var sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10_000; ++i) {
                var ptr = allocator.Alloc(10);
                list[i] = ptr;
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Alloc: {sw.ElapsedMilliseconds}ms");

            list = list.OrderBy(x => UnityEngine.Random.value).ToArray();
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10_000; ++i) {
                allocator.Free(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");

            allocator.Dispose();
            
        }

        [Test]
        public void Test() {
            
            var allocator = Base.GetAllocator(40000);

            var cnt = 50;
            UnityEngine.Random.InitState(10);
            
            var list = new TestData[cnt];
            for (int i = 0; i < cnt; ++i) {
                var ptr = allocator.Alloc<TestData>();
                allocator.Ref<TestData>(ptr).a = i;
                allocator.Ref<TestData>(ptr).test = 1.5f * i;
                allocator.Ref<TestData>(ptr).ptr = ptr;
                list[i] = allocator.Ref<TestData>(ptr);
            }

            var remList = list.OrderBy(x => UnityEngine.Random.value).ToArray();

            for (int i = 0; i < cnt / 2; ++i) {
                var ptr = remList[i].ptr;
                allocator.Free(ptr);
                list[remList[i].a].ptr = 0L;
            }

            for (int i = 0; i < cnt; ++i) {
                var ptr = list[i];
                if (ptr.ptr != 0L) {
                    Assert.IsTrue(ptr.a >= 0 && ptr.a < cnt);
                    Assert.AreEqual(i, ptr.a);
                    Assert.AreEqual(1.5f * i, ptr.test);
                }
            }

            for (int i = 0; i < cnt; ++i) {
                var ptr = allocator.Alloc<TestData>();
                allocator.Ref<TestData>(ptr).a = i + 100;
                allocator.Ref<TestData>(ptr).ptr = ptr;
                list[i] = allocator.Ref<TestData>(ptr);
            }
            
            for (int i = 0; i < cnt; ++i) {
                var ptr = list[i];
                Assert.IsTrue(ptr.a >= 100 && ptr.a < cnt + 100);
                Assert.AreEqual(i + 100, ptr.a);
            }
            
            allocator.Dispose();

        }
        
        public class TestObj {

            public int a;
            public int b;
            public byte c;
            public byte d;

        }
        
        [Test]
        public void PerfGC() {
            
            var allocator = Base.GetAllocator(100_000);

            var list = new TestObj[10_000];
            var sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10_000; ++i) {
                var ptr = new TestObj();
                list[i] = ptr;
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Alloc: {sw.ElapsedMilliseconds}ms");

            list = list.OrderBy(x => UnityEngine.Random.value).ToArray();
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10000; ++i) {
                //allocator.Dealloc(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");
               
            allocator.Dispose();
            
        }

    }

}