#if !FILTERS_STORAGE_LEGACY
namespace ME.ECS.Tests {

    public class Tests_Filters_Any {

        private struct TestComponent : IComponent {}
        private struct TestComponent1 : IComponent {}
        private struct TestComponent2 : IComponent {}
        private struct TestComponent3 : IComponent {}
        
        private class TestSystem_Any : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestComponent>().Any<TestComponent1, TestComponent2>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                {
                    var entity4 = this.world.AddEntity();
                    entity4.Set(new TestComponent3());

                    var entity = this.world.AddEntity();
                    entity.Set(new TestComponent());
                    entity.Set(new TestComponent1());

                    var entity2 = this.world.AddEntity();
                    entity2.Set(new TestComponent());
                    entity2.Set(new TestComponent3());

                    var entity3 = this.world.AddEntity();
                    entity3.Set(new TestComponent());
                    entity3.Set(new TestComponent2());

                    var entity5 = this.world.AddEntity();
                    entity5.Set(new TestComponent3());

                    var entity6 = this.world.AddEntity();
                    entity6.Set(new TestComponent());

                    NUnit.Framework.Assert.IsTrue(this.filter.Count == 2);

                    foreach (var ent in this.filter) {
                        ent.Destroy();
                    }
                    
                    entity2.Destroy();
                    entity4.Destroy();
                    entity5.Destroy();
                    entity6.Destroy();
                }

                {
                    var entity = this.world.AddEntity();
                    entity.Set(new TestComponent());
                    entity.Set(new TestComponent2());

                    NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);

                    foreach (var ent in this.filter) {
                        ent.Destroy();
                    }
                }

                {
                    var entity = this.world.AddEntity();
                    entity.Set(new TestComponent());
                    entity.Set(new TestComponent3());

                    NUnit.Framework.Assert.IsTrue(this.filter.Count == 0);

                    entity.Destroy();
                }

                {
                    var entity = this.world.AddEntity();
                    entity.Set(new TestComponent());
                    entity.Set(new TestComponent1());
                    entity.Set(new TestComponent2());

                    NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);

                    foreach (var ent in this.filter) {
                        ent.Destroy();
                    }
                }

            }

        }

        [NUnit.Framework.TestAttribute]
        public void Any() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, true);
                WorldUtilities.InitComponentTypeId<TestComponent1>(false, true);
                WorldUtilities.InitComponentTypeId<TestComponent2>(false, true);
                WorldUtilities.InitComponentTypeId<TestComponent3>(false, true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateData<TestComponent>();
                    e.ValidateData<TestComponent1>();
                    e.ValidateData<TestComponent2>();
                    e.ValidateData<TestComponent3>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_Any());

            });

        }

    }

}
#endif