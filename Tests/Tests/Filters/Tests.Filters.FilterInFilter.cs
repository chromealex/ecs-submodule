
namespace ME.ECS.Tests {

    public class Tests_Filters_FilterInFilter {

        private struct TestComponent : IComponent {}
        private struct TestComponent2 : IComponent {}
        
        private class TestSystem_FilterInFilter : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter rootFilter;
            private Filter filter;
            
            public void OnConstruct() {
                
                this.rootFilter = Filter.Create("Root").With<TestComponent>().With<TestComponent2>().Push();
                this.filter = Filter.Create("Test").With<TestComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entity = this.world.AddEntity();
                entity.Set(new TestComponent());
                entity.Set(new TestComponent2());

                var entity2 = this.world.AddEntity();
                entity2.Set(new TestComponent());
                entity2.Set(new TestComponent2());

                NUnit.Framework.Assert.IsTrue(this.rootFilter.Count == 2);
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 2);

                foreach (var entRoot in this.rootFilter) {

                    foreach (var ent in this.filter) {

                        if (ent == entity) ent.Remove<TestComponent>();

                    }

                    NUnit.Framework.Assert.IsTrue(this.filter.Count == 2);

                }

                NUnit.Framework.Assert.IsTrue(this.rootFilter.Count == 1);
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);

                entity.Destroy();
                entity2.Destroy();

                NUnit.Framework.Assert.IsTrue(this.rootFilter.Count == 0);
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 0);

            }

        }

        [NUnit.Framework.TestAttribute]
        public void FilterInFilter_RemoveComponent() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, isBlittable: true);
                WorldUtilities.InitComponentTypeId<TestComponent2>(false, isBlittable: true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateDataBlittable<TestComponent>();
                    e.ValidateDataBlittable<TestComponent2>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_FilterInFilter());

            });

        }

    }

}