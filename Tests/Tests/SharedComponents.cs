
namespace ME.ECS.Tests {

    public class SharedComponents {
        
        private class TestState : State {}

        private struct TestComponent : IComponentShared {

            public int data;

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
                    WorldUtilities.InitComponentTypeId<TestComponent>(false, isShared: true);
                    ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<TestComponent>();
                
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

        }
        
        [NUnit.Framework.TestAttribute]
        public void Set() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                });

                var entity2 = world.AddEntity();
                entity2.SetShared(new TestComponent() {
                    data = 2,
                });

                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>().data == 2);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void Remove() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                });

                var entity2 = world.AddEntity();
                entity2.SetShared(new TestComponent() {
                    data = 1,
                });
                
                entity2.RemoveShared<TestComponent>();
                entity.RemoveShared<TestComponent>();

                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>().data == 0);
                UnityEngine.Debug.Assert(entity2.ReadShared<TestComponent>().data == 0);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void Get() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                });
                
                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>().data == 1);

                ref var data = ref entity.GetShared<TestComponent>();
                data.data = 2;

                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>().data == 2);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void GetOrCreate() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                });
                
                var entity2 = world.AddEntity();
                
                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>().data == 1);

                ref var data = ref entity2.GetShared<TestComponent>();
                data.data = 2;

                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>().data == 2);
                UnityEngine.Debug.Assert(entity2.ReadShared<TestComponent>().data == 2);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void Read() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                });
                
                var entity2 = world.AddEntity();
                
                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>().data == 1);
                UnityEngine.Debug.Assert(entity2.ReadShared<TestComponent>().data == 0);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void SetGroup() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                }, 1);

                var entity2 = world.AddEntity();
                entity2.SetShared(new TestComponent() {
                    data = 2,
                });

                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>(1).data == 1);
                UnityEngine.Debug.Assert(entity2.ReadShared<TestComponent>().data == 2);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void RemoveGroup() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                }, 1);

                var entity2 = world.AddEntity();
                entity2.SetShared(new TestComponent() {
                    data = 1,
                });
                
                entity2.RemoveShared<TestComponent>();
                entity.RemoveShared<TestComponent>(1);

                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>(1).data == 0);
                UnityEngine.Debug.Assert(entity2.ReadShared<TestComponent>().data == 0);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void GetGroup() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                }, 1);
                
                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>(1).data == 1);

                ref var data = ref entity.GetShared<TestComponent>(1);
                data.data = 2;

                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>(1).data == 2);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void GetOrCreateGroup() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                }, 1);
                
                var entity2 = world.AddEntity();
                
                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>(1).data == 1);

                ref var data = ref entity2.GetShared<TestComponent>();
                data.data = 2;

                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>(1).data == 1);
                UnityEngine.Debug.Assert(entity2.ReadShared<TestComponent>().data == 2);
                
            });
            
        }

        [NUnit.Framework.TestAttribute]
        public void ReadGroup() {

            this.TestDo((world) => {
                
                var entity = world.AddEntity();
                entity.SetShared(new TestComponent() {
                    data = 1,
                }, 1);
                
                var entity2 = world.AddEntity();
                
                UnityEngine.Debug.Assert(entity.ReadShared<TestComponent>(1).data == 1);
                UnityEngine.Debug.Assert(entity2.ReadShared<TestComponent>().data == 0);
                
            });
            
        }

    }

}