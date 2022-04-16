#if !FILTERS_STORAGE_LEGACY
namespace ME.ECS.Tests {

    public class Tests_Filters_OnChanged {

        private struct TestComponent : IComponent, IVersioned {}
        
        private class TestSystem_OnChanged : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            private Filter filter2;
            private Filter filterNoChanged;
            public Entity entity;
            public Entity entity2;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestComponent>().OnChanged<TestComponent>().Push();
                this.filter2 = Filter.Create("Test").With<TestComponent>().OnChanged<TestComponent>().Push();
                this.filterNoChanged = Filter.Create("Test").With<TestComponent>().Push();
                
                var entity = this.world.AddEntity();
                entity.Set(new TestComponent());
                this.entity = entity;

                var entity2 = this.world.AddEntity();
                entity2.Set(new TestComponent());
                this.entity2 = entity2;

            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                var cnt = 0;
                var changed = (this.world.GetCurrentTick() == 0 || this.world.GetCurrentTick() == 1 || this.world.GetCurrentTick() == 5);
                var changed2 = (this.world.GetCurrentTick() == 0 || this.world.GetCurrentTick() == 7 || this.world.GetCurrentTick() == 9);
                if (changed == true) {
                    ++cnt;
                    this.entity.Set<TestComponent>();
                }

                if (changed2 == true) {
                    ++cnt;
                    this.entity2.Set<TestComponent>();
                }

                {
                    NUnit.Framework.Assert.AreEqual(changed == true || changed2 == true ? cnt : 0, this.filter.Count);

                    var actualCount = 0;
                    foreach (var item in this.filter) {
                        ++actualCount;
                    }

                    NUnit.Framework.Assert.AreEqual(changed == true || changed2 == true ? cnt : 0, actualCount);
                }
                
                {
                    NUnit.Framework.Assert.AreEqual(changed == true || changed2 == true ? cnt : 0, this.filter2.Count);

                    var actualCount = 0;
                    foreach (var item in this.filter2) {
                        ++actualCount;
                    }

                    NUnit.Framework.Assert.AreEqual(changed == true || changed2 == true ? cnt : 0, actualCount);
                }

                NUnit.Framework.Assert.AreEqual(2, this.filterNoChanged.Count);
                
            }

        }

        [NUnit.Framework.TestAttribute]
        public void OnChanged() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, isVersioned: true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateData<TestComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_OnChanged());

            }, from: 0, to: 10);

        }

    }

}
#endif