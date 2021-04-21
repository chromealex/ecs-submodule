using System.Collections;
using System.Collections.Generic;
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

}