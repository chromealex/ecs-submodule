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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Archetype GetPrevious(int entityId) {

            return ref this.prevTypes.arr[entityId];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Archetype Get(int entityId) {

            return ref this.types.arr[entityId];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Archetype GetPrevious(in Entity entity) {

            var id = entity.id;
            //ArrayUtils.Resize(id, ref this.prevTypes);
            return ref this.prevTypes.arr[id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Archetype Get(in Entity entity) {

            var id = entity.id;
            //ArrayUtils.Resize(id, ref this.types);
            return ref this.types.arr[id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Has<T>(in Entity entity) {

            var id = entity.id;
            //ArrayUtils.Resize(id, ref this.types);
            return this.types.arr[id].Has<T>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in Entity entity, int index) {

            var id = entity.id;
            var val = this.Get(id);
            this.prevTypes.arr[id] = val;
            this.types.arr[id].AddBit(index);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set<T>(in Entity entity) {

            var id = entity.id;
            var val = this.Get(id);
            this.prevTypes.arr[id] = val;
            this.types.arr[id].Add<T>();
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove(in Entity entity, int index) {

            var id = entity.id;
            var val = this.Get(id);
            this.prevTypes.arr[id] = val;
            this.types.arr[id].SubtractBit(index);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Remove<T>(in Entity entity) {

            var id = entity.id;
            var val = this.Get(id);
            this.prevTypes.arr[id] = val;
            this.types.arr[id].Subtract<T>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveAll<T>(in Entity entity) {

            var id = entity.id;
            this.prevTypes.arr[id].Subtract<T>();
            this.types.arr[id].Subtract<T>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clear(in Entity entity) {

            var id = entity.id;
            this.prevTypes.arr[id].Clear();
            this.types.arr[id].Clear();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveAll<T>() {

            NativeArrayUtils.Copy(in this.types, ref this.prevTypes);
            for (int i = 0; i < this.types.Length; ++i) {

                this.types.arr[i].Subtract<T>();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void RemoveAll(in Entity entity) {

            var id = entity.id;
            var val = this.Get(in entity);
            this.prevTypes.arr[id] = val;
            this.types.arr[id].Clear();

        }

    }

}