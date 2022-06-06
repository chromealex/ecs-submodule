
namespace ME.ECS.Tests {

    public class Tests_Filters_AddRemove {

        private struct TestComponent : IComponent {}
        
        private class TestSystem_AddRemove : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entity = this.world.AddEntity();
                entity.Set(new TestComponent());
                
                //this.filter.ApplyAllRequests();
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);
                foreach (var ent in this.filter) {
                
                    var ent2 = this.world.AddEntity();
                    ent2.Set(new TestComponent());
                    
                    NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);
                    ent.Destroy();
                    NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);
                    
                    var ent3 = this.world.AddEntity();
                    ent3.Set(new TestComponent());
                    
                    NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);
                
                }
                
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 2);

                foreach (var ent in this.filter) {
                    ent.Destroy();
                }

            }

        }

        [NUnit.Framework.TestAttribute]
        public void AddRemove() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, true, true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateDataBlittable<TestComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_AddRemove());

            });

        }

    }

}