
using Unity.Jobs;

namespace ME.ECS.Tests {

    public class Tests_Filters_FilterBag {

        public struct TestData : IComponent {

            public int a;

        } 

        public struct TestData2 : IComponent {

            public int a;

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

                /*{
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    sw.Start();
                    this.filter.ForEach((in Entity entity, in TestData2 data2, ref TestData data) => {
                        ++data.a;
                    }).WithBurst().Do();
                    sw.Stop();
                    UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms. Entities: " + this.filter.Count + ": " + this.testEntity + " has data: " +
                                          this.testEntity.Read<TestData>().a);
                }*/
                
            }

        }

        [NUnit.Framework.TestAttribute]
        public void FilterBag() {

            TestsHelper.Do((w) => {
                
                ref var str = ref w.GetStructComponents();
                ref var str2 = ref w.GetNoStateStructComponents();
                CoreComponentsInitializer.InitTypeId();
                CoreComponentsInitializer.Init(ref str, ref str2);
                WorldUtilities.InitComponentTypeId<TestData>();
                WorldUtilities.InitComponentTypeId<TestData2>();
                str.Validate<TestData>();
                str.Validate<TestData2>();
                
            }, (w) => {
                
                var testEntity = new Entity("Test");
                var group = new SystemGroup(w, "TestGroup");
                group.AddSystem(new TestJobSystem() { testEntity = testEntity });

                testEntity.Set(new TestData());

                w.SetEntitiesCapacity(10000);
                for (int i = 0; i < 10000; ++i) {
                
                    var test = new Entity("Test");
                    test.Set(new TestData());
                
                }
            
                for (int i = 200; i < 500; ++i) {

                    var ent = w.GetEntityById(i);
                    ent.Destroy();

                }

            }, from: 0, to: 10);

        }

    }
}
