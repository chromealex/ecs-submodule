
namespace ME.ECS.Tests {

    #if FILTERS_STORAGE_LEGACY
    public class Tests_Filters_Multiple {

        public struct TestData : IStructComponent {

            public int a;

        } 

        public struct TestData2 : IStructComponent {

            public int a;

        } 

        private class TestMultipleFiltersSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            private MultipleFilter filter;
            private MultipleFilter filter2;
            
            public void OnConstruct() {
                
                this.filter = MultipleFilter.Create("Test").Any<TestData, TestData2>().Push();
                this.filter2 = MultipleFilter.Create("Test2").WithoutAny<TestData, TestData2>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                UnityEngine.Debug.Log("Filter foreach:");
                foreach (var entity in this.filter) {

                    UnityEngine.Debug.Log("Entity: " + entity);

                }

                UnityEngine.Debug.Log("Filter foreach 2:");
                foreach (var entity in this.filter2) {

                    UnityEngine.Debug.Log("Entity: " + entity);

                }

            }

        }

        private class TestDataIndexFiltersSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test")/*.With<TestData>()*/.Without<TestData2>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                UnityEngine.Debug.Log("Filter foreach:");
                foreach (var entity in this.filter) {

                    UnityEngine.Debug.Log("Entity: " + entity);

                }

            }

        }

        [NUnit.Framework.TestAttribute]
        public void DataIndexToFilters() {

            TestsHelper.Do((w) => {
                
                ref var str = ref w.GetStructComponents();
                CoreComponentsInitializer.InitTypeId();
                CoreComponentsInitializer.Init(ref str);
                WorldUtilities.InitComponentTypeId<TestData>();
                WorldUtilities.InitComponentTypeId<TestData2>();
                str.Validate<TestData>();
                str.Validate<TestData2>();
                
            }, (w) => {
                
                var testEntity = new Entity("Test");
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestDataIndexFiltersSystem());

                var index = 0;
                if (ComponentTypesRegistry.allTypeId.TryGetValue(typeof(TestData), out index) == true) {

                        
                        
                }

                w.SetData(testEntity, new TestData(), index);
                    
                index = 0;
                if (ComponentTypesRegistry.allTypeId.TryGetValue(typeof(TestData2), out index) == true) {

                        
                        
                }

                w.SetData(testEntity, new TestData2(), index);

            }, from: 0, to: 10);
            
        }

        [NUnit.Framework.TestAttribute]
        public void MultipleFilters() {

            TestsHelper.Do((w) => {
                
                ref var str = ref w.GetStructComponents();
                CoreComponentsInitializer.InitTypeId();
                CoreComponentsInitializer.Init(ref str);
                WorldUtilities.InitComponentTypeId<TestData>();
                WorldUtilities.InitComponentTypeId<TestData2>();
                str.Validate<TestData>();
                str.Validate<TestData2>();
                
            }, (w) => {
                
                var testEntity = new Entity("Test");
                var testEntity2 = new Entity("Test2");
                var testEntity3 = new Entity("Test3");
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestMultipleFiltersSystem());

                testEntity.Set(new TestData());
                testEntity2.Set(new TestData2());
                testEntity3.Set(new TestData());
                testEntity3.Set(new TestData2());

            }, from: 0, to: 10);

        }

    }
    #endif

}
