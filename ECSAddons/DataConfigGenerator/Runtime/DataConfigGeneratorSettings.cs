using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.DataConfigGenerator {

    [CreateAssetMenu(menuName = "ME.ECS/Addons/Data Config Generator Settings")]
    public class DataConfigGeneratorSettings : ScriptableObject {

        [System.Serializable]
        public struct Element {

            public Object directory;
            public string caption;
            public int version;
            [TextArea(3, 3)]
            public string path;
            public string visitedFiles;
            
        }

        public Element[] paths;

    }

}