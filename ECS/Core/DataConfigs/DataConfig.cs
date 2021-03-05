using System.Collections.Generic;
using System.Linq;
using ME.ECS.Extensions;
using UnityEngine;

namespace ME.ECS.DataConfigs {

    [CreateAssetMenu(menuName = "ME.ECS/Data Config")]
    public class DataConfig : ScriptableObject {

        [SerializeReference]
        public IStructComponent[] structComponents = new IStructComponent[0];
        //public int[] structComponentsDataTypeIds = new int[0];
        
        [SerializeReference]
        public IStructComponent[] removeStructComponents = new IStructComponent[0];
        //public int[] removeStructComponentsDataTypeIds = new int[0];

        public string[] templates;
        
        [System.NonSerialized]
        private int[] structComponentsDataTypeIds = new int[0];
        [System.NonSerialized]
        private int[] removeStructComponentsDataTypeIds = new int[0];
        [System.NonSerialized]
        private bool isPrewarmed;

        private void Reset() {

	        this.isPrewarmed = false;
	        
	        System.Array.Resize(ref this.removeStructComponentsDataTypeIds, this.removeStructComponents.Length);
	        for (int i = 0; i < this.removeStructComponents.Length; ++i) {

		        this.removeStructComponentsDataTypeIds[i] = -1;

	        }
	        
	        System.Array.Resize(ref this.structComponentsDataTypeIds, this.structComponents.Length);
	        for (int i = 0; i < this.structComponents.Length; ++i) {

		        this.structComponentsDataTypeIds[i] = -1;

	        }
	        
        }
        
        public void Prewarm() {

	        if (this.isPrewarmed == true) return;
	        
	        System.Array.Resize(ref this.removeStructComponentsDataTypeIds, this.removeStructComponents.Length);
	        for (int i = 0; i < this.removeStructComponents.Length; ++i) {

		        this.removeStructComponentsDataTypeIds[i] = this.GetComponentDataIndexByType(this.removeStructComponents[i]);

	        }

	        System.Array.Resize(ref this.structComponentsDataTypeIds, this.structComponents.Length);
	        for (int i = 0; i < this.structComponents.Length; ++i) {

		        this.structComponentsDataTypeIds[i] = this.GetComponentDataIndexByType(this.structComponents[i]);

	        }

	        this.isPrewarmed = true;

        }
        
        public void Apply(in Entity entity) {

	        //this.Reset();
	        this.Prewarm();

            var world = Worlds.currentWorld;
            for (int i = 0; i < this.removeStructComponents.Length; ++i) {

                world.RemoveData(in entity, this.GetComponentDataIndexByTypeWithCache(this.removeStructComponents[i], i), -1);

            }

            for (int i = 0; i < this.structComponents.Length; ++i) {

                world.SetData(in entity, in this.structComponents[i], this.GetComponentDataIndexByTypeWithCache(this.structComponents[i], i), -1);

            }
            
            // Update filters
            {
                ComponentsInitializerWorld.Init(in entity);
                world.UpdateFilters(in entity);
            }

        }

        public int GetComponentDataIndexByTypeWithCache(IStructComponent component, int idx) {

	        if (this.structComponentsDataTypeIds[idx] >= 0) return this.structComponentsDataTypeIds[idx];
	        
	        if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {

		        this.structComponentsDataTypeIds[idx] = index;
		        return index;

	        }

	        #if UNITY_EDITOR
	        throw new System.Exception($"ComponentTypesRegistry has no type {component.GetType()} for DataConfig {this}.");
	        #else
	        return -1;
	        #endif

        }

        public int GetComponentDataIndexByType(IStructComponent component) {

	        if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {

		        return index;

	        }

	        #if UNITY_EDITOR
	        throw new System.Exception($"ComponentTypesRegistry has no type {component.GetType()} for DataConfig {this}.");
	        #else
	        return -1;
	        #endif

        }
        
        public System.Type[] GetStructComponentTypes() {
            
            var types = new System.Type[this.structComponents.Length];
            for (int i = 0; i < this.structComponents.Length; ++i) {

                types[i] = (this.structComponents[i] != null ? this.structComponents[i].GetType() : null);

            }
            
            return types;

        }
        
        public void SetByType(IStructComponent component) {

	        var type = component.GetType();
	        for (int i = 0; i < this.structComponents.Length; ++i) {

		        if (this.structComponents[i] != null && this.structComponents[i].GetType() == type) {

			        this.structComponents[i] = component;
			        return;

		        }
		        
	        }
	        
        }
        
        public T GetByType<T>(T[] arr, System.Type type) {
            
	        for (int i = 0; i < arr.Length; ++i) {

		        if (arr[i] != null && arr[i].GetType() == type) {

			        return arr[i];

		        }
		        
	        }
	        
            return default;

        }

        public bool Has<T>() where T : struct, IStructComponent {
	        
	        var type = typeof(T);
	        for (int i = 0; i < this.structComponents.Length; ++i) {

		        if (this.structComponents[i] != null && this.structComponents[i].GetType() == type) {

			        return true;

		        }
		        
	        }
	        
            return false;

        }

        public T Get<T>() where T : struct, IStructComponent {

	        var type = typeof(T);
	        for (int i = 0; i < this.structComponents.Length; ++i) {

		        if (this.structComponents[i].GetType() == type) {

			        return (T)this.structComponents[i];

		        }
		        
	        }

            return default;

        }

        public void AddTo<T>(ref T[] arr, T component) {

            var found = false;
            for (int i = 0; i < arr.Length; ++i) {

                var comp = arr[i];
                if (comp.GetType() == component.GetType()) {

                    found = true;
                    break;

                }

            }

            if (found == false) {
                
                System.Array.Resize(ref arr, arr.Length + 1);
                arr[arr.Length - 1] = component;

            }
            
        }
        
        public bool HasByType<T>(T[] arr, object component) {

            return this.HasByType(arr, component.GetType());

        }

        public bool HasByType<T>(T[] arr, System.Type componentType) {

            for (int i = 0; i < arr.Length; ++i) {

                var comp = arr[i];
                if (comp.GetType() == componentType) {

                    return true;

                }

            }

            return false;

        }

        public void RemoveFrom<T>(ref T[] arr, object component) {

            this.RemoveFrom(ref arr, component.GetType());

        }

        public void RemoveFrom<T>(ref T[] arr, System.Type componentType) {

            for (int i = 0; i < arr.Length; ++i) {

                var comp = arr[i];
                if (comp.GetType() == componentType) {

                    var list = arr.ToList();
                    list.RemoveAt(i);
                    arr = list.ToArray();
                    break;

                }

            }

        }

        public bool UpdateValue(IStructComponent component) {

            var componentType = component.GetType();
            if (this.HasByType(this.structComponents, componentType) == false) {
                
                return false;
                
            }

            for (int i = 0; i < this.structComponents.Length; ++i) {

                var comp = this.structComponents[i];
                if (comp.GetType() == componentType) {

                    this.structComponents[i] = component;
                    break;

                }

            }
            
            this.Save();

            return true;

        }

        public bool OnAddToTemplate(DataConfigTemplate template, System.Type componentType) {
            
            if (this.HasByType(this.structComponents, componentType) == true) {
                
                return false;
                
            }

            var data = template.GetByType(template.structComponents, componentType);
            this.AddTo(ref this.structComponents, data);

            this.Save();

            return true;
            
        }

        public bool OnRemoveFromTemplate(DataConfigTemplate template, System.Type componentType) {
            
            if (this.HasByType(this.structComponents, componentType) == false) {
                
                return false;
                
            }

            this.RemoveFrom(ref this.structComponents, componentType);

            this.Save();

            return true;
            
        }

        public bool OnAddToTemplateRemoveList(DataConfigTemplate template, System.Type componentType) {
            
            if (this.HasByType(this.removeStructComponents, componentType) == true) {
                
                return false;
                
            }

            var data = template.GetByType(template.removeStructComponents, componentType);
            this.AddTo(ref this.removeStructComponents, data);

            this.Save();

            return true;
            
        }

        public bool OnRemoveFromTemplateRemoveList(DataConfigTemplate template, System.Type componentType) {
            
            if (this.HasByType(this.removeStructComponents, componentType) == false) {
                
                return false;
                
            }

            this.RemoveFrom(ref this.removeStructComponents, componentType);

            this.Save();

            return true;
            
        }

        public void AddTemplate(DataConfigTemplate template) {

            template.Use(this);

            for (var i = 0; i < template.structComponents.Length; ++i) {

                this.AddTo(ref this.structComponents, template.structComponents[i]);

            }

            for (var i = 0; i < template.removeStructComponents.Length; ++i) {

                this.AddTo(ref this.removeStructComponents, template.removeStructComponents[i]);

            }

            this.Save();

        }

        public void RemoveTemplate(DataConfigTemplate template, System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate> allTemplates) {
            
            template.UnUse(this);

            for (var i = 0; i < template.structComponents.Length; ++i) {

                var hasOther = allTemplates.Any(x => x != template && x.HasByType(x.structComponents, template.structComponents[i]));
                if (hasOther == false) this.RemoveFrom(ref this.structComponents, template.structComponents[i]);

            }

            for (var i = 0; i < template.removeStructComponents.Length; ++i) {

                var hasOther = allTemplates.Any(x => x != template && x.HasByType(x.structComponents, template.structComponents[i]));
                if (hasOther == false) this.RemoveFrom(ref this.removeStructComponents, template.removeStructComponents[i]);

            }

            this.Save();

        }

        public void Save() {
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.ForceReserializeAssets(new [] { UnityEditor.AssetDatabase.GetAssetPath(this) }, UnityEditor.ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);
            #endif
            
        }

    }

    public struct DataConfigSlice {

        public DataConfig[] configs;
        public System.Type[] structComponentsTypes;

        public void Set(IStructComponent[] components) {

            for (int i = 0; i < this.configs.Length; ++i) {

                this.configs[i].SetByType(components[i]);

            }
            
        }
        
        public static DataConfigSlice Distinct(DataConfig[] configs) {
            
            var slice = new DataConfigSlice();
            slice.configs = configs;
            
            {

                var listIdx = new Dictionary<System.Type, int>();
                for (int i = 0; i < configs.Length; ++i) {

                    var config = configs[i];
                    for (int j = 0; j < config.structComponents.Length; ++j) {

                        if (config.structComponents[j] == null) continue;
                        var type = config.structComponents[j].GetType();
                        if (listIdx.TryGetValue(type, out var count) == true) {

                            listIdx[type] = count + 1;

                        } else {

                            listIdx.Add(type, 1);

                        }

                    }

                }

                var list = new List<System.Type>();
                foreach (var kv in listIdx) {

                    if (kv.Value == configs.Length) {

                        list.Add(kv.Key);

                    }

                }

                slice.structComponentsTypes = list.ToArray();

            }
            
            return slice;

        }

    }
    
}
