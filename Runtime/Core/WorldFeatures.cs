namespace ME.ECS {

    [System.Serializable]
    public sealed class FeaturesListCategories {

        public System.Collections.Generic.List<FeaturesListCategory> items = new System.Collections.Generic.List<FeaturesListCategory>();

        public void Initialize(World world) {

            for (int i = 0; i < this.items.Count; ++i) {
                
                this.items[i].features.Initialize(world);
                
            }
            
            for (int i = 0; i < this.items.Count; ++i) {
                
                this.items[i].features.InitializePost(world);
                
            }
            
        }

        public void DeInitialize(World world) {

            for (int i = 0; i < this.items.Count; ++i) {
                
                this.items[i].features.DeInitialize(world);
                
            }

        }

    }

    [System.Serializable]
    public sealed class FeaturesListCategory {

        public string folderCaption;
        public FeaturesList features = new FeaturesList();

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public sealed class FeaturesList {

        public interface IFeatureData {

            FeatureBase featureInstance { get; set; }

            FeatureBase GetSource();
            bool IsEnabled();
            IFeatureData[] GetSubFeatures();

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Serializable]
        public sealed class FeatureData : IFeatureData {

            public bool enabled;
            public FeatureBase feature;
            public FeatureBase featureInstance { get; set; }
            [UnityEngine.SerializeReference]
            public IFeatureData[] innerFeatures;

            public bool IsEnabled() => this.enabled;
            public FeatureBase GetSource() => this.feature;
            public IFeatureData[] GetSubFeatures() => this.innerFeatures;

        }

        public System.Collections.Generic.List<FeatureData> features = new System.Collections.Generic.List<FeatureData>();

        internal void Initialize(World world) {

            this.InitializePre(world, this.features);
            
        }

        private void InitializePre(World world, System.Collections.IList features) {

            for (int i = 0; i < features.Count; ++i) {

                var item = (FeatureData)features[i];
                if (item.IsEnabled() == true) {

                    var instance = (world.settings.createInstanceForFeatures == true ? UnityEngine.Object.Instantiate(item.GetSource()) : item.GetSource());
                    if (world.settings.createInstanceForFeatures == true) instance.name = item.GetSource().name;
                    item.featureInstance = instance;
                    world.AddFeature(instance, doConstruct: false);

                    if (item.GetSubFeatures() != null) {

                        this.InitializePre(world, item.GetSubFeatures());

                    }
                    
                }

            }
            
        }

        public void InitializePost(World world) {
            
            this.InitializePost(world, this.features);
            
        }
        
        public void InitializePost(World world, System.Collections.IList features) {

            for (int i = 0; i < features.Count; ++i) {
                
                var item = (FeatureData)features[i];
                if (item.IsEnabled() == true) {
                    
                    item.featureInstance.DoConstruct();
                    
                    if (item.GetSubFeatures() != null) {

                        this.InitializePost(world, item.GetSubFeatures());

                    }
                    
                }
                
            }

        }

        internal void DeInitialize(World world) {
            
            this.DeInitialize(world, this.features);
            
        }

        internal void DeInitialize(World world, System.Collections.IList features) {
            
            for (int i = 0; i < features.Count; ++i) {
                
                var item = (FeatureData)features[i];
                if (item.IsEnabled() == true) {
                    
                    world.RemoveFeature(item.featureInstance);
                    if (world.settings.createInstanceForFeatures == true) UnityEngine.Object.DestroyImmediate(item.featureInstance);
                    item.featureInstance = null;

                    if (item.GetSubFeatures() != null) {

                        this.DeInitialize(world, item.GetSubFeatures());

                    }

                }
                
            }

        }

    }

    public abstract class FeatureBase : UnityEngine.ScriptableObject, IFeatureBase {

        [UnityEngine.TextAreaAttribute]
        public string editorComment;

        public World world { get; set; }
        protected SystemGroup systemGroup;

        internal void DoConstruct() {
            
            this.systemGroup = new SystemGroup(this.world, this.GetType().Name);
            Filter.RegisterInject(this.InjectFilter);
            this.OnConstruct();
            Filter.UnregisterInject(this.InjectFilter);
            
        }

        internal void DoDeconstruct() {

            this.world = null;
            this.systemGroup = default;
            this.OnDeconstruct();
            
        }

        protected virtual void InjectFilter(ref FilterBuilder builder) {}
        
        protected abstract void OnConstruct();
        protected abstract void OnDeconstruct();

        protected bool AddSystem<TSystem>() where TSystem : class, ISystemBase, new() {

            if (this.systemGroup.HasSystem<TSystem>() == false) {

                return this.systemGroup.AddSystem<TSystem>();
                
            }

            return false;

        }

        protected bool AddModule<TModule>() where TModule : class, IModuleBase, new() {

            if (this.world.HasModule<TModule>() == false) {
                
                return this.world.AddModule<TModule>();
                
            }

            return false;

        }

    }

    public abstract class Feature : FeatureBase, IFeature {

    }

}