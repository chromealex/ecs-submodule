
namespace ME.ECS.Collections.Tests {
    
    public class IntrusiveDictionaryTests {

        public struct Data : System.IEquatable<Data> {

            public int a;

            public bool Equals(Data other) {
                return this.a == other.a;
            }

            public override bool Equals(object obj) {
                return obj is Data other && this.Equals(other);
            }

            public override int GetHashCode() {
                return this.a;
            }

        }

        [NUnit.Framework.TestAttribute]
        public void Add() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveDictionary<int, Data>();
            list.Add(1, new Data() { a = 10 });
            list.Add(2, new Data() { a = 20 });
            list.Add(3, new Data() { a = 30 });
            list.Add(4, new Data() { a = 40 });
            list.Add(4, new Data() { a = 40 });
            list.Add(4, new Data() { a = 40 });
            UnityEngine.Debug.Assert(list.Count == 4);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveDictionary<int, Data>();
            list.Add(1, new Data() { a = 10 });
            list.Add(2, new Data() { a = 20 });
            list.Add(3, new Data() { a = 30 });
            list.Add(4, new Data() { a = 40 });
            list.Add(4, new Data() { a = 40 });
            list.Add(4, new Data() { a = 40 });
            UnityEngine.Debug.Assert(list.Count == 4);
            UnityEngine.Debug.Assert(list.RemoveKey(2));
            UnityEngine.Debug.Assert(list.RemoveKey(100) == false);
            UnityEngine.Debug.Assert(list.Count == 3);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void TryGetValue() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveDictionary<int, Data>();
            list.Add(1, new Data() { a = 10 });
            list.Add(2, new Data() { a = 20 });
            list.Add(3, new Data() { a = 30 });
            list.Add(4, new Data() { a = 40 });

            UnityEngine.Debug.Assert(list.TryGetValue(2, out var value));
            UnityEngine.Debug.Assert(value.a == 20);

            UnityEngine.Debug.Assert(list.TryGetValue(5, out var valueNotExist) == false);
            UnityEngine.Debug.Assert(valueNotExist.a == 0);
                
            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Contains() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveDictionary<int, Data>();
            list.Add(1, new Data() { a = 10 });
            list.Add(2, new Data() { a = 20 });
            list.Add(3, new Data() { a = 30 });
            list.Add(4, new Data() { a = 40 });

            UnityEngine.Debug.Assert(list.ContainsKey(2) == true);
            UnityEngine.Debug.Assert(list.ContainsKey(5) == false);
                
            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Indexer() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveDictionary<int, Data>();
            list.Add(1, new Data() { a = 10 });
            list.Add(2, new Data() { a = 20 });
            list.Add(3, new Data() { a = 30 });
            list.Add(4, new Data() { a = 40 });
            list[5] = new Data() { a = 40 };
            list[5] = new Data() { a = 50 };
            
            UnityEngine.Debug.Assert(list.Count == 5);

            UnityEngine.Debug.Assert(list.ContainsKey(2) == true);
            UnityEngine.Debug.Assert(list.ContainsKey(5) == true);
                
            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void Clear() {

            var world = Helpers.PrepareWorld();
            
            var list = new ME.ECS.Collections.IntrusiveDictionary<int, Data>();
            list.Add(1, new Data() { a = 10 });
            list.Add(2, new Data() { a = 20 });
            list.Add(3, new Data() { a = 30 });
            list.Add(4, new Data() { a = 40 });
            
            UnityEngine.Debug.Assert(list.Count == 4);
            list.Clear();
            UnityEngine.Debug.Assert(list.Count == 0);

            Helpers.CompleteWorld(world);

        }

        [NUnit.Framework.TestAttribute]
        public void ForEach() {

            var world = Helpers.PrepareWorld();

            var e1 = new Data() { a = 10 };
            var e2 = new Data() { a = 20 };
            var e3 = new Data() { a = 30 };
            var e4 = new Data() { a = 40 };
            
            var list = new ME.ECS.Collections.IntrusiveDictionary<int, Data>();
            list.Add(1, e1);
            list.Add(2, e2);
            list.Add(3, e3);
            list.Add(4, e4);

            var listArr = new System.Collections.Generic.List<Data>();
            foreach (var item in list) {

                listArr.Add(item.value);

            }

            UnityEngine.Debug.Assert(listArr.Contains(e1));
            UnityEngine.Debug.Assert(listArr.Contains(e2));
            UnityEngine.Debug.Assert(listArr.Contains(e3));
            UnityEngine.Debug.Assert(listArr.Contains(e4));

            Helpers.CompleteWorld(world);

        }

    }

}