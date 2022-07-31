namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    using Unity.Collections.LowLevel.Unsafe;
    using MemPtr = System.Int64;

    public struct EquatableDictionary<TKey, TValue> where TKey : unmanaged, System.IEquatable<TKey> where TValue : unmanaged {

        private struct InternalData {

            public MemArrayAllocator<int> buckets;
            public MemArrayAllocator<Entry> entries;
            public int count;
            public int version;
            public int freeList;
            public int freeCount;

            public void Dispose(ref MemoryAllocator allocator) {
                
                this.buckets.Dispose(ref allocator);
                this.entries.Dispose(ref allocator);
                this = default;

            }

        }
        
        public struct Enumerator : System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<TKey, TValue>> {

            private readonly State state;
            private ref MemoryAllocator allocator => ref this.state.allocator;
            private readonly EquatableDictionary<TKey, TValue> dictionary;
            private readonly int version;
            private int index;
            private System.Collections.Generic.KeyValuePair<TKey, TValue> current;

            internal Enumerator(State state, EquatableDictionary<TKey, TValue> dictionary) {
                this.state = state;
                this.dictionary = dictionary;
                this.version = dictionary.version(in state.allocator);
                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
            }

            public bool MoveNext() {
                if (this.version != this.dictionary.version(in this.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                // Use unsigned comparison since we set index to dictionary.count+1 when the enumeration ends.
                // dictionary.count+1 could be negative if dictionary.count is Int32.MaxValue
                while ((uint)this.index < (uint)this.dictionary.count(in this.allocator)) {
                    if (this.dictionary.entries(in this.allocator)[in this.allocator, this.index].hashCode >= 0) {
                        this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>(this.dictionary.entries(in this.allocator)[in this.allocator, this.index].key,
                                                                                                 this.dictionary.entries(in this.allocator)[in this.allocator, this.index].value);
                        this.index++;
                        return true;
                    }

                    this.index++;
                }

                this.index = this.dictionary.count(in this.allocator) + 1;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
                return false;
            }

            public System.Collections.Generic.KeyValuePair<TKey, TValue> Current => this.current;

            public void Dispose() { }

            object System.Collections.IEnumerator.Current => this.current;

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.dictionary.version(in this.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
            }

        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<TKey, TValue>> {

            private readonly MemoryAllocator allocator;
            private readonly EquatableDictionary<TKey, TValue> dictionary;
            private readonly int version;
            private int index;
            private System.Collections.Generic.KeyValuePair<TKey, TValue> current;

            internal EnumeratorNoState(in MemoryAllocator allocator, EquatableDictionary<TKey, TValue> dictionary) {
                this.allocator = allocator;
                this.dictionary = dictionary;
                this.version = dictionary.version(in this.allocator);
                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
            }

            public bool MoveNext() {
                if (this.version != this.dictionary.version(in this.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                // Use unsigned comparison since we set index to dictionary.count+1 when the enumeration ends.
                // dictionary.count+1 could be negative if dictionary.count is Int32.MaxValue
                while ((uint)this.index < (uint)this.dictionary.count(in this.allocator)) {
                    if (this.dictionary.entries(in this.allocator)[in this.allocator, this.index].hashCode >= 0) {
                        this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>(this.dictionary.entries(in this.allocator)[in this.allocator, this.index].key,
                                                                                                 this.dictionary.entries(in this.allocator)[in this.allocator, this.index].value);
                        this.index++;
                        return true;
                    }

                    this.index++;
                }

                this.index = this.dictionary.count(in this.allocator) + 1;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
                return false;
            }

            public System.Collections.Generic.KeyValuePair<TKey, TValue> Current => this.current;

            public void Dispose() { }

            object System.Collections.IEnumerator.Current => this.current;

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.dictionary.version(in this.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
            }

        }

        private struct Entry {

            public int hashCode; // Lower 31 bits of hash code, -1 if unused
            public int next; // Index of next entry, -1 if last
            public TKey key; // Key of entry
            public TValue value; // Value of entry

        }

        [ME.ECS.Serializer.SerializeField]
        private readonly MemPtr ptr;

        private readonly ref MemArrayAllocator<int> buckets(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).buckets;
        private readonly ref MemArrayAllocator<Entry> entries(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).entries;
        private readonly ref int count(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).count;
        private readonly ref int version(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).version;
        private readonly ref int freeList(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).freeList;
        private readonly ref int freeCount(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).freeCount;

        public bool isCreated => this.ptr != 0;
        public readonly int Count(in MemoryAllocator allocator) => this.count(in allocator) - this.freeCount(in allocator);

        public EquatableDictionary(ref MemoryAllocator allocator, int capacity) {

            this.ptr = allocator.AllocData<InternalData>(default);
            this.Initialize(ref allocator, capacity);

        }

        public void Dispose(ref MemoryAllocator allocator) {
            
            allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;
            
        }

        public void CopyFrom(ref MemoryAllocator allocator, in EquatableDictionary<TKey, TValue> other) {

            NativeArrayUtils.CopyExact(ref allocator, other.buckets(in allocator), ref this.buckets(in allocator));
            NativeArrayUtils.CopyExact(ref allocator, other.entries(in allocator), ref this.entries(in allocator));
            this.count(in allocator) = other.count(in allocator);
            this.version(in allocator) = other.version(in allocator);
            this.freeCount(in allocator) = other.freeCount(in allocator);
            this.freeList(in allocator) = other.freeList(in allocator);

        }

        public readonly Enumerator GetEnumerator(State state) {

            return new Enumerator(state, this);

        }
        
        public readonly Enumerator GetEnumerator() {
            
            return GetEnumerator(Worlds.current.GetState());
            
        }

        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {

            return new EnumeratorNoState(in allocator, this);

        }

        /// <summary>
        /// Remove all items from this set. This clears the elements but not the underlying 
        /// buckets and slots array. Follow this call by TrimExcess to release these.
        /// </summary>
        /// <param name="allocator"></param>
        public void Clear(in MemoryAllocator allocator) {
            if (this.count(in allocator) > 0) {
                for (var i = 0; i < this.buckets(in allocator).Length(in allocator); i++) {
                    this.buckets(in allocator)[in allocator, i] = -1;
                }

                this.entries(in allocator).Clear(in allocator, 0, this.count(in allocator));
                this.freeList(in allocator) = -1;
                this.count(in allocator) = 0;
                this.freeCount(in allocator) = 0;
                this.version(in allocator)++;
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
            for (var i = 0; i < this.count(in allocator); i++) {
                if (this.entries(in allocator)[in allocator, i].hashCode >= 0 && AreEquals(this.entries(in allocator)[in allocator, i].value, value)) {
                    return true;
                }
            }

            return false;
        }

        public ref TValue this[in MemoryAllocator allocator, TKey key] {
            get {
                var i = this.FindEntry(in allocator, key);
                if (i >= 0) {
                    return ref this.entries(in allocator)[in allocator, i].value;
                }

                throw new System.Collections.Generic.KeyNotFoundException();
            }
        }

        public bool Remove(ref MemoryAllocator allocator, TKey key) {
            
            if (this.buckets(in allocator).isCreated == true) {
                var hashCode = GetHash(key);
                var bucket = hashCode % this.buckets(in allocator).Length(in allocator);
                var last = -1;
                for (var i = this.buckets(in allocator)[in allocator, bucket]; i >= 0; last = i, i = this.entries(in allocator)[in allocator, i].next) {
                    if (this.entries(in allocator)[in allocator, i].hashCode == hashCode && AreEquals(this.entries(in allocator)[in allocator, i].key, key)) {
                        if (last < 0) {
                            this.buckets(in allocator)[in allocator, bucket] = this.entries(in allocator)[in allocator, i].next;
                        } else {
                            this.entries(in allocator)[in allocator, last].next = this.entries(in allocator)[in allocator, i].next;
                        }

                        this.entries(in allocator)[in allocator, i].hashCode = -1;
                        this.entries(in allocator)[in allocator, i].next = this.freeList(in allocator);
                        this.entries(in allocator)[in allocator, i].key = default(TKey);
                        this.entries(in allocator)[in allocator, i].value = default(TValue);
                        this.freeList(in allocator) = i;
                        this.freeCount(in allocator)++;
                        this.version(in allocator)++;
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
            var i = this.FindEntry(in allocator, key);
            if (i >= 0) {
                value = this.entries(in allocator)[in allocator, i].value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public ref TValue GetValue(ref MemoryAllocator allocator, TKey key, out bool exists) {

            var i = this.FindEntry(in allocator, key);
            if (i >= 0) {
                exists = true;
                return ref this.entries(in allocator)[in allocator, i].value;
            }

            exists = false;
            return ref this.Insert(ref allocator, key, default, true);

        }

        public ref TValue GetValue(ref MemoryAllocator allocator, TKey key) {

            var i = this.FindEntry(in allocator, key);
            if (i >= 0) {
                return ref this.entries(in allocator)[in allocator, i].value;
            }

            return ref this.Insert(ref allocator, key, default, true);

        }

        public TValue GetValueAndRemove(ref MemoryAllocator allocator, TKey key) {

            var i = this.FindEntry(in allocator, key);
            if (i >= 0) {
                var v = this.entries(in allocator)[in allocator, i].value;
                this.Remove(ref allocator, key);
                return v;
            }

            return default;

        }

        #region Helper
        private readonly int FindEntry(in MemoryAllocator allocator, TKey key) {
            if (this.buckets(in allocator).isCreated == true) {
                var hashCode = GetHash(key);
                for (var i = this.buckets(in allocator)[in allocator, hashCode % this.buckets(in allocator).Length(in allocator)]; i >= 0; i = this.entries(in allocator)[in allocator, i].next) {
                    if (this.entries(in allocator)[in allocator, i].hashCode == hashCode && AreEquals(this.entries(in allocator)[in allocator, i].key, key)) {
                        return i;
                    }
                }
            }

            return -1;
        }

        private ref TValue Insert(ref MemoryAllocator allocator, TKey key, TValue value, bool add) {

            if (this.buckets(in allocator).isCreated == false) {
                this.Initialize(ref allocator, 0);
            }

            var hashCode = GetHash(key);
            int targetBucket = hashCode % this.buckets(in allocator).Length(in allocator);

            for (int i = this.buckets(in allocator)[in allocator, targetBucket]; i >= 0; i = this.entries(in allocator)[in allocator, i].next) {
                if (this.entries(in allocator)[in allocator, i].hashCode == hashCode && AreEquals(key, this.entries(in allocator)[in allocator, i].key)) {
                    if (add) {
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
                    }

                    this.entries(in allocator)[in allocator, i].value = value;
                    this.version(in allocator)++;
                    return ref this.entries(in allocator)[in allocator, i].value;
                } 

            }
            
            int index;
            if (this.freeCount(in allocator) > 0) {
                index = this.freeList(in allocator);
                this.freeList(in allocator) = this.entries(in allocator)[in allocator, index].next;
                this.freeCount(in allocator)--;
            } else {
                if (this.count(in allocator) == this.entries(in allocator).Length(in allocator)) {
                    this.Resize(ref allocator);
                    targetBucket = hashCode % this.buckets(in allocator).Length(in allocator);
                }
                index = this.count(in allocator);
                this.count(in allocator)++;
            }

            this.entries(in allocator)[in allocator, index].hashCode = hashCode;
            this.entries(in allocator)[in allocator, index].next = this.buckets(in allocator)[in allocator, targetBucket];
            this.entries(in allocator)[in allocator, index].key = key;
            this.entries(in allocator)[in allocator, index].value = value;
            this.buckets(in allocator)[in allocator, targetBucket] = index;
            this.version(in allocator)++;

            return ref this.entries(in allocator)[in allocator, index].value;

        }

        /// <summary>
        /// Initializes buckets and slots arrays. Uses suggested capacity by finding next prime
        /// greater than or equal to capacity.
        /// </summary>
        /// <param name="allocator"></param>
        /// <param name="capacity"></param>
        private void Initialize(ref MemoryAllocator allocator, int capacity) {
            var size = HashHelpers.GetPrime(capacity);
            this.buckets(in allocator) = new MemArrayAllocator<int>(ref allocator, size);
            for (var i = 0; i < this.buckets(in allocator).Length(in allocator); i++) {
                this.buckets(in allocator)[in allocator, i] = -1;
            }

            this.entries(in allocator) = new MemArrayAllocator<Entry>(ref allocator, size);
            this.freeList(in allocator) = -1;
        }

        private void Resize(ref MemoryAllocator allocator) {
            this.Resize(ref allocator, HashHelpers.ExpandPrime(this.count(in allocator)), false);
        }

        private unsafe void Resize(ref MemoryAllocator allocator, int newSize, bool forceNewHashCodes) {

            var newBuckets = new MemArrayAllocator<int>(ref allocator, newSize);
            for (var i = 0; i < newBuckets.Length(in allocator); i++) {
                newBuckets[in allocator, i] = -1;
            }

            var newEntries = new MemArrayAllocator<Entry>(ref allocator, newSize);
            allocator.MemCopy(newEntries.GetMemPtr(in allocator), 0, this.entries(in allocator).GetMemPtr(in allocator), 0, this.count(in allocator) * sizeof(Entry));
            if (forceNewHashCodes) {
                for (var i = 0; i < this.count(in allocator); i++) {
                    if (newEntries[in allocator, i].hashCode != -1) {
                        newEntries[in allocator, i].hashCode = GetHash(newEntries[in allocator, i].key);
                    }
                }
            }

            for (var i = 0; i < this.count(in allocator); i++) {
                if (newEntries[in allocator, i].hashCode >= 0) {
                    var bucket = newEntries[in allocator, i].hashCode % newSize;
                    newEntries[in allocator, i].next = newBuckets[in allocator, bucket];
                    newBuckets[in allocator, bucket] = i;
                }
            }

            if (this.buckets(in allocator).isCreated == true) {
                this.buckets(in allocator).Dispose(ref allocator);
            }

            if (this.entries(in allocator).isCreated == true) {
                this.entries(in allocator).Dispose(ref allocator);
            }

            this.buckets(in allocator) = newBuckets;
            this.entries(in allocator) = newEntries;
        }

        private static int GetHash(TKey key) {

            return key.GetHashCode() & 0x7FFFFFFF;

        }

        private static bool AreEquals(TKey k1, TKey k2) {

            return k1.Equals(k2);

        }
        
        private static bool AreEquals(TValue k1, TValue k2) {

            return k1.Equals(k2);

        }
        #endregion

    }

}