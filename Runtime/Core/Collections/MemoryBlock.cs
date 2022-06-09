namespace ME.ECS.Collections {

    public struct Pointer {

        public int index;
        public int size;
        
        public int next;
        public int prev;
        //public bool isFree;

    }

    public readonly struct MemPtr {

        public readonly int value;

        internal MemPtr(int value) {
            this.value = value;
        }

    }
    
    public struct MemoryBlock {

        private ListCopyable<Pointer> free;
        private ListCopyable<Pointer> allocated;
        private DictionaryInt<int> allocatedIndex;
        private DictionaryInt<int> freeIndex;
        private byte[] data;
        private int maxSize;
        
        public void Initialize(int initialSize, int maxSize = -1) {
            
            this.free = new ListCopyable<Pointer>();
            this.allocated = new ListCopyable<Pointer>();
            this.allocatedIndex = new DictionaryInt<int>();
            this.freeIndex = new DictionaryInt<int>();
            
            var ptr = new Pointer() {
                index = 0,
                size = initialSize,
                next = -1,
                prev = -1,
            };
            this.free.Add(ptr);
            this.maxSize = maxSize;
            this.data = new byte[initialSize];
            
        }

        public bool Dealloc(MemPtr ptr) {

            if (this.allocatedIndex.TryGetValue(ptr.value, out var idx) == true) {
                
                var pointer = this.allocated[idx];
                {
                    if (pointer.prev != -1 && this.IsFree(pointer.prev, out var prevPointer, out var prevIdx) == true) {
                        // merge with prev pointer
                        pointer.prev = prevPointer.prev;
                        pointer.index = prevPointer.index;
                        pointer.size += prevPointer.size;
                        this.RemoveAtFast(this.free, prevIdx, this.freeIndex, prevPointer.index);
                        if (pointer.next != -1 && this.IsAlloc(pointer.next, out var nextPointer, out var nextIdx) == true) {
                            nextPointer.prev = pointer.index;
                            this.allocated[nextIdx] = nextPointer;
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
                            this.allocated[nextIdx] = nextPointer;
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

        private void RemoveAtFast(ListCopyable<Pointer> list, int index, DictionaryInt<int> indexer, int indexerValue) {

            var targetIndex = list.Count - 1;
            if (targetIndex == index) {
                list.RemoveAtFast(index);
                indexer.Remove(indexerValue);
            } else {
                var last = list[targetIndex];
                list.RemoveAtFast(index);
                indexer.Remove(indexerValue);
                indexer[last.index] = index;
            }

        }

        private bool IsFree(int index, out Pointer pointer, out int idx) {

            pointer = default;
            if (this.freeIndex.TryGetValue(index, out idx) == false) return false;
            pointer = this.free[idx];
            return true;

        }

        private bool IsAlloc(int index, out Pointer pointer, out int idx) {

            pointer = default;
            if (this.allocatedIndex.TryGetValue(index, out idx) == false) return false;
            pointer = this.allocated[idx];
            return true;

        }

        public MemPtr Alloc(int size) {

            // lookup for free pointers with required size
            int headPointerIdx = -1;
            Pointer headPointer = default;
            for (int i = 0; i < this.free.Count; ++i) {

                var ptr = this.free[i];
                if (size <= ptr.size) {

                    var memPtr = new MemPtr(ptr.index);
                    if (ptr.size == size) {
                        // if we alloc all block
                        //ptr.isFree = false;
                        this.RemoveAtFast(this.free, i, this.freeIndex, ptr.index);
                    } else {
                        // if block is not full
                        // create new free pointer
                        var freePointer = new Pointer() {
                            index = ptr.index + size,
                            size = ptr.size - size,
                            next = ptr.next,
                            prev = ptr.index,
                            //isFree = true,
                        };
                        this.freeIndex.Remove(ptr.index);
                        this.free[i] = freePointer;
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
                    headPointer = ptr;
                    headPointerIdx = i;
                }

            }

            // no free pointers found
            // re-alloc data with required size
            // use headPointer to resize data
            //headPointer.isFree = false;
            var newSize = this.data.Length - headPointer.index + size * 2;
            if (this.maxSize > 0 && newSize > this.maxSize) {
                throw new System.Exception($"Overflow max size: {newSize}/{this.maxSize}");
            }
            System.Array.Resize(ref this.data, newSize);

            this.free[headPointerIdx].size = newSize;
            
            return this.Alloc(size);

        }

    }

}