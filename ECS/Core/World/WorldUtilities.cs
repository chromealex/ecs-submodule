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
        public static void Release(ref Storage storage) {

            //PoolClass<Storage>.Recycle(ref storage);
            storage.Recycle();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Release(ref FiltersStorage storage) {

            if (storage == null) return;
            PoolClass<FiltersStorage>.Recycle(ref storage);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Release(ref StructComponentsContainer storage) {

            storage.OnRecycle();

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
            ComponentTypesRegistry.allTypeId.Clear();
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
            ComponentTypesRegistry.allTypeId.Add(typeof(TComponent), AllComponentTypes<TComponent>.typeId);

            ComponentTypesRegistry.reset += () => {

                AllComponentTypes<TComponent>.typeId = -1;
                AllComponentTypes<TComponent>.isTag = false;

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

            if (state == true) ComponentTypes<TComponent>.isFilterVersioned = state;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentTypeId<TComponent>() {

            if (ComponentTypes<TComponent>.typeId < 0) {

                ComponentTypes<TComponent>.typeId = ++ComponentTypesCounter.counter;
                ComponentTypesRegistry.typeId.Add(typeof(TComponent), ComponentTypes<TComponent>.typeId);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool IsComponentAsTag<TComponent>() where TComponent : struct, IStructComponentBase {

            return AllComponentTypes<TComponent>.isTag;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool IsComponentAsCopyable<TComponent>() where TComponent : struct, IStructComponentBase {

            return AllComponentTypes<TComponent>.isCopyable;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsTag<TComponent>() {

            AllComponentTypes<TComponent>.isTag = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsVersioned<TComponent>() {

            AllComponentTypes<TComponent>.isVersioned = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsShared<TComponent>() {

            AllComponentTypes<TComponent>.isShared = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsOneShot<TComponent>() {

            AllComponentTypes<TComponent>.isOneShot = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsVersionedNoState<TComponent>() {

            AllComponentTypes<TComponent>.isVersionedNoState = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetComponentAsCopyable<TComponent>() {

            AllComponentTypes<TComponent>.isCopyable = true;

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
        public static bool InitComponentTypeId<TComponent>(bool isTag = false, bool isCopyable = false, bool isDisposable = false, bool isVersioned = false, bool isVersionedNoState = false, bool isShared = false, bool isOneShot = false) {

            var isNew = (AllComponentTypes<TComponent>.typeId == -1);
            if (isTag == true) WorldUtilities.SetComponentAsTag<TComponent>();
            if (isVersioned == true) WorldUtilities.SetComponentAsVersioned<TComponent>();
            if (isVersionedNoState == true) WorldUtilities.SetComponentAsVersionedNoState<TComponent>();
            if (isCopyable == true) WorldUtilities.SetComponentAsCopyable<TComponent>();
            if (isShared == true) WorldUtilities.SetComponentAsShared<TComponent>();
            if (isDisposable == true) WorldUtilities.SetComponentAsDisposable<TComponent>();
            if (isOneShot == true) WorldUtilities.SetComponentAsOneShot<TComponent>();

            WorldUtilities.GetAllComponentTypeId<TComponent>();

            return isNew;

        }

    }

}