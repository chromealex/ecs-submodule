
using System.Linq;

namespace ME.ECSEditor {

    public class ComponentIndexGeneratorData : UnityEngine.ScriptableObject {

        [System.Serializable]
        public class State {

            public System.Collections.Generic.List<string> asmTypesStructs = new System.Collections.Generic.List<string>();
            public System.Collections.Generic.List<string> asmTypesRefs = new System.Collections.Generic.List<string>();
            public System.Collections.Generic.List<string> typesStructs = new System.Collections.Generic.List<string>();
            public System.Collections.Generic.List<string> typesRefs = new System.Collections.Generic.List<string>();

            public void CopyFrom(State other) {

                this.asmTypesStructs = other.asmTypesStructs.ToArray().ToList();
                this.asmTypesRefs = other.asmTypesRefs.ToArray().ToList();
                this.typesStructs = other.typesStructs.ToArray().ToList();
                this.typesRefs = other.typesRefs.ToArray().ToList();

            }
            
        }

        public State current = new State();
        
        public void ResetCurrent() {

        }

        public void ApplyCurrent() {

            for (var i = 0; i < this.current.typesStructs.Count; ++i) {

                var asmType = this.current.asmTypesStructs[i];
                var entityType = this.current.typesStructs[i];
                var hasType = System.Type.GetType(entityType + ", " + asmType) != null;
                if (hasType == false) {
                    
                    this.current.typesStructs.RemoveAt(i);
                    this.current.asmTypesStructs.RemoveAt(i);
                    --i;

                }

            }
            
            for (var i = 0; i < this.current.typesRefs.Count; ++i) {

                var asmType = this.current.asmTypesRefs[i];
                var entityType = this.current.typesRefs[i];
                var hasType = System.Type.GetType(entityType + ", " + asmType) != null;
                if (hasType == false) {
                    
                    this.current.typesRefs.RemoveAt(i);
                    this.current.asmTypesRefs.RemoveAt(i);
                    --i;

                }

            }
            
        }
        
        public void SetStruct(System.Type type) {
            
            var name = type.FullName;
            var asmName = type.Assembly.GetName().Name;
            var idx = this.current.typesStructs.IndexOf(name);
            if (idx < 0 || this.current.asmTypesStructs[idx] != asmName) {
                
                this.current.typesStructs.Add(name);
                this.current.asmTypesStructs.Add(asmName);
                UnityEditor.EditorUtility.SetDirty(this);
                
            }
            
        }

        public void SetRef(System.Type type) {
            
            var name = type.FullName;
            var asmName = type.Assembly.GetName().Name;
            var idx = this.current.typesRefs.IndexOf(name);
            if (idx < 0 || this.current.asmTypesRefs[idx] != asmName) {
                
                this.current.typesRefs.Add(name);
                this.current.asmTypesRefs.Add(asmName);
                UnityEditor.EditorUtility.SetDirty(this);
                
            }
            
        }

        public static ComponentIndexGeneratorData Generate(string generatorPath) {

            var path = generatorPath + "/ComponentIndexGeneratorData.asset";

            if (System.IO.File.Exists(path) == false) {

                var instance = ComponentIndexGeneratorData.CreateInstance<ComponentIndexGeneratorData>();
                UnityEditor.AssetDatabase.CreateAsset(instance, path);
                UnityEditor.AssetDatabase.ImportAsset(path);

            }
            
            return UnityEditor.AssetDatabase.LoadAssetAtPath<ComponentIndexGeneratorData>(path);

        }

    }

}