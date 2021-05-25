
namespace ME.ECSBurst {
    
    using Collections;
    using Unity.Collections.LowLevel.Unsafe;

    public struct Worlds {

        public static readonly Unity.Burst.SharedStatic<World> currentWorld = Unity.Burst.SharedStatic<World>.GetOrCreate<Worlds, Locker>();

        public class Locker { }

    }

    public unsafe struct World {

        public class Systems {

            public ISystem[] allSystems;
            public IOnDispose[] disposable;
            public IAdvanceTick[] advanceTick;
            public IUpdateInput[] updateInput;
            public IUpdateVisual[] updateVisual;

            public void Dispose() {

                if (this.disposable != null) {

                    for (int i = 0, cnt = this.disposable.Length; i < cnt; ++i) {

                        this.disposable[i].OnDispose();

                    }

                }

            }
            
            public void Add<T>(T system) where T : struct, ISystem {

                this.Add(system, ref this.allSystems);
                
                if (system is IOnDispose sysDispose) {

                    this.Add(sysDispose, ref this.disposable);

                }

                if (system is IAdvanceTick sysLogic) {

                    this.Add(sysLogic, ref this.advanceTick);

                }

                if (system is IUpdateInput sysInput) {

                    this.Add(sysInput, ref this.updateInput);

                }

                if (system is IUpdateVisual sysVisual) {

                    this.Add(sysVisual, ref this.updateVisual);

                }

            }

            private void Add<T, TI>(T system, ref TI[] arr) {

                if (arr == null) {
                    arr = new TI[1];
                } else {
                    System.Array.Resize(ref arr, arr.Length + 1);
                }

                arr[arr.Length - 1] = (TI)(object)system;

            }

            public void Run(float dt) {

                if (this.updateInput != null) {

                    for (int i = 0, cnt = this.updateInput.Length; i < cnt; ++i) {

                        this.updateInput[i].UpdateInput(dt);

                    }

                }

                if (this.advanceTick != null) {

                    for (int i = 0, cnt = this.advanceTick.Length; i < cnt; ++i) {

                        this.advanceTick[i].AdvanceTick(dt);

                    }

                }

                if (this.updateVisual != null) {

                    for (int i = 0, cnt = this.updateVisual.Length; i < cnt; ++i) {

                        this.updateVisual[i].UpdateVisual(dt);

                    }

                }

            }

        }

        [NativeSetClassTypeToNullOnSchedule]
        public string name;
        public State resetState;
        public State currentState;
        [NativeSetClassTypeToNullOnSchedule]
        public Systems systems;

        public World(string name, int entitiesCapacity = 100) {

            this.name = name;

            this.resetState = new State();
            this.resetState.Initialize(entitiesCapacity);

            this.currentState = new State();
            this.currentState.Initialize(entitiesCapacity);

            this.systems = new Systems();
            
            Worlds.currentWorld.Data = this;

        }

        public void Dispose() {

            this.name = default;
            this.resetState.Dispose();
            this.currentState.Dispose();
            this.systems.Dispose();

        }
        
        public void Update(float deltaTime) {

            Worlds.currentWorld.Data = this;
            this.systems.Run(deltaTime);

        }
        
        public void AddSystem<T>(T system) where T : struct, ISystem {

            this.systems.Add(system);

            if (system is IOnCreate onCreate) {

                onCreate.OnCreate();

            }

        }

    }

    public interface ISystem {

        

    }

    public interface IOnCreate : ISystem {

        void OnCreate();

    }
    
    public interface IOnDispose : ISystem {

        void OnDispose();

    }
    
    public interface IAdvanceTick : ISystem {

        void AdvanceTick(float deltaTime);

    }

    public interface IUpdateInput : ISystem {

        void UpdateInput(float deltaTime);

    }

    public interface IUpdateVisual : ISystem {

        void UpdateVisual(float deltaTime);

    }

}
