
using Unity.Jobs;
using TestView1 = ECS.submodule.Tests.TestView1;
using TestView2 = ECS.submodule.Tests.TestView2;

namespace ME.ECS.Tests {

    public class Filters {

        private class TestState : State {}

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

        [NUnit.Framework.TestAttribute]
        public void MultipleFilters() {

            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    ref var str = ref world.GetStructComponents();
                    CoreComponentsInitializer.InitTypeId();
                    CoreComponentsInitializer.Init(ref str);
                    WorldUtilities.InitComponentTypeId<TestData>();
                    WorldUtilities.InitComponentTypeId<TestData2>();
                    str.Validate<TestData>();
                    str.Validate<TestData2>();

                    var testEntity = new Entity("Test");
                    var testEntity2 = new Entity("Test2");
                    var testEntity3 = new Entity("Test3");
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestMultipleFiltersSystem());

                    testEntity.Set(new TestData());
                    testEntity2.Set(new TestData2());
                    testEntity3.Set(new TestData());
                    testEntity3.Set(new TestData2());
                    
                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 10);

            world.PreUpdate(1f);
            world.Update(1f);
            world.LateUpdate(1f);

            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

        private class TestSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            public Entity testEntity;
            
            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestData>().OnVersionChangedOnly().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                UnityEngine.Debug.Log("Filter foreach:");
                foreach (var entity in this.filter) {

                    UnityEngine.Debug.Log("Entity: " + entity);

                }

                if (this.world.GetCurrentTick() % 5 == 0) {

                    this.testEntity.Set<TestData>();
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
                world.SetSeed(1u);
                {
                    ref var str = ref world.GetStructComponents();
                    CoreComponentsInitializer.InitTypeId();
                    CoreComponentsInitializer.Init(ref str);
                    WorldUtilities.InitComponentTypeId<TestData>();
                    str.Validate<TestData>();

                    testEntity = new Entity("Test");                    
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestSystem() { testEntity = testEntity });

                    testEntity.Set(new TestData());
                    
                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 10);

            world.PreUpdate(1f);
            world.Update(1f);
            world.LateUpdate(1f);

            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true)]
        public struct FilterBagJobTest : Unity.Jobs.IJob, IBurst {

            public Buffers.FilterBag<TestData, TestData2> data;

            public void Execute() {

                for (int i = 0; i < this.data.Length; ++i) {

                    ref var comp = ref this.data.GetT0(i);
                    comp.a += 1;
                    
                    this.data.RemoveT1(i);

                }
                
            }

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        private class TestJobSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            public Entity testEntity;

            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestData>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {

                {
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    sw.Start();
                    var job = new FilterBagJobTest() {
                        data = new Buffers.FilterBag<TestData, TestData2>(this.filter, Unity.Collections.Allocator.TempJob),
                    };
                    var handle = job.Schedule();
                    handle.Complete();
                    sw.Stop();
                    var step1 = sw.ElapsedMilliseconds;

                    sw = System.Diagnostics.Stopwatch.StartNew();
                    sw.Start();
                    job.data.Push();

                    sw.Stop();
                    UnityEngine.Debug.Log(step1 + "ms, " + sw.ElapsedMilliseconds + "ms. Entities: " + this.filter.Count + ": " + this.testEntity + " has data: " +
                                          this.testEntity.Read<TestData>().a);
                }

                {
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    sw.Start();
                    this.filter.ForEach((in Entity entity, in TestData2 data2, ref TestData data) => {
                        ++data.a;
                    }).WithBurst().Do();
                    sw.Stop();
                    UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms. Entities: " + this.filter.Count + ": " + this.testEntity + " has data: " +
                                          this.testEntity.Read<TestData>().a);
                }
                
            }

        }

        [NUnit.Framework.TestAttribute]
        public void FilterBag() {

            Entity testEntity;
            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    ref var str = ref world.GetStructComponents();
                    CoreComponentsInitializer.InitTypeId();
                    CoreComponentsInitializer.Init(ref str);
                    WorldUtilities.InitComponentTypeId<TestData>();
                    WorldUtilities.InitComponentTypeId<TestData2>();
                    str.Validate<TestData>();
                    str.Validate<TestData2>();

                    testEntity = new Entity("Test");
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestJobSystem() { testEntity = testEntity });

                    testEntity.Set(new TestData());
                    
                }
            }
            
            world.SetEntitiesCapacity(10000);
            for (int i = 0; i < 10000; ++i) {
                
                var test = new Entity("Test");
                test.Set(new TestData());
                
            }
            
            for (int i = 200; i < 500; ++i) {

                var ent = world.GetEntityById(i);
                ent.Destroy();

            }

            world.SaveResetState<TestState>();

            world.SetFromToTicks(0, 10);

            world.PreUpdate(1f);
            world.Update(1f);
            world.LateUpdate(1f);
            
            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

    }
}
