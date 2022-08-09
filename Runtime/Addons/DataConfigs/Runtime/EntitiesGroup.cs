#if !ENTITIES_GROUP_DISABLED
using ME.ECS.Collections;

namespace ME.ECS.DataConfigs {

    public partial class DataConfig : ConfigBase {

        /// <summary>
        /// Apply config onto the group.
        /// </summary>
        /// <param name="group"></param>
        public override void Apply(in EntitiesGroup group) {
            
            if (this.GetType() == typeof(DataConfig)) {
                
                this.ApplyImpl(in group);
                
            } else {
                
                for (int e = 0; e < group.slice.Length; ++e) {
                    
                    this.Apply(group.slice[e]);
                    
                }
                
            }
            
        }
        
        protected void ApplyImpl(in EntitiesGroup group) {

            this.Prewarm();

            var world = Worlds.currentWorld;
            for (int i = 0; i < this.removeStructComponents.Length; ++i) {

                var dataIndex = this.GetComponentDataIndexByTypeRemoveWithCache(this.removeStructComponents[i], i);
                if (group.copyMode == true) {
                
                    var maxEntity = group.slice[group.slice.Length - 1];
                    if (world.HasDataBit(in maxEntity, dataIndex) == true) {
                        for (int e = 0; e < group.Length; ++e) {

                            ref readonly var entity = ref group.slice.GetRefRead(e);
                            var data = world.ReadData(in entity, dataIndex);
                            if (data is IComponentDeinitializable deinitializable) {

                                deinitializable.Deinitialize(in entity);

                            }

                        }

                    }
                    
                } else {
                    
                    for (int e = 0; e < group.Length; ++e) {

                        ref readonly var entity = ref group.slice.GetRefRead(e);
                        if (world.HasDataBit(in entity, dataIndex) == true) {

                            var data = world.ReadData(in entity, dataIndex);
                            if (data is IComponentDeinitializable deinitializable) {

                                deinitializable.Deinitialize(in entity);

                            }

                        }

                    }
                    
                }

                group.Remove(dataIndex);

            }

            for (int i = 0; i < this.structComponents.Length; ++i) {

                var dataIndex = this.GetComponentDataIndexByTypeWithCache(this.structComponents[i], i);
                if (this.structComponents[i] is IComponentInitializable initializable) {

                    for (int e = 0; e < group.Length; ++e) {
                        initializable.Initialize(in group.slice.GetRefRead(e));
                    }

                }
                if (this.structComponents[i] is IComponentStatic) continue;

                #if !SHARED_COMPONENTS_DISABLED
                var isShared = (this.structComponents[i] is IComponentShared);
                if (isShared == true) { // is shared?

                    throw new System.NotImplementedException("Set Shared for EntityGroups is not implemented");

                } else
                #endif
                {

                    group.Set(this.structComponents[i], dataIndex);

                }

            }

            // Update filters
            {
                group.UpdateFilters();
            }

        }

    }

}
#endif