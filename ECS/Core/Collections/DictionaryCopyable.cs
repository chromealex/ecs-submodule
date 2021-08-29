namespace ME.ECS.Collections {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;

    public enum InsertionBehavior {

        None,
        ThrowOnExisting,
        OverwriteExisting,

    }

    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class DictionaryCopyable<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>, ISerializable {

        private struct Entry {

            public int hashCode; // Lower 31 bits of hash code, -1 if unused
            public int next; // Index of next entry, -1 if last
            public TKey key; // Key of entry
            public TValue value; // Value of entry

        }
        
        [ME.ECS.Serializer.SerializeField]
        private int[] _buckets;
        [ME.ECS.Serializer.SerializeField]
        private Entry[] _entries;
        [ME.ECS.Serializer.SerializeField]
        private int _count;
        [ME.ECS.Serializer.SerializeField]
        private int _version;
        [ME.ECS.Serializer.SerializeField]
        private int _freeList;
        [ME.ECS.Serializer.SerializeField]
        private int _freeCount;
        [ME.ECS.Serializer.SerializeField]
        private IEqualityComparer<TKey> _comparer;
        [ME.ECS.Serializer.SerializeField]
        private KeyCollection _keys;
        [ME.ECS.Serializer.SerializeField]
        private ValueCollection _values;

        private Object _syncRoot;

        // constants for serialization
        private const String VersionName = "Version";
        private const String HashSizeName = "HashSize"; // Must save buckets.Length
        private const String KeyValuePairsName = "KeyValuePairs";
        private const String ComparerName = "Comparer";

        public DictionaryCopyable() : this(0, null) { }

        public DictionaryCopyable(int capacity) : this(capacity, null) { }

        public DictionaryCopyable(IEqualityComparer<TKey> comparer) : this(0, comparer) { }

        public DictionaryCopyable(int capacity, IEqualityComparer<TKey> comparer) {
            if (capacity > 0) {
                this.Initialize(capacity);
            }

            this._comparer = comparer ?? EqualityComparer<TKey>.Default;

            #if FEATURE_CORECLR
            if (HashHelpers.s_UseRandomizedStringHashing && comparer == EqualityComparer<string>.Default)
            {
                this.comparer = (IEqualityComparer<TKey>) NonRandomizedStringEqualityComparer.Default;
            }
            #endif // FEATURE_CORECLR
        }

        public DictionaryCopyable(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer = null) : this(dictionary?.Count ?? 0, comparer) {
            if (dictionary == null) {
                throw new ArgumentNullException();
            }

            foreach (var pair in dictionary) {
                this.Add(pair.Key, pair.Value);
            }
        }

        public IEqualityComparer<TKey> Comparer => this._comparer;

        public int Count => this._count - this._freeCount;

        public KeyCollection Keys {
            get {
                Contract.Ensures(Contract.Result<KeyCollection>() != null);
                if (this._keys == null) {
                    this._keys = new KeyCollection(this);
                }

                return this._keys;
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys {
            get {
                if (this._keys == null) {
                    this._keys = new KeyCollection(this);
                }

                return this._keys;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys {
            get {
                if (this._keys == null) {
                    this._keys = new KeyCollection(this);
                }

                return this._keys;
            }
        }

        public ValueCollection Values {
            get {
                Contract.Ensures(Contract.Result<ValueCollection>() != null);
                if (this._values == null) {
                    this._values = new ValueCollection(this);
                }

                return this._values;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values {
            get {
                if (this._values == null) {
                    this._values = new ValueCollection(this);
                }

                return this._values;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values {
            get {
                if (this._values == null) {
                    this._values = new ValueCollection(this);
                }

                return this._values;
            }
        }

        public TValue this[TKey key] {
            get {
                var i = this.FindEntry(key);
                if (i >= 0) {
                    return this._entries[i].value;
                }

                throw new KeyNotFoundException();
            }
            set => this.Insert(key, value, false);
        }

        public ref TValue Get(TKey key) {
            var i = this.FindEntry(key);
            if (i >= 0) {
                return ref this._entries[i].value;
            }

            this.Insert(key, default, false);
            return ref this.Get(key);
        }

        public void Add(TKey key, TValue value) {
            this.Insert(key, value, true);
        }

        public void CopyFrom(DictionaryCopyable<TKey, TValue> other) {

            this.Clear();
            foreach (var item in other) {

                this.Add(item.Key, item.Value);

            }

            this._version = other._version;

        }

        public void CopyFrom<TElementCopy>(DictionaryCopyable<TKey, TValue> other, TElementCopy copy) where TElementCopy : IArrayElementCopy<TValue> {

            this.Clear(copy);
            foreach (var item in other) {

                this.Insert(item.Key, item.Value, true, copy);

            }

            this._version = other._version;

        }

        public void Clear<TElementCopy>(TElementCopy copy) where TElementCopy : IArrayElementCopy<TValue> {

            foreach (var item in this) {
                copy.Recycle(item.Value);
            }

            this.Clear();

        }

        private bool TryInsert<TElementCopy>(TKey key, TValue value, InsertionBehavior behavior, TElementCopy copy) where TElementCopy : IArrayElementCopy<TValue> {
            ++this._version;
            if (this._buckets == null) {
                this.Initialize(0);
            }

            var entries = this._entries;
            var comparer = this._comparer;
            var num1 = (comparer == null ? key.GetHashCode() : comparer.GetHashCode(key)) & int.MaxValue;
            var num2 = 0;
            ref var local1 = ref this._buckets[num1 % this._buckets.Length];
            var index1 = local1 - 1;
            if (comparer == null) {
                if ((object)default(TKey) != null) {
                    while ((uint)index1 < (uint)entries.Length) {
                        if (entries[index1].hashCode == num1 && EqualityComparer<TKey>.Default.Equals(entries[index1].key, key)) {
                            switch (behavior) {
                                case InsertionBehavior.OverwriteExisting:
                                    copy.Copy(value, ref entries[index1].value);
                                    return true;

                                case InsertionBehavior.ThrowOnExisting:
                                    throw new NotSupportedException();
                            }

                            return false;
                        }

                        index1 = entries[index1].next;
                        if (num2 >= entries.Length) {
                            throw new NotSupportedException();
                        }

                        ++num2;
                    }
                } else {
                    var equalityComparer = EqualityComparer<TKey>.Default;
                    while ((uint)index1 < (uint)entries.Length) {
                        if (entries[index1].hashCode == num1 && equalityComparer.Equals(entries[index1].key, key)) {
                            switch (behavior) {
                                case InsertionBehavior.OverwriteExisting:
                                    copy.Copy(value, ref entries[index1].value);
                                    return true;

                                case InsertionBehavior.ThrowOnExisting:
                                    throw new NotSupportedException();
                            }

                            return false;
                        }

                        index1 = entries[index1].next;
                        if (num2 >= entries.Length) {
                            throw new NotSupportedException();
                        }

                        ++num2;
                    }
                }
            } else {
                while ((uint)index1 < (uint)entries.Length) {
                    if (entries[index1].hashCode == num1 && comparer.Equals(entries[index1].key, key)) {
                        switch (behavior) {
                            case InsertionBehavior.OverwriteExisting:
                                copy.Copy(value, ref entries[index1].value);
                                return true;

                            case InsertionBehavior.ThrowOnExisting:
                                throw new NotSupportedException();
                        }

                        return false;
                    }

                    index1 = entries[index1].next;
                    if (num2 >= entries.Length) {
                        throw new NotSupportedException();
                    }

                    ++num2;
                }
            }

            var flag1 = false;
            var flag2 = false;
            int index2;
            if (this._freeCount > 0) {
                index2 = this._freeList;
                flag2 = true;
                --this._freeCount;
            } else {
                var count = this._count;
                if (count == entries.Length) {
                    this.Resize();
                    flag1 = true;
                }

                index2 = count;
                this._count = count + 1;
                entries = this._entries;
            }

            ref var local2 = ref flag1 ? ref this._buckets[num1 % this._buckets.Length] : ref local1;
            ref var local3 = ref entries[index2];
            if (flag2) {
                this._freeList = local3.next;
            }

            local3.hashCode = num1;
            local3.next = local2 - 1;
            local3.key = key;
            copy.Copy(value, ref local3.value);
            local2 = index2 + 1;
            return true;
        }

        private bool TryInsert(TKey key, TValue value, InsertionBehavior behavior) {
            ++this._version;
            if (this._buckets == null) {
                this.Initialize(0);
            }

            var entries = this._entries;
            var comparer = this._comparer;
            var num1 = (comparer == null ? key.GetHashCode() : comparer.GetHashCode(key)) & int.MaxValue;
            var num2 = 0;
            ref var local1 = ref this._buckets[num1 % this._buckets.Length];
            var index1 = local1 - 1;
            if (comparer == null) {
                if ((object)default(TKey) != null) {
                    while ((uint)index1 < (uint)entries.Length) {
                        if (entries[index1].hashCode == num1 && EqualityComparer<TKey>.Default.Equals(entries[index1].key, key)) {
                            switch (behavior) {
                                case InsertionBehavior.OverwriteExisting:
                                    entries[index1].value = value;
                                    return true;

                                case InsertionBehavior.ThrowOnExisting:
                                    throw new NotSupportedException();
                            }

                            return false;
                        }

                        index1 = entries[index1].next;
                        if (num2 >= entries.Length) {
                            throw new NotSupportedException();
                        }

                        ++num2;
                    }
                } else {
                    var equalityComparer = EqualityComparer<TKey>.Default;
                    while ((uint)index1 < (uint)entries.Length) {
                        if (entries[index1].hashCode == num1 && equalityComparer.Equals(entries[index1].key, key)) {
                            switch (behavior) {
                                case InsertionBehavior.OverwriteExisting:
                                    entries[index1].value = value;
                                    return true;

                                case InsertionBehavior.ThrowOnExisting:
                                    throw new NotSupportedException();
                            }

                            return false;
                        }

                        index1 = entries[index1].next;
                        if (num2 >= entries.Length) {
                            throw new NotSupportedException();
                        }

                        ++num2;
                    }
                }
            } else {
                while ((uint)index1 < (uint)entries.Length) {
                    if (entries[index1].hashCode == num1 && comparer.Equals(entries[index1].key, key)) {
                        switch (behavior) {
                            case InsertionBehavior.OverwriteExisting:
                                entries[index1].value = value;
                                return true;

                            case InsertionBehavior.ThrowOnExisting:
                                throw new NotSupportedException();
                        }

                        return false;
                    }

                    index1 = entries[index1].next;
                    if (num2 >= entries.Length) {
                        throw new NotSupportedException();
                    }

                    ++num2;
                }
            }

            var flag1 = false;
            var flag2 = false;
            int index2;
            if (this._freeCount > 0) {
                index2 = this._freeList;
                flag2 = true;
                --this._freeCount;
            } else {
                var count = this._count;
                if (count == entries.Length) {
                    this.Resize();
                    flag1 = true;
                }

                index2 = count;
                this._count = count + 1;
                entries = this._entries;
            }

            ref var local2 = ref flag1 ? ref this._buckets[num1 % this._buckets.Length] : ref local1;
            ref var local3 = ref entries[index2];
            if (flag2) {
                this._freeList = local3.next;
            }

            local3.hashCode = num1;
            local3.next = local2 - 1;
            local3.key = key;
            local3.value = value;
            local2 = index2 + 1;
            return true;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair) {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair) {
            var i = this.FindEntry(keyValuePair.Key);
            if (i >= 0 && EqualityComparer<TValue>.Default.Equals(this._entries[i].value, keyValuePair.Value)) {
                return true;
            }

            return false;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair) {
            var i = this.FindEntry(keyValuePair.Key);
            if (i >= 0 && EqualityComparer<TValue>.Default.Equals(this._entries[i].value, keyValuePair.Value)) {
                this.Remove(keyValuePair.Key);
                return true;
            }

            return false;
        }

        public void Clear() {
            if (this._count > 0) {
                for (var i = 0; i < this._buckets.Length; i++) {
                    this._buckets[i] = -1;
                }

                Array.Clear(this._entries, 0, this._count);
                this._freeList = -1;
                this._count = 0;
                this._freeCount = 0;
                this._version++;
            }
        }

        public bool ContainsKey(TKey key) {
            return this.FindEntry(key) >= 0;
        }

        public bool ContainsValue(TValue value) {
            if (value == null) {
                for (var i = 0; i < this._count; i++) {
                    if (this._entries[i].hashCode >= 0 && this._entries[i].value == null) {
                        return true;
                    }
                }
            } else {
                var c = EqualityComparer<TValue>.Default;
                for (var i = 0; i < this._count; i++) {
                    if (this._entries[i].hashCode >= 0 && c.Equals(this._entries[i].value, value)) {
                        return true;
                    }
                }
            }

            return false;
        }

        private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) {
            if (array == null) {

                throw new ArgumentNullException();
            }

            if (index < 0 || index > array.Length) {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Length - index < this.Count) {
                throw new ArgumentException();
            }

            var count = this._count;
            var entries = this._entries;
            for (var i = 0; i < count; i++) {
                if (entries[i].hashCode >= 0) {
                    array[index++] = new KeyValuePair<TKey, TValue>(entries[i].key, entries[i].value);
                }
            }
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(this, Enumerator.KeyValuePair);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() {
            return new Enumerator(this, Enumerator.KeyValuePair);
        }

        [System.Security.SecurityCritical] // auto-generated_required
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException();
            }

            info.AddValue(DictionaryCopyable<TKey, TValue>.VersionName, this._version);

            #if FEATURE_RANDOMIZED_STRING_HASHING
            info.AddValue(ComparerName, HashHelpers.GetEqualityComparerForSerialization(comparer), typeof(IEqualityComparer<TKey>));
            #else
            info.AddValue(DictionaryCopyable<TKey, TValue>.ComparerName, this._comparer, typeof(IEqualityComparer<TKey>));
            #endif

            info.AddValue(DictionaryCopyable<TKey, TValue>.HashSizeName, this._buckets == null ? 0 : this._buckets.Length); //This is the length of the bucket array.
            if (this._buckets != null) {
                var array = new KeyValuePair<TKey, TValue>[this.Count];
                this.CopyTo(array, 0);
                info.AddValue(DictionaryCopyable<TKey, TValue>.KeyValuePairsName, array, typeof(KeyValuePair<TKey, TValue>[]));
            }
        }

        private int FindEntry(TKey key) {
            /*if (key == default) {
                throw new ArgumentNullException();
            }*/

            if (this._buckets != null) {
                var hashCode = this._comparer.GetHashCode(key) & 0x7FFFFFFF;
                for (var i = this._buckets[hashCode % this._buckets.Length]; i >= 0; i = this._entries[i].next) {
                    if (this._entries[i].hashCode == hashCode && this._comparer.Equals(this._entries[i].key, key)) {
                        return i;
                    }
                }
            }

            return -1;
        }

        private void Initialize(int capacity) {
            var size = HashHelpers.GetPrime(capacity);
            this._buckets = new int[size];
            for (var i = 0; i < this._buckets.Length; i++) {
                this._buckets[i] = -1;
            }

            this._entries = new Entry[size];
            this._freeList = -1;
        }

        private void Insert<TElementCopy>(TKey key, TValue value, bool add, TElementCopy copy) where TElementCopy : IArrayElementCopy<TValue> {

            if (key == null) {
                throw new ArgumentNullException();
            }

            if (this._buckets == null) {
                this.Initialize(0);
            }

            var hashCode = this._comparer.GetHashCode(key) & 0x7FFFFFFF;
            var targetBucket = hashCode % this._buckets.Length;

            #if FEATURE_RANDOMIZED_STRING_HASHING
            int collisionCount = 0;
            #endif

            for (var i = this._buckets[targetBucket]; i >= 0; i = this._entries[i].next) {
                if (this._entries[i].hashCode == hashCode && this._comparer.Equals(this._entries[i].key, key)) {
                    if (add) {
                        throw new ArgumentException();
                    }

                    copy.Copy(value, ref this._entries[i].value);
                    this._version++;
                    return;
                }

                #if FEATURE_RANDOMIZED_STRING_HASHING
                collisionCount++;
                #endif
            }

            int index;
            if (this._freeCount > 0) {
                index = this._freeList;
                this._freeList = this._entries[index].next;
                this._freeCount--;
            } else {
                if (this._count == this._entries.Length) {
                    this.Resize();
                    targetBucket = hashCode % this._buckets.Length;
                }

                index = this._count;
                this._count++;
            }

            this._entries[index].hashCode = hashCode;
            this._entries[index].next = this._buckets[targetBucket];
            this._entries[index].key = key;
			copy.Copy(value, ref this._entries[index].value);
            this._buckets[targetBucket] = index;
            this._version++;

            #if FEATURE_RANDOMIZED_STRING_HASHING
#if FEATURE_CORECLR
            // In case we hit the collision threshold we'll need to switch to the comparer which is using randomized string hashing
            // in this case will be EqualityComparer<string>.Default.
            // Note, randomized string hashing is turned on by default on coreclr so EqualityComparer<string>.Default will
            // be using randomized string hashing

            if (collisionCount > HashHelpers.HashCollisionThreshold && comparer == NonRandomizedStringEqualityComparer.Default)
            {
                comparer = (IEqualityComparer<TKey>) EqualityComparer<string>.Default;
                Resize(entries.Length, true);
            }
#else
            if(collisionCount > HashHelpers.HashCollisionThreshold && HashHelpers.IsWellKnownEqualityComparer(comparer))
            {
                comparer = (IEqualityComparer<TKey>) HashHelpers.GetRandomizedEqualityComparer(comparer);
                Resize(entries.Length, true);
            }
#endif // FEATURE_CORECLR

            #endif

        }

        private void Insert(TKey key, TValue value, bool add) {

            if (key == null) {
                throw new ArgumentNullException();
            }

            if (this._buckets == null) {
                this.Initialize(0);
            }

            var hashCode = this._comparer.GetHashCode(key) & 0x7FFFFFFF;
            var targetBucket = hashCode % this._buckets.Length;

            #if FEATURE_RANDOMIZED_STRING_HASHING
            int collisionCount = 0;
            #endif

            for (var i = this._buckets[targetBucket]; i >= 0; i = this._entries[i].next) {
                if (this._entries[i].hashCode == hashCode && this._comparer.Equals(this._entries[i].key, key)) {
                    if (add) {
                        throw new ArgumentException();
                    }

                    this._entries[i].value = value;
                    this._version++;
                    return;
                }

                #if FEATURE_RANDOMIZED_STRING_HASHING
                collisionCount++;
                #endif
            }

            int index;
            if (this._freeCount > 0) {
                index = this._freeList;
                this._freeList = this._entries[index].next;
                this._freeCount--;
            } else {
                if (this._count == this._entries.Length) {
                    this.Resize();
                    targetBucket = hashCode % this._buckets.Length;
                }

                index = this._count;
                this._count++;
            }

            this._entries[index].hashCode = hashCode;
            this._entries[index].next = this._buckets[targetBucket];
            this._entries[index].key = key;
            this._entries[index].value = value;
            this._buckets[targetBucket] = index;
            this._version++;

            #if FEATURE_RANDOMIZED_STRING_HASHING
#if FEATURE_CORECLR
            // In case we hit the collision threshold we'll need to switch to the comparer which is using randomized string hashing
            // in this case will be EqualityComparer<string>.Default.
            // Note, randomized string hashing is turned on by default on coreclr so EqualityComparer<string>.Default will 
            // be using randomized string hashing
 
            if (collisionCount > HashHelpers.HashCollisionThreshold && comparer == NonRandomizedStringEqualityComparer.Default) 
            {
                comparer = (IEqualityComparer<TKey>) EqualityComparer<string>.Default;
                Resize(entries.Length, true);
            }
#else
            if(collisionCount > HashHelpers.HashCollisionThreshold && HashHelpers.IsWellKnownEqualityComparer(comparer)) 
            {
                comparer = (IEqualityComparer<TKey>) HashHelpers.GetRandomizedEqualityComparer(comparer);
                Resize(entries.Length, true);
            }
#endif // FEATURE_CORECLR

            #endif

        }

        private void Resize() {
            this.Resize(HashHelpers.ExpandPrime(this._count), false);
        }

        private void Resize(int newSize, bool forceNewHashCodes) {
            Contract.Assert(newSize >= this._entries.Length);
            var newBuckets = new int[newSize];
            for (var i = 0; i < newBuckets.Length; i++) {
                newBuckets[i] = -1;
            }

            var newEntries = new Entry[newSize];
            Array.Copy(this._entries, 0, newEntries, 0, this._count);
            if (forceNewHashCodes) {
                for (var i = 0; i < this._count; i++) {
                    if (newEntries[i].hashCode != -1) {
                        newEntries[i].hashCode = this._comparer.GetHashCode(newEntries[i].key) & 0x7FFFFFFF;
                    }
                }
            }

            for (var i = 0; i < this._count; i++) {
                if (newEntries[i].hashCode >= 0) {
                    var bucket = newEntries[i].hashCode % newSize;
                    newEntries[i].next = newBuckets[bucket];
                    newBuckets[bucket] = i;
                }
            }

            this._buckets = newBuckets;
            this._entries = newEntries;
        }

        public bool Remove(TKey key) {
            if (key == null) {
                throw new ArgumentNullException();
            }

            if (this._buckets != null) {
                var hashCode = this._comparer.GetHashCode(key) & 0x7FFFFFFF;
                var bucket = hashCode % this._buckets.Length;
                var last = -1;
                for (var i = this._buckets[bucket]; i >= 0; last = i, i = this._entries[i].next) {
                    if (this._entries[i].hashCode == hashCode && this._comparer.Equals(this._entries[i].key, key)) {
                        if (last < 0) {
                            this._buckets[bucket] = this._entries[i].next;
                        } else {
                            this._entries[last].next = this._entries[i].next;
                        }

                        this._entries[i].hashCode = -1;
                        this._entries[i].next = this._freeList;
                        this._entries[i].key = default(TKey);
                        this._entries[i].value = default(TValue);
                        this._freeList = i;
                        this._freeCount++;
                        this._version++;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value) {
            var i = this.FindEntry(key);
            if (i >= 0) {
                value = this._entries[i].value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        // This is a convenience method for the internal callers that were converted from using Hashtable.
        // Many were combining key doesn't exist and key exists but null value (for non-value types) checks.
        // This allows them to continue getting that behavior with minimal code delta. This is basically
        // TryGetValue without the out param
        internal TValue GetValueOrDefault(TKey key) {
            var i = this.FindEntry(key);
            if (i >= 0) {
                return this._entries[i].value;
            }

            return default(TValue);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index) {
            this.CopyTo(array, index);
        }

        void ICollection.CopyTo(Array array, int index) {
            if (array == null) {
                throw new ArgumentNullException();
            }

            if (array.Rank != 1) {
                throw new ArgumentException();
            }

            if (array.GetLowerBound(0) != 0) {
                throw new ArgumentException();
            }

            if (index < 0 || index > array.Length) {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Length - index < this.Count) {
                throw new ArgumentException();
            }

            var pairs = array as KeyValuePair<TKey, TValue>[];
            if (pairs != null) {
                this.CopyTo(pairs, index);
            } else if (array is DictionaryEntry[]) {
                var dictEntryArray = array as DictionaryEntry[];
                var entries = this._entries;
                for (var i = 0; i < this._count; i++) {
                    if (entries[i].hashCode >= 0) {
                        dictEntryArray[index++] = new DictionaryEntry(entries[i].key, entries[i].value);
                    }
                }
            } else {
                var objects = array as object[];
                if (objects == null) {
                    throw new ArgumentException();
                }

                try {
                    var count = this._count;
                    var entries = this._entries;
                    for (var i = 0; i < count; i++) {
                        if (entries[i].hashCode >= 0) {
                            objects[index++] = new KeyValuePair<TKey, TValue>(entries[i].key, entries[i].value);
                        }
                    }
                } catch (ArrayTypeMismatchException) {
                    throw new ArgumentException();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return new Enumerator(this, Enumerator.KeyValuePair);
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot {
            get {
                if (this._syncRoot == null) {
                    System.Threading.Interlocked.CompareExchange<Object>(ref this._syncRoot, new Object(), null);
                }

                return this._syncRoot;
            }
        }

        bool IDictionary.IsFixedSize => false;

        bool IDictionary.IsReadOnly => false;

        ICollection IDictionary.Keys => (ICollection)this.Keys;

        ICollection IDictionary.Values => (ICollection)this.Values;

        object IDictionary.this[object key] {
            get {
                if (DictionaryCopyable<TKey, TValue>.IsCompatibleKey(key)) {
                    var i = this.FindEntry((TKey)key);
                    if (i >= 0) {
                        return this._entries[i].value;
                    }
                }

                return null;
            }
            set {
                if (key == null) {
                    throw new ArgumentNullException();
                }

                if (value == null && !(default(TValue) == null)) {
                    throw new ArgumentNullException();
                }

                try {
                    var tempKey = (TKey)key;
                    try {
                        this[tempKey] = (TValue)value;
                    } catch (InvalidCastException) {
                        throw new ArgumentException();
                    }
                } catch (InvalidCastException) {
                    throw new ArgumentException();
                }
            }
        }

        private static bool IsCompatibleKey(object key) {
            if (key == null) {
                throw new ArgumentNullException();
            }

            return key is TKey;
        }

        void IDictionary.Add(object key, object value) {
            if (key == null) {
                throw new ArgumentNullException();
            }

            if (value == null && !(default(TValue) == null)) {
                throw new ArgumentNullException();
            }

            try {
                var tempKey = (TKey)key;

                try {
                    this.Add(tempKey, (TValue)value);
                } catch (InvalidCastException) {
                    throw new ArgumentException();
                }
            } catch (InvalidCastException) {
                throw new ArgumentException();
            }
        }

        bool IDictionary.Contains(object key) {
            if (DictionaryCopyable<TKey, TValue>.IsCompatibleKey(key)) {
                return this.ContainsKey((TKey)key);
            }

            return false;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() {
            return new Enumerator(this, Enumerator.DictEntry);
        }

        void IDictionary.Remove(object key) {
            if (DictionaryCopyable<TKey, TValue>.IsCompatibleKey(key)) {
                this.Remove((TKey)key);
            }
        }

        [Serializable]
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator {

            private DictionaryCopyable<TKey, TValue> dictionary;
            private int version;
            private int index;
            private KeyValuePair<TKey, TValue> current;
            private int getEnumeratorRetType; // What should Enumerator.Current return?

            internal const int DictEntry = 1;
            internal const int KeyValuePair = 2;

            internal Enumerator(DictionaryCopyable<TKey, TValue> dictionary, int getEnumeratorRetType) {
                this.dictionary = dictionary;
                this.version = dictionary._version;
                this.index = 0;
                this.getEnumeratorRetType = getEnumeratorRetType;
                this.current = new KeyValuePair<TKey, TValue>();
            }

            public bool MoveNext() {
                if (this.version != this.dictionary._version) {
                    throw new InvalidOperationException();
                }

                // Use unsigned comparison since we set index to dictionary.count+1 when the enumeration ends.
                // dictionary.count+1 could be negative if dictionary.count is Int32.MaxValue
                while ((uint)this.index < (uint)this.dictionary._count) {
                    if (this.dictionary._entries[this.index].hashCode >= 0) {
                        this.current = new KeyValuePair<TKey, TValue>(this.dictionary._entries[this.index].key, this.dictionary._entries[this.index].value);
                        this.index++;
                        return true;
                    }

                    this.index++;
                }

                this.index = this.dictionary._count + 1;
                this.current = new KeyValuePair<TKey, TValue>();
                return false;
            }

            public KeyValuePair<TKey, TValue> Current => this.current;

            public void Dispose() { }

            object IEnumerator.Current {
                get {
                    if (this.index == 0 || this.index == this.dictionary._count + 1) {
                        throw new InvalidOperationException();
                    }

                    if (this.getEnumeratorRetType == DictionaryCopyable<TKey, TValue>.Enumerator.DictEntry) {
                        return new System.Collections.DictionaryEntry(this.current.Key, this.current.Value);
                    } else {
                        return new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
                    }
                }
            }

            void IEnumerator.Reset() {
                if (this.version != this.dictionary._version) {
                    throw new InvalidOperationException();
                }

                this.index = 0;
                this.current = new KeyValuePair<TKey, TValue>();
            }

            DictionaryEntry IDictionaryEnumerator.Entry {
                get {
                    if (this.index == 0 || this.index == this.dictionary._count + 1) {
                        throw new InvalidOperationException();
                    }

                    return new DictionaryEntry(this.current.Key, this.current.Value);
                }
            }

            object IDictionaryEnumerator.Key {
                get {
                    if (this.index == 0 || this.index == this.dictionary._count + 1) {
                        throw new InvalidOperationException();
                    }

                    return this.current.Key;
                }
            }

            object IDictionaryEnumerator.Value {
                get {
                    if (this.index == 0 || this.index == this.dictionary._count + 1) {
                        throw new InvalidOperationException();
                    }

                    return this.current.Value;
                }
            }

        }

        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class KeyCollection : ICollection<TKey>, ICollection, IReadOnlyCollection<TKey> {

            private DictionaryCopyable<TKey, TValue> dictionary;

            public KeyCollection(DictionaryCopyable<TKey, TValue> dictionary) {
                if (dictionary == null) {
                    throw new ArgumentNullException();
                }

                this.dictionary = dictionary;
            }

            public Enumerator GetEnumerator() {
                return new Enumerator(this.dictionary);
            }

            public void CopyTo(TKey[] array, int index) {
                if (array == null) {
                    throw new ArgumentNullException();
                }

                if (index < 0 || index > array.Length) {
                    throw new ArgumentOutOfRangeException();
                }

                if (array.Length - index < this.dictionary.Count) {
                    throw new ArgumentOutOfRangeException();
                }

                var count = this.dictionary._count;
                var entries = this.dictionary._entries;
                for (var i = 0; i < count; i++) {
                    if (entries[i].hashCode >= 0) {
                        array[index++] = entries[i].key;
                    }
                }
            }

            public int Count => this.dictionary.Count;

            bool ICollection<TKey>.IsReadOnly => true;

            void ICollection<TKey>.Add(TKey item) {
                throw new NotSupportedException();
            }

            void ICollection<TKey>.Clear() {
                throw new NotSupportedException();
            }

            bool ICollection<TKey>.Contains(TKey item) {
                return this.dictionary.ContainsKey(item);
            }

            bool ICollection<TKey>.Remove(TKey item) {
                throw new NotSupportedException();
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() {
                return new Enumerator(this.dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return new Enumerator(this.dictionary);
            }

            void ICollection.CopyTo(Array array, int index) {
                if (array == null) {
                    throw new ArgumentNullException();
                }

                if (array.Rank != 1) {
                    throw new ArgumentException();
                }

                if (array.GetLowerBound(0) != 0) {
                    throw new ArgumentException();
                }

                if (index < 0 || index > array.Length) {
                    throw new ArgumentOutOfRangeException();
                }

                if (array.Length - index < this.dictionary.Count) {
                    throw new ArgumentException();
                }

                var keys = array as TKey[];
                if (keys != null) {
                    this.CopyTo(keys, index);
                } else {
                    var objects = array as object[];
                    if (objects == null) {
                        throw new ArgumentException();
                    }

                    var count = this.dictionary._count;
                    var entries = this.dictionary._entries;
                    try {
                        for (var i = 0; i < count; i++) {
                            if (entries[i].hashCode >= 0) {
                                objects[index++] = entries[i].key;
                            }
                        }
                    } catch (ArrayTypeMismatchException) {
                        throw new ArgumentException();
                    }
                }
            }

            bool ICollection.IsSynchronized => false;

            Object ICollection.SyncRoot => ((ICollection)this.dictionary).SyncRoot;

            [Serializable]
            public struct Enumerator : IEnumerator<TKey>, System.Collections.IEnumerator {

                private DictionaryCopyable<TKey, TValue> dictionary;
                private int index;
                private int version;
                private TKey currentKey;

                internal Enumerator(DictionaryCopyable<TKey, TValue> dictionary) {
                    this.dictionary = dictionary;
                    this.version = dictionary._version;
                    this.index = 0;
                    this.currentKey = default(TKey);
                }

                public void Dispose() { }

                public bool MoveNext() {
                    if (this.version != this.dictionary._version) {
                        throw new InvalidOperationException();
                    }

                    while ((uint)this.index < (uint)this.dictionary._count) {
                        if (this.dictionary._entries[this.index].hashCode >= 0) {
                            this.currentKey = this.dictionary._entries[this.index].key;
                            this.index++;
                            return true;
                        }

                        this.index++;
                    }

                    this.index = this.dictionary._count + 1;
                    this.currentKey = default(TKey);
                    return false;
                }

                public TKey Current => this.currentKey;

                Object System.Collections.IEnumerator.Current {
                    get {
                        if (this.index == 0 || this.index == this.dictionary._count + 1) {
                            throw new InvalidOperationException();
                        }

                        return this.currentKey;
                    }
                }

                void System.Collections.IEnumerator.Reset() {
                    if (this.version != this.dictionary._version) {
                        throw new InvalidOperationException();
                    }

                    this.index = 0;
                    this.currentKey = default(TKey);
                }

            }

        }

        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class ValueCollection : ICollection<TValue>, ICollection, IReadOnlyCollection<TValue> {

            private DictionaryCopyable<TKey, TValue> dictionary;

            public ValueCollection(DictionaryCopyable<TKey, TValue> dictionary) {
                if (dictionary == null) {
                    throw new ArgumentNullException();
                }

                this.dictionary = dictionary;
            }

            public Enumerator GetEnumerator() {
                return new Enumerator(this.dictionary);
            }

            public void CopyTo(TValue[] array, int index) {
                if (array == null) {
                    throw new ArgumentNullException();
                }

                if (index < 0 || index > array.Length) {
                    throw new ArgumentOutOfRangeException();
                }

                if (array.Length - index < this.dictionary.Count) {
                    throw new ArgumentException();
                }

                var count = this.dictionary._count;
                var entries = this.dictionary._entries;
                for (var i = 0; i < count; i++) {
                    if (entries[i].hashCode >= 0) {
                        array[index++] = entries[i].value;
                    }
                }
            }

            public int Count => this.dictionary.Count;

            bool ICollection<TValue>.IsReadOnly => true;

            void ICollection<TValue>.Add(TValue item) {
                throw new NotSupportedException();
            }

            bool ICollection<TValue>.Remove(TValue item) {
                throw new NotSupportedException();
            }

            void ICollection<TValue>.Clear() {
                throw new NotSupportedException();
            }

            bool ICollection<TValue>.Contains(TValue item) {
                return this.dictionary.ContainsValue(item);
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() {
                return new Enumerator(this.dictionary);
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return new Enumerator(this.dictionary);
            }

            void ICollection.CopyTo(Array array, int index) {
                if (array == null) {
                    throw new ArgumentNullException();
                }

                if (array.Rank != 1) {
                    throw new ArgumentException();
                }

                if (array.GetLowerBound(0) != 0) {
                    throw new ArgumentException();
                }

                if (index < 0 || index > array.Length) {
                    throw new ArgumentOutOfRangeException();
                }

                if (array.Length - index < this.dictionary.Count) {
                    throw new ArgumentException();
                }

                var values = array as TValue[];
                if (values != null) {
                    this.CopyTo(values, index);
                } else {
                    var objects = array as object[];
                    if (objects == null) {
                        throw new ArgumentException();
                    }

                    var count = this.dictionary._count;
                    var entries = this.dictionary._entries;
                    try {
                        for (var i = 0; i < count; i++) {
                            if (entries[i].hashCode >= 0) {
                                objects[index++] = entries[i].value;
                            }
                        }
                    } catch (ArrayTypeMismatchException) {
                        throw new ArgumentException();
                    }
                }
            }

            bool ICollection.IsSynchronized => false;

            Object ICollection.SyncRoot => ((ICollection)this.dictionary).SyncRoot;

            [Serializable]
            public struct Enumerator : IEnumerator<TValue>, System.Collections.IEnumerator {

                private DictionaryCopyable<TKey, TValue> dictionary;
                private int index;
                private int version;
                private TValue currentValue;

                internal Enumerator(DictionaryCopyable<TKey, TValue> dictionary) {
                    this.dictionary = dictionary;
                    this.version = dictionary._version;
                    this.index = 0;
                    this.currentValue = default(TValue);
                }

                public void Dispose() { }

                public bool MoveNext() {
                    if (this.version != this.dictionary._version) {
                        throw new InvalidOperationException();
                    }

                    while ((uint)this.index < (uint)this.dictionary._count) {
                        if (this.dictionary._entries[this.index].hashCode >= 0) {
                            this.currentValue = this.dictionary._entries[this.index].value;
                            this.index++;
                            return true;
                        }

                        this.index++;
                    }

                    this.index = this.dictionary._count + 1;
                    this.currentValue = default(TValue);
                    return false;
                }

                public TValue Current => this.currentValue;

                Object System.Collections.IEnumerator.Current {
                    get {
                        if (this.index == 0 || this.index == this.dictionary._count + 1) {
                            throw new InvalidOperationException();
                        }

                        return this.currentValue;
                    }
                }

                void System.Collections.IEnumerator.Reset() {
                    if (this.version != this.dictionary._version) {
                        throw new InvalidOperationException();
                    }

                    this.index = 0;
                    this.currentValue = default(TValue);
                }

            }

        }

    }

}