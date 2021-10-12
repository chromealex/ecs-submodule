namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Timers {

        public ME.ECS.Collections.DictionaryCopyable<long, float> values;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Initialize() {

            this.values = PoolDictionaryCopyable<long, float>.Spawn(10);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {
        
            PoolDictionaryCopyable<long, float>.Recycle(ref this.values);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(in Timers other) {

            ArrayUtils.Copy(other.values, ref this.values);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Update(float deltaTime) {

            var temp = PoolList<long>.Spawn(10);
            foreach (var value in this.values) {

                temp.Add(value.Key);

            }

            foreach (var key in temp) {
                
                ref var val = ref this.values.Get(key);
                val -= deltaTime;
                if (val <= 0f) {

                    this.values.Remove(key);

                }
                
            }
            PoolList<long>.Recycle(ref temp);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(in Entity entity, int index, float time) {
            
            var key = MathUtils.GetKey(entity.id, index);
            if (this.values.ContainsKey(key) == true) {

                this.values[key] = time;

            } else {
                
                this.values.Add(key, time);
                
            }
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public float Read(in Entity entity, int index) {
            
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
            
            var key = MathUtils.GetKey(entity.id, index);
            return ref this.values.Get(key);

        }

    }

}