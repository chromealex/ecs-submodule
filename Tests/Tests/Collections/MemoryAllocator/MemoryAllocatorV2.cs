using System.Linq;
using Unity.Jobs;

namespace ME.ECS.Tests.MemoryAllocator.V2 {

    using ME.ECS.Collections;
    using ME.ECS.Collections.V2;
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

        [NUnit.Framework.TestAttribute]
        public void BFT() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(10, -1);

            {
                var list = new System.Collections.Generic.List<MemPtr>();
                for (int i = 2; i < 100; ++i) {

                    var typeId = i;

                    list.Add(memBlock.Alloc(typeId + typeId * 20));

                }

                for (int i = 0; i < list.Count; ++i) {

                    memBlock.Free(list[i]);

                }
            }


            {
                var list = new System.Collections.Generic.List<MemArrayAllocator<MemPtr>>();
                for (int i = 2; i < 100; ++i) {

                    var typeId = i;

                    var arr = new MemArrayAllocator<MemPtr>();
                    arr.Resize(ref memBlock, typeId + 1);
                    list.Add(arr);

                }

                for (int i = 0; i < list.Count; ++i) {

                    list[i].Dispose(ref memBlock);

                }

                //UnityEngine.Assertions.Assert.AreEqual(memBlock.freeList.Count, 98);
            }



            {
                var list = new System.Collections.Generic.List<MemArrayAllocator<MemPtr>>();
                for (int i = 2; i < 100; ++i) {

                    var typeId = i;

                    var arr = new MemArrayAllocator<MemPtr>();
                    arr.Resize(ref memBlock, typeId + 1);
                    list.Add(arr);

                }

                for (int i = list.Count - 1; i >= 0; --i) {

                    list[i].Dispose(ref memBlock);

                }

            }



            {
                for (int i = 2; i < 100; ++i) {

                    var bytes100 = memBlock.Alloc(100);
                    var bytes100_1 = memBlock.Alloc(100);

                    UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100));
                    UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100_1));

                }

                var bytes100_2 = memBlock.Alloc(150);
                memBlock.Free(bytes100_2);
            }



            {
                
                var list = new System.Collections.Generic.List<MemPtr>();
                for (int i = 2; i < 10; ++i) {

                    var len = i + 1;
                    var bytes100 = memBlock.Alloc(len * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<MemPtr>(), ClearOptions.UninitializedMemory);
                    list.Add(bytes100);

                }

                for (int i = 0; i < list.Count; ++i) {

                    UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(list[i]));

                }

                var bytes100_2 = memBlock.Alloc(150);
                memBlock.Free(bytes100_2);

            }


            {
                
                var bytes100 = memBlock.Alloc(100);
                var bytes50_1 = memBlock.Alloc(50);
                var bytes50_2 = memBlock.Alloc(50);
                var bytes50_3 = memBlock.Alloc(50);

                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes50_1));
                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes50_2));
                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes50_3));

                var bytes50_4 = memBlock.Alloc(50);
                //UnityEngine.Assertions.Assert.AreEqual(bytes50_4.value, bytes50_1.value);

                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100));
                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes50_4));
            }


            {
                var bytes100 = memBlock.Alloc(100);
                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100));
                UnityEngine.Assertions.Assert.IsFalse(memBlock.Free(bytes100));
                UnityEngine.Assertions.Assert.IsFalse(memBlock.Free(new MemPtr()));
            }


            {
                var bytes100 = memBlock.Alloc(100);
                var bytes100_2 = memBlock.Alloc(150);
                var bytes100_3 = memBlock.Alloc(100);
                memBlock.Free(bytes100_2);
                bytes100 = memBlock.ReAlloc(bytes100, 200, ClearOptions.ClearMemory);
                memBlock.Free(bytes100);
                memBlock.Free(bytes100_3);
            }


            {
                var arr = new MemArrayAllocator<TestData>(ref memBlock, 100);
                var pr = new MemArrayAllocatorProxy<TestData>(ref memBlock, arr);
                for (int i = 0; i < 100; ++i) {
                    arr[in memBlock, i] = new TestData() { a = i };
                }

                UnityEngine.Assertions.Assert.IsFalse(arr.Resize(ref memBlock, 50));
                UnityEngine.Assertions.Assert.IsTrue(arr.Resize(ref memBlock, 200));

                for (int i = 0; i < 100; ++i) {
                    UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock, i].a, i);
                }

                arr.Dispose(ref memBlock);
            }

            for (int i = 0; i < 10; ++i) {
                var bytes100 = memBlock.Alloc(1000000L);
                var bytes100_2 = memBlock.Alloc(1500000L);
                var bytes100_3 = memBlock.Alloc(10000000L);
                memBlock.Free(bytes100_2);
                bytes100 = memBlock.ReAlloc(bytes100, 200000000L, ClearOptions.ClearMemory);
                memBlock.Free(bytes100);
                memBlock.Free(bytes100_3);
            }

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void AllocLarge() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(10, -1);
            
            UnityEngine.Random.InitState(10);

            for (int i = 0; i < 10; ++i) {

                var bytes100 = memBlock.Alloc(UnityEngine.Random.Range(1, 5) * 1000000L);
                var bytes100_2 = memBlock.Alloc(UnityEngine.Random.Range(1, 5) * 1500000L);
                var bytes100_3 = memBlock.Alloc(UnityEngine.Random.Range(1, 5) * 10000000L);
                memBlock.Free(bytes100_2);
                bytes100 = memBlock.ReAlloc(bytes100, UnityEngine.Random.Range(1, 5) * 200000000L, ClearOptions.ClearMemory);
                memBlock.Free(bytes100);
                memBlock.Free(bytes100_3);

            }

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void AllocSteps() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(10, -1);

            for (int i = 2; i < 100; ++i) {

                var typeId = i;

                memBlock.Alloc(typeId + typeId * 20);

            }

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void AllocAndFree() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(10, -1);

            var list = new System.Collections.Generic.List<MemArrayAllocator<MemPtr>>();
            for (int i = 2; i < 100; ++i) {

                var typeId = i;

                var arr = new MemArrayAllocator<MemPtr>();
                arr.Resize(ref memBlock, typeId + 1);
                list.Add(arr);
                
            }

            for (int i = 0; i < list.Count; ++i) {
                
                list[i].Dispose(ref memBlock);
                
            }
            
            UnityEngine.Assertions.Assert.AreEqual(memBlock.freeList.Count, 98);

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void AllocAndFreeReverse() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(10, -1);

            var list = new System.Collections.Generic.List<MemArrayAllocator<MemPtr>>();
            for (int i = 2; i < 100; ++i) {

                var typeId = i;

                var arr = new MemArrayAllocator<MemPtr>();
                arr.Resize(ref memBlock, typeId + 1);
                list.Add(arr);
                
            }

            for (int i = list.Count - 1; i >= 0; --i) {
                
                list[i].Dispose(ref memBlock);
                
            }
            
            UnityEngine.Assertions.Assert.AreEqual(memBlock.freeList.Count, 1);
            UnityEngine.Assertions.Assert.AreEqual(memBlock.top, MemoryAllocator.ALLOCATOR_HEADER_SIZE);

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void AllocResize() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(10, 2000);

            for (int i = 2; i < 100; ++i) {

                var bytes100 = memBlock.Alloc(100);
                var bytes100_1 = memBlock.Alloc(100);

                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100));
                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100_1));

            }

            var bytes100_2 = memBlock.Alloc(150);

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void AllocResizeCache() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(1, 2000);

            var list = new System.Collections.Generic.List<MemPtr>();
            for (int i = 2; i < 10; ++i) {

                var len = i + 1;
                var bytes100 = memBlock.Alloc(len * Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<MemPtr>(), ClearOptions.UninitializedMemory);
                list.Add(bytes100);

            }

            for (int i = 0; i < list.Count; ++i) {

                UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(list[i]));

            }

            var bytes100_2 = memBlock.Alloc(150);

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void Alloc() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(1000, 2000);

            var bytes100 = memBlock.Alloc(100);
            var bytes50_1 = memBlock.Alloc(50);
            var bytes50_2 = memBlock.Alloc(50);
            var bytes50_3 = memBlock.Alloc(50);

            UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes50_1));
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes50_2));
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes50_3));

            var bytes50_4 = memBlock.Alloc(50);
            //UnityEngine.Assertions.Assert.AreEqual(bytes50_4.value, bytes50_1.value);

            UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100));
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes50_4));

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void Free() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(1000, 2000);

            var bytes100 = memBlock.Alloc(100);
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100));
            UnityEngine.Assertions.Assert.IsFalse(memBlock.Free(bytes100));
            UnityEngine.Assertions.Assert.IsFalse(memBlock.Free(new MemPtr()));
            
            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void ReAlloc() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(100, 2000);

            var bytes100 = memBlock.Alloc(100);
            var bytes100_2 = memBlock.Alloc(150);
            var bytes100_3 = memBlock.Alloc(100);
            memBlock.Free(bytes100_2);
            memBlock.ReAlloc(bytes100, 200, ClearOptions.ClearMemory);
            
            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void CopyFrom() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(1000, 2000);

            var memBlock2 = new MemoryAllocator();
            memBlock2.Initialize(1000, 2000);

            var arr = new MemArrayAllocator<TestData>(ref memBlock, 100);
            for (int i = 0; i < 100; ++i) {
                arr[in memBlock, i] = new TestData() { a = i, test = 1.5f * i };
            }
            
            var arr2 = new MemArrayAllocator<TestData>(ref memBlock2, 200);
            for (int i = 0; i < 200; ++i) {
                arr2[in memBlock2, i] = new TestData() { a = 100 + i, test = 1.5f * i };
            }
            
            memBlock2.CopyFrom(memBlock);
            
            for (int i = 0; i < 100; ++i) {
                UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock, i].a, i);
                UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock, i].test, 1.5f * i);
                UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock2, i].a, arr[in memBlock, i].a);
                UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock2, i].test, arr[in memBlock, i].test);
            }

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void LargeAlloc() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(10, -1);

            for (int i = 0; i < 10_000; ++i) {

                var typeId = i + 1;
                memBlock.Alloc(typeId);
                
            }

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void LargeAllocNoRealloc() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(10_000 * 10_000, -1);

            for (int i = 0; i < 10_000; ++i) {

                var typeId = i + 1;
                memBlock.Alloc(typeId);
                
            }

            memBlock.Dispose();

        }

    }

    public class Arrays {
        
        [NUnit.Framework.TestAttribute]
        public void LargeAlloc() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(1_000, -1);

            memBlock.Prepare(10_000 * 10_000);
            for (int i = 2; i < 10_000; ++i) {

                var typeId = i;

                var arr = new MemArrayAllocator<MemPtr>();
                arr.Resize(ref memBlock, typeId + 1);
                ref var ptr = ref arr[in memBlock, typeId];
                if (ptr != 0L) {
                    ptr = arr[in memBlock, typeId] = memBlock.Alloc<TestData>();
                }

                ref var item = ref memBlock.Ref<TestData>(ptr);
                item.Test(i, 1.5f * i);
                
            }

            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
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

        [NUnit.Framework.TestAttribute]
        public void MemArrayStaticAllocator() {

            var arr = new MemArray<TestData>(100, AllocatorType.Persistent);
            for (int i = 0; i < 100; ++i) {
                arr[i] = new TestData() { a = i };
            }

            for (int i = 0; i < 100; ++i) {
                UnityEngine.Assertions.Assert.AreEqual(arr[i].a, i);
            }

            arr.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void AllocArray() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(1000, -1);

            var arr = new MemArrayAllocator<TestData>(ref memBlock, 100);
            for (int i = 0; i < 100; ++i) {
                arr[in memBlock, i] = new TestData() { a = i };
            }

            for (int i = 0; i < 100; ++i) {
                UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock, i].a, i);
            }

            arr.Dispose(ref memBlock);
            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void ResizeArray() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(1000, -1);

            var arr = new MemArrayAllocator<TestData>(ref memBlock, 100);
            var pr = new MemArrayAllocatorProxy<TestData>(ref memBlock, arr);
            for (int i = 0; i < 100; ++i) {
                arr[in memBlock, i] = new TestData() { a = i };
            }

            UnityEngine.Assertions.Assert.IsFalse(arr.Resize(ref memBlock, 50));
            UnityEngine.Assertions.Assert.IsTrue(arr.Resize(ref memBlock, 200));

            for (int i = 0; i < 100; ++i) {
                UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock, i].a, i);
            }

            arr.Dispose(ref memBlock);
            memBlock.Dispose();

        }

    }
    
    public class MemoryAllocatorTests {

        [NUnit.Framework.TestAttribute]
        public void Perf() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(100_000_000);

            var list = new MemPtr[10_000];
            var sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10_000; ++i) {
                var ptr = memBlock.Alloc(10);
                list[i] = ptr;
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Alloc: {sw.ElapsedMilliseconds}ms");

            list = list.OrderBy(x => UnityEngine.Random.value).ToArray();
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10_000; ++i) {
                memBlock.Free(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");

            memBlock.Dispose();
            
        }

        [NUnit.Framework.TestAttribute]
        public void Test() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(40000);

            var cnt = 50;
            UnityEngine.Random.InitState(10);
            
            var list = new TestData[cnt];
            for (int i = 0; i < cnt; ++i) {
                var ptr = memBlock.Alloc<TestData>();
                memBlock.Ref<TestData>(ptr).a = i;
                memBlock.Ref<TestData>(ptr).test = 1.5f * i;
                memBlock.Ref<TestData>(ptr).ptr = ptr;
                list[i] = memBlock.Ref<TestData>(ptr);
            }

            var remList = list.OrderBy(x => UnityEngine.Random.value).ToArray();

            for (int i = 0; i < cnt / 2; ++i) {
                var ptr = remList[i].ptr;
                memBlock.Free(ptr);
                list[remList[i].a].ptr = 0;
            }

            for (int i = 0; i < cnt; ++i) {
                var ptr = list[i];
                if (ptr.ptr != 0) {
                    UnityEngine.Assertions.Assert.IsTrue(ptr.a >= 0 && ptr.a < cnt);
                    UnityEngine.Assertions.Assert.AreEqual(i, ptr.a);
                    UnityEngine.Assertions.Assert.AreEqual(1.5f * i, ptr.test);
                }
            }

            for (int i = 0; i < cnt; ++i) {
                var ptr = memBlock.Alloc<TestData>();
                memBlock.Ref<TestData>(ptr).a = i + 100;
                memBlock.Ref<TestData>(ptr).ptr = ptr;
                list[i] = memBlock.Ref<TestData>(ptr);
            }
            
            for (int i = 0; i < cnt; ++i) {
                var ptr = list[i];
                UnityEngine.Assertions.Assert.IsTrue(ptr.a >= 100 && ptr.a < cnt + 100);
                UnityEngine.Assertions.Assert.AreEqual(i + 100, ptr.a);
            }
            
            memBlock.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void Perf2() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(100_000_000);

            var list = new MemPtr[10_000];
            var sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10_000; ++i) {
                var ptr = memBlock.Alloc(10);
                list[i] = ptr;
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Alloc: {sw.ElapsedMilliseconds}ms");

            list = list.OrderBy(x => UnityEngine.Random.value).ToArray();
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 5_000; ++i) {
                memBlock.Free(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");

            var st = new MemoryAllocatorProxyDebugger(memBlock);
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10_000; ++i) {
                var ptr = memBlock.Alloc(5);
                list[i] = ptr;
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Alloc: {sw.ElapsedMilliseconds}ms");
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10_000; ++i) {
                memBlock.Free(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");
            
            memBlock.Dispose();

        }

        public class TestObj {

            public int a;
            public int b;
            public byte c;
            public byte d;

        }
        
        [NUnit.Framework.TestAttribute]
        public void PerfGC() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(100_000);

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
                //memBlock.Dealloc(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");
               
            memBlock.Dispose();
            
        }

    }

}