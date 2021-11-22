using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS {

    [System.Serializable]
    public struct FilterDataTypes {

        [SerializeReference]
        public IStructComponentBase[] with;
        [SerializeReference]
        public IStructComponentBase[] without;

    }

    [System.Serializable]
    public struct ComponentData {

        [SerializeReference]
        public IStructComponentBase component;

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
