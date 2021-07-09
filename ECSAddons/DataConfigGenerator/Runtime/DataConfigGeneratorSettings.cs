using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.DataConfigGenerator {

    public interface IGeneratorBehaviour {
        
        IDataConfigGenerator generator { get; set; }
        
        ME.ECS.DataConfigs.DataConfig CreateConfigInstance(ConfigInfo configInfo);
        void DeleteConfigAsset(string path);

        object CreateComponentInstance(ME.ECS.DataConfigs.DataConfig config, ComponentInfo componentInfo, bool allValuesAreNull);
        void ParseComponentField(ref bool allValuesAreNull, object instance, System.Type componentType, ComponentInfo componentInfo, string fieldName, string data);

    }

    public interface IDataConfigGenerator {

        void Log(string str);
        void LogWarning(string str);
        void LogError(string str);

        System.Type GetComponentType(ComponentInfo info);
        bool TryToConvert(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result);

    }

    public struct ComponentInfo : System.IEquatable<ComponentInfo> {

        public string name;
        public int offset;
        public int length;
        public List<string> fields;

        public bool Equals(ComponentInfo other) {

            return other.name == this.name;

        }

    }

    public struct ConfigInfo : System.IEquatable<ConfigInfo> {

        public string comment;
        public string name;
        public string[] templates;
        public Dictionary<ComponentInfo, List<string>> data;

        public bool Equals(ConfigInfo other) {

            return other.name == this.name;

        }
            
    }

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
            public GeneratorBehaviour behaviour;

        }

        public Element[] paths;

    }

    public abstract class GeneratorBehaviour : ScriptableObject, IGeneratorBehaviour {

        public IDataConfigGenerator generator { get; set; }
        public abstract ME.ECS.DataConfigs.DataConfig CreateConfigInstance(ConfigInfo configInfo);
        public abstract void DeleteConfigAsset(string path);
        public abstract object CreateComponentInstance(ME.ECS.DataConfigs.DataConfig config, ComponentInfo componentInfo, bool allValuesAreNull);
        public abstract void ParseComponentField(ref bool allValuesAreNull, object instance, System.Type componentType, ComponentInfo componentInfo, string fieldName, string data);

    }

}