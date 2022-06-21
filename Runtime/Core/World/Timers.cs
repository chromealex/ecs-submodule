#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Timers {

        public ME.ECS.Collections.DictionaryCopyable<long, tfloat> values;
        public ME.ECS.Collections.HashSetCopyable<int> indexes;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Initialize() {

            if (this.values == null) this.values = PoolDictionaryCopyable<long, tfloat>.Spawn(10);
            if (this.indexes == null) this.indexes = PoolHashSetCopyable<int>.Spawn();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {
        
            PoolDictionaryCopyable<long, tfloat>.Recycle(ref this.values);
            PoolHashSetCopyable<int>.Recycle(ref this.indexes);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void OnEntityDestroy(in Entity entity) {

            this.RemoveAll(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(in Timers other) {

            ArrayUtils.Copy(other.values, ref this.values);
            ArrayUtils.Copy(other.indexes, ref this.indexes);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public unsafe void Update(tfloat deltaTime) {

            var tempList = stackalloc long[this.values.Count];
            var k = 0;
            foreach (var value in this.values) {

                var key = value.Key;
                ref var val = ref this.values.Get(key);
                val -= deltaTime;
                if (val <= 0f) {

                    tempList[k++] = key;

                }

            }

            for (int i = 0; i < k; ++i) {
                this.values.Remove(tempList[i]);
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in Entity entity, int index, tfloat time) {

            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            if (time <= 0f) return;
            var key = MathUtils.GetKey(entity.id, index);
            if (this.values.ContainsKey(key) == true) {

                this.values[key] = time;

            } else {

                if (this.indexes.Contains(index) == false) this.indexes.Add(index);
                this.values.Add(key, time);
                
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public tfloat Read(in Entity entity, int index) {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var key = MathUtils.GetKey(entity.id, index);
            if (this.values.TryGetValue(key, out var timer) == true) {

                return timer;

            }

            return 0f;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref tfloat Get(in Entity entity, int index) {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var key = MathUtils.GetKey(entity.id, index);
            if (this.indexes.Contains(index) == false) this.indexes.Add(index);
            return ref this.values.Get(key);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Remove(in Entity entity, int index) {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var key = MathUtils.GetKey(entity.id, index);
            return this.values.Remove(key);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool RemoveAll(in Entity entity) {
            
            #if WORLD_EXCEPTIONS
            if (entity.IsAlive() == false) {
                
                EmptyEntityException.Throw(entity);
                
            }
            #endif

            var result = false;
            foreach (var index in this.indexes) {

                var key = MathUtils.GetKey(entity.id, index);
                result |= this.values.Remove(key);

            }

            return result;

        }

    }

}