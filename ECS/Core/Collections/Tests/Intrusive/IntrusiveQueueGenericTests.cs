
namespace ME.ECS.Collections.Tests {

    public class IntrusiveQueueGenericTests {

        public struct Data : System.IEquatable<Data> {

            public string a;

            public Data(string str) {

                this.a = str;

            }

            public override int GetHashCode() {
                return (this.a != null ? this.a.GetHashCode() : 0);
            }

            public bool Equals(Data other) {
                return this.a == other.a;
            }

            public override bool Equals(object obj) {
                return obj is Data other && this.Equals(other);
            }

        }
        
        [NUnit.Framework.TestAttribute]
        public void Enqueue() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveQueueGeneric<Data>();
            list.Enqueue(new Data("data1"));
            list.Enqueue(new Data("data2"));
            list.Enqueue(new Data("data3"));
            list.Enqueue(new Data("data4"));
            list.Enqueue(new Data("data5"));
            
            UnityEngine.Debug.Assert(list.Count == 5);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Dequeue() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveQueueGeneric<Data>();
            list.Enqueue(first);
            list.Enqueue(new Data("data2"));
            list.Enqueue(e);
            list.Enqueue(new Data("data4"));
            list.Enqueue(last);

            var element = list.Dequeue();
            UnityEngine.Debug.Assert(list.Count == 4);
            UnityEngine.Debug.Assert(element.a == first.a);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Clear() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveQueueGeneric<Data>();
            list.Enqueue(first);
            list.Enqueue(new Data("data2"));
            list.Enqueue(e);
            list.Enqueue(new Data("data4"));
            list.Enqueue(last);

            list.Clear();
            UnityEngine.Debug.Assert(list.Count == 0);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Contains() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var notE = new Data("data3.1");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveQueueGeneric<Data>();
            list.Enqueue(first);
            list.Enqueue(new Data("data2"));
            list.Enqueue(e);
            list.Enqueue(new Data("data4"));
            list.Enqueue(last);

            UnityEngine.Debug.Assert(list.Contains(e) == true);
            UnityEngine.Debug.Assert(list.Contains(notE) == false);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Peek() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveQueueGeneric<Data>();
            list.Enqueue(first);
            list.Enqueue(new Data("data2"));
            list.Enqueue(e);
            list.Enqueue(new Data("data4"));
            list.Enqueue(last);

            UnityEngine.Debug.Assert(list.Peek().a == first.a);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void ForEach() {

            var world = Helpers.PrepareWorld();

            var e1 = new Data("data1");
            var e2 = new Data("data2");
            var e3 = new Data("data3");
            var e4 = new Data("data4");
            var e5 = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveQueueGeneric<Data>();
            list.Enqueue(e1);
            list.Enqueue(e2);
            list.Enqueue(e3);
            list.Enqueue(e4);
            list.Enqueue(e5);
            
            UnityEngine.Debug.Assert(list.Count == 5);

            var listArr = new Data[5];
            var i = 0;
            foreach (var item in list) {

                listArr[i++] = item;

            }

            UnityEngine.Debug.Assert(listArr[0].a == e1.a);
            UnityEngine.Debug.Assert(listArr[1].a == e2.a);
            UnityEngine.Debug.Assert(listArr[2].a == e3.a);
            UnityEngine.Debug.Assert(listArr[3].a == e4.a);
            UnityEngine.Debug.Assert(listArr[4].a == e5.a);

            Helpers.CompleteWorld(world);

        }

    }

}