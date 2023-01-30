#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {

    public static class WorldStaticCallbacks {

        public delegate void Init(World world);
        public delegate void Dispose(World world);

        public delegate void InitState(State state);
        public delegate void DisposeState(State state);
        
        public delegate void ViewCreated(in Entity entity, ME.ECS.Views.IView view, int parentId);
        public delegate void ViewDestroy(in Entity entity, ME.ECS.Views.IView view);
        
        public delegate uint EntityGetVersion(in Entity entity);
        public delegate void EntityDestroy(State state, in Entity entity, bool cleanUpHierarchy);
        public delegate void EntityCopyFrom(World world, in Entity from, in Entity to, bool copyHierarchy);
        
        public delegate void WorldStep(World world, ME.ECS.WorldCallbackStep step);
        public delegate void WorldLifetimeStep(World world, ComponentLifetime step, tfloat deltaTime);

        private static Init onInit;
        private static Dispose onDispose;

        private static InitState onInitResetState;
        
        private static ViewCreated onViewCreated;
        private static ViewDestroy onViewDestroy;

        private static EntityDestroy onEntityDestroy;
        private static EntityCopyFrom onEntityCopyFrom;

        private static WorldStep onWorldStep;
        private static WorldLifetimeStep onWorldLifetimeStep;
        
        private static EntityGetVersion entityGetVersion;

        public static void SetGetVersionCallback(EntityGetVersion getVersion) {

            WorldStaticCallbacks.entityGetVersion = getVersion;

        }
        
        public static void UnSetGetVersionCallback(EntityGetVersion getVersion) {

            if (WorldStaticCallbacks.entityGetVersion == getVersion) WorldStaticCallbacks.entityGetVersion = null;

        }

        public static uint GetEntityVersion(in Entity entity) {

            if (WorldStaticCallbacks.entityGetVersion != null) {

                return WorldStaticCallbacks.entityGetVersion.Invoke(in entity);

            }

            return entity.GetVersion();

        }
        
        public static void RegisterCallbacks(Init onInit, Dispose onDispose) {

            WorldStaticCallbacks.onInit += onInit;
            WorldStaticCallbacks.onDispose += onDispose;

        }

        public static void RegisterCallbacks(WorldStep callback) {

            WorldStaticCallbacks.onWorldStep += callback;

        }

        public static void RegisterCallbacks(EntityCopyFrom callback) {

            WorldStaticCallbacks.onEntityCopyFrom += callback;

        }

        public static void RegisterCallbacks(WorldLifetimeStep callback) {

            WorldStaticCallbacks.onWorldLifetimeStep += callback;

        }

        public static void RegisterCallbacks(InitState onInitResetState) {

            WorldStaticCallbacks.onInitResetState += onInitResetState;

        }

        public static void RegisterCallbacks(EntityDestroy onEntityDestroy) {

            WorldStaticCallbacks.onEntityDestroy += onEntityDestroy;

        }

        public static void RegisterViewCreatedCallback(ViewCreated onViewCreated) {
            
            WorldStaticCallbacks.onViewCreated += onViewCreated;

        }

        public static void RegisterViewDestroyCallback(ViewDestroy onViewDestroy) {
            
            WorldStaticCallbacks.onViewDestroy += onViewDestroy;

        }

        public static void UnRegisterViewDestroyCallback(ViewDestroy onViewDestroy) {
            
            WorldStaticCallbacks.onViewDestroy -= onViewDestroy;

        }

        public static void UnRegisterViewCreatedCallback(ViewCreated onViewCreated) {
            
            WorldStaticCallbacks.onViewCreated -= onViewCreated;

        }

        public static void UnRegisterCallbacks(EntityCopyFrom callback) {

            WorldStaticCallbacks.onEntityCopyFrom -= callback;

        }

        public static void UnRegisterCallbacks(WorldStep callback) {

            WorldStaticCallbacks.onWorldStep -= callback;

        }

        public static void UnRegisterCallbacks(WorldLifetimeStep callback) {

            WorldStaticCallbacks.onWorldLifetimeStep -= callback;

        }

        public static void UnRegisterCallbacks(Init onInit, Dispose onDispose) {

            WorldStaticCallbacks.onInit -= onInit;
            WorldStaticCallbacks.onDispose -= onDispose;

        }

        public static void UnRegisterCallbacks(InitState onInitResetState) {

            WorldStaticCallbacks.onInitResetState -= onInitResetState;

        }

        public static void UnRegisterCallbacks(EntityDestroy onEntityDestroy) {

            WorldStaticCallbacks.onEntityDestroy -= onEntityDestroy;

        }

        public static void RaiseCallbackInitResetState(State state) {
            
            WorldStaticCallbacks.onInitResetState?.Invoke(state);
            
        }

        public static void RaiseCallbackInit(World world) {
            
            WorldStaticCallbacks.onInit?.Invoke(world);
            
        }

        public static void RaiseCallbackDispose(World world) {
            
            WorldStaticCallbacks.onDispose?.Invoke(world);
            
        }

        public static void RaiseCallbackEntityDestroy(State state, in Entity entity, bool cleanUpHierarchy) {

            WorldStaticCallbacks.onEntityDestroy?.Invoke(state, in entity, cleanUpHierarchy);

        }

        public static void RaiseCallbackEntityCopyFrom(World world, in Entity from, in Entity to, bool copyHierarchy) {

            WorldStaticCallbacks.onEntityCopyFrom?.Invoke(world, in from, in to, copyHierarchy);

        }

        public static void RaiseCallbackStep(World world, ME.ECS.WorldCallbackStep step) {
            
            WorldStaticCallbacks.onWorldStep?.Invoke(world, step);

        }

        public static void RaiseCallbackLifetimeStep(World world, ComponentLifetime step, tfloat deltaTime) {
            
            WorldStaticCallbacks.onWorldLifetimeStep?.Invoke(world, step, deltaTime);

        }

        public static void RaiseCallbackOnViewCreated(in Entity entity, ME.ECS.Views.IView view, int parentId) {
            
            WorldStaticCallbacks.onViewCreated?.Invoke(in entity, view, parentId);

        }

        public static void RaiseCallbackOnViewDestroy(in Entity entity, ME.ECS.Views.IView view) {
            
            WorldStaticCallbacks.onViewDestroy?.Invoke(in entity, view);

        }

    }

}