using ME.ECS.DataConfigs;
using System.Linq;
using UnityEngine;

namespace ME.ECS {

    public class ConfigIdReadFromAttribute : PropertyAttribute {

        public string fieldName;
        
        public ConfigIdReadFromAttribute(string fieldName) {

            this.fieldName = fieldName;

        }

    }
    
    [System.Serializable]
    public struct ConfigId<T> where T : ME.ECS.DataConfigs.DataConfig {

        [SerializeField]
        [ME.ECS.Serializer.SerializeField]
        internal int id;

        public ConfigId(T config) {
            
            var module = Worlds.current.GetFeature<ME.ECS.DataConfigs.DataConfigIndexerFeature>();
			if (module == null) {
				Worlds.current.AddFeature(DataConfigComponentsInitializer.GetFeature());
				module = Worlds.current.GetFeature<ME.ECS.DataConfigs.DataConfigIndexerFeature>();
			}
            this = module.RegisterConfig(config);

        }

        public ConfigId(int id) {

            this.id = id;

        }

        public T GetData() {

            var module = Worlds.current.GetFeature<ME.ECS.DataConfigs.DataConfigIndexerFeature>();
			if (module == null) {
				Worlds.current.AddFeature(DataConfigComponentsInitializer.GetFeature());
				module = Worlds.current.GetFeature<ME.ECS.DataConfigs.DataConfigIndexerFeature>();
			}
            return module.GetData(this);

        }
        
        public static implicit operator ConfigId<T>(T config) {
            return new ConfigId<T>(config);
        }

        public static implicit operator T(ConfigId<T> config) {
            return config.GetData();
        }

    }
    
    /// <summary>
    /// Used in data configs
    /// If component has this interface - data will be initialized in DataConfig::Apply method
    /// </summary>
    public interface IComponentInitializable {

        void Initialize(in Entity entity);

    }

    /// <summary>
    /// Used in data configs
    /// If component has this interface - data will be deinitialized in DataConfig::Apply method on removing component
    /// </summary>
    public interface IComponentDeinitializable {

        void Deinitialize(in Entity entity);

    }

}

namespace ME.ECS.DataConfigs {

    [CreateAssetMenu(menuName = "ME.ECS/Data Config")]
    public partial class DataConfig : ConfigBase {

        public uint sharedGroupId;
        [SerializeReference]
        public IComponentBase[] structComponents = new IComponentBase[0];
        [SerializeReference]
        public IComponentBase[] removeStructComponents = new IComponentBase[0];

        public string[] templates;
        
        [System.NonSerialized]
        private int[] structComponentsDataTypeIds = new int[0];
        [System.NonSerialized]
        private int[] removeStructComponentsDataTypeIds = new int[0];
        [System.NonSerialized]
        private bool isPrewarmed;
        private System.Collections.Generic.Dictionary<int, int> typeIndexToArrayIndex = new System.Collections.Generic.Dictionary<int, int>();

        #region Initialization
        public static void InitTypeId() {
            
            #if !SHARED_COMPONENTS_DISABLED
            WorldUtilities.InitComponentTypeId<SharedData>(isCopyable: true);
            #endif
            
        }

        public static void Init(State state) {
            
            #if !SHARED_COMPONENTS_DISABLED
            state.structComponents.ValidateUnmanaged<SharedData>(ref state.allocator, false);
            #endif

        }

        public static void Init(in Entity entity) {
            
            #if !SHARED_COMPONENTS_DISABLED
            entity.ValidateDataUnmanaged<SharedData>(false);
            #endif

        }
        #endregion

        #region Public API
        public static void AddSource(in Entity entity, DataConfig config) {
            
            #if !STATIC_API_DISABLED
            if (entity.Has<SourceConfig>() == true) {
                
                // We already has SourceConfig onto this entity,
                // so need to add config's list
                ref var configs = ref entity.Get<SourceConfigs>();
                ref var allocator = ref Worlds.current.GetState().allocator;
                if (configs.configs.isCreated == false) configs.configs = new ME.ECS.Collections.MemoryAllocator.List<ConfigId<DataConfig>>(ref allocator, 1);
                configs.configs.Add(ref allocator, config);

            } else
            #endif
            {

                entity.Set(new SourceConfig() {
                    config = config,
                });

            }
            
        }

        public override void Apply(in Entity entity, bool overrideIfExist = true) {

            //this.Reset();
            this.Prewarm();

            AddSource(in entity, this);
            
            var world = Worlds.currentWorld;
            for (int i = 0; i < this.removeStructComponents.Length; ++i) {

                var dataIndex = this.GetComponentDataIndexByTypeRemoveWithCache(this.removeStructComponents[i], i);
                if (world.HasDataBit(in entity, dataIndex) == true) {

                    var data = world.ReadData(in entity, dataIndex);
                    if (data is IComponentDeinitializable deinitializable) {

                        deinitializable.Deinitialize(in entity);

                    }

                    world.RemoveData(in entity, dataIndex);
                    
                }

            }

            for (int i = 0; i < this.structComponents.Length; ++i) {

                var dataIndex = this.GetComponentDataIndexByTypeWithCache(this.structComponents[i], i);
                if (this.structComponents[i] is IComponentInitializable initializable) {
                    
                    initializable.Initialize(in entity);
                    
                }

                if (this.structComponents[i] is IComponentStatic) continue;
                
                #if !SHARED_COMPONENTS_DISABLED
                var isShared = (this.structComponents[i] is IComponentShared);
                if (isShared == true) { // is shared?

                    if (overrideIfExist == true || world.HasSharedDataBit(in entity, dataIndex, this.sharedGroupId) == false) {

                        ref var sharedData = ref entity.Get<SharedData>();
                        ref var allocator = ref world.GetState().allocator;
                        if (sharedData.archetypeToId.isCreated == false) sharedData.archetypeToId = new ME.ECS.Collections.MemoryAllocator.Dictionary<int, uint>(ref allocator, 10);

                        world.SetSharedData(in entity, in this.structComponents[i], dataIndex, this.sharedGroupId);
                        sharedData.archetypeToId.Add(ref allocator, dataIndex, this.sharedGroupId);

                    }

                } else
                #endif
                {

                    if (overrideIfExist == true || world.HasDataBit(in entity, dataIndex) == false) {

                        world.SetData(in entity, in this.structComponents[i], dataIndex);

                    }
                
                }

            }
            
            // Update filters
            {
                ComponentsInitializerWorld.Init(in entity);
                world.UpdateFilters(in entity);
            }

        }

        public void Reset() {

	        this.isPrewarmed = false;
	        
	        System.Array.Resize(ref this.removeStructComponentsDataTypeIds, this.removeStructComponents.Length);
	        for (int i = 0; i < this.removeStructComponents.Length; ++i) {

		        this.removeStructComponentsDataTypeIds[i] = -1;

	        }
	        
	        System.Array.Resize(ref this.structComponentsDataTypeIds, this.structComponents.Length);
	        for (int i = 0; i < this.structComponents.Length; ++i) {

		        this.structComponentsDataTypeIds[i] = -1;

	        }
            
            this.typeIndexToArrayIndex.Clear();
	        
        }
        
        public override void Prewarm(bool forced = false) {

            if (forced == false) {

                if (this.isPrewarmed == true) return;
                #if UNITY_EDITOR
                try {
                    if (Application.isPlaying == false) return;
                } catch (System.Exception) { }
                #endif

            }

            System.Array.Resize(ref this.removeStructComponentsDataTypeIds, this.removeStructComponents.Length);
	        for (int i = 0; i < this.removeStructComponents.Length; ++i) {

		        this.removeStructComponentsDataTypeIds[i] = this.GetComponentDataIndexByType(this.removeStructComponents[i]);

	        }

            this.typeIndexToArrayIndex.Clear();
	        System.Array.Resize(ref this.structComponentsDataTypeIds, this.structComponents.Length);
	        for (int i = 0; i < this.structComponents.Length; ++i) {

		        this.structComponentsDataTypeIds[i] = this.GetComponentDataIndexByType(this.structComponents[i]);
                this.typeIndexToArrayIndex.Add(this.structComponentsDataTypeIds[i], i);

	        }

	        this.isPrewarmed = true;

        }

        public bool TryRead<T>(out T component) where T : struct, IComponentBase {
            
            this.Prewarm();
            component = default;
            var type = typeof(T);
            var index = this.GetComponentDataIndexByType(type);
            if (this.typeIndexToArrayIndex.TryGetValue(index, out var idx) == true) {

                var c = this.structComponents[idx];
                if (c != null) {
                    component = (T)c;
                    return true;
                }
                return false;

            }

            for (int i = 0; i < this.structComponents.Length; ++i) {

                var c = this.structComponents[i];
                if (c != null && c.GetType() == type) {

                    component = (T)c;
                    return true;

                }
		        
            }
	        
            return false;
            
        }
        
        public bool Has<T>() where T : struct, IComponentBase {
	        
            this.Prewarm();
            var type = typeof(T);
            var index = this.GetComponentDataIndexByType(type);
            if (this.typeIndexToArrayIndex.TryGetValue(index, out var idx) == true) {

                return this.structComponents[idx] != null;

            }

	        for (int i = 0; i < this.structComponents.Length; ++i) {

		        if (this.structComponents[i] != null && this.structComponents[i].GetType() == type) {

			        return true;

		        }
		        
	        }
	        
            return false;

        }

        public T Get<T>() where T : struct, IComponentBase {

            return this.Read<T>();

        }

        public T Read<T>() where T : struct, IComponentBase {

            this.Prewarm();
            var type = typeof(T);
            var index = this.GetComponentDataIndexByType(type);
            if (this.typeIndexToArrayIndex.TryGetValue(index, out var idx) == true) {

                return (T)this.structComponents[idx];

            }
            
            for (int i = 0; i < this.structComponents.Length; ++i) {

                if (this.structComponents[i] != null && this.structComponents[i].GetType() == type) {

                    return (T)this.structComponents[i];

                }
		        
            }

            return default;

        }
        #endregion

        #region Internal API
        internal int GetComponentDataIndexByTypeRemoveWithCache(IComponentBase component, int idx) {

            if (this.removeStructComponentsDataTypeIds[idx] >= 0) return this.removeStructComponentsDataTypeIds[idx];
	        
            if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {

                this.removeStructComponentsDataTypeIds[idx] = index;
                return index;

            }

            #if UNITY_EDITOR
            if (Application.isPlaying == false) return -1;
            throw new System.Exception($"ComponentTypesRegistry has no type {component.GetType()} for DataConfig {this}.");
            #else
	        return -1;
            #endif

        }

        internal int GetComponentDataIndexByTypeWithCache(IComponentBase component, int idx) {

	        if (this.structComponentsDataTypeIds[idx] >= 0) return this.structComponentsDataTypeIds[idx];
	        
	        if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {

		        this.structComponentsDataTypeIds[idx] = index;
		        return index;

	        }

	        #if UNITY_EDITOR
            if (Application.isPlaying == false) return -1;
	        throw new System.Exception($"ComponentTypesRegistry has no type {component.GetType()} for DataConfig {this}.");
	        #else
	        return -1;
	        #endif

        }

        internal int GetComponentDataIndexByType(IComponentBase component) {

            if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {

                return index;

            }

            #if UNITY_EDITOR
            if (Application.isPlaying == false) return -1;
            throw new System.Exception($"ComponentTypesRegistry has no type {component.GetType()} for DataConfig {this}.");
            #else
	        return -1;
            #endif

        }

        internal int GetComponentDataIndexByType(System.Type type) {

            if (ComponentTypesRegistry.allTypeId.TryGetValue(type, out var index) == true) {

                return index;

            }

            #if UNITY_EDITOR
            if (Application.isPlaying == false) return -1;
            throw new System.Exception($"ComponentTypesRegistry has no type {type} for DataConfig {this}.");
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
        
        public void AddByType(IComponentBase component) {

            this.AddTo(ref this.structComponents, component);

        }

        public void RemoveByType<T>() where T: IComponentBase {

            this.RemoveFrom(ref this.structComponents, typeof(T));

        }

        public void SetByType(IComponentBase component) {

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

        protected void AddTo<T>(ref T[] arr, T component) {

            var found = false;
            var nullIdx = -1;
            for (int i = 0; i < arr.Length; ++i) {

                var comp = arr[i];
                if (comp == null) nullIdx = i;
                if (comp != null && comp.GetType() == component.GetType()) {

                    found = true;
                    break;

                }

            }

            if (found == false) {

                if (nullIdx >= 0) {

                    arr[nullIdx] = component;
                    return;

                }
                
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

        public bool UpdateValue(IComponentBase component) {

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

        public void Save(bool dirtyOnly = false) {
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            if (dirtyOnly == false) UnityEditor.AssetDatabase.ForceReserializeAssets(new [] { UnityEditor.AssetDatabase.GetAssetPath(this) }, UnityEditor.ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
            #endif

        }
        #endregion

    }

    public struct DataConfigSlice {

        public DataConfig[] configs;
        public System.Type[] structComponentsTypes;

        public void Set(IComponentBase[] components) {

            for (int i = 0; i < this.configs.Length; ++i) {

                this.configs[i].SetByType(components[i]);

            }
            
        }
        
        public static DataConfigSlice Distinct(DataConfig[] configs) {
            
            var slice = new DataConfigSlice();
            slice.configs = configs;
            
            {

                var listIdx = new System.Collections.Generic.Dictionary<System.Type, int>();
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

                var list = new System.Collections.Generic.List<System.Type>();
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
