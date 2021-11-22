namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class StructComponentsDisposable<TComponent> : StructComponents<TComponent> where TComponent : struct, IStructComponentBase, IComponentDisposable {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        protected override StructRegistryBase SpawnInstance() {

            return PoolRegistries.SpawnDisposable<TComponent>();

        }
        
        public override void OnRecycle() {

            if (this.sharedGroups.sharedGroups != null) {

                foreach (var kv in this.sharedGroups.sharedGroups) {

                    kv.Value.data.OnDispose();

                }

            }

            for (int i = 0; i < this.components.Length; ++i) {
                
                ref var bucket = ref this.components[i];
                if (bucket.state > 0) bucket.data.OnDispose();
                
            }

            base.OnRecycle();
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void Replace(ref Component<TComponent> bucket, in TComponent data) {
            
            bucket.data.OnDispose();
            bucket.data = data;
            
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override void RemoveData(ref Component<TComponent> bucket) {

            bucket.data.OnDispose();
            bucket.data = default;

        }

    }

}