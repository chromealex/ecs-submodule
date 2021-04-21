using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ME.ECSEditor {

    public static class EditorUtilities {

        private static readonly string[] searchPaths = new[] {
            "Packages/com.me.ecs/",
            "Assets/ECS/",
            "Assets/ME.ECS/",
            "Assets/ME.ECS-submodule/",
            "Assets/ECS-submodule/",
            "Assets/ecs-submodule/",
        };
        
        public static int GetPropertyChildCount(UnityEditor.SerializedProperty property) {
        
            var prop = property.Copy();
            var enter = true;
            var count = 0;
            while (prop.NextVisible(enter) == true) {

                ++count;
                enter = false;
                
            }
            
            return count;
            
        }

        public static float GetPropertyHeight(UnityEditor.SerializedProperty property, bool includeChildren, GUIContent label) {
            
            var prop = property.Copy();
            if (EditorUtilities.GetPropertyChildCount(prop) == 1 && prop.NextVisible(true) == true) {

                prop.isExpanded = true;
                return UnityEditor.EditorGUI.GetPropertyHeight(prop, label, includeChildren);

            }
            
            return UnityEditor.EditorGUI.GetPropertyHeight(property, label, includeChildren);

        }
        
        public static T Load<T>(string path, bool isRequired = false) where T : Object {

            foreach (var searchPath in EditorUtilities.searchPaths) {
            
                var data = UnityEditor.Experimental.EditorResources.Load<T>($"{searchPath}{path}", false);
                if (data != null) return data;

            }
            
            if (isRequired == true) {

                throw new System.IO.FileNotFoundException($"Could not find editor resource {path}");

            }
            
            return null;

        }

    }
    
    public static class SerializedPropertyExtensions {
        
        public static System.Type GetArrayOrListElementType(this System.Type listType) {
            if (listType.IsArray)
                return listType.GetElementType();
            return listType.IsGenericType && (object) listType.GetGenericTypeDefinition() == (object) typeof (List<>) ? listType.GetGenericArguments()[0] : (System.Type) null;
        }
        
        public static bool IsArrayOrList(this System.Type listType) => listType.IsArray || listType.IsGenericType && (object) listType.GetGenericTypeDefinition() == (object) typeof (List<>);

        public static T GetSerializedValue<T>(this UnityEditor.SerializedProperty property) {

            object @object = property.serializedObject.targetObject;
            string[] propertyNames = property.propertyPath.Split('.');
 
            // Clear the property path from "Array" and "data[i]".
            if (propertyNames.Length >= 3 && propertyNames[propertyNames.Length - 2] == "Array")
                propertyNames = propertyNames.Take(propertyNames.Length - 2).ToArray();
 
            // Get the last object of the property path.
            foreach (string path in propertyNames) {
                
                @object = @object.GetType()
                                 .GetField(path, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                 .GetValue(@object);
                
            }
 
            if (@object.GetType().GetInterfaces().Contains(typeof(IList<T>))) {
            
                int propertyIndex = int.Parse(property.propertyPath[property.propertyPath.Length - 2].ToString());
                return ((IList<T>) @object)[propertyIndex];
                
            } else {
                
                return (T) @object;
                
            }
            
        }
    }
    
}