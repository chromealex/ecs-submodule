namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    using MemPtr = System.Int64;
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;
    
    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(HashSetProxy<>))]
    public struct HashSet<T> where T : unmanaged {

        private struct InternalData {

            public MemArrayAllocator<int> buckets;
            public MemArrayAllocator<Slot> slots;
            public int count;
            public int lastIndex;
            public int freeList;
            public int version;
            
            public void Dispose(ref MemoryAllocator allocator) {
                
                this.buckets.Dispose(ref allocator);
                this.slots.Dispose(ref allocator);
                this = default;

            }

        }
        
        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private readonly State state;
            private ref MemoryAllocator allocator => ref this.state.allocator;
            private readonly HashSet<T> set;
            private int index;
            private T current;

            internal Enumerator(State state, HashSet<T> set) {
                this.state = state;
                this.set = set;
                this.index = 0;
                this.current = default(T);
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                while (this.index < this.set.lastIndex(in this.allocator)) {
                    if (this.set.slots(in this.allocator)[in this.allocator, this.index].hashCode >= 0) {
                        this.current = this.set.slots(in this.allocator)[in this.allocator, this.index].value;
                        this.index++;
                        return true;
                    }

                    this.index++;
                }

                this.index = this.set.lastIndex(in this.allocator) + 1;
                this.current = default(T);
                return false;
            }

            public T Current => this.current;

            object System.Collections.IEnumerator.Current => this.Current;

            void System.Collections.IEnumerator.Reset() {
                this.index = 0;
                this.current = default(T);
            }
            
        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<T> {

            private readonly MemoryAllocator allocator;
            private readonly HashSet<T> set;
            private int index;
            private T current;

            internal EnumeratorNoState(in MemoryAllocator allocator, HashSet<T> set) {
                this.allocator = allocator;
                this.set = set;
                this.index = 0;
                this.current = default(T);
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                while (this.index < this.set.lastIndex(in this.allocator)) {
                    if (this.set.slots(in this.allocator)[in this.allocator, this.index].hashCode >= 0) {
                        this.current = this.set.slots(in this.allocator)[in this.allocator, this.index].value;
                        this.index++;
                        return true;
                    }

                    this.index++;
                }

                this.index = this.set.lastIndex(in this.allocator) + 1;
                this.current = default(T);
                return false;
            }

            public T Current => this.current;
            public int Index => this.index - 1;
            
            object System.Collections.IEnumerator.Current => this.Current;

            void System.Collections.IEnumerator.Reset() {
                this.index = 0;
                this.current = default(T);
            }
            
        }

        public struct Slot {
            internal int hashCode;      // Lower 31 bits of hash code, -1 if unused
            internal int next;          // Index of next entry, -1 if last
            internal T value;
        }
        
        private const int LOWER31_BIT_MASK = 0x7FFFFFFF;
        [ME.ECS.Serializer.SerializeField]
        private readonly MemPtr ptr;

        [INLINE(256)]
        internal readonly ref MemArrayAllocator<int> buckets(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).buckets;
        [INLINE(256)]
        internal readonly ref MemArrayAllocator<Slot> slots(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).slots;
        [INLINE(256)]
        internal readonly ref int count(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).count;
        [INLINE(256)]
        internal readonly ref int lastIndex(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).lastIndex;
        [INLINE(256)]
        internal readonly ref int freeList(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).freeList;
        [INLINE(256)]
        internal readonly ref int version(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).version;

        public bool isCreated {
            [INLINE(256)]
            get => this.ptr != 0;
        }

        [INLINE(256)]
        public int Count(in MemoryAllocator allocator) => this.count(in allocator);

        [INLINE(256)]
        public HashSet(ref MemoryAllocator allocator, int capacity) {

            this.ptr = allocator.AllocData<InternalData>(default);
            this.Initialize(ref allocator, capacity);

        }
        
        [INLINE(256)]
        public void Dispose(ref MemoryAllocator allocator) {
            
            allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;
            
        }

        [INLINE(256)]
        public readonly Enumerator GetEnumerator(State state) {
            
            return new Enumerator(state, this);
            
        }
        
        [INLINE(256)]
        public readonly Enumerator GetEnumerator() {
            
            return this.GetEnumerator(Worlds.current.GetState());
            
        }

        [INLINE(256)]
        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
            
            return new EnumeratorNoState(in allocator, this);
            
        }

        [INLINE(256)]
        public ref T GetByIndex(in MemoryAllocator allocator, int index) {
            return ref this.slots(in allocator)[in allocator, index].value;
        }

        /// <summary>
        /// Remove all items from this set. This clears the elements but not the underlying 
        /// buckets and slots array. Follow this call by TrimExcess to release these.
        /// </summary>
        /// <param name="allocator"></param>
        [INLINE(256)]
        public void Clear(in MemoryAllocator allocator) {
            if (this.lastIndex(in allocator) > 0) {
                // clear the elements so that the gc can reclaim the references.
                // clear only up to m_lastIndex for m_slots
                this.slots(in allocator).Clear(in allocator, 0, this.lastIndex(in allocator));
                this.buckets(in allocator).Clear(in allocator, 0, this.buckets(in allocator).Length);
                this.lastIndex(in allocator) = 0;
                this.count(in allocator) = 0;
                this.freeList(in allocator) = -1;
            }
            this.version(in allocator)++;
        }

        /// <summary>
        /// Checks if this hashset contains the item
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="item">item to check for containment</param>
        /// <returns>true if item contained; false if not</returns>
        [INLINE(256)]
        public readonly bool Contains(in MemoryAllocator allocator, T item) {
            if (this.buckets(in allocator).isCreated == true) {
                var c = System.Collections.Generic.EqualityComparer<T>.Default;
                int hashCode = this.InternalGetHashCode(item);
                // see note at "HashSet" level describing why "- 1" appears in for loop
                for (int i = this.buckets(in allocator)[in allocator, hashCode % this.buckets(in allocator).Length] - 1; i >= 0; i = this.slots(in allocator)[in allocator, i].next) {
                    if (this.slots(in allocator)[in allocator, i].hashCode == hashCode &&
                        c.Equals(this.slots(in allocator)[in allocator, i].value, item) == true) {
                        return true;
                    }
                }
            }
            // either m_buckets is null or wasn't found
            return false;
        }

        /// <summary>
        /// Remove item from this hashset
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="item">item to remove</param>
        /// <returns>true if removed; false if not (i.e. if the item wasn't in the HashSet)</returns>
        [INLINE(256)]
        public bool Remove(ref MemoryAllocator allocator, T item) {
            if (this.buckets(in allocator).isCreated == true) {
                var c = System.Collections.Generic.EqualityComparer<T>.Default;
                int hashCode = this.InternalGetHashCode(item);
                int bucket = hashCode % this.buckets(in allocator).Length;
                int last = -1;
                for (int i = this.buckets(in allocator)[in allocator, bucket] - 1; i >= 0; last = i, i = this.slots(in allocator)[in allocator, i].next) {
                    if (this.slots(in allocator)[in allocator, i].hashCode == hashCode &&
                        c.Equals(this.slots(in allocator)[in allocator, i].value, item) == true) {
                        if (last < 0) {
                            // first iteration; update buckets
                            this.buckets(in allocator)[in allocator, bucket] = this.slots(in allocator)[in allocator, i].next + 1;
                        }
                        else {
                            // subsequent iterations; update 'next' pointers
                            this.slots(in allocator)[in allocator, last].next = this.slots(in allocator)[in allocator, i].next;
                        }
                        this.slots(in allocator)[in allocator, i].hashCode = -1;
                        this.slots(in allocator)[in allocator, i].value = default(T);
                        this.slots(in allocator)[in allocator, i].next = this.freeList(in allocator);

                        this.count(in allocator)--;
                        this.version(in allocator)++;
                        if (this.count(in allocator) == 0) {
                            this.lastIndex(in allocator) = 0;
                            this.freeList(in allocator) = -1;
                        }
                        else {
                            this.freeList(in allocator) = i;
                        }
                        return true;
                    }
                }
            }
            // either m_buckets is null or wasn't found
            return false;
        }

        /// <summary>
        /// Add item to this HashSet. Returns bool indicating whether item was added (won't be 
        /// added if already present)
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="item"></param>
        /// <returns>true if added, false if already present</returns>
        [INLINE(256)]
        public bool Add(ref MemoryAllocator allocator, T item) {
            return this.AddIfNotPresent(ref allocator, item);
        }

        /// <summary>
        /// Searches the set for a given value and returns the equal value it finds, if any.
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="equalValue">The value to search for.</param>
        /// <param name="actualValue">The value from the set that the search found, or the default value of <typeparamref name="T"/> when the search yielded no match.</param>
        /// <returns>A value indicating whether the search was successful.</returns>
        /// <remarks>
        /// This can be useful when you want to reuse a previously stored reference instead of 
        /// a newly constructed one (so that more sharing of references can occur) or to look up
        /// a value that has more complete data than the value you currently have, although their
        /// comparer functions indicate they are equal.
        /// </remarks>
        [INLINE(256)]
        public readonly bool TryGetValue(ref MemoryAllocator allocator, T equalValue, out T actualValue) {
            if (this.buckets(in allocator).isCreated == true) {
                int i = this.InternalIndexOf(in allocator, equalValue);
                if (i >= 0) {
                    actualValue = this.slots(in allocator)[in allocator, i].value;
                    return true;
                }
            }
            actualValue = default(T);
            return false;
        }
        
        [INLINE(256)]
        public ref T GetValue(in MemoryAllocator allocator, T equalValue) {
            
            int i = this.InternalIndexOf(in allocator, equalValue);
            if (i >= 0) {
                return ref this.slots(in allocator)[in allocator, i].value;
            }
            
            throw new System.Collections.Generic.KeyNotFoundException();
            
        }

        #region Helper
        /// <summary>
        /// Initializes buckets and slots arrays. Uses suggested capacity by finding next prime
        /// greater than or equal to capacity.
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="capacity"></param>
        [INLINE(256)]
        private void Initialize(ref MemoryAllocator allocator, int capacity) {
            int size = HashHelpers.GetPrime(capacity);
            this.buckets(in allocator) = new MemArrayAllocator<int>(ref allocator, size);
            this.slots(in allocator) = new MemArrayAllocator<Slot>(ref allocator, size);
            this.freeList(in allocator) = -1;
        }

        /// <summary>
        /// Expand to new capacity. New capacity is next prime greater than or equal to suggested 
        /// size. This is called when the underlying array is filled. This performs no 
        /// defragmentation, allowing faster execution; note that this is reasonable since 
        /// AddIfNotPresent attempts to insert new elements in re-opened spots.
        /// </summary>
        /// <param name="allocator"></param>
        [INLINE(256)]
        private void IncreaseCapacity(ref MemoryAllocator allocator) {
            int newSize = HashHelpers.ExpandPrime(this.count(in allocator));
            if (newSize <= this.count(in allocator)) {
                throw new System.ArgumentException();
            }

            // Able to increase capacity; copy elements to larger array and rehash
            this.SetCapacity(ref allocator, newSize, false);
        }

        /// <summary>
        /// Set the underlying buckets array to size newSize and rehash.  Note that newSize
        /// *must* be a prime.  It is very likely that you want to call IncreaseCapacity()
        /// instead of this method.
        /// </summary>
        [INLINE(256)]
        private void SetCapacity(ref MemoryAllocator allocator, int newSize, bool forceNewHashCodes) { 
            System.Diagnostics.Contracts.Contract.Assert(HashHelpers.IsPrime(newSize), "New size is not prime!");

            System.Diagnostics.Contracts.Contract.Assert(this.buckets(in allocator).isCreated, "SetCapacity called on a set with no elements");

            var newSlots = new MemArrayAllocator<Slot>(ref allocator, newSize);
            if (this.slots(in allocator).isCreated == true) {
                NativeArrayUtils.CopyNoChecks(ref allocator, in this.slots(in allocator), 0, ref newSlots, 0, this.lastIndex(in allocator));
            }

            if (forceNewHashCodes == true) {
                for(int i = 0; i < this.lastIndex(in allocator); i++) {
                    if(newSlots[in allocator, i].hashCode != -1) {
                        newSlots[in allocator, i].hashCode = this.InternalGetHashCode(newSlots[in allocator, i].value);
                    }
                }
            }

            var newBuckets = new MemArrayAllocator<int>(ref allocator, newSize);
            for (int i = 0; i < this.lastIndex(in allocator); i++) {
                int bucket = newSlots[in allocator, i].hashCode % newSize;
                newSlots[in allocator, i].next = newBuckets[in allocator, bucket] - 1;
                newBuckets[in allocator, bucket] = i + 1;
            }
            if (this.slots(in allocator).isCreated == true) this.slots(in allocator).Dispose(ref allocator);
            if (this.buckets(in allocator).isCreated == true) this.buckets(in allocator).Dispose(ref allocator);
            this.slots(in allocator) = newSlots;
            this.buckets(in allocator) = newBuckets;
        }

        /// <summary>
        /// Adds value to HashSet if not contained already
        /// Returns true if added and false if already present
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="value">value to find</param>
        /// <returns></returns>
        [INLINE(256)]
        private bool AddIfNotPresent(ref MemoryAllocator allocator, T value) {
            if (this.buckets(in allocator).isCreated == false) {
                this.Initialize(ref allocator, 0);
            }

            int hashCode = this.InternalGetHashCode(value);
            int bucket = hashCode % this.buckets(in allocator).Length;
            var c = System.Collections.Generic.EqualityComparer<T>.Default;
            for (int i = this.buckets(in allocator)[in allocator, hashCode % this.buckets(in allocator).Length] - 1; i >= 0; i = this.slots(in allocator)[in allocator, i].next) {
                if (this.slots(in allocator)[in allocator, i].hashCode == hashCode &&
                    c.Equals(this.slots(in allocator)[in allocator, i].value, value) == true) {
                    return false;
                }
            }

            int index;
            if (this.freeList(in allocator) >= 0) {
                index = this.freeList(in allocator);
                this.freeList(in allocator) = this.slots(in allocator)[in allocator, index].next;
            }
            else {
                if (this.lastIndex(in allocator) == this.slots(in allocator).Length) {
                    this.IncreaseCapacity(ref allocator);
                    // this will change during resize
                    bucket = hashCode % this.buckets(in allocator).Length;
                }
                index = this.lastIndex(in allocator);
                this.lastIndex(in allocator)++;
            }
            this.slots(in allocator)[in allocator, index].hashCode = hashCode;
            this.slots(in allocator)[in allocator, index].value = value;
            this.slots(in allocator)[in allocator, index].next = this.buckets(in allocator)[in allocator, bucket] - 1;
            this.buckets(in allocator)[in allocator, bucket] = index + 1;
            this.count(in allocator)++;
            this.version(in allocator)++;

            return true;
        }

        // Add value at known index with known hash code. Used only
        // when constructing from another HashSet.
        [INLINE(256)]
        private void AddValue(ref MemoryAllocator allocator, int index, int hashCode, T value) {
            int bucket = hashCode % this.buckets(in allocator).Length;
            this.slots(in allocator)[in allocator, index].hashCode = hashCode;
            this.slots(in allocator)[in allocator, index].value = value;
            this.slots(in allocator)[in allocator, index].next = this.buckets(in allocator)[in allocator, bucket] - 1;
            this.buckets(in allocator)[in allocator, bucket] = index + 1;
        }

        /// <summary>
        /// Used internally by set operations which have to rely on bit array marking. This is like
        /// Contains but returns index in slots array. 
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [INLINE(256)]
        private readonly int InternalIndexOf(in MemoryAllocator allocator, T item) {
            int hashCode = this.InternalGetHashCode(item);
            var c = System.Collections.Generic.EqualityComparer<T>.Default;
            for (int i = this.buckets(in allocator)[in allocator, hashCode % this.buckets(in allocator).Length] - 1; i >= 0; i = this.slots(in allocator)[in allocator, i].next) {
                if ((this.slots(in allocator)[in allocator, i].hashCode) == hashCode &&
                    c.Equals(this.slots(in allocator)[in allocator, i].value, item) == true) {
                    return i;
                }
            }
            // wasn't found
            return -1;
        }
        
        /// <summary>
        /// Workaround Comparers that throw ArgumentNullException for GetHashCode(null).
        /// </summary>
        /// <param name="item"></param>
        /// <returns>hash code</returns>
        [INLINE(256)]
        private readonly int InternalGetHashCode(T item) {
            return System.Collections.Generic.EqualityComparer<T>.Default.GetHashCode(item) & HashSet<T>.LOWER31_BIT_MASK;
        }
        #endregion

    }

}
