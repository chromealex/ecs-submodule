#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if WORLD_TICK_THREADED
#define TICK_THREADED
#endif
#if UNITY_EDITOR || DEVELOPMENT_BUILD
#define CHECKPOINT_COLLECTOR
#endif
#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif
using Unity.Jobs;

namespace ME.ECS {

    using ME.ECS.Collections;
    using Collections.V3;
    using Collections.MemoryAllocator;

    public enum WorldCallbackStep {

        None = 0,
        LogicTick,
        VisualTick,
        UpdateVisualPreStageEnd,

    }

    [System.Flags]
    public enum WorldStep : byte {

        None = 0x0,

        // Default types
        Modules = 0x1,
        Systems = 0x2,
        Plugins = 0x4,

        // Tick type
        LogicTick = 0x8,
        VisualTick = 0x10,
        Simulate = 0x20,

        // Fast links
        ModulesVisualTick = WorldStep.Modules | WorldStep.VisualTick,
        SystemsVisualTick = WorldStep.Systems | WorldStep.VisualTick,
        ModulesLogicTick = WorldStep.Modules | WorldStep.LogicTick,
        PluginsLogicTick = WorldStep.Plugins | WorldStep.LogicTick,
        SystemsLogicTick = WorldStep.Systems | WorldStep.LogicTick,
        PluginsLogicSimulate = WorldStep.Plugins | WorldStep.Simulate | WorldStep.LogicTick,

    }

    [System.Flags]
    public enum ModuleState : byte {

        AllActive = 0x0,
        VisualInactive = 0x1,
        LogicInactive = 0x2,

    }

    #pragma warning disable
    [System.Serializable]
    public partial struct WorldViewsSettings { }

    public enum FrameFixBehaviour {

        None = 0,
        /// <summary>
        /// This means that if ticks per frame is over than X - exception will be thrown
        /// </summary>
        ExceptionOverTicksPreFrame,
        /// <summary>
        /// This means that if ticks per frame is over than X - simulation will be stopped at this frame and continues at the next frame
        /// </summary>
        AsyncOverTicksPerFrame,
        /// <summary>
        /// This means that if milliseconds per frame is over than X - simulation will be stopped at this frame and continues at the next frame
        /// </summary>
        AsyncOverMillisecondsPerFrame,

    }
    
    [System.Serializable]
    public struct WorldSettings {

        public bool useJobsForSystems;
        public bool useJobsForViews;
        public bool createInstanceForFeatures;
        public bool turnOffViews;
        public FrameFixBehaviour frameFixType;
        public int frameFixValue;

        public WorldViewsSettings viewsSettings;

        public static WorldSettings Default => new WorldSettings() {
            useJobsForSystems = true,
            useJobsForViews = true,
            createInstanceForFeatures = true,
            turnOffViews = false,
            frameFixType = FrameFixBehaviour.None,
            viewsSettings = new WorldViewsSettings(),
        };

    }

    [System.Serializable]
    public partial struct WorldDebugViewsSettings { }

    [System.Serializable]
    public struct WorldDebugSettings {

        public bool createGameObjectsRepresentation;
        public bool collectStatistic;
        public ME.ECS.DebugUtils.StatisticsObject statisticsObject;
        public bool showViewsOnScene;
        public WorldDebugViewsSettings viewsSettings;

        public static WorldDebugSettings Default => new WorldDebugSettings() {
            createGameObjectsRepresentation = false,
            collectStatistic = false,
            showViewsOnScene = false,
            viewsSettings = new WorldDebugViewsSettings(),
        };

    }
    #pragma warning restore

    public interface ICheckpointCollector {

        void Reset();
        void Checkpoint(object interestObj, WorldStep step);

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class WorldBase {

        internal WorldStep currentStep;
        internal System.Collections.Generic.Dictionary<System.Type, IFeatureBase> features;
        internal ListCopyable<IModuleBase> modules;
        internal BufferArray<SystemGroup> systemGroups;
        internal int systemGroupsLength;

        internal ListCopyable<ModuleState> statesModules;

        internal ICheckpointCollector checkpointCollector;

        internal float tickTime;
        internal double timeSinceStart;
        internal float speed;
        public bool isActive;

        public IContext currentSystemContext { get; internal set; }
        public ISystemFilter currentSystemContextFilter { get; internal set; }
        public BufferArray<bool> currentSystemContextFiltersUsed;
        public bool currentSystemContextFiltersUsedAnyChanged;

        internal Tick simulationFromTick;
        internal Tick simulationToTick;

        public WorldSettings settings { get; internal set; }
        public WorldDebugSettings debugSettings { get; internal set; }

        internal System.Threading.Thread worldThread;
        internal System.Threading.Thread mainThread;
        
    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed partial class World : WorldBase, IPoolableSpawn, IPoolableRecycle {

        private const int FEATURES_CAPACITY = 100;
        private const int SYSTEMS_CAPACITY = 100;
        private const int MODULES_CAPACITY = 100;
        private const int ENTITIES_CACHE_CAPACITY = 500;
        public const int WORLDS_CAPACITY = 4;
        private const int FILTERS_CACHE_CAPACITY = 10;
        
        private static int registryWorldId = 0;

        public int id { get; private set; }

        public MemoryAllocator tempAllocator;
        private State resetState;
        private bool hasResetState;
        internal State currentState;
        private uint seed;
        private int cpf; // CPF = Calculations per frame
        internal int entitiesCapacity;
        private bool isLoading;
        private bool isLoaded;
        private float loadingProgress;
        private System.Diagnostics.Stopwatch tickTimeWatcher;
        public bool isPaused { private set; get; }

        void IPoolableSpawn.OnSpawn() {

            Unity.Burst.BurstCompiler.SetExecutionMode(Unity.Burst.BurstExecutionEnvironment.Deterministic);
            
            this.tempAllocator = new MemoryAllocator().Initialize(1024 * 512);

            this.InitializePools();
            ME.WeakRef.Reg(this);

            this.tickTimeWatcher = new System.Diagnostics.Stopwatch();
            
            this.isPaused = false;
            this.speed = 1f;
            this.seed = default;

            this.worldThread = System.Threading.Thread.CurrentThread;
            
            #if UNITY_EDITOR
            try {
                Unity.Jobs.LowLevel.Unsafe.JobsUtility.JobDebuggerEnabled = false;
            } catch (System.Exception) { }
            #endif
            
            this.noStateData.Initialize();

            this.currentSystemContextFiltersUsed = PoolArray<bool>.Spawn(World.FILTERS_CACHE_CAPACITY);
            this.currentSystemContextFiltersUsedAnyChanged = false;

            this.simulationFromTick = default;
            this.simulationToTick = default;

            this.currentState = default;
            this.resetState = default;
            this.hasResetState = false;
            this.currentStep = default;
            this.checkpointCollector = default;
            this.tickTime = default;
            this.timeSinceStart = default;
            this.entitiesCapacity = default;

            this.features = PoolDictionary<System.Type, IFeatureBase>.Spawn(World.FEATURES_CAPACITY);
            this.modules = PoolListCopyable<IModuleBase>.Spawn(World.MODULES_CAPACITY);
            this.statesModules = PoolListCopyable<ModuleState>.Spawn(World.MODULES_CAPACITY);
            this.systemGroups = PoolArray<SystemGroup>.Spawn(World.SYSTEMS_CAPACITY);
            this.systemGroupsLength = 0;

            this.OnSpawnStructComponents();
            this.OnSpawnComponents();
            this.OnSpawnFilters();

            WorldStaticCallbacks.RaiseCallbackInit(this);

            #if UNITY_EDITOR
            this.SetDebugStatisticKey(null);
            #endif
            
            this.isActive = true;

        }

        void IPoolableRecycle.OnRecycle() {

            this.worldThread = null;
            this.mainThread = null;
            this.isActive = false;
            this.speed = 0f;
            this.seed = default;

            this.tickTimeWatcher.Stop();

            WorldStaticCallbacks.RaiseCallbackDispose(this);

            this.noStateData.Dispose();

            var list = PoolListCopyable<Entity>.Spawn(World.ENTITIES_CACHE_CAPACITY);
            if (this.ForEachEntity(list) == true) {

                for (int i = list.Count - 1; i >= 0; --i) {

                    ref var item = ref list[i];
                    if (item.IsAlive() == true) this.RemoveEntity(item, cleanUpHierarchy: false);

                }

            }
            PoolListCopyable<Entity>.Recycle(ref list);
            var state = this.GetState();
            if (state != null) state.storage.ApplyDead(ref state.allocator);

            PoolArray<bool>.Recycle(ref this.currentSystemContextFiltersUsed);
            this.currentSystemContextFiltersUsedAnyChanged = default;

            this.OnRecycleFilters();
            this.OnRecycleComponents();
            this.OnRecycleStructComponents();

            // We don't need to remove features
            /*foreach (var feature in this.features) {
                var instance = feature.Value;
                if (instance != null) ((FeatureBase)instance).DoDeconstruct();
            }*/
            PoolDictionary<System.Type, IFeatureBase>.Recycle(ref this.features);

            for (int i = this.systemGroupsLength - 1; i >= 0; --i) {

                this.systemGroups.arr[i].Deconstruct();

            }

            PoolArray<SystemGroup>.Recycle(ref this.systemGroups);

            if (this.modules != null) {

                for (int i = this.modules.Count - 1; i >= 0; --i) {

                    this.modules[i].OnDeconstruct();
                    PoolModules.Recycle(this.modules[i]);

                }

                PoolListCopyable<IModuleBase>.Recycle(ref this.modules);
                PoolListCopyable<ModuleState>.Recycle(ref this.statesModules);

            }

            this.simulationFromTick = default;
            this.simulationToTick = default;
            this.currentState = default;
            this.resetState = default;
            this.hasResetState = false;
            this.currentStep = default;
            this.checkpointCollector = default;
            this.tickTime = default;
            this.timeSinceStart = default;
            this.entitiesCapacity = default;
            this.isPaused = false;
            this.id = default;

            //PoolInternalBaseNoStackPool.Clear();
            //PoolInternalBase.Clear();

            this.DeInitializePools();
            
            this.tempAllocator.Dispose();

        }

        partial void InitializePools();
        partial void DeInitializePools();

        /// <summary>
        /// Returns last frame CPF
        /// </summary>
        /// <returns></returns>
        public int GetCPF() {

            return this.cpf;

        }

        /// <summary>
        /// Calculates constant operation
        /// Useful for matching servers
        /// </summary>
        /// <returns>string to match players</returns>
        public string GetIEEEFloat() {
            
            var p = new UnityEngine.Vector3(-0.9150986f, 0f, 0.4032301f);
            var t = new UnityEngine.Vector3(0.5726798f, 0f, 0.8197792f);
            var rotationSpeed = 50f;
            var deltaTime = 0.04f;
            var res = UnityEngine.Vector3.RotateTowards(p, t, deltaTime * rotationSpeed, 0f);
            return res.x.ToStringDec() + res.y.ToStringDec() + res.z.ToStringDec();// + " :: " + res.x + " :: " + res.y + " :: " + res.z;
            
        }
        
        #if FIXED_POINT_MATH
        /// <summary>
        /// Calculates constant operation with fp
        /// Useful for matching servers
        /// </summary>
        /// <returns>string to match players</returns>
        public string GetIEEEFloatFixed() {
            
            var p = new ME.ECS.Mathematics.float3(-0.9150986f, 0f, 0.4032301f);
            var t = new ME.ECS.Mathematics.float3(0.5726798f, 0f, 0.8197792f);
            var rotationSpeed = (sfloat)50f;
            var deltaTime = (sfloat)0.04f;
            var res = ME.ECS.VecMath.RotateTowards(p, t, deltaTime * rotationSpeed, 0f);
            return res.x.ToStringDec() + res.y.ToStringDec() + res.z.ToStringDec();// + " :: " + res.x + " :: " + res.y + " :: " + res.z;
            
        }
        #endif

        public float GetLoadingProgress() {

            return this.loadingProgress;

        }
        
        public bool IsLoaded() {

            return this.isLoaded;

        }

        public bool IsLoading() {

            return this.isLoading;

        }

        public struct WorldState {

            public State state;
            public ME.ECS.StatesHistory.HistoryStorage events;
            public Tick tick;

        }

        public byte[] Serialize() {

            var statesHistory = this.GetModule<ME.ECS.StatesHistory.IStatesHistoryModuleBase>();

            var data = new WorldState();
            data.state = statesHistory.GetOldestState();
            data.events = statesHistory.GetHistoryStorage(data.state.tick + Tick.One, this.GetCurrentTick());
            data.tick = this.GetCurrentTick();

            var networkModule = this.GetModule<ME.ECS.Network.INetworkModuleBase>();
            var serializer = networkModule.GetSerializer();
            return serializer.SerializeWorld(data);

        }

        public void Deserialize<TState>(byte[] worldData, System.Collections.Generic.List<byte[]> eventsWhileConnecting, System.Action onCustomLogicUpdate = null) where TState : State, new() {

            var statesHistory = this.GetModule<ME.ECS.StatesHistory.IStatesHistoryModuleBase>();
            statesHistory.Reset();

            var networkModule = this.GetModule<ME.ECS.Network.INetworkModuleBase>();
            var data = networkModule.GetSerializer().DeserializeWorld(worldData);

            this.SetState<TState>(data.state);
            statesHistory.AddEvents(data.events.events);

            statesHistory.BeginAddEvents();
            for (int i = 0; i < eventsWhileConnecting.Count; ++i) {

                var package = networkModule.GetSerializer().Deserialize(eventsWhileConnecting[i]);
                statesHistory.AddEvent(package);

            }

            statesHistory.EndAddEvents();

            // Update logic
            var list = PoolListCopyable<Entity>.Spawn(World.ENTITIES_CACHE_CAPACITY);
            if (this.ForEachEntity(list) == true) {

                for (int i = 0; i < list.Count; ++i) {

                    ref var entity = ref list[i];
                    ComponentsInitializerWorld.Init(in entity);
                    this.CreateEntityPlugins(entity, false);
                    //this.UpdateFiltersOnFilterCreate(entity);

                }

            }

            PoolListCopyable<Entity>.Recycle(ref list);
            
            onCustomLogicUpdate?.Invoke();

            this.SetFromToTicks(data.state.tick, data.tick);
            this.UpdateLogic(0f);

            // Update views
            this.GetModule<ME.ECS.Views.ViewsModule>().SetRequestsAsDirty();

        }

        public void Load(System.Action onComplete) {

            this.isLoading = true;
            this.isLoaded = false;
            this.loadingProgress = 0f;

            var awaitingCount = 0;
            for (int i = 0; i < this.systemGroupsLength; ++i) {

                var group = this.systemGroups.arr[i];
                awaitingCount += (group.runtimeSystem.systemLoadable != null ? group.runtimeSystem.systemLoadable.Count : 0);

            }

            this.LoadSystems(awaitingCount, 0, 0, awaitingCount, onComplete);
            
        }

        private void LoadSystems(int count, int groupsOffset, int sysOffset, int awaitingCount, System.Action onComplete) {

            if (awaitingCount == 0) {
                
                this.isLoading = false;
                this.isLoaded = true;
                onComplete.Invoke();
                return;

            }
            
            for (int i = groupsOffset; i < this.systemGroupsLength; ++i) {

                if (this.isActive == false) {
                    
                    // Break loading at this point - may be we call Unload world
                    return;
                    
                }
                var group = this.systemGroups.arr[i];
                if (group.runtimeSystem.systemLoadable == null) continue;
                for (int j = sysOffset; j < group.runtimeSystem.systemLoadable.Count; ++j) {

                    if (this.isActive == false) {
                    
                        // Break loading at this point - may be we call Unload world
                        return;
                    
                    }
                    
                    var loadableSystem = group.runtimeSystem.systemLoadable[j];
                    if (loadableSystem is ILoadableSync) {

                        var groupId = i;
                        var idx = j + 1;
                        if (idx >= group.runtimeSystem.systemLoadable.Count) {
                            
                            ++groupId;
                            idx = 0;
                            
                        }
                        loadableSystem.Load(() => {
                            
                            this.LoadSystems(count, groupId, idx, awaitingCount - 1, onComplete);
                            
                        });
                        return;

                    } else {

                        loadableSystem.Load(() => {

                            --awaitingCount;
                            this.loadingProgress = 1f - awaitingCount / (float)count;
                            
                            if (awaitingCount == 0) {

                                this.isLoading = false;
                                this.isLoaded = true;
                                onComplete.Invoke();

                            }

                        });

                    }

                }

            }

        }

        internal void UpdateGroup(SystemGroup group) {

            this.systemGroups.arr[group.worldIndex] = group;

        }

        public int AddSystemGroup(ref SystemGroup group) {

            var index = this.systemGroupsLength++;
            ArrayUtils.Resize(index, ref this.systemGroups, resizeWithOffset: false);
            this.systemGroups.arr[index] = group;
            return index;

        }

        public void SetWorldThread(System.Threading.Thread thread) {

            this.worldThread = thread;

        }
        
        public void RecycleResetState<TState>() where TState : State, new() {

            if (this.resetState != this.currentState) WorldUtilities.ReleaseState<TState>(ref this.resetState);

        }

        public void RecycleStates<TState>() where TState : State, new() {

            WorldUtilities.ReleaseState<TState>(ref this.currentState);

        }

        public void SetSettings(WorldSettings settings) {

            this.settings = settings;

        }

        public void SetDebugSettings(WorldDebugSettings settings) {

            this.debugSettings = settings;
            #if UNITY_EDITOR
            this.OnDebugWorldCreated();
            #endif

        }

        public void SetCheckpointCollector(ICheckpointCollector checkpointCollector) {

            this.checkpointCollector = checkpointCollector;

        }

        public void Checkpoint(object interestObj) {

            #if CHECKPOINT_COLLECTOR
            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(interestObj, this.currentStep);
            #endif

        }

        public bool IsSystemActive(ISystemBase system, RuntimeSystemFlag state) {

            for (int i = 0; i < this.systemGroupsLength; ++i) {

                if (this.systemGroups.arr[i].IsSystemActive(system, state) == true) {

                    return true;

                }

            }

            return false;

        }

        public void SetModuleState(IModuleBase module, ModuleState state) {

            var index = this.modules.IndexOf(module);
            if (index >= 0) {

                this.statesModules[index] = state;

            }

        }

        public ModuleState GetModuleState(IModuleBase module) {

            var index = this.modules.IndexOf(module);
            if (index >= 0) {

                return this.statesModules[index];

            }

            return ModuleState.AllActive;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetId(int forcedWorldId = 0) {

            if (forcedWorldId > 0) {

                this.id = forcedWorldId;

            } else {

                this.id = ++World.registryWorldId;

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public float3 GetRandomInSphere(float3 center, tfloat maxRadius) {
        
            E.IS_LOGIC_STEP(this);
            E.IS_WORLD_THREAD();
            
            return this.currentState.randomState.GetRandomInSphere(center, maxRadius);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public float2 GetRandomInCircle(float2 center, tfloat maxRadius) {
        
            E.IS_LOGIC_STEP(this);
            E.IS_WORLD_THREAD();
            
            return this.currentState.randomState.GetRandomInCircle(center, maxRadius);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        /// <summary>
        /// Returns random number in range [from..to)
        /// </summary>
        /// <param name="from">Inclusive</param>
        /// <param name="to">Exclusive</param>
        /// <returns></returns>
        public int GetRandomRange(int from, int to) {
            
            E.IS_LOGIC_STEP(this);
            E.IS_WORLD_THREAD();
            
            return this.currentState.randomState.GetRandomRange(from, to);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        /// <summary>
        /// Returns random number in range [from..to]
        /// </summary>
        /// <param name="from">Inclusive</param>
        /// <param name="to">Inclusive</param>
        /// <returns></returns>
        public tfloat GetRandomRange(tfloat from, tfloat to) {
        
            E.IS_LOGIC_STEP(this);
            E.IS_WORLD_THREAD();
            
            return this.currentState.randomState.GetRandomRange(from, to);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        /// <summary>
        /// Returns random number 0..1
        /// </summary>
        /// <returns></returns>
        public tfloat GetRandomValue() {
            
            E.IS_LOGIC_STEP(this);
            E.IS_WORLD_THREAD();

            return this.currentState.randomState.GetRandomValue();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int GetSeedValue() {

            return this.GetCurrentTick();

        }

        public void SetSeed(uint seed) {
            
            E.IS_LOGIC_STEP(this);
            E.IS_WORLD_THREAD();

            this.seed = seed;
            this.currentState?.randomState.SetSeed(seed);
            //this.resetState?.randomState.SetSeed(this.seed);
            
        }

        public uint GetSeed() {

            return this.seed;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetTickTime(float tickTime) {

            this.tickTime = tickTime;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public float GetTickTime() {

            return this.tickTime;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Tick GetStateTick() {

            return this.GetState().tick;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Tick GetLastSavedTick() {

            var net = this.GetModule<StatesHistory.IStatesHistoryModuleBase>();
            return net.GetLastSavedTick();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public double GetTimeSinceStart() {

            return this.timeSinceStart;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public tfloat GetTime() {
        
            return this.GetTimeFromTick(this.GetStateTick());
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetTimeSinceStart(double time) {

            this.timeSinceStart = time;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public tfloat GetTimeFromTick(Tick tick) {

            return (tfloat)tick * this.tickTime;

        }

        public enum RewindAsyncState {

            CacheBackwardRewind,
            LongBackwardRewind,
            ShortForwardRewind,
            LongForwardRewind,

        }

        public void SetSpeed(float speed) {

            this.speed = speed;

        }

        private RewindAsyncState GetRewindState(Tick targetTick, float maxSimulationTime) {

            var currentTick = this.GetCurrentTick();
            if (currentTick < targetTick) {

                var delta = targetTick - currentTick;
                var duration = (float)(long)delta * this.GetTickTime();
                return duration > maxSimulationTime ? RewindAsyncState.LongForwardRewind : RewindAsyncState.ShortForwardRewind;

            } else {

                var cacheSize = this.statesHistoryModule.GetCacheSize();
                var delta = currentTick - targetTick;
                if (delta <= cacheSize) return RewindAsyncState.CacheBackwardRewind;

            }

            return RewindAsyncState.LongBackwardRewind;

        }
        
        public async System.Threading.Tasks.Task RewindToAsync(Tick tick, bool doVisualUpdate = true, System.Action<RewindAsyncState> onState = null, float maxSimulationTime = 1f) {

            var rewindState = this.GetRewindState(tick, maxSimulationTime);
            onState?.Invoke(rewindState);
            if (rewindState == RewindAsyncState.LongBackwardRewind ||
                rewindState == RewindAsyncState.LongForwardRewind) {

                var isPaused = this.isPaused;
                this.Pause();
                {
                    
                    var currentTick = tick;
                    var prevStateTick = currentTick - currentTick % this.statesHistoryModule.GetTicksPerState();
                    var cacheSize = this.statesHistoryModule.GetCacheSize();
                    this.statesHistoryModule.PauseStoreStateSinceTick(prevStateTick - cacheSize);

                    if (tick <= 0) tick = 1;
                    this.timeSinceStart = (float)((tfloat)tick * this.GetTickTime());
                    this.statesHistoryModule.HardResetTo(tick);

                    this.networkModule.SetAsyncMode(true);
                    this.PreUpdate(0f);
                    await this.SimulateAsync(this.simulationFromTick, this.simulationToTick, maxSimulationTime);
                    this.networkModule.SetAsyncMode(false);
                    if (doVisualUpdate == true) {
                        
                        this.GetModule<ME.ECS.Views.ViewsModule>().SetRequestsAsDirty();
                        this.LateUpdate(0f);
                        
                    }

                    this.statesHistoryModule.ResumeStoreState();
                }
                if (isPaused == false) this.Play();

            } else {
                
                // Sync rewind
                this.RewindTo(tick, doVisualUpdate);
                
            }

        }

        public void RewindTo(Tick tick, bool doVisualUpdate = true) {

            {
                if (tick <= 0) tick = 1;
                this.timeSinceStart = (float)((tfloat)tick * this.GetTickTime());
                
                var prevState = this.statesHistoryModule.GetStateBeforeTick(tick);
                if (prevState == null) prevState = this.GetResetState();

                var sourceTick = prevState.tick;
                var currentState = this.GetState();
                currentState.CopyFrom(prevState);
                currentState.Initialize(this, freeze: false, restore: true);
                this.Simulate(sourceTick, tick);
                this.Refresh(doVisualUpdate);
            }

            /*
            var currentTick = this.GetCurrentTick();
            if (tick >= currentTick) {
                
                var prevStateTick = currentTick - currentTick % this.statesHistoryModule.GetTicksPerState();
                var cacheSize = this.statesHistoryModule.GetCacheSize();
                this.statesHistoryModule.PauseStoreStateSinceTick(prevStateTick - cacheSize);
                
            }

            if (tick <= 0) tick = 1;
            this.timeSinceStart = (float)tick * this.GetTickTime();
            var prevState = this.statesHistoryModule.GetStateBeforeTick(tick);
            if (prevState == null) prevState = this.GetResetState();
            this.currentState.tick = prevState.tick;
            this.statesHistoryModule.HardResetTo(tick);
            this.GetModule<ME.ECS.Views.ViewsModule>().SetRequestsAsDirty();
            this.Refresh(doVisualUpdate);

            this.statesHistoryModule.ResumeStoreState();
            */

        }

        public void Refresh(bool doVisualUpdate = true) {

            var pausedState = this.isPaused;
            this.isPaused = false;
            this.PreUpdate(0f);
            this.Update(0f);
            if (doVisualUpdate == true) this.LateUpdate(0f);
            this.isPaused = pausedState;
            
        }

        public void Stop() {
            
            this.RewindTo(0);
            this.Pause();
            
        }

        public void Pause() {
            
            this.isPaused = true;
            
        }

        public void Play() {
            
            this.isPaused = false;
            
        }

        partial void OnSpawnComponents();
        partial void OnRecycleComponents();

        partial void OnSpawnStructComponents();
        partial void OnRecycleStructComponents();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int GetStateHash() {

            if (this.statesHistoryModule != null) {

                return this.statesHistoryModule.GetStateHash(this.GetState());

            }

            return this.currentState.GetHash();

        }

        #region EntityVersionIncrementActions
        #if ENTITY_VERSION_INCREMENT_ACTIONS
        private event System.Action<Entity, int> onEntityVersionIncrement;

        public void RaiseEntityVersionIncrementAction(in Entity entity, int version) {

            this.onEntityVersionIncrement?.Invoke(entity, version);

        }

        public void RegisterEntityVersionIncrementAction(System.Action<Entity, int> action) {

            this.onEntityVersionIncrement += action;

        }

        public void UnRegisterEntityVersionIncrementAction(System.Action<Entity, int> action) {

            this.onEntityVersionIncrement -= action;

        }
        #endif
        #endregion

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasResetState() {

            return this.hasResetState;

        }

        public void SaveResetState<TState>() where TState : State, new() {

            WorldStaticCallbacks.RaiseCallbackInitResetState(this.GetState());

            if (this.resetState != null) WorldUtilities.ReleaseState<TState>(ref this.resetState);
            this.resetState = WorldUtilities.CreateState<TState>();
            this.resetState.Initialize(this, freeze: true, restore: false);
            this.resetState.CopyFrom(this.GetState());
            this.resetState.tick = Tick.Zero;
            this.resetState.structComponents.Merge(in this.resetState.allocator);
            this.hasResetState = true;

            this.currentState.structComponents.Merge(in this.currentState.allocator);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public State GetResetState() {

            return this.resetState;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public T GetResetState<T>() where T : State {

            return (T)this.resetState;

        }

        internal void TryInitializeDefaults() {
            
            if (this.entitiesOneShotFilter.IsAlive() == false) {
                
                Filter.Create().Any<IsEntityOneShot, IsEntityEmptyOneShot>().Push(ref this.entitiesOneShotFilter);
                
            }

        }

        public void SetState<TState>(State state) where TState : State, new() {

            if (this.currentState != null && this.currentState != state) WorldUtilities.ReleaseState<TState>(ref this.currentState);
            this.currentState = state;
            state.Initialize(this, freeze: false, restore: true);

            if (state.storage.nextEntityId > 0) {
                this.noStateData.storage.SetEntityCapacity(ref this.noStateData.allocator, state.storage.nextEntityId);
                ComponentsInitializerWorld.Init(state.storage.cache[in state.allocator, state.storage.nextEntityId - 1]);
            } else {
                this.noStateData.storage.SetEntityCapacity(ref this.noStateData.allocator, state.storage.AliveCount + state.storage.DeadCount(in this.currentState.allocator));
            }

            this.noStateData.storage.Merge(in this.noStateData.allocator);

        }

        public void SetStateDirect(State state) {

            this.currentState = state;
            state.Initialize(this, freeze: false, restore: false);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public State GetState() {

            return this.currentState;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public T GetState<T>() where T : State {

            return (T)this.currentState;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public WorldStep GetCurrentStep() {

            return this.currentStep;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool HasStep(WorldStep step) {

            return (this.currentStep & step) != 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(in Entity from, in Entity to, bool copyHierarchy = true) {
            
            E.IS_LOGIC_STEP(this);
            E.IS_ALIVE(in from);
            E.IS_ALIVE(in to);

            {
                // Clear entity
                this.currentState.structComponents.RemoveAll(this.currentState, ref this.currentState.allocator, in to);
            }

            {
                // Copy data
                this.currentState.structComponents.CopyFrom(in from, in to);
                this.UpdateFilters(in to);

                WorldStaticCallbacks.RaiseCallbackEntityCopyFrom(this, in from, in to, copyHierarchy);
                
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsAlive(int entityId, ushort generation) {

            // Inline manually
            return this.currentState.storage.cache[in this.currentState.allocator, entityId].generation == generation;
            //return this.currentState.storage.IsAlive(entityId, version);

        }

        public ref readonly Entity GetEntityById(int id) {

            ref var ent = ref this.currentState.storage.GetEntityById(in this.currentState.allocator, id);
            if (this.IsAlive(ent.id, ent.generation) == false) return ref Entity.Empty;

            return ref ent;

        }

        public void SetEntitiesCapacity(int capacity) {

            var curCap = this.entitiesCapacity + this.currentState.storage.AliveCount;
            
            this.entitiesCapacity = capacity;
            this.SetEntityCapacityPlugins(capacity);
            this.SetEntityCapacityInFilters(ref this.currentState.allocator, capacity);

            Entity maxEntity = default;
            for (int i = 0; i < capacity - curCap; ++i) {

                var e = this.AddEntity_INTERNAL(validate: false);
                this.RemoveEntity(in e, false);
                if (e.id > maxEntity.id) maxEntity = e;

            }

            if (maxEntity.id > 0) {
                this.UpdateEntityOnCreate(maxEntity, isNew: true);
            }

            this.currentState.storage.ApplyDead(ref this.currentState.allocator);

        }

        public ref Entity AddEntity(string name = null, EntityFlag flags = EntityFlag.None) {

            return ref this.AddEntity_INTERNAL(name, flags: flags);

        }
        
        private ref Entity AddEntity_INTERNAL(string name = null, bool validate = true, EntityFlag flags = EntityFlag.None) {
            
            E.IS_LOGIC_STEP(this);
            E.IS_WORLD_THREAD();
            
            var isNew = (validate == true && this.currentState.storage.WillNew(in this.currentState.allocator));
            ref var entity = ref this.currentState.storage.Alloc(ref this.currentState.allocator);
            if (validate == true) this.UpdateEntityOnCreate(in entity, isNew);
            
            if (name != null) {

                entity.Set(new ME.ECS.Name.Name() {
                    value = name,
                });

            }

            if ((flags & EntityFlag.OneShot) != 0) {

                entity.SetOneShot<IsEntityOneShot>();

            }

            this.currentState.storage.flags.Set(in this.currentState.allocator, entity.id, flags);

            return ref entity;

        }

        public int GetEntitiesCount() {

            return this.currentState.storage.AliveCount;

        }

        internal void UpdateEntityOnCreate(in Entity entity, bool isNew) {

            if (isNew == true) {
                ComponentsInitializerWorld.Init(in entity);
                this.currentState.storage.versions.Validate(ref this.currentState.allocator, in entity);
                this.CreateEntityPlugins(entity, true);
                this.CreateEntityInFilters(ref this.currentState.allocator, entity);
            } else {
                this.CreateEntityPlugins(entity, false);
            }

        }

        public bool ForEachEntity(ListCopyable<Entity> results) {

            if (this.currentState == null) return false;
            return this.currentState.storage.ForEach(in this.currentState.allocator, results);

        }

        public List<int> GetAliveEntities() {

            if (this.currentState == null) return default;
            return this.currentState.storage.GetAlive();

        }

        public bool RemoveEntity(in Entity entity, bool cleanUpHierarchy = true) {

            E.IS_ALIVE(in entity);

            if (this.currentState.storage.Dealloc(ref this.currentState.allocator, in entity) == true) {

                this.RemoveFromAllFilters(ref this.currentState.allocator, entity);
                this.DestroyEntityPlugins(in entity);

                WorldStaticCallbacks.RaiseCallbackEntityDestroy(this.currentState, in entity, cleanUpHierarchy);
                
                this.currentState.storage.IncrementGeneration(in this.currentState.allocator, in entity);

                return true;

            } else {

                UnityEngine.Debug.LogError("Failed to remove entity " + entity);

            }

            return false;

        }

        /// <summary>
        /// Get first module by type
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        public TModule GetModule<TModule>() where TModule : IModuleBase {

            if (this.modules == null) return default;

            for (int i = 0, count = this.modules.Count; i < count; ++i) {

                var module = this.modules[i];
                if (module is TModule tModule) {

                    return tModule;

                }

            }

            return default;

        }

        public System.Collections.Generic.List<TModule> GetModules<TModule>(System.Collections.Generic.List<TModule> output) where TModule : IModuleBase {

            output.Clear();
            for (int i = 0, count = this.modules.Count; i < count; ++i) {

                var module = this.modules[i];
                if (module is TModule tModule) {

                    output.Add(tModule);

                }

            }

            return output;

        }

        public bool HasModule<TModule>() where TModule : class, IModuleBase {

            for (int i = 0, count = this.modules.Count; i < count; ++i) {

                var module = this.modules[i];
                if (module is TModule) {

                    return true;

                }

            }

            return false;

        }

        /// <summary>
        /// Add module by type
        /// Retrieve module from pool, OnConstruct() call
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        public bool AddModule<TModule>() where TModule : class, IModuleBase, new() {

            WorldUtilities.SetWorld(this);

            var instance = PoolModules.Spawn<TModule>();
            ME.WeakRef.Reg(instance);
            instance.world = this;
            if (instance is IModuleValidation instanceValidate) {

                if (instanceValidate.CanBeAdded() == false) {

                    instance.world = null;
                    PoolModules.Recycle(ref instance);
                    //throw new System.Exception("Couldn't add new module `" + instanceValidate + "`(" + nameof(TModule) + ") because of CouldBeAdded() returns false.");
                    return false;

                }

            }

            this.modules.Add(instance);
            this.statesModules.Add(ModuleState.AllActive);
            instance.OnConstruct();

            return true;

        }

        /// <summary>
        /// Remove modules by type
        /// Return modules into pool, OnDeconstruct() call
        /// </summary>
        public void RemoveModules<TModule>() where TModule : class, IModuleBase, new() {

            for (int i = 0, count = this.modules.Count; i < count; ++i) {

                var module = this.modules[i];
                if (module is TModule tModule) {

                    PoolModules.Recycle(tModule);
                    this.modules.RemoveAt(i);
                    this.statesModules.RemoveAt(i);
                    module.OnDeconstruct();
                    --i;
                    --count;

                }

            }

        }

        public bool HasFeature<TFeature>() where TFeature : class, IFeatureBase, new() {

            return this.features.ContainsKey(typeof(TFeature));
            
        }

        public void GetFeature<TFeature>(out TFeature feature) where TFeature : IFeatureBase {

            feature = this.GetFeature<TFeature>();

        }

        public TFeature GetFeature<TFeature>() where TFeature : IFeatureBase {

            {
                if (this.features.TryGetValue(typeof(TFeature), out var feature) == true) {

                    return (TFeature)feature;

                }
            }

            {
                foreach (var feature in this.features) {
                    if (feature.Value is TFeature featureBase) return featureBase;
                }
            }

            return default;

        }

        /// <summary>
        /// Add feature manually
        /// Pool will not be used, OnConstruct() call
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="parameters"></param>
        public bool AddFeature(IFeatureBase instance, bool doConstruct = true) {

            WorldUtilities.SetWorld(this);

            instance.world = this;
            if (instance is IFeatureValidation instanceValidate) {

                if (instanceValidate.CanBeAdded() == false) {

                    instance.world = null;
                    return false;

                }

            }
            
            ME.WeakRef.Reg(instance);
            this.features.Add(instance.GetType(), instance);
            if (doConstruct == true) {
                ((FeatureBase)instance).DoConstruct();
                ((FeatureBase)instance).DoConstructLate();
            }

            return true;

        }

        /// <summary>
        /// Remove feature manually
        /// Pool will not be used, OnDeconstruct() call
        /// </summary>
        /// <param name="instance"></param>
        public void RemoveFeature(IFeatureBase instance) {

            if (this.isActive == false) return;

            if (this.features.Remove(instance.GetType()) == true) {
                
                ((FeatureBase)instance).DoDeconstruct();

            }

        }

        #region Systems
        /// <summary>
        /// Search TSystem in all groups
        /// </summary>
        /// <typeparam name="TSystem"></typeparam>
        /// <returns></returns>
        public bool HasSystem<TSystem>() where TSystem : class, ISystemBase, new() {

            for (int i = 0; i < this.systemGroupsLength; ++i) {

                if (this.systemGroups.arr[i].HasSystem<TSystem>() == true) {

                    return true;

                }

            }

            return false;

        }

        /// <summary>
        /// Get first system by type
        /// </summary>
        /// <typeparam name="TSystem"></typeparam>
        /// <returns></returns>
        public TSystem GetSystem<TSystem>() where TSystem : class, ISystemBase {

            for (int i = 0, count = this.systemGroupsLength; i < count; ++i) {

                var system = this.systemGroups.arr[i].GetSystem<TSystem>();
                if (system != null) return system;

            }

            return default;

        }
        #endregion

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool IsModuleActive(int index) {

            var step = this.currentStep;
            if ((step & WorldStep.LogicTick) != 0) {

                return (this.statesModules[index] & ModuleState.LogicInactive) == 0;

            } else if ((step & WorldStep.VisualTick) != 0) {

                return (this.statesModules[index] & ModuleState.VisualInactive) == 0;

            }

            return false;

        }

        public MemArrayAllocator<Entity> GetEntityStorage() {

            return this.currentState.storage.cache;

        }

        public bool IsReverting() {

            var module = this.GetModule<ME.ECS.Network.INetworkModuleBase>();
            if (module == null) return false;
            
            return module.IsReverting();

        }

        public bool IsReverting(out Tick targetTick) {

            targetTick = Tick.Invalid;
            
            var module = this.GetModule<ME.ECS.Network.INetworkModuleBase>();
            if (module == null) return false;

            targetTick = module.GetRevertingTargetTick();
            return module.IsReverting();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdatePhysics(float deltaTime) {

            for (int i = 0, count = this.modules.Count; i < count; ++i) {

                if (this.IsModuleActive(i) == true) {

                    if (this.modules[i] is IModulePhysicsUpdate module) {

                        module.UpdatePhysics(deltaTime);

                    }

                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateLogic(float deltaTime) {

            if (deltaTime < 0f) return;
            
            this.TryInitializeDefaults();
            
            ////////////////
            // Update Logic Tick
            ////////////////
            #if CHECKPOINT_COLLECTOR
            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("Simulate", WorldStep.None);
            #endif

            ECSProfiler.SampleWorld(this);

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample($"Simulate");
            #endif

            this.Simulate(this.simulationFromTick, this.simulationToTick);

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif

            #if CHECKPOINT_COLLECTOR
            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("Simulate", WorldStep.None);
            #endif

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateVisualPre(float deltaTime) {

            if (deltaTime < 0f) return;

            // modules.Update
            this.StepElement<IModuleBase, IUpdate, Tick>(0, this.modules, deltaTime, WorldStep.ModulesVisualTick, (t, module, idx) => module.world.IsModuleActive(idx), (module, dt) => module.Update(dt));
            
            // systems.Update
            this.StepGroup(0, this.systemGroups, deltaTime, WorldStep.SystemsVisualTick, (in SystemGroup group) => group.runtimeSystem.systemUpdates, (t, module, dt) => module.Update(dt));

            // Remove markers
            WorldStaticCallbacks.RaiseCallbackStep(this, WorldCallbackStep.UpdateVisualPreStageEnd);
            
            // modules.UpdateLate
            this.StepElement<IModuleBase, IUpdateLate, Tick>(0, this.modules, deltaTime, WorldStep.ModulesVisualTick, (t, module, idx) => module.world.IsModuleActive(idx), (module, dt) => module.UpdateLate(dt));

            // systems.UpdateLate
            this.StepGroup(0, this.systemGroups, deltaTime, WorldStep.SystemsVisualTick, (in SystemGroup group) => group.runtimeSystem.systemUpdatesLate, (t, module, dt) => module.UpdateLate(dt));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UpdateVisualPost(float deltaTime) {

            if (deltaTime < 0f) return;
            
            #if ENABLE_PROFILER
            ECSProfiler.VisualViews.Value = 0;
            var tickSw = System.Diagnostics.Stopwatch.StartNew();
            #endif

            // modules.UpdatePost
            this.StepElement<IModuleBase, IUpdatePost, Tick>(0, this.modules, deltaTime, WorldStep.ModulesVisualTick, (t, module, idx) => module.world.IsModuleActive(idx), (module, dt) => module.UpdatePost(dt));

            // systems.UpdatePost
            this.StepGroup(0, this.systemGroups, deltaTime, WorldStep.SystemsVisualTick, (in SystemGroup group) => group.runtimeSystem.systemUpdatesPost, (t, module, dt) => module.UpdatePost(dt));

            // Process visual events
            WorldStaticCallbacks.RaiseCallbackStep(this, WorldCallbackStep.VisualTick);

            #if ENABLE_PROFILER
            ECSProfiler.VisualViews.Value += (long)((tickSw.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency) * 1000000000L);
            ECSProfiler.VisualViews.Sample();
            #endif

        }

        #if TICK_THREADED
        private struct UpdateJob : Unity.Jobs.IJobParallelFor {

            public float deltaTime;

            void Unity.Jobs.IJobParallelFor.Execute(int index) {

                Worlds.currentWorld.UpdateLogic(this.deltaTime);

            }

        }
        #endif

        [System.Diagnostics.ConditionalAttribute("UNITY_EDITOR")]
        public void OnDrawGizmos() {

            foreach (var module in this.modules) {

                if (module is IDrawGizmos gizmos) {
                    
                    gizmos.OnDrawGizmos();
                    
                }
                
            }

            foreach (var group in this.systemGroups) {

                if (group.runtimeSystem.allSystems == null) continue;
                foreach (var sys in group.runtimeSystem.allSystems) {

                    if (sys is IDrawGizmos gizmos) {
                        
                        gizmos.OnDrawGizmos();
                        
                    }

                }
                
            }

        }

        public void PreUpdate(float deltaTime) {

            if (deltaTime < 0f) return;

            this.UpdateVisualPre(deltaTime * this.speed);

        }

        public void Update(float deltaTime) {

            if (this.isPaused == true) return;
            if (deltaTime < 0f) return;

            this.mainThread = System.Threading.Thread.CurrentThread;
            
            #if CHECKPOINT_COLLECTOR
            if (this.checkpointCollector != null) this.checkpointCollector.Reset();
            #endif

            // Setup current static variables
            WorldUtilities.SetWorld(this);
            
            deltaTime *= this.speed;

            // Update time
            this.timeSinceStart += deltaTime;
            if (this.timeSinceStart < 0d) this.timeSinceStart = 0d;

            #if TICK_THREADED
            var job = new UpdateJob() {
                deltaTime = deltaTime,
            };
            var jobHandle = job.Schedule(1, 64);
            jobHandle.Complete();
            #else
            this.UpdateLogic(deltaTime);
            #endif

        }

        public void LateUpdate(float deltaTime) {

            deltaTime *= this.speed;

            this.UpdateVisualPost(deltaTime);

        }

        public void SetFromToTicks(Tick from, Tick to) {

            //UnityEngine.Debug.Log("Set FromTo: " + from + " >> " + to);
            this.simulationFromTick = from;
            this.simulationToTick = to;

        }

        private struct ForeachFilterJob : Unity.Jobs.IJobParallelFor {

            [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
            [Unity.Collections.ReadOnlyAttribute]
            public Unity.Collections.NativeArray<Entity> slice;
            [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
            [Unity.Collections.ReadOnlyAttribute]
            public Unity.Collections.NativeArray<byte> dataContains;
            [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
            [Unity.Collections.ReadOnlyAttribute]
            public Unity.Collections.NativeArray<byte> dataVersions;
            public float deltaTime;

            void Unity.Jobs.IJobParallelFor.Execute(int index) {

                var entity = this.slice[index];
                if (entity.IsAlive() == false) return;

                if (this.dataContains.GetRefRead(entity.id) == 1 && (this.dataVersions.IsCreated == false || this.dataVersions.GetRefRead(entity.id) == 1)) {
                    Worlds.currentWorld.currentSystemContextFilter.AdvanceTick(entity, in this.deltaTime);
                }

            }

        }

        /*[Unity.Burst.BurstCompileAttribute]
        private unsafe struct ForeachFilterJobBurst : Unity.Jobs.IJobParallelFor {

            [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestrictionAttribute]
            public void* bws;
            public Unity.Burst.FunctionPointer<SystemFilterAdvanceTick> function;
            public Unity.Collections.NativeArray<Entity> entities;
            public float deltaTime;

            void Unity.Jobs.IJobParallelFor.Execute(int index) {

                this.function.Invoke(this.entities[index], this.deltaTime, this.bws);
                
            }

        }

        public unsafe delegate void SystemFilterAdvanceTick(in Entity entity, in float deltaTime, void* burstWorldStructComponentsAccess);*/

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void PrepareAdvanceTickForSystem<T>(T system) where T : class, IContext {

            this.currentSystemContext = system;
            System.Array.Clear(this.currentSystemContextFiltersUsed.arr, 0, this.currentSystemContextFiltersUsed.Length);
            this.currentSystemContextFiltersUsedAnyChanged = false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void PostAdvanceTickForSystem() {

            this.currentState.storage.ApplyDead(ref this.currentState.allocator);
            this.currentState.structComponents.Merge(in this.currentState.allocator);

            this.currentSystemContext = null;
            this.currentSystemContextFilter = null;

            if (this.currentSystemContextFiltersUsedAnyChanged == true) {

                for (int f = 1, cnt = this.currentSystemContextFiltersUsed.Length; f < cnt; ++f) {

                    if (this.currentSystemContextFiltersUsed.arr[f] == true) {

                        var filter = this.GetFilter(f);
                        filter.ApplyAllRequests();

                    }

                }

            }

        }

        public async System.Threading.Tasks.Task SimulateAsync(Tick from, Tick to, float maxTime) {

            if (from > to) {

                //UnityEngine.Debug.LogError( UnityEngine.Time.frameCount + " From: " + from + ", To: " + to);
                return;

            }

            if (from < Tick.Zero) from = Tick.Zero;

            var state = this.GetState();

            //UnityEngine.Debug.Log("Simulate " + from + " to " + to);
            this.cpf = to - from;
            var maxTickTime = (tfloat)(this.cpf / maxTime);
            if (maxTickTime < this.GetTickTime()) maxTickTime = this.GetTickTime();
            var fixedDeltaTime = this.GetTickTime();
            var sw = PoolClass<System.Diagnostics.Stopwatch>.Spawn();
            sw.Reset();
            sw.Start();
            for (state.tick = from; state.tick < to; ++state.tick) {

                if (sw.ElapsedMilliseconds >= maxTickTime) {
                
                    await System.Threading.Tasks.Task.Yield();
                    sw.Restart();
                    
                }

                this.RunTick(state.tick, fixedDeltaTime);

            }
            PoolClass<System.Diagnostics.Stopwatch>.Recycle(ref sw);

            ////////////////
            this.currentStep |= WorldStep.PluginsLogicSimulate;
            ////////////////
            {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("SimulatePluginsForTicks", WorldStep.None);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"SimulatePluginsForTicks");
                #endif

                this.SimulatePluginsForTicks(from, to);

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("SimulatePluginsForTicks", WorldStep.None);
                #endif

            }
            ////////////////
            this.currentStep &= ~WorldStep.PluginsLogicSimulate;
            ////////////////

        }

        private struct Closure {

            public Tick tick;
            public World world;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void RunTick(Tick tick, float fixedDeltaTime) {

            #if ENABLE_PROFILER
            var tickSw = System.Diagnostics.Stopwatch.StartNew();
            #endif

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("Tick");
            #endif
            
            ////////////////
            this.currentStep |= WorldStep.PluginsLogicTick;
            ////////////////
            {
                
                // Pick random number
                this.GetRandomValue();

                using (new Checkpoint("UseLifetimeStep NotifyAllSystems")) {

                    this.UseLifetimeStep(ComponentLifetime.NotifyAllSystems, fixedDeltaTime);
                    
                }

                try {

                    using (new Checkpoint("PlayPluginsForTickPre", "PlayPluginsForTickPre", WorldStep.None)) {

                        this.PlayPluginsForTickPre(tick);

                    }
                    
                } catch (System.Exception ex) {

                    UnityEngine.Debug.LogException(ex);

                }
                
            }
            ////////////////
            this.currentStep &= ~WorldStep.PluginsLogicTick;
            ////////////////

            // modules.AdvanceTickPre
            this.StepElement<IModuleBase, IAdvanceTickPre, Tick>(tick, this.modules, fixedDeltaTime, WorldStep.ModulesLogicTick, (t, module, idx) => {
                if (module is IAdvanceTickStep step && t % step.step != Tick.Zero) return false;
                return module.world.IsModuleActive(idx);
            }, (system, dt) => system.AdvanceTickPre(dt));

            // systems.AdvanceTickPre
            this.StepGroup(tick, this.systemGroups, fixedDeltaTime, WorldStep.SystemsLogicTick, (in SystemGroup group) => group.runtimeSystem.systemAdvanceTickPre, (t, system, dt) => {
                if (system is IAdvanceTickStep step && t % step.step != Tick.Zero) return;
                system.AdvanceTickPre(dt);
            });

            // modules.AdvanceTick
            this.StepElement<IModuleBase, IAdvanceTick, Tick>(tick, this.modules, fixedDeltaTime, WorldStep.ModulesLogicTick, (t, module, index) => {
                if (module is IAdvanceTickStep step && t % step.step != Tick.Zero) return false;
                return true;
            }, (module, dt) => module.AdvanceTick(dt));

            // systems.AdvanceTick
            var closure = new Closure() {
                tick = tick,
                world = this,
            };
            this.StepGroup(closure, this.systemGroups, fixedDeltaTime, WorldStep.SystemsLogicTick, (in SystemGroup group) => group.runtimeSystem.systemAdvanceTick, (c, systemBase, dt) => {
                if (systemBase is IAdvanceTickStep step && c.tick % step.step != Tick.Zero) return;
                
                if (systemBase is ISystemFilter system) {

                    c.world.currentSystemContextFilter = system;

                    using (new Checkpoint("PrepareAdvanceTickForSystem", system, WorldStep.LogicTick)) {

                        c.world.PrepareAdvanceTickForSystem(system);

                    }

                    using (new Checkpoint(system.GetType().FullName, system, WorldStep.LogicTick)) {

                        system.filter = (system.filter.IsAlive() == true ? system.filter : system.CreateFilter());
                        if (system.filter.IsAlive() == true) {

                            #pragma warning disable
                            var jobs = system.jobs;
                            var batch = system.jobsBatchCount;
                            if (system is ISystemJobs systemJobs) {
                                jobs = true;
                                batch = systemJobs.jobsBatchCount;
                            }
                            #pragma warning restore
                            if (c.world.settings.useJobsForSystems == true && jobs == true) {

                                // TODO: Make a job
                                foreach (var entity in system.filter) {

                                    system.AdvanceTick(in entity, dt);

                                }
                                
                            } else {

                                foreach (var entity in system.filter) {

                                    system.AdvanceTick(in entity, dt);

                                }

                            }

                        }

                    }

                    using (new Checkpoint("PostAdvanceTickForSystem", system, WorldStep.LogicTick)) {

                        c.world.PostAdvanceTickForSystem();

                    }

                } else if (systemBase is IAdvanceTick advanceTickSystem) {

                    using (new Checkpoint("PrepareAdvanceTickForSystem", advanceTickSystem, WorldStep.LogicTick)) {

                        c.world.PrepareAdvanceTickForSystem(advanceTickSystem);

                    }

                    using (new Checkpoint(advanceTickSystem.GetType().FullName, advanceTickSystem, WorldStep.LogicTick)) {

                        advanceTickSystem.AdvanceTick(dt);

                    }

                    using (new Checkpoint("PostAdvanceTickForSystem", advanceTickSystem, WorldStep.LogicTick)) {

                        c.world.PostAdvanceTickForSystem();

                    }

                }

            });

            // modules.AdvanceTickPost
            this.StepElement<IModuleBase, IAdvanceTickPost, Tick>(tick, this.modules, fixedDeltaTime, WorldStep.ModulesLogicTick, (t, module, idx) => {
                if (module is IAdvanceTickStep step && t % step.step != Tick.Zero) return false;
                return module.world.IsModuleActive(idx);
            }, (system, dt) => system.AdvanceTickPost(dt));

            // systems.AdvanceTickPost
            this.StepGroup(tick, this.systemGroups, fixedDeltaTime, WorldStep.SystemsLogicTick, (in SystemGroup group) => group.runtimeSystem.systemAdvanceTickPost, (t, system, dt) => {
                if (system is IAdvanceTickStep step && t % step.step != Tick.Zero) return;
                system.AdvanceTickPost(dt);
            });

            ////////////////
            this.currentStep |= WorldStep.PluginsLogicTick;
            ////////////////
            
            using (new Checkpoint("UseLifetimeStep NotifyAllSystemsBelow", null, WorldStep.None)) {

                // Use one-shot entities
                this.UseEntityFlags();
                // Use lifetime step
                this.UseLifetimeStep(ComponentLifetime.NotifyAllSystemsBelow, fixedDeltaTime);

            }

            try {

                WorldStaticCallbacks.RaiseCallbackStep(this, WorldCallbackStep.LogicTick);

                using (new Checkpoint("PlayPluginsForTickPost", "PlayPluginsForTickPost", WorldStep.None)) {

                    this.PlayPluginsForTickPost(tick);

                }

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~WorldStep.PluginsLogicTick;
            ////////////////

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
            
            #if ENABLE_PROFILER
            ECSProfiler.LogicSystems.Value += (long)((tickSw.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency) * 1000000000L);
            #endif
            
        }

        /// <summary>
        /// Simulates world ticks [source..target)
        /// </summary>
        /// <param name="from">Source tick</param>
        /// <param name="to">Target tick</param>
        /// <returns>New target tick</returns>
        /// <exception cref="Exception">Failed if frame fix behaviour is FrameFixBehaviour.ExceptionOverTicksPreFrame and </exception>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Tick Simulate(Tick from, Tick to) {
            
            if (this.settings.frameFixType == FrameFixBehaviour.ExceptionOverTicksPreFrame &&
                this.settings.frameFixValue > 0L &&
                this.simulationToTick > this.simulationFromTick + this.settings.frameFixValue) {

                throw new System.Exception($"Simulation failed because of ticks count is out of range: [{this.simulationFromTick}..{this.simulationToTick})");

            }

            if (from > to) {

                //UnityEngine.Debug.LogError( UnityEngine.Time.frameCount + " From: " + from + ", To: " + to);
                return to;

            }

            if (from < Tick.Zero) from = Tick.Zero;

            var state = this.GetState();
            
            #if ENABLE_PROFILER
            ECSProfiler.LogicSystems.Value = 0;
            #endif
            
            if (this.settings.frameFixType == FrameFixBehaviour.AsyncOverTicksPerFrame &&
                to - from > this.settings.frameFixValue) {

                // Clamp simulation to frame fix value
                // to be sure we have reached target cpf value
                to = from + this.settings.frameFixValue;

            }
            
            this.cpf = to - from;
            var fixedDeltaTime = this.GetTickTime();
            var frameTime = 0L;
            for (state.tick = from; state.tick < to; ++state.tick) {
                
                this.tickTimeWatcher.Restart();
                {
                    this.RunTick(state.tick, fixedDeltaTime);
                }
                this.tickTimeWatcher.Stop();

                frameTime += this.tickTimeWatcher.ElapsedMilliseconds;
                if (this.settings.frameFixType == FrameFixBehaviour.AsyncOverMillisecondsPerFrame &&
                    frameTime > this.settings.frameFixValue) {

                    // Stop simulation at this point
                    // because we have reached max ms per frame
                    ++state.tick;
                    to = state.tick + 1;
                    break;

                }

            }

            #if ENABLE_PROFILER
            ECSProfiler.LogicSystems.Sample();
            #endif

            ////////////////
            this.currentStep |= WorldStep.PluginsLogicSimulate;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("SimulatePluginsForTicks", WorldStep.None);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"SimulatePluginsForTicks");
                #endif

                this.SimulatePluginsForTicks(from, to);

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("SimulatePluginsForTicks", WorldStep.None);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            
            ////////////////
            this.currentStep &= ~WorldStep.PluginsLogicSimulate;
            ////////////////
            
            return to;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void PlayPluginsForTickPre(Tick tick) {

            this.PlayPlugin1ForTickPre(tick);
            this.PlayPlugin2ForTickPre(tick);
            this.PlayPlugin3ForTickPre(tick);
            this.PlayPlugin4ForTickPre(tick);
            this.PlayPlugin5ForTickPre(tick);
            this.PlayPlugin6ForTickPre(tick);
            this.PlayPlugin7ForTickPre(tick);
            this.PlayPlugin8ForTickPre(tick);
            this.PlayPlugin9ForTickPre(tick);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void PlayPluginsForTickPost(Tick tick) {

            this.PlayPlugin1ForTickPost(tick);
            this.PlayPlugin2ForTickPost(tick);
            this.PlayPlugin3ForTickPost(tick);
            this.PlayPlugin4ForTickPost(tick);
            this.PlayPlugin5ForTickPost(tick);
            this.PlayPlugin6ForTickPost(tick);
            this.PlayPlugin7ForTickPost(tick);
            this.PlayPlugin8ForTickPost(tick);
            this.PlayPlugin9ForTickPost(tick);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void SimulatePluginsForTicks(Tick from, Tick to) {

            this.SimulatePlugin1ForTicks(from, to);
            this.SimulatePlugin2ForTicks(from, to);
            this.SimulatePlugin3ForTicks(from, to);
            this.SimulatePlugin4ForTicks(from, to);
            this.SimulatePlugin5ForTicks(from, to);
            this.SimulatePlugin6ForTicks(from, to);
            this.SimulatePlugin7ForTicks(from, to);
            this.SimulatePlugin8ForTicks(from, to);
            this.SimulatePlugin9ForTicks(from, to);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void RegisterPluginsModuleForEntity() {

            this.RegisterPlugin1ModuleForEntity();
            this.RegisterPlugin2ModuleForEntity();
            this.RegisterPlugin3ModuleForEntity();
            this.RegisterPlugin4ModuleForEntity();
            this.RegisterPlugin5ModuleForEntity();
            this.RegisterPlugin6ModuleForEntity();
            this.RegisterPlugin7ModuleForEntity();
            this.RegisterPlugin8ModuleForEntity();
            this.RegisterPlugin9ModuleForEntity();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void DestroyEntityPlugins(in Entity entity) {

            this.DestroyEntityPlugin1(entity);
            this.DestroyEntityPlugin2(entity);
            this.DestroyEntityPlugin3(entity);
            this.DestroyEntityPlugin4(entity);
            this.DestroyEntityPlugin5(entity);
            this.DestroyEntityPlugin6(entity);
            this.DestroyEntityPlugin7(entity);
            this.DestroyEntityPlugin8(entity);
            this.DestroyEntityPlugin9(entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void BeginRestoreEntities() {

            this.BeginRestoreEntitiesPlugins();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void BeginRestoreEntitiesPlugins() {

            this.BeginRestoreEntitiesPlugin1();
            this.BeginRestoreEntitiesPlugin2();
            this.BeginRestoreEntitiesPlugin3();
            this.BeginRestoreEntitiesPlugin4();
            this.BeginRestoreEntitiesPlugin5();
            this.BeginRestoreEntitiesPlugin6();
            this.BeginRestoreEntitiesPlugin7();
            this.BeginRestoreEntitiesPlugin8();
            this.BeginRestoreEntitiesPlugin9();

        }

        partial void BeginRestoreEntitiesPlugin1();
        partial void BeginRestoreEntitiesPlugin2();
        partial void BeginRestoreEntitiesPlugin3();
        partial void BeginRestoreEntitiesPlugin4();
        partial void BeginRestoreEntitiesPlugin5();
        partial void BeginRestoreEntitiesPlugin6();
        partial void BeginRestoreEntitiesPlugin7();
        partial void BeginRestoreEntitiesPlugin8();
        partial void BeginRestoreEntitiesPlugin9();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void EndRestoreEntities() {

            this.EndRestoreEntitiesPlugins();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void EndRestoreEntitiesPlugins() {

            this.EndRestoreEntitiesPlugin1();
            this.EndRestoreEntitiesPlugin2();
            this.EndRestoreEntitiesPlugin3();
            this.EndRestoreEntitiesPlugin4();
            this.EndRestoreEntitiesPlugin5();
            this.EndRestoreEntitiesPlugin6();
            this.EndRestoreEntitiesPlugin7();
            this.EndRestoreEntitiesPlugin8();
            this.EndRestoreEntitiesPlugin9();

        }

        partial void EndRestoreEntitiesPlugin1();
        partial void EndRestoreEntitiesPlugin2();
        partial void EndRestoreEntitiesPlugin3();
        partial void EndRestoreEntitiesPlugin4();
        partial void EndRestoreEntitiesPlugin5();
        partial void EndRestoreEntitiesPlugin6();
        partial void EndRestoreEntitiesPlugin7();
        partial void EndRestoreEntitiesPlugin8();
        partial void EndRestoreEntitiesPlugin9();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void SetEntityCapacityPlugins(int capacity) {

            this.SetEntityCapacityPlugin1(capacity);
            this.SetEntityCapacityPlugin2(capacity);
            this.SetEntityCapacityPlugin3(capacity);
            this.SetEntityCapacityPlugin4(capacity);
            this.SetEntityCapacityPlugin5(capacity);
            this.SetEntityCapacityPlugin6(capacity);
            this.SetEntityCapacityPlugin7(capacity);
            this.SetEntityCapacityPlugin8(capacity);
            this.SetEntityCapacityPlugin9(capacity);

        }

        partial void SetEntityCapacityPlugin1(int capacity);
        partial void SetEntityCapacityPlugin2(int capacity);
        partial void SetEntityCapacityPlugin3(int capacity);
        partial void SetEntityCapacityPlugin4(int capacity);
        partial void SetEntityCapacityPlugin5(int capacity);
        partial void SetEntityCapacityPlugin6(int capacity);
        partial void SetEntityCapacityPlugin7(int capacity);
        partial void SetEntityCapacityPlugin8(int capacity);
        partial void SetEntityCapacityPlugin9(int capacity);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CreateEntityPlugins(in Entity entity, bool isNew) {

            this.CreateEntityPlugin1(entity, isNew);
            this.CreateEntityPlugin2(entity, isNew);
            this.CreateEntityPlugin3(entity, isNew);
            this.CreateEntityPlugin4(entity, isNew);
            this.CreateEntityPlugin5(entity, isNew);
            this.CreateEntityPlugin6(entity, isNew);
            this.CreateEntityPlugin7(entity, isNew);
            this.CreateEntityPlugin8(entity, isNew);
            this.CreateEntityPlugin9(entity, isNew);

        }

        partial void CreateEntityPlugin1(Entity entity, bool isNew);
        partial void CreateEntityPlugin2(Entity entity, bool isNew);
        partial void CreateEntityPlugin3(Entity entity, bool isNew);
        partial void CreateEntityPlugin4(Entity entity, bool isNew);
        partial void CreateEntityPlugin5(Entity entity, bool isNew);
        partial void CreateEntityPlugin6(Entity entity, bool isNew);
        partial void CreateEntityPlugin7(Entity entity, bool isNew);
        partial void CreateEntityPlugin8(Entity entity, bool isNew);
        partial void CreateEntityPlugin9(Entity entity, bool isNew);

        partial void DestroyEntityPlugin1(Entity entity);
        partial void DestroyEntityPlugin2(Entity entity);
        partial void DestroyEntityPlugin3(Entity entity);
        partial void DestroyEntityPlugin4(Entity entity);
        partial void DestroyEntityPlugin5(Entity entity);
        partial void DestroyEntityPlugin6(Entity entity);
        partial void DestroyEntityPlugin7(Entity entity);
        partial void DestroyEntityPlugin8(Entity entity);
        partial void DestroyEntityPlugin9(Entity entity);

        partial void RegisterPlugin1ModuleForEntity();
        partial void RegisterPlugin2ModuleForEntity();
        partial void RegisterPlugin3ModuleForEntity();
        partial void RegisterPlugin4ModuleForEntity();
        partial void RegisterPlugin5ModuleForEntity();
        partial void RegisterPlugin6ModuleForEntity();
        partial void RegisterPlugin7ModuleForEntity();
        partial void RegisterPlugin8ModuleForEntity();
        partial void RegisterPlugin9ModuleForEntity();

        partial void PlayPlugin1ForTickPre(Tick tick);
        partial void PlayPlugin2ForTickPre(Tick tick);
        partial void PlayPlugin3ForTickPre(Tick tick);
        partial void PlayPlugin4ForTickPre(Tick tick);
        partial void PlayPlugin5ForTickPre(Tick tick);
        partial void PlayPlugin6ForTickPre(Tick tick);
        partial void PlayPlugin7ForTickPre(Tick tick);
        partial void PlayPlugin8ForTickPre(Tick tick);
        partial void PlayPlugin9ForTickPre(Tick tick);

        partial void PlayPlugin1ForTickPost(Tick tick);
        partial void PlayPlugin2ForTickPost(Tick tick);
        partial void PlayPlugin3ForTickPost(Tick tick);
        partial void PlayPlugin4ForTickPost(Tick tick);
        partial void PlayPlugin5ForTickPost(Tick tick);
        partial void PlayPlugin6ForTickPost(Tick tick);
        partial void PlayPlugin7ForTickPost(Tick tick);
        partial void PlayPlugin8ForTickPost(Tick tick);
        partial void PlayPlugin9ForTickPost(Tick tick);

        partial void SimulatePlugin1ForTicks(Tick from, Tick to);
        partial void SimulatePlugin2ForTicks(Tick from, Tick to);
        partial void SimulatePlugin3ForTicks(Tick from, Tick to);
        partial void SimulatePlugin4ForTicks(Tick from, Tick to);
        partial void SimulatePlugin5ForTicks(Tick from, Tick to);
        partial void SimulatePlugin6ForTicks(Tick from, Tick to);
        partial void SimulatePlugin7ForTicks(Tick from, Tick to);
        partial void SimulatePlugin8ForTicks(Tick from, Tick to);
        partial void SimulatePlugin9ForTicks(Tick from, Tick to);

    }

}