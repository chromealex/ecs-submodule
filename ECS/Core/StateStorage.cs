using System.Collections;
using System.Collections.Generic;

namespace ME.ECS {
    
    using ME.ECS.Collections;
    
    public interface IStorage : IPoolableRecycle {

        int AliveCount { get; }
        int DeadCount { get; }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        bool IsAlive(int id, ushort generation);

        bool ForEach(ListCopyable<Entity> results);
        
        Entity Alloc();
        bool Dealloc(in Entity entity);

        void ApplyDead();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class Storage : IStorage {

        [ME.ECS.Serializer.SerializeField]
        internal BufferArray<ushort> generations;
        [ME.ECS.Serializer.SerializeField]
        private ListCopyable<int> alive;
        [ME.ECS.Serializer.SerializeField]
        private ListCopyable<int> dead;
        [ME.ECS.Serializer.SerializeField]
        private ListCopyable<int> deadPrepared;
        [ME.ECS.Serializer.SerializeField]
        private int aliveCount;
        [ME.ECS.Serializer.SerializeField]
        private int entityId;
        [ME.ECS.Serializer.SerializeField]
        internal ArchetypeEntities archetypes;
        [ME.ECS.Serializer.SerializeField]
        internal EntityVersions versions;

        public override int GetHashCode() {

            return this.versions.GetHashCode() ^ this.aliveCount ^ this.entityId ^ this.dead.Count;

        }

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

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ListCopyable<int> GetAlive() {
            
            return this.alive;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool ForEach(ListCopyable<Entity> results) {
            
            results.Clear();
            for (int i = 0; i < this.alive.Count; ++i) {

                var id = this.alive[i];
                results.Add(new Entity(id, this.generations.arr[id]));
                
            }

            return true;

        }
        
        public void Initialize(int capacity) {
            
            this.generations = PoolArray<ushort>.Spawn(capacity);
            this.alive = PoolListCopyable<int>.Spawn(capacity);
            this.dead = PoolListCopyable<int>.Spawn(capacity);
            this.deadPrepared = PoolListCopyable<int>.Spawn(capacity);
            this.aliveCount = 0;
            this.entityId = -1;
            this.archetypes = PoolClass<ArchetypeEntities>.Spawn();
            this.versions = PoolClass<EntityVersions>.Spawn();

        }
        
        void IPoolableRecycle.OnRecycle() {

            PoolArray<ushort>.Recycle(ref this.generations);
            PoolListCopyable<int>.Recycle(ref this.alive);
            PoolListCopyable<int>.Recycle(ref this.dead);
            PoolListCopyable<int>.Recycle(ref this.deadPrepared);
            this.aliveCount = 0;
            this.entityId = -1;
            PoolClass<ArchetypeEntities>.Recycle(ref this.archetypes);
            PoolClass<EntityVersions>.Recycle(ref this.versions);

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetFreeze(bool freeze) {
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CopyFrom(Storage other) {
            
            ArrayUtils.Copy(other.generations, ref this.generations);
            ArrayUtils.Copy(other.alive, ref this.alive);
            ArrayUtils.Copy(other.dead, ref this.dead);
            ArrayUtils.Copy(other.deadPrepared, ref this.deadPrepared);
            this.aliveCount = other.aliveCount;
            this.entityId = other.entityId;
            this.archetypes.CopyFrom(other.archetypes);
            this.versions.CopyFrom(other.versions);

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Entity Alloc() {

            int id = -1;
            if (this.dead.Count > 0) {
                
                id = this.dead[0];
                this.dead.RemoveAtFast(0);
                
            } else {

                id = ++this.entityId;
                ArrayUtils.Resize(id, ref this.generations, true);

            }
            
            ++this.aliveCount;
            this.alive.Add(id);
            ref var v = ref this.generations.arr[id];
            if (v == 0u) ++v;
            this.versions.Reset(id);
            return new Entity(id, v);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Dealloc(in Entity entity) {

            if (this.IsAlive(entity.id, entity.generation) == false) return false;

            this.deadPrepared.Add(entity.id);
            
            return true;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void IncrementGeneration(in Entity entity) {
            
            // Make this entity not alive, but not completely destroyed at this time
            ++this.generations.arr[entity.id];
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsAlive(int id, ushort generation) {

            return this.generations.arr[id] == generation;

        }
        
        public Entity this[int id] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return new Entity(id, this.generations.arr[id]);
            }
        }
        
        public override string ToString() {
            
            return "Storage: dead(" + this.dead.Count + ") alive(" + this.aliveCount + ")";
            
        }
        
    }

}