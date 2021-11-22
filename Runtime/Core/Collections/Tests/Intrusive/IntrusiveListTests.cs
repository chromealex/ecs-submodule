
namespace ME.ECS.Collections.Tests {

    public class IntrusiveListTests {

        [NUnit.Framework.TestAttribute]
        public void Add() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(new Entity("data1"));
            list.Add(new Entity("data2"));
            list.Add(new Entity("data3"));
            list.Add(new Entity("data4"));
            list.Add(new Entity("data5"));
            
            UnityEngine.Debug.Assert(list.Count == 5);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            var world = Helpers.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(last);

            list.Remove(e);
            UnityEngine.Debug.Assert(list.Count == 4);

            list.Remove(first);
            UnityEngine.Debug.Assert(list.Count == 3);

            list.Remove(last);
            UnityEngine.Debug.Assert(list.Count == 2);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Insert() {

            var world = Helpers.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            var insert = new Entity("data6");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(last);

            UnityEngine.Debug.Assert(list.Insert(insert, 2));
            UnityEngine.Debug.Assert(list.Count == 6);

            UnityEngine.Debug.Assert(list.Insert(insert, 6));
            UnityEngine.Debug.Assert(list.Count == 7);

            UnityEngine.Debug.Assert(list.Insert(insert, 0));
            UnityEngine.Debug.Assert(list.Count == 8);

            UnityEngine.Debug.Assert(list.Insert(insert, 10) == false);
            UnityEngine.Debug.Assert(list.Count == 8);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Clear() {

            var world = Helpers.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(last);

            list.Clear();
            UnityEngine.Debug.Assert(list.Count == 0);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Contains() {

            var world = Helpers.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var notE = new Entity("data3.1");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(last);

            UnityEngine.Debug.Assert(list.Contains(e) == true);
            UnityEngine.Debug.Assert(list.Contains(notE) == false);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveAll() {

            var world = Helpers.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var notE = new Entity("data3.1");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(e);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(e);
            list.Add(last);

            UnityEngine.Debug.Assert(list.RemoveAll(notE) == 0);
            UnityEngine.Debug.Assert(list.Count == 7);

            UnityEngine.Debug.Assert(list.RemoveAll(e) == 3);
            UnityEngine.Debug.Assert(list.Count == 4);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveAt() {

            var world = Helpers.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(e);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(e);
            list.Add(last);

            UnityEngine.Debug.Assert(list.RemoveAt(3) == true);
            UnityEngine.Debug.Assert(list.Count == 6);

            UnityEngine.Debug.Assert(list.RemoveAt(10) == false);
            UnityEngine.Debug.Assert(list.Count == 6);

            UnityEngine.Debug.Assert(list.RemoveAt(0) == true);
            UnityEngine.Debug.Assert(list.Count == 5);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveRange() {

            var world = Helpers.PrepareWorld();

            var first = new Entity("data1");
            var e = new Entity("data3");
            var last = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(first);
            list.Add(e);
            list.Add(new Entity("data2"));
            list.Add(e);
            list.Add(new Entity("data4"));
            list.Add(e);
            list.Add(last);

            UnityEngine.Debug.Assert(list.RemoveRange(0, 2) == 2);
            UnityEngine.Debug.Assert(list.Count == 5);

            UnityEngine.Debug.Assert(list.RemoveRange(-2, 2) == 0);
            UnityEngine.Debug.Assert(list.Count == 5);

            UnityEngine.Debug.Assert(list.RemoveRange(-2, -1) == 0);
            UnityEngine.Debug.Assert(list.Count == 5);

            UnityEngine.Debug.Assert(list.RemoveRange(3, 10) == 2);
            UnityEngine.Debug.Assert(list.Count == 3);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void ForEach() {

            var world = Helpers.PrepareWorld();

            var e1 = new Entity("data1");
            var e2 = new Entity("data2");
            var e3 = new Entity("data3");
            var e4 = new Entity("data4");
            var e5 = new Entity("data5");
            
            var list = new ME.ECS.Collections.IntrusiveList();
            list.Add(e1);
            list.Add(e2);
            list.Add(e3);
            list.Add(e4);
            list.Add(e5);
            
            UnityEngine.Debug.Assert(list.Count == 5);

            var listArr = new Entity[5];
            var i = 0;
            foreach (var item in list) {

                listArr[i++] = item;

            }

            UnityEngine.Debug.Assert(listArr[0] == e1);
            UnityEngine.Debug.Assert(listArr[1] == e2);
            UnityEngine.Debug.Assert(listArr[2] == e3);
            UnityEngine.Debug.Assert(listArr[3] == e4);
            UnityEngine.Debug.Assert(listArr[4] == e5);

            Helpers.CompleteWorld(world);

        }

    }

}