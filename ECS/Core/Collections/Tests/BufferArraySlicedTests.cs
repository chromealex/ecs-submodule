
using Unity.Jobs;

namespace ME.ECS.Collections.Tests {

    public class BufferArraySlicedTests {

        private ME.ECS.Collections.BufferArraySliced<ME.ECS.Name.Name> Prepare() {

            var data = new ME.ECS.Collections.BufferArraySliced<ME.ECS.Name.Name>(new BufferArray<ME.ECS.Name.Name>());
            data = data.Resize(2, false, out _);
            
            data[0] = new ME.ECS.Name.Name() { value = "1" };
            data[1] = new ME.ECS.Name.Name() { value = "2" };
            
            data = data.Resize(4, false, out _);
            data[2] = new ME.ECS.Name.Name() { value = "3" };
            data[3] = new ME.ECS.Name.Name() { value = "4" };
            
            data = data.Resize(6, false, out _);
            data[4] = new ME.ECS.Name.Name() { value = "5" };
            data[5] = new ME.ECS.Name.Name() { value = "6" };
            
            data = data.Resize(8, false, out _);
            data[6] = new ME.ECS.Name.Name() { value = "7" };
            data[7] = new ME.ECS.Name.Name() { value = "8" };
            
            data = data.Resize(10, false, out _);
            data[8] = new ME.ECS.Name.Name() { value = "9" };
            data[9] = new ME.ECS.Name.Name() { value = "10" };

            data = data.Resize(12, false, out _);
            
            data[10] = new ME.ECS.Name.Name() { value = "11" };
            data[11] = new ME.ECS.Name.Name() { value = "12" };

            return data;

        }
        
        [NUnit.Framework.TestAttribute]
        public void Change() {

            var buffer = this.Prepare();
            ref var data = ref buffer[5];
            buffer = buffer.Resize(14, false, out _);
            data.value = "Test";
            UnityEngine.Debug.Assert(buffer[5].value == data.value);
            
            buffer.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void Add() {

            var buffer = this.Prepare();
            UnityEngine.Debug.Assert(buffer[5].value == "6");
            UnityEngine.Debug.Assert(buffer[11].value == "12");
            UnityEngine.Debug.Assert(buffer.Length == 14);

            buffer.Dispose();

        }

        [NUnit.Framework.TestAttribute]
        public void Merge() {

            var buffer = this.Prepare();
            var len = buffer.Length;
            var merged = buffer.Merge();
            UnityEngine.Debug.Assert(merged[5].value == "6");
            UnityEngine.Debug.Assert(merged[11].value == "12");
            UnityEngine.Debug.Assert(len == merged.Length);
            
            merged = merged.Resize(20, false, out _);
            
            merged[16] = new ME.ECS.Name.Name() { value = "17" };
            merged[17] = new ME.ECS.Name.Name() { value = "18" };
            
            merged.Dispose();

        }

    }

}