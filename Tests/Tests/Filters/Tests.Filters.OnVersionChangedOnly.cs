
namespace ME.ECS.Tests {

    #if FILTERS_STORAGE_LEGACY
    public class Tests_Filters_OnVersionChangedOnly {

        public struct TestData : IStructComponent {

            public int a;

        } 

        private class TestSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            public Entity testEntity;
            
            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestData>().OnVersionChangedOnly().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                UnityEngine.Debug.Log("Filter foreach:");
                foreach (var entity in this.filter) {

                    UnityEngine.Debug.Log("Entity: " + entity);

                }

                if (this.world.GetCurrentTick() % 5 == 0) {

                    this.testEntity.Set<TestData>();
                    UnityEngine.Debug.Log("Update entity: " + this.testEntity);

                } 
                
            }

        }

        [NUnit.Framework.TestAttribute]
        public void OnVersionChangedOnly() {

            TestsHelper.Do((w) => {
                
                ref var str = ref w.GetStructComponents();
                CoreComponentsInitializer.InitTypeId();
                CoreComponentsInitializer.Init(ref str);
                WorldUtilities.InitComponentTypeId<TestData>(isBlittable: true);
                str.ValidateBlittable<TestData>();
                
            }, (w) => {
                
                var testEntity = new Entity("Test");                    
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem() { testEntity = testEntity });

                testEntity.Set(new TestData());

            }, from: 0, to: 10);

        }

    }
    #endif
    
}
