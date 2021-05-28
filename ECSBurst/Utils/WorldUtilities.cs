
namespace ME.ECSBurst {
    
    using Unity.Burst;

    public struct AllComponentTypes<TComponent> {

        public static readonly SharedStatic<int> typeId = SharedStatic<int>.GetOrCreate<AllComponentTypes<TComponent>, TComponentKey>();
        public static readonly bool isTag = false;
        public static readonly bool isVersioned = false;
        public static readonly bool isVersionedNoState = false;
        public static readonly bool isCopyable = false;
        public static readonly bool isShared = false;
        public static readonly bool isDisposable = false;
        public static readonly bool isInHash = true;
        public static readonly TComponent empty = default;
        
        public class TComponentKey {}

    }

    public struct ComponentTypes<TComponent> {

        public static readonly SharedStatic<int> typeId = SharedStatic<int>.GetOrCreate<ComponentTypes<TComponent>, TComponentKey>();
        public static readonly bool isFilterVersioned = false;

        public class TComponentKey {}

    }

    public struct ComponentTypesRegistry {

        public static System.Collections.Generic.Dictionary<System.Type, int> allTypeId = new System.Collections.Generic.Dictionary<System.Type, int>();
        public static System.Collections.Generic.Dictionary<System.Type, int> typeId = new System.Collections.Generic.Dictionary<System.Type, int>();
        public static System.Collections.Generic.Dictionary<int, int> typeIdToAlign = new System.Collections.Generic.Dictionary<int, int>();
        public static System.Action reset;

    }

    public struct AllComponentTypesCounter {

        public static readonly SharedStatic<int> counter = SharedStatic<int>.GetOrCreate<AllComponentTypesCounter, AllComponentTypesCounterKey>();

        public class AllComponentTypesCounterKey {}

    }

    public struct ComponentTypesCounter {

        public static int counter = -1;

    }

    public static class WorldUtilities {

        public static int GetComponentAlign(int index) {

            if (ComponentTypesRegistry.typeIdToAlign.TryGetValue(index, out var align) == true) {

                return align;

            }

            return 0;
            
        }
        
        private static void CacheAllComponentTypeId<TComponent>() where TComponent : struct {
            
            AllComponentTypes<TComponent>.typeId.Data = ++AllComponentTypesCounter.counter.Data;
            ComponentTypesRegistry.allTypeId.Add(typeof(TComponent), AllComponentTypes<TComponent>.typeId.Data);
            ComponentTypesRegistry.typeIdToAlign.Add(AllComponentTypes<TComponent>.typeId.Data, Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AlignOf<TComponent>());

            ComponentTypesRegistry.reset += () => {

                AllComponentTypes<TComponent>.typeId.Data = -1;
                
            };

        }

        private static void CacheComponentTypeId<TComponent>() {
            
            ComponentTypes<TComponent>.typeId.Data = ++ComponentTypesCounter.counter;
            ComponentTypesRegistry.typeId.Add(typeof(TComponent), ComponentTypes<TComponent>.typeId.Data);

            ComponentTypesRegistry.reset += () => {

                ComponentTypes<TComponent>.typeId.Data = -1;
                
            };

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void UpdateAllComponentTypeId<TComponent>() where TComponent : struct {

            if (AllComponentTypes<TComponent>.typeId.Data < 0) {

                WorldUtilities.CacheAllComponentTypeId<TComponent>();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void UpdateComponentTypeId<TComponent>() {

            if (ComponentTypes<TComponent>.typeId.Data < 0) {

                WorldUtilities.CacheComponentTypeId<TComponent>();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetComponentTypeId<TComponent>() {

            return ComponentTypes<TComponent>.typeId.Data;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int GetAllComponentTypeId<TComponent>() {

            return AllComponentTypes<TComponent>.typeId.Data;

        }

    }
    
}