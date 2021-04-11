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