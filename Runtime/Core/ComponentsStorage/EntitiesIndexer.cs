#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {
    
    using Collections.LowLevel.Unsafe;
    using Collections.LowLevel;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct EntitiesIndexer {

        [ME.ECS.Serializer.SerializeField]
        private ME.ECS.Collections.LowLevel.MemArrayAllocator<ME.ECS.Collections.LowLevel.EquatableHashSet<int>> data;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void Initialize(ref MemoryAllocator allocator, int capacity) {

            if (this.data.isCreated == false) this.data = new ME.ECS.Collections.LowLevel.MemArrayAllocator<ME.ECS.Collections.LowLevel.EquatableHashSet<int>>(ref allocator, capacity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void Validate(ref MemoryAllocator allocator, int entityId) {

            this.data.Resize(ref allocator, entityId + 1);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly int GetCount(in MemoryAllocator allocator, int entityId) {

            var arr = this.data[in allocator, entityId];
            if (arr.isCreated == false) return 0;
            
            return arr.Count;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly bool Has(in MemoryAllocator allocator, int entityId, int componentId) {

            var arr = this.data[in allocator, entityId];
            if (arr.isCreated == false) return false;

            return arr.Contains(in allocator, componentId);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly ref EquatableHashSet<int> Get(in MemoryAllocator allocator, int entityId) {

            return ref this.data[in allocator, entityId];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void Set(ref MemoryAllocator allocator, int entityId, int componentId) {

            ref var item = ref this.data[in allocator, entityId];
            if (item.isCreated == false) item = new EquatableHashSet<int>(ref allocator, 1);
            item.Add(ref allocator, componentId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void Remove(ref MemoryAllocator allocator, int entityId, int componentId) {
            
            ref var item = ref this.data[in allocator, entityId];
            if (item.isCreated == true) item.Remove(ref allocator, componentId);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void RemoveAll(ref MemoryAllocator allocator, int entityId) {
            
            ref var item = ref this.data[in allocator, entityId];
            if (item.isCreated == true) item.Clear(in allocator);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal void Dispose(ref MemoryAllocator allocator) {

            for (int i = 0; i < this.data.Length; ++i) {

                ref var set = ref this.data[in allocator, i];
                if (set.isCreated == true) {
                    
                    set.Dispose(ref allocator);
                    
                }

            }
            
            this.data.Dispose(ref allocator);
            
        }

    }

}