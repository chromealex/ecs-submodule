namespace ME.ECS.Collections {
 
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using TKey = System.Int32;

    public interface IDictionaryInt : ICollection, IEnumerable {

        void Add(object key, object value);

        bool ContainsKey(TKey key);

        int[] GetKeys();

    }
 
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    public class DictionaryInt<TValue> : IDictionaryInt {
    
        private struct Entry {
            public int hashCode;    // Lower 31 bits of hash code, -1 if unused
            public int next;        // Index of next entry, -1 if last
            public TKey key;           // Key of entry
            public TValue value;         // Value of entry
        }
 
        [ME.ECS.Serializer.SerializeField]
        private BufferArray<int> buckets;
        [ME.ECS.Serializer.SerializeField]
        private BufferArray<Entry> entries;
        [ME.ECS.Serializer.SerializeField]
        private int count;
        [ME.ECS.Serializer.SerializeField]
        private int version;
        [ME.ECS.Serializer.SerializeField]
        private int freeList;
        [ME.ECS.Serializer.SerializeField]
        private int freeCount;
        private KeyCollection keys;
        private ValueCollection values;
        private Object _syncRoot;
        
        // constants for serialization
        private const String VersionName = "Version";
        private const String HashSizeName = "HashSize";  // Must save buckets.Length
        private const String KeyValuePairsName = "KeyValuePairs";
        private const String ComparerName = "Comparer";

        int[] IDictionaryInt.GetKeys() {

            var result = new int[this.Count];
            this.Keys.CopyTo(result, 0);

            return result;

        }

        /*public void CopyFrom(DictionaryInt<TValue> other) {

            this.Clear();
            foreach (var item in other) {

                this.Add(item.Key, item.Value);

            }

            this.version = other.version;

        }
        
        private struct EntryCopy<T> : IArrayElementCopy<Entry> where T : IArrayElementCopy<TValue> {

            public T copy;

            public void Copy(Entry @from, ref Entry to) {
                
                this.copy.Copy(from.value, ref to.value);
                to.key = from.key;
                to.next = from.next;
                to.hashCode = from.hashCode;

            }

            public void Recycle(Entry item) {
                
                this.copy.Recycle(item.value);
                
            }

        }

        public void CopyFrom<TCopy>(DictionaryInt<TValue> other, TCopy copy) where TCopy : IArrayElementCopy<TValue> {

            this.Clear(copy);
            foreach (var item in other) {

                this.Insert(item.Key, item.Value, true, copy);

            }

            this.version = other.version;

        }*/

        
        public void CopyFrom(DictionaryInt<TValue> other) {

            ArrayUtils.Copy(other.buckets, ref this.buckets);
            ArrayUtils.Copy(other.entries, ref this.entries);
            this.count = other.count;
            this.version = other.version;
            this.freeList = other.freeList;
            this.freeCount = other.freeCount;
            
        }

        private struct EntryCopy<T> : IArrayElementCopy<Entry> where T : IArrayElementCopy<TValue> {

            public T copy;

            public void Copy(Entry @from, ref Entry to) {
                
                this.copy.Copy(from.value, ref to.value);
                to.key = from.key;
                to.next = from.next;
                to.hashCode = from.hashCode;
                
            }

            public void Recycle(Entry item) {
                
                this.copy.Recycle(item.value);
                
            }

        }

        public void CopyFrom<TCopy>(DictionaryInt<TValue> other, TCopy copy) where TCopy : IArrayElementCopy<TValue> {

            ArrayUtils.Copy(other.buckets, ref this.buckets);
            ArrayUtils.Copy(other.entries, ref this.entries, new EntryCopy<TCopy>() { copy = copy });
            this.count = other.count;
            this.version = other.version;
            this.freeList = other.freeList;
            this.freeCount = other.freeCount;
            
        }
        

        public DictionaryInt(): this(0, null) {}
 
        public DictionaryInt(int capacity): this(capacity, null) {}
 
        public DictionaryInt(IEqualityComparer<TKey> comparer): this(0, comparer) {}
 
        public DictionaryInt(int capacity, IEqualityComparer<TKey> comparer) {
            if (capacity < 0) ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
            if (capacity > 0) this.Initialize(capacity);
            
#if FEATURE_CORECLR
            if (HashHelpers.s_UseRandomizedStringHashing && comparer == EqualityComparer<string>.Default)
            {
                this.comparer = (IEqualityComparer<TKey>) NonRandomizedStringEqualityComparer.Default;
            }
#endif // FEATURE_CORECLR
        }
 
        protected DictionaryInt(SerializationInfo info, StreamingContext context) {
            //We can't do anything with the keys and values until the entire graph has been deserialized
            //and we have a resonable estimate that GetHashCode is not going to fail.  For the time being,
            //we'll just cache this.  The graph is not valid until OnDeserialization has been called.
            //HashHelpers.SerializationInfoTable.Add(this, info);
        }
        
        public DictionaryInt(DictionaryInt<TValue> dictionary): this(dictionary, null) {}
 
        public DictionaryInt(DictionaryInt<TValue> dictionary, IEqualityComparer<TKey> comparer):
            this(dictionary != null? dictionary.Count: 0, comparer) {
 
            foreach (KeyValuePair<TKey,TValue> pair in dictionary) {
                Add(pair.Key, pair.Value);
            }
        }

        public int Count {
            get { return this.count - this.freeCount; }
        }
 
        public KeyCollection Keys {
            get {
                Contract.Ensures(Contract.Result<KeyCollection>() != null);
                if (this.keys == null) this.keys = new KeyCollection(this);
                return this.keys;
            }
        }
 
        public ValueCollection Values {
            get {
                Contract.Ensures(Contract.Result<ValueCollection>() != null);
                if (this.values == null) this.values = new ValueCollection(this);
                return this.values;
            }
        }
 
        public TValue this[TKey key] {
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                int i = this.FindEntry(key);
                if (i >= 0) return this.entries.arr[i].value;
                ThrowHelper.ThrowKeyNotFoundException();
                return default(TValue);
            }
            #if INLINE_METHODS
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            #endif
            set {
                this.Insert(key, value, false);
            }
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TValue GetValue(TKey key) {
            int i = this.FindEntry(key);
            if (i >= 0) {
                return ref this.entries.arr[i].value;
            }
            
            return ref this.Insert(key, default, false);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public ref TValue GetValue(TKey key, out bool exist) {
            int i = this.FindEntry(key);
            if (i >= 0) {
                exist = true;
                return ref this.entries.arr[i].value;
            }

            exist = false;
            return ref this.Insert(key, default, false);
            
        }

        #if INLINE_METHODS
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        #endif
        public TValue GetValueAndRemove(TKey key) {
            
            var val = this.GetValue(key, out var result);
            if (result == false) throw new Exception($"GetValueAndRemove: key doesn't exist: {key}");
            this.Remove(key);
            return val;

        }

        public void Add(object key, object value) {
            this.Add((TKey)key, (TValue)value);
        }

        public void Add(TKey key, TValue value) {
            this.Insert(key, value, true);
        }
 
        public void Clear<TElementCopy>(TElementCopy copy) where TElementCopy : IArrayElementCopy<TValue> {

            foreach (var item in this) {
                copy.Recycle(item.Value);
            }

            this.Clear();

        }

        public void Clear() {
            if (this.count > 0) {
                for (int i = 0; i < this.buckets.Length; i++) this.buckets.arr[i] = -1;
                ArrayUtils.Clear(this.entries);
                this.freeList = -1;
                this.count = 0;
                this.freeCount = 0;
                this.version++;
            }
        }
 
        public bool ContainsKey(TKey key) {
            return this.FindEntry(key) >= 0;
        }
 
        public bool ContainsValue(TValue value) {
            if (value == null) {
                for (int i = 0; i < this.count; i++) {
                    if (this.entries.arr[i].hashCode >= 0 && this.entries.arr[i].value == null) return true;
                }
            }
            else {
                EqualityComparer<TValue> c = EqualityComparer<TValue>.Default;
                for (int i = 0; i < this.count; i++) {
                    if (this.entries.arr[i].hashCode >= 0 && c.Equals(this.entries.arr[i].value, value)) return true;
                }
            }
            return false;
        }
 
        private void CopyTo(KeyValuePair<TKey,TValue>[] array, int index) {
            if (array == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }
            
            if (index < 0 || index > array.Length ) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }
 
            if (array.Length - index < this.Count) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
            }
 
            int count = this.count;
            var entries = this.entries;
            for (int i = 0; i < count; i++) {
                if (entries[i].hashCode >= 0) {
                    array[index++] = new KeyValuePair<TKey,TValue>(entries[i].key, entries[i].value);
                }
            }
        }
 
        public Enumerator GetEnumerator() {
            return new Enumerator(this, Enumerator.DictEntry);
        }
 
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int FindEntry(TKey key) {
            if (this.buckets.isCreated == true) {
                int hashCode = key & 0x7FFFFFFF;
                for (int i = this.buckets.arr[hashCode % this.buckets.Length]; i >= 0; i = this.entries.arr[i].next) {
                    var item = this.entries.arr[i];
                    if (item.key == key) return i;
                }
            }
            return -1;
        }
 
        private void Initialize(int capacity) {
            int size = HashHelpers.GetPrime(capacity);
            this.buckets = PoolArray<int>.Spawn(size);
            for (int i = 0; i < this.buckets.Length; i++) this.buckets.arr[i] = -1;
            this.entries = PoolArray<Entry>.Spawn(size);
            this.freeList = -1;
        }
 
        private void Insert<TElementCopy>(TKey key, TValue value, bool add, TElementCopy copy) where TElementCopy : IArrayElementCopy<TValue> {

            if (this.buckets.isCreated == false) {
                this.Initialize(0);
            }

            var hashCode = key & 0x7FFFFFFF;
            var targetBucket = hashCode % this.buckets.Length;

            for (var i = this.buckets.arr[targetBucket]; i >= 0; i = this.entries.arr[i].next) {
                if (this.entries.arr[i].key == key) {
                    if (add) {
                        throw new ArgumentException();
                    }

                    copy.Copy(value, ref this.entries.arr[i].value);
                    this.version++;
                    return;
                }

                #if FEATURE_RANDOMIZED_STRING_HASHING
                collisionCount++;
                #endif
            }

            int index;
            if (this.freeCount > 0) {
                index = this.freeList;
                this.freeList = this.entries.arr[index].next;
                this.freeCount--;
            } else {
                if (this.count == this.entries.Length) {
                    this.Resize();
                    targetBucket = hashCode % this.buckets.Length;
                }

                index = this.count;
                this.count++;
            }

            ref var item = ref this.entries.arr[index];
            item.hashCode = hashCode;
            item.next = this.buckets.arr[targetBucket];
            item.key = key;
            copy.Copy(value, ref item.value);
            this.buckets.arr[targetBucket] = index;
            this.version++;

        }

        private ref TValue Insert(TKey key, TValue value, bool add) {
        
            if (this.buckets.isCreated == false) this.Initialize(0);
            int hashCode = key & 0x7FFFFFFF;
            int targetBucket = hashCode % this.buckets.Length;
 
#if FEATURE_RANDOMIZED_STRING_HASHING
            int collisionCount = 0;
#endif
 
            for (int i = this.buckets.arr[targetBucket]; i >= 0; i = this.entries.arr[i].next) {
                if (this.entries.arr[i].key == key) {
                    if (add) { 
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
                    }

                    this.entries.arr[i].value = value;
                    this.version++;
                    return ref this.entries.arr[i].value;
                } 
 
#if FEATURE_RANDOMIZED_STRING_HASHING
                collisionCount++;
#endif
            }
            int index;
            if (this.freeCount > 0) {
                index = this.freeList;
                this.freeList = this.entries.arr[index].next;
                this.freeCount--;
            }
            else {
                if (this.count == this.entries.Length)
                {
                    this.Resize();
                    targetBucket = hashCode % this.buckets.Length;
                }
                index = this.count;
                this.count++;
            }

            ref var item = ref this.entries.arr[index];
            item.hashCode = hashCode;
            item.next = this.buckets.arr[targetBucket];
            item.key = key;
            item.value = value;
            this.buckets.arr[targetBucket] = index;
            this.version++;
 
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

            return ref this.entries.arr[index].value;

        }
 
        private void Resize() {
            this.Resize(HashHelpers.ExpandPrime(this.count), false);
        }
 
        private void Resize(int newSize, bool forceNewHashCodes) {
            Contract.Assert(newSize >= this.entries.Length);
            
            ArrayUtils.Resize(newSize - 1, ref this.buckets);
            ArrayUtils.Resize(newSize - 1, ref this.entries);
            
            for (int i = 0; i < this.buckets.Length; i++) this.buckets.arr[i] = -1;

            if(forceNewHashCodes) {
                for (int i = 0; i < this.count; i++) {
                    if(this.entries.arr[i].hashCode != -1) {
                        this.entries.arr[i].hashCode = (this.entries.arr[i].key & 0x7FFFFFFF);
                    }
                }
            }
            for (int i = 0; i < this.count; i++) {
                if (this.entries.arr[i].hashCode >= 0) {
                    int bucket = this.entries.arr[i].hashCode % newSize;
                    this.entries.arr[i].next = this.buckets.arr[bucket];
                    this.buckets.arr[bucket] = i;
                }
            }
        }
 
        public bool Remove(TKey key) {
            
            if (this.buckets.isCreated == true) {
                int hashCode = key & 0x7FFFFFFF;
                int bucket = hashCode % this.buckets.Length;
                int last = -1;
                for (int i = this.buckets.arr[bucket]; i >= 0; last = i, i = this.entries.arr[i].next) {
                    if (this.entries.arr[i].key == key) {
                        if (last < 0) {
                            this.buckets.arr[bucket] = this.entries.arr[i].next;
                        }
                        else {
                            this.entries.arr[last].next = this.entries.arr[i].next;
                        }

                        this.entries.arr[i].hashCode = -1;
                        this.entries.arr[i].next = this.freeList;
                        this.entries.arr[i].key = default(TKey);
                        this.entries.arr[i].value = default(TValue);
                        this.freeList = i;
                        this.freeCount++;
                        this.version++;
                        return true;
                    }
                }
            }
            return false;
        }
 
        public bool TryGetValue(TKey key, out TValue value) {
            int i = this.FindEntry(key);
            if (i >= 0) {
                value = this.entries.arr[i].value;
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
            int i = this.FindEntry(key);
            if (i >= 0) {
                return this.entries.arr[i].value;
            }
            return default(TValue);
        }
 
        void ICollection.CopyTo(Array array, int index) {
            if (array == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
            }
            
            if (array.Rank != 1) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }
 
            if( array.GetLowerBound(0) != 0 ) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
            }
            
            if (index < 0 || index > array.Length) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }
 
            if (array.Length - index < this.Count) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
            }
            
            KeyValuePair<TKey,TValue>[] pairs = array as KeyValuePair<TKey,TValue>[];
            if (pairs != null) {
                this.CopyTo(pairs, index);
            }
            else if( array is DictionaryEntry[]) {
                DictionaryEntry[] dictEntryArray = array as DictionaryEntry[];
                var entries = this.entries;
                for (int i = 0; i < this.count; i++) {
                    if (entries[i].hashCode >= 0) {
                        dictEntryArray[index++] = new DictionaryEntry(entries[i].key, entries[i].value);
                    }
                }                
            }
            else {
                object[] objects = array as object[];
                if (objects == null) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
 
                try {
                    int count = this.count;
                    var entries = this.entries;
                    for (int i = 0; i < count; i++) {
                        if (entries[i].hashCode >= 0) {
                            objects[index++] = new KeyValuePair<TKey,TValue>(entries[i].key, entries[i].value);
                        }
                    }
                }
                catch(ArrayTypeMismatchException) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                }
            }
        }
 
        IEnumerator IEnumerable.GetEnumerator() {
            return new Enumerator(this, Enumerator.DictEntry);
        }
    
        bool ICollection.IsSynchronized {
            get { return false; }
        }
 
        object ICollection.SyncRoot { 
            get { 
                if(this._syncRoot == null) {
                    System.Threading.Interlocked.CompareExchange<Object>(ref this._syncRoot, new Object(), null);    
                }
                return this._syncRoot; 
            }
        }
 
        /*
        bool IDictionary.IsFixedSize {
            get { return false; }
        }
 
        bool IDictionary.IsReadOnly {
            get { return false; }
        }
 
        ICollection IDictionary.Keys {
            get { return (ICollection)this.Keys; }
        }
    
        ICollection IDictionary.Values {
            get { return (ICollection)this.Values; }
        }
    
        object IDictionary.this[object key] {
            get { 
                if( DictionaryInt<TValue>.IsCompatibleKey(key)) {                
                    int i = this.FindEntry((TKey)key);
                    if (i >= 0) { 
                        return this.entries.arr[i].value;                
                    }
                }
                return null;
            }
            set {                 
                if (key == null)
                {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);                          
                }
                ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
 
                try {
                    TKey tempKey = (TKey)key;
                    try {
                        this[tempKey] = (TValue)value; 
                    }
                    catch (InvalidCastException) { 
                        ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));   
                    }
                }
                catch (InvalidCastException) { 
                    ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
                }
            }
        }
 
        void IDictionary.Add(object key, object value) {            
            if (key == null)
            {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);                          
            }
            ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
 
            try {
                TKey tempKey = (TKey)key;
 
                try {
                    this.Add(tempKey, (TValue)value);
                }
                catch (InvalidCastException) { 
                    ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));   
                }
            }
            catch (InvalidCastException) { 
                ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
            }
        }
    
        bool IDictionary.Contains(object key) {    
            if(DictionaryInt<TValue>.IsCompatibleKey(key)) {
                return this.ContainsKey((TKey)key);
            }
       
            return false;
        }
    
        IDictionaryEnumerator IDictionary.GetEnumerator() {
            return new Enumerator(this, Enumerator.DictEntry);
        }
    
        void IDictionary.Remove(object key) {            
            if(DictionaryInt<TValue>.IsCompatibleKey(key)) {
                this.Remove((TKey)key);
            }
        }*/

        private static bool IsCompatibleKey(object key) {
            if( key == null) {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);                          
                }
            return (key is TKey); 
        }
    
        [Serializable]
        public struct Enumerator: IEnumerator<KeyValuePair<TKey,TValue>>,
            IDictionaryEnumerator
        {
            private DictionaryInt<TValue> dictionary;
            private int version;
            private int index;
            private KeyValuePair<TKey,TValue> current;
            private int getEnumeratorRetType;  // What should Enumerator.Current return?
            
            internal const int DictEntry = 1;
            internal const int KeyValuePair = 2;
 
            internal Enumerator(DictionaryInt<TValue> dictionary, int getEnumeratorRetType) {
                this.dictionary = dictionary;
                this.version = dictionary.version;
                this.index = 0;
                this.getEnumeratorRetType = getEnumeratorRetType;
                this.current = new KeyValuePair<TKey, TValue>();
            }
 
            public bool MoveNext() {
                if (this.version != this.dictionary.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }
 
                // Use unsigned comparison since we set index to dictionary.count+1 when the enumeration ends.
                // dictionary.count+1 could be negative if dictionary.count is Int32.MaxValue
                while ((TKey)this.index < (TKey)this.dictionary.count) {
                    if (this.dictionary.entries[this.index].hashCode >= 0) {
                        this.current = new KeyValuePair<TKey, TValue>(this.dictionary.entries[this.index].key, this.dictionary.entries[this.index].value);
                        this.index++;
                        return true;
                    }

                    this.index++;
                }

                this.index = this.dictionary.count + 1;
                this.current = new KeyValuePair<TKey, TValue>();
                return false;
            }
 
            public KeyValuePair<TKey,TValue> Current {
                get { return this.current; }
            }
 
            public void Dispose() {
            }
 
            object IEnumerator.Current {
                get { 
                    if(this.index == 0 || (this.index == this.dictionary.count + 1)) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);                        
                    }      
 
                    if (this.getEnumeratorRetType == DictionaryInt<TValue>.Enumerator.DictEntry) {
                        return new System.Collections.DictionaryEntry(this.current.Key, this.current.Value);
                    } else {
                        return new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
                    }
                }
            }
 
            void IEnumerator.Reset() {
                if (this.version != this.dictionary.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = 0;
                this.current = new KeyValuePair<TKey, TValue>();    
            }
 
            DictionaryEntry IDictionaryEnumerator.Entry {
                get { 
                    if(this.index == 0 || (this.index == this.dictionary.count + 1)) {
                         ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);                        
                    }                        
                    
                    return new DictionaryEntry(this.current.Key, this.current.Value); 
                }
            }
 
            object IDictionaryEnumerator.Key {
                get { 
                    if(this.index == 0 || (this.index == this.dictionary.count + 1)) {
                         ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);                        
                    }                        
                    
                    return this.current.Key; 
                }
            }
 
            object IDictionaryEnumerator.Value {
                get { 
                    if(this.index == 0 || (this.index == this.dictionary.count + 1)) {
                         ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);                        
                    }                        
                    
                    return this.current.Value; 
                }
            }
        }
 
        [DebuggerDisplay("Count = {Count}")]        
        [Serializable]
        public sealed class KeyCollection: ICollection<TKey>, ICollection, IReadOnlyCollection<TKey>
        {
            private DictionaryInt<TValue> dictionary;
 
            public KeyCollection(DictionaryInt<TValue> dictionary) {
                if (dictionary == null) {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
                }
                this.dictionary = dictionary;
            }
 
            public Enumerator GetEnumerator() {
                return new Enumerator(this.dictionary);
            }
 
            public void CopyTo(TKey[] array, int index) {
                if (array == null) {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                }
 
                if (index < 0 || index > array.Length) {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }
 
                if (array.Length - index < this.dictionary.Count) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }
                
                int count = this.dictionary.count;
                var entries = this.dictionary.entries;
                for (int i = 0; i < count; i++) {
                    if (entries[i].hashCode >= 0) array[index++] = entries[i].key;
                }
            }
 
            public int Count {
                get { return this.dictionary.Count; }
            }
 
            bool ICollection<TKey>.IsReadOnly {
                get { return true; }
            }
 
            void ICollection<TKey>.Add(TKey item){
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
            }
            
            void ICollection<TKey>.Clear(){
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
            }
 
            bool ICollection<TKey>.Contains(TKey item){
                return this.dictionary.ContainsKey(item);
            }
 
            bool ICollection<TKey>.Remove(TKey item){
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
                return false;
            }
            
            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() {
                return new Enumerator(this.dictionary);
            }
 
            IEnumerator IEnumerable.GetEnumerator() {
                return new Enumerator(this.dictionary);                
            }
 
            void ICollection.CopyTo(Array array, int index) {
                if (array==null) {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                }
 
                if (array.Rank != 1) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
                }
 
                if( array.GetLowerBound(0) != 0 ) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
                }
 
                if (index < 0 || index > array.Length) {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }
 
                if (array.Length - index < this.dictionary.Count) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }
                
                TKey[] keys = array as TKey[];
                if (keys != null) {
                    this.CopyTo(keys, index);
                }
                else {
                    object[] objects = array as object[];
                    if (objects == null) {
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    }
                                         
                    int count = this.dictionary.count;
                    var entries = this.dictionary.entries;
                    try {
                        for (int i = 0; i < count; i++) {
                            if (entries[i].hashCode >= 0) objects[index++] = entries[i].key;
                        }
                    }                    
                    catch(ArrayTypeMismatchException) {
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    }
                }
            }
 
            bool ICollection.IsSynchronized {
                get { return false; }
            }
 
            Object ICollection.SyncRoot { 
                get { return ((ICollection)this.dictionary).SyncRoot; }
            }
 
            [Serializable]
            public struct Enumerator : IEnumerator<TKey>, System.Collections.IEnumerator
            {
                private DictionaryInt<TValue> dictionary;
                private int index;
                private int version;
                private TKey currentKey;
            
                internal Enumerator(DictionaryInt<TValue> dictionary) {
                    this.dictionary = dictionary;
                    this.version = dictionary.version;
                    this.index = 0;
                    this.currentKey = default(TKey);                    
                }
 
                public void Dispose() {
                }
 
                public bool MoveNext() {
                    if (this.version != this.dictionary.version) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                    }
 
                    while ((TKey)this.index < (TKey)this.dictionary.count) {
                        if (this.dictionary.entries[this.index].hashCode >= 0) {
                            this.currentKey = this.dictionary.entries[this.index].key;
                            this.index++;
                            return true;
                        }

                        this.index++;
                    }

                    this.index = this.dictionary.count + 1;
                    this.currentKey = default(TKey);
                    return false;
                }
                
                public TKey Current {
                    get {                        
                        return this.currentKey;
                    }
                }
 
                Object System.Collections.IEnumerator.Current {
                    get {                      
                        if(this.index == 0 || (this.index == this.dictionary.count + 1)) {
                             ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);                        
                        }                        
                        
                        return this.currentKey;
                    }
                }
                
                void System.Collections.IEnumerator.Reset() {
                    if (this.version != this.dictionary.version) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);                        
                    }

                    this.index = 0;
                    this.currentKey = default(TKey);
                }
            }                        
        }
 
        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class ValueCollection: ICollection<TValue>, ICollection, IReadOnlyCollection<TValue>
        {
            private DictionaryInt<TValue> dictionary;
 
            public ValueCollection(DictionaryInt<TValue> dictionary) {
                if (dictionary == null) {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
                }
                this.dictionary = dictionary;
            }
 
            public Enumerator GetEnumerator() {
                return new Enumerator(this.dictionary);                
            }
 
            public void CopyTo(TValue[] array, int index) {
                if (array == null) {
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                }
 
                if (index < 0 || index > array.Length) {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }
 
                if (array.Length - index < this.dictionary.Count) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                }
                
                int count = this.dictionary.count;
                var entries = this.dictionary.entries;
                for (int i = 0; i < count; i++) {
                    if (entries[i].hashCode >= 0) array[index++] = entries[i].value;
                }
            }
 
            public int Count {
                get { return this.dictionary.Count; }
            }
 
            bool ICollection<TValue>.IsReadOnly {
                get { return true; }
            }
 
            void ICollection<TValue>.Add(TValue item){
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
            }
 
            bool ICollection<TValue>.Remove(TValue item){
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
                return false;
            }
 
            void ICollection<TValue>.Clear(){
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
            }
 
            bool ICollection<TValue>.Contains(TValue item){
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
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                }
 
                if (array.Rank != 1) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
                }
 
                if( array.GetLowerBound(0) != 0 ) {
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
                }
 
                if (index < 0 || index > array.Length) { 
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                }
 
                if (array.Length - index < this.dictionary.Count)
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                
                TValue[] values = array as TValue[];
                if (values != null) {
                    this.CopyTo(values, index);
                }
                else {
                    object[] objects = array as object[];
                    if (objects == null) {
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    }
 
                    int count = this.dictionary.count;
                    var entries = this.dictionary.entries;
                    try {
                        for (int i = 0; i < count; i++) {
                            if (entries[i].hashCode >= 0) objects[index++] = entries[i].value;
                        }
                    }
                    catch(ArrayTypeMismatchException) {
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    }
                }
            }
 
            bool ICollection.IsSynchronized {
                get { return false; }
            }
 
            Object ICollection.SyncRoot { 
                get { return ((ICollection)this.dictionary).SyncRoot; }
            }
 
            [Serializable]
            public struct Enumerator : IEnumerator<TValue>, System.Collections.IEnumerator
            {
                private DictionaryInt<TValue> dictionary;
                private int index;
                private int version;
                private TValue currentValue;
            
                internal Enumerator(DictionaryInt<TValue> dictionary) {
                    this.dictionary = dictionary;
                    this.version = dictionary.version;
                    this.index = 0;
                    this.currentValue = default(TValue);
                }
 
                public void Dispose() {
                }
 
                public bool MoveNext() {                    
                    if (this.version != this.dictionary.version) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                    }
                    
                    while ((TKey)this.index < (TKey)this.dictionary.count) {
                        if (this.dictionary.entries[this.index].hashCode >= 0) {
                            this.currentValue = this.dictionary.entries[this.index].value;
                            this.index++;
                            return true;
                        }

                        this.index++;
                    }

                    this.index = this.dictionary.count + 1;
                    this.currentValue = default(TValue);
                    return false;
                }
                
                public TValue Current {
                    get {                        
                        return this.currentValue;
                    }
                }
 
                Object System.Collections.IEnumerator.Current {
                    get {                      
                        if(this.index == 0 || (this.index == this.dictionary.count + 1)) {
                             ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);                        
                        }                        
                        
                        return this.currentValue;
                    }
                }
                
                void System.Collections.IEnumerator.Reset() {
                    if (this.version != this.dictionary.version) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                    }

                    this.index = 0;
                    this.currentValue = default(TValue);
                }
            }
        }
    }
    
}
