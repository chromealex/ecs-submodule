namespace ME.ECS.Tests {

    public class EntitiesGroupTests {

        private class TestState : State {}
        private struct TestComponent : IStructComponent {}

        private class TestSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            public int testValueCount;
            private Filter filter;
            
            public void OnConstruct() {
                
                this.filter = Filter.Create("Test").With<TestComponent>().Push();
                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
                
                NUnit.Framework.Assert.AreEqual(this.testValueCount, this.filter.Count);
                
            }

        }

        private class TestStatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<TestState> {

        }

        private class TestNetworkModule : ME.ECS.Network.NetworkModule<TestState> {

            protected override ME.ECS.Network.NetworkType GetNetworkType() {
                return ME.ECS.Network.NetworkType.RunLocal | ME.ECS.Network.NetworkType.SendToNet;
            }


        }

        [NUnit.Framework.TestAttribute]
        public void AddGroup() {

            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.AddModule<TestStatesHistoryModule>();
                world.AddModule<TestNetworkModule>();
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    WorldUtilities.InitComponentTypeId<TestComponent>(false);
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<TestComponent>();
                
                    });
                }
                {
                    
                    var count = 10000;
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestSystem() {
                        testValueCount = count,
                    });

                    var marker = new Unity.Profiling.ProfilerMarker("AddEntities");
                    marker.Begin();
                    var entitiesGroup = world.AddEntities(count, Unity.Collections.Allocator.Temp, copyMode: true);
                    marker.End();
                    marker = new Unity.Profiling.ProfilerMarker("Set");
                    marker.Begin();
                    entitiesGroup.Set(new TestComponent());
                    marker.End();
                    entitiesGroup.Dispose();

                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 1);
            world.Update(1f);
            
            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

        [NUnit.Framework.TestAttribute]
        public void RemoveGroup() {

            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.AddModule<TestStatesHistoryModule>();
                world.AddModule<TestNetworkModule>();
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    WorldUtilities.InitComponentTypeId<TestComponent>(false);
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<TestComponent>();
                
                    });
                }
                {
                    
                    var count = 10000;
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestSystem() {
                        testValueCount = 0,
                    });
                    
                    var entitiesGroup = world.AddEntities(count, Unity.Collections.Allocator.Temp, copyMode: true);
                    entitiesGroup.Set(new TestComponent());
                    entitiesGroup.Remove<TestComponent>();
                    entitiesGroup.Dispose();

                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 1);
            world.Update(1f);
            
            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

        [NUnit.Framework.TestAttribute]
        public void ApplyConfig() {

            var config = UnityEngine.Resources.Load<ME.ECS.DataConfigs.DataConfig>("Test");
            
            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.AddModule<TestStatesHistoryModule>();
                world.AddModule<TestNetworkModule>();
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    WorldUtilities.InitComponentTypeId<TestComponent>(false);
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<TestComponent>();
                
                    });
                }
                {
                    
                    var count = 10000;
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestSystem() {
                        testValueCount = count,
                    });
                    
                    config.Prewarm(true);

                    var entitiesGroup = world.AddEntities(count, Unity.Collections.Allocator.Temp, copyMode: true);
                    config.Apply(entitiesGroup);
                    entitiesGroup.Dispose();

                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 1);
            world.Update(1f);
            
            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }

    }

}