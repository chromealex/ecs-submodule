namespace ME.ECS {

    public static partial class EntityExtensionsV2 {
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity SetAs<TComponent>(this in Entity entity, DataConfigs.DataConfig source) where TComponent : struct, IStructComponent {

            if (source.TryRead(out TComponent c) == true) {

                if (AllComponentTypes<TComponent>.isTag == true) {

                    TComponent data = default;
                    entity.Set(data);

                } else {
                    
                    entity.Set(c);

                }

            } else {
                
                entity.Remove<TComponent>();
                
            }
            return entity;

        }

    }

}