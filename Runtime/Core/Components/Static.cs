namespace ME.ECS {

    public struct ComponentTypesCounter {

        public static int counter = -1;

    }

    public struct AllComponentTypesCounter {

        public static int counter = -1;

    }

    public struct OneShotComponentTypesCounter {

        public static int counter = -1;

    }

    public struct ComponentTypesRegistry {

        public static System.Collections.Generic.Dictionary<System.Type, int> allTypeId = new System.Collections.Generic.Dictionary<System.Type, int>();
        public static System.Collections.Generic.Dictionary<System.Type, int> oneShotTypeId = new System.Collections.Generic.Dictionary<System.Type, int>();
        public static System.Collections.Generic.Dictionary<System.Type, int> typeId = new System.Collections.Generic.Dictionary<System.Type, int>();
        public static System.Action reset;

    }

    #if !FILTERS_LAMBDA_DISABLED
    public struct ComponentTypesLambda {

        public static System.Collections.Generic.Dictionary<int, System.Action<Entity>> itemsSet = new System.Collections.Generic.Dictionary<int, System.Action<Entity>>();
        public static System.Collections.Generic.Dictionary<int, System.Action<Entity>> itemsRemove = new System.Collections.Generic.Dictionary<int, System.Action<Entity>>();

    }
    #endif

    public struct ComponentTypes<TComponent> {

        public static readonly Unity.Burst.SharedStatic<int> burstTypeId = Unity.Burst.SharedStatic<int>.GetOrCreate<ComponentTypes<TComponent>, int>();
        public static readonly Unity.Burst.SharedStatic<byte> burstIsFilterVersioned = Unity.Burst.SharedStatic<byte>.GetOrCreate<ComponentTypes<TComponent>, byte>();
        public static int typeId = -1;
        public static bool isFilterVersioned = false;
        #if !FILTERS_LAMBDA_DISABLED
        public static readonly Unity.Burst.SharedStatic<byte> burstIsFilterLambda = Unity.Burst.SharedStatic<byte>.GetOrCreate<ComponentTypes<TComponent>, byte>();
        #endif
        public static bool isFilterLambda = false;

    }

    public struct OneShotComponentTypes<TComponent> {

        public static int typeId = -1;

    }
    
    public struct AllComponentTypes<TComponent> {

        public static readonly Unity.Burst.SharedStatic<int> burstTypeId = Unity.Burst.SharedStatic<int>.GetOrCreate<AllComponentTypes<TComponent>, int>();
        public static readonly Unity.Burst.SharedStatic<byte> burstIsVersionedNoState = Unity.Burst.SharedStatic<byte>.GetOrCreate<AllComponentTypes<TComponent>, byte>();
        public static int typeId = -1;
        public static bool isTag = false;
        public static bool isVersioned = false;
        #if !COMPONENTS_VERSION_NO_STATE_DISABLED
        public static bool isVersionedNoState = false;
        #endif
        public static bool isSimple = false;
        public static bool isBlittable = false;
        public static bool isDisposable = false;
        #if COMPONENTS_COPYABLE
        public static bool isCopyable = false;
        #endif
        #if !SHARED_COMPONENTS_DISABLED
        public static bool isShared = false;
        #endif
        public static bool isOneShot = false;
        public static bool isInHash = true;

    }

}