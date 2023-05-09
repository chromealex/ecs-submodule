using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Tests {

    public class Tests_Entities_Archetypes {

        [NUnit.Framework.TestAttribute]
        public void Add() {

            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            var allocator = new ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator();
            allocator.Initialize(100);
            var st = new ME.ECS.FiltersArchetype.FiltersArchetypeStorage();
            st.Initialize(ref allocator, 100);

            var entity = st.Alloc(ref allocator);
            NUnit.Framework.Assert.AreEqual(entity.id, 0);
            NUnit.Framework.Assert.AreEqual(entity.generation, 1);

            NUnit.Framework.Assert.AreEqual(st.AliveCount, 1);
            NUnit.Framework.Assert.AreEqual(st.DeadCount(allocator), 0);
            
        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            var allocator = new ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator();
            allocator.Initialize(100);
            var st = new ME.ECS.FiltersArchetype.FiltersArchetypeStorage();
            st.Initialize(ref allocator, 100);

            var entity = st.Alloc(ref allocator);
            st.Dealloc(ref allocator, entity);
            st.ApplyDead(ref allocator);
            st.IncrementGeneration(in allocator, entity);

            NUnit.Framework.Assert.IsTrue(st.IsAlive(in allocator, entity.id, entity.generation) == false);

            NUnit.Framework.Assert.AreEqual(st.AliveCount, 0);
            NUnit.Framework.Assert.AreEqual(st.DeadCount(allocator), 1);

            {
                var entity2 = st.Alloc(ref allocator);
                NUnit.Framework.Assert.AreEqual(entity2.id, 0);
                NUnit.Framework.Assert.AreEqual(entity2.generation, 2);

                NUnit.Framework.Assert.AreEqual(st.AliveCount, 1);
                NUnit.Framework.Assert.AreEqual(st.DeadCount(allocator), 0);
            }
            
            st.Dispose(ref allocator);

        }

        [NUnit.Framework.TestAttribute]
        public void AddRemoveMulti() {

            ME.ECS.Pools.current = new ME.ECS.PoolImplementation(isNull: false);
            var allocator = new ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator();
            allocator.Initialize(100);
            var st = new ME.ECS.FiltersArchetype.FiltersArchetypeStorage();
            st.Initialize(ref allocator, 20);

            var list = new System.Collections.Generic.List<Entity>();
            var v = 1;
            for (int j = 0; j < 10; ++j) {

                list.Clear();
                for (int i = 0; i < 10000; ++i) {

                    var entity = st.Alloc(ref allocator);
                    list.Add(entity);
                    //NUnit.Framework.Assert.AreEqual(entity.id, i);
                    NUnit.Framework.Assert.AreEqual(v, entity.generation);

                    //NUnit.Framework.Assert.AreEqual(st.AliveCount, i + 1);

                    NUnit.Framework.Assert.IsTrue(st.IsAlive(in allocator, entity.id, entity.generation));

                }

                for (int i = 0; i < list.Count; ++i) {

                    st.Dealloc(ref allocator, list[i]);
                    st.IncrementGeneration(in allocator, list[i]);

                }
                
                for (int i = 0; i < 10000; ++i) {

                    var entity = st.Alloc(ref allocator);
                    list.Add(entity);
                    //NUnit.Framework.Assert.AreEqual(entity.id, i);
                    NUnit.Framework.Assert.AreEqual(v, entity.generation);

                    //NUnit.Framework.Assert.AreEqual(st.AliveCount, i + 1);

                    NUnit.Framework.Assert.IsTrue(st.IsAlive(in allocator, entity.id, entity.generation));

                }

                for (int i = 0; i < list.Count; ++i) {

                    st.Dealloc(ref allocator, list[i]);
                    st.IncrementGeneration(in allocator, list[i]);

                }

                st.ApplyDead(ref allocator);
                v += 1;

            }
            
            //UnityEngine.Debug.Log("Stats: " + st.AliveCount + " :: " + st.DeadCount);

            st.Dispose(ref allocator);

        }

    }

}
