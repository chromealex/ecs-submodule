namespace ME.ECS.Collections.LowLevel {

    using Unsafe;
    
    using INLINE = System.Runtime.CompilerServices.MethodImplAttribute;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ME.ECS.DebugUtils.DictionaryProxyDebugger<,>))]
    public struct Dictionary<TKey, TValue> : IIsCreated where TKey : unmanaged where TValue : unmanaged {

        public struct Enumerator : System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<TKey, TValue>> {

            private readonly State state;
            private ref MemoryAllocator allocator => ref this.state.allocator;
            private readonly Dictionary<TKey, TValue> dictionary;
            private readonly int version;
            private int index;
            private System.Collections.Generic.KeyValuePair<TKey, TValue> current;

            internal Enumerator(State state, Dictionary<TKey, TValue> dictionary) {
                this.state = state;
                this.dictionary = dictionary;
                this.version = dictionary.version;
                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
            }

            public bool MoveNext() {

                if (this.version != this.dictionary.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                while ((uint)this.index < (uint)this.dictionary.count) {
                    ref var local = ref this.dictionary.entries[in this.allocator, this.index++];
                    if (local.hashCode >= 0) {
                        this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>(local.key, local.value);
                        return true;
                    }
                }

                this.index = this.dictionary.count + 1;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
                return false;
            }

            public System.Collections.Generic.KeyValuePair<TKey, TValue> Current => this.current;

            public void Dispose() { }

            object System.Collections.IEnumerator.Current => this.current;

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.dictionary.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
            }

        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<TKey, TValue>> {

            private readonly MemoryAllocator allocator;
            private readonly Dictionary<TKey, TValue> dictionary;
            private readonly int version;
            private int index;
            private System.Collections.Generic.KeyValuePair<TKey, TValue> current;

            internal EnumeratorNoState(in MemoryAllocator allocator, Dictionary<TKey, TValue> dictionary) {
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
                        this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>(this.dictionary.entries[in this.allocator, this.index].key,
                                                                                                 this.dictionary.entries[in this.allocator, this.index].value);
                        this.index++;
                        return true;
                    }

                    this.index++;
                }

                this.index = this.dictionary.count + 1;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
                return false;
            }

            public System.Collections.Generic.KeyValuePair<TKey, TValue> Current => this.current;

            public void Dispose() { }

            object System.Collections.IEnumerator.Current => this.current;

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.dictionary.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>();
            }

        }

        public struct Entry {

            public int hashCode; // Lower 31 bits of hash code, -1 if unused
            public int next; // Index of next entry, -1 if last
            public TKey key; // Key of entry
            public TValue value; // Value of entry

        }

        [ME.ECS.Serializer.SerializeField]
        internal MemArrayAllocator<int> buckets;
        [ME.ECS.Serializer.SerializeField]
        internal MemArrayAllocator<Entry> entries;
        [ME.ECS.Serializer.SerializeField]
        internal int count;
        [ME.ECS.Serializer.SerializeField]
        internal int version;
        [ME.ECS.Serializer.SerializeField]
        internal int freeList;
        [ME.ECS.Serializer.SerializeField]
        internal int freeCount;

        public bool isCreated {
            [INLINE(256)]
            get => this.buckets.isCreated;
        }

        public readonly int Count {
            [INLINE(256)]
            get => this.count - this.freeCount;
        }

        [INLINE(256)]
        public Dictionary(ref MemoryAllocator allocator, int capacity) {

            this = default;
            this.Initialize(ref allocator, capacity);

        }

        [INLINE(256)]
        public void Dispose(ref MemoryAllocator allocator) {

            this.buckets.Dispose(ref allocator);
            this.entries.Dispose(ref allocator);
            this = default;

        }

        [INLINE(256)]
        public readonly MemPtr GetMemPtr(in MemoryAllocator allocator) {
            return this.buckets.arrPtr;

        }

        [INLINE(256)]
        public void ReplaceWith(ref MemoryAllocator allocator, in Dictionary<TKey, TValue> other) {
            
            if (this.GetMemPtr(in allocator) == other.GetMemPtr(in allocator)) return;
            
            this.Dispose(ref allocator);
            this = other;

        }

        [INLINE(256)]
        public void CopyFrom(ref MemoryAllocator allocator, in Dictionary<TKey, TValue> other) {

            if (this.GetMemPtr(in allocator) == other.GetMemPtr(in allocator)) return;
            if (this.GetMemPtr(in allocator) == MemPtr.Null && other.GetMemPtr(in allocator) == MemPtr.Null) return;
            if (this.GetMemPtr(in allocator) != MemPtr.Null && other.GetMemPtr(in allocator) == MemPtr.Null) {
                this.Dispose(ref allocator);
                return;
            }
            if (this.GetMemPtr(in allocator) == MemPtr.Null) this = new Dictionary<TKey, TValue>(ref allocator, other.Count);
            
            NativeArrayUtils.CopyExact(ref allocator, other.buckets, ref this.buckets);
            NativeArrayUtils.CopyExact(ref allocator, other.entries, ref this.entries);
            this.count = other.count;
            this.version = other.version;
            this.freeCount = other.freeCount;
            this.freeList = other.freeList;

        }

        [INLINE(256)]
        public readonly Enumerator GetEnumerator(State state) {
			E.IS_CREATED(this);
            return new Enumerator(state, this);

        }
        
        [INLINE(256)]
        public readonly Enumerator GetEnumerator() {
			E.IS_CREATED(this);
            return this.GetEnumerator(Worlds.current.GetState());
            
        }

        [INLINE(256)]
        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
			E.IS_CREATED(this);
            return new EnumeratorNoState(in allocator, this);

        }

        /// <summary><para>Gets or sets the value associated with the specified key.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key whose value is to be gotten or set.</param>
        public readonly ref TValue this[in MemoryAllocator allocator, TKey key] {
            [INLINE(256)]
            get {
				E.IS_CREATED(this);
                var entry = this.FindEntry(in allocator, key);
                if (entry >= 0) {
                    return ref this.entries[in allocator, entry].value;
                }

                throw new System.Collections.Generic.KeyNotFoundException();
            }
        }

        [INLINE(256)]
        public ref TValue GetValue(ref MemoryAllocator allocator, TKey key) {
			E.IS_CREATED(this);
            var entry = this.FindEntry(in allocator, key);
            if (entry >= 0) {
                return ref this.entries[in allocator, entry].value;
            }

            this.TryInsert(ref allocator, key, default, InsertionBehavior.OverwriteExisting);
            return ref this.entries[in allocator, this.FindEntry(in allocator, key)].value;

        }

        [INLINE(256)]
        public ref TValue GetValue(ref MemoryAllocator allocator, TKey key, out bool exist) {
			E.IS_CREATED(this);
            var entry = this.FindEntry(in allocator, key);
            if (entry >= 0) {
                exist = true;
                return ref this.entries[in allocator, entry].value;
            }

            exist = false;
            this.TryInsert(ref allocator, key, default, InsertionBehavior.OverwriteExisting);
            return ref this.entries[in allocator, this.FindEntry(in allocator, key)].value;

        }

        [INLINE(256)]
        public TValue GetValueAndRemove(ref MemoryAllocator allocator, TKey key) {
			E.IS_CREATED(this);
            this.Remove(ref allocator, key, out var value);
            return value;

        }

        /// <summary><para>Adds an element with the specified key and value to the dictionary.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key of the element to add to the dictionary.</param>
        /// <param name="value"></param>
        [INLINE(256)]
        public void Add(ref MemoryAllocator allocator, TKey key, TValue value) {
			E.IS_CREATED(this);
            this.TryInsert(ref allocator, key, value, InsertionBehavior.ThrowOnExisting);
        }

        /// <summary><para>Removes all elements from the dictionary.</para></summary>
        [INLINE(256)]
        public void Clear(in MemoryAllocator allocator) {
			E.IS_CREATED(this);
            var count = this.count;
            if (count > 0) {
                this.buckets.Clear(in allocator);
                this.count = 0;
                this.freeList = -1;
                this.freeCount = 0;
                this.entries.Clear(in allocator, 0, count);
            }

            ++this.version;
        }

        /// <summary><para>Determines whether the dictionary contains an element with a specific key.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key to locate in the dictionary.</param>
        [INLINE(256)]
        public readonly bool ContainsKey(in MemoryAllocator allocator, TKey key) {
			E.IS_CREATED(this);
            return this.FindEntry(in allocator, key) >= 0;
        }

        /// <summary><para>Determines whether the dictionary contains an element with a specific value.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="value">The value to locate in the dictionary.</param>
        [INLINE(256)]
        public readonly bool ContainsValue(in MemoryAllocator allocator, TValue value) { 
			E.IS_CREATED(this);
            for (var index = 0; index < this.count; ++index) {
                if (this.entries[in allocator, index].hashCode >= 0 && System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(this.entries[in allocator, index].value, value)) {
                    return true;
                }
            }
            return false;
        }

        [INLINE(256)]
        private readonly int FindEntry(in MemoryAllocator allocator, TKey key) {
			E.IS_CREATED(this);
            var index = -1;
            var num1 = 0;
            if (this.buckets.isCreated == true) {
                var comparer = System.Collections.Generic.EqualityComparer<TKey>.Default;
                var num2 = comparer.GetHashCode(key) & int.MaxValue;
                index = this.buckets[in allocator, num2 % this.buckets.Length] - 1;
                while ((uint)index < (uint)this.entries.Length &&
                       (this.entries[in allocator, index].hashCode != num2 || !comparer.Equals(this.entries[in allocator, index].key, key))) {
                    index = this.entries[in allocator, index].next;
                    if (num1 >= this.entries.Length) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.ArgumentOutOfRange_Count);
                    }

                    ++num1;
                }
            }

            return index;
        }

        [INLINE(256)]
        private int Initialize(ref MemoryAllocator allocator, int capacity) {
            var prime = HashHelpers.GetPrime(capacity);
            this.freeList = -1;
            this.buckets = new MemArrayAllocator<int>(ref allocator, prime);
            this.entries = new MemArrayAllocator<Entry>(ref allocator, prime);
            return prime;
        }

        [INLINE(256)]
        private bool TryInsert(ref MemoryAllocator allocator, TKey key, TValue value, InsertionBehavior behavior) {
			E.IS_CREATED(this);
            ++this.version;
            if (this.buckets.isCreated == false) {
                this.Initialize(ref allocator, 0);
            }

            ref var entries = ref this.entries;
            var num1 = System.Collections.Generic.EqualityComparer<TKey>.Default.GetHashCode(key) & int.MaxValue;
            var num2 = 0;
            ref var local1 = ref this.buckets[in allocator, num1 % this.buckets.Length];
            var index1 = local1 - 1;
            {
                while ((uint)index1 < (uint)entries.Length) {
                    if (entries[in allocator, index1].hashCode == num1 &&
                        System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(entries[in allocator, index1].key, key)) {
                        switch (behavior) {
                            case InsertionBehavior.OverwriteExisting:
                                entries[in allocator, index1].value = value;
                                return true;

                            case InsertionBehavior.ThrowOnExisting:
                                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.Argument_AddingDuplicate);
                                break;
                        }

                        return false;
                    }

                    index1 = entries[in allocator, index1].next;
                    if (num2 >= entries.Length) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.ArgumentOutOfRange_Count);
                    }

                    ++num2;
                }
            }
            var flag1 = false;
            var flag2 = false;
            int index2;
            if (this.freeCount > 0) {
                index2 = this.freeList;
                flag2 = true;
                --this.freeCount;
            } else {
                var count = this.count;
                if (count == entries.Length) {
                    this.Resize(ref allocator);
                    flag1 = true;
                }

                index2 = count;
                this.count = count + 1;
                entries = ref this.entries;
            }

            ref var local2 = ref (flag1 ? ref this.buckets[in allocator, num1 % this.buckets.Length] : ref local1);
            ref var local3 = ref entries[in allocator, index2];
            if (flag2) {
                this.freeList = local3.next;
            }

            local3.hashCode = num1;
            local3.next = local2 - 1;
            local3.key = key;
            local3.value = value;
            local2 = index2 + 1;
            return true;
        }

        [INLINE(256)]
        private void Resize(ref MemoryAllocator allocator) {
			E.IS_CREATED(this);
            this.Resize(ref allocator, HashHelpers.ExpandPrime(this.count));
        }

        [INLINE(256)]
        private void Resize(ref MemoryAllocator allocator, int newSize) {
			E.IS_CREATED(this);
            var numArray = new MemArrayAllocator<int>(ref allocator, newSize);
            var entryArray = new MemArrayAllocator<Entry>(ref allocator, newSize);
            var count = this.count;
            NativeArrayUtils.CopyNoChecks(ref allocator, this.entries, 0, ref entryArray, 0, count);
            for (var index1 = 0; index1 < count;  ++index1) {
                if (entryArray[in allocator, index1].hashCode >= 0) {
                    var index2 = entryArray[in allocator, index1].hashCode % newSize;
                    entryArray[in allocator, index1].next = numArray[in allocator, index2] - 1;
                    numArray[in allocator, index2] = index1 + 1;
                }
            }

            if (this.buckets.isCreated == true) {
                this.buckets.Dispose(ref allocator);
            }

            if (this.entries.isCreated == true) {
                this.entries.Dispose(ref allocator);
            }

            this.buckets = numArray;
            this.entries = entryArray;
        }

        /// <summary><para>Removes the element with the specified key from the dictionary.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key of the element to be removed from the dictionary.</param>
        [INLINE(256)]
        public bool Remove(ref MemoryAllocator allocator, TKey key) {
			E.IS_CREATED(this);
            if (this.buckets.isCreated == true) {
                var num = System.Collections.Generic.EqualityComparer<TKey>.Default.GetHashCode(key) & int.MaxValue;
                var index1 = num % this.buckets.Length;
                var index2 = -1;
                // ISSUE: variable of a reference type
                var next = 0;
                for (var index3 = this.buckets[in allocator, index1] - 1; index3 >= 0; index3 = next) {
                    ref var local = ref this.entries[in allocator, index3];
                    next = local.next;
                    if (local.hashCode == num) {
                        if ((System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(local.key, key) ? 1 : 0) != 0) {
                            if (index2 < 0) {
                                this.buckets[in allocator, index1] = local.next + 1;
                            } else {
                                this.entries[in allocator, index2].next = local.next;
                            }

                            local.hashCode = -1;
                            local.next = this.freeList;

                            this.freeList = index3;
                            ++this.freeCount;
                            ++this.version;
                            return true;
                        }
                    }

                    index2 = index3;
                }
            }

            return false;
        }

        /// <summary><para>Removes the element with the specified key from the dictionary.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key of the element to be removed from the dictionary.</param>
        /// <param name="value"></param>
        [INLINE(256)]
        public bool Remove(ref MemoryAllocator allocator, TKey key, out TValue value) {
			E.IS_CREATED(this);
            if (this.buckets.isCreated == true) {
                var num = System.Collections.Generic.EqualityComparer<TKey>.Default.GetHashCode(key) & int.MaxValue;
                var index1 = num % this.buckets.Length;
                var index2 = -1;
                // ISSUE: variable of a reference type
                var next = 0;
                for (var index3 = this.buckets[in allocator, index1] - 1; index3 >= 0; index3 = next) {
                    ref var local = ref this.entries[in allocator, index3];
                    next = local.next;
                    if (local.hashCode == num) {
                        if ((System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(local.key, key) ? 1 : 0) != 0) {
                            if (index2 < 0) {
                                this.buckets[in allocator, index1] = local.next + 1;
                            } else {
                                this.entries[in allocator, index2].next = local.next;
                            }

                            value = local.value;
                            local.hashCode = -1;
                            local.next = this.freeList;

                            this.freeList = index3;
                            ++this.freeCount;
                            ++this.version;
                            return true;
                        }
                    }

                    index2 = index3;
                }
            }

            value = default(TValue);
            return false;
        }

        /// <summary>To be added.</summary>
        /// <param name="allocator"></param>
        /// <param name="key">To be added.</param>
        /// <param name="value"></param>
        [INLINE(256)]
        public readonly bool TryGetValue(in MemoryAllocator allocator, TKey key, out TValue value) {
			E.IS_CREATED(this);
            var entry = this.FindEntry(in allocator, key);
            if (entry >= 0) {
                value = this.entries[in allocator, entry].value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        [INLINE(256)]
        public bool TryAdd(ref MemoryAllocator allocator, TKey key, TValue value) {
			E.IS_CREATED(this);
            return this.TryInsert(ref allocator, key, value, InsertionBehavior.None);
        }

        [INLINE(256)]
        public int EnsureCapacity(ref MemoryAllocator allocator, int capacity) {
			E.IS_CREATED(this);
            if (capacity < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
            }

            var num = this.entries.Length;
            if (num >= capacity) {
                return num;
            }

            if (this.buckets.isCreated == false) {
                return this.Initialize(ref allocator, capacity);
            }

            var prime = HashHelpers.GetPrime(capacity);
            this.Resize(ref allocator, prime);
            return prime;
        }
        
    }

}
