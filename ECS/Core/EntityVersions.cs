
namespace ME.ECS {

    using Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class EntityVersions : IPoolableRecycle {

        [ME.ECS.Serializer.SerializeField]
        private BufferArray<ushort> values;

        public override int GetHashCode() {

            var hash = 0;
            for (int i = 0; i < this.values.Length; ++i) {
                hash ^= (int)(this.values.arr[i] + 100000u);
            }
            return hash;
            
        }

        public void OnRecycle() {

            PoolArray<ushort>.Recycle(ref this.values);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Validate(int capacity) {

            ArrayUtils.Resize(capacity, ref this.values);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Validate(in Entity entity) {

            var id = entity.id;
            ArrayUtils.Resize(id, ref this.values);
            
        }

        public void CopyFrom(EntityVersions other) {
            
            ArrayUtils.Copy(in other.values, ref this.values);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ref ushort Get(int entityId) {

            return ref this.values.arr[entityId];

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ref ushort Get(in Entity entity) {

            var id = entity.id;
            return ref this.values.arr[id];

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Increment(in Entity entity) {

            unchecked {
                ++this.values.arr[entity.id];
            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Reset(in Entity entity) {

            this.values.arr[entity.id] = 0;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Reset(int entityId) {

            this.Validate(entityId);
            this.values.arr[entityId] = 0;

        }

    }

}
