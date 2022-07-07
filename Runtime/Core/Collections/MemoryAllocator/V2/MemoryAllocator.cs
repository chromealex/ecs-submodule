namespace ME.ECS.Collections.V2 {
    
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Collections;
    
    using word_t = System.UInt64;
    using size_t = System.Int64;
    using ptr = System.Int64;
    using MemPtr = System.Int64;

    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
    #endif
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
            
            // 4 MB of persistent memory + no max size
            StaticAllocators.persistent = new MemoryAllocator().Initialize(4 * 1024 * 1024, -1);
            
            // 256 KB of temp memory + max size = 256 KB
            StaticAllocators.temp = new MemoryAllocator().Initialize(256 * 1024, 256 * 1024);
            
        }

        private sealed class Destructor {
            ~Destructor() {
                StaticAllocators.persistent.Dispose();
                StaticAllocators.temp.Dispose();
            }
        }

    }

    public class MemoryAllocatorProxyDebugger {

        private MemoryAllocator allocator;
        
        public MemoryAllocatorProxyDebugger(MemoryAllocator allocator) {

            this.allocator = allocator;

        }

        public MemoryAllocator.Block[] allBlocks {
            get {
                var blocks = new System.Collections.Generic.List<MemoryAllocator.Block>();
                var blockAddr = this.allocator.heapStart;
                while (blockAddr != 0) {
                    var block = this.allocator.GetBlock(blockAddr);
                    blocks.Add(block);
                    blockAddr = block.nextBlockPtr;
                }
                return blocks.ToArray();
            }
        }

        public MemoryAllocator.Block[] freeBlocks {
            get {
                var blocks = new System.Collections.Generic.List<MemoryAllocator.Block>();
                var list = this.allocator.freeList;
                foreach (var item in list) {
                    var block = this.allocator.GetBlock(item);
                    blocks.Add(block);
                }
                return blocks.ToArray();
            }
        }

    }

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(MemoryAllocatorProxyDebugger))]
    public unsafe partial struct MemoryAllocator : IMemoryAllocator<MemoryAllocator, MemPtr> {

        internal const size_t BLOCK_HEADER_SIZE = sizeof(size_t) + sizeof(ptr) + sizeof(ptr) + sizeof(ptr) + sizeof(int)
                                                  // + paddings
                                                  + sizeof(int) + sizeof(long) + sizeof(long) + sizeof(long);
        internal const size_t ALLOCATOR_HEADER_SIZE = 64; //sizeof(word_t);

        private enum SearchMode {

            FirstFit,
            NextFit,
            FreeList,

        }
        
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Block {

            public size_t dataSize;
            public ptr nextBlockPtr; // next index block in data array
            public ptr blockHeadPtr; // index in data array
            public ptr prevBlockPtr; // prev index block in data array
            public int freeIndex;
            // do not use, just for alignment
            private readonly int padding;
            private readonly long padding1;
            private readonly long padding2;
            private readonly long padding3;

            public size_t fullSize => this.dataSize + BLOCK_HEADER_SIZE;
            public ptr blockDataPtr => this.blockHeadPtr + BLOCK_HEADER_SIZE;
            public ptr blockEndPtr => this.blockDataPtr + this.dataSize;

            public override string ToString() {
                
                return $"B: {this.dataSize} head: {this.blockHeadPtr} prev: {this.prevBlockPtr} next: {this.nextBlockPtr} end: {this.blockEndPtr} freeIndex: {this.freeIndex}";
                
            }

            public MemPtr GetMemPtr() {

                return (MemPtr)this.blockHeadPtr;

            }

            public bool IsValid() {
                return this.freeIndex == 0 &&
                       this.dataSize > 0 &&
                       (this.nextBlockPtr == 0 || this.nextBlockPtr > this.blockHeadPtr);
            }

        }
        
        // [Block][Block]...
        // [Block:
        //   Header
        //   Data
        // ]
        private void* data;
        private size_t currentSize;
        private size_t allocatedSize;
        private size_t maxSize;

        internal ptr heapStart;
        internal ptr top;
        internal ListCopyable<ptr> freeList;

        private SearchMode searchMode;

        private void* AllocData_INTERNAL(size_t size) {

            // we need to add ALLOCATOR_HEADER_SIZE to the size to skip first byte
            var data = UnsafeUtility.Malloc(size + ALLOCATOR_HEADER_SIZE, UnsafeUtility.AlignOf<byte>(), Allocator.Persistent);
            this.allocatedSize = size;
            if (data == null) {
                throw new System.OutOfMemoryException();
            }

            return data;
            
        }

        private void ReAllocData_INTERNAL(size_t size, ClearOptions options) {

            size += BLOCK_HEADER_SIZE;
            if (this.heapStart == 0L) {
                size += ALLOCATOR_HEADER_SIZE;
            }

            var dt = (size - this.currentSize) * 2;
            size += dt;
            var sizeWithOffset = this.currentSize * 2L;
            if (size < sizeWithOffset) size = sizeWithOffset;

            size = Align(size);
            
            //UnityEngine.Debug.Log($"realloc_data: {this.currentSize} => {size} with top {this.top}");

            var newData = this.AllocData_INTERNAL(size);
            if (this.heapStart != 0L) {
                UnityEngine.Assertions.Assert.IsTrue(this.currentSize < size);
                UnsafeUtility.MemCpy(newData, this.data, this.currentSize);
                this.FreeData_INTERNAL(this.data);
                this.data = newData;
                // seek for last block
                ref var topBlock = ref this.GetBlock(this.top);
                if (topBlock.freeIndex > 0) {
                    // current top block is free,
                    // so just resize it
                    var delta = size - this.currentSize;
                    this.currentSize = topBlock.blockEndPtr + delta;
                    if (options == ClearOptions.ClearMemory) {
                        this.MemClear((MemPtr)topBlock.blockEndPtr, 0, delta);
                    }
                    
                    //UnityEngine.Debug.Log($"resize_top_block: {topBlock.dataSize} += {delta}, new size: {this.currentSize}");
                    topBlock.dataSize += delta;
                } else {
                    // current top block is not free
                    // create new empty block
                    var block = new Block() {
                        blockHeadPtr = topBlock.blockEndPtr,
                        dataSize = size - this.currentSize - BLOCK_HEADER_SIZE,
                        freeIndex = 1,
                        nextBlockPtr = 0L,
                        prevBlockPtr = topBlock.blockHeadPtr,
                    };
                    this.currentSize = block.blockEndPtr;
                    // connect with current top
                    topBlock.nextBlockPtr = block.blockHeadPtr;
                    // set new block as top
                    this.top = block.blockHeadPtr;
                    
                    if (this.searchMode == SearchMode.FreeList) {
                        block.freeIndex = this.freeList.Count + 1;
                        this.freeList.Add(this.top);
                    }
                
                    if (options == ClearOptions.ClearMemory) {
                        this.MemClear((MemPtr)block.blockDataPtr, 0L, block.dataSize);
                    }
                    
                    this.GetBlock(this.top) = block;
                    //UnityEngine.Debug.Log($"create_new_block: {block.dataSize}, block head ptr: {block.blockHeadPtr}, block data ptr: {block.blockDataPtr}, block end ptr: {block.blockEndPtr}");
                }
            } else {
                this.data = newData;
                // create first empty block
                var block = new Block() {
                    blockHeadPtr = this.top,
                    dataSize = size - BLOCK_HEADER_SIZE - ALLOCATOR_HEADER_SIZE,
                    freeIndex = 1,
                    nextBlockPtr = 0L,
                    prevBlockPtr = 0L,
                };
                this.currentSize = block.blockEndPtr;
                this.heapStart = this.top;

                //UnityEngine.Debug.Log($"create_first_block: {block.dataSize}, block head ptr: {block.blockHeadPtr}, block data ptr: {block.blockDataPtr}, block end ptr: {block.blockEndPtr}");
                
                if (this.searchMode == SearchMode.FreeList) {
                    block.freeIndex = this.freeList.Count + 1;
                    this.freeList.Add(this.top);
                }
                
                if (options == ClearOptions.ClearMemory) {
                    this.MemClear((MemPtr)block.blockDataPtr, 0L, block.dataSize);
                }
                
                this.GetBlock(this.top) = block;
            }

            UnityEngine.Assertions.Assert.AreEqual(this.currentSize, this.allocatedSize);

        }

        private void FreeData_INTERNAL(void* ptr) {
            
            UnsafeUtility.Free(ptr, Allocator.Persistent);
            
        }
        
        public MemoryAllocator Initialize(size_t initialSize, size_t maxSize = -1L) {

            if (Worlds.current == null) Worlds.current = new World();
            if (Worlds.current.worldThread == null) Worlds.current.worldThread = System.Threading.Thread.CurrentThread;
            this.maxSize = maxSize;
            // May be we need to use bidirectional list instead of one-dir,
            // so we don't need to be use freeList heap list at all
            this.freeList = PoolListCopyable<ptr>.Spawn(10);
            this.heapStart = 0L;
            // start from offset ALLOCATOR_HEADER_SIZE because ALLOCATOR_HEADER_SIZE is allocator system header
            this.top = ALLOCATOR_HEADER_SIZE;
            this.searchMode = SearchMode.FreeList;
            this.ReAllocData_INTERNAL(initialSize, ClearOptions.UninitializedMemory);
            UnsafeUtility.MemClear(this.data, ALLOCATOR_HEADER_SIZE);
            
            return this;

        }

        public void Dispose() {

            PoolListCopyable<ptr>.Recycle(ref this.freeList);
            this.FreeData_INTERNAL(this.data);
            this = default;

        }

        public void CopyFrom(in MemoryAllocator other) {

            if (this.currentSize != other.currentSize) {
                
                this.FreeData_INTERNAL(this.data);
                this.data = this.AllocData_INTERNAL(other.currentSize);

            }
            
            UnsafeUtility.MemCpy(this.data, other.data, other.currentSize);
            this.allocatedSize = other.allocatedSize;
            this.currentSize = other.currentSize;
            this.heapStart = other.heapStart;
            this.maxSize = other.maxSize;
            this.searchMode = other.searchMode;
            this.top = other.top;
            ArrayUtils.Copy(other.freeList, ref this.freeList);
            
        }

        //
        // Base
        //
        
        private static size_t Align(size_t x) {
            var newSize = (x + sizeof(word_t) - 1) & ~(sizeof(word_t) - 1);
            var align = newSize - x;
            align = 1 << ((byte)(32 - Unity.Mathematics.math.lzcnt(Unity.Mathematics.math.max(1, align) - 1)));
            var alignment = Unity.Mathematics.math.max(Unity.Jobs.LowLevel.Unsafe.JobsUtility.CacheLineSize, align);
            return x + (alignment - x % alignment);
        }
        
        private static bool CanSplit(ref Block block, size_t size) {
            return block.dataSize >= (size + BLOCK_HEADER_SIZE);
        }

        private size_t Split(ref Block freeBlock, size_t size, ClearOptions options) {
            // Calculate new free size
            var newFreeSize = freeBlock.dataSize - size - BLOCK_HEADER_SIZE;
            // Check if new size fits the new block
            if (newFreeSize > 0) {
                // create free block
                var freePart = new Block {
                    dataSize = newFreeSize,
                    freeIndex = 1,
                    nextBlockPtr = freeBlock.nextBlockPtr,
                    prevBlockPtr = freeBlock.blockHeadPtr,
                    blockHeadPtr = freeBlock.blockHeadPtr + size + BLOCK_HEADER_SIZE,
                };
                if (freePart.blockHeadPtr > this.top) {
                    // move top if this is the last block
                    this.top = freePart.blockHeadPtr;
                }
                if (this.searchMode == SearchMode.FreeList) {
                    freePart.freeIndex = this.freeList.Count + 1;
                    this.freeList.Add(freePart.blockHeadPtr);
                }
                // write free part
                this.GetBlock(freePart.blockHeadPtr) = freePart;
                // split current block, change its size
                // mute free block to allocated block
                freeBlock.dataSize = size;
                freeBlock.nextBlockPtr = freePart.blockHeadPtr;
                if (options == ClearOptions.ClearMemory) {
                    this.MemClear((MemPtr)freeBlock.blockDataPtr, 0L, freeBlock.dataSize);
                }
            } else if (newFreeSize <= BLOCK_HEADER_SIZE) {
                size += BLOCK_HEADER_SIZE;
            } else {
                // if target size is zero - we don't need to create new free block
                // nothing to do here
            }

            return size;
        }

        private ref Block ListAllocate(ref Block block, size_t size, ClearOptions options) {
            // Split the larger block, reusing the free part.
            if (MemoryAllocator.CanSplit(ref block, size) == true) {
                size = this.Split(ref block, size, options);
            }

            block.freeIndex = 0;
            block.dataSize = size;
            return ref block;
        }
        
        public MemPtr Alloc(size_t size) {

            return this.Alloc(size, ClearOptions.ClearMemory);

        }

        public MemPtr Alloc(size_t size, ClearOptions options) {

            if (System.Threading.Thread.CurrentThread != Worlds.current.worldThread) {
                throw new System.Exception();
            }
            
            size = MemoryAllocator.Align(size);
            //UnityEngine.Debug.Log("MEM_ALLOC: " + size);

            // ---------------------------------------------------------
            // 1. Search for a free block in the free-list:
            //
            // Traverse the blocks list, searching for a block of
            // the appropriate size
            if (this.FindBlock(size, out var block, options) == true) {
                return block.GetMemPtr();
            }

            // ---------------------------------------------------------
            // 2. If block not found in the free list, request from OS:
            // No block found, request to map more memory from the OS,
            // bumping the program break (brk).
            var newSize = this.currentSize + size + BLOCK_HEADER_SIZE;
            this.ReAllocData_INTERNAL(newSize, options);

            // Now we have enough data size to allocate,
            // so just run Alloc method again
            return this.Alloc(size, options);
            
        }

        internal readonly ref Block GetBlock(ptr ptr) {
            if (ptr <= 0L || ptr >= this.currentSize) {
                throw new System.OverflowException("ptr: " + ptr);
            }

            return ref UnsafeUtility.AsRef<Block>((void*)((ptr)this.data + ptr));
        }
        
        internal readonly ref T GetBlockData<T>(ptr ptr, ptr offset) where T : struct {
            if (ptr <= 0L || ptr + BLOCK_HEADER_SIZE + offset >= this.currentSize) {
                throw new System.OverflowException("ptr: " + ptr + ", offset: " + offset);
            }
            return ref UnsafeUtility.AsRef<T>((void*)((ptr)this.data + ptr + BLOCK_HEADER_SIZE + offset));
        }

        private bool CanCoalesce(ref Block block) {
            return block.nextBlockPtr != 0L && this.GetBlock(block.nextBlockPtr).freeIndex > 0;
        }

        private void Coalesce(ref Block block, bool removeFromList = true) {

            if (block.nextBlockPtr == this.top) {
                this.top = block.blockHeadPtr;
            }
            
            var next = this.GetBlock(block.nextBlockPtr);
            block.nextBlockPtr = next.nextBlockPtr;
            block.dataSize += BLOCK_HEADER_SIZE + next.dataSize;

            if (next.blockHeadPtr != 0L) {
                ref var nextNext = ref this.GetBlock(next.blockHeadPtr);
                nextNext.prevBlockPtr = block.blockHeadPtr;
            }

            if (removeFromList == true && this.searchMode == SearchMode.FreeList) {
                this.RemoveFreeIndex(next.freeIndex - 1);
            }

        }

        private void RemoveFreeIndex(int index) {
            
            this.freeList.RemoveAtFast(index, out var movedPtr);
            ref var moved = ref this.GetBlock(movedPtr);
            moved.freeIndex = index + 1;
            
        }

        public bool Free(MemPtr ptr) {
            if (System.Threading.Thread.CurrentThread != Worlds.current.worldThread) {
                throw new System.Exception();
            }
            if (ptr == 0L) return false;
            if ((ptr)ptr % 64 != 0) return false;
            ref var block = ref this.GetBlock((ptr)ptr);
            if (block.IsValid() == false) return false;
            if (this.CanCoalesce(ref block) == true) {
                this.Coalesce(ref block);
            }
            block.freeIndex = 1;
            if (this.searchMode == SearchMode.FreeList) {
                block.freeIndex = this.freeList.Count + 1;
                this.freeList.Add(block.blockHeadPtr);
            }
            return true;
        }

        public readonly ref T Ref<T>(MemPtr ptr) where T : unmanaged {
            return ref this.RefUnmanaged<T>(ptr);
        }

        public readonly ref T RefUnmanaged<T>(MemPtr ptr) where T : struct {
            if ((ptr)ptr <= 0L || (ptr)ptr + BLOCK_HEADER_SIZE >= this.allocatedSize) {
                throw new System.ArgumentOutOfRangeException(nameof(ptr), ptr, $"pointer address is out of range {0}..{this.allocatedSize}");
            }

            if ((ptr)ptr % 64 != 0) {
                throw new System.Exception("ptr is not aligned correctly");
            }

            var block = this.GetBlock((ptr)ptr);
            if (block.blockHeadPtr <= 0L || block.blockHeadPtr + block.dataSize >= this.allocatedSize) {
                throw new OutOfBoundsException();
            }
            return ref UnsafeUtility.AsRef<T>((void*)((ptr)this.data + (ptr)ptr + BLOCK_HEADER_SIZE));
        }

        public MemPtr Alloc<T>() where T : unmanaged {
            var size = sizeof(T);
            var alignOf = UnsafeUtility.AlignOf<T>();
            return this.Alloc(size + alignOf);
        }

        public MemPtr AllocUnmanaged<T>() where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            var alignOf = UnsafeUtility.AlignOf<T>();
            return this.Alloc(size + alignOf);
        }

        public MemPtr ReAlloc(MemPtr ptr, size_t newSize, ClearOptions options) {

            if (ptr == 0L) {

                return this.Alloc(newSize, options);

            }
            var block = this.GetBlock((ptr)ptr);
            if (newSize <= block.dataSize) return ptr;

            // TODO: Implement the faster realloc
            // try to find fast way to realloc
            /*if (block.nextBlockPtr != 0L) {
                // we just need to check next block
                // if it has used flag as not set - try use it
                var requestedSize = newSize - block.dataSize;
                var next = this.GetBlock(block.nextBlockPtr);
                if (next.usedFlag == 0 && next.fullSize >= requestedSize) {
                    
                    // if next block is free and size enough for the new alloc
                    block.nextBlockPtr = next.nextBlockPtr;
                    
                    var lastPoint = next.blockEndPtr;
                    if (options == ClearOptions.ClearMemory) {
                        this.MemClear((MemPtr)block.blockDataPtr, block.dataSize, lastPoint - block.dataSize);
                    }
                    block.dataSize = lastPoint - block.blockDataPtr;
                    block = this.listAllocate(ref block, newSize);
                    return block.GetMemPtr();
                    
                }
                //var requestedSize = newSize - block.dataSize;
                while (requestedSize > 0L) {
                    if (next.usedFlag == 1) {
                        break;
                    }
                    // we can use next block (data size + header size)
                    requestedSize -= next.dataSize + BLOCK_HEADER_SIZE;
                    next = this.GetBlock(block.nextBlockPtr);
                }

                if (requestedSize <= 0L) {
                    // we have found block to stop
                    block.nextBlockPtr = next.nextBlockPtr;
                    var lastPoint = next.blockEndPtr;
                    if (options == ClearOptions.ClearMemory) {
                        this.MemClear((MemPtr)block.blockDataPtr, block.dataSize, lastPoint - block.dataSize);
                    }
                    block.dataSize = lastPoint - block.blockDataPtr;
                    block = this.listAllocate(ref block, newSize);
                    return block.GetMemPtr();
                }
            }*/

            // slow re-alloc
            var newPtr = this.Alloc(newSize, ClearOptions.UninitializedMemory);
            this.MemCopy(newPtr, BLOCK_HEADER_SIZE, ptr, BLOCK_HEADER_SIZE, block.dataSize);
            if (options == ClearOptions.ClearMemory) {
                this.MemClear(newPtr, BLOCK_HEADER_SIZE + block.dataSize, newSize - block.dataSize);
            }
            this.Free(ptr);
            return newPtr;

        }

        public void MemCopy(MemPtr dest, size_t destOffset, MemPtr source, size_t sourceOffset, size_t length) {
            
            UnityEngine.Assertions.Assert.IsTrue(length > 0L);
            
            /*if (dest == null) {
                throw new System.NullReferenceException();
            }

            if (source == null) {
                throw new System.NullReferenceException();
            }*/

            var offsetDest = (ptr)dest + destOffset;
            if (offsetDest + length >= this.currentSize) {
                throw new System.OverflowException();
            }

            var offsetSrc = (ptr)source + sourceOffset;
            if (offsetSrc + length >= this.currentSize) {
                throw new System.OverflowException();
            }

            if (offsetDest < 0L || offsetSrc < 0L) {
                throw  new OutOfBoundsException();
            }
            
            UnsafeUtility.MemCpy(
                (void*)((ptr)this.data + offsetDest),
                (void*)((ptr)this.data + offsetSrc),
                length);

        }

        public void MemClear(MemPtr dest, size_t destOffset, size_t length) {
            
            UnityEngine.Assertions.Assert.IsTrue(length > 0L);
            
            /*if (dest == null) {
                throw new System.NullReferenceException();
            }*/
            
            var offsetDest = (ptr)dest + destOffset;
            if (offsetDest + length > this.currentSize) {
                throw new System.OverflowException();
            }

            if (offsetDest < 0L) {
                throw  new OutOfBoundsException();
            }

            UnsafeUtility.MemClear(
                (void*)((ptr)this.data + offsetDest),
                length);

        }

        public void Prepare(size_t size) {
            this.ReAllocData_INTERNAL(size, ClearOptions.UninitializedMemory);
        }

        public void* GetUnsafePtr(in MemPtr ptr) {
            return (void*)((ptr)this.data + (ptr)ptr);
        }

        //
        // Find block
        // Algorithms
        // 
        
        private bool FindBlock(size_t size, out Block block, ClearOptions options) {

            return this.FreeListAlgorithm(size, out block, options);

        }

        private bool FreeListAlgorithm(size_t size, out Block block, ClearOptions options) {

            block = default;
            for (int i = 0; i < this.freeList.Count; ++i) {

                var ptr = this.freeList[i];
                ref var b = ref this.GetBlock(ptr);
                if (b.dataSize < size) {
                    continue;
                }
                
                UnityEngine.Assertions.Assert.IsTrue(b.freeIndex > 0);
                
                this.RemoveFreeIndex(i);
                block = this.ListAllocate(ref b, size, options);
                return true;

            }
            
            return false;
            
        }
        
        /*
        private bool FirstFitAlgorithm(size_t size, ref Block found) {

            var blockIdx = this.heapStart;
            while (blockIdx != 0) {
                // O(n) search.
                ref var block = ref this.GetBlock(blockIdx);
                if (block.usedFlag == 1 || block.dataSize < size) {
                    blockIdx = block.nextBlockPtr;
                    continue;
                }

                // Found the block:
                found = this.ListAllocate(ref block, size);
                return true;
            }

            return false;
            
        }
        */

    }

}