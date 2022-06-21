
using System.Linq;

namespace ME.ECSEditor {

    using ME.ECS;
    using UnityEditor;
    using UnityEngine;

    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.ComponentData))]
    public class ComponentDataEditor : ComponentDataGenericEditor {

        public override System.Type GetGenericType() {
            
            return typeof(IComponentBase);
            
        }

    }

}
