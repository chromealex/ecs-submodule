
namespace ME.ECS.Collections.Tests {

    public class IntrusiveSortedListGenericTests {

        public struct Data : System.IEquatable<Data>, System.IComparable<Data> {

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

            public int CompareTo(Data other) {
                return string.Compare(this.a, other.a, System.StringComparison.Ordinal);
            }

            public override string ToString() {
                return "Data:" + this.a;
            }

        }
        
        [NUnit.Framework.TestAttribute]
        public void Add() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>();
            list.Add(new Data("data5"));
            list.Add(new Data("data1"));
            list.Add(new Data("data2"));
            list.Add(new Data("data3"));
            list.Add(new Data("data4"));
            list.Add(new Data("data0"));
            
            UnityEngine.Debug.Assert(list.Count == 6);

            var arr = new System.Collections.Generic.List<Data>();
            foreach (var item in list) {
                
                arr.Add(item);
                
            }
            
            NUnit.Framework.Assert.IsTrue(arr[0].a == "data0");
            NUnit.Framework.Assert.IsTrue(arr[1].a == "data1");
            NUnit.Framework.Assert.IsTrue(arr[2].a == "data2");
            NUnit.Framework.Assert.IsTrue(arr[3].a == "data3");
            NUnit.Framework.Assert.IsTrue(arr[4].a == "data4");
            NUnit.Framework.Assert.IsTrue(arr[5].a == "data5");
            
            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>();
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

            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>();
            list.Add(new Data("data5"));
            list.Add(new Data("data1"));
            list.Add(new Data("data2"));
            list.Add(new Data("data3"));
            list.Add(new Data("data4"));
            list.Add(new Data("data0"));

            list.Clear();
            UnityEngine.Debug.Assert(list.Count == 0);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Contains() {

            var world = Helpers.PrepareWorld();
            
            var e = new Data("data3");

            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>();
            list.Add(new Data("data5"));
            list.Add(new Data("data1"));
            list.Add(e);
            list.Add(new Data("data2"));
            list.Add(new Data("data3"));
            list.Add(new Data("data4"));
            list.Add(new Data("data0"));

            NUnit.Framework.Assert.IsTrue(list.Contains(e));
            
            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveAll() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var notE = new Data("data3.1");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>();
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
        public void RemoveAt() {

            var world = Helpers.PrepareWorld();

            var first = new Data("data1");
            var e = new Data("data3");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>();
            list.Add(first);
            list.Add(e);
            list.Add(new Data("data2"));
            list.Add(e);
            list.Add(new Data("data4"));
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

            var first = new Data("data1");
            var e = new Data("data3");
            var last = new Data("data5");
            
            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>();
            list.Add(first);
            list.Add(e);
            list.Add(new Data("data2"));
            list.Add(e);
            list.Add(new Data("data4"));
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

            var e0 = new Data("data5");
            var e1 = new Data("data0");
            var e2 = new Data("data4");
            var e3 = new Data("data1");
            var e4 = new Data("data3");
            var e5 = new Data("data2");
            
            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>();
            list.Add(e0);
            list.Add(e1);
            list.Add(e2);
            list.Add(e3);
            list.Add(e4);
            list.Add(e5);
            
            UnityEngine.Debug.Assert(list.Count == 6);

            var arr = new Data[6];
            var i = 0;
            foreach (var item in list) {

                arr[i++] = item;

            }

            NUnit.Framework.Assert.IsTrue(arr[0].a == "data0");
            NUnit.Framework.Assert.IsTrue(arr[1].a == "data1");
            NUnit.Framework.Assert.IsTrue(arr[2].a == "data2");
            NUnit.Framework.Assert.IsTrue(arr[3].a == "data3");
            NUnit.Framework.Assert.IsTrue(arr[4].a == "data4");
            NUnit.Framework.Assert.IsTrue(arr[5].a == "data5");

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void ForEachDescending() {

            var world = Helpers.PrepareWorld();

            var e0 = new Data("data5");
            var e1 = new Data("data0");
            var e2 = new Data("data4");
            var e3 = new Data("data1");
            var e4 = new Data("data3");
            var e5 = new Data("data2");
            
            var list = new ME.ECS.Collections.IntrusiveSortedListGeneric<Data>(descending: true);
            list.Add(e0);
            list.Add(e1);
            list.Add(e2);
            list.Add(e3);
            list.Add(e4);
            list.Add(e5);
            
            UnityEngine.Debug.Assert(list.Count == 6);

            var arr = new Data[6];
            var i = 0;
            foreach (var item in list) {

                arr[i++] = item;

            }

            NUnit.Framework.Assert.IsTrue(arr[0].a == "data5");
            NUnit.Framework.Assert.IsTrue(arr[1].a == "data4");
            NUnit.Framework.Assert.IsTrue(arr[2].a == "data3");
            NUnit.Framework.Assert.IsTrue(arr[3].a == "data2");
            NUnit.Framework.Assert.IsTrue(arr[4].a == "data1");
            NUnit.Framework.Assert.IsTrue(arr[5].a == "data0");

            Helpers.CompleteWorld(world);

        }

    }

}