namespace ME.ECS.Collections.LowLevel.Unsafe {
    
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    using static Cuts;
    using Unity.Collections.LowLevel.Unsafe;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(AllocatorDebugProxy))]
    public unsafe partial struct MemoryAllocator {

        [INLINE(256)]
        public MemPtr Alloc<T>() where T : struct {
            return this.Alloc(TSize<T>.size);
        }

        [INLINE(256)]
        public MemPtr Alloc(int size) {
            if (size <= 0) return MemPtr.Invalid;
            return this.Alloc((uint)size, out _);
        }

        [INLINE(256)]
        public MemPtr AllocArray<T>(uint length) where T : struct {
            return this.Alloc(length * Align(TSize<T>.size));
        }

        [INLINE(256)]
        public MemPtr AllocArray<T>(uint length, out safe_ptr<T> ptr) where T : unmanaged {
            var res = this.Alloc(length * Align(TSize<T>.size), out var memPtr);
            ptr = memPtr;
            return res;
        }

        [INLINE(256)]
        public MemPtr AllocArray(uint length, uint elementSize) {
            return this.Alloc(length * Align(elementSize));
        }

        public MemPtr ReAllocArray<T>(in MemPtr memPtr, uint newLength) where T : struct {
            var res = this.ReAlloc(memPtr, newLength * Align(TSize<T>.size));
            return res;
        }

        public MemPtr ReAllocArray(in MemPtr memPtr, uint newLength) {
            var res = this.ReAlloc(memPtr, newLength);
            return res;
        }

        [INLINE(256)]
        public readonly ref T Ref<T>(in MemPtr ptr) where T : unmanaged {
            return ref *(T*)this.GetPtr(ptr);
        }

        [INLINE(256)]
        public readonly ref T Ref<T>(MemPtr ptr) where T : struct {
            // return ref *(T*)this.GetPtr(ptr);
            return ref UnsafeUtility.AsRef<T>(this.GetPtr(ptr));
        }

        [INLINE(256)]
        public readonly ref T RefArray<T>(in MemPtr ptr, uint index) where T : unmanaged {
            return ref *(T*)((byte*)this.GetPtr(in ptr) + Align(TSize<T>.size) * index);
        }

        [INLINE(256)]
        public readonly ref T RefArray<T>(MemPtr ptr, uint index) where T : unmanaged {
            return ref *(T*)((byte*)this.GetPtr(in ptr) + Align(TSize<T>.size) * index);
        }

        [INLINE(256)]
        public readonly ref T RefArray<T>(in MemPtr ptr, int index) where T : unmanaged {
            return ref *(T*)((byte*)this.GetPtr(ptr) + Align(TSize<T>.size) * index);
        }

        [INLINE(256)]
        public readonly ref T RefArray<T>(MemPtr ptr, int index) where T : struct {
            // return ref *(T*)((byte*)this.GetPtr(ptr) + Align(TSize<T>.size) * index);
            return ref UnsafeUtility.AsRef<T>((byte*)this.GetPtr(ptr) + Align(TSize<T>.size) * index);
        }

        [INLINE(256)]
        public readonly MemPtr RefArrayPtr<T>(in MemPtr ptr, uint index) where T : unmanaged {
            return this.GetSafePtr((byte*)((T*)this.GetPtr(ptr) + Align(TSize<T>.size) * index), ptr.zoneId);
        }

        [INLINE(256)]
        public readonly MemPtr RefArrayPtr<T>(MemPtr ptr, uint index) where T : struct {
            return this.GetSafePtr(this.GetPtr(ptr) + Align(TSize<T>.size) * index, ptr.zoneId);
        }

        [INLINE(256)]
        public readonly safe_ptr<T> GetUnsafePtr<T>(in MemPtr ptr, uint offset = 0u) where T : unmanaged {
            return (safe_ptr)this.GetPtr(new MemPtr(ptr.zoneId, ptr.offset + offset));
        }

        [INLINE(256)]
        public readonly safe_ptr GetUnsafePtr(in MemPtr ptr, uint offset = 0u) {
            return (safe_ptr)this.GetPtr(new MemPtr(ptr.zoneId, ptr.offset + offset));
        }

        [INLINE(256)]
        public readonly safe_ptr<T> GetUnsafePtr<T>(MemPtr ptr, uint offset = 0u) where T : unmanaged {
            return (safe_ptr)this.GetPtr(new MemPtr(ptr.zoneId, ptr.offset + offset));
        }

        [INLINE(256)]
        public readonly safe_ptr GetUnsafePtr(MemPtr ptr, uint offset = 0u) {
            return (safe_ptr)this.GetPtr(new MemPtr(ptr.zoneId, ptr.offset + offset));
        }

    }

}