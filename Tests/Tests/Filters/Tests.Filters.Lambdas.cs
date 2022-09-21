#if !FILTERS_LAMBDA_DISABLED
namespace ME.ECS.Tests {

    public class Tests_Filters_Lambdas {

        public struct TestComponent : IComponent {

            public int value;

        }

        public struct TestComponentLambda : ILambda<TestComponent> {

            public bool Execute(in TestComponent data) {

                return data.value > 0;

            }

        }
        
        public struct TestComponentLambda2 : ILambda<TestComponent> {

            public bool Execute(in TestComponent data) {

                return data.value > 1;

            }

        }

        private class TestSystem_Multiple : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            private Filter filter2;

            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").WithLambda<TestComponentLambda, TestComponent>().Push();
                this.filter2 = Filter.Create("Test2").WithLambda<TestComponentLambda2, TestComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entity3 = this.world.AddEntity();
                entity3.Set(new TestComponent() {
                    value = 0,
                });

                var entity = this.world.AddEntity();
                entity.Set(new TestComponent() {
                    value = 1,
                });
                
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);
                NUnit.Framework.Assert.IsTrue(this.filter2.Count == 0);
                
                var entity2 = this.world.AddEntity();
                entity2.Set(new TestComponent() {
                    value = 10,
                });

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 2);
                NUnit.Framework.Assert.IsTrue(this.filter2.Count == 1);
                
                entity.Destroy();
                entity2.Destroy();
                entity3.Destroy();

            }

        }

        private class TestSystem_Set : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").WithLambda<TestComponentLambda, TestComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entity = this.world.AddEntity();
                entity.Set(new TestComponent() {
                    value = 0,
                });
                entity.Set(new TestComponent() {
                    value = 1,
                });
                
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);
                
                var entity2 = this.world.AddEntity();
                entity2.Set(new TestComponent() {
                    value = 0,
                });

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);
                
                entity.Destroy();
                entity2.Destroy();

            }

        }

        private class TestSystem_Remove : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").WithLambda<TestComponentLambda, TestComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entity = this.world.AddEntity();
                entity.Set(new TestComponent() {
                    value = 1,
                });
                
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);

                entity.Remove<TestComponent>();

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 0);
                
                entity.Destroy();

            }

        }

        private class TestSystem_Get : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").WithLambda<TestComponentLambda, TestComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                var entity = this.world.AddEntity();
                entity.Set(new TestComponent() {
                    value = 1,
                });
                
                NUnit.Framework.Assert.IsTrue(this.filter.Count == 1);

                ref var data = ref entity.Get<TestComponent>();
                data.value = 0;

                NUnit.Framework.Assert.IsTrue(this.filter.Count == 0);
                
                entity.Destroy();

            }

        }

        [NUnit.Framework.TestAttribute]
        public void Multiple() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, isBlittable: true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateDataUnmanaged<TestComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_Multiple());

            });

        }

        [NUnit.Framework.TestAttribute]
        public void Set() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, isBlittable: true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateDataUnmanaged<TestComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_Set());

            });

        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, isBlittable: true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateDataUnmanaged<TestComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_Remove());

            });

        }

        [NUnit.Framework.TestAttribute]
        public void Get() {

            TestsHelper.Do((w) => {
                
                WorldUtilities.InitComponentTypeId<TestComponent>(false, isBlittable: true);
                ComponentsInitializerWorld.Setup((e) => {
                            
                    e.ValidateDataUnmanaged<TestComponent>();
                            
                });
                
            }, (w) => {
                
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestSystem_Get());

            });

        }

    }

}
#endif