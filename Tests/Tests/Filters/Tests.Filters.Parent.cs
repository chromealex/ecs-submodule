#if FILTERS_STORAGE_ARCHETYPES
namespace ME.ECS.Tests {

    public class Tests_Filters_Parent {

        private struct TestComponent : IStructComponent {}
        private struct TestParentComponent : IStructComponent {}
        
        private class TestSystem_Parent : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestComponent>().Parent(x => x.With<TestParentComponent>()).Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entity = this.world.AddEntity();
                entity.Set(new TestParentComponent());
                entity.Set(new TestComponent());

                var entityParent = this.world.AddEntity();
                entityParent.Set(new TestComponent());
                entityParent.SetParent(entity);

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);

                foreach (var item in this.filter) {
                    UnityEngine.Debug.Log(item);
                }
                
                var entityParent2 = this.world.AddEntity();
                entityParent2.Set(new TestComponent());
                entityParent2.SetParent(entity);

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 2);

                entity.Destroy();

            }

        }

        [NUnit.Framework.TestAttribute]
        public void Parent() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, true);
                WorldUtilities.InitComponentTypeId<TestParentComponent>(false, true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateData<TestComponent>();
                    e.ValidateData<TestParentComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_Parent());

            });

        }

    }

}
#endif