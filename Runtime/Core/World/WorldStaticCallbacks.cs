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
        
        public delegate void EntityDestroy(State state, in Entity entity);
        
        public delegate void WorldStep(World world, ME.ECS.WorldStep step);
        public delegate void WorldLifetimeStep(World world, ComponentLifetime step, tfloat deltaTime);

        private static Init onInit;
        private static Dispose onDispose;

        private static InitState onInitResetState;

        private static EntityDestroy onEntityDestroy;

        private static WorldStep onWorldStep;
        private static WorldLifetimeStep onWorldLifetimeStep;
        
        public static void RegisterCallbacks(Init onInit, Dispose onDispose) {

            WorldStaticCallbacks.onInit += onInit;
            WorldStaticCallbacks.onDispose += onDispose;

        }

        public static void RegisterCallbacks(WorldStep callback) {

            WorldStaticCallbacks.onWorldStep += callback;

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

        public static void RaiseCallbackEntityDestroy(State state, in Entity entity) {

            WorldStaticCallbacks.onEntityDestroy?.Invoke(state, in entity);

        }

        public static void RaiseCallbackStep(World world, ME.ECS.WorldStep step) {
            
            WorldStaticCallbacks.onWorldStep?.Invoke(world, step);

        }

        public static void RaiseCallbackLifetimeStep(World world, ComponentLifetime step, tfloat deltaTime) {
            
            WorldStaticCallbacks.onWorldLifetimeStep?.Invoke(world, step, deltaTime);

        }

    }

}