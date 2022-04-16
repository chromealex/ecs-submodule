using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if FILTERS_STORAGE_LEGACY
namespace ME.ECS.Tests {

    public class Tests_Entities_Storage {

        [NUnit.Framework.TestAttribute]
        public void Add() {

            var st = new Storage();
            st.Initialize(100);

            var entity = st.Alloc();
            NUnit.Framework.Assert.AreEqual(entity.id, 0);
            NUnit.Framework.Assert.AreEqual(entity.generation, 1);

            NUnit.Framework.Assert.AreEqual(st.AliveCount, 1);
            NUnit.Framework.Assert.AreEqual(st.DeadCount, 0);
            
        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            var st = new Storage();
            st.Initialize(100);

            var entity = st.Alloc();
            st.Dealloc(entity);
            st.ApplyDead();
            st.IncrementGeneration(entity);

            NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.generation) == false);

            NUnit.Framework.Assert.AreEqual(st.AliveCount, 0);
            NUnit.Framework.Assert.AreEqual(st.DeadCount, 1);

            {
                var entity2 = st.Alloc();
                NUnit.Framework.Assert.AreEqual(entity2.id, 0);
                NUnit.Framework.Assert.AreEqual(entity2.generation, 2);

                NUnit.Framework.Assert.AreEqual(st.AliveCount, 1);
                NUnit.Framework.Assert.AreEqual(st.DeadCount, 0);
            }
            
            st.Recycle();

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
                    NUnit.Framework.Assert.AreEqual(entity.generation, v);

                    //NUnit.Framework.Assert.AreEqual(st.AliveCount, i + 1);

                    NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.generation));

                }

                for (int i = 0; i < list.Count; ++i) {

                    st.Dealloc(list[i]);
                    st.IncrementGeneration(list[i]);

                }
                
                for (int i = 0; i < 10000; ++i) {

                    var entity = st.Alloc();
                    list.Add(entity);
                    //NUnit.Framework.Assert.AreEqual(entity.id, i);
                    NUnit.Framework.Assert.AreEqual(entity.generation, v);

                    //NUnit.Framework.Assert.AreEqual(st.AliveCount, i + 1);

                    NUnit.Framework.Assert.IsTrue(st.IsAlive(entity.id, entity.generation));

                }

                for (int i = 0; i < list.Count; ++i) {

                    st.Dealloc(list[i]);
                    st.IncrementGeneration(list[i]);

                }

                st.ApplyDead();
                v += 1;

            }
            
            //UnityEngine.Debug.Log("Stats: " + st.AliveCount + " :: " + st.DeadCount);

            st.Recycle();

        }

    }

}
#endif