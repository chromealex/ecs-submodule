
namespace ME.ECS.Collections.Tests {

    public class IntrusiveHashSetGenericTests {

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
        public void Add() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveHashSetGeneric<Data>();
            list.Add(new Data("data1"));
            list.Add(new Data("data2"));
            list.Add(new Data("data3"));
            list.Add(new Data("data4"));
            list.Add(new Data("data5"));
            
            UnityEngine.Debug.Assert(list.Count == 5);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveHashSetGeneric<Data>();
            list.Add(first);
            list.Add(new Data("data2"));
            list.Add(e);
            list.Add(new Data("data4"));
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
        public void Clear() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveHashSetGeneric<Data>();
            list.Add(first);
            list.Add(new Data("data2"));
            list.Add(e);
            list.Add(new Data("data4"));
            list.Add(last);

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
            
            var list = new ME.ECS.Collections.IntrusiveHashSetGeneric<Data>();
            list.Add(first);
            list.Add(new Data("data2"));
            list.Add(e);
            list.Add(new Data("data4"));
            list.Add(last);

            UnityEngine.Debug.Assert(list.Contains(e) == true);
            UnityEngine.Debug.Assert(list.Contains(notE) == false);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveAll() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var notE = new Data("data3.1");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveHashSetGeneric<Data>();
            list.Add(first);
            list.Add(e);
            list.Add(new Data("data2"));
            list.Add(e);
            list.Add(new Data("data4"));
            list.Add(e);
            list.Add(last);

            UnityEngine.Debug.Assert(list.RemoveAll(notE) == 0);
            UnityEngine.Debug.Assert(list.Count == 7);

            UnityEngine.Debug.Assert(list.RemoveAll(e) == 3);
            UnityEngine.Debug.Assert(list.Count == 4);

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
            
            var list = new ME.ECS.Collections.IntrusiveHashSetGeneric<Data>();
            list.Add(e1);
            list.Add(e2);
            list.Add(e3);
            list.Add(e4);
            list.Add(e5);
            
            UnityEngine.Debug.Assert(list.Count == 5);

            var listArr = new System.Collections.Generic.List<Data>();
            foreach (var item in list) {

                listArr.Add(item);

            }

            UnityEngine.Debug.Assert(listArr.Contains(e1));
            UnityEngine.Debug.Assert(listArr.Contains(e2));
            UnityEngine.Debug.Assert(listArr.Contains(e3));
            UnityEngine.Debug.Assert(listArr.Contains(e4));
            UnityEngine.Debug.Assert(listArr.Contains(e5));

            Helpers.CompleteWorld(world);

        }

    }

}