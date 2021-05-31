
using Unity.Jobs;

namespace ME.ECSBurst {
    
    using Collections;
    using Unity.Collections;
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Burst;
    using static MemUtilsCuts;

    public abstract unsafe class Worlds {

        public static ref World currentWorld => ref mref<World>((void*)Worlds.currentInternalWorld.Data);

        internal static readonly Unity.Burst.SharedStatic<System.IntPtr> currentInternalWorld = Unity.Burst.SharedStatic<System.IntPtr>.GetOrCreate<Worlds, WorldsKey>();

        public class WorldsKey {}

        public static readonly Unity.Burst.SharedStatic<TimeData> time = Unity.Burst.SharedStatic<TimeData>.GetOrCreate<Worlds, TimeDataKey>();

        public class TimeDataKey {}

    }

    public struct TimeData {

        public float deltaTime;

    }

    public unsafe partial struct World {

        public void Validate<T>() where T : struct, IComponentBase {

            this.currentState->Validate<T>();

        }

        public void Set<T>(in Entity entity, T component) where T : struct, IComponentBase {

            this.currentState->Set(in entity, component);

        }

        public bool Remove<T>(in Entity entity) where T : struct, IComponentBase {

            return this.currentState->Remove<T>(in entity);

        }

        public bool Has<T>(in Entity entity) where T : struct, IComponentBase {

            return this.currentState->Has<T>(in entity);

        }
        
        public ref T Get<T>(in Entity entity) where T : struct, IComponentBase {

            return ref this.currentState->Get<T>(in entity);

        }

        public ref readonly T Read<T>(in Entity entity) where T : struct, IComponentBase {

            return ref this.currentState->Read<T>(in entity);

        }

        public ref readonly Entity AddEntity() {

            return ref this.currentState->AddEntity();

        }

        public bool RemoveEntity(in Entity entity) {

            return this.currentState->RemoveEntity(in entity);

        }

        public bool IsAlive(in Entity entity) {

            return this.currentState->IsAlive(in entity);

        }

    }

    public unsafe partial struct World {

        public struct Info {

            public Unity.Collections.FixedString32 name;

        }

        public struct Systems {

            internal delegate void SystemExecute();

            [BurstCompile(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
            internal struct Job : IJob {

                [NativeDisableUnsafePtrRestriction]
                public void* system;
                public FunctionPointer<FunctionPointerDelegate> func;

                public void Execute() {
                    
                    this.func.Invoke(ref this.system);
                    
                }

            }

            internal struct SystemJobData {

                [NativeDisableUnsafePtrRestriction]
                public void* system;
                [NativeDisableUnsafePtrRestriction]
                public void* job;
                [NativeDisableUnsafePtrRestriction]
                public void* jobData;

            }

            internal struct SystemData {

                [NativeDisableUnsafePtrRestriction]
                public void* system;
                [NativeDisableUnsafePtrRestriction]
                public void* method;

            }

            internal Unity.Collections.NativeArray<SystemData> allSystems;
            internal Unity.Collections.NativeArray<SystemData> disposable;
            internal Unity.Collections.NativeArray<SystemJobData> advanceTick;
            internal Unity.Collections.NativeArray<SystemData> updateInput;
            internal Unity.Collections.NativeArray<SystemData> updateVisual;

            public void Initialize() {
                
                this.allSystems = new NativeArray<SystemData>(0, Allocator.Persistent);
                this.disposable = new NativeArray<SystemData>(0, Allocator.Persistent);
                this.advanceTick = new NativeArray<SystemJobData>(0, Allocator.Persistent);
                this.updateInput = new NativeArray<SystemData>(0, Allocator.Persistent);
                this.updateVisual = new NativeArray<SystemData>(0, Allocator.Persistent);
                
            }

            private ref SystemData Add<T>(T system, ref NativeArray<SystemData> arr) where T : struct {

                var size = UnsafeUtility.SizeOf<T>();
                var addr = UnsafeUtility.AddressOf(ref system);
                var buffer = pnew(system);
                UnsafeUtility.MemCpy(buffer, addr, size);

                var sysData = new SystemData();
                sysData.system = buffer;
                
                var idx = arr.Length;
                ArrayUtils.Resize(idx, ref arr);
                arr[idx] = sysData;
                
                return ref arr.GetRef(idx);

            }

            private ref SystemJobData Add(void* system, ref NativeArray<SystemJobData> arr) {

                var sysData = new SystemJobData();
                sysData.system = system;
                
                var idx = arr.Length;
                ArrayUtils.Resize(idx, ref arr);
                arr[idx] = sysData;
                
                return ref arr.GetRef(idx);

            }

            private ref SystemData Add(void* system, ref NativeArray<SystemData> arr) {

                var sysData = new SystemData();
                sysData.system = system;
                
                var idx = arr.Length;
                ArrayUtils.Resize(idx, ref arr);
                arr[idx] = sysData;
                
                return ref arr.GetRef(idx);

            }

            private void RunAllMethods(ref NativeArray<SystemData> arr) {
                
                for (int i = 0, cnt = arr.Length; i < cnt; ++i) {

                    System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<SystemExecute>((System.IntPtr)arr[i].method).Invoke();

                }

            }

            #region Public API
            public void Dispose() {

                if (this.disposable.IsCreated == true) {

                    this.RunAllMethods(ref this.disposable);

                }
                
                if (this.allSystems.IsCreated == true) {

                    for (int i = 0, cnt = this.advanceTick.Length; i < cnt; ++i) {

                        ref var data = ref this.advanceTick.GetRef(i);
                        free(ref data.job);
                        free(ref data.jobData);

                    }

                    for (int i = 0, cnt = this.allSystems.Length; i < cnt; ++i) {

                        free(ref this.allSystems.GetRef(i).system);

                    }

                }

                this.allSystems.Dispose();
                this.disposable.Dispose();
                this.advanceTick.Dispose();
                this.updateVisual.Dispose();
                this.updateInput.Dispose();

            }
            
            public void AddAdvanceTick<T>(T system) where T : struct, ISystem, IAdvanceTick {

                ref var mainData = ref this.Add(system, ref this.allSystems);
                
                ref var sysData = ref this.Add(mainData.system, ref this.advanceTick);
                var job = new Job() {
                    func = Burst<T>.cache,
                    system = sysData.system,
                };
                var ptr = pnew(job);
                var handle = Unity.Jobs.LowLevel.Unsafe.JobsUtility.CreateJobReflectionData(typeof(Job), (SystemExecute)job.Execute);
                sysData.job = ptr;
                sysData.jobData = (void*)handle;
                
            }

            public void AddVisual<T>(T system) where T : struct, ISystem, IUpdateVisual {

                ref var mainData = ref this.Add(system, ref this.allSystems);
                
                ref var sysData = ref this.Add(mainData.system, ref this.updateVisual);
                sysData.method = (void*)System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate((SystemExecute)system.UpdateVisual);
                
            }

            public void AddInput<T>(T system) where T : struct, ISystem, IUpdateInput {

                ref var mainData = ref this.Add(system, ref this.allSystems);
                
                ref var sysData = ref this.Add(mainData.system, ref this.updateVisual);
                sysData.method = (void*)System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate((SystemExecute)system.UpdateInput);
                
            }

            public void Run(void* world, float dt) {

                if (this.updateInput.IsCreated == true) {

                    Worlds.time.Data.deltaTime = dt;
                    this.RunAllMethods(ref this.updateInput);
                    
                }
                
                if (this.advanceTick.IsCreated == true) {

                    Worlds.time.Data.deltaTime = 0.033f;
                    for (int i = 0, cnt = this.advanceTick.Length; i < cnt; ++i) {

                        var data = this.advanceTick[i];
                        var p = new Unity.Jobs.LowLevel.Unsafe.JobsUtility.JobScheduleParameters(data.job, (System.IntPtr)data.jobData, default, Unity.Jobs.LowLevel.Unsafe.ScheduleMode.Run);
                        var handle = Unity.Jobs.LowLevel.Unsafe.JobsUtility.Schedule(ref p);
                        handle.Complete();

                    }

                }
                
                if (this.updateVisual.IsCreated == true) {

                    Worlds.time.Data.deltaTime = dt;
                    this.RunAllMethods(ref this.updateVisual);

                }

            }
            #endregion

        }

        [NativeDisableUnsafePtrRestriction]
        public State* resetState;
        [NativeDisableUnsafePtrRestriction]
        public State* currentState;
        
        public Info info;
        internal Systems systems;
        
        // Current world link
        [NativeDisableUnsafePtrRestriction]
        internal void* buffer;

        public World(string name, int entitiesCapacity = 100) {

            this.resetState = State.Create(entitiesCapacity);
            this.currentState = State.Create(entitiesCapacity);
            
            this.info = new Info() { name = name };
            this.systems = new Systems();
            this.systems.Initialize();

            this.buffer = null;
            this.buffer = pnew(this);
            
            Worlds.currentInternalWorld.Data = (System.IntPtr)this.buffer;

        }
        
        #region Public API
        public void Dispose() {

            this.resetState->Dispose();
            this.currentState->Dispose();
            this.systems.Dispose();
            free(ref this.buffer);

        }
        
        public void Update(float deltaTime) {

            Worlds.currentInternalWorld.Data = (System.IntPtr)this.buffer;
            this.systems.Run(this.buffer, deltaTime);

        }
        
        public void AddSystemAdvanceTick<T>(T system) where T : struct, ISystem, IAdvanceTick {

            this.systems.AddAdvanceTick(system);

            if (system is IOnCreate onCreate) {

                onCreate.OnCreate();

            }

        }
        
        public void AddSystemVisual<T>(T system) where T : struct, ISystem, IUpdateVisual {

            this.systems.AddVisual(system);

            if (system is IOnCreate onCreate) {

                onCreate.OnCreate();

            }

        }
        
        public void AddSystemInput<T>(T system) where T : struct, ISystem, IUpdateInput {

            this.systems.AddInput(system);

            if (system is IOnCreate onCreate) {

                onCreate.OnCreate();

            }

        }
        #endregion

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

        void AdvanceTick();

    }

    public interface IUpdateInput : ISystem {

        void UpdateInput();

    }

    public interface IUpdateVisual : ISystem {

        void UpdateVisual();

    }

}
