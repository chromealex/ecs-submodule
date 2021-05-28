
namespace ME.ECSBurst {

    using Collections;
    using Unity.Collections;
    using FieldType = System.Int64;
    using static MemUtilsCuts;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public readonly struct Entity {

        public readonly int id;
        public readonly ushort generation;

        public Entity(int id, ushort generation) {

            this.id = id;
            this.generation = generation;

        }

        public override string ToString() {
            
            return $"#{this.id} Gen:{this.generation}";
            
        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static unsafe class EntitiesAPI {

        public static T Get<T>(this Entity entity) where T : struct, IComponentBase {

            var world = mref<World>((void*)Worlds.currentInternalWorld.Data);
            return world.Get<T>(entity);

        }

        public static T Read<T>(this Entity entity) where T : struct, IComponentBase {

            var world = mref<World>((void*)Worlds.currentInternalWorld.Data);
            return world.Get<T>(entity);

        }

        public static bool Remove<T>(this Entity entity) where T : struct, IComponentBase {

            var world = mref<World>((void*)Worlds.currentInternalWorld.Data);
            return world.Remove<T>(entity);

        }

        public static void Set<T>(this Entity entity, T data) where T : struct, IComponentBase {

            var world = mref<World>((void*)Worlds.currentInternalWorld.Data);
            world.Set<T>(entity, data);

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct EntityVersions {

        private NativeBufferArray<ushort> values;
        private static ushort defaultValue;

        public override int GetHashCode() {

            var hash = 0;
            for (int i = 0; i < this.values.Length; ++i) {
                hash ^= (int)(this.values.arr[i] + 100000u);
            }

            return hash;

        }

        public void Recycle() {

            PoolArrayNative<ushort>.Recycle(ref this.values);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(int capacity) {

            ArrayUtils.Resize(capacity, ref this.values);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(in Entity entity) {

            var id = entity.id;
            ArrayUtils.Resize(id, ref this.values, true);

        }

        public void CopyFrom(EntityVersions other) {

            ArrayUtils.Copy(in other.values, ref this.values);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref ushort Get(int entityId) {

            return ref this.values.arr.GetRef(entityId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref ushort Get(in Entity entity) {

            var id = entity.id;
            if (id >= this.values.Length) return ref EntityVersions.defaultValue;
            return ref this.values.arr.GetRef(id);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Increment(in Entity entity) {

            unchecked {
                ++this.values.arr.GetRef(entity.id);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Increment(int entityId) {

            unchecked {
                ++this.values.arr.GetRef(entityId);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(in Entity entity) {

            this.values.arr.GetRef(entity.id) = 0;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Reset(int entityId) {

            this.Validate(entityId);
            this.values.arr.GetRef(entityId) = 0;

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct ArchetypeEntities {

        internal NativeBufferArray<Archetype> prevTypes;
        internal NativeBufferArray<Archetype> types;

        public void Recycle() {

            PoolArrayNative<Archetype>.Recycle(ref this.prevTypes);
            PoolArrayNative<Archetype>.Recycle(ref this.types);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(int capacity) {

            ArrayUtils.Resize(capacity, ref this.types);
            ArrayUtils.Resize(capacity, ref this.prevTypes);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Validate(in Entity entity) {

            var id = entity.id;
            ArrayUtils.Resize(id, ref this.types);
            ArrayUtils.Resize(id, ref this.prevTypes);

        }

        public void CopyFrom(ArchetypeEntities other) {

            ArrayUtils.Copy(in other.prevTypes, ref this.prevTypes);
            ArrayUtils.Copy(in other.types, ref this.types);

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

            ArrayUtils.Copy(in this.types, ref this.prevTypes);
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

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Archetype : System.IEquatable<Archetype> {
        
        #if ARCHETYPE_SIZE_256
        private const int FIELD_COUNT = 4;
        #elif ARCHETYPE_SIZE_192
        private const int FIELD_COUNT = 3;
        #elif ARCHETYPE_SIZE_128
        private const int FIELD_COUNT = 2;
        #else
        private const int FIELD_COUNT = 1;
        #endif
        private const int BITS_PER_FIELD = 8 * sizeof(FieldType);
        public const int MAX_BIT_INDEX = Archetype.FIELD_COUNT * Archetype.BITS_PER_FIELD - 1;
        //public const int BitSize = Archetype.FIELD_COUNT * Archetype.BITS_PER_FIELD;

        private FieldType field0;
        #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
        private FieldType field1;
        #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
        private FieldType field2;
        #if ARCHETYPE_SIZE_256
        private FieldType field3;
        #endif
        #endif
        #endif

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Archetype(in Archetype value) {

            this.field0 = value.field0;
            #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            this.field1 = value.field1;
            #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            this.field2 = value.field2;
            #if ARCHETYPE_SIZE_256
            this.field3 = value.field3;
            #endif
            #endif
            #endif

        }

        public int Count {
            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
             Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
             Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                var count = 0;
                for (int i = 0; i <= Archetype.MAX_BIT_INDEX; ++i) {
                    if (this.HasBit(i) == true) ++count;
                }

                return count;
            }
        }

        public int BitsCount {
            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
             Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
             Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return Archetype.MAX_BIT_INDEX;
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool HasBit(int bit) {

            if (bit < 0 || bit > Archetype.MAX_BIT_INDEX) {
                //throw new Exception($"Attempted to set bit #{bit}, but the maximum is {BitMask.MAX_BIT_INDEX}");
                return false;
            }

            var dataIndex = bit / Archetype.BITS_PER_FIELD;
            var bitIndex = bit % Archetype.BITS_PER_FIELD;
            var mask = (FieldType)1 << bitIndex;
            switch (dataIndex) {
                case 0:
                    return (this.field0 & mask) != 0;
                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                case 1:
                    return (this.field1 & mask) != 0;
                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                case 2:
                    return (this.field2 & mask) != 0;
                #if ARCHETYPE_SIZE_256
                case 3:
                    return (this.field3 & mask) != 0;
                #endif
                #endif
                #endif
                
                default:
                    throw new System.Exception($"Nonexistent field: {dataIndex}");
            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AddBits(in Archetype bits) {

            this.field0 |= bits.field0;
            #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            this.field1 |= bits.field1;
            #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            this.field2 |= bits.field2;
            #if ARCHETYPE_SIZE_256
            this.field3 |= bits.field3;
            #endif
            #endif
            #endif

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AddBit(int bit) {

            #if UNITY_EDITOR
            if (bit < 0 || bit > Archetype.MAX_BIT_INDEX) {
                throw new System.Exception($"Attempted to set bit #{bit}, but the maximum is {Archetype.MAX_BIT_INDEX}");
            }
            #endif

            var dataIndex = bit / Archetype.BITS_PER_FIELD;
            var bitIndex = bit % Archetype.BITS_PER_FIELD;
            var mask = (FieldType)1 << bitIndex;
            switch (dataIndex) {
                case 0: {
                    ref var f = ref this.field0;
                    #if BIT_MULTITHREAD_SUPPORT
                    while (true) {
                        var oldFlags = f;
                        var newFlags = f | mask;
                        if (System.Threading.Interlocked.CompareExchange(ref f, newFlags, oldFlags) == oldFlags) break;
                    }
                    #else
                    f |= mask;
                    #endif
                }
                    break;

                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                case 1: {
                    ref var f = ref this.field1;
                    #if BIT_MULTITHREAD_SUPPORT
                    while (true) {
                        var oldFlags = f;
                        var newFlags = f | mask;
                        if (System.Threading.Interlocked.CompareExchange(ref f, newFlags, oldFlags) == oldFlags) break;
                    }
                    #else
                    f |= mask;
                    #endif
                }
                    break;

                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                case 2: {
                    ref var f = ref this.field2;
                    #if BIT_MULTITHREAD_SUPPORT
                    while (true) {
                        var oldFlags = f;
                        var newFlags = f | mask;
                        if (System.Threading.Interlocked.CompareExchange(ref f, newFlags, oldFlags) == oldFlags) break;
                    }
                    #else
                    f |= mask;
                    #endif
                }
                    break;

                #if ARCHETYPE_SIZE_256
                case 3: {
                    ref var f = ref this.field3;
                    #if BIT_MULTITHREAD_SUPPORT
                    while (true) {
                        var oldFlags = f;
                        var newFlags = f | mask;
                        if (System.Threading.Interlocked.CompareExchange(ref f, newFlags, oldFlags) == oldFlags) break;
                    }
                    #else
                    f |= mask;
                    #endif
                }
                    break;
                #endif
                #endif
                #endif

                default:
                    throw new System.Exception($"Nonexistent field: {dataIndex}");
            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear() {

            this.field0 = 0;
            #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            this.field1 = 0;
            #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            this.field2 = 0;
            #if ARCHETYPE_SIZE_256
            this.field3 = 0;
            #endif
            #endif
            #endif

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SubtractBit(int bit) {

            if (bit < 0 || bit > Archetype.MAX_BIT_INDEX) {
                throw new System.Exception($"Attempted to set bit #{bit}, but the maximum is {Archetype.MAX_BIT_INDEX}");
            }

            var dataIndex = bit / Archetype.BITS_PER_FIELD;
            var bitIndex = bit % Archetype.BITS_PER_FIELD;
            var mask = (FieldType)1 << bitIndex;

            switch (dataIndex) {
                case 0: {
                    ref var f = ref this.field0;
                    #if BIT_MULTITHREAD_SUPPORT
                    while (true) {
                        var oldFlags = f;
                        var newFlags = f & (~mask);
                        if (System.Threading.Interlocked.CompareExchange(ref f, newFlags, oldFlags) == oldFlags) break;
                    }
                    #else
                    f &= ~mask;
                    #endif
                }
                    break;

                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                case 1: {
                    ref var f = ref this.field1;
                    #if BIT_MULTITHREAD_SUPPORT
                    while (true) {
                        var oldFlags = f;
                        var newFlags = f & (~mask);
                        if (System.Threading.Interlocked.CompareExchange(ref f, newFlags, oldFlags) == oldFlags) break;
                    }
                    #else
                    f &= ~mask;
                    #endif
                }
                    break;

                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                case 2: {
                    ref var f = ref this.field2;
                    #if BIT_MULTITHREAD_SUPPORT
                    while (true) {
                        var oldFlags = f;
                        var newFlags = f & (~mask);
                        if (System.Threading.Interlocked.CompareExchange(ref f, newFlags, oldFlags) == oldFlags) break;
                    }
                    #else
                    f &= ~mask;
                    #endif
                }
                    break;

                #if ARCHETYPE_SIZE_256
                case 3: {
                    ref var f = ref this.field3;
                    #if BIT_MULTITHREAD_SUPPORT
                    while (true) {
                        var oldFlags = f;
                        var newFlags = f & (~mask);
                        if (System.Threading.Interlocked.CompareExchange(ref f, newFlags, oldFlags) == oldFlags) break;
                    }
                    #else
                    f &= ~mask;
                    #endif
                }
                    break;
                #endif
                #endif
                #endif

                default:
                    throw new System.Exception($"Nonexistent field: {dataIndex}");
            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public bool this[int index] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                if (index < 0 || index > Archetype.MAX_BIT_INDEX) {
                    throw new System.Exception($"Invalid bit index: {index}");
                }

                var dataIndex = index / Archetype.BITS_PER_FIELD;
                var bitIndex = index % Archetype.BITS_PER_FIELD;
                switch (dataIndex) {
                    case 0:
                        return (this.field0 & ((FieldType)1 << bitIndex)) != 0;

                    #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                    case 1:
                        return (this.field1 & ((FieldType)1 << bitIndex)) != 0;

                    #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                    case 2:
                        return (this.field2 & ((FieldType)1 << bitIndex)) != 0;

                    #if ARCHETYPE_SIZE_256
                    case 3:
                        return (this.field3 & ((FieldType)1 << bitIndex)) != 0;
                    #endif
                    #endif
                    #endif

                    default:
                        return false;
                }
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() {

            return (int)this.field0
                   #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                   ^ (int)this.field1
                   #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                   ^ (int)this.field2
                   #if ARCHETYPE_SIZE_256
                   ^ (int)this.field3
                   #endif
                   #endif
                   #endif
                ;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(Archetype other) {

            if (this.field0 != other.field0) {
                return false;
            }

            #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            if (this.field1 != other.field1) {
                return false;
            }

            #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            if (this.field2 != other.field2) {
                return false;
            }

            #if ARCHETYPE_SIZE_256
            if (this.field3 != other.field3) {
                return false;
            }
            #endif
            #endif
            #endif

            return true;
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public override bool Equals(object obj) {
            if (obj is Archetype) {
                return this.Equals((Archetype)obj);
            }

            return base.Equals(obj);
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Archetype mask1, Archetype mask2) {
            return mask1.Equals(mask2);
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Archetype mask1, Archetype mask2) {
            return !mask1.Equals(mask2);
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsEmpty() {

            return this.field0 == 0
                   #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                   && this.field1 == 0
                   #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                   && this.field2 == 0
                   #if ARCHETYPE_SIZE_256
                   && this.field3 == 0
                   #endif
                   #endif
                   #endif
                ;
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Has(in Archetype mask) {

            if ((this.field0 & mask.field0) != mask.field0
                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (this.field1 & mask.field1) != mask.field1
                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (this.field2 & mask.field2) != mask.field2
                #if ARCHETYPE_SIZE_256
                || (this.field3 & mask.field3) != mask.field3
                #endif
                #endif
                #endif
                ) {
                return false;
            }

            return true;
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool HasAny(in Archetype mask) {

            if ((this.field0 & mask.field0) != 0
                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (this.field1 & mask.field1) != 0
                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (this.field2 & mask.field2) != 0
                #if ARCHETYPE_SIZE_256
                || (this.field3 & mask.field3) != 0
                #endif
                #endif
                #endif
                ) {
                return false;
            }

            return true;
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool HasNot(in Archetype mask) {

            if ((this.field0 & mask.field0) != 0
                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (this.field1 & mask.field1) != 0
                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (this.field2 & mask.field2) != 0
                #if ARCHETYPE_SIZE_256
                || (this.field3 & mask.field3) != 0
                #endif
                #endif
                #endif
                ) {
                return false;
            }

            return true;
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public override string ToString() {
            var builder = new System.Text.StringBuilder();
            var fields = new FieldType[Archetype.FIELD_COUNT];
            fields[0] = this.field0;
            #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            fields[1] = this.field1;
            #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
            fields[2] = this.field2;
            #if ARCHETYPE_SIZE_256
            fields[3] = this.field3;
            #endif
            #endif
            #endif
            for (var i = 0; i < Archetype.FIELD_COUNT; ++i) {
                var binaryString = System.Convert.ToString((long)fields[i], 2);
                builder.Append(binaryString.PadLeft(Archetype.BITS_PER_FIELD, '0'));
            }

            return builder.ToString();
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Has<T>() {

            var tId = WorldUtilities.GetComponentTypeId<T>();
            if (tId == -1) return false;
            return this.HasBit(tId);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add(in Archetype archetype) {

            this.AddBits(in archetype);

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add<T>() {

            this.AddBit(WorldUtilities.GetComponentTypeId<T>());

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Subtract<T>() {

            var tId = WorldUtilities.GetComponentTypeId<T>();
            if (tId == -1) return;
            this.SubtractBit(tId);

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Storage {

        public bool isCreated;
        internal NativeBufferArray<Entity> cache;
        internal NativeList<int> alive;
        internal NativeList<int> dead;
        internal NativeList<int> deadPrepared;
        private int aliveCount;
        private int entityId;
        internal ArchetypeEntities archetypes;
        internal EntityVersions versions;

        public void Dispose() {
            
            this.Recycle();
            
        }

        public override int GetHashCode() {

            return this.versions.GetHashCode() ^ this.aliveCount ^ this.entityId ^ this.dead.Length;

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
                return this.dead.Length;
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeList<int> GetAlive() {

            return this.alive;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool ForEach(NativeList<Entity> results) {

            results.Clear();
            for (int i = 0; i < this.alive.Length; ++i) {

                var id = this.alive[i];
                results.Add(this.cache.arr[id]);

            }

            return true;

        }

        public void Initialize(int capacity) {

            this.cache = PoolArrayNative<Entity>.Spawn(capacity);
            this.alive = new NativeList<int>(capacity, Allocator.Persistent);
            this.dead = new NativeList<int>(capacity, Allocator.Persistent);
            this.deadPrepared = new NativeList<int>(capacity, Allocator.Persistent);
            this.aliveCount = 0;
            this.entityId = -1;
            this.archetypes = new ArchetypeEntities();//PoolClass<ArchetypeEntities>.Spawn();
            this.versions = new EntityVersions();//PoolClass<EntityVersions>.Spawn();

        }

        public void Recycle() {

            PoolArrayNative<Entity>.Recycle(ref this.cache);
            this.alive.Dispose();
            this.dead.Dispose();
            this.deadPrepared.Dispose();
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

            ArrayUtils.Copy(other.cache, ref this.cache);
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

            return this.dead.Length == 0;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref Entity Alloc() {

            int id = -1;
            if (this.dead.Length > 0) {

                id = this.dead[0];
                this.dead.RemoveAtSwapBack(0);

            } else {

                id = ++this.entityId;
                ArrayUtils.Resize(id, ref this.cache, true);

            }

            ++this.aliveCount;
            this.alive.Add(id);
            ref var e = ref this.cache.arr[id];
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
            this.cache.arr[entity.id] = new Entity(entity.id, unchecked((ushort)(entity.generation + 1)));
            return ref this.cache.arr[entity.id];

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void ApplyDead() {

            var cnt = this.deadPrepared.Length;
            if (cnt > 0) {

                for (int i = 0; i < cnt; ++i) {

                    var id = this.deadPrepared[i];
                    --this.aliveCount;
                    this.dead.Add(id);
                    this.alive.RemoveAt(this.alive.IndexOf(id));

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
                return ref this.cache.arr[id];
            }
        }

        public override string ToString() {

            return "Storage: dead(" + this.dead.Length + ") alive(" + this.aliveCount + ")";

        }

    }

}
