
namespace ME.ECS.Tests {

    using System.Linq;

    public class StorageTest {

        [NUnit.Framework.TestAttribute]
        public void AddRemove() {
            
            var st = new Storage();
            st.Initialize(100);

            var entity = st.Alloc();
            NUnit.Framework.Assert.AreEqual(entity.id, 0);
            NUnit.Framework.Assert.AreEqual(entity.version, 1);

            NUnit.Framework.Assert.AreEqual(st.AliveCount, 1);
            NUnit.Framework.Assert.AreEqual(st.DeadCount, 0);

            NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.version));

            st.Dealloc(entity);
            st.ApplyDead();

            NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.version) == false);

            NUnit.Framework.Assert.AreEqual(st.AliveCount, 0);
            NUnit.Framework.Assert.AreEqual(st.DeadCount, 1);

            {
                var entity2 = st.Alloc();
                NUnit.Framework.Assert.AreEqual(entity2.id, 0);
                NUnit.Framework.Assert.AreEqual(entity2.version, 2);

                NUnit.Framework.Assert.AreEqual(st.AliveCount, 1);
                NUnit.Framework.Assert.AreEqual(st.DeadCount, 0);
            }

        }

        [NUnit.Framework.TestAttribute]
        public void AddRemoveMulti() {

            var st = new Storage();
            st.Initialize(20);

            var list = new System.Collections.Generic.List<Entity>();
            var v = 1;
            for (int j = 0; j < 10; ++j) {

                list.Clear();
                for (int i = 0; i < 10000; ++i) {

                    var entity = st.Alloc();
                    list.Add(entity);
                    //NUnit.Framework.Assert.AreEqual(entity.id, i);
                    NUnit.Framework.Assert.AreEqual(entity.version, v);

                    //NUnit.Framework.Assert.AreEqual(st.AliveCount, i + 1);

                    NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.version));

                }

                for (int i = 0; i < list.Count; ++i) {

                    st.Dealloc(list[i]);

                }
                
                for (int i = 0; i < 10000; ++i) {

                    var entity = st.Alloc();
                    list.Add(entity);
                    //NUnit.Framework.Assert.AreEqual(entity.id, i);
                    NUnit.Framework.Assert.AreEqual(entity.version, v);

                    //NUnit.Framework.Assert.AreEqual(st.AliveCount, i + 1);

                    NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.version));

                }
                
                for (int i = 0; i < list.Count; ++i) {

                    st.Dealloc(list[i]);

                }

                st.ApplyDead();
                ++v;

            }
            
            UnityEngine.Debug.Log("Stats: " + st.AliveCount + " :: " + st.DeadCount);
            
        }

    }

}