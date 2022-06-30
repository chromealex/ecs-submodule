using System.Linq;
using Unity.Jobs;

namespace ME.ECS.Tests.MemoryAllocator.V2 {

    using ME.ECS.Collections;
    using ME.ECS.Collections.V2;
    using MemPtr = System.IntPtr;
    
    public struct TestData {

        public int a;
        public MemPtr ptr;

        public void Test(int a) {
            this.a = a;
        }

    }

    public class Base {

        [NUnit.Framework.TestAttribute]
        public void AllocResize() {

            var memBlock = new MemoryAllocator();
            memBlock.Initialize(150, 2000);

            var bytes100 = memBlock.Alloc(100);
            var bytes100_1 = memBlock.Alloc(100);

            UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100));
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Free(bytes100_1));
            
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
                arr[in memBlock, i] = new TestData() { a = i };
            }
            
            var arr2 = new MemArrayAllocator<TestData>(ref memBlock2, 200);
            for (int i = 0; i < 200; ++i) {
                arr2[in memBlock2, i] = new TestData() { a = 100 + i };
            }
            
            memBlock2.CopyFrom(memBlock);
            
            for (int i = 0; i < 100; ++i) {
                UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock, i].a, i);
                UnityEngine.Assertions.Assert.AreEqual(arr[in memBlock2, i].a, arr[in memBlock, i].a);
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
                if (ptr != null) {
                    ptr = arr[in memBlock, typeId] = memBlock.Alloc<TestData>();
                }

                ref var item = ref memBlock.Ref<TestData>(ptr);
                item.Test(i);
                
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
                ptr.Test(i);
                
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
                memBlock.Ref<TestData>(ptr).ptr = ptr;
                list[i] = memBlock.Ref<TestData>(ptr);
            }

            list = list.OrderBy(x => UnityEngine.Random.value).ToArray();

            for (int i = 0; i < cnt / 2; ++i) {
                var ptr = list[i].ptr;
                memBlock.Free(ptr);
                list[i].ptr = MemPtr.Zero;
            }

            for (int i = 0; i < cnt; ++i) {
                var ptr = list[i];
                if (ptr.ptr != MemPtr.Zero) {
                    UnityEngine.Assertions.Assert.IsTrue(ptr.a >= 0 && ptr.a < cnt);
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