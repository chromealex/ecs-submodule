using Unity.Jobs;

namespace ME.ECSBurst.Tests {

    public class Tests {
        
        public struct Item : IComponentBase {

            public int a;

        }

        public struct Item2 : IComponentBase {

            public int a;

        }

        [Unity.Burst.BurstCompileAttribute]
        private struct Job : Unity.Jobs.IJob {

            public Filter filter;
            public World world;

            public void Execute() {

                var entity = this.world.AddEntity();

                this.world.Set(entity, new Item() { a = 999 });
                var element = this.world.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", element.a));
                ref var a = ref this.world.Get<Item>(entity);
                a.a = 123;
                UnityEngine.Debug.Log(string.Format("{0}", a.a));
                var element2 = this.world.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", element2.a));
                this.world.Remove<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", this.world.Has<Item>(entity)));
                ref var a2 = ref this.world.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", a2.a));
                a2.a = 234;
                
                foreach (var ent in filter) {

                    UnityEngine.Debug.Log(string.Format("ENTITY #{0} gen {1}", ent.id, ent.generation));
                    this.world.RemoveEntity(in ent);
                
                }
                
                UnityEngine.Debug.Log(string.Format("ISALIVE: {0}", this.world.IsAlive(entity)));

                foreach (var ent in filter) {

                    UnityEngine.Debug.Log(string.Format("ENTITY #{0} gen {1}", ent.id, ent.generation));
                
                }

            }

        }

        public struct TestSystem : IOnCreate, IAdvanceTick {

            public Filter filter;
            public World world;
            
            public void OnCreate() {

                //UnityEngine.Debug.Log("OnCreate");
                this.world = Worlds.currentWorld;
                Filter.Create().With<Item>().Without<Item2>().Push(ref this.filter);

            }
            
            public void AdvanceTick() {
                
                UnityEngine.Debug.Log(string.Format("TestSystem: {0}", Worlds.time.Data.deltaTime));
                
                var entity = this.world.AddEntity();

                this.world.Set(entity, new Item() { a = 999 });
                var element = this.world.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", element.a));
                ref var a = ref this.world.Get<Item>(entity);
                a.a = 123;
                UnityEngine.Debug.Log(string.Format("{0}", a.a));
                var element2 = this.world.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", element2.a));
                this.world.Remove<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", this.world.Has<Item>(entity)));
                ref var a2 = ref this.world.Get<Item>(entity);
                UnityEngine.Debug.Log(string.Format("{0}", a2.a));
                a2.a = 234;
                
                foreach (var ent in filter) {

                    UnityEngine.Debug.Log(string.Format("ENTITY #{0} gen {1}", ent.id, ent.generation));
                    this.world.RemoveEntity(in ent);
                
                }
                
                UnityEngine.Debug.Log(string.Format("ISALIVE: {0}", this.world.IsAlive(entity)));

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

            Burst<TestSystem>.Prewarm();

            var w = new World("MyWorld");
            //w.currentState.Validate<Item>();
            w.Validate<Item>();
            w.AddSystemAdvanceTick(new TestSystem());
            w.Update(0.1f);
            
            // Test filter
            /*var filter = Filter.Create().With<Item>().Without<Item2>().Push(ref w);
            
            // Components
            var job = new Job() {
                filter = filter,
                world = w,
            };
            job.Run();*/

            w.Dispose();
            
        }

    }
    
}