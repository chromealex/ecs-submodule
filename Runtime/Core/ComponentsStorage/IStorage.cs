#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using ME.ECS.Collections;
    using Collections.V3;

    public interface IStorage {

        int AliveCount { get; }
        int DeadCount(in MemoryAllocator allocator);

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        bool IsAlive(in MemoryAllocator allocator, int id, ushort generation);

        bool ForEach(in MemoryAllocator allocator, ListCopyable<Entity> results);

        ref Entity Alloc(ref MemoryAllocator allocator);
        bool Dealloc(ref MemoryAllocator allocator, in Entity entity);

        void ApplyDead(ref MemoryAllocator allocator);

    }

}
