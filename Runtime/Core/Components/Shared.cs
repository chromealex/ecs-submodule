namespace ME.ECS {

    #if !SHARED_COMPONENTS_DISABLED
    public struct SharedData : IComponent, IComponentDisposable<SharedData> {

        public ME.ECS.Collections.LowLevel.Dictionary<int, uint> archetypeToId;

        public void OnDispose(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator) {
            if (this.archetypeToId.isCreated == true) this.archetypeToId.Dispose(ref allocator);
        }

        public void ReplaceWith(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, in SharedData other) {
            this.archetypeToId.ReplaceWith(ref allocator, other.archetypeToId);
        }
        
        public void CopyFrom(ref ME.ECS.Collections.LowLevel.Unsafe.MemoryAllocator allocator, in SharedData other) {
            this.archetypeToId.CopyFrom(ref allocator, other.archetypeToId);
        }

    }
    #endif

}