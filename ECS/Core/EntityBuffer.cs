
using ME.ECS.Collections;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public readonly struct DataBuffer<T> where T : struct, IStructComponent {

        private readonly Unity.Collections.NativeSlice<T> data;
        private readonly int minIdx;
        
        public readonly Unity.Collections.NativeArray<T> arr;
        public readonly int Length;
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {

            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            this.minIdx = minIdx;
            if (this.minIdx > maxIdx) {

                this.minIdx = 0;
                maxIdx = 0;

            }
            if (this.minIdx < 0) this.minIdx = 0;
            if (maxIdx >= reg.components.Length) maxIdx = reg.components.Length - 1;
            this.Length = maxIdx - this.minIdx;
            this.arr = new Unity.Collections.NativeArray<T>(reg.components.data.arr, Unity.Collections.Allocator.Persistent);
            this.data = new Unity.Collections.NativeSlice<T>(this.arr, this.minIdx, maxIdx);
            
        }

        public void Push(World world, ME.ECS.Collections.BufferArray<Entity> arr) {
        
            var reg = (StructComponents<T>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T>()];
            for (int i = 0; i < arr.Length; ++i) {

                var entity = arr.arr[i];
                reg.components[entity.id] = this.Get(entity.id);
                reg.componentsStates.arr[entity.id] = 1;

            }
            
        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ref T Get(int entityId) {

            return ref this.data.GetRef(entityId - this.minIdx);
            
        }

    }
    
}