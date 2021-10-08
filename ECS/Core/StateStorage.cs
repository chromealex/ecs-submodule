#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using ME.ECS.Collections;

    public interface IStorage {

        int AliveCount { get; }
        int DeadCount { get; }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        bool IsAlive(int id, ushort generation);

        bool ForEach(ListCopyable<Entity> results);

        ref Entity Alloc();
        bool Dealloc(in Entity entity);

        void ApplyDead();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Storage : IStorage {

        public bool isCreated;
        [ME.ECS.Serializer.SerializeField]
        internal NativeBufferArray<Entity> cache;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> alive;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> dead;
        [ME.ECS.Serializer.SerializeField]
        internal ListCopyable<int> deadPrepared;
        [ME.ECS.Serializer.SerializeField]
        private int aliveCount;
        [ME.ECS.Serializer.SerializeField]
        private int entityId;
        [ME.ECS.Serializer.SerializeField]
        internal ArchetypeEntities archetypes;
        [ME.ECS.Serializer.SerializeField]
        internal EntityVersions versions;

        public int GetMaxId() => this.entityId;

        public override int GetHashCode() {

            if (this.dead == null) return 0;
            
            return this.versions.GetHashCode() ^ this.aliveCount ^ this.entityId ^ this.dead.Count;

        }

        public int AliveCount {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return this.aliveCount;
            }
        }

        public int DeadCount {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return this.dead.Count;
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ListCopyable<int> GetAlive() {

            return this.alive;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool ForEach(ListCopyable<Entity> results) {

            results.Clear();
            for (int i = 0; i < this.alive.Count; ++i) {

                var id = this.alive[i];
                results.Add(this.cache.arr[id]);

            }

            return true;

        }

        public void Initialize(int capacity) {

            this.cache = PoolArrayNative<Entity>.Spawn(capacity);
            this.alive = PoolListCopyable<int>.Spawn(capacity);
            this.dead = PoolListCopyable<int>.Spawn(capacity);
            this.deadPrepared = PoolListCopyable<int>.Spawn(capacity);
            this.isCreated = true;
            this.aliveCount = 0;
            this.entityId = -1;
            this.archetypes = new ArchetypeEntities();//PoolClass<ArchetypeEntities>.Spawn();
            this.versions = new EntityVersions();//PoolClass<EntityVersions>.Spawn();
            ME.WeakRef.Reg(this.alive);
            ME.WeakRef.Reg(this.dead);
            ME.WeakRef.Reg(this.deadPrepared);

        }

        public void Recycle() {

            PoolArrayNative<Entity>.Recycle(ref this.cache);
            PoolListCopyable<int>.Recycle(ref this.alive);
            PoolListCopyable<int>.Recycle(ref this.dead);
            PoolListCopyable<int>.Recycle(ref this.deadPrepared);
            this.isCreated = false;
            this.aliveCount = 0;
            this.entityId = -1;
            //PoolClass<ArchetypeEntities>.Recycle(ref this.archetypes);
            this.archetypes.Recycle();
            //PoolClass<EntityVersions>.Recycle(ref this.versions);
            this.versions.Recycle();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void SetFreeze(bool freeze) { }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(Storage other) {

            this.isCreated = other.isCreated;
            NativeArrayUtils.Copy(other.cache, ref this.cache);
            ArrayUtils.Copy(other.alive, ref this.alive);
            ArrayUtils.Copy(other.dead, ref this.dead);
            ArrayUtils.Copy(other.deadPrepared, ref this.deadPrepared);
            this.aliveCount = other.aliveCount;
            this.entityId = other.entityId;
            this.archetypes.CopyFrom(other.archetypes);
            this.versions.CopyFrom(other.versions);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool WillNew() {

            return this.dead.Count == 0;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Alloc(int count, ref EntitiesGroup group, Unity.Collections.Allocator allocator, bool copyMode) {

            var lastId = ++this.entityId + count;
            NativeArrayUtils.Resize(lastId, ref this.cache);

            this.aliveCount += count;

            var from = this.entityId;
            var id = this.entityId;
            var list = PoolArray<int>.Spawn(count);
            for (int i = 0; i < list.Length; ++i) {
                this.cache.arr[id] = new Entity(id, 1);
                list.arr[i] = id++;
            }
            this.alive.AddRange(list);
            PoolArray<int>.Recycle(ref list);
            this.versions.Reset(this.entityId);

            this.entityId += count;

            var slice = new Unity.Collections.NativeSlice<Entity>(this.cache.arr, from, count);
            var array = new Unity.Collections.NativeArray<Entity>(count, allocator, Unity.Collections.NativeArrayOptions.UninitializedMemory);
            slice.CopyTo(array);
            group = new EntitiesGroup(from, from + count - 1, array, copyMode);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity Alloc() {

            int id = -1;
            if (this.dead.Count > 0) {

                id = this.dead[0];
                this.dead.RemoveAtFast(0);

            } else {

                id = ++this.entityId;
                NativeArrayUtils.Resize(id, ref this.cache);

            }

            ++this.aliveCount;
            this.alive.Add(id);
            ref var e = ref this.cache[id];
            if (e.generation == 0) e = new Entity(id, 1);
            this.versions.Reset(id);
            e = ref this.IncrementGeneration(in e);
            return ref e;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Dealloc(in Entity entity) {

            if (this.IsAlive(entity.id, entity.generation) == false) return false;

            this.deadPrepared.Add(entity.id);

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity IncrementGeneration(in Entity entity) {

            // Make this entity not alive, but not completely destroyed at this time
            this.cache[entity.id] = new Entity(entity.id, unchecked((ushort)(entity.generation + 1)));
            return ref this.cache[entity.id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void ApplyDead() {

            var cnt = this.deadPrepared.Count;
            if (cnt > 0) {

                for (int i = 0; i < cnt; ++i) {

                    var id = this.deadPrepared[i];
                    --this.aliveCount;
                    this.dead.Add(id);
                    this.alive.Remove(id);

                }

                this.deadPrepared.Clear();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsAlive(int id, ushort generation) {

            return this.cache.arr[id].generation == generation;

        }

        public ref Entity this[int id] {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return ref this.cache[id];
            }
        }

        public override string ToString() {

            return "Storage: dead(" + this.dead.Count + ") alive(" + this.aliveCount + ")";

        }

    }

}