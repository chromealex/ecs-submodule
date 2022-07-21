#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using ME.ECS.Collections;

    public interface IStorage {

        int AliveCount { get; }
        int DeadCount { get; }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        bool IsAlive(int id, ushort generation);

        bool ForEach(ListCopyable<Entity> results);

        ref Entity Alloc();
        bool Dealloc(in Entity entity);

        void ApplyDead();

    }

}
