
namespace ME.ECS {

    #if UNITY_2020_OR_NEWER
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public readonly ref struct DataBuffer<T> where T : struct, IStructComponent {

        private readonly System.Span<T> data;
        private readonly int minIdx;
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr) {

            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            this.minIdx = 0;
            var maxIdx = reg.components.Length;
            for (int i = 0; i < arr.Length; ++i) {

                var entity = arr.arr[i];
                if (this.minIdx > entity.id) this.minIdx = entity.id;
                if (maxIdx < entity.id) maxIdx = entity.id;

            }

            this.data = new System.Span<T>(reg.components.arr, this.minIdx, maxIdx);
            
        }
        
        public void Push(World world, ME.ECS.Collections.BufferArray<Entity> arr) {
        
            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            for (int i = 0; i < arr.Length; ++i) {

                var entity = arr.arr[i];
                reg.components.arr[entity.id] = this.Get(entity.id);
                reg.componentsStates.arr[entity.id] = 1;

            }
            
        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ref T Get(int entityId) {

            return ref this.data[entityId - this.minIdx];
            
        }

    }
    #else
    public readonly ref struct DataBuffer<T> where T : struct, IStructComponent {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr) {

            throw new System.Exception("To use Span please use Unity 2020 or newer");
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ref T Get(int entityId) {

            throw new System.Exception("To use Span please use Unity 2020 or newer");
            
        }

    }
    #endif

    /*public class Test {
        
        public struct A1 : IStructComponent {}
        public struct A2 : IStructComponent {}
        public struct A3 : IStructComponent {}
        
        public static void T() {

            var a = new EntityBuffer<A1, A2, A3>();
            

        }

    }*/

}