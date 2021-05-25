using Unity.Jobs;

namespace ME.ECSBurst.Tests {

    public class Tests {
        
        public struct Item : IComponentBase {

            public int a;

        }

        [Unity.Burst.BurstCompileAttribute]
        private struct Job : Unity.Jobs.IJob {

            public Filter filter;
            public World world;

            public void Execute() {

                var sc = world.currentState;
                
                var entity = sc.AddEntity();

                sc.Set(entity, new Item() { a = 999 });
                var element = sc.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", element.a));
                ref var a = ref sc.Get<Item>(entity);
                a.a = 123;
                UnityEngine.Debug.Log(string.Format("{0}", a.a));
                var element2 = sc.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", element2.a));
                sc.Remove<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", sc.Has<Item>(entity)));
                ref var a2 = ref sc.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", a2.a));
                a2.a = 234;
                
                foreach (var ent in filter) {

                    UnityEngine.Debug.Log(string.Format("ENTITY #{0} gen {1}", ent.id, ent.generation));
                
                }
                
                sc.RemoveEntity(in entity);
                UnityEngine.Debug.Log(string.Format("ISALIVE: {0}", sc.IsAlive(entity)));

                foreach (var ent in filter) {

                    UnityEngine.Debug.Log(string.Format("ENTITY #{0} gen {1}", ent.id, ent.generation));
                
                }

            }

        }

        public struct TestSystem : IOnCreate, IAdvanceTick {

            public void OnCreate() {

                UnityEngine.Debug.Log("OnCreate");

            }
            
            [Unity.Burst.BurstCompileAttribute]
            public void AdvanceTick(float deltaTime) {
                
                UnityEngine.Debug.Log(string.Format("TestSystem: {0}", deltaTime));
                
            }

        }
        
        [NUnit.Framework.TestAttribute]
        public void Test() {

            // TODO: Generator job - initialization - Update all components
            WorldUtilities.UpdateAllComponentTypeId<Item>();

            // TODO: Generator job - initialization - Update components in filters only
            WorldUtilities.UpdateComponentTypeId<Item>();

            var w = new World("MyWorld");
            w.AddSystem(new TestSystem());
            w.Update(0.1f);
            
            // Test filter
            var filter = Filter.Create().With<Item>().Push(ref w);
            
            // TODO: Generator job - validate per component for any new entity (inside AddEntity method)
            //w.currentState.components.Validate<Item>(1);

            // Components
            var job = new Job() {
                filter = filter,
                world = w,
            };
            job.Run();

            w.Dispose();

        }

    }
    
}