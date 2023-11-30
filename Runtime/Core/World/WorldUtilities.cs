#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class WorldUtilities {

        private static readonly System.Reflection.MethodInfo setComponentTypeIdMethodInfo = typeof(WorldUtilities).GetMethod(nameof(WorldUtilities.SetComponentTypeId));
        #if !FILTERS_LAMBDA_DISABLED
        private static readonly System.Reflection.MethodInfo setComponentLambdaMethodInfo = typeof(WorldUtilities).GetMethod(nameof(WorldUtilities.SetComponentFilterLambda));
        #endif

        public static bool IsMainThread() {

            if (Worlds.current == null) return true;
            return Unity.Jobs.LowLevel.Unsafe.JobsUtility.IsExecutingJob == false && Worlds.current.mainThread == System.Threading.Thread.CurrentThread;

        }

        public static bool IsWorldThread() {

            if (Worlds.current == null) return true;
            return Unity.Jobs.LowLevel.Unsafe.JobsUtility.IsExecutingJob == false && Worlds.current.worldThread == System.Threading.Thread.CurrentThread;

        }

        public static void SetWorld(World world) {

            Worlds.currentWorld = world;
            Worlds.current = world;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static TState CreateState<TState>() where TState : State, new() {

            var state = PoolStates<TState>.Spawn();
            ME.WeakRef.Reg(state);
            state.tick = default;
            state.randomState = default;
            return state;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ReleaseState<TState>(ref State state) where TState : State, new() {

            if (state == null) return;
            state.tick = default;
            state.randomState = default;
            PoolStates<TState>.Recycle((TState)state);
            state = null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ReleaseState<TState>(ref TState state) where TState : State, new() {

            state.tick = default;
            state.randomState = default;
            PoolStates<TState>.Recycle(ref state);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void CreateWorld<TState>(ref World worldRef, float tickTime, int forcedWorldId = 0, uint seed = 1u) where TState : State, new() {

            if (seed <= 0u) seed = 1u;

            if (worldRef != null) WorldUtilities.ReleaseWorld<TState>(ref worldRef);
            worldRef = PoolClass<World>.Spawn();
            worldRef.SetId(forcedWorldId);
            var worldInt = worldRef;
            worldInt.SetSeed(seed);
            worldInt.SetTickTime(tickTime);
            Worlds.Register(worldRef);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ReleaseWorld<TState>(ref World world) where TState : State, new() {

            if (world == null || world.isActive == false) {

                world = null;
                return;

            }

            Worlds.DeInitializeBegin();
            var w = world;
            var id = w.id;
            w.isActive = false;
            Worlds.currentWorld = w;
            Worlds.current = w;
            world.RecycleResetState<TState>();
            world.RecycleStates<TState>();
            PoolClass<World>.Recycle(ref w);
            Worlds.UnRegister(world, id);
            Worlds.DeInitializeEnd();
            world = null;

        }

        public static void ResetTypeIds() {

            AllComponentTypesCounter.counter = -1;
            OneShotComponentTypesCounter.counter = -1;
            ComponentTypesCounter.counter = -1;
            ComponentTypesRegistry.allTypeId.Clear();
            ComponentTypesRegistry.oneShotTypeId.Clear();
            ComponentTypesRegistry.typeId.Clear();
            if (ComponentTypesRegistry.reset != null) ComponentTypesRegistry.reset.Invoke();
            ComponentTypesRegistry.reset = null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool IsAllComponentInHash<TComponent>() {

            return AllComponentTypes<TComponent>.isInHash;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetAllComponentInHash<TComponent>(bool state) {

            AllComponentTypes<TComponent>.isInHash = state;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetAllComponentTypeId<TComponent>() {

            if (AllComponentTypes<TComponent>.typeId < 0) {

                WorldUtilities.CacheAllComponentTypeId<TComponent>();

            }

            return AllComponentTypes<TComponent>.typeId;

        }

        private static void CacheAllComponentTypeId<TComponent>() {
            
            AllComponentTypes<TComponent>.typeId = ++AllComponentTypesCounter.counter;
            AllComponentTypes<TComponent>.burstTypeId.Data = AllComponentTypes<TComponent>.typeId;
            ComponentTypesRegistry.allTypeId.Add(typeof(TComponent), AllComponentTypes<TComponent>.typeId);

            ComponentTypesRegistry.reset += () => {

                AllComponentTypes<TComponent>.burstTypeId.Data = -1;
                AllComponentTypes<TComponent>.typeId = -1;
                AllComponentTypes<TComponent>.isTag = false;

            };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetOneShotComponentTypeId<TComponent>() {

            if (OneShotComponentTypes<TComponent>.typeId < 0) {

                WorldUtilities.CacheOneShotComponentTypeId<TComponent>();

            }

            return OneShotComponentTypes<TComponent>.typeId;

        }

        private static void CacheOneShotComponentTypeId<TComponent>() {
            
            OneShotComponentTypes<TComponent>.typeId = ++OneShotComponentTypesCounter.counter;
            ComponentTypesRegistry.oneShotTypeId.Add(typeof(TComponent), OneShotComponentTypes<TComponent>.typeId);

            ComponentTypesRegistry.reset += () => {

                OneShotComponentTypes<TComponent>.typeId = -1;

            };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetComponentTypeId<TComponent>() {

            return ComponentTypes<TComponent>.typeId;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool IsVersioned<TComponent>() {

            return ComponentTypes<TComponent>.isFilterVersioned;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsFilterVersioned<TComponent>(bool state) {

            if (state == true) {
                ComponentTypes<TComponent>.isFilterVersioned = state;
                ComponentTypes<TComponent>.burstIsFilterVersioned.Data = 1;
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int SetComponentTypeIdByType(System.Type type) {

            var generic = WorldUtilities.setComponentTypeIdMethodInfo.MakeGenericMethod(type);
            return (int)generic.Invoke(obj: null, parameters: null);

        }

        #if !FILTERS_LAMBDA_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentFilterLambdaByType(System.Type type) {

            var generic = WorldUtilities.setComponentLambdaMethodInfo.MakeGenericMethod(type);
            generic.Invoke(obj: null, parameters: null);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentFilterLambda<TComponent>() {
            
            ComponentTypes<TComponent>.isFilterLambda = true;
            ComponentTypes<TComponent>.burstIsFilterLambda.Data = 1;
            
        }
        #endif
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int SetComponentTypeId<TComponent>() {

            if (ComponentTypes<TComponent>.typeId < 0) {

                ComponentTypes<TComponent>.typeId = ++ComponentTypesCounter.counter;
                ComponentTypes<TComponent>.burstTypeId.Data = ComponentTypes<TComponent>.typeId;
                ComponentTypesRegistry.typeId.Add(typeof(TComponent), ComponentTypes<TComponent>.typeId);

                ComponentTypesRegistry.reset += () => {

                    ComponentTypes<TComponent>.typeId = -1;
                    ComponentTypes<TComponent>.burstTypeId.Data = -1;

                };

            }

            return ComponentTypes<TComponent>.typeId;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool IsComponentAsTag<TComponent>() where TComponent : struct, IComponentBase {

            return AllComponentTypes<TComponent>.isTag;

        }

        #if COMPONENTS_COPYABLE
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool IsComponentAsCopyable<TComponent>() where TComponent : struct, IComponentBase {

            return AllComponentTypes<TComponent>.isCopyable;

        }
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsTag<TComponent>() {

            AllComponentTypes<TComponent>.isTag = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsSimple<TComponent>() {

            AllComponentTypes<TComponent>.isSimple = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsBlittable<TComponent>() {

            AllComponentTypes<TComponent>.isBlittable = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsDisposable<TComponent>() {

            AllComponentTypes<TComponent>.isDisposable = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsVersioned<TComponent>() {

            AllComponentTypes<TComponent>.isVersioned = true;

        }

        #if !SHARED_COMPONENTS_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsShared<TComponent>() {

            AllComponentTypes<TComponent>.isShared = true;

        }
        #endif

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsOneShot<TComponent>() {

            AllComponentTypes<TComponent>.isOneShot = true;
            WorldUtilities.GetOneShotComponentTypeId<TComponent>();

        }

        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsVersionedNoState<TComponent>() {

            AllComponentTypes<TComponent>.isVersionedNoState = true;
            AllComponentTypes<TComponent>.burstIsVersionedNoState.Data = 1;

        }
        #endif

        #if COMPONENTS_COPYABLE
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsCopyable<TComponent>() {

            AllComponentTypes<TComponent>.isCopyable = true;

        }
        #endif
        
        public static void SetComponentAsCopyableUnmanaged<TComponent>() {

            AllComponentTypes<TComponent>.isCopyableUnmanaged = true;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void InitComponentTypeId<TComponent>(bool isTag = false, bool isSimple = false, bool isBlittable = false, bool isDisposable = false, bool isCopyable = false, bool isVersioned = false, bool isVersionedNoState = false, bool isShared = false, bool isOneShot = false, bool isCopyableUnmanaged = false) {

            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            if (isSimple == true) WorldUtilities.SetComponentAsSimple<TComponent>();
            if (isBlittable == true) WorldUtilities.SetComponentAsBlittable<TComponent>();
            if (isDisposable == true) WorldUtilities.SetComponentAsDisposable<TComponent>();
            if (isVersioned == true) WorldUtilities.SetComponentAsVersioned<TComponent>();
            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (isVersionedNoState == true) WorldUtilities.SetComponentAsVersionedNoState<TComponent>();
            #endif
            #if COMPONENTS_COPYABLE
            if (isCopyable == true) WorldUtilities.SetComponentAsCopyable<TComponent>();
            #endif
            if (isCopyableUnmanaged == true) WorldUtilities.SetComponentAsCopyableUnmanaged<TComponent>();
            #if !SHARED_COMPONENTS_DISABLED
            if (isShared == true) WorldUtilities.SetComponentAsShared<TComponent>();
            #endif
            if (isOneShot == true) WorldUtilities.SetComponentAsOneShot<TComponent>();

            WorldUtilities.GetAllComponentTypeId<TComponent>();

        }

    }

}