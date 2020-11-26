using System.Collections;
using System.Collections.Generic;

namespace ME.ECS {
    
    using ME.ECS.Collections;
    
    public interface IStorage : IPoolableRecycle {

        int AliveCount { get; }
        int DeadCount { get; }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        bool IsAlive(int id, ushort version);

        bool ForEach(ListCopyable<Entity> results);
        
        Entity Alloc();
        bool Dealloc(Entity entity);

        void ApplyDead();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class Storage : IStorage {

        [ME.ECS.Serializer.SerializeField]
        private BufferArray<ushort> versions;
        [ME.ECS.Serializer.SerializeField]
        private ListCopyable<int> alive;
        [ME.ECS.Serializer.SerializeField]
        private ListCopyable<int> dead;
        [ME.ECS.Serializer.SerializeField]
        private HashSetCopyable<int> deadPrepared;
        [ME.ECS.Serializer.SerializeField]
        private int aliveCount;
        [ME.ECS.Serializer.SerializeField]
        private int entityId;
        [ME.ECS.Serializer.SerializeField]
        internal ArchetypeEntities archetypes;

        public int AliveCount {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return this.aliveCount;
            }
        }

        public int DeadCount {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return this.dead.Count;
            }
        }

        public ListCopyable<int> GetAlive() {
            
            return this.alive;

        }

        public bool ForEach(ListCopyable<Entity> results) {
            
            results.Clear();
            for (int i = 0; i < this.alive.Count; ++i) {

                var id = this.alive[i];
                results.Add(new Entity(id, this.versions.arr[id]));
                
            }

            return true;

        }
        
        public void Initialize(int capacity) {
            
            this.versions = PoolArray<ushort>.Spawn(capacity);
            this.alive = PoolList<int>.Spawn(capacity);
            this.dead = PoolList<int>.Spawn(capacity);
            this.deadPrepared = PoolHashSetCopyable<int>.Spawn(capacity);
            this.aliveCount = 0;
            this.entityId = -1;
            this.archetypes = PoolClass<ArchetypeEntities>.Spawn();

        }
        
        void IPoolableRecycle.OnRecycle() {

            PoolArray<ushort>.Recycle(ref this.versions);
            PoolList<int>.Recycle(ref this.alive);
            PoolList<int>.Recycle(ref this.dead);
            PoolHashSetCopyable<int>.Recycle(ref this.deadPrepared);
            this.aliveCount = 0;
            this.entityId = -1;
            PoolClass<ArchetypeEntities>.Recycle(ref this.archetypes);

        }
        
        public void SetFreeze(bool freeze) {
        }

        public void CopyFrom(Storage other) {
            
            ArrayUtils.Copy(other.versions, ref this.versions);
            ArrayUtils.Copy(other.alive, ref this.alive);
            ArrayUtils.Copy(other.dead, ref this.dead);
            this.deadPrepared.CopyFrom(other.deadPrepared);
            this.aliveCount = other.aliveCount;
            this.entityId = other.entityId;
            this.archetypes.CopyFrom(other.archetypes);

        }
        
        public Entity Alloc() {

            int id = -1;
            if (this.dead.Count > 0) {
                
                id = this.dead[0];
                this.dead[0] = this.dead[this.dead.Count - 1];
                this.dead.RemoveAt(this.dead.Count - 1);
                
            } else {

                id = ++this.entityId;
                ArrayUtils.Resize(id, ref this.versions, true);

            }
            
            ++this.aliveCount;
            this.alive.Add(id);
            ref var v = ref this.versions.arr[id];
            if (v == 0) ++v;
            return new Entity(id, v);
            
        }

        public bool Dealloc(Entity entity) {

            if (this.IsAlive(entity.id, entity.version) == false) return false;
            if (this.deadPrepared.Contains(entity.id) == true) return false;

            this.deadPrepared.Add(entity.id);
            
            return true;

        }

        public void ApplyDead() {

            foreach (var id in this.deadPrepared) {

                --this.aliveCount;
                ++this.versions.arr[id];
                this.dead.Add(id);
                this.alive.Remove(id);

            }
            this.deadPrepared.Clear();

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(int id, ushort version) {

            return this.versions.arr[id] == version;

        }
        
        public Entity this[int id] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return new Entity(id, this.versions.arr[id]);
            }
        }
        
        public override string ToString() {
            
            return "Storage: dead(" + this.dead.Count + ") alive(" + this.aliveCount + ")";
            
        }
        
        /*
        public struct StorageEnumerator : IEnumerator<int> {

            private Storage storage;
            private int index;

            public StorageEnumerator(Storage storage) {
                
                this.storage = storage;
                this.index = this.storage.ToIndex;

            }

            public int Current {
                get {
                    return this.index;
                }
            }

            public bool MoveNext() {

                do {
                    --this.index;
                } while (this.storage.IsFree(this.index) == true);
                return this.index >= this.storage.FromIndex;

            }

            public void Reset() {

                this.index = this.storage.ToIndex;

            }

            object IEnumerator.Current {
                get {
                    throw new AllocationException();
                }
            }

            bool IEnumerator.MoveNext() {

                throw new AllocationException();

            }

            int IEnumerator<int>.Current {
                get {
                    return this.index;
                }
            }

            void System.IDisposable.Dispose() {
                
            }

        }

        [ME.ECS.Serializer.SerializeField]
        private RefList<Entity> list;
        [ME.ECS.Serializer.SerializeField]
        private bool freeze;
        [ME.ECS.Serializer.SerializeField]
        internal ArchetypeEntities archetypes;

        void IPoolableRecycle.OnRecycle() {

            PoolClass<ArchetypeEntities>.Recycle(ref this.archetypes);
            
            if (this.list != null) PoolRefList<Entity>.Recycle(ref this.list);
            this.freeze = false;

        }

        public int Count {

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {

                return this.list.SizeCount;

            }

        }

        public int FromIndex {

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {

                return this.list.FromIndex;

            }

        }

        public int ToIndex {

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {

                return this.list.SizeCount;

            }

        }

        public ref Entity this[int index] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return ref this.list[index];
            }
        }

        public void ApplyPrepared() {
            
            this.list.ApplyPrepared();
            
        }
        
        IEnumerator IEnumerable.GetEnumerator() {

            throw new AllocationException();

        }

        public StorageEnumerator GetEnumerator() {

            return new StorageEnumerator(this);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsFree(int index) {

            return this.list.IsFree(index);

        }

        public void Initialize(int capacity) {
            
            this.list = PoolRefList<Entity>.Spawn(capacity);
            this.archetypes = PoolClass<ArchetypeEntities>.Spawn();

        }

        public void SetFreeze(bool freeze) {

            this.freeze = freeze;

        }

        public void CopyFrom(Storage other) {
            
            this.archetypes.CopyFrom(other.archetypes);
            if (this.list != null) PoolRefList<Entity>.Recycle(ref this.list);
            this.list = PoolRefList<Entity>.Spawn(other.list.Capacity);
            this.list.CopyFrom(other.list);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ref RefList<Entity> GetData() {

            return ref this.list;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetData(RefList<Entity> data) {

            if (this.freeze == false && data != null && this.list != data) {

                if (this.list != null) PoolRefList<Entity>.Recycle(ref this.list);
                this.list = data;

            }

        }

        public override string ToString() {
            
            return "Storage Count: " + this.list.ToString();
            
        }*/

    }

}