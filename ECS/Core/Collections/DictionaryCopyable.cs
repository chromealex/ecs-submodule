namespace ME.ECS.Collections {

    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Collections.Generic;

    public enum InsertionBehavior {

        None,
        ThrowOnExisting,
        OverwriteExisting,

    }

    /// <summary><para>Represents a collection of key/value pairs that are organized based on the key.</para></summary>
    /// <typeparam name="TKey">To be added.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class DictionaryCopyable<TKey, TValue> {

        [ME.ECS.Serializer.SerializeField]
        private int[] _buckets;
        [ME.ECS.Serializer.SerializeField]
        private DictionaryCopyable<TKey, TValue>.Entry[] _entries;
        [ME.ECS.Serializer.SerializeField]
        private int _count;
        [ME.ECS.Serializer.SerializeField]
        private int _freeList;
        [ME.ECS.Serializer.SerializeField]
        private int _freeCount;
        [ME.ECS.Serializer.SerializeField]
        private int _version;
        [ME.ECS.Serializer.SerializeField]
        private IEqualityComparer<TKey> _comparer;
        [ME.ECS.Serializer.SerializeField]
        private DictionaryCopyable<TKey, TValue>.KeyCollection _keys;
        [ME.ECS.Serializer.SerializeField]
        private DictionaryCopyable<TKey, TValue>.ValueCollection _values;
        private object _syncRoot;
        private const string VersionName = "Version";
        private const string HashSizeName = "HashSize";
        private const string KeyValuePairsName = "KeyValuePairs";
        private const string ComparerName = "Comparer";

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        public DictionaryCopyable() : this(0, (IEqualityComparer<TKey>)null) { }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        public DictionaryCopyable(int capacity) : this(capacity, (IEqualityComparer<TKey>)null) { }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        public DictionaryCopyable(IEqualityComparer<TKey> comparer) : this(0, comparer) { }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        public DictionaryCopyable(int capacity, IEqualityComparer<TKey> comparer) {
            if (capacity > 0) {
                this.Initialize(capacity);
            }

            if (comparer == EqualityComparer<TKey>.Default) {
                return;
            }

            this._comparer = comparer;
        }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        public DictionaryCopyable(IDictionary<TKey, TValue> dictionary) : this(dictionary, (IEqualityComparer<TKey>)null) { }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        public DictionaryCopyable(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : this(dictionary != null ? dictionary.Count : 0, comparer) {
            if (dictionary.GetType() == typeof(Dictionary<TKey, TValue>)) {
                var dictionary1 = (DictionaryCopyable<TKey, TValue>)dictionary;
                var count = dictionary1._count;
                var entries = dictionary1._entries;
                for (var index = 0; index < count; ++index) {
                    if (entries[index].hashCode >= 0) {
                        this.Add(entries[index].key, entries[index].value);
                    }
                }
            } else {
                foreach (var keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>)dictionary) {
                    this.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        public DictionaryCopyable(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, (IEqualityComparer<TKey>)null) { }

        /// <summary><para>Initializes a new dictionary that is empty, has the default initial capacity, and uses the default equality comparer.</para></summary>
        public DictionaryCopyable(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : this(
            collection is ICollection<KeyValuePair<TKey, TValue>> keyValuePairs ? keyValuePairs.Count : 0, comparer) {
            foreach (var keyValuePair in collection) {
                this.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public void CopyFrom<TElementCopy>(DictionaryCopyable<TKey, TValue> other, TElementCopy copy) where TElementCopy : IArrayElementCopy<TValue> {

            this.Clear(copy);
            foreach (var item in other) {
                
                this.TryInsert(item.Key, item.Value, InsertionBehavior.ThrowOnExisting, copy);
                
            }

        }

        /// <summary>To be added.</summary>
        public IEqualityComparer<TKey> Comparer => this._comparer != null ? this._comparer : (IEqualityComparer<TKey>)EqualityComparer<TKey>.Default;

        /// <summary><para>Gets the number of key/value pairs contained in the dictionary.</para></summary>
        public int Count => this._count - this._freeCount;

        /// <summary><para>Gets a collection that contains the keys in the dictionary.</para></summary>
        public DictionaryCopyable<TKey, TValue>.KeyCollection Keys {
            get {
                if (this._keys == null) {
                    this._keys = new DictionaryCopyable<TKey, TValue>.KeyCollection(this);
                }

                return this._keys;
            }
        }

        /// <summary><para>Gets a collection that contains the values in the dictionary.</para></summary>
        public DictionaryCopyable<TKey, TValue>.ValueCollection Values {
            get {
                if (this._values == null) {
                    this._values = new DictionaryCopyable<TKey, TValue>.ValueCollection(this);
                }

                return this._values;
            }
        }

        /// <summary><para>Gets or sets the value associated with the specified key.</para></summary>
        /// <param name="key">The key whose value is to be gotten or set.</param>
        private TValue defaultValue;

        public ref TValue this[TKey key] {
            get {
                var entry = this.FindEntry(key);
                if (entry >= 0) {
                    return ref this._entries[entry].value;
                }

                return ref this.defaultValue;
            }
            //set => this.TryInsert(key, value, InsertionBehavior.OverwriteExisting);
        }

        /// <summary><para>Adds an element with the specified key and value to the dictionary.</para></summary>
        /// <param name="key">The key of the element to add to the dictionary.</param>
        public void Add(TKey key, TValue value) {
            this.TryInsert(key, value, InsertionBehavior.ThrowOnExisting);
        }

        public void Clear<TElementCopy>(TElementCopy copy) where TElementCopy : IArrayElementCopy<TValue> {

            foreach (var item in this) {
                copy.Recycle(item.Value);
            }
            this.Clear();
            
        }

        /// <summary><para>Removes all elements from the dictionary.</para></summary>
        public void Clear() {
            var count = this._count;
            if (count > 0) {
                Array.Clear((Array)this._buckets, 0, this._buckets.Length);
                this._count = 0;
                this._freeList = -1;
                this._freeCount = 0;
                Array.Clear((Array)this._entries, 0, count);
            }

            ++this._version;
        }

        /// <summary><para>Determines whether the dictionary contains an element with a specific key.</para></summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        public bool ContainsKey(TKey key) {
            return this.FindEntry(key) >= 0;
        }

        /// <summary><para>Determines whether the dictionary contains an element with a specific value.</para></summary>
        /// <param name="value">The value to locate in the dictionary.</param>
        public bool ContainsValue(TValue value) {
            var entries = this._entries;
            if ((object)value == null) {
                for (var index = 0; index < this._count; ++index) {
                    if (entries[index].hashCode >= 0 && (object)entries[index].value == null) {
                        return true;
                    }
                }
            } else if ((object)default(TValue) != null) {
                for (var index = 0; index < this._count; ++index) {
                    if (entries[index].hashCode >= 0 && EqualityComparer<TValue>.Default.Equals(entries[index].value, value)) {
                        return true;
                    }
                }
            } else {
                var equalityComparer = EqualityComparer<TValue>.Default;
                for (var index = 0; index < this._count; ++index) {
                    if (entries[index].hashCode >= 0 && equalityComparer.Equals(entries[index].value, value)) {
                        return true;
                    }
                }
            }

            return false;
        }

        private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) {
            var count = this._count;
            var entries = this._entries;
            for (var index1 = 0; index1 < count; ++index1) {
                if (entries[index1].hashCode >= 0) {
                    array[index++] = new KeyValuePair<TKey, TValue>(entries[index1].key, entries[index1].value);
                }
            }
        }

        /// <summary><para>Returns an enumerator that can be used to iterate over the dictionary.</para></summary>
        public DictionaryCopyable<TKey, TValue>.Enumerator GetEnumerator() {
            return new DictionaryCopyable<TKey, TValue>.Enumerator(this, 2);
        }

        private int FindEntry(TKey key) {
            var index = -1;
            var buckets = this._buckets;
            var entries = this._entries;
            var num1 = 0;
            if (buckets != null) {
                var comparer = this._comparer;
                if (comparer == null) {
                    var num2 = key.GetHashCode() & int.MaxValue;
                    index = buckets[num2 % buckets.Length] - 1;
                    if ((object)default(TKey) != null) {
                        while ((uint)index < (uint)entries.Length && (entries[index].hashCode != num2 || !EqualityComparer<TKey>.Default.Equals(entries[index].key, key))) {
                            index = entries[index].next;
                            if (num1 >= entries.Length) {
                                throw new NotSupportedException();
                            }

                            ++num1;
                        }
                    } else {
                        var equalityComparer = EqualityComparer<TKey>.Default;
                        while ((uint)index < (uint)entries.Length && (entries[index].hashCode != num2 || !equalityComparer.Equals(entries[index].key, key))) {
                            index = entries[index].next;
                            if (num1 >= entries.Length) {
                                throw new NotSupportedException();
                            }

                            ++num1;
                        }
                    }
                } else {
                    var num2 = comparer.GetHashCode(key) & int.MaxValue;
                    index = buckets[num2 % buckets.Length] - 1;
                    while ((uint)index < (uint)entries.Length && (entries[index].hashCode != num2 || !comparer.Equals(entries[index].key, key))) {
                        index = entries[index].next;
                        if (num1 >= entries.Length) {
                            throw new NotSupportedException();
                        }

                        ++num1;
                    }
                }
            }

            return index;
        }

        private int Initialize(int capacity) {
            var prime = HashHelpers.GetPrime(capacity);
            this._freeList = -1;
            this._buckets = new int[prime];
            this._entries = new DictionaryCopyable<TKey, TValue>.Entry[prime];
            return prime;
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

        private void Resize() {
            this.Resize(HashHelpers.ExpandPrime(this._count), false);
        }

        private void Resize(int newSize, bool forceNewHashCodes) {
            var numArray = new int[newSize];
            var entryArray = new DictionaryCopyable<TKey, TValue>.Entry[newSize];
            var count = this._count;
            Array.Copy((Array)this._entries, 0, (Array)entryArray, 0, count);
            if (((object)default(TKey) == null) & forceNewHashCodes) {
                for (var index = 0; index < count; ++index) {
                    if (entryArray[index].hashCode >= 0) {
                        entryArray[index].hashCode = entryArray[index].key.GetHashCode() & int.MaxValue;
                    }
                }
            }

            for (var index1 = 0; index1 < count; ++index1) {
                if (entryArray[index1].hashCode >= 0) {
                    var index2 = entryArray[index1].hashCode % newSize;
                    entryArray[index1].next = numArray[index2] - 1;
                    numArray[index2] = index1 + 1;
                }
            }

            this._buckets = numArray;
            this._entries = entryArray;
        }

        /// <summary><para>Removes the element with the specified key from the dictionary.</para></summary>
        /// <param name="key">The key of the element to be removed from the dictionary.</param>
        public bool Remove(TKey key) {
 
            if (this._buckets != null) {
                int hashCode = this._comparer.GetHashCode(key) & 0x7FFFFFFF;
                int bucket = hashCode % this._buckets.Length;
                int last = -1;
                for (int i = this._buckets[bucket]; i >= 0; last = i, i = this._entries[i].next) {
                    if (this._entries[i].hashCode == hashCode && this._comparer.Equals(this._entries[i].key, key)) {
                        if (last < 0) {
                            this._buckets[bucket] = this._entries[i].next;
                        }
                        else {
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

        /// <summary>To be added.</summary>
        /// <param name="key">To be added.</param>
        public bool TryGetValue(TKey key, out TValue value) {
            var entry = this.FindEntry(key);
            if (entry >= 0) {
                value = this._entries[entry].value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public bool TryAdd(TKey key, TValue value) {
            return this.TryInsert(key, value, InsertionBehavior.None);
        }

        public int EnsureCapacity(int capacity) {
            var num = this._entries == null ? 0 : this._entries.Length;
            if (num >= capacity) {
                return num;
            }

            if (this._buckets == null) {
                return this.Initialize(capacity);
            }

            var prime = HashHelpers.GetPrime(capacity);
            this.Resize(prime, false);
            return prime;
        }

        private static bool IsCompatibleKey(object key) {
            return key is TKey;
        }

        private struct Entry {

            public int hashCode;
            public int next;
            public TKey key;
            public TValue value;

        }

        [Serializable]
        public struct Enumerator {

            private DictionaryCopyable<TKey, TValue> _dictionary;
            private int _version;
            private int _index;
            private KeyValuePair<TKey, TValue> _current;
            private int _getEnumeratorRetType;
            internal const int DictEntry = 1;
            internal const int KeyValuePair = 2;

            internal Enumerator(DictionaryCopyable<TKey, TValue> dictionary, int getEnumeratorRetType) {
                this._dictionary = dictionary;
                this._version = dictionary._version;
                this._index = 0;
                this._getEnumeratorRetType = getEnumeratorRetType;
                this._current = new KeyValuePair<TKey, TValue>();
            }

            public bool MoveNext() {
                while ((uint)this._index < (uint)this._dictionary._count) {
                    ref var local = ref this._dictionary._entries[this._index++];
                    if (local.hashCode >= 0) {
                        this._current = new KeyValuePair<TKey, TValue>(local.key, local.value);
                        return true;
                    }
                }

                this._index = this._dictionary._count + 1;
                this._current = new KeyValuePair<TKey, TValue>();
                return false;
            }

            public KeyValuePair<TKey, TValue> Current => this._current;

            public void Dispose() { }

        }

        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class KeyCollection : ICollection<TKey> {

            private DictionaryCopyable<TKey, TValue> _dictionary;

            public KeyCollection(DictionaryCopyable<TKey, TValue> dictionary) {
                this._dictionary = dictionary;
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() {
                return new DictionaryCopyable<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);
            }

            public DictionaryCopyable<TKey, TValue>.KeyCollection.Enumerator GetEnumerator() {
                return new DictionaryCopyable<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);
            }

            public void Add(TKey item) {
                throw new NotImplementedException();
            }

            public void Clear() {
                throw new NotImplementedException();
            }

            public bool Contains(TKey item) {
                throw new NotImplementedException();
            }

            public void CopyTo(TKey[] array, int arrayIndex) {
                throw new NotImplementedException();
            }

            public bool Remove(TKey item) {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return new DictionaryCopyable<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);
            }

            public int Count => this._dictionary.Count;
            public bool IsReadOnly { get; }

            [Serializable]
            public struct Enumerator : IDisposable, System.Collections.Generic.IEnumerator<TKey> {

                private DictionaryCopyable<TKey, TValue> _dictionary;
                private int _index;
                private int _version;
                private TKey _currentKey;

                internal Enumerator(DictionaryCopyable<TKey, TValue> dictionary) {
                    this._dictionary = dictionary;
                    this._version = dictionary._version;
                    this._index = 0;
                    this._currentKey = default(TKey);
                }

                public void Reset() { }

                object System.Collections.IEnumerator.Current => this.Current;

                public void Dispose() { }

                public bool MoveNext() {
                    while ((uint)this._index < (uint)this._dictionary._count) {
                        ref var local = ref this._dictionary._entries[this._index++];
                        if (local.hashCode >= 0) {
                            this._currentKey = local.key;
                            return true;
                        }
                    }

                    this._index = this._dictionary._count + 1;
                    this._currentKey = default(TKey);
                    return false;
                }

                public TKey Current => this._currentKey;

            }

        }

        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class ValueCollection {

            private DictionaryCopyable<TKey, TValue> _dictionary;

            public ValueCollection(DictionaryCopyable<TKey, TValue> dictionary) {
                this._dictionary = dictionary;
            }

            public DictionaryCopyable<TKey, TValue>.ValueCollection.Enumerator GetEnumerator() {
                return new DictionaryCopyable<TKey, TValue>.ValueCollection.Enumerator(this._dictionary);
            }

            public void CopyTo(TValue[] array, int index) {
                var count = this._dictionary._count;
                var entries = this._dictionary._entries;
                for (var index1 = 0; index1 < count; ++index1) {
                    if (entries[index1].hashCode >= 0) {
                        array[index++] = entries[index1].value;
                    }
                }
            }

            public int Count => this._dictionary.Count;

            [Serializable]
            public struct Enumerator {

                private DictionaryCopyable<TKey, TValue> _dictionary;
                private int _index;
                private int _version;
                private TValue _currentValue;

                internal Enumerator(DictionaryCopyable<TKey, TValue> dictionary) {
                    this._dictionary = dictionary;
                    this._version = dictionary._version;
                    this._index = 0;
                    this._currentValue = default(TValue);
                }

                public void Dispose() { }

                public bool MoveNext() {
                    while ((uint)this._index < (uint)this._dictionary._count) {
                        ref var local = ref this._dictionary._entries[this._index++];
                        if (local.hashCode >= 0) {
                            this._currentValue = local.value;
                            return true;
                        }
                    }

                    this._index = this._dictionary._count + 1;
                    this._currentValue = default(TValue);
                    return false;
                }

                public TValue Current => this._currentValue;

            }

        }

    }

}