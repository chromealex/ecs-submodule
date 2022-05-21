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

            for (int i = 0; i < this.items.Count; ++i) {
                
                this.items[i].features.InitializeLate(world);
                
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
        public FeaturesList<FeatureData> features = new FeaturesList<FeatureData>();

    }

    [System.Serializable]
    public sealed class SubFeaturesList {

        public System.Collections.Generic.List<SubFeatureData> features = new System.Collections.Generic.List<SubFeatureData>();

    }
    
    public interface IFeatureData {

        FeatureBase featureInstance { get; set; }

        FeatureBase GetSource();
        bool IsEnabled();
        SubFeaturesList GetSubFeatures();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public abstract class BaseFeatureData : IFeatureData {

        public bool enabled;
        public FeatureBase feature;
        public FeatureBase featureInstance { get; set; }
            
        public bool IsEnabled() => this.enabled;
        public FeatureBase GetSource() => this.feature;

        public virtual SubFeaturesList GetSubFeatures() => null;

    }

    [System.Serializable]
    public class FeatureData : BaseFeatureData {
            
        public SubFeaturesList innerFeatures;

        public override SubFeaturesList GetSubFeatures() => this.innerFeatures;
            
    }

    [System.Serializable]
    public class SubFeatureData : BaseFeatureData {

        public override SubFeaturesList GetSubFeatures() => null;

    }

    public abstract class FeaturesListBase {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public sealed class FeaturesList<T> : FeaturesListBase where T : BaseFeatureData {

        public System.Collections.Generic.List<T> features = new System.Collections.Generic.List<T>();
        
        internal void Initialize(World world) {

            this.InitializePre(world, this.features);
            
        }

        private void InitializePre(World world, System.Collections.Generic.List<T> features) {

            for (int i = 0; i < features.Count; ++i) {

                var item = features[i];
                this.InitializePre(world, item);
                
            }
            
        }
        
        private void InitializePre(World world, BaseFeatureData item) {

            if (item.IsEnabled() == true) {

                var instance = (world.settings.createInstanceForFeatures == true ? UnityEngine.Object.Instantiate(item.GetSource()) : item.GetSource());
                if (world.settings.createInstanceForFeatures == true) instance.name = item.GetSource().name;
                item.featureInstance = instance;
                world.AddFeature(instance, doConstruct: false);

                if (item.GetSubFeatures() != null) {

                    foreach (var subItem in item.GetSubFeatures().features) {

                        this.InitializePre(world, subItem);

                    }

                }
                
            }

        }

        public void InitializePost(World world) {
            
            this.InitializePost(world, this.features);
            
        }

        public void InitializePost(World world, System.Collections.Generic.List<T> features) {

            for (int i = 0; i < features.Count; ++i) {
                
                var item = features[i];
                this.InitializePost(world, item);

            }

        }
        
        private void InitializePost(World world, BaseFeatureData item) {

            if (item.IsEnabled() == true) {

                item.featureInstance.DoConstruct();
                    
                if (item.GetSubFeatures() != null) {

                    foreach (var subItem in item.GetSubFeatures().features) {

                        this.InitializePost(world, subItem);

                    }

                }
                
            }

        }

        public void InitializeLate(World world) {
            
            this.InitializeLate(world, this.features);
            
        }

        public void InitializeLate(World world, System.Collections.Generic.List<T> features) {

            for (int i = 0; i < features.Count; ++i) {
                
                var item = features[i];
                this.InitializeLate(world, item);
                
            }

        }
        
        public void InitializeLate(World world, BaseFeatureData item) {

            if (item.IsEnabled() == true) {
                
                item.featureInstance.DoConstructLate();
                
                if (item.GetSubFeatures() != null) {

                    foreach (var subItem in item.GetSubFeatures().features) {

                        this.InitializeLate(world, subItem);

                    }

                }
                
            }

        }

        internal void DeInitialize(World world) {
            
            this.DeInitialize(world, this.features);
            
        }

        internal void DeInitialize(World world, System.Collections.Generic.List<T> features) {
            
            for (int i = features.Count - 1; i >= 0; --i) {
                
                var item = features[i];
                this.DeInitialize(world, item);

            }

        }
        
        private void DeInitialize(World world, BaseFeatureData item) {
            
            if (item.IsEnabled() == true) {
                
                if (item.GetSubFeatures() != null) {

                    for (int i = item.GetSubFeatures().features.Count - 1; i >= 0; --i) {

                        var subItem = item.GetSubFeatures().features[i];
                        this.DeInitialize(world, subItem);

                    }
                    
                }

                world.RemoveFeature(item.featureInstance);
                if (world.settings.createInstanceForFeatures == true) UnityEngine.Object.DestroyImmediate(item.featureInstance);
                item.featureInstance = null;

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

        internal void DoConstructLate() {
            
            Filter.RegisterInject(this.InjectFilter);
            this.OnConstructLate();
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
        protected virtual void OnConstructLate() {}

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