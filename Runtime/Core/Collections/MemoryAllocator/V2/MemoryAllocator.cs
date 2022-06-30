namespace ME.ECS.Collections.V2 {
    
    using Unity.Collections.LowLevel.Unsafe;
    using Unity.Collections;
    
    using word_t = System.UIntPtr;
    using size_t = System.Int64;
    using ptr = System.Int64;
    using MemPtr = System.IntPtr;

    [UnityEditor.InitializeOnLoadAttribute]
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

    public unsafe class MemoryAllocatorProxyDebugger {

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

        private const size_t BLOCK_HEADER_SIZE = sizeof(size_t) + sizeof(int) + sizeof(ptr) + sizeof(ptr);

        public enum SearchMode {

            FirstFit,
            NextFit,
            FreeList,

        }
        
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public struct Block {

            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public size_t dataSize;
            [System.Runtime.InteropServices.FieldOffsetAttribute(sizeof(size_t))]
            public int freeIndex;
            [System.Runtime.InteropServices.FieldOffsetAttribute(sizeof(int) + sizeof(size_t))]
            public ptr nextBlockPtr; // next index block in data array
            [System.Runtime.InteropServices.FieldOffsetAttribute(sizeof(int) + sizeof(size_t) + sizeof(size_t))]
            public ptr blockHeadPtr; // index in data array

            public size_t fullSize => this.dataSize + BLOCK_HEADER_SIZE;
            public ptr blockDataPtr => this.blockHeadPtr + BLOCK_HEADER_SIZE;
            public ptr blockEndPtr => this.blockDataPtr + this.dataSize;

            public MemPtr GetMemPtr() {

                return new MemPtr(this.blockHeadPtr);

            }

        }
        
        // [Block][Block]...
        // [Block:
        //   Block Header
        //   Block Data
        // ]
        private void* data;
        private long currentSize;
        private long allocatedSize;
        private long maxSize;

        internal ptr heapStart;
        private ptr top;
        internal ListCopyable<ptr> freeList;

        private SearchMode searchMode;

        private void* AllocData_INTERNAL(size_t size) {

            // we need to add 1L to the size to skip first byte
            var data = UnsafeUtility.Malloc(size + 1L, UnsafeUtility.AlignOf<byte>(), Allocator.Persistent);
            this.allocatedSize = size;
            if (data == null) {
                throw new System.OutOfMemoryException();
            }

            return data;
            
        }

        private ref Block ReAllocData_INTERNAL(size_t size, ClearOptions options) {

            size += BLOCK_HEADER_SIZE;

            var dt = (size - this.currentSize) * 2;
            size += dt;
            var sizeWithOffset = this.currentSize * 2L;
            if (size < sizeWithOffset) size = sizeWithOffset;
            
            //UnityEngine.Debug.Log($"realloc_data: {this.currentSize} => {size}");
            
            var newData = this.AllocData_INTERNAL(size);
            if (this.data != null) {
                UnsafeUtility.MemCpy(newData, this.data, this.currentSize);
                this.data_free_to_os(this.data);
            }

            this.data = newData;
            
            ref var topBlock = ref this.GetBlock(this.top);

            var createNewBlock = true;
            var ptr = this.top + topBlock.fullSize;
            if (this.heapStart == 0L) {
                // initialize first block
                this.heapStart = this.top;
                ptr = this.top;
            } else {
                if (topBlock.freeIndex > 0) {
                    // top block not used - just resize this block
                    var delta = size - this.currentSize;
                    // we need to subtract 1L from the size to skip first byte
                    this.currentSize = topBlock.blockEndPtr - 1L + delta;
                    if (options == ClearOptions.ClearMemory) {
                        this.MemClear((MemPtr)(topBlock.blockEndPtr - 1L), 0, delta);
                    }
                    topBlock.dataSize += delta;
                    
                    // we don't need to change top ptr
                    ptr = this.top;
                    createNewBlock = false;
                } else {
                    topBlock.nextBlockPtr = ptr;
                }
            }

            ref var block = ref this.GetBlock(ptr);
            if (createNewBlock == true) {

                // create an empty block with size
                block.dataSize = size - this.currentSize - BLOCK_HEADER_SIZE;
                block.freeIndex = 1;
                block.nextBlockPtr = 0;
                block.blockHeadPtr = ptr;

                this.top = ptr;
                // we need to subtract 1L from the size to skip first byte
                this.currentSize = block.blockEndPtr - 1L;

                if (this.searchMode == SearchMode.FreeList) {
                    block.freeIndex = this.freeList.Count + 1;
                    this.freeList.Add(ptr);
                }

                if (options == ClearOptions.ClearMemory) {
                    this.MemClear((MemPtr)(block.blockDataPtr - 1L), 0L, block.dataSize);
                }

            }

            UnityEngine.Assertions.Assert.AreEqual(this.currentSize, this.allocatedSize);
            //this.top += block.fullSize;

            return ref block;

        }

        private void data_free_to_os(void* ptr) {
            
            UnsafeUtility.Free(ptr, Allocator.Persistent);
            
        }
        
        public MemoryAllocator Initialize(size_t initialSize, size_t maxSize = -1L) {

            this.maxSize = maxSize;
            this.freeList = new ListCopyable<long>();
            this.heapStart = 0L;
            // start from offset = 1 because 0 is nullptr
            this.top = 1L;
            this.searchMode = SearchMode.FreeList;
            this.ReAllocData_INTERNAL(initialSize, ClearOptions.UninitializedMemory);
            
            return this;

        }

        public void Dispose() {

            this.data_free_to_os(this.data);
            this = default;

        }

        public void CopyFrom(in MemoryAllocator other) {

            if (this.currentSize != other.currentSize) {
                
                this.data_free_to_os(this.data);
                this.data = this.AllocData_INTERNAL(other.currentSize);

            }
            
            UnsafeUtility.MemCpy(this.data, other.data, other.currentSize);
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
            return (x + sizeof(word_t) - 1) & ~(sizeof(word_t) - 1);
        }
        
        private static bool CanSplit(ref Block block, size_t size) {
            return block.dataSize >= size;
        }

        private void Split(ref Block block, size_t size) {
            var targetSize = block.dataSize - size;
            if (targetSize > 0) {
                // create free block
                var freePart = new Block {
                    dataSize = targetSize - BLOCK_HEADER_SIZE,
                    freeIndex = 1,
                    nextBlockPtr = block.nextBlockPtr,
                    blockHeadPtr = block.blockHeadPtr + size + BLOCK_HEADER_SIZE,
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
                // split current block, change it size
                block.dataSize = size;
                block.nextBlockPtr = freePart.blockHeadPtr;
            } else {
                // if target size is zero - we don't need to create new free block
            }
        }

        private ref Block ListAllocate(ref Block block, size_t size) {
            // Split the larger block, reusing the free part.
            if (MemoryAllocator.CanSplit(ref block, size)) {
                this.Split(ref block, size);
            }

            block.freeIndex = 0;
            block.dataSize = size;

            return ref block;
        }
        
        public MemPtr Alloc(size_t size) {

            return this.Alloc(size, ClearOptions.ClearMemory);

        }

        public MemPtr Alloc(size_t size, ClearOptions options) {

            size = MemoryAllocator.Align(size);

            // ---------------------------------------------------------
            // 1. Search for a free block in the free-list:
            //
            // Traverse the blocks list, searching for a block of
            // the appropriate size

            if (this.FindBlock(size, out var block) == true) {
                return block.GetMemPtr();
            }

            // ---------------------------------------------------------
            // 2. If block not found in the free list, request from OS:
            // No block found, request to map more memory from the OS,
            // bumping the program break (brk).
            var newSize = this.currentSize + size;
            ref var freeBlock = ref this.ReAllocData_INTERNAL(newSize, options);

            // Now we have enough data size to allocate,
            // so just run Alloc method again
            return this.Alloc(size, options);
            
        }

        internal readonly ref Block GetBlock(ptr ptr) {
            return ref UnsafeUtility.AsRef<Block>((void*)((ptr)this.data + ptr));
        }

        internal readonly ref T GetBlockData<T>(ptr ptr, ptr offset) where T : struct {
            return ref UnsafeUtility.AsRef<T>((void*)((ptr)this.data + ptr + BLOCK_HEADER_SIZE + offset));
        }

        private bool CanCoalesce(ref Block block) {
            return block.nextBlockPtr != 0L && this.GetBlock(block.nextBlockPtr).freeIndex > 0;
        }

        private ref Block Coalesce(ref Block block) {

            if (block.nextBlockPtr == this.top) {
                this.top = block.blockHeadPtr;
            }
            
            var next = this.GetBlock(block.nextBlockPtr);
            block.nextBlockPtr = next.nextBlockPtr;
            block.dataSize += BLOCK_HEADER_SIZE + next.dataSize;

            if (this.searchMode == SearchMode.FreeList) {
                this.freeList.RemoveAtFast(next.freeIndex - 1, out var movedPtr);
                ref var moved = ref this.GetBlock(movedPtr);
                moved.freeIndex = next.freeIndex;
            }

            return ref block;

        }

        public bool Free(MemPtr ptr) {
            ref var block = ref this.GetBlock((ptr)ptr);
            if (block.freeIndex != 0) return false;
            if (this.CanCoalesce(ref block) == true) {
                block = this.Coalesce(ref block);
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
            return ref UnsafeUtility.AsRef<T>((void*)((ptr)this.data + (ptr)ptr + BLOCK_HEADER_SIZE));
        }

        public MemPtr Alloc<T>() where T : unmanaged {
            var size = sizeof(T);
            return this.Alloc(size);
        }

        public MemPtr AllocUnmanaged<T>() where T : struct {
            var size = UnsafeUtility.SizeOf<T>();
            return this.Alloc(size);
        }

        public MemPtr ReAlloc(MemPtr ptr, size_t newSize, ClearOptions options) {

            if (ptr == MemPtr.Zero) {

                return this.Alloc(newSize, options);

            }
            ref var block = ref this.GetBlock((ptr)ptr);
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
            
            if (dest == null) {
                throw new System.NullReferenceException();
            }

            if (source == null) {
                throw new System.NullReferenceException();
            }

            var offsetDest = (ptr)dest + destOffset;
            if (offsetDest + length >= this.currentSize) {
                throw new System.OverflowException();
            }

            var offsetSrc = (ptr)source + sourceOffset;
            if (offsetSrc + length >= this.currentSize) {
                throw new System.OverflowException();
            }

            UnsafeUtility.MemCpy(
                (void*)((ptr)this.data + offsetDest),
                (void*)((ptr)this.data + offsetSrc),
                length);

        }

        public void MemClear(MemPtr dest, size_t destOffset, size_t length) {
            
            if (dest == null) {
                throw new System.NullReferenceException();
            }
            
            var offsetDest = (ptr)dest + destOffset;
            if (offsetDest + length > this.currentSize) {
                throw new System.OverflowException();
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
        
        private bool FindBlock(size_t size, out Block block) {

            return this.FreeListAlgorithm(size, out block);

        }

        private bool FreeListAlgorithm(size_t size, out Block block) {

            block = default;
            for (int i = 0, len = this.freeList.Count; i < len; ++i) {

                var ptr = this.freeList[i];
                ref var b = ref this.GetBlock(ptr);
                if (b.dataSize < size + BLOCK_HEADER_SIZE) {
                    continue;
                }
                
                this.freeList.RemoveAtFast(i);
                block = this.ListAllocate(ref b, size);
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