using System.Linq;
using Unity.Jobs;

namespace ME.ECS.Tests {

    public class MemoryBlock {

        [NUnit.Framework.TestAttribute]
        public void Alloc() {

            var memBlock = new Collections.MemoryBlock();
            memBlock.Initialize(1000, 2000);

            var bytes100 = memBlock.Alloc(100);
            var bytes50_1 = memBlock.Alloc(50);
            var bytes50_2 = memBlock.Alloc(50);
            var bytes50_3 = memBlock.Alloc(50);

            UnityEngine.Assertions.Assert.IsTrue(memBlock.Dealloc(bytes50_1));
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Dealloc(bytes50_2));
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Dealloc(bytes50_3));
            
            var bytes50_4 = memBlock.Alloc(50);
            //UnityEngine.Assertions.Assert.AreEqual(bytes50_4.value, bytes50_1.value);
            
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Dealloc(bytes100));
            UnityEngine.Assertions.Assert.IsTrue(memBlock.Dealloc(bytes50_4));
            
        }

        [NUnit.Framework.TestAttribute]
        public void Perf() {

            var memBlock = new Collections.MemoryBlock();
            memBlock.Initialize(100000);

            var list = new ME.ECS.Collections.MemPtr[10000];
            var sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10000; ++i) {
                var ptr = memBlock.Alloc(10);
                list[i] = ptr;
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Alloc: {sw.ElapsedMilliseconds}ms");

            list = list.OrderBy(x => UnityEngine.Random.value).ToArray();
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10000; ++i) {
                memBlock.Dealloc(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");

        }

        [NUnit.Framework.TestAttribute]
        public void Perf2() {

            var memBlock = new Collections.MemoryBlock();
            memBlock.Initialize(100000);

            var list = new ME.ECS.Collections.MemPtr[10000];
            var sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10000; ++i) {
                var ptr = memBlock.Alloc(10);
                list[i] = ptr;
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Alloc: {sw.ElapsedMilliseconds}ms");

            list = list.OrderBy(x => UnityEngine.Random.value).ToArray();
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 5000; ++i) {
                memBlock.Dealloc(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");

            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10000; ++i) {
                var ptr = memBlock.Alloc(5);
                list[i] = ptr;
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Alloc: {sw.ElapsedMilliseconds}ms");
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10000; ++i) {
                memBlock.Dealloc(list[i]);
            }
            sw.Stop();
            UnityEngine.Debug.Log($"Dealloc: {sw.ElapsedMilliseconds}ms");

        }

        public class TestObj {

            public int a;
            public int b;
            public byte c;
            public byte d;

        }
        
        [NUnit.Framework.TestAttribute]
        public void PerfGC() {

            var memBlock = new Collections.MemoryBlock();
            memBlock.Initialize(100000);

            var list = new TestObj[10000];
            var sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            for (int i = 0; i < 10000; ++i) {
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
               
        }

    }

}