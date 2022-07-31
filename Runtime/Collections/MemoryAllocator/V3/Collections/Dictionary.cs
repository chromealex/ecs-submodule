namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    using Unity.Collections.LowLevel.Unsafe;
    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(DictionaryProxy<,>))]
    public struct Dictionary<TKey, TValue> where TKey : unmanaged where TValue : unmanaged {

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
            private readonly Dictionary<TKey, TValue> dictionary;
            private readonly int version;
            private int index;
            private System.Collections.Generic.KeyValuePair<TKey, TValue> current;

            internal Enumerator(State state, Dictionary<TKey, TValue> dictionary) {
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

                while ((uint)this.index < (uint)this.dictionary.count(in this.allocator)) {
                    ref var local = ref this.dictionary.entries(in this.allocator)[in this.allocator, this.index++];
                    if (local.hashCode >= 0) {
                        this.current = new System.Collections.Generic.KeyValuePair<TKey, TValue>(local.key, local.value);
                        return true;
                    }
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
            private readonly Dictionary<TKey, TValue> dictionary;
            private readonly int version;
            private int index;
            private System.Collections.Generic.KeyValuePair<TKey, TValue> current;

            internal EnumeratorNoState(in MemoryAllocator allocator, Dictionary<TKey, TValue> dictionary) {
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

        public struct Entry {

            public int hashCode; // Lower 31 bits of hash code, -1 if unused
            public int next; // Index of next entry, -1 if last
            public TKey key; // Key of entry
            public TValue value; // Value of entry

        }

        [ME.ECS.Serializer.SerializeField]
        private readonly MemPtr ptr;

        internal readonly ref MemArrayAllocator<int> buckets(in MemoryAllocator allocator) {
            return ref allocator.Ref<InternalData>(this.ptr).buckets;
        }

        internal readonly ref MemArrayAllocator<Entry> entries(in MemoryAllocator allocator) {
            return ref allocator.Ref<InternalData>(this.ptr).entries;
        }

        internal readonly ref int count(in MemoryAllocator allocator) {
            return ref allocator.Ref<InternalData>(this.ptr).count;
        }

        internal readonly ref int version(in MemoryAllocator allocator) {
            return ref allocator.Ref<InternalData>(this.ptr).version;
        }

        internal readonly ref int freeList(in MemoryAllocator allocator) {
            return ref allocator.Ref<InternalData>(this.ptr).freeList;
        }

        internal readonly ref int freeCount(in MemoryAllocator allocator) {
            return ref allocator.Ref<InternalData>(this.ptr).freeCount;
        }

        public bool isCreated => this.ptr != 0;
        public readonly int Count(in MemoryAllocator allocator) {
            return this.count(in allocator) - this.freeCount(in allocator);
        }

        public Dictionary(ref MemoryAllocator allocator, int capacity) {

            this.ptr = allocator.AllocData<InternalData>(default);
            this.Initialize(ref allocator, capacity);

        }

        public void Dispose(ref MemoryAllocator allocator) {

            allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;

        }

        public readonly void CopyFrom(ref MemoryAllocator allocator, in Dictionary<TKey, TValue> other) {

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

        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {

            return new EnumeratorNoState(in allocator, this);

        }

        /// <summary><para>Gets or sets the value associated with the specified key.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key whose value is to be gotten or set.</param>
        public readonly ref TValue this[in MemoryAllocator allocator, TKey key] {
            get {
                var entry = this.FindEntry(in allocator, key);
                if (entry >= 0) {
                    return ref this.entries(in allocator)[in allocator, entry].value;
                }

                throw new System.Collections.Generic.KeyNotFoundException();
            }
        }

        public readonly ref TValue GetValue(ref MemoryAllocator allocator, TKey key) {

            var entry = this.FindEntry(in allocator, key);
            if (entry >= 0) {
                return ref this.entries(in allocator)[in allocator, entry].value;
            }

            this.TryInsert(ref allocator, key, default, InsertionBehavior.OverwriteExisting);
            return ref this.entries(in allocator)[in allocator, this.FindEntry(in allocator, key)].value;

        }

        public readonly ref TValue GetValue(ref MemoryAllocator allocator, TKey key, out bool exist) {
            
            var entry = this.FindEntry(in allocator, key);
            if (entry >= 0) {
                exist = true;
                return ref this.entries(in allocator)[in allocator, entry].value;
            }

            exist = false;
            this.TryInsert(ref allocator, key, default, InsertionBehavior.OverwriteExisting);
            return ref this.entries(in allocator)[in allocator, this.FindEntry(in allocator, key)].value;

        }

        public readonly TValue GetValueAndRemove(ref MemoryAllocator allocator, TKey key) {

            this.Remove(ref allocator, key, out var value);
            return value;

        }

        /// <summary><para>Adds an element with the specified key and value to the dictionary.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key of the element to add to the dictionary.</param>
        /// <param name="value"></param>
        public readonly void Add(ref MemoryAllocator allocator, TKey key, TValue value) {
            this.TryInsert(ref allocator, key, value, InsertionBehavior.ThrowOnExisting);
        }

        /// <summary><para>Removes all elements from the dictionary.</para></summary>
        public readonly void Clear(in MemoryAllocator allocator) {
            var count = this.count(in allocator);
            if (count > 0) {
                this.buckets(in allocator).Clear(in allocator);
                this.count(in allocator) = 0;
                this.freeList(in allocator) = -1;
                this.freeCount(in allocator) = 0;
                this.entries(in allocator).Clear(in allocator, 0, count);
            }

            ++this.version(in allocator);
        }

        /// <summary><para>Determines whether the dictionary contains an element with a specific key.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key to locate in the dictionary.</param>
        public readonly bool ContainsKey(in MemoryAllocator allocator, TKey key) {
            return this.FindEntry(in allocator, key) >= 0;
        }

        /// <summary><para>Determines whether the dictionary contains an element with a specific value.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="value">The value to locate in the dictionary.</param>
        public readonly bool ContainsValue(in MemoryAllocator allocator, TValue value) {
            ref var entries = ref this.entries(in allocator);
            {
                for (var index = 0; index < this.count(in allocator); ++index) {
                    if (entries[in allocator, index].hashCode >= 0 && System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(entries[in allocator, index].value, value)) {
                        return true;
                    }
                }
            }
            return false;
        }

        private readonly int FindEntry(in MemoryAllocator allocator, TKey key) {
            var index = -1;
            ref var buckets = ref this.buckets(in allocator);
            ref var entries = ref this.entries(in allocator);
            var num1 = 0;
            if (buckets.isCreated == true) {
                {
                    var comparer = System.Collections.Generic.EqualityComparer<TKey>.Default;
                    var num2 = comparer.GetHashCode(key) & int.MaxValue;
                    index = buckets[in allocator, num2 % buckets.Length(in allocator)] - 1;
                    while ((uint)index < (uint)entries.Length(in allocator) &&
                           (entries[in allocator, index].hashCode != num2 || !comparer.Equals(entries[in allocator, index].key, key))) {
                        index = entries[in allocator, index].next;
                        if (num1 >= entries.Length(in allocator)) {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.ArgumentOutOfRange_Count);
                        }

                        ++num1;
                    }
                }
            }

            return index;
        }

        private readonly int Initialize(ref MemoryAllocator allocator, int capacity) {
            var prime = HashHelpers.GetPrime(capacity);
            this.freeList(in allocator) = -1;
            this.buckets(in allocator) = new MemArrayAllocator<int>(ref allocator, prime);
            this.entries(in allocator) = new MemArrayAllocator<Entry>(ref allocator, prime);
            return prime;
        }

        private readonly bool TryInsert(ref MemoryAllocator allocator, TKey key, TValue value, InsertionBehavior behavior) {
            ++this.version(in allocator);
            if (this.buckets(in allocator).isCreated == false) {
                this.Initialize(ref allocator, 0);
            }

            ref var entries = ref this.entries(in allocator);
            var num1 = System.Collections.Generic.EqualityComparer<TKey>.Default.GetHashCode(key) & int.MaxValue;
            var num2 = 0;
            ref var local1 = ref this.buckets(in allocator)[in allocator, num1 % this.buckets(in allocator).Length(in allocator)];
            var index1 = local1 - 1;
            {
                {
                    while ((uint)index1 < (uint)entries.Length(in allocator)) {
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
                        if (num2 >= entries.Length(in allocator)) {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.ArgumentOutOfRange_Count);
                        }

                        ++num2;
                    }
                }
            }
            var flag1 = false;
            var flag2 = false;
            int index2;
            if (this.freeCount(in allocator) > 0) {
                index2 = this.freeList(in allocator);
                flag2 = true;
                --this.freeCount(in allocator);
            } else {
                var count = this.count(in allocator);
                if (count == entries.Length(in allocator)) {
                    this.Resize(ref allocator);
                    flag1 = true;
                }

                index2 = count;
                this.count(in allocator) = count + 1;
                entries = ref this.entries(in allocator);
            }

            ref var local2 = ref (flag1 ? ref this.buckets(in allocator)[in allocator, num1 % this.buckets(in allocator).Length(in allocator)] : ref local1);
            ref var local3 = ref entries[in allocator, index2];
            if (flag2) {
                this.freeList(in allocator) = local3.next;
            }

            local3.hashCode = num1;
            local3.next = local2 - 1;
            local3.key = key;
            local3.value = value;
            local2 = index2 + 1;
            return true;
        }

        private readonly void Resize(ref MemoryAllocator allocator) {
            this.Resize(ref allocator, HashHelpers.ExpandPrime(this.count(in allocator)));
        }

        private readonly void Resize(ref MemoryAllocator allocator, int newSize) {
            var numArray = new MemArrayAllocator<int>(ref allocator, newSize);
            var entryArray = new MemArrayAllocator<Entry>(ref allocator, newSize);
            var count = this.count(in allocator);
            NativeArrayUtils.Copy(ref allocator, this.entries(in allocator), 0, ref entryArray, 0, count);
            for (var index1 = 0; index1 < count;  ++index1) {
                if (entryArray[in allocator, index1].hashCode >= 0) {
                    var index2 = entryArray[in allocator, index1].hashCode % newSize;
                    entryArray[in allocator, index1].next = numArray[in allocator, index2] - 1;
                    numArray[in allocator, index2] = index1 + 1;
                }
            }

            if (this.buckets(in allocator).isCreated == true) {
                this.buckets(in allocator).Dispose(ref allocator);
            }

            if (this.entries(in allocator).isCreated == true) {
                this.entries(in allocator).Dispose(ref allocator);
            }

            this.buckets(in allocator) = numArray;
            this.entries(in allocator) = entryArray;
        }

        /// <summary><para>Removes the element with the specified key from the dictionary.</para></summary>
        /// <param name="allocator"></param>
        /// <param name="key">The key of the element to be removed from the dictionary.</param>
        public readonly bool Remove(ref MemoryAllocator allocator, TKey key) {
            if (this.buckets(in allocator).isCreated == true) {
                var num = System.Collections.Generic.EqualityComparer<TKey>.Default.GetHashCode(key) & int.MaxValue;
                var index1 = num % this.buckets(in allocator).Length(in allocator);
                var index2 = -1;
                // ISSUE: variable of a reference type
                var next = 0;
                for (var index3 = this.buckets(in allocator)[in allocator, index1] - 1; index3 >= 0; index3 = next) {
                    ref var local = ref this.entries(in allocator)[in allocator, index3];
                    next = local.next;
                    if (local.hashCode == num) {
                        if ((System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(local.key, key) ? 1 : 0) != 0) {
                            if (index2 < 0) {
                                this.buckets(in allocator)[in allocator, index1] = local.next + 1;
                            } else {
                                this.entries(in allocator)[in allocator, index2].next = local.next;
                            }

                            local.hashCode = -1;
                            local.next = this.freeList(in allocator);
                            if (System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<TKey>()) {
                                local.key = default(TKey);
                            }

                            if (System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<TValue>()) {
                                local.value = default(TValue);
                            }

                            this.freeList(in allocator) = index3;
                            ++this.freeCount(in allocator);
                            ++this.version(in allocator);
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
        public readonly bool Remove(ref MemoryAllocator allocator, TKey key, out TValue value) {
            if (this.buckets(in allocator).isCreated == true) {
                var num = System.Collections.Generic.EqualityComparer<TKey>.Default.GetHashCode(key) & int.MaxValue;
                var index1 = num % this.buckets(in allocator).Length(in allocator);
                var index2 = -1;
                // ISSUE: variable of a reference type
                var next = 0;
                for (var index3 = this.buckets(in allocator)[in allocator, index1] - 1; index3 >= 0; index3 = next) {
                    ref var local = ref this.entries(in allocator)[in allocator, index3];
                    next = local.next;
                    if (local.hashCode == num) {
                        if ((System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(local.key, key) ? 1 : 0) != 0) {
                            if (index2 < 0) {
                                this.buckets(in allocator)[in allocator, index1] = local.next + 1;
                            } else {
                                this.entries(in allocator)[in allocator, index2].next = local.next;
                            }

                            value = local.value;
                            local.hashCode = -1;
                            local.next = this.freeList(in allocator);
                            if (System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<TKey>()) {
                                local.key = default(TKey);
                            }

                            if (System.Runtime.CompilerServices.RuntimeHelpers.IsReferenceOrContainsReferences<TValue>()) {
                                local.value = default(TValue);
                            }

                            this.freeList(in allocator) = index3;
                            ++this.freeCount(in allocator);
                            ++this.version(in allocator);
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
        public readonly bool TryGetValue(in MemoryAllocator allocator, TKey key, out TValue value) {
            var entry = this.FindEntry(in allocator, key);
            if (entry >= 0) {
                value = this.entries(in allocator)[in allocator, entry].value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public readonly bool TryAdd(ref MemoryAllocator allocator, TKey key, TValue value) {
            return this.TryInsert(ref allocator, key, value, InsertionBehavior.None);
        }

        public readonly int EnsureCapacity(ref MemoryAllocator allocator, int capacity) {
            if (capacity < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
            }

            var num = this.entries(in allocator).Length(in allocator);
            if (num >= capacity) {
                return num;
            }

            if (this.buckets(in allocator).isCreated == false) {
                return this.Initialize(ref allocator, capacity);
            }

            var prime = HashHelpers.GetPrime(capacity);
            this.Resize(ref allocator, prime);
            return prime;
        }

    }

}