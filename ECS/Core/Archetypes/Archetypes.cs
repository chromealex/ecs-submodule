#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct ArchetypeEntities {

        [ME.ECS.Serializer.SerializeField]
        internal NativeBufferArray<Archetype> prevTypes;
        [ME.ECS.Serializer.SerializeField]
        internal NativeBufferArray<Archetype> types;

        public void Recycle() {

            PoolArrayNative<Archetype>.Recycle(ref this.prevTypes);
            PoolArrayNative<Archetype>.Recycle(ref this.types);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(int capacity) {

            NativeArrayUtils.Resize(capacity, ref this.types);
            NativeArrayUtils.Resize(capacity, ref this.prevTypes);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(in Entity entity) {

            var id = entity.id;
            NativeArrayUtils.Resize(id, ref this.types);
            NativeArrayUtils.Resize(id, ref this.prevTypes);

        }

        public void CopyFrom(ArchetypeEntities other) {

            NativeArrayUtils.Copy(in other.prevTypes, ref this.prevTypes);
            NativeArrayUtils.Copy(in other.types, ref this.types);

        }

        public void CopyFrom(in Entity from, in Entity to) {
            
            this.types[to.id] = this.types[from.id];
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Archetype GetPrevious(int entityId) {

            return ref this.prevTypes[entityId];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Archetype Get(int entityId) {

            return ref this.types[entityId];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Archetype GetPrevious(in Entity entity) {

            var id = entity.id;
            //ArrayUtils.Resize(id, ref this.prevTypes);
            return ref this.prevTypes[id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Archetype Get(in Entity entity) {

            var id = entity.id;
            //ArrayUtils.Resize(id, ref this.types);
            return ref this.types[id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Has<T>(in Entity entity) {

            var id = entity.id;
            //ArrayUtils.Resize(id, ref this.types);
            return this.types[id].Has<T>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in EntitiesGroup group, int index) {

            NativeArrayUtils.Copy(this.types, group.fromId, ref this.prevTypes, group.fromId, group.Length);
            for (int i = group.fromId; i <= group.toId; ++i) {
                this.types[i].AddBit(index);
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in EntitiesGroup group, int index) {

            NativeArrayUtils.Copy(this.types, group.fromId, ref this.prevTypes, group.fromId, group.Length);
            for (int i = group.fromId; i <= group.toId; ++i) {
                this.types[i].SubtractBit(index);
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in Entity entity, int index) {

            var id = entity.id;
            var val = this.Get(id);
            this.prevTypes[id] = val;
            this.types[id].AddBit(index);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set<T>(in Entity entity) {

            var id = entity.id;
            var val = this.Get(id);
            this.prevTypes[id] = val;
            this.types[id].Add<T>();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set<T>(int entityId) {

            var id = entityId;
            var val = this.Get(id);
            this.prevTypes[id] = val;
            this.types[id].Add<T>();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(int entityId, int index) {

            var id = entityId;
            var val = this.Get(id);
            this.prevTypes[id] = val;
            this.types[id].AddBit(index);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(int entityId, int index) {

            var id = entityId;
            var val = this.Get(id);
            this.prevTypes[id] = val;
            this.types[id].SubtractBit(index);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in Entity entity, int index) {

            var id = entity.id;
            var val = this.Get(id);
            this.prevTypes[id] = val;
            this.types[id].SubtractBit(index);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove<T>(in Entity entity) {

            var id = entity.id;
            var val = this.Get(id);
            this.prevTypes[id] = val;
            this.types[id].Subtract<T>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveAll<T>(in Entity entity) {

            var id = entity.id;
            this.prevTypes[id].Subtract<T>();
            this.types[id].Subtract<T>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clear(in Entity entity) {

            var id = entity.id;
            this.prevTypes[id].Clear();
            this.types[id].Clear();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveAll<T>() {

            NativeArrayUtils.Copy(in this.types, ref this.prevTypes);
            for (int i = 0; i < this.types.Length; ++i) {

                this.types[i].Subtract<T>();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveAll(in Entity entity) {

            var id = entity.id;
            var val = this.Get(in entity);
            this.prevTypes[id] = val;
            this.types[id].Clear();

        }

    }

}