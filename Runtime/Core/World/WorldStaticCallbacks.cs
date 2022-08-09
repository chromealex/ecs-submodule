namespace ME.ECS {

    public static class WorldStaticCallbacks {

        public delegate void Init(World world);
        public delegate void Dispose(World world);

        public delegate void InitState(State state);
        public delegate void DisposeState(State state);
        
        public delegate void EntityDestroy(State state, in Entity entity);

        private static Init onInit;
        private static Dispose onDispose;

        private static InitState onInitState;
        private static DisposeState onDisposeState;
        private static InitState onInitResetState;

        private static EntityDestroy onEntityDestroy;
        
        public static void RegisterCallbacks(Init onInit, Dispose onDispose) {

            WorldStaticCallbacks.onInit += onInit;
            WorldStaticCallbacks.onDispose += onDispose;

        }

        public static void RegisterCallbacks(InitState onInitState, DisposeState onDisposeState, InitState onInitResetState) {

            WorldStaticCallbacks.onInitState += onInitState;
            WorldStaticCallbacks.onDisposeState += onDisposeState;
            WorldStaticCallbacks.onInitResetState += onInitResetState;

        }

        public static void RegisterCallbacks(EntityDestroy onEntityDestroy) {

            WorldStaticCallbacks.onEntityDestroy += onEntityDestroy;

        }

        public static void UnRegisterCallbacks(Init onInit, Dispose onDispose) {

            WorldStaticCallbacks.onInit -= onInit;
            WorldStaticCallbacks.onDispose -= onDispose;

        }

        public static void UnRegisterCallbacks(InitState onInitState, DisposeState onDisposeState, InitState onInitResetState) {

            WorldStaticCallbacks.onInitState -= onInitState;
            WorldStaticCallbacks.onDisposeState -= onDisposeState;
            WorldStaticCallbacks.onInitResetState -= onInitResetState;

        }

        public static void UnRegisterCallbacks(EntityDestroy onEntityDestroy) {

            WorldStaticCallbacks.onEntityDestroy -= onEntityDestroy;

        }

        public static void RaiseCallbackInitResetState(State state) {
            
            WorldStaticCallbacks.onInitResetState?.Invoke(state);
            
        }

        public static void RaiseCallbackInitState(State state) {
            
            WorldStaticCallbacks.onInitState?.Invoke(state);
            
        }

        public static void RaiseCallbackDisposeState(State state) {
            
            WorldStaticCallbacks.onDisposeState?.Invoke(state);
            
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

    }

}