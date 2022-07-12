#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS.Collections {

    public interface IIntrusiveHashSet {

        int Count { get; }

        void Add(in Entity entityData);
        bool Remove(in Entity entityData);
        int RemoveAll(in Entity entityData);
        void Clear(bool destroyData = false);
        bool Contains(in Entity entityData);

        BufferArray<Entity> ToArray();
        IntrusiveHashSet.Enumerator GetEnumerator();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct IntrusiveHashSet : IIntrusiveHashSet {

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public struct Enumerator : System.Collections.Generic.IEnumerator<Entity> {

            private IntrusiveHashSet hashSet;
            private int bucketIndex;
            private IntrusiveList.Enumerator listEnumerator;
            
            Entity System.Collections.Generic.IEnumerator<Entity>.Current => this.listEnumerator.Current;
            public ref readonly Entity Current => ref this.listEnumerator.Current;

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public Enumerator(IntrusiveHashSet hashSet) {

                this.hashSet = hashSet;
                this.bucketIndex = 0;
                this.listEnumerator = default;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {

                while (this.bucketIndex <= this.hashSet.buckets.Length) {

                    if (this.listEnumerator.MoveNext() == true) {

                        return true;

                    }

                    var bucket = this.hashSet.buckets[this.bucketIndex];
                    if (bucket.IsAlive() == true) {

                        var node = bucket.Read<IntrusiveHashSetBucket>();
                        this.listEnumerator = node.list.GetEnumerator();

                    }

                    ++this.bucketIndex;

                }

                return false;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Reset() {

                this.bucketIndex = 0;
                this.listEnumerator = default;

            }

            object System.Collections.IEnumerator.Current {
                get {
                    throw new AllocationException();
                }
            }

            public void Dispose() { }

        }

        [ME.ECS.Serializer.SerializeField]
        private Entity data;
        
        private StackArray10<Entity> buckets {
            get {
                if (this.data == Entity.Null) return default;
                return this.data.Read<IntrusiveHashSetData>().buckets;
            }
            set {
                this.ValidateData();
                this.data.Get<IntrusiveHashSetData>().buckets = value;
            }
        }
        
        private int count {
            get {
                if (this.data == Entity.Null) return 0;
                return this.data.Read<IntrusiveHashSetData>().count;   
            }
            set {
                this.ValidateData();
                this.data.Get<IntrusiveHashSetData>().count = value;
            }
        }

        public int Count => this.count;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateData() {
            
            if (this.data == Entity.Null) {
                this.data = new Entity(EntityFlag.None);
                this.data.ValidateDataUnmanaged<IntrusiveHashSetData>();
                this.data.Set(new IntrusiveHashSetData());
            }
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Enumerator GetEnumerator() {

            return new Enumerator(this);

        }

        /// <summary>
        /// Put entity data into array.
        /// </summary>
        /// <returns>Buffer array from pool</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public BufferArray<Entity> ToArray() {

            var arr = PoolArray<Entity>.Spawn(this.count);
            var i = 0;
            foreach (var entity in this) {

                arr.arr[i++] = entity;

            }

            return arr;

        }

        /// <summary>
        /// Find an element.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Contains(in Entity entityData) {

            var bucket = (entityData.GetHashCode() & 0x7fffffff) % this.buckets.Length;
            var bucketEntity = this.buckets[bucket];
            if (bucketEntity.IsAlive() == false) return false;

            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucket>();
            return bucketList.list.Contains(entityData);

        }

        /// <summary>
        /// Clear the list.
        /// </summary>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clear(bool destroyData = false) {

            for (int i = 0; i < this.buckets.Length; ++i) {

                var bucket = this.buckets[i];
                if (bucket.IsAlive() == true) {

                    ref var data = ref bucket.Get<IntrusiveHashSetBucket>();
                    data.list.Clear();

                }

            }

            this.count = 0;

        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        public void Dispose() {

            this.Clear();
            var buckets = this.buckets;
            for (int i = 0; i < buckets.Length; ++i) {
                
                if (buckets[i].IsAlive() == true) buckets[i].Destroy();
                buckets[i] = default;

            }

            this.buckets = buckets;

        }

        /// <summary>
        /// Remove data from list.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Remove(in Entity entityData) {

            var bucket = (entityData.GetHashCode() & 0x7fffffff) % this.buckets.Length;
            var bucketEntity = this.buckets[bucket];
            if (bucketEntity.IsAlive() == false) return false;

            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucket>();
            if (bucketList.list.Remove(entityData) == true) {

                --this.count;
                return true;

            }

            return false;

        }

        /// <summary>
        /// Remove all nodes data from list.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int RemoveAll(in Entity entityData) {

            var bucket = (entityData.GetHashCode() & 0x7fffffff) % this.buckets.Length;
            var bucketEntity = this.buckets[bucket];
            if (bucketEntity.IsAlive() == false) return 0;

            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucket>();
            var count = bucketList.list.RemoveAll(in entityData);
            this.count -= count;
            return count;

        }

        /// <summary>
        /// Add new data at the end of the list.
        /// </summary>
        /// <param name="entityData"></param>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add(in Entity entityData) {

            IntrusiveHashSet.Initialize(ref this);

            var buckets = this.buckets;
            var bucket = (entityData.GetHashCode() & 0x7fffffff) % buckets.Length;
            var bucketEntity = buckets[bucket];
            if (bucketEntity.IsAlive() == false) bucketEntity = buckets[bucket] = new Entity(EntityFlag.None);
            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucket>();
            bucketList.list.Add(entityData);
            ++this.count;
            this.buckets = buckets;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add(in Entity entityData, out Entity node) {

            IntrusiveHashSet.Initialize(ref this);

            var buckets = this.buckets;
            var bucket = (entityData.GetHashCode() & 0x7fffffff) % buckets.Length;
            var bucketEntity = this.buckets[bucket];
            if (bucketEntity.IsAlive() == false) bucketEntity = buckets[bucket] = new Entity(EntityFlag.None);
            ref var bucketList = ref bucketEntity.Get<IntrusiveHashSetBucket>();
            bucketList.list.Add(entityData, out node);
            ++this.count;
            this.buckets = buckets;

        }

        #region Helpers
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static void Initialize(ref IntrusiveHashSet hashSet) {

            if (hashSet.buckets.Length == 0) hashSet.buckets = new StackArray10<Entity>(10);

        }
        #endregion

    }

}