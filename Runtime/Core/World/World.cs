#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if WORLD_TICK_THREADED
#define TICK_THREADED
#endif
#if UNITY_EDITOR || DEVELOPMENT_BUILD
#define CHECKPOINT_COLLECTOR
#endif
using System.Collections.Generic;
using Unity.Jobs;

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {

    using ME.ECS.Collections;

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

    [System.Serializable]
    public struct WorldSettings {

        public bool useJobsForSystems;
        public bool useJobsForViews;
        public bool createInstanceForFeatures;
        public int maxTicksSimulationCount;
        public bool turnOffViews;

        public WorldViewsSettings viewsSettings;

        public static WorldSettings Default => new WorldSettings() {
            useJobsForSystems = true,
            useJobsForViews = true,
            createInstanceForFeatures = true,
            turnOffViews = false,
            viewsSettings = new WorldViewsSettings()
        };

    }

    [System.Serializable]
    public partial struct WorldDebugViewsSettings { }

    [System.Serializable]
    public struct WorldDebugSettings {

        public bool createGameObjectsRepresentation;
        public bool collectStatistic;
        public ME.ECS.Debug.StatisticsObject statisticsObject;
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
        internal Dictionary<System.Type, IFeatureBase> features;
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
        private const int WORLDS_CAPACITY = 4;
        private const int FILTERS_CACHE_CAPACITY = 10;
        
        #if FILTERS_STORAGE_LEGACY
        private static class FiltersDirectCache {

            internal static BufferArray<BufferArray<bool>> dic = new BufferArray<BufferArray<bool>>(null, 0); //new bool[World.WORLDS_CAPACITY][];

        }
        #endif

        private static int registryWorldId = 0;

        public int id { get; private set; }

        private State resetState;
        private bool hasResetState;
        internal State currentState;
        internal StructComponentsContainer structComponentsNoState;
        private uint seed;
        private int cpf; // CPF = Calculations per frame
        internal int entitiesCapacity;
        private bool isLoading;
        private bool isLoaded;
        private float loadingProgress;
        public bool isPaused { private set; get; }

        void IPoolableSpawn.OnSpawn() {

            this.InitializePools();
            ME.WeakRef.Reg(this);
            
            this.isPaused = false;
            this.speed = 1f;
            this.seed = default;

            this.worldThread = System.Threading.Thread.CurrentThread;
            
            #if UNITY_EDITOR
            try {
                Unity.Jobs.LowLevel.Unsafe.JobsUtility.JobDebuggerEnabled = false;
            } catch (System.Exception) { }
            #endif
            
            this.structComponentsNoState = new StructComponentsContainer();
            this.structComponentsNoState.Initialize(true);

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
            this.OnSpawnMarkers();
            this.OnSpawnFilters();

            this.InitializeGlobalEvents();

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

            this.structComponentsNoState.OnRecycle();

            this.DisposeGlobalEvents();

            var list = PoolListCopyable<Entity>.Spawn(World.ENTITIES_CACHE_CAPACITY);
            if (this.ForEachEntity(list) == true) {

                for (int i = list.Count - 1; i >= 0; --i) {

                    ref var item = ref list[i];
                    if (item.IsAlive() == true) this.RemoveEntity(item, cleanUpHierarchy: false);

                }

            }
            PoolListCopyable<Entity>.Recycle(ref list);
            this.GetState()?.storage.ApplyDead();

            PoolArray<bool>.Recycle(ref this.currentSystemContextFiltersUsed);
            this.currentSystemContextFiltersUsedAnyChanged = default;

            this.OnRecycleFilters();
            this.OnRecycleMarkers();
            this.OnRecycleComponents();
            this.OnRecycleStructComponents();
        
            #if FILTERS_STORAGE_LEGACY
            if (FiltersDirectCache.dic.arr != null) PoolArray<bool>.Recycle(ref FiltersDirectCache.dic.arr[this.id]);
            #endif

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
            var res = ME.ECS.Mathematics.VecMath.RotateTowards(p, t, deltaTime * rotationSpeed, 0f);
            return res.x.ToStringDec() + res.y.ToStringDec() + res.z.ToStringDec();// + " :: " + res.x + " :: " + res.y + " :: " + res.z;
            
        }

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

            // Make a ref of current filters to the new state
            #if FILTERS_STORAGE_LEGACY
            this.GetState().filters.Clear();
            data.state.filters = this.GetState().filters;
            this.GetState().filters = null;
            #endif

            this.SetState<TState>(data.state);
            #if FILTERS_STORAGE_LEGACY
            data.state.filters.OnDeserialize(this.GetEntitiesCount());
            #endif
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

            #if FILTERS_STORAGE_LEGACY
            ArrayUtils.Resize(this.id, ref FiltersDirectCache.dic);
            #endif

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

        partial void OnSpawnMarkers();
        partial void OnRecycleMarkers();

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

        #region GlobalEvents
        private struct GlobalEventFrameItem {

            public GlobalEvent globalEvent;
            public Entity data;

        }

        public enum GlobalEventType : byte {

            Logic,
            Visual,

        }

        private List<GlobalEventFrameItem> globalEventFrameItems;
        private HashSet<long> globalEventFrameEvents;

        internal void InitializeGlobalEvents() {
            
            this.globalEventFrameItems = PoolList<GlobalEventFrameItem>.Spawn(10);
            this.globalEventFrameEvents = PoolHashSet<long>.Spawn(10);

        }

        internal void DisposeGlobalEvents() {

            GlobalEvent.ResetCache();
            
            PoolList<GlobalEventFrameItem>.Recycle(ref this.globalEventFrameItems);
            PoolHashSet<long>.Recycle(ref this.globalEventFrameEvents);
            
        }

        public void ProcessGlobalEvents(GlobalEventType globalEventType) {

            if (globalEventType == GlobalEventType.Visual) {

                try {

                    for (int i = 0; i < this.globalEventFrameItems.Count; ++i) {

                        var item = this.globalEventFrameItems[i];
                        item.globalEvent.Run(in item.data);

                    }

                } catch (System.Exception ex) {

                    UnityEngine.Debug.LogException(ex);

                }

                this.globalEventFrameItems.Clear();
                this.globalEventFrameEvents.Clear();

            } else if (globalEventType == GlobalEventType.Logic) {

                for (int i = 0; i < this.currentState.globalEvents.globalEventLogicItems.Count; ++i) {

                    var item = this.currentState.globalEvents.globalEventLogicItems[i];
                    GlobalEvent.GetEventById(item.globalEvent).Run(in item.data);

                }

                this.currentState.globalEvents.globalEventLogicItems.Clear();
                this.currentState.globalEvents.globalEventLogicEvents.Clear();

            }

        }

        public bool CancelGlobalEvent(GlobalEvent globalEvent, in Entity entity, GlobalEventType globalEventType) {

            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (globalEventType == GlobalEventType.Visual) {

                if (this.globalEventFrameEvents.Contains(key) == true) {

                    for (int i = 0; i < this.globalEventFrameItems.Count; ++i) {

                        var item = this.globalEventFrameItems[i];
                        if (item.globalEvent == globalEvent && item.data == entity) {

                            this.globalEventFrameEvents.Remove(key);
                            this.globalEventFrameItems.RemoveAt(i);
                            return true;

                        }

                    }

                }

            } else if (globalEventType == GlobalEventType.Logic) {

                this.currentState.globalEvents.Remove(globalEvent, in entity);
                
            }

            return false;

        }

        public void RegisterGlobalEvent(GlobalEvent globalEvent, in Entity entity, GlobalEventType globalEventType) {

            var key = MathUtils.GetKey(globalEvent.GetHashCode(), entity.GetHashCode());
            if (globalEventType == GlobalEventType.Visual) {

                if (this.globalEventFrameEvents.Contains(key) == false) {

                    this.globalEventFrameEvents.Add(key);
                    this.globalEventFrameItems.Add(new GlobalEventFrameItem() {
                        globalEvent = globalEvent,
                        data = entity,
                    });

                }

            } else if (globalEventType == GlobalEventType.Logic) {

                this.currentState.globalEvents.Add(globalEvent, in entity);
                
            }

        }
        #endregion

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

            if (this.resetState != null) WorldUtilities.ReleaseState<TState>(ref this.resetState);
            this.resetState = WorldUtilities.CreateState<TState>();
            this.resetState.Initialize(this, freeze: true, restore: false);
            this.resetState.CopyFrom(this.GetState());
            this.resetState.tick = Tick.Zero;
            this.resetState.structComponents.Merge();
            this.hasResetState = true;

            this.currentState.structComponents.Merge();

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
                this.structComponentsNoState.SetEntityCapacity(state.storage.nextEntityId);
                ComponentsInitializerWorld.Init(state.storage.cache[state.storage.nextEntityId - 1]);
            } else {
                this.structComponentsNoState.SetEntityCapacity(state.storage.AliveCount + state.storage.DeadCount);
            }

            this.structComponentsNoState.Merge();

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
                this.currentState.structComponents.RemoveAll(in to);
                this.currentState.storage.archetypes.Clear(in to);
            }

            {
                // Copy data
                this.currentState.structComponents.CopyFrom(in from, in to);
                this.currentState.storage.archetypes.CopyFrom(in from, in to);
                this.UpdateFilters(in to);
                
                // Copy hierarchy data
                to.Remove<ME.ECS.Transform.Container>();
                if (from.TryRead(out ME.ECS.Transform.Container container) == true) {
                    to.SetParent(container.entity);
                }

                if (copyHierarchy == true) {

                    var nodes = from.Read<ME.ECS.Transform.Nodes>();
                    foreach (var child in nodes.items) {
                        var newChild = new Entity(EntityFlag.None);
                        newChild.CopyFrom(child);
                        newChild.SetParent(to);
                    }

                }
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsAlive(int entityId, ushort generation) {

            // Inline manually
            return this.currentState.storage.cache.arr[entityId].generation == generation;
            //return this.currentState.storage.IsAlive(entityId, version);

        }

        public ref readonly Entity GetEntityById(int id) {

            ref var ent = ref this.currentState.storage.GetEntityById(id);
            if (this.IsAlive(ent.id, ent.generation) == false) return ref Entity.Empty;

            return ref ent;

        }

        public void SetEntitiesCapacity(int capacity) {

            var curCap = this.entitiesCapacity + this.currentState.storage.AliveCount;
            
            this.entitiesCapacity = capacity;
            this.SetEntityCapacityPlugins(capacity);
            this.SetEntityCapacityInFilters(capacity);

            Entity maxEntity = default;
            for (int i = 0; i < capacity - curCap; ++i) {

                var e = this.AddEntity_INTERNAL(validate: false);
                this.RemoveEntity(in e, false);
                if (e.id > maxEntity.id) maxEntity = e;

            }

            if (maxEntity.id > 0) {
                this.UpdateEntityOnCreate(maxEntity, isNew: true);
            }

            this.currentState.storage.ApplyDead();

        }

        public ref Entity AddEntity(string name = null, EntityFlag flags = EntityFlag.None) {

            return ref this.AddEntity_INTERNAL(name, flags: flags);

        }
        
        private ref Entity AddEntity_INTERNAL(string name = null, bool validate = true, EntityFlag flags = EntityFlag.None) {
            
            E.IS_LOGIC_STEP(this);
            E.IS_WORLD_THREAD("AddEntity");
            
            var isNew = (validate == true && this.currentState.storage.WillNew());
            ref var entity = ref this.currentState.storage.Alloc();
            if (validate == true) this.UpdateEntityOnCreate(in entity, isNew);
            
            if (name != null) {

                entity.Set(new ME.ECS.Name.Name() {
                    value = name,
                });

            }

            if ((flags & EntityFlag.OneShot) != 0) {

                entity.SetOneShot<IsEntityOneShot>();

            }

            this.currentState.storage.flags.Set(entity.id, flags);

            return ref entity;

        }

        public int GetEntitiesCount() {

            return this.currentState.storage.AliveCount;

        }

        internal void UpdateEntityOnCreate(in Entity entity, bool isNew) {

            #if !FILTERS_STORAGE_LEGACY
            if (isNew == true) {
                ComponentsInitializerWorld.Init(in entity);
                this.currentState.storage.versions.Validate(in entity);
                this.CreateEntityPlugins(entity, true);
                this.CreateEntityInFilters(entity);
            } else {
                this.CreateEntityPlugins(entity, false);
            }
            #else
            if (isNew == true) ComponentsInitializerWorld.Init(in entity);
            this.currentState.storage.versions.Validate(in entity);
            this.CreateEntityPlugins(entity, isNew);
            this.CreateEntityInFilters(entity);
            #endif

        }

        public bool ForEachEntity(ListCopyable<Entity> results) {

            if (this.currentState == null) return false;
            return this.currentState.storage.ForEach(results);

        }

        public ListCopyable<int> GetAliveEntities() {

            if (this.currentState == null) return null;
            return this.currentState.storage.GetAlive();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void OnEntityVersionChanged(in Entity entity) {
            
            ECSTransformHierarchy.OnEntityVersionChanged(in entity);
            
        }

        public bool RemoveEntity(in Entity entity, bool cleanUpHierarchy = true) {

            E.IS_ALIVE(in entity);

            if (this.currentState.storage.Dealloc(in entity) == true) {

                if (cleanUpHierarchy == true) ECSTransformHierarchy.OnEntityDestroy(in entity);
                this.RemoveFromAllFilters(entity);
                this.DestroyEntityPlugins(in entity);
                #if !ENTITY_TIMERS_DISABLED
                this.currentState.timers.OnEntityDestroy(in entity);
                #endif

                this.currentState.storage.IncrementGeneration(in entity);

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

        public List<TModule> GetModules<TModule>(List<TModule> output) where TModule : IModuleBase {

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

        public NativeBufferArray<Entity> GetEntityStorage() {

            return this.currentState.storage.cache;

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

            if (this.settings.maxTicksSimulationCount > 0L && this.simulationToTick > this.simulationFromTick + this.settings.maxTicksSimulationCount) {

                throw new System.Exception("Simulation failed because of ticks count is out of range: [" + this.simulationFromTick + ".." + this.simulationToTick + ")");

            }

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

            ////////////////
            this.currentStep |= WorldStep.ModulesVisualTick;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, WorldStep.VisualTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"VisualTick-Pre [All Modules]");
                #endif

                for (int i = 0, count = this.modules.Count; i < count; ++i) {

                    if (this.IsModuleActive(i) == true) {

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules[i], WorldStep.VisualTick);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(this.modules[i].GetType().FullName);
                        #endif

                        if (this.modules[i] is IUpdate moduleBase) {

                            moduleBase.Update(deltaTime);

                        }

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules[i], WorldStep.VisualTick);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, WorldStep.VisualTick);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~WorldStep.ModulesVisualTick;
            ////////////////

            ////////////////
            this.currentStep |= WorldStep.SystemsVisualTick;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.systemGroups.arr, WorldStep.VisualTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"VisualTick-Pre [All Systems]");
                #endif

                for (int i = 0, count = this.systemGroupsLength; i < count; ++i) {

                    ref var group = ref this.systemGroups.arr[i];
                    if (group.runtimeSystem.systemUpdates == null) continue;
                    for (int j = 0; j < group.runtimeSystem.systemUpdates.Count; ++j) {

                        ref var system = ref group.runtimeSystem.systemUpdates[j];

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.VisualTick);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(system.GetType().FullName);
                        #endif

                        system.Update(deltaTime);

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.VisualTick);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.systemGroups.arr, WorldStep.VisualTick);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~WorldStep.SystemsVisualTick;
            ////////////////

            #if CHECKPOINT_COLLECTOR
            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("RemoveMarkers", WorldStep.None);
            #endif

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample($"Remove Markers");
            #endif

            this.RemoveMarkers();

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif

            #if CHECKPOINT_COLLECTOR
            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("RemoveMarkers", WorldStep.None);
            #endif

            ////////////////
            this.currentStep |= WorldStep.ModulesVisualTick;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, WorldStep.VisualTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"VisualTick-Pre-Late [All Modules]");
                #endif

                for (int i = 0, count = this.modules.Count; i < count; ++i) {

                    if (this.IsModuleActive(i) == true) {

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules[i], WorldStep.VisualTick);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(this.modules[i].GetType().FullName);
                        #endif

                        if (this.modules[i] is IUpdateLate moduleBase) {

                            moduleBase.UpdateLate(deltaTime);

                        }

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules[i], WorldStep.VisualTick);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, WorldStep.VisualTick);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~WorldStep.ModulesVisualTick;
            ////////////////

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

            ////////////////
            this.currentStep |= WorldStep.ModulesVisualTick;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, WorldStep.VisualTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"VisualTick-Post [All Modules]");
                #endif

                for (int i = 0, count = this.modules.Count; i < count; ++i) {

                    if (this.IsModuleActive(i) == true) {

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules[i], WorldStep.VisualTick);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(this.modules[i].GetType().FullName);
                        #endif

                        if (this.modules[i] is IUpdatePost moduleBase) {

                            moduleBase.UpdatePost(deltaTime);

                        }

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules[i], WorldStep.VisualTick);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, WorldStep.VisualTick);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~WorldStep.ModulesVisualTick;
            ////////////////

            ////////////////
            this.currentStep |= WorldStep.SystemsVisualTick;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.systemGroups.arr, WorldStep.VisualTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"VisualTick-Post [All Systems]");
                #endif

                for (int i = 0, count = this.systemGroupsLength; i < count; ++i) {

                    ref var group = ref this.systemGroups.arr[i];
                    if (group.runtimeSystem.systemUpdatesPost == null) continue;
                    for (int j = 0; j < group.runtimeSystem.systemUpdatesPost.Count; ++j) {

                        ref var system = ref group.runtimeSystem.systemUpdatesPost[j];

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.VisualTick);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(system.GetType().FullName);
                        #endif

                        system.UpdatePost(deltaTime);

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.VisualTick);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.systemGroups.arr, WorldStep.VisualTick);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~WorldStep.SystemsVisualTick;
            ////////////////

            this.ProcessGlobalEvents(GlobalEventType.Visual);

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

            if (this.settings.maxTicksSimulationCount > 0L && this.simulationToTick > this.simulationFromTick + this.settings.maxTicksSimulationCount) {

                throw new System.Exception("Simulation failed because of ticks count is out of range: [" + this.simulationFromTick + ".." + this.simulationToTick + ")");

            }

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

            this.currentState.storage.ApplyDead();
            this.currentState.structComponents.Merge();

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
            
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample($"UseLifetimeStep NotifyAllSystems");
            #endif

            this.UseLifetimeStep(ComponentLifetime.NotifyAllSystems, fixedDeltaTime);

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif

            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("PlayPluginsForTickPre", WorldStep.None);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"PlayPluginsForTickPre");
                #endif

                this.PlayPluginsForTickPre(tick);

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("PlayPluginsForTickPre", WorldStep.None);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~WorldStep.PluginsLogicTick;
            ////////////////

            ////////////////
            this.currentStep |= WorldStep.ModulesLogicTick;
            ////////////////
            try {

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, WorldStep.LogicTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"LogicTick [All Modules]");
                #endif

                for (int i = 0, count = this.modules.Count; i < count; ++i) {

                    if (this.IsModuleActive(i) == true) {

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules[i], WorldStep.LogicTick);
                        #endif

                        var module = this.modules[i];
                        if (module is IAdvanceTickStep step && tick % step.step != Tick.Zero) continue;

                        if (module is IAdvanceTick moduleBase) {

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.BeginSample(moduleBase.GetType().FullName);
                            #endif

                            moduleBase.AdvanceTick(in fixedDeltaTime);

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.EndSample();
                            #endif

                        }

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules[i], WorldStep.LogicTick);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.modules, WorldStep.LogicTick);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            ////////////////
            this.currentStep &= ~WorldStep.ModulesLogicTick;
            ////////////////

            ////////////////
            this.currentStep |= WorldStep.SystemsLogicTick;
            ////////////////
            try {

                // Pick random number
                this.GetRandomValue();

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.systemGroups.arr, WorldStep.LogicTick);
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("AdvanceTickPre", WorldStep.LogicTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"LogicTick [AdvanceTickPre]");
                #endif

                for (int i = 0, count = this.systemGroupsLength; i < count; ++i) {

                    ref var group = ref this.systemGroups.arr[i];
                    if (group.runtimeSystem.systemAdvanceTickPre == null) continue;
                    for (int j = 0; j < group.runtimeSystem.systemAdvanceTickPre.Count; ++j) {

                        ref var system = ref group.runtimeSystem.systemAdvanceTickPre[j];
                        if (system is IAdvanceTickStep step && tick % step.step != Tick.Zero) continue;

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.LogicTick);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(system.GetType().FullName);
                        #endif

                        system.AdvanceTickPre(fixedDeltaTime);

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.LogicTick);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("AdvanceTickPre", WorldStep.LogicTick);
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("AdvanceTickFilters", WorldStep.LogicTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"LogicTick [AdvanceTickFilters]");
                #endif

                for (int i = 0, count = this.systemGroupsLength; i < count; ++i) {

                    ref var group = ref this.systemGroups.arr[i];
                    if (group.runtimeSystem.systemAdvanceTick == null) continue;
                    for (int j = 0; j < group.runtimeSystem.systemAdvanceTick.Count; ++j) {

                        ref var systemBase = ref group.runtimeSystem.systemAdvanceTick[j];
                        if (systemBase is IAdvanceTickStep step && tick % step.step != Tick.Zero) continue;

                        if (systemBase is ISystemFilter system) {

                            this.currentSystemContextFilter = system;

                            #if CHECKPOINT_COLLECTOR
                            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.LogicTick);
                            #endif

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.BeginSample($"PrepareAdvanceTickForSystem");
                            #endif

                            this.PrepareAdvanceTickForSystem(system);

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.EndSample();
                            #endif

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.BeginSample(system.GetType().FullName);
                            #endif

                            {

                                /*if (sysFilter is IAdvanceTickBurst advTick) {

                                    // Under the development process
                                    // This should not used right now
                                    
                                    var functionPointer = Unity.Burst.BurstCompiler.CompileFunctionPointer(advTick.GetAdvanceTickForBurst());
                                    var arrEntities = sysFilter.filter.GetArray();
                                    using (var arr = new Unity.Collections.NativeArray<Entity>(arrEntities.arr, Unity.Collections.Allocator.TempJob)) {

                                        var length = arrEntities.Length;
                                        var burstWorldStructComponentsAccess = new BurstWorldStructComponentsAccess();
                                        unsafe {

                                            var bws = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf(ref burstWorldStructComponentsAccess);
                                            PoolArray<Entity>.Recycle(ref arrEntities);
                                            var job = new ForeachFilterJobBurst() {
                                                deltaTime = fixedDeltaTime,
                                                entities = arr,
                                                function = functionPointer,
                                                bws = bws
                                            };
                                            var jobHandle = job.Schedule(length, sysFilter.jobsBatchCount);
                                            jobHandle.Complete();
                                            
                                        }

                                    }
                                    
                                }*/

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
                                    if (this.settings.useJobsForSystems == true && jobs == true) {

                                        #if FILTERS_STORAGE_LEGACY
                                        var arrEntities = system.filter.ToArray();
                                        
                                        var filter = this.GetFilter(system.filter.id);
                                        var currentPools = Pools.current;
                                        Pools.current = this.currentThreadPools;
                                        {
                                            var job = new ForeachFilterJob() {
                                                deltaTime = fixedDeltaTime,
                                                slice = arrEntities,
                                                dataContains = filter.data.dataContains,
                                                dataVersions = (filter.data.onVersionChangedOnly == 1 ? filter.data.dataVersions : default),
                                            };
                                            var jobHandle = job.Schedule(arrEntities.Length, batch);
                                            jobHandle.Complete();
                                        }
                                        arrEntities.Dispose();
                                        Pools.current = currentPools;
                                        
                                        filter.UseVersioned();
                                        #else
                                        // TODO: Make a job
                                        foreach (var entity in system.filter) {

                                            system.AdvanceTick(in entity, fixedDeltaTime);

                                        }
                                        #endif
                                        
                                    } else {

                                        foreach (var entity in system.filter) {

                                            system.AdvanceTick(in entity, fixedDeltaTime);

                                        }

                                    }

                                }

                            }

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.EndSample();
                            #endif

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.BeginSample($"PostAdvanceTickForSystem");
                            #endif

                            this.PostAdvanceTickForSystem();

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.EndSample();
                            #endif

                            #if CHECKPOINT_COLLECTOR
                            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.LogicTick);
                            #endif

                        } else if (systemBase is IAdvanceTick advanceTickSystem) {

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.BeginSample($"PrepareAdvanceTickForSystem");
                            #endif

                            this.PrepareAdvanceTickForSystem(advanceTickSystem);

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.EndSample();
                            #endif

                            #if CHECKPOINT_COLLECTOR
                            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(advanceTickSystem, WorldStep.LogicTick);
                            #endif

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.BeginSample(advanceTickSystem.GetType().FullName);
                            #endif

                            advanceTickSystem.AdvanceTick(fixedDeltaTime);

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.EndSample();
                            #endif

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.BeginSample($"PostAdvanceTickForSystem");
                            #endif

                            this.PostAdvanceTickForSystem();

                            #if UNITY_EDITOR
                            UnityEngine.Profiling.Profiler.EndSample();
                            #endif

                            #if CHECKPOINT_COLLECTOR
                            if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(advanceTickSystem, WorldStep.LogicTick);
                            #endif
                        }

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("AdvanceTickFilters", WorldStep.LogicTick);
                #endif


                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("AdvanceTickPost", WorldStep.LogicTick);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"LogicTick [AdvanceTickPost]");
                #endif

                for (int i = 0, count = this.systemGroupsLength; i < count; ++i) {

                    ref var group = ref this.systemGroups.arr[i];
                    if (group.runtimeSystem.systemAdvanceTickPost == null) continue;
                    for (int j = 0; j < group.runtimeSystem.systemAdvanceTickPost.Count; ++j) {

                        ref var system = ref group.runtimeSystem.systemAdvanceTickPost[j];
                        if (system is IAdvanceTickStep step && tick % step.step != Tick.Zero) continue;

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.LogicTick);
                        #endif

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.BeginSample(system.GetType().FullName);
                        #endif

                        system.AdvanceTickPost(fixedDeltaTime);

                        #if UNITY_EDITOR
                        UnityEngine.Profiling.Profiler.EndSample();
                        #endif

                        #if CHECKPOINT_COLLECTOR
                        if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(system, WorldStep.LogicTick);
                        #endif

                    }

                }

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("AdvanceTickPost", WorldStep.LogicTick);
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint(this.systemGroups.arr, WorldStep.LogicTick);
                #endif

            } catch (System.Exception ex) {

                UnityEngine.Debug.LogException(ex);

            }
            
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample($"UseLifetimeStep NotifyAllSystemsBelow");
            #endif

            this.UseEntityFlags();
            this.UseLifetimeStep(ComponentLifetime.NotifyAllSystemsBelow, fixedDeltaTime);

            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif

            ////////////////
            this.currentStep &= ~WorldStep.SystemsLogicTick;
            ////////////////

            ////////////////
            this.currentStep |= WorldStep.PluginsLogicTick;
            ////////////////
            try {

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"ProcessGlobalEvents [Logic]");
                #endif

                this.ProcessGlobalEvents(GlobalEventType.Logic);

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("PlayPluginsForTickPost", WorldStep.None);
                #endif

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample($"PlayPluginsForTickPost");
                #endif

                this.PlayPluginsForTickPost(tick);

                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif

                #if CHECKPOINT_COLLECTOR
                if (this.checkpointCollector != null) this.checkpointCollector.Checkpoint("PlayPluginsForTickPost", WorldStep.None);
                #endif

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
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Simulate(Tick from, Tick to) {
            
            if (from > to) {

                //UnityEngine.Debug.LogError( UnityEngine.Time.frameCount + " From: " + from + ", To: " + to);
                return;

            }

            if (from < Tick.Zero) from = Tick.Zero;

            var state = this.GetState();
            
            #if ENABLE_PROFILER
            ECSProfiler.LogicSystems.Value = 0;
            #endif
            
            this.cpf = to - from;
            var fixedDeltaTime = this.GetTickTime();
            for (state.tick = from; state.tick < to; ++state.tick) {
                
                this.RunTick(state.tick, fixedDeltaTime);

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