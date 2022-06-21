
namespace ME.ECSEditor {

    using ME.ECS;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;
    
    [UnityEditor.CustomPropertyDrawer(typeof(ME.ECS.FilterDataTypes))]
    public class FilterDataTypesEditor : FilterDataTypesOptionalEditor {

        public override ComponentDataTypeAttribute.Type GetDefaultDrawType() {
            
            return ComponentDataTypeAttribute.Type.WithData;
            
        }

        public override string GetSubName() {
            return null;
        }

        internal static bool GetTypeFromManagedReferenceFullTypeName(string managedReferenceFullTypename, out System.Type managedReferenceInstanceType) {
            managedReferenceInstanceType = null;

            var parts = managedReferenceFullTypename.Split(' ');
            if (parts.Length == 2) {
                var assemblyPart = parts[0];
                var nsClassnamePart = parts[1];
                managedReferenceInstanceType = System.Type.GetType($"{nsClassnamePart}, {assemblyPart}");
            }

            return managedReferenceInstanceType != null;
        }

    }

}
