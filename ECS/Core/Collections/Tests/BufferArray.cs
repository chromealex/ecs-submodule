using Unity.Jobs;

namespace ME.ECS.Collections.Tests {

    public class BufferArray {

        public struct TestData {

            

        }

        [Unity.Burst.BurstCompileAttribute]
        public struct BurstAccess : Unity.Jobs.IJob {

            public Unity.Collections.NativeArray<TestData> arr;
            public int index;

            public void Execute() {
                
                this.arr[this.index] = new TestData();
                
            }

        }

        [NUnit.Framework.TestAttribute]
        public void CompareBurstWithManagedAccess() {

            var arr = new Unity.Collections.NativeArray<TestData>(100, Unity.Collections.Allocator.Persistent);
            var managedArr = new TestData[100];

            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                new BurstAccess() { arr = arr, index = 50 }.Schedule().Complete();
                UnityEngine.Debug.Log("Burst ticks: " + sw.ElapsedTicks);
                sw.Restart();
            }

            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                managedArr[50] = new TestData();
                UnityEngine.Debug.Log("Managed ticks: " + sw.ElapsedTicks);
                sw.Restart();
            }

        }

        [NUnit.Framework.TestAttribute]
        public void CompareBoolWithBitArray() {

            const int size = 100000;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            {
                var arr = new System.Collections.BitArray(size);
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = true;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == true);
                }
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = false;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == false);
                }
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = i % 2 == 0 ? true : false;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == (i % 2 == 0 ? true : false));
                }
                UnityEngine.Debug.Log("BitArray ticks: " + sw.ElapsedTicks);
            }
            sw.Restart();
            {
                var arr = new BufferArrayBool(size);
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = true;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == true);
                }
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = false;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == false);
                }
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = i % 2 == 0 ? true : false;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == (i % 2 == 0 ? true : false));
                }
                UnityEngine.Debug.Log("BufferArrayBool ticks: " + sw.ElapsedTicks);
            }
            sw.Restart();
            {
                var arr = PoolArray<bool>.Spawn(size);
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = true;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == true);
                }
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = false;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == false);
                }
                for (int i = 0; i < arr.Length; ++i) {
                    arr[i] = i % 2 == 0 ? true : false;
                }
                for (int i = 0; i < arr.Length; ++i) {
                    NUnit.Framework.Assert.True(arr[i] == (i % 2 == 0 ? true : false));
                }
                UnityEngine.Debug.Log("BufferArray<bool> ticks: " + sw.ElapsedTicks);
            }

        }

    }

}