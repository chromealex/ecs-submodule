#if !FILTERS_STORAGE_LEGACY
namespace ME.ECS.Tests {

    public class Tests_Filters_Connect {

        private struct TestComponent : IComponent {}
        private struct TestSecondComponent : IComponent {}

        private struct TestConnectComponent : IComponent, IFilterConnect {

            public Entity entity { get; set; }

        }
        
        private class TestSystem_Connect : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            private Filter customFilter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestComponent>().Connect<TestConnectComponent>(x => x.With<TestSecondComponent>()).Push();
                this.customFilter = Filter.Create("Test").With<TestComponent>().Connect(x => x.With<TestSecondComponent>(), (in Entity entity) => entity.Read<TestConnectComponent>().entity).Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var connectedEntity = this.world.AddEntity();
                connectedEntity.Set(new TestSecondComponent());

                var entity = this.world.AddEntity();
                entity.Set(new TestConnectComponent() {
                    entity = connectedEntity,
                });
                entity.Set(new TestComponent());

                var entity2 = this.world.AddEntity();
                entity2.Set(new TestSecondComponent());

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);
                NUnit.Framework.Assert.IsTrue(this.customFilter.Count == 1);

                {
                    var realCount = 0;
                    foreach (var item in this.filter) {
                        ++realCount;
                    }

                    NUnit.Framework.Assert.IsTrue(realCount == 1);
                }
                
                {
                    var realCount = 0;
                    foreach (var item in this.customFilter) {
                        ++realCount;
                    }

                    NUnit.Framework.Assert.IsTrue(realCount == 1);
                }

                entity.Destroy();
                entity2.Destroy();
                connectedEntity.Destroy();

            }

        }

        [NUnit.Framework.TestAttribute]
        public void Connect() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, true, true);
                WorldUtilities.InitComponentTypeId<TestSecondComponent>(false, true, true);
                WorldUtilities.InitComponentTypeId<TestConnectComponent>(false, true, true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateDataBlittable<TestComponent>();
                    e.ValidateDataBlittable<TestSecondComponent>();
                    e.ValidateDataBlittable<TestConnectComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_Connect());

            });

        }

    }

}
#endif