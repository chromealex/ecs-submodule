#if !ENTITY_TIMERS_DISABLED
#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {
    
    using Collections.V3;
    using Collections.MemoryAllocator;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Timers {

        public Dictionary<ulong, tfloat> values;
        public HashSet<uint> indexes;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Initialize(ref MemoryAllocator allocator) {

            this.values = new Dictionary<ulong, sfloat>(ref allocator, 10);
            this.indexes = new HashSet<uint>(ref allocator, 10);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose(ref MemoryAllocator allocator) {
        
            this.values.Dispose(ref allocator);
            this.indexes.Dispose(ref allocator);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void OnEntityDestroy(ref MemoryAllocator allocator, in Entity entity) {

            this.RemoveAll(ref allocator, in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public unsafe void Update(ref MemoryAllocator allocator, tfloat deltaTime) {

            var tempList = stackalloc ulong[this.values.Count(in allocator)];
            var k = 0;
            var e = this.values.GetEnumerator(in allocator);
            while (e.MoveNext() == true) {

                var value = e.Current;
                var key = value.Key;
                ref var val = ref this.values.GetValue(ref allocator, key);
                val -= deltaTime;
                if (val <= 0f) {

                    tempList[k++] = key;

                }

            }
            e.Dispose();

            for (int i = 0; i < k; ++i) {
                this.values.Remove(ref allocator, tempList[i]);
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(ref MemoryAllocator allocator, in Entity entity, uint index, tfloat time) {

            if (time <= 0f) return;
            var key = MathUtils.GetKey((uint)entity.id, index);
            if (this.values.ContainsKey(in allocator, key) == true) {

                this.values[in allocator, key] = time;

            } else {

                this.indexes.Add(ref allocator, index);
                this.values.Add(ref allocator, key, time);
                
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public tfloat Read(in MemoryAllocator allocator, in Entity entity, uint index) {
            
            var key = MathUtils.GetKey((uint)entity.id, index);
            if (this.values.TryGetValue(in allocator, key, out var timer) == true) {

                return timer;

            }

            return 0f;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref tfloat Get(ref MemoryAllocator allocator, in Entity entity, uint index) {
            
            var key = MathUtils.GetKey((uint)entity.id, index);
            if (this.indexes.Contains(in allocator, index) == false) this.indexes.Add(ref allocator, index);
            return ref this.values.GetValue(ref allocator, key);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Remove(ref MemoryAllocator allocator, in Entity entity, uint index) {
            
            var key = MathUtils.GetKey((uint)entity.id, index);
            return this.values.Remove(ref allocator, key);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool RemoveAll(ref MemoryAllocator allocator, in Entity entity) {
            
            var result = false;
            var e = this.indexes.GetEnumerator(in allocator);
            while (e.MoveNext() == true) {

                var index = e.Current;
                var key = MathUtils.GetKey((uint)entity.id, index);
                result |= this.values.Remove(ref allocator, key);

            }
            e.Dispose();

            return result;

        }

    }

}
#endif