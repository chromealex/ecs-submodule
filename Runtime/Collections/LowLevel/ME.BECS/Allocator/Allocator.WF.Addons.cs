namespace ME.ECS.Collections.LowLevel.Unsafe {

    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    using static Cuts;
    using Unity.Collections.LowLevel.Unsafe;

    public unsafe partial struct MemoryAllocator {

        public bool isValid => this.allocatorLabel != Unity.Collections.Allocator.Invalid;

        public static MemoryAllocatorContext CreateContext() {

            return new MemoryAllocatorContext() {
                allocator = Worlds.current.currentState.allocator,
            }.Create();

        }

        public readonly void MemClear<T>(MemPtr dest) where T : struct {
            UnsafeUtility.MemClear(this.GetUnsafePtr(dest).ptr, TSize<T>.size);
        }

    }

}
