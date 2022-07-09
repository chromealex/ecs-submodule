namespace ME.ECS.Collections.V1 {
    
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Unity.Collections.LowLevel.Unsafe;

    /// <summary>
    /// Pointer to MemoryAllocator::data (index)
    /// </summary>
    public readonly struct MemPtr {

        public readonly int value;
        public readonly bool isValid;

        internal MemPtr(int value) {
            this.value = value;
            this.isValid = true;
        }

        public bool IsValid() => this.isValid;

    }

    //[UnityEditor.InitializeOnLoadAttribute]
    public static class StaticAllocators {

        private static readonly Destructor finalize = new Destructor();

        private static MemoryAllocator persistent;
        private static MemoryAllocator temp;

        public static ref MemoryAllocator GetAllocator(AllocatorType type) {

            switch (type) {
                case AllocatorType.Persistent: return ref StaticAllocators.persistent;
                case AllocatorType.Temp: return ref StaticAllocators.temp;
            }
            
            throw new System.Exception($"Allocator type {type} is unknown");
            
        }
        
        static StaticAllocators() {
            
            /*
            // 4 MB of persistent memory + no max size
            StaticAllocators.persistent = new MemoryAllocator().Initialize(4 * 1024 * 1024, -1);
            
            // 256 KB of temp memory + max size = 256 KB
            StaticAllocators.temp = new MemoryAllocator().Initialize(256 * 1024, 256 * 1024);
            */
            
        }

        private sealed class Destructor {
            ~Destructor() {
                //StaticAllocators.persistent.Dispose();
                //StaticAllocators.temp.Dispose();
            }
        }

    }

    public unsafe struct MemoryAllocator : IMemoryAllocator<MemoryAllocator, MemPtr> {

        private struct Block {

            public int index;
            public long size;
        
            // next ptr to data[] (MemPtr)
            public int next;
            // prev ptr to data[] (MemPtr)
            public int prev;

        }

        private ListCopyable<Block> allocated;
        private ListCopyable<Block> free;
        private DictionaryInt<int> allocatedIndex;
        private DictionaryInt<int> freeIndex;
        private System.IntPtr data;
        private long maxSize;
        private long length;
        private bool isValid;
        private object sync;
        
        public MemoryAllocator Initialize(long initialSize, long maxSize = -1L) {

            const int capacity = 100;
            this.sync = new object();
            this.allocated = PoolListCopyable<Block>.Spawn(capacity);
            this.free = PoolListCopyable<Block>.Spawn(capacity);
            this.allocatedIndex = PoolDictionaryInt<int>.Spawn(capacity);
            this.freeIndex = PoolDictionaryInt<int>.Spawn(capacity);
            
            var ptr = new Block() {
                index = 0,
                size = initialSize,
                next = -1,
                prev = -1,
            };
            this.free.Add(ptr);
            this.maxSize = maxSize;
            this.data = (System.IntPtr)UnsafeUtility.Malloc(initialSize, UnsafeUtility.AlignOf<byte>(), Unity.Collections.Allocator.Persistent);
            UnsafeUtility.MemClear((void*)this.data, initialSize);
            this.length = initialSize;
            this.isValid = true;

            return this;

        }

        public void Dispose() {
            
            PoolListCopyable<Block>.Recycle(ref this.allocated);
            PoolListCopyable<Block>.Recycle(ref this.free);
            PoolDictionaryInt<int>.Recycle(ref this.allocatedIndex);
            PoolDictionaryInt<int>.Recycle(ref this.freeIndex);
            if (this.data != System.IntPtr.Zero) UnsafeUtility.Free((void*)this.data, Unity.Collections.Allocator.Persistent);
            this = default;

        }

        public void CopyFrom(in MemoryAllocator other) {
            
            ArrayUtils.Copy(other.allocated, ref this.allocated);
            ArrayUtils.Copy(other.free, ref this.free);
            ArrayUtils.Copy(other.allocatedIndex, ref this.allocatedIndex);
            ArrayUtils.Copy(other.freeIndex, ref this.freeIndex);
            {
                if (this.data == System.IntPtr.Zero &&
                    other.data == System.IntPtr.Zero) {
                    // nothing to copy - all is null
                } else if (this.data != System.IntPtr.Zero &&
                           other.data == System.IntPtr.Zero) {
                    // dispose current data
                    UnsafeUtility.Free((void*)this.data, Unity.Collections.Allocator.Persistent);
                } else if ((this.data == System.IntPtr.Zero &&
                           other.data != System.IntPtr.Zero) ||
                           this.length != other.length) {
                    if (this.length != other.length) {
                        if (this.data != System.IntPtr.Zero) UnsafeUtility.Free((void*)this.data, Unity.Collections.Allocator.Persistent);
                    }
                    // create current data and copy from other.data
                    this.data = (System.IntPtr)UnsafeUtility.Malloc(other.length, UnsafeUtility.AlignOf<byte>(), Unity.Collections.Allocator.Persistent);
                    UnsafeUtility.MemCpy(
                        (void*)this.data,
                        (void*)other.data,
                        other.length
                        );
                } else {
                    // copy data
                    UnsafeUtility.MemCpy(
                        (void*)this.data,
                        (void*)other.data,
                        other.length
                    );
                }
            }
            this.maxSize = other.maxSize;
            this.length = other.length;

        }

        [MethodImpl(256)]
        public readonly ref T Ref<T>(MemPtr ptr) where T : unmanaged {

            lock (this.sync)
            {

                var offset = ptr.value;
                if (offset >= this.length) {
                    throw new System.OverflowException();
                }

                return ref UnsafeUtility.AsRef<T>((void*)(this.data + offset));
                
            }

        }

        [MethodImpl(256)]
        public readonly ref T RefUnmanaged<T>(MemPtr ptr) where T : struct {

            lock (this.sync)
            {

                var offset = ptr.value;
                if (offset >= this.length) {
                    throw new System.OverflowException();
                }

                return ref UnsafeUtility.AsRef<T>((void*)(this.data + ptr.value));

            }

        }

        [MethodImpl(256)]
        public readonly ref T RefArray<T>(MemPtr ptr, int index) where T : unmanaged {

            lock (this.sync)
            {

                var size = sizeof(T);
                var offset = ptr.value + size * index;
                if (offset >= this.length) {
                    throw new System.OverflowException();
                }

                return ref UnsafeUtility.AsRef<T>((void*)(this.data + offset));

            }

        }

        [MethodImpl(256)]
        public MemPtr ReAllocArray<T>(MemPtr ptr, int newLength, ClearOptions options = ClearOptions.ClearMemory) where T : unmanaged {

            lock (this.sync)
            {

                var size = sizeof(T);
                return this.ReAlloc_INTERNAL(ptr, size * newLength, options);

            }

        }

        [MethodImpl(256)]
        public MemPtr AllocArray<T>(int length) where T : unmanaged {

            lock (this.sync)
            {

                var size = sizeof(T);
                return this.Alloc_INTERNAL(size * length);

            }

        }

        [MethodImpl(256)]
        public MemPtr Alloc<T>() where T : unmanaged {

            lock (this.sync)
            {

                var size = sizeof(T);
                return this.Alloc_INTERNAL(size);

            }

        }

        [MethodImpl(256)]
        public MemPtr Alloc(long size) {

            lock (this.sync)
            {

                return this.Alloc_INTERNAL(size);

            }

        }

        [MethodImpl(256)]
        public readonly ref T RefArrayUnmanaged<T>(MemPtr ptr, int index) where T : struct {

            lock (this.sync)
            {

                var size = UnsafeUtility.SizeOf<T>();
                var offset = ptr.value + size * index;
                if (offset >= this.length) {
                    throw new System.OverflowException();
                }

                return ref UnsafeUtility.AsRef<T>((void*)(this.data + offset));

            }

        }

        [MethodImpl(256)]
        public MemPtr ReAllocArrayUnmanaged<T>(MemPtr ptr, int newLength, ClearOptions options = ClearOptions.ClearMemory) where T : struct {

            lock (this.sync)
            {

                var size = UnsafeUtility.SizeOf<T>();
                return this.ReAlloc_INTERNAL(ptr, size * newLength, options);

            }

        }

        [MethodImpl(256)]
        public MemPtr AllocArrayUnmanaged<T>(int length) where T : struct {

            lock (this.sync)
            {

                var size = UnsafeUtility.SizeOf<T>();
                return this.Alloc_INTERNAL(size * length);

            }

        }

        [MethodImpl(256)]
        public MemPtr AllocUnmanaged<T>() where T : struct {

            lock (this.sync)
            {

                var size = UnsafeUtility.SizeOf<T>();
                return this.Alloc_INTERNAL(size);

            }

        }

        [MethodImpl(256)]
        public bool Free(MemPtr ptr) {

            lock (this.sync)
            {

                return this.Free_INTERNAL(ptr);

            }

        }

        public bool Free_INTERNAL(MemPtr ptr) {

            if (this.allocatedIndex.TryGetValue(ptr.value, out var idx) == true) {

                var pointer = this.allocated.innerArray[idx];
                {
                    if (pointer.prev != -1 && this.IsFree(pointer.prev, out var prevPointer, out var prevIdx) == true) {
                        // merge with prev pointer
                        pointer.prev = prevPointer.prev;
                        pointer.index = prevPointer.index;
                        pointer.size += prevPointer.size;
                        this.RemoveAtFast(this.free, prevIdx, this.freeIndex, prevPointer.index);
                        if (pointer.next != -1 && this.IsAlloc(pointer.next, out var nextPointer, out var nextIdx) == true) {
                            nextPointer.prev = pointer.index;
                            this.allocated.innerArray[nextIdx] = nextPointer;
                            this.allocatedIndex[nextPointer.index] = nextIdx;
                        }
                    }
                }

                {
                    if (pointer.next != -1 && this.IsFree(pointer.next, out var nextPointer, out var nextIdx) == true) {
                        // merge with next pointer
                        pointer.next = nextPointer.next;
                        pointer.size += nextPointer.size;
                        this.RemoveAtFast(this.free, nextIdx, this.freeIndex, nextPointer.index);
                        if (pointer.next != -1 && this.IsAlloc(pointer.next, out nextPointer, out nextIdx) == true) {
                            nextPointer.prev = pointer.index;
                            this.allocated.innerArray[nextIdx] = nextPointer;
                            this.allocatedIndex[nextPointer.index] = nextIdx;
                        }
                    }
                }

                this.RemoveAtFast(this.allocated, idx, this.allocatedIndex, ptr.value);
                //pointer.isFree = true
                this.freeIndex[pointer.index] = this.free.Count;
                this.free.Add(pointer);
                return true;

            }

            return false;

        }

        public MemPtr ReAlloc(MemPtr ptr, long size, ClearOptions options) {
            
            lock (this.sync)
            {
                
                return this.ReAlloc_INTERNAL(ptr, size, options);

            }

        }
        
        private MemPtr ReAlloc_INTERNAL(MemPtr ptr, long size, ClearOptions options) {
            
            if (ptr.IsValid() == false) {
                return this.Alloc_INTERNAL(size, options: options);
            }

            var prevLength = 0L;
            if (this.allocatedIndex.TryGetValue(ptr.value, out var idx) == true) {

                ref var pointer = ref this.allocated.innerArray[idx];
                // if we have the next block and its free
                if (pointer.next != -1 && this.IsFree(pointer.next, out var nextBlock, out var nextIdx) == true) {
                    // use next block size
                    if (nextBlock.size + pointer.size > size) {
                        {
                            this.freeIndex.Remove(nextBlock.index);
                            nextBlock.index += (int)(size - pointer.size);
                            nextBlock.size = size - pointer.size;
                            this.free.innerArray[nextIdx] = nextBlock;
                            this.freeIndex[nextBlock.index] = nextIdx;

                            pointer.size = size;
                        }
                        // return the same pointer back
                        return ptr;
                    }
                }

                prevLength = pointer.size;

            }

            var newPtr = this.Alloc_INTERNAL(size, options: options);
            this.MemCopy(newPtr, 0, ptr, 0, prevLength);
            this.Free_INTERNAL(ptr);
            return newPtr;

        }

        [MethodImpl(256)]
        public void MemCopy(MemPtr dest, long destOffset, MemPtr source, long sourceOffset, long length) {

            if (dest.IsValid() == false) {
                throw new System.NullReferenceException();
            }

            if (source.IsValid() == false) {
                throw new System.NullReferenceException();
            }

            var offsetDest = dest.value + destOffset;
            if (offsetDest + length >= this.length) {
                throw new System.OverflowException();
            }

            var offsetSrc = source.value + sourceOffset;
            if (offsetSrc + length >= this.length) {
                throw new System.OverflowException();
            }

            UnsafeUtility.MemCpy(
                (void*)(this.data + (int)offsetDest),
                (void*)(this.data + (int)offsetSrc),
                length);
            
        }

        [MethodImpl(256)]
        public void MemClear(MemPtr dest, long destOffset, long length) {

            if (dest.IsValid() == false) {
                throw new System.NullReferenceException();
            }
            
            var offsetDest = dest.value + destOffset;
            if (offsetDest + length >= this.length) {
                throw new System.OverflowException();
            }

            UnsafeUtility.MemClear(
                (void*)(this.data + (int)offsetDest),
                length);
            
        }

        public void Prepare(long size) {

            int headPointerIdx = -1;
            Block headBlock = default;
            for (int i = 0, cnt = this.free.Count; i < cnt; ++i) {

                var ptr = this.free.innerArray[i];
                if (size < ptr.size) {

                    return;

                }
                
                if (ptr.next == -1) {
                    headBlock = ptr;
                    headPointerIdx = i;
                }

            }
            
            this.Reallocate_INTERNAL(size, headPointerIdx, in headBlock);
            
        }

        private MemPtr Alloc_INTERNAL(long size, bool throwExceptionOnOverflow = false, ClearOptions options = ClearOptions.ClearMemory) {

            UnityEngine.Assertions.Assert.IsTrue(size > 0L);
            
            // lookup for free pointers with required size
            int headPointerIdx = -1;
            Block headBlock = default;
            for (int i = 0, cnt = this.free.Count; i < cnt; ++i) {

                var ptr = this.free.innerArray[i];
                if (size < ptr.size) {

                    var memPtr = new MemPtr(ptr.index);
                    {
                        // if block is not full
                        // create new free pointer
                        var freePointer = new Block() {
                            index = ptr.index + (int)size,
                            size = ptr.size - size,
                            next = ptr.next,
                            prev = ptr.index,
                            //isFree = true,
                        };
                        this.freeIndex.Remove(ptr.index);
                        this.free.innerArray[i] = freePointer;
                        this.freeIndex[freePointer.index] = i;
                        //ptr.isFree = false;
                        ptr.next = freePointer.index;
                        ptr.size = size;
                    }
                    
                    this.allocatedIndex[ptr.index] = this.allocated.Count;
                    this.allocated.Add(ptr);
                    
                    if (options == ClearOptions.ClearMemory) {
                        this.MemClear(memPtr, 0, size);
                    }

                    return memPtr;

                }
                
                if (ptr.next == -1) {
                    headBlock = ptr;
                    headPointerIdx = i;
                }

            }

            if (throwExceptionOnOverflow == true) {
                throw new System.OutOfMemoryException($"Overflow with size: {size}");
            }

            this.Reallocate_INTERNAL(size, headPointerIdx, in headBlock);
            
            return this.Alloc_INTERNAL(size, throwExceptionOnOverflow: true);

        }

        [MethodImplAttribute(256)]
        private void Reallocate_INTERNAL(long size, int headPointerIdx, in Block headBlock) {
            
            UnityEngine.Assertions.Assert.IsTrue(headPointerIdx >= 0);
            UnityEngine.Assertions.Assert.IsTrue(headBlock.size > 0);

            // no free pointers found
            // re-alloc data with required size
            // use headPointer to resize data
            //headPointer.isFree = false;
            long sizeWithOffset;
            {
                if (this.length >= int.MaxValue / 2) {
                    sizeWithOffset = size * 2;
                } else {
                    sizeWithOffset = (size >= this.length ? size : this.length) * 3L;
                    if (sizeWithOffset % 2L == 0) {
                        sizeWithOffset /= 2L;
                    } else {
                        sizeWithOffset += 1L;
                        sizeWithOffset /= 2L;
                    }
                }
            }

            var newSize = headBlock.index + headBlock.size + sizeWithOffset;
            
            if (newSize > int.MaxValue) {
                throw new System.OutOfMemoryException($"Size max: {int.MaxValue}, prev size: {this.length}, trying to allocate: {size}");
            }

            if (this.maxSize > 0L && newSize > this.maxSize) {
                throw new System.OutOfMemoryException($"Size: {newSize}/{this.maxSize}");
            }
            
            if (newSize < 0L) {
                throw new System.OutOfMemoryException($"Internal error while allocating {newSize}:{sizeWithOffset} bytes (requested size: {size}, length: {this.length}, headBlock.index: {headBlock.index}, headBlock.size: {headBlock.size})");
            }

            if (newSize <= this.length) {
                throw new System.OutOfMemoryException($"Internal error while allocating {newSize}:{sizeWithOffset} bytes (requested size: {size}, length: {this.length}, headBlock.index: {headBlock.index}, headBlock.size: {headBlock.size})");
            }
            
            UnityEngine.Debug.Log($"RE_ALLOC: {this.length} -> {newSize}, requested {size}, free head size: {headBlock.size}");
            var newData = UnsafeUtility.Malloc(newSize, UnsafeUtility.AlignOf<byte>(), Unity.Collections.Allocator.Persistent);
            if (this.data != System.IntPtr.Zero) {
                UnsafeUtility.MemCpy(newData, (void*)this.data, this.length);
                UnsafeUtility.Free((void*)this.data, Unity.Collections.Allocator.Persistent);
            }
            
            this.length = newSize;
            this.data = (System.IntPtr)newData;
            var newBlockSize = newSize - headBlock.index;
            this.free.innerArray[headPointerIdx].size = newBlockSize;

        }

        [MethodImplAttribute(256)]
        private void RemoveAtFast(ListCopyable<Block> list, int index, DictionaryInt<int> indexer, int indexerValue) {

            var targetIndex = list.Count - 1;
            if (targetIndex == index) {
                list.RemoveAtFast(index);
                indexer.Remove(indexerValue);
            } else {
                var last = list.innerArray[targetIndex];
                list.RemoveAtFast(index);
                indexer.Remove(indexerValue);
                indexer[last.index] = index;
            }

        }

        [MethodImplAttribute(256)]
        private bool IsFree(int index, out Block block, out int idx) {

            block = default;
            if (this.freeIndex.TryGetValue(index, out idx) == false) return false;
            block = this.free.innerArray[idx];
            return true;

        }

        [MethodImplAttribute(256)]
        private bool IsAlloc(int index, out Block block, out int idx) {

            block = default;
            if (this.allocatedIndex.TryGetValue(index, out idx) == false) return false;
            block = this.allocated.innerArray[idx];
            return true;

        }

        [MethodImplAttribute(256)]
        public void* GetUnsafePtr(in MemPtr ptr) {

            return (void*)(this.data + ptr.value);

        }

        public bool IsValid() => this.isValid;

    }

}