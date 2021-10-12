namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Timers {

        public ME.ECS.Collections.DictionaryCopyable<long, float> values;
        public ME.ECS.Collections.HashSetCopyable<int> indexes;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Initialize() {

            this.values = PoolDictionaryCopyable<long, float>.Spawn(10);
            this.indexes = PoolHashSetCopyable<int>.Spawn();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {
        
            PoolDictionaryCopyable<long, float>.Recycle(ref this.values);
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
        public void Update(float deltaTime) {

            var temp = PoolList<long>.Spawn(10);
            foreach (var value in this.values) {

                var key = value.Key;
                ref var val = ref this.values.Get(key);
                val -= deltaTime;
                if (val <= 0f) {

                    temp.Add(key);

                }

            }

            foreach (var key in temp) {
                
                this.values.Remove(key);

            }
            PoolList<long>.Recycle(ref temp);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in Entity entity, int index, float time) {

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
        public float Read(in Entity entity, int index) {
            
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
        public ref float Get(in Entity entity, int index) {
            
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