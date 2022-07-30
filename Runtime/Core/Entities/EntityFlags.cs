#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Collections.V3;

    [System.Flags]
    public enum EntityFlag : int {

        None = 0x0,
        /// <summary>
        /// Destroy entity at the end of tick
        /// </summary>
        OneShot = 0x1,
        /// <summary>
        /// Destroy entity at the end of tick if it has no components
        /// </summary>
        DestroyWithoutComponents = 0x2,

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct EntityVersions {

        [ME.ECS.Serializer.SerializeField]
        private MemArrayAllocator<ushort> values;
        private static ushort defaultValue;

        public EntityVersions(ref MemoryAllocator allocator, int capacity) {

            this.values = new MemArrayAllocator<ushort>(ref allocator, capacity, ME.ECS.Collections.ClearOptions.ClearMemory, growFactor: 2);
            this.Validate(ref allocator, capacity);
            
        }

        public int GetHash(in MemoryAllocator allocator) {

            var hash = 0;
            for (int i = 0; i < this.values.Length(in allocator); ++i) {
                hash ^= (int)(this.values[in allocator, i] + 100000);
            }

            return hash;

        }

        public void Dispose(ref MemoryAllocator allocator) {

            this.values.Dispose(ref allocator);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(ref MemoryAllocator allocator, int capacity) {

            this.values.Resize(ref allocator, capacity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(ref MemoryAllocator allocator, in Entity entity) {

            this.values.Resize(ref allocator, entity.id + 1);
            //NativeArrayUtils.Resize(id, ref this.values, true);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref ushort Get(in MemoryAllocator allocator, int entityId) {

            return ref this.values[in allocator, entityId];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref ushort Get(in MemoryAllocator allocator, in Entity entity) {

            var id = entity.id;
            if (id >= this.values.Length(in allocator)) return ref EntityVersions.defaultValue;
            return ref this.values[in allocator, id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Increment(in MemoryAllocator allocator, in Entity entity) {

            unchecked {
                ++this.values[in allocator, entity.id];
            }

            #if ENTITY_VERSION_INCREMENT_ACTIONS
            World world = Worlds.currentWorld;
            world.RaiseEntityVersionIncrementAction(entity, this.values[in allocator, entity.id]);
            #endif

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Increment(in MemoryAllocator allocator, int entityId) {

            unchecked {
                ++this.values[in allocator, entityId];
            }

            #if ENTITY_VERSION_INCREMENT_ACTIONS
            World world = Worlds.currentWorld;
            world.RaiseEntityVersionIncrementAction(world.GetEntityById(entityId), this.values[in allocator, entityId]);
            #endif

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(in MemoryAllocator allocator, in Entity entity) {

            this.values[in allocator, entity.id] = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(ref MemoryAllocator allocator, int entityId) {

            this.Validate(ref allocator, entityId + 1);
            this.values[in allocator, entityId] = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(ref MemoryAllocator allocator, int fromId, int toId) {

            this.values.Clear(in allocator, fromId, toId - fromId);
            
        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct EntityFlags {

        [ME.ECS.Serializer.SerializeField]
        private MemArrayAllocator<byte> values;
        private static byte defaultValue;

        public EntityFlags(ref MemoryAllocator allocator, int capacity) {

            this.values = new MemArrayAllocator<byte>(ref allocator, capacity, ME.ECS.Collections.ClearOptions.ClearMemory, growFactor: 2);
            this.Validate(ref allocator, capacity);
            
        }

        public int GetHash(in MemoryAllocator allocator) {

            var hash = 0;
            for (int i = 0; i < this.values.Length(in allocator); ++i) {
                hash ^= (this.values[in allocator, i] + 100000);
            }

            return hash;

        }

        public void Dispose(ref MemoryAllocator allocator) {

            this.values.Dispose(ref allocator);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(ref MemoryAllocator allocator, int capacity) {

            this.values.Resize(ref allocator, capacity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(ref MemoryAllocator allocator, in Entity entity) {

            this.values.Resize(ref allocator, entity.id + 1);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref byte Get(in MemoryAllocator allocator, int entityId) {

            return ref this.values[in allocator, entityId];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref byte Get(in MemoryAllocator allocator, in Entity entity) {

            var id = entity.id;
            if (id >= this.values.Length(in allocator)) return ref EntityFlags.defaultValue;
            return ref this.values[in allocator, id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in MemoryAllocator allocator, int entityId, EntityFlag flags) {

            this.values[in allocator, entityId] = (byte)flags;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(in MemoryAllocator allocator, in Entity entity) {

            this.values[in allocator, entity.id] = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(ref MemoryAllocator allocator, int entityId) {

            this.Validate(ref allocator, entityId + 1);
            this.values[in allocator, entityId] = default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(ref MemoryAllocator allocator, int fromId, int toId) {

            this.values.Clear(in allocator, fromId, toId - fromId);

        }

    }
    
}