namespace ME.ECS {

    #if COMPONENTS_COPYABLE
    using Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class StructComponentsBlittableCopyable<TComponent> : StructComponentsBlittable<TComponent> where TComponent : struct, IStructCopyable<TComponent> {

        public override void Recycle() {
            
            PoolRegistries.Recycle(this);

        }

        public override bool IsNeedToDispose() {

            return true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void RemoveData(in Entity entity, ref Component<TComponent> bucket) {

            bucket.data.OnRecycle();
            base.RemoveData(in entity, ref bucket);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnBlittableCopyable<TComponent>();

        }

        public override void OnRecycle() {

            #if !SHARED_COMPONENTS_DISABLED
            if (this.sharedStorage.sharedGroups != null) {

                foreach (var kv in this.sharedStorage.sharedGroups) {

                    kv.Value.data.OnRecycle();

                }

            }
            #endif

            for (int i = 0; i < this.components.Length; ++i) {
                
                ref var bucket = ref this.components[i];
                if (bucket.state > 0) bucket.data.OnRecycle();
                
            }
            
            base.OnRecycle();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Replace(ref Component<TComponent> bucket, in TComponent data) {
            
            bucket.data.OnRecycle();
            bucket.data = data;
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void CopyFrom(StructRegistryBase other) {

            var _other = (StructComponentsBlittable<TComponent>)other;
            NativeArrayUtils.Copy(_other.components, ref this.components, new StructComponentsCopyable<TComponent>.CopyItem());

            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.CopyFrom(ref this.sharedStorage, _other.sharedStorage, new StructComponentsCopyable<TComponent>.ElementCopy());
            #endif
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override byte CopyFromState(in Entity @from, in Entity to) {
            
            ref var fromBucket = ref this.components[from.id];
            ref var toBucket = ref this.components[to.id];
            var hasFrom = (fromBucket.state > 0);
            var hasTo = (toBucket.state > 0);
            
            toBucket.state = fromBucket.state;
            if (hasFrom == true && hasTo == true) {

                toBucket.data.CopyFrom(in fromBucket.data);
                
            } else if (hasFrom == true && hasTo == false) {
                
                toBucket.data.CopyFrom(in fromBucket.data);

            } else if (hasFrom == false && hasTo == true) {
                
                toBucket.data.OnRecycle();
                
            }
            
            return toBucket.state;
            
        }

    }
    #endif

}