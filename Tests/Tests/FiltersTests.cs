
using TestView1 = ECS.submodule.Tests.TestView1;
using TestView2 = ECS.submodule.Tests.TestView2;

namespace ME.ECS.Tests {

    public class FiltersTests {

        private class TestState : State {}

        public struct TestData : IStructComponent {} 

        private class TestSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            public Entity testEntity;
            
            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").WithStructComponent<TestData>().OnVersionChangedOnly().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                UnityEngine.Debug.Log("Filter foreach:");
                foreach (var entity in this.filter) {

                    UnityEngine.Debug.Log("Entity: " + entity);

                }

                if (this.world.GetCurrentTick() % 5 == 0) {

                    this.testEntity.SetData<TestData>();
                    UnityEngine.Debug.Log("Update entity: " + this.testEntity);

                } 
                
            }

        }

        [NUnit.Framework.TestAttribute]
        public void OnVersionChangedOnly() {

            Entity testEntity;
            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                {
                    ref var str = ref world.GetStructComponents();
                    CoreComponentsInitializer.InitTypeId();
                    CoreComponentsInitializer.Init(ref str);
                    WorldUtilities.InitComponentTypeId<TestData>();
                    str.Validate<TestData>();

                    testEntity = new Entity("Test");                    
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestSystem() { testEntity = testEntity });

                    testEntity.SetData(new TestData());
                    
                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 10);

            world.PreUpdate(1f);
            world.Update(1f);
            world.LateUpdate(1f);

        }

    }
}
