namespace ME.ECS {

    #if COMPONENTS_COPYABLE
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class StructComponentsCopyable<TComponent> : StructComponents<TComponent> where TComponent : struct, IStructCopyable<TComponent> {

        public override void Recycle() {
            
            PoolRegistries.Recycle(this);

        }

        internal struct CopyItem : IArrayElementCopy<Component<TComponent>> {

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Copy(in Component<TComponent> @from, ref Component<TComponent> to) {

                var hasFrom = (from.state > 0);
                var hasTo = (to.state > 0);
                if (hasFrom == false && hasTo == false) return;

                to.state = from.state;
                to.version = from.version;

                if (hasFrom == false && hasTo == true) {
                    
                    to.data.OnRecycle();
                    
                } else {

                    to.data.CopyFrom(in from.data);

                }

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Recycle(ref Component<TComponent> item) {

                item.data.OnRecycle();
                item = default;

            }

        }

        #if !SHARED_COMPONENTS_DISABLED
        internal struct ElementCopy : IArrayElementCopy<SharedDataStorage<TComponent>> {

            public void Copy(in SharedDataStorage<TComponent> @from, ref SharedDataStorage<TComponent> to) {
                
                to.data.CopyFrom(from.data);
                
            }

            public void Recycle(ref SharedDataStorage<TComponent> item) {
                
                item.data.OnRecycle();
                item = default;

            }

        }
        #endif

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

            return PoolRegistries.SpawnCopyable<TComponent>();

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

            var _other = (StructComponents<TComponent>)other;
            #if !COMPONENTS_VERSION_NO_STATE_DISABLED
            if (AllComponentTypes<TComponent>.isVersionedNoState == true) _other.versionsNoState = this.versionsNoState;
            #endif
            ArrayUtils.Copy(_other.components, ref this.components, new CopyItem());

            #if !SHARED_COMPONENTS_DISABLED
            if (AllComponentTypes<TComponent>.isShared == true) SharedGroupsAPI<TComponent>.CopyFrom(ref this.sharedStorage, _other.sharedStorage, new ElementCopy());
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