using System.Linq;
using System.Reflection;

namespace ME.ECS.BlackBox {

    [System.Serializable]
    public struct FieldData {

        public bool isArray;
        [UnityEngine.SerializeReference]
        public System.Object value;
        [UnityEngine.SerializeReference]
        public System.Object[] valueArr;

        public System.Type GetTypeOfValue() {

            if (this.isArray == true) {
                
                if (this.valueArr == null) return null;
                return this.valueArr.GetType().GetElementType();
                
            }

            if (this.value == null) return null;
            return this.value.GetType();

        }

    }

    [System.Serializable]
    public struct FieldDataContainer {

        public string[] captions;
        public FieldData[] data;
        public BoxVariable[] boxVars;
        public bool isInput;

        public void Process(IStructComponentBase component) {

            this.Validate(component);
            for (int i = 0; i < this.boxVars.Length; ++i) {

                if (this.boxVars[i] != null) this.boxVars[i].Save(this, i);

            }
            
        }

        public void Apply(ref IStructComponentBase component) {
            
            var fields = component.GetType().GetCachedFields();
            for (int i = 0; i < this.boxVars.Length; ++i) {

                if (this.boxVars[i] != null) {
                    
                    var boxVarValue = this.boxVars[i].Load();
                    fields[i].SetValue(component, boxVarValue);
                    
                }

            }
            
        }
        
        public void Validate(IStructComponentBase component) {
            
            var fields = component.GetType().GetCachedFields();
            if (this.data == null || this.data.Length != fields.Length) this.data = new FieldData[fields.Length];
            if (this.captions == null || this.captions.Length != this.data.Length) this.captions = new string[this.data.Length];
            if (this.boxVars == null) this.boxVars = new BoxVariable[this.data.Length];
            if (this.boxVars.Length != this.data.Length) {
                
                System.Array.Resize(ref this.boxVars, this.data.Length);
                
            }
            
            for (int i = 0; i < this.data.Length; ++i) {

                this.captions[i] = fields[i].Name;
                var val = fields[i].GetValue(component);
                System.Type type = null;
                if (val == null) {
                    
                    type = fields[i].FieldType;
                    
                } else {
                    
                    type = val.GetType();
                    
                }
                
                if (type.IsArray == true) {
                        
                    this.data[i].value = null;
                    this.data[i].valueArr = ((System.Collections.IList)val).Cast<object>().ToArray();
                    this.data[i].isArray = true;
                        
                } else {

                    this.data[i].value = val;
                    this.data[i].valueArr = null;
                    this.data[i].isArray = false;

                }
                    
            }
            
        }

    }

}