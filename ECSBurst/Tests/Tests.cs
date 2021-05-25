using Unity.Jobs;

namespace ME.ECSBurst.Tests {

    public class Tests {
        
        public struct Item : IComponentBase {

            public int a;

        }

        [Unity.Burst.BurstCompileAttribute]
        private struct Job : Unity.Jobs.IJob {

            public Filter filter;
            public Entity entity;
            public StateStruct sc;

            public void Execute() {
                
                sc.Set(this.entity, new Item() { a = 999 });
                var element = sc.Get<Item>(this.entity);
                UnityEngine.Debug.Log(string.Format("{0}", element.a));
                ref var a = ref sc.Get<Item>(this.entity);
                a.a = 123;
                UnityEngine.Debug.Log(string.Format("{0}", a.a));
                var element2 = sc.Get<Item>(this.entity);
                UnityEngine.Debug.Log(string.Format("{0}", element2.a));
                sc.Remove<Item>(this.entity);
                UnityEngine.Debug.Log(string.Format("{0}", sc.Has<Item>(this.entity)));
                ref var a2 = ref sc.Get<Item>(this.entity);
                UnityEngine.Debug.Log(string.Format("{0}", a2.a));
                a2.a = 234;

                foreach (var ent in filter) {

                    UnityEngine.Debug.Log(string.Format("ENTITY #{0} gen {1}", ent.id, ent.generation));
                
                }

            }

        }
        
        [NUnit.Framework.TestAttribute]
        public void Test() {

            // TODO: Generator job - initialization - Update all components
            WorldUtilities.UpdateAllComponentTypeId<Item>();

            // TODO: Generator job - initialization - Update components in filters only
            WorldUtilities.UpdateComponentTypeId<Item>();

            var sc = new StateStruct();
            sc.Initialize(100);
            
            // TODO: Generator job - validate per component for any new entity
            sc.components.Validate<Item>(1);
            
            // Test filter
            var filter = Filter.Create().With<Item>().Push(ref sc);
            
            var entity = sc.AddEntity();
            
            // Components
            var job = new Job() {
                entity = entity,
                filter = filter,
                sc = sc,
            };
            job.Run();

            UnityEngine.Debug.Log("RES: " + sc.Get<Item>(entity).a);

            // TODO: Generator job - dispose per component
            sc.components.Dispose<Item>();
            
            sc.Dispose();

        }

    }
    
}