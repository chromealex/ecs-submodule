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
        public Optional[] without;

        public bool Has(in Entity entity) {
            
            foreach (var comp in this.with) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.data.GetType(), out var dataIndex) == true) {
                    if (Worlds.current.HasDataBit(in entity, dataIndex) == false) return false;
                }
            }
            
            foreach (var comp in this.without) {
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.data.GetType(), out var dataIndex) == true) {
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
                if (ComponentTypesRegistry.allTypeId.TryGetValue(comp.data.GetType(), out var dataIndex) == true) {
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

        public bool Apply(in Entity entity) {
            
            if (ComponentTypesRegistry.allTypeId.TryGetValue(this.component.GetType(), out var dataIndex) == true) {
                Worlds.current.SetData(in entity, this.component, dataIndex);
                return true;
            }

            return false;

        }

    }

    [System.Serializable]
    public struct ComponentData<T> where T : class, IComponentBase {

        [SerializeReference]
        public T component;

        public bool Apply(in Entity entity) {
            
            if (this.component == null) return false;
            if (ComponentTypesRegistry.allTypeId.TryGetValue(this.component.GetType(), out var dataIndex) == true) {
                Worlds.current.SetData(in entity, this.component, dataIndex);
                return true;
            }

            return false;
            
        }

        public bool Remove(in Entity entity) {
            
            if (this.component == null) return false;
            if (ComponentTypesRegistry.allTypeId.TryGetValue(this.component.GetType(), out var dataIndex) == true) {
                Worlds.current.RemoveData(in entity, dataIndex);
                return true;
            }

            return false;
            
        }
        
        public bool TryRead(in Entity entity, out T component) {
            
            component = default;

            if (this.component == null) return false;
            if (ComponentTypesRegistry.allTypeId.TryGetValue(this.component.GetType(), out var dataIndex) == true) {
                component = (T)Worlds.current.ReadData(in entity, dataIndex);
                if (component == null) return false;
                return true;
            }

            return false;
            
        }
        
        

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

    public class FilterDataTypesLabelsAttribute : PropertyAttribute {

        public string include;
        public string exclude;
        
        public FilterDataTypesLabelsAttribute(string include = "Include", string exclude = "Exclude") {

            this.include = include;
            this.exclude = exclude;

        }
        
    }

    public class FilterDataTypesFoldoutAttribute : PropertyAttribute {

        public bool foldout;
        
        public FilterDataTypesFoldoutAttribute(bool foldout) {

            this.foldout = foldout;

        }
        
    }

    public class DescriptionAttribute : PropertyAttribute {

        public string text;

        public DescriptionAttribute(string text) {
            
            this.text = text;
            
        }

    }

}
