
using System.Linq;

namespace ME.ECSEditor {

    public class ComponentIndexGeneratorData : UnityEngine.ScriptableObject {

        [System.Serializable]
        public class State {

            public System.Collections.Generic.List<string> asmTypesStructs = new System.Collections.Generic.List<string>();
            public System.Collections.Generic.List<string> typesStructs = new System.Collections.Generic.List<string>();

            public void CopyFrom(State other) {

                this.asmTypesStructs = other.asmTypesStructs.ToArray().ToList();
                this.typesStructs = other.typesStructs.ToArray().ToList();

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

            this.SetDirty();

        }
        
        public void SetStruct(System.Type type) {
            
            var name = type.FullName;
            var asmName = type.Assembly.GetName().Name;
            var idx = this.current.typesStructs.IndexOf(name);
            if (idx < 0 || this.current.asmTypesStructs[idx] != asmName) {
                
                this.current.typesStructs.Add(name);
                this.current.asmTypesStructs.Add(asmName);
                
                this.SetDirty();

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

        new private void SetDirty() {
	        
	        #if UNITY_EDITOR
	        UnityEditor.EditorUtility.SetDirty(this);
	        #endif
	        
        }

    }

}
