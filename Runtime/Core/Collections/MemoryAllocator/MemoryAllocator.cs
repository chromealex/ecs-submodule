namespace ME.ECS.Collections {

    public enum AllocatorType {

        Invalid = 0,
        Persistent,
        Temp,

    }

    public enum ClearOptions {

        ClearMemory,
        UninitializedMemory,

    }

    public unsafe interface IMemoryAllocator<TAllocator, TMemPtr> {

        /// 
        /// Constructors
        /// 
        TAllocator Initialize(long initialSize, long maxSize = -1L);
        void Dispose();
        void CopyFrom(in TAllocator other);

        /// 
        /// Base
        /// 
        ref T Ref<T>(TMemPtr ptr) where T : unmanaged;
        ref T RefUnmanaged<T>(TMemPtr ptr) where T : struct;
        TMemPtr Alloc<T>() where T : unmanaged;
        TMemPtr AllocUnmanaged<T>() where T : struct;
        TMemPtr Alloc(long size);
        bool Free(TMemPtr ptr);
        TMemPtr ReAlloc(TMemPtr ptr, long size, ClearOptions options);
        void MemCopy(TMemPtr dest, long destOffset, TMemPtr source, long sourceOffset, long length);
        void MemClear(TMemPtr dest, long destOffset, long length);
        void Prepare(long size);
        void* GetUnsafePtr(in TMemPtr ptr);

        /// 
        /// Arrays
        /// 
        ref T RefArray<T>(TMemPtr ptr, int index) where T : unmanaged;
        TMemPtr ReAllocArray<T>(TMemPtr ptr, int newLength, ClearOptions options) where T : unmanaged;
        TMemPtr AllocArray<T>(int length) where T : unmanaged;
        ref T RefArrayUnmanaged<T>(TMemPtr ptr, int index) where T : struct;
        TMemPtr ReAllocArrayUnmanaged<T>(TMemPtr ptr, int newLength, ClearOptions options) where T : struct;
        TMemPtr AllocArrayUnmanaged<T>(int length) where T : struct;

    }
    
}