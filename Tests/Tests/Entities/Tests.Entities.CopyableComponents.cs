
namespace ME.ECS.Tests {
    
    #if COMPONENTS_COPYABLE
    public class Tests_Entities_CopyableComponents {
        
        private class TestState : State {}

        private struct TestComponent : IStructCopyable<TestComponent> {

            public ME.ECS.Collections.ListCopyable<int> data;

            public void CopyFrom(in TestComponent other) {

                ArrayUtils.Copy(other.data, ref this.data);
                
            }

            public void OnRecycle() {
                
                PoolListCopyable<int>.Recycle(ref this.data);
                
            }

        }
        
        private class TestSystem : ISystem, IAdvanceTick {

            public World world { get; set; }

            public System.Action<World> action;
            
            public void OnConstruct() {

                
            }

            public void OnDeconstruct() {
                
            }

            public void AdvanceTick(in float deltaTime) {
            
                this.action.Invoke(this.world);
                
            }

        }

        private class TestStatesHistoryModule : ME.ECS.StatesHistory.StatesHistoryModule<TestState> {

        }

        private class TestNetworkModule : ME.ECS.Network.NetworkModule<TestState> {

            protected override ME.ECS.Network.NetworkType GetNetworkType() {
                return ME.ECS.Network.NetworkType.RunLocal | ME.ECS.Network.NetworkType.SendToNet;
            }

        }

        private void TestDo(System.Action<World> action) {
            
            World world = null;
            WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
            {
                world.AddModule<TestStatesHistoryModule>();
                world.AddModule<TestNetworkModule>();
                world.SetState<TestState>(WorldUtilities.CreateState<TestState>());
                world.SetSeed(1u);
                {
                    WorldUtilities.InitComponentTypeId<TestComponent>(false, isCopyable: true);
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateDataCopyable<TestComponent>();
                
                    });
                }
                {
                    
                    var group = new SystemGroup(world, "TestGroup");
                    group.AddSystem(new TestSystem() {
                        action = action,
                    });

                }
            }
            world.SaveResetState<TestState>();
            
            world.SetFromToTicks(0, 1);
            world.Update(1f);

            WorldUtilities.ReleaseWorld<TestState>(ref world);

        }
        
        [NUnit.Framework.TestAttribute]
        public void Set() {

            this.TestDo((world) => {

                var list = PoolListCopyable<int>.Spawn(10);
                list.Add(1);
                list.Add(2);
                list.Add(3);
                
                var entity = world.AddEntity();
                entity.Set(new TestComponent() {
                    data = list,
                });
                
                var entity2 = world.AddEntity();
                entity2.SetAs<TestComponent>(entity);
                entity2.Get<TestComponent>().data.Add(4);

                UnityEngine.Debug.Assert(entity.Read<TestComponent>().data.Count == 3);
                UnityEngine.Debug.Assert(entity2.Read<TestComponent>().data.Count == 4);

            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void Get() {

            this.TestDo((world) => {

                var list = PoolListCopyable<int>.Spawn(10);
                list.Add(1);
                list.Add(2);
                list.Add(3);
                
                var entity = world.AddEntity();
                ref var data = ref entity.Get<TestComponent>();
                data.data = list;
                
                UnityEngine.Debug.Assert(entity.Read<TestComponent>().data.Count == 3);
                
            });
            
        }

    }
    #endif

}