
namespace ME.ECS.Collections.Tests {

    public class IntrusiveRingBufferGenericTests {

        [NUnit.Framework.TestAttribute]
        public void Push() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveRingBufferGeneric<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);
            
            UnityEngine.Debug.Assert(list.Count == 3);

            list.Push(4);
            list.Push(5);
            list.Push(6);
            
            UnityEngine.Debug.Assert(list.Count == 4);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Dequeue() {

            var world = Helpers.PrepareWorld();

            var list = new ME.ECS.Collections.IntrusiveRingBufferGeneric<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);
            
            UnityEngine.Debug.Assert(list.Count == 3);

            list.Push(4);
            list.Push(5);
            list.Push(6);
            
            UnityEngine.Debug.Assert(list.Count == 4);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Clear() {

            var world = Helpers.PrepareWorld();

            var list = new ME.ECS.Collections.IntrusiveRingBufferGeneric<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);
            
            UnityEngine.Debug.Assert(list.Count == 3);

            list.Push(4);
            list.Push(5);
            list.Push(6);
            
            UnityEngine.Debug.Assert(list.Count == 4);
            
            list.Clear();

            UnityEngine.Debug.Assert(list.Count == 0);
            
            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Contains() {

            var world = Helpers.PrepareWorld();

            var list = new ME.ECS.Collections.IntrusiveRingBufferGeneric<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);
            list.Push(4);
            
            list.Push(5);
            list.Push(6);
            
            UnityEngine.Debug.Assert(list.Contains(4) == true);
            UnityEngine.Debug.Assert(list.Contains(2) == false);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void GetValue() {

            var world = Helpers.PrepareWorld();

            var list = new ME.ECS.Collections.IntrusiveRingBufferGeneric<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);
            list.Push(4);
            
            list.Push(5);
            list.Push(6);

            UnityEngine.Debug.Assert(list.GetValue(0) == 6);
            UnityEngine.Debug.Assert(list.GetValue(2) == 4);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void ForEach() {

            var world = Helpers.PrepareWorld();

            var list = new ME.ECS.Collections.IntrusiveRingBufferGeneric<int>();
            list.Push(1);
            list.Push(2);
            list.Push(3);
            list.Push(4);
            
            list.Push(5);
            list.Push(6);

            UnityEngine.Debug.Assert(list.Count == 4);

            var listArr = new int[4];
            var i = 0;
            foreach (var item in list) {

                listArr[i++] = item;

            }

            UnityEngine.Debug.Assert(listArr[0] == 6);
            UnityEngine.Debug.Assert(listArr[1] == 5);
            UnityEngine.Debug.Assert(listArr[2] == 4);
            UnityEngine.Debug.Assert(listArr[3] == 3);
            
            Helpers.CompleteWorld(world);

        }

    }

}