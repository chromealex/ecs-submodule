using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ME.ECS {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class InitializerBase : MonoBehaviour {

        public enum ConfigurationType {
            DebugOnly,
            ReleaseOnly,
            DebugAndRelease,
        }

        public struct DefineInfo {

            public string define;
            public string description;
            public System.Func<bool> isActive;
            public bool showInList;
            public ConfigurationType configurationType;

            public DefineInfo(string define, string description, System.Func<bool> isActive, bool showInList, ConfigurationType configurationType) {

                this.define = define;
                this.description = description;
                this.isActive = isActive;
                this.showInList = showInList;
                this.configurationType = configurationType;

            }

        }

        [System.Serializable]
        public struct Configuration {

            [System.Serializable]
            public struct Define {

                public bool enabled;
                public string name;
                
            }

            public string name;
            public bool isDirty;
            public ConfigurationType configurationType;
            public System.Collections.Generic.List<Define> defines;

            public bool Add(DefineInfo info) {

                if (this.configurationType == ConfigurationType.DebugOnly &&
                    info.configurationType == InitializerBase.ConfigurationType.ReleaseOnly) {

                    return false;

                }

                if (this.configurationType == ConfigurationType.ReleaseOnly &&
                    info.configurationType == InitializerBase.ConfigurationType.DebugOnly) {
                    
                    return false;
                    
                }

                if (this.defines == null) this.defines = new System.Collections.Generic.List<Define>();
                var isExists = false;
                foreach (var item in this.defines) {

                    if (item.name == info.define) {

                        isExists = true;
                        break;

                    }
                    
                }

                if (isExists == false) {
                    
                    this.defines.Add(new Define() {
                        enabled = info.isActive.Invoke(),
                        name = info.define,
                    });
                    return true;
                    
                }

                return false;

            }

            public void SetEnabled(string define) {

                for (var i = 0; i < this.defines.Count; ++i) {
                    
                    var item = this.defines[i];
                    if (item.name == define) {

                        item.enabled = true;
                        this.defines[i] = item;
                        break;

                    }
                }

            }

            public void SetDisabled(string define) {

                for (var i = 0; i < this.defines.Count; ++i) {
                    
                    var item = this.defines[i];
                    if (item.name == define) {

                        item.enabled = false;
                        this.defines[i] = item;
                        break;

                    }
                }

            }

        }

        [System.Serializable]
        public struct EndOfBaseClass { }
        
        public System.Collections.Generic.List<Configuration> configurations = new System.Collections.Generic.List<Configuration>();
        public string selectedConfiguration;

        public FeaturesListCategories featuresListCategories = new FeaturesListCategories();
        public WorldSettings worldSettings = WorldSettings.Default;
        public WorldDebugSettings worldDebugSettings = WorldDebugSettings.Default;
        public EndOfBaseClass endOfBaseClass;

        protected void Initialize(World world, bool callLateInitialization = true) {

            world.SetSettings(this.worldSettings);
            world.SetDebugSettings(this.worldDebugSettings);
            world.TryInitializeDefaults();

            // Initialize features
            this.InitializeFeatures(world, callLateInitialization);
            
            // Initialize scene
            this.InitializeScene(world);

        }

        private void InitializeScene(World world) {

            var sceneEntityViews = InitializerBase.FindObjectsOfType<ME.ECS.Views.Providers.SceneViewInitializer>();
            var list = PoolList<ME.ECS.Views.Providers.SceneViewInitializer>.Spawn(sceneEntityViews.Length);
            for (int i = 0; i < sceneEntityViews.Length; ++i) {

                var view = sceneEntityViews[i];
                if (view != null) {

                    var parent = view.GetComponentsInParent<ME.ECS.Views.Providers.SceneViewInitializer>(true);
                    if (parent.Length <= 1) {

                        list.Add(view);

                    }

                }

            }
            for (int i = 0; i < list.Count; ++i) {

                var view = list[i];
                ((ME.ECS.Views.Providers.ISceneView)view).Initialize(world);
                
            }
            
            PoolList<ME.ECS.Views.Providers.SceneViewInitializer>.Recycle(ref list);

        }

        protected void InitializeFeatures(World world, bool callLateInitialization) {

            this.featuresListCategories.Initialize(world, callLateInitialization);

        }

        protected void DeInitializeFeatures(World world) {

            this.featuresListCategories.DeInitialize(world);

        }

    }

}
