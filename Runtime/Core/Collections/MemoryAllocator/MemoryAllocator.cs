namespace ME.ECS.Collections {

    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public struct MemBlockArray<T> where T : unmanaged {

        public MemPtr ptr;
        public int Length;

        public MemBlockArray(ref MemoryAllocator allocator, int length) {

            this.ptr = allocator.AllocArray<T>(length);
            this.Length = length;

        }

        public void Dispose(ref MemoryAllocator allocator) {

            allocator.Free(this.ptr);
            this.ptr = default;

        }

        public ref T this[in MemoryAllocator allocator, int index] => ref allocator.RefArray<T>(this.ptr, index);

        public bool Resize(ref MemoryAllocator allocator, int newLength) {

            if (newLength <= this.Length) {

                return false;
                
            }
            
            this.ptr = allocator.ReallocArray<T>(this.ptr, newLength);
            return true;

        }
        
    }
    
    public struct Block {

        public int index;
        public int size;
        
        // next ptr to data[] (MemPtr)
        public int next;
        // prev ptr to data[] (MemPtr)
        public int prev;

    }

    /// <summary>
    /// Pointer to MemoryAllocator::data (index)
    /// </summary>
    public readonly struct MemPtr {

        public readonly int value;

        internal MemPtr(int value) {
            this.value = value;
        }

    }
    
    public unsafe struct MemoryAllocator {

        private ListCopyable<Block> free;
        private ListCopyable<Block> allocated;
        private DictionaryInt<int> allocatedIndex;
        private DictionaryInt<int> freeIndex;
        private System.IntPtr data;
        private int maxSize;
        private int length;
        
        public void Initialize(int initialSize, int maxSize = -1) {
            
            this.free = new ListCopyable<Block>();
            this.allocated = new ListCopyable<Block>();
            this.allocatedIndex = new DictionaryInt<int>();
            this.freeIndex = new DictionaryInt<int>();
            
            var ptr = new Block() {
                index = 0,
                size = initialSize,
                next = -1,
                prev = -1,
            };
            this.free.Add(ptr);
            this.maxSize = maxSize;
            this.data = Marshal.AllocHGlobal(initialSize);
            this.length = initialSize;

        }

        public void Dispose() {
            
            Marshal.FreeHGlobal(this.data);
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
                    Marshal.FreeHGlobal(this.data);
                } else if (this.data == System.IntPtr.Zero &&
                           other.data != System.IntPtr.Zero) {
                    // create current data and copy from other.data
                    this.data = Marshal.AllocHGlobal(other.length);
                    Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemCpy(
                        (void*)this.data,
                        (void*)other.data,
                        other.length
                        );
                } else {
                    // copy data
                    Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemCpy(
                        (void*)this.data,
                        (void*)other.data,
                        other.length
                    );
                }
            }
            this.maxSize = other.maxSize;
            this.length = other.length;

        }

        public readonly ref T RefArray<T>(MemPtr ptr, int index) where T : unmanaged {

            var size = sizeof(T);
            return ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AsRef<T>((void*)(this.data + ptr.value + size * index));

        }

        public MemPtr ReallocArray<T>(MemPtr ptr, int newLength) where T : unmanaged {
            
            var size = sizeof(T);
            return this.Realloc(ptr, size * newLength);
            
        }

        public MemPtr AllocArray<T>(int length) where T : unmanaged {

            var size = sizeof(T);
            return this.Alloc(size * length);

        }

        public MemPtr Alloc<T>() where T : unmanaged {

            var size = sizeof(T);
            return this.Alloc(size);

        }
        
        public bool Free(MemPtr ptr) {

            if (this.allocatedIndex.TryGetValue(ptr.value, out var idx) == true) {
                
                var pointer = this.allocated.innerArray.arr[idx];
                {
                    if (pointer.prev != -1 && this.IsFree(pointer.prev, out var prevPointer, out var prevIdx) == true) {
                        // merge with prev pointer
                        pointer.prev = prevPointer.prev;
                        pointer.index = prevPointer.index;
                        pointer.size += prevPointer.size;
                        this.RemoveAtFast(this.free, prevIdx, this.freeIndex, prevPointer.index);
                        if (pointer.next != -1 && this.IsAlloc(pointer.next, out var nextPointer, out var nextIdx) == true) {
                            nextPointer.prev = pointer.index;
                            this.allocated.innerArray.arr[nextIdx] = nextPointer;
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
                            this.allocated.innerArray.arr[nextIdx] = nextPointer;
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

        public MemPtr Realloc(MemPtr ptr, int size) {

            var prevLength = 0;
            if (this.allocatedIndex.TryGetValue(ptr.value, out var idx) == true) {

                ref var pointer = ref this.allocated.innerArray.arr[idx];
                // if we have the next block and its free
                /*if (pointer.next != -1 && this.IsFree(pointer.next, out var nextBlock, out var nextIdx) == true) {
                    // use next block size
                    if (nextBlock.size + pointer.size >= size) {
                        if (nextBlock.size + pointer.size == size) {
                            // size matched
                            // resize current pointer
                            pointer.size += nextBlock.size;
                            pointer.next = nextBlock.next;
                            nextBlock.prev = pointer.index;
                            // remove free block
                            this.RemoveAtFast(this.free, nextIdx, this.freeIndex, nextBlock.index);
                        } else {
                            // required size is less
                        }
                        // return the same pointer back
                        return ptr;
                    } else {
                        // TODO: check next free block
                    }
                }*/

                prevLength = pointer.size;

            }

            var newPtr = this.Alloc(size);
            this.MemCopy(newPtr, 0, ptr, 0, prevLength);
            this.Free(ptr);
            return newPtr;

        }

        public void MemCopy(MemPtr dest, int destOffset, MemPtr source, int sourceOffset, int length) {

            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemCpy(
                (void*)(this.data + dest.value + destOffset),
                (void*)(this.data + source.value + sourceOffset),
                length);
            
        }

        public void MemClear(MemPtr dest, int destOffset, int length) {

            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.MemClear(
                (void*)(this.data + dest.value + destOffset),
                length);
            
        }

        public MemPtr Alloc(int size) {

            // lookup for free pointers with required size
            int headPointerIdx = -1;
            Block headBlock = default;
            for (int i = 0; i < this.free.Count; ++i) {

                var ptr = this.free.innerArray.arr[i];
                if (size <= ptr.size) {

                    var memPtr = new MemPtr(ptr.index);
                    if (ptr.size == size) {
                        // if we alloc all block
                        //ptr.isFree = false;
                        this.RemoveAtFast(this.free, i, this.freeIndex, ptr.index);
                    } else {
                        // if block is not full
                        // create new free pointer
                        var freePointer = new Block() {
                            index = ptr.index + size,
                            size = ptr.size - size,
                            next = ptr.next,
                            prev = ptr.index,
                            //isFree = true,
                        };
                        this.freeIndex.Remove(ptr.index);
                        this.free.innerArray.arr[i] = freePointer;
                        this.freeIndex[freePointer.index] = i;
                        //ptr.isFree = false;
                        ptr.next = freePointer.index;
                        ptr.size = size;
                        ptr.next = freePointer.index;
                    }
                    
                    this.allocatedIndex[ptr.index] = this.allocated.Count;
                    this.allocated.Add(ptr);
                    
                    return memPtr;

                }
                if (ptr.next == -1) {
                    headBlock = ptr;
                    headPointerIdx = i;
                }

            }

            // no free pointers found
            // re-alloc data with required size
            // use headPointer to resize data
            //headPointer.isFree = false;
            var newSize = this.length - headBlock.index + size * 2;
            if (this.maxSize > 0 && newSize > this.maxSize) {
                throw new System.OutOfMemoryException($"Size: {newSize}/{this.maxSize}");
            }

            this.data = System.Runtime.InteropServices.Marshal.ReAllocHGlobal(this.data, (System.IntPtr)newSize);

            this.free.innerArray.arr[headPointerIdx].size = newSize;
            
            return this.Alloc(size);

        }

        [MethodImplAttribute(256)]
        private void RemoveAtFast(ListCopyable<Block> list, int index, DictionaryInt<int> indexer, int indexerValue) {

            var targetIndex = list.Count - 1;
            if (targetIndex == index) {
                list.RemoveAtFast(index);
                indexer.Remove(indexerValue);
            } else {
                var last = list.innerArray.arr[targetIndex];
                list.RemoveAtFast(index);
                indexer.Remove(indexerValue);
                indexer[last.index] = index;
            }

        }

        [MethodImplAttribute(256)]
        private bool IsFree(int index, out Block block, out int idx) {

            block = default;
            if (this.freeIndex.TryGetValue(index, out idx) == false) return false;
            block = this.free.innerArray.arr[idx];
            return true;

        }

        [MethodImplAttribute(256)]
        private bool IsAlloc(int index, out Block block, out int idx) {

            block = default;
            if (this.allocatedIndex.TryGetValue(index, out idx) == false) return false;
            block = this.allocated.innerArray.arr[idx];
            return true;

        }

    }

}