using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS {

    [System.Serializable]
    public struct FilterDataTypes {

        [SerializeReference]
        public IComponentBase[] with;
        [SerializeReference]
        public IComponentBase[] without;

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
