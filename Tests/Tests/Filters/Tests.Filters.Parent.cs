namespace ME.ECS.Tests {

    public class Tests_Filters_Parent {

        private struct TestComponent : IComponent {}
        private struct TestParentComponent : IComponent {}
        
        private class TestSystem_Parent : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestComponent>().Parent(x => x.With<TestParentComponent>()).Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entityIgnore = this.world.AddEntity();
                entityIgnore.Set(new TestComponent());

                var entityIgnore2 = this.world.AddEntity();
                entityIgnore2.Set(new TestParentComponent());

                var entityIgnore3 = this.world.AddEntity();
                entityIgnore3.Set(new TestComponent());
                entityIgnore3.Set(new TestParentComponent());

                var entity = this.world.AddEntity();
                entity.Set(new TestParentComponent());
                entity.Set(new TestComponent());

                var entityParent = this.world.AddEntity();
                entityParent.Set(new TestComponent());
                entityParent.SetParent(entity);

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);

                {
                    var realCount = 0;
                    foreach (var item in this.filter) {
                        ++realCount;
                    }

                    NUnit.Framework.Assert.IsTrue(realCount == 1);
                }

                var entityParent2 = this.world.AddEntity();
                entityParent2.Set(new TestComponent());
                entityParent2.SetParent(entity);

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 2);

                {
                    var realCount = 0;
                    foreach (var item in this.filter) {
                        ++realCount;
                    }

                    NUnit.Framework.Assert.IsTrue(realCount == 2);
                }

                entity.Destroy();
                
                entityIgnore.Destroy();
                entityIgnore2.Destroy();
                entityIgnore3.Destroy();

            }

        }

        [NUnit.Framework.TestAttribute]
        public void Parent() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, true, true);
                WorldUtilities.InitComponentTypeId<TestParentComponent>(false, true, true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateDataUnmanaged<TestComponent>();
                    e.ValidateDataUnmanaged<TestParentComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_Parent());

            });

        }

    }

}