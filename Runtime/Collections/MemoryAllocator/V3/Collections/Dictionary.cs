namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    using Unity.Collections.LowLevel.Unsafe;
    
    public struct Dictionary<TKey, TValue> where TKey : unmanaged, System.IEquatable<TKey> where TValue : unmanaged {
        
        public struct Enumerator : System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<TKey,TValue>> {
            
            private Dictionary<TKey,TValue> dictionary;
            private int version;
            private int index;
            private System.Collections.Generic.KeyValuePair<TKey,TValue> current;
            private readonly MemoryAllocator allocator;
            
            internal Enumerator(in MemoryAllocator allocator, Dictionary<TKey,TValue> dictionary) {
                this.allocator = allocator;
                this.dictionary = dictionary;
                this.version = dictionary.version;
                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
            }
 
            public bool MoveNext() {
                if (this.version != this.dictionary.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
 
                // Use unsigned comparison since we set index to dictionary.count+1 when the enumeration ends.
                // dictionary.count+1 could be negative if dictionary.count is Int32.MaxValue
                while ((uint)this.index < (uint)this.dictionary.count) {
                    if (this.dictionary.entries[in this.allocator, this.index].hashCode >= 0) {
                        this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>(this.dictionary.entries[in this.allocator, this.index].key, this.dictionary.entries[in this.allocator, this.index].value);
                        this.index++;
                        return true;
                    }

                    this.index++;
                }

                this.index = this.dictionary.count + 1;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
                return false;
            }
 
            public System.Collections.Generic.KeyValuePair<TKey,TValue> Current {
                get { return this.current; }
            }
 
            public void Dispose() {
            }
 
            object System.Collections.IEnumerator.Current {
                get {
                    return this.current;
                }
            }
 
            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.dictionary.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();    
            }
 
        }
        
        private struct Entry {
            public int hashCode;    // Lower 31 bits of hash code, -1 if unused
            public int next;        // Index of next entry, -1 if last
            public TKey key;           // Key of entry
            public TValue value;         // Value of entry
        }
 
        private MemArrayAllocator<int> buckets;
        private MemArrayAllocator<Entry> entries;
        private int count;
        private int version;
        private int freeList;
        private int freeCount;
        
        public bool isCreated => this.buckets.isCreated;
        public int Count => this.count - this.freeCount;

        public Dictionary(ref MemoryAllocator allocator, int capacity) {

            this = default;
            this.Initialize(ref allocator, capacity);

        }
        
        public void Dispose(ref MemoryAllocator allocator) {
            
            this.buckets.Dispose(ref allocator);
            this.entries.Dispose(ref allocator);
            this = default;

        }

        public readonly Enumerator GetEnumerator(in MemoryAllocator allocator) {
            
            return new Enumerator(in allocator, this);
            
        }

        /// <summary>
        /// Remove all items from this set. This clears the elements but not the underlying 
        /// buckets and slots array. Follow this call by TrimExcess to release these.
        /// </summary>
        /// <param name="allocator"></param>
        public void Clear(in MemoryAllocator allocator) {
            if (this.count > 0) {
                for (int i = 0; i < this.buckets.Length; i++) this.buckets[in allocator, i] = -1;
                this.entries.Clear(in allocator, 0, this.count);
                this.freeList = -1;
                this.count = 0;
                this.freeCount = 0;
                this.version++;
            }
        }

        /// <summary>
        /// Checks if this dictionary contains the key
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="key">key to check for containment</param>
        /// <returns>true if key contained; false if not</returns>
        public readonly bool ContainsKey(in MemoryAllocator allocator, TKey key) {
            return this.FindEntry(in allocator, key) >= 0;
        }
 
        /// <summary>
        /// Checks if this dictionary contains the value
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public readonly bool ContainsValue(in MemoryAllocator allocator, TValue value) {
            var c = System.Collections.Generic.EqualityComparer<TValue>.Default;
            for (int i = 0; i < this.count; i++) {
                if (this.entries[in allocator, i].hashCode >= 0 && c.Equals(this.entries[in allocator, i].value, value)) return true;
            }
            return false;
        }
        
        public ref TValue this[in MemoryAllocator allocator, TKey key] {
            get {
                int i = this.FindEntry(in allocator, key);
                if (i >= 0) return ref this.entries[in allocator, i].value;
                throw new System.Collections.Generic.KeyNotFoundException();
            }
        }

        public bool Remove(ref MemoryAllocator allocator, TKey key) {
            var comparer = System.Collections.Generic.EqualityComparer<TKey>.Default;
            if (this.buckets.isCreated == true) {
                int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
                int bucket = hashCode % this.buckets.Length;
                int last = -1;
                for (int i = this.buckets[in allocator, bucket]; i >= 0; last = i, i = this.entries[in allocator, i].next) {
                    if (this.entries[in allocator, i].hashCode == hashCode && comparer.Equals(this.entries[in allocator, i].key, key)) {
                        if (last < 0) {
                            this.buckets[in allocator, bucket] = this.entries[in allocator, i].next;
                        }
                        else {
                            this.entries[in allocator, last].next = this.entries[in allocator, i].next;
                        }

                        this.entries[in allocator, i].hashCode = -1;
                        this.entries[in allocator, i].next = this.freeList;
                        this.entries[in allocator, i].key = default(TKey);
                        this.entries[in allocator, i].value = default(TValue);
                        this.freeList = i;
                        this.freeCount++;
                        this.version++;
                        return true;
                    }
                }
            }
            return false;
        }
        
        public void Add(ref MemoryAllocator allocator, TKey key, TValue value) {
            this.Insert(ref allocator, key, value, true);
        }
 
        public readonly bool TryGetValue(in MemoryAllocator allocator, TKey key, out TValue value) {
            int i = this.FindEntry(in allocator, key);
            if (i >= 0) {
                value = this.entries[in allocator, i].value;
                return true;
            }
            value = default(TValue);
            return false;
        }
        
        #region Helper
        private readonly int FindEntry(in MemoryAllocator allocator, TKey key) {
            if (this.buckets.isCreated == true) {
                var c = System.Collections.Generic.EqualityComparer<TKey>.Default;
                int hashCode = c.GetHashCode(key) & 0x7FFFFFFF;
                for (int i = this.buckets[in allocator, hashCode % this.buckets.Length]; i >= 0; i = this.entries[in allocator, i].next) {
                    if (this.entries[in allocator, i].hashCode == hashCode && c.Equals(this.entries[in allocator, i].key, key)) return i;
                }
            }
            return -1;
        }
        
        private void Insert(ref MemoryAllocator allocator, TKey key, TValue value, bool add) {
        
            var comparer = System.Collections.Generic.EqualityComparer<TKey>.Default;
            if (this.buckets.isCreated == false) {
                this.Initialize(ref allocator, 0);
            }
            int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
            int targetBucket = hashCode % this.buckets.Length;
 
            for (int i = this.buckets[in allocator, targetBucket]; i >= 0; i = this.entries[in allocator, i].next) {
                if (this.entries[in allocator, i].hashCode == hashCode && comparer.Equals(this.entries[in allocator, i].key, key)) {
                    if (add) { 
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
                    }

                    this.entries[in allocator, i].value = value;
                    this.version++;
                    return;
                }
            }
            int index;
            if (this.freeCount > 0) {
                index = this.freeList;
                this.freeList = this.entries[in allocator, index].next;
                this.freeCount--;
            }
            else {
                if (this.count == this.entries.Length)
                {
                    this.Resize(ref allocator);
                    targetBucket = hashCode % this.buckets.Length;
                }
                index = this.count;
                this.count++;
            }

            this.entries[in allocator, index].hashCode = hashCode;
            this.entries[in allocator, index].next = this.buckets[in allocator, targetBucket];
            this.entries[in allocator, index].key = key;
            this.entries[in allocator, index].value = value;
            this.buckets[in allocator, targetBucket] = index;
            this.version++;
 
        }
        
        /// <summary>
        /// Initializes buckets and slots arrays. Uses suggested capacity by finding next prime
        /// greater than or equal to capacity.
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="capacity"></param>
        private void Initialize(ref MemoryAllocator allocator, int capacity) {
            int size = HashHelpers.GetPrime(capacity);
            this.buckets = new MemArrayAllocator<int>(ref allocator, size);
            for (int i = 0; i < this.buckets.Length; i++) this.buckets[in allocator, i] = -1;
            this.entries = new MemArrayAllocator<Entry>(ref allocator, size);
            this.freeList = -1;
        }
        
        private void Resize(ref MemoryAllocator allocator) {
            this.Resize(ref allocator, HashHelpers.ExpandPrime(this.count), false);
        }
 
        private void Resize(ref MemoryAllocator allocator, int newSize, bool forceNewHashCodes) {
            var comparer = System.Collections.Generic.EqualityComparer<TKey>.Default;
            var newBuckets = new MemArrayAllocator<int>(ref allocator, newSize);
            for (int i = 0; i < newBuckets.Length; i++) newBuckets[in allocator, i] = -1;
            var newEntries = new MemArrayAllocator<Entry>(ref allocator, newSize);
            allocator.MemCopy(newEntries.GetMemPtr(), 0, this.entries.GetMemPtr(), 0, this.count);
            if(forceNewHashCodes) {
                for (int i = 0; i < this.count; i++) {
                    if(newEntries[in allocator, i].hashCode != -1) {
                        newEntries[in allocator, i].hashCode = (comparer.GetHashCode(newEntries[in allocator, i].key) & 0x7FFFFFFF);
                    }
                }
            }
            for (int i = 0; i < this.count; i++) {
                if (newEntries[in allocator, i].hashCode >= 0) {
                    int bucket = newEntries[in allocator, i].hashCode % newSize;
                    newEntries[in allocator, i].next = newBuckets[in allocator, bucket];
                    newBuckets[in allocator, bucket] = i;
                }
            }
            if (this.buckets.isCreated == true) this.buckets.Dispose(ref allocator);
            if (this.entries.isCreated == true) this.entries.Dispose(ref allocator);
            this.buckets = newBuckets;
            this.entries = newEntries;
        }
        #endregion

    }

}
