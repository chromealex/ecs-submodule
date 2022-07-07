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

            public struct TestComponent : ME.ECS.IComponentBase {

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

                #if !SHARED_COMPONENTS_DISABLED
                ME.ECS.AllComponentTypes<TestComponent>.isShared = true;
                #endif
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
                
                var components = new Component<TestDisposableComponent>[3] {
                    new Component<TestDisposableComponent>() { data = new TestDisposableComponent() { data = 1 }, state = 1 },
                    new Component<TestDisposableComponent>() { data = new TestDisposableComponent() { data = 2 }, state = 1 },
                    new Component<TestDisposableComponent>() { data = new TestDisposableComponent() { data = 3 }, state = 1 }, 
                };
                var baseComponentsReg = new ME.ECS.StructComponentsDisposable<TestDisposableComponent>() {
                    components = new ME.ECS.Collections.BufferArraySliced<Component<TestDisposableComponent>>(new ME.ECS.Collections.BufferArray<Component<TestDisposableComponent>>(components, components.Length)),
                };

                return baseComponentsReg;

            }

            private ME.ECS.StructRegistryBase CreateCopyable() {
                
                var components = new Component<TestCopyableComponent>[3] {
                    new Component<TestCopyableComponent>() { data = new TestCopyableComponent() { data = 1 }, state = 1 },
                    new Component<TestCopyableComponent>() { data = new TestCopyableComponent() { data = 2 }, state = 1 },
                    new Component<TestCopyableComponent>() { data = new TestCopyableComponent() { data = 3 }, state = 1 },
                };
                var baseComponentsReg = new ME.ECS.StructComponentsCopyable<TestCopyableComponent>() {
                    components = new ME.ECS.Collections.BufferArraySliced<Component<TestCopyableComponent>>(new ME.ECS.Collections.BufferArray<Component<TestCopyableComponent>>(components, components.Length)),
                };

                return baseComponentsReg;

            }

            private ME.ECS.StructRegistryBase CreateDefault() {
                
                var components = new Component<TestComponent>[3] {
                    new Component<TestComponent>() { data = new TestComponent() { data = 1 }, state = 1 },
                    new Component<TestComponent>() { data = new TestComponent() { data = 2 }, state = 1 },
                    new Component<TestComponent>() { data = new TestComponent() { data = 3 }, state = 1 },
                };

                #if !SHARED_COMPONENTS_DISABLED
                var shared = new ME.ECS.Collections.DictionaryCopyable<uint, SharedDataStorage<TestComponent>>();
                shared.Add(10, new SharedDataStorage<TestComponent>() {
                    data = new TestComponent() { data = 4, },
                    states = new ME.ECS.Collections.BufferArray<bool>(new bool[] { true, true, false }, 3),
                });
                #endif
                
                var baseComponentsReg = new ME.ECS.StructComponents<TestComponent>() {
                    components = new ME.ECS.Collections.BufferArraySliced<Component<TestComponent>>(new ME.ECS.Collections.BufferArray<Component<TestComponent>>(components, components.Length)),
                    #if !SHARED_COMPONENTS_DISABLED
                    sharedStorage = new SharedDataStorageGroup<TestComponent>() {
                        sharedGroups = shared,
                    },
                    #endif
                };

                return baseComponentsReg;

            }

        }
        
    }

}