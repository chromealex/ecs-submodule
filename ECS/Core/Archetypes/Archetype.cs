#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
#define BIT_MULTITHREAD_SUPPORT

//#define ARCHETYPE_SIZE_128
//#define ARCHETYPE_SIZE_192
//#define ARCHETYPE_SIZE_256

namespace ME.ECS {

    using Collections;
    using FieldType = System.Int64;

    public static class ArchetypeExt {
        
        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool Has(in this Archetype arch, in Archetype mask) {

            if ((arch.field0 & mask.field0) != mask.field0
                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (arch.field1 & mask.field1) != mask.field1
                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (arch.field2 & mask.field2) != mask.field2
                #if ARCHETYPE_SIZE_256
                || (arch.field3 & mask.field3) != mask.field3
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
        public static bool HasNot(in this Archetype arch, in Archetype mask) {
            
            if ((arch.field0 & mask.field0) != 0
                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (arch.field1 & mask.field1) != 0
                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                || (arch.field2 & mask.field2) != 0
                #if ARCHETYPE_SIZE_256
                || (arch.field3 & mask.field3) != 0
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
        public static bool HasBit(this in Archetype arch, int bit) {

            if (bit < 0 || bit > Archetype.MAX_BIT_INDEX) {
                //throw new Exception($"Attempted to set bit #{bit}, but the maximum is {BitMask.MAX_BIT_INDEX}");
                return false;
            }

            var dataIndex = bit / Archetype.BITS_PER_FIELD;
            var bitIndex = bit % Archetype.BITS_PER_FIELD;
            var mask = (FieldType)1 << bitIndex;
            switch (dataIndex) {
                case 0:
                    return (arch.field0 & mask) != 0;
                #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                case 1:
                    return (arch.field1 & mask) != 0;
                #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
                case 2:
                    return (arch.field2 & mask) != 0;
                #if ARCHETYPE_SIZE_256
                case 3:
                    return (arch.field3 & mask) != 0;
                #endif
                #endif
                #endif
                
                default:
                    throw new System.Exception($"Nonexistent field: {dataIndex}");
            }

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
        internal const int BITS_PER_FIELD = 8 * sizeof(FieldType);
        public const int MAX_BIT_INDEX = Archetype.FIELD_COUNT * Archetype.BITS_PER_FIELD - 1;
        //public const int BitSize = Archetype.FIELD_COUNT * Archetype.BITS_PER_FIELD;

        [ME.ECS.Serializer.SerializeField]
        internal FieldType field0;
        #if ARCHETYPE_SIZE_128 || ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
        [ME.ECS.Serializer.SerializeField]
        internal FieldType field1;
        #if ARCHETYPE_SIZE_192 || ARCHETYPE_SIZE_256
        [ME.ECS.Serializer.SerializeField]
        internal FieldType field2;
        #if ARCHETYPE_SIZE_256
        [ME.ECS.Serializer.SerializeField]
        internal FieldType field3;
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

        /*#if ECS_COMPILE_IL2CPP_OPTIONS
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
        }*/

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

}