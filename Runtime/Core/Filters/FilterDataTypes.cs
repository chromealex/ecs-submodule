using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS {

    [System.Serializable]
    public struct FilterDataTypesOptional {

        [System.Serializable]
        public struct Optional {

            [SerializeReference]
            public IComponentBase data;
            public bool optional;

        }
        
        public Optional[] with;
        [SerializeReference]
        public IComponentBase[] without;

        public bool Has(in Entity entity) {
            
            foreach (var comp in this.with) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.data.GetType(), out var dataIndex) == true) {
                    if (Worlds.current.HasDataBit(in entity, dataIndex) == false) return false;
                }
            }
            
            foreach (var comp in this.without) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.GetType(), out var dataIndex) == true) {
                    if (Worlds.current.HasDataBit(in entity, dataIndex) == true) return false;
                }
            }

            return true;

        }

        public void Apply(in Entity entity) {
            
            foreach (var comp in this.with) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.data.GetType(), out var dataIndex) == true) {
                    Worlds.current.SetData(in entity, comp.data, dataIndex);
                }
            }
            
            foreach (var comp in this.without) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.GetType(), out var dataIndex) == true) {
                    Worlds.current.RemoveData(in entity, dataIndex);
                }
            }

        }
        
    }

    [System.Serializable]
    public struct FilterDataTypes {

        [SerializeReference]
        public IComponentBase[] with;
        [SerializeReference]
        public IComponentBase[] without;

        public bool Has(in Entity entity) {
            
            foreach (var comp in this.with) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.GetType(), out var dataIndex) == true) {
                    if (Worlds.current.HasDataBit(in entity, dataIndex) == false) return false;
                }
            }
            
            foreach (var comp in this.without) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.GetType(), out var dataIndex) == true) {
                    if (Worlds.current.HasDataBit(in entity, dataIndex) == true) return false;
                }
            }

            return true;

        }

        public void Apply(in Entity entity) {
            
            foreach (var comp in this.with) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.GetType(), out var dataIndex) == true) {
                    Worlds.current.SetData(in entity, comp, dataIndex);
                }
            }
            
            foreach (var comp in this.without) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.GetType(), out var dataIndex) == true) {
                    Worlds.current.RemoveData(in entity, dataIndex);
                }
            }

        }
        
    }

    [System.Serializable]
    public struct ComponentData {

        [SerializeReference]
        public IComponentBase component;

    }

    public class ComponentDataTypeAttribute : PropertyAttribute {

        public enum Type {

            WithData,
            NoData,

        }

        public Type type;
        
        public ComponentDataTypeAttribute(Type type) {
            
            this.type = type;
            
        }
        
    }

}
