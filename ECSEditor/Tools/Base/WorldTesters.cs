using ME.ECS;

namespace ME.ECSEditor.Tools {

    namespace WorldTesters {

        public class World : ITestGenerator {

            public class TestState : ME.ECS.State {
            }

            public int priority => 0;
            public bool IsValid(System.Type type) {
                return typeof(ME.ECS.World) == type;
            }

            public object Fill(ITester tester, object instance, System.Type type) {

                ME.ECS.World world = null;
                ME.ECS.WorldUtilities.CreateWorld<TestState>(ref world, 0.033f);
                {
                    world.SetState<TestState>(ME.ECS.WorldUtilities.CreateState<TestState>());
                    ME.ECS.WorldUtilities.InitComponentTypeId<BufferArrayStructRegistryBase.TestComponent>(false);
                    ME.ECS.ComponentsInitializerWorld.Setup((e) => {
                
                        e.ValidateData<BufferArrayStructRegistryBase.TestComponent>();
                
                    });
                }
                world.SaveResetState<TestState>();
                return world;

            }

        }

        public class BufferArrayStructRegistryBase : ITestGenerator {

            public struct TestComponent : ME.ECS.IStructComponentBase {

                public int data;

            }

            public struct TestDisposableComponent : ME.ECS.IComponentDisposable {

                public int data;

                public void OnDispose() {

                    this.data = default;

                }

            }

            public struct TestCopyableComponent : ME.ECS.IStructCopyable<TestCopyableComponent> {

                public int data;

                public void CopyFrom(in TestCopyableComponent other) {

                    this.data = other.data;

                }

                public void OnRecycle() {
                    
                    this.data = default;
                    
                }

            }

            public int priority => 0;
            
            public bool IsValid(System.Type type) {
                
                return typeof(ME.ECS.Collections.BufferArray<ME.ECS.StructRegistryBase>) == type;
                
            }

            public object Fill(ITester tester, object instance, System.Type type) {

                ME.ECS.AllComponentTypes<TestComponent>.isShared = true;
                ME.ECS.AllComponentTypes<TestCopyableComponent>.isCopyable = true;
                ME.ECS.AllComponentTypes<TestDisposableComponent>.isDisposable = true;
                
                var arr = new [] {
                    CreateDefault(),
                    CreateCopyable(),
                    CreateDisposable(),
                };
                return new ME.ECS.Collections.BufferArray<ME.ECS.StructRegistryBase>(arr, arr.Length);
                
            }

            private ME.ECS.StructRegistryBase CreateDisposable() {
                
                var components = new TestDisposableComponent[3] {
                    new TestDisposableComponent() { data = 1 },
                    new TestDisposableComponent() { data = 2 },
                    new TestDisposableComponent() { data = 3 }, 
                };
                var componentsStates = new byte[] {
                    1, 1, 1,
                };
                var baseComponentsReg = new ME.ECS.StructComponentsDisposable<TestDisposableComponent>() {
                    components = new ME.ECS.Collections.BufferArraySliced<TestDisposableComponent>(new ME.ECS.Collections.BufferArray<TestDisposableComponent>(components, components.Length)),
                    componentsStates = new ME.ECS.Collections.BufferArray<byte>(componentsStates, componentsStates.Length),
                };

                return baseComponentsReg;

            }

            private ME.ECS.StructRegistryBase CreateCopyable() {
                
                var components = new TestCopyableComponent[3] {
                    new TestCopyableComponent() { data = 1 },
                    new TestCopyableComponent() { data = 2 },
                    new TestCopyableComponent() { data = 3 }, 
                };
                var componentsStates = new byte[] {
                    1, 1, 1,
                };
                var baseComponentsReg = new ME.ECS.StructComponentsCopyable<TestCopyableComponent>() {
                    components = new ME.ECS.Collections.BufferArraySliced<TestCopyableComponent>(new ME.ECS.Collections.BufferArray<TestCopyableComponent>(components, components.Length)),
                    componentsStates = new ME.ECS.Collections.BufferArray<byte>(componentsStates, componentsStates.Length),
                };

                return baseComponentsReg;

            }

            private ME.ECS.StructRegistryBase CreateDefault() {
                
                var components = new TestComponent[3] {
                    new TestComponent() { data = 1 },
                    new TestComponent() { data = 2 },
                    new TestComponent() { data = 3 }, 
                };
                var componentsStates = new byte[] {
                    1, 1, 1,
                };

                var shared = new ME.ECS.Collections.DictionaryCopyable<uint, ME.ECS.StructComponents<TestComponent>.SharedGroupData>();
                shared.Add(10, new ME.ECS.StructComponents<TestComponent>.SharedGroupData() {
                    data = new TestComponent() { data = 4, },
                    states = new ME.ECS.Collections.BufferArray<bool>(new bool[] { true, true, false }, 3),
                });
                
                var baseComponentsReg = new ME.ECS.StructComponents<TestComponent>() {
                    components = new ME.ECS.Collections.BufferArraySliced<TestComponent>(new ME.ECS.Collections.BufferArray<TestComponent>(components, components.Length)),
                    componentsStates = new ME.ECS.Collections.BufferArray<byte>(componentsStates, componentsStates.Length),
                    sharedGroups = new ME.ECS.StructComponents<TestComponent>.SharedGroups() {
                        sharedGroups = shared,
                    },
                };

                return baseComponentsReg;

            }

        }
        
        public class CCListTask : ITestGenerator {

            internal class TestTask : ME.ECS.StructComponentsContainer.ITask {

                public ME.ECS.Entity entity;
                public string data;
                public ME.ECS.ComponentLifetime lifetime;
                public float secondsLifetime;

                public ME.ECS.Entity GetEntity() {
                    throw new System.NotImplementedException();
                }

                public void Execute() {
                    throw new System.NotImplementedException();
                }

                public void Recycle() {
                    
                    this.entity = default;
                    this.data = default;
                    this.lifetime = default;
                    this.secondsLifetime = default;
                    
                }

                public ME.ECS.StructComponentsContainer.ITask Clone() {

                    var data = new TestTask();
                    data.CopyFrom(this);
                    return data;

                }

                public void CopyFrom(ME.ECS.StructComponentsContainer.ITask other) {

                    var task = (TestTask)other;
                    this.entity = task.entity;
                    this.data = task.data;
                    this.lifetime = task.lifetime;
                    this.secondsLifetime = task.secondsLifetime;

                }

            }

            public int priority => 0;
            public bool IsValid(System.Type type) {

                return typeof(ME.ECS.Collections.CCList<ME.ECS.StructComponentsContainer.ITask>) == type;

            }

            public object Fill(ITester tester, object instance, System.Type type) {

                var list = new ME.ECS.Collections.CCList<ME.ECS.StructComponentsContainer.ITask>();
                var task = new TestTask();
                tester.FillRandom(task);
                for (int i = 0; i < 16; ++i) {
                    list.Add(task);
                }

                return list;

            }

        }
        
    }

}