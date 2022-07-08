using System.Linq;
using UnityEngine.UIElements;

namespace ME.ECSEditor {

    using UnityEngine;
    using UnityEditor;
    using ME.ECS;
    
    public class ViewProviderCustomEditorAttribute : CustomEditorAttribute {

        public ViewProviderCustomEditorAttribute(System.Type type, int order = 0) : base(type, order) {}

    }

    [UnityEditor.CustomEditor(typeof(InitializerBase), true)]
    public class InitializerEditor : Editor {

        private const float ONE_LINE_HEIGHT = 22f;
        
        private static System.Collections.Generic.Dictionary<object, bool> systemFoldouts = new System.Collections.Generic.Dictionary<object, bool>();
        private static System.Collections.Generic.Dictionary<object, bool> moduleFoldouts = new System.Collections.Generic.Dictionary<object, bool>();
        private static System.Collections.Generic.Dictionary<object, bool> featureFoldouts = new System.Collections.Generic.Dictionary<object, bool>();
        private static System.Collections.Generic.Dictionary<object, UnityEditorInternal.ReorderableList> subFeatureLists = new System.Collections.Generic.Dictionary<object, UnityEditorInternal.ReorderableList>();
        private static System.Collections.Generic.Dictionary<object, Editor> editorForTarget = new System.Collections.Generic.Dictionary<object, Editor>();
        
        private System.Collections.Generic.Dictionary<FeatureBase, System.Collections.Generic.List<ObjectInfo>> cacheSystems = new System.Collections.Generic.Dictionary<FeatureBase, System.Collections.Generic.List<ObjectInfo>>();
        private System.Collections.Generic.Dictionary<FeatureBase, System.Collections.Generic.List<ObjectInfo>> cacheModules = new System.Collections.Generic.Dictionary<FeatureBase, System.Collections.Generic.List<ObjectInfo>>();

        private System.Collections.Generic.Dictionary<System.Type, IDebugViewGUIEditor<InitializerBase>> viewsDebugEditors;
        private System.Collections.Generic.Dictionary<System.Type, IJobsViewGUIEditor<InitializerBase>> viewsJobsEditors;
        public struct ObjectInfo {

            public string typeName;
            public System.Type type;

        }

        private SerializedProperty listCategoriesProp;
        
        private static GUIStyle styleFoldout {
            get {
                var s = new GUIStyle(EditorStyles.foldoutHeader);
                s.fixedHeight = 26f;
                return s;
            }
        }

        private static readonly InitializerBase.DefineInfo[] defines = new[] {
            new InitializerBase.DefineInfo("GAMEOBJECT_VIEWS_MODULE_SUPPORT", "Turn on/off GameObject View Provider.", () => {
                #if GAMEOBJECT_VIEWS_MODULE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("PARTICLES_VIEWS_MODULE_SUPPORT", "Turn on/off Particles View Provider.", () => {
                #if PARTICLES_VIEWS_MODULE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("DRAWMESH_VIEWS_MODULE_SUPPORT", "Turn on/off Graphics View Provider.", () => {
                #if DRAWMESH_VIEWS_MODULE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("WORLD_STATE_CHECK", "If turned on, ME.ECS will check that all write data methods are in right state. If you turn off this check, you'll be able to write data in any state, but it could cause out of sync state.", () => {
                #if WORLD_STATE_CHECK
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugOnly),
            new InitializerBase.DefineInfo("WORLD_THREAD_CHECK", "If turned on, ME.ECS will check random number usage from non-world thread. If you don't want to synchronize the game, you could turn this check off.", () => {
                #if WORLD_THREAD_CHECK
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugOnly),
            new InitializerBase.DefineInfo("WORLD_EXCEPTIONS", "If turned on, ME.ECS will throw exceptions on unexpected behaviour. Turn off this check in release builds.", () => {
                #if WORLD_EXCEPTIONS
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugOnly),
            new InitializerBase.DefineInfo("WORLD_TICK_THREADED", "If turned on, ME.ECS will run logic in another thread.", () => {
                #if WORLD_TICK_THREADED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("FPS_MODULE_SUPPORT", "FPS module support.", () => {
                #if FPS_MODULE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ECS_COMPILE_IL2CPP_OPTIONS", "If turned on, ME.ECS will use IL2CPP options for the faster runtime, this flag removed unnecessary null-checks and bounds array checks.", () => {
                #if ECS_COMPILE_IL2CPP_OPTIONS
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ECS_COMPILE_IL2CPP_OPTIONS_FILE_INCLUDE", "Turn off this option if you provide your own Il2CppSetOptionAttribute. Works with ECS_COMPILE_IL2CPP_OPTIONS.", () => {
                #if ECS_COMPILE_IL2CPP_OPTIONS_FILE_INCLUDE
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("MULTITHREAD_SUPPORT", "Turn on this option if you need to add/remove components inside jobs.", () => {
                #if MULTITHREAD_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("MESSAGE_PACK_SUPPORT", "MessagePack package support.", () => {
                #if MESSAGE_PACK_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ENTITY_VERSION_INCREMENT_ACTIONS", "Turn on to raise events on entity version increments.", () => {
                #if ENTITY_VERSION_INCREMENT_ACTIONS
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ENTITY_API_VERSION1_DEPRECATED", "Turn on Entity API with SetData/ReadData/GetData methods.", () => {
                #if ENTITY_API_VERSION1_DEPRECATED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("BUFFER_SLICED_DISABLED", "Turn on to use Sliced Buffers which allows to add entities in Get<> API.", () => {
                #if BUFFER_SLICED_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            #if FILTERS_STORAGE_LEGACY
            new InitializerBase.DefineInfo("ARCHETYPE_SIZE_128", "Set archetype max bits size to 128 (Components in filters).", () => {
                #if ARCHETYPE_SIZE_128
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ARCHETYPE_SIZE_192", "Set archetype max bits size to 192 (Components in filters).", () => {
                #if ARCHETYPE_SIZE_192
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ARCHETYPE_SIZE_256", "Set archetype max bits size to 256 (Components in filters).", () => {
                #if ARCHETYPE_SIZE_256
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            #endif
            new InitializerBase.DefineInfo("VIEWS_REGISTER_VIEW_SOURCE_CHECK_STATE", "Forbid RegisterViewSource after world initialization.", () => {
                #if VIEWS_REGISTER_VIEW_SOURCE_CHECK_STATE
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ME_ECS_COLLECT_WEAK_REFERENCES", "Collect weak references for ecs modules and provide public api (weak/unweak).", () => {
                #if ME_ECS_COLLECT_WEAK_REFERENCES
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugOnly),
            new InitializerBase.DefineInfo("FILTERS_STORAGE_LEGACY", "Legacy storage instead of Archetypes.", () => {
                #if FILTERS_STORAGE_LEGACY
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("FIXED_POINT_MATH", "Fixed-Point Math.", () => {
                #if FIXED_POINT_MATH
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("SHARED_COMPONENTS_DISABLED", "Disable shared components storage and entity shared API. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if SHARED_COMPONENTS_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ENTITY_TIMERS_DISABLED", "Disable timers storage and entity timers API. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if ENTITY_TIMERS_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("COMPONENTS_VERSION_NO_STATE_DISABLED", "Disable components version no state storage and entity no state API. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if COMPONENTS_VERSION_NO_STATE_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("ENTITIES_GROUP_DISABLED", "Disable entities group storage and entities group API. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if ENTITIES_GROUP_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
            new InitializerBase.DefineInfo("STATIC_API_DISABLED", "Disable static API for entities. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if STATIC_API_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease),
        };
        
        private bool settingsFoldOut {
            get {
                return EditorPrefs.GetBool("ME.ECS.InitializerEditor.settingsFoldoutState", false);
            }
            set {
                EditorPrefs.SetBool("ME.ECS.InitializerEditor.settingsFoldoutState", value);
            }
            
        }

        private bool definesFoldOut {
            get {
                return EditorPrefs.GetBool("ME.ECS.InitializerEditor.definesFoldoutState", false);
            }
            set {
                EditorPrefs.SetBool("ME.ECS.InitializerEditor.definesFoldoutState", value);
            }
            
        }

        private bool settingsDebugFoldOut {
            get {
                return EditorPrefs.GetBool("ME.ECS.InitializerEditor.settingsDebugFoldoutState", false);
            }
            set {
                EditorPrefs.SetBool("ME.ECS.InitializerEditor.settingsDebugFoldoutState", value);
            }
        }

        public void OnEnable() {

            if (this.target == null) return;
            var target = this.target as InitializerBase;
            if (target == null) Debug.LogWarning($"Target is null, but base.target is {this.target}");
            if (target.configurations.Count == 0) {

                this.MakeDefaultConfigurations();

            }

            this.listCategoriesProp = this.serializedObject.FindProperty("featuresListCategories");
            
            this.ValidateConfigurations();
            
            if (this.targets.Length > 0 && this.target is Component) {

                ((Component)this.target).transform.hideFlags = HideFlags.HideInInspector;

            }

        }

        public void OnDisable() {
            
            //((Component)this.target).transform.hideFlags = HideFlags.None;

        }

        private void MakeDefaultConfigurations() {

            var target = this.target as InitializerBase;
            
            var debugConfiguration = new InitializerBase.Configuration() {
                name = "Debug",
                configurationType = InitializerBase.ConfigurationType.DebugOnly,
            };
            var releaseConfiguration = new InitializerBase.Configuration() {
                name = "Release",
                configurationType = InitializerBase.ConfigurationType.ReleaseOnly,
            };
            
            target.configurations.Add(debugConfiguration);
            target.configurations.Add(releaseConfiguration);
            target.selectedConfiguration = "Debug";
            
            EditorUtility.SetDirty(target);

        }

        private void ValidateConfigurations() {

            var target = this.target as InitializerBase;

            var changed = false;
            for (int i = 0; i < target.configurations.Count; ++i) {

                var conf = target.configurations[i];
                foreach (var define in InitializerEditor.defines) {

                    if (conf.Add(define) == true) {

                        changed = true;

                    }

                }
                target.configurations[i] = conf;

            }
            
            if (changed == true) EditorUtility.SetDirty(target);

        }

        private Editor GetEditorForTarget(Object target) {

            return Editor.CreateEditor(target);
            
        }

        private InitializerBase.DefineInfo GetDefineInfo(string define) {

            foreach (var defineInfo in InitializerEditor.defines) {

                if (defineInfo.define == define) {
                    
                    return defineInfo;
                    
                }
                
            }

            return default;

        }

        public override void OnInspectorGUI() {
            
            this.serializedObject.Update();

            ((Component)this.target).transform.hideFlags = HideFlags.HideInInspector;
            
            GUILayoutExt.CollectEditors<IDebugViewGUIEditor<InitializerBase>, ViewProviderCustomEditorAttribute>(ref this.viewsDebugEditors);
            GUILayoutExt.CollectEditors<IJobsViewGUIEditor<InitializerBase>, ViewProviderCustomEditorAttribute>(ref this.viewsJobsEditors);
            
            var target = this.target as InitializerBase;
            
            GUILayoutExt.Box(15f, 0f, () => {

                var isDirty = false;
                
                this.definesFoldOut = GUILayoutExt.BeginFoldoutHeaderGroup(this.definesFoldOut, new GUIContent("Defines"), InitializerEditor.styleFoldout);
                if (this.definesFoldOut == true) {

                    GUILayoutExt.Padding(2f, 2f, () => {

                        EditorGUI.BeginDisabledGroup(
                            EditorApplication.isCompiling == true || EditorApplication.isPlaying == true ||
                            EditorApplication.isPaused == true /* || InitializerEditor.isCompilingManual == true*/);

                        var configurations = target.configurations.Select(x => x.name).ToArray();
                        var selectedIndex = System.Array.IndexOf(configurations, target.selectedConfiguration);

                        var conf = target.configurations[selectedIndex];
                        GUILayoutExt.Box(2f, 2f, () => {

                            var newIndex = EditorGUILayout.Popup("Configuration", selectedIndex, configurations);
                            if (newIndex != selectedIndex) {

                                target.selectedConfiguration = target.configurations[newIndex].name;
                                EditorUtility.SetDirty(this.target);
                                this.BuildConfiguration(target.configurations[newIndex]);

                            }

                            GUILayoutExt.SmallLabel("Select configuration to build with. Be sure Release configuration is selected when you build final product.");

                            if (conf.isDirty == true) {

                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.HelpBox("You have unsaved changes", MessageType.Warning);
                                    if (GUILayout.Button("Apply") == true) {

                                        conf.isDirty = false;
                                        target.configurations[selectedIndex] = conf;
                                        EditorUtility.SetDirty(this.target);
                                        this.BuildConfiguration(conf);

                                    }
                                }
                                GUILayout.EndHorizontal();

                            }

                        });

                        EditorGUILayout.Space();

                        EditorGUI.BeginChangeCheck();
                        for (var i = 0; i < conf.defines.Count; ++i) {

                            var define = conf.defines[i];
                            var defineInfo = this.GetDefineInfo(define.name);
                            if (defineInfo.showInList == false) continue;

                            if (conf.configurationType == InitializerBase.ConfigurationType.ReleaseOnly) {

                                if (defineInfo.configurationType == InitializerBase.ConfigurationType.DebugOnly) continue;

                            }

                            if (conf.configurationType == InitializerBase.ConfigurationType.DebugOnly) {

                                if (defineInfo.configurationType == InitializerBase.ConfigurationType.ReleaseOnly) continue;

                            }

                            var value = define.enabled;
                            if (GUILayoutExt.ToggleLeft(
                                    ref value,
                                    ref isDirty,
                                    defineInfo.define,
                                    defineInfo.description, () => {

                                        if (defineInfo.configurationType == InitializerBase.ConfigurationType.DebugOnly) {

                                            using (new GUILayoutExt.GUIColorUsing(new Color(0.9f, 0.7f, 1f, 0.8f))) {
                                                GUILayout.Label("It is Debug-only define.", EditorStyles.miniLabel);
                                            }

                                        }

                                        if (defineInfo.configurationType == InitializerBase.ConfigurationType.ReleaseOnly) {

                                            using (new GUILayoutExt.GUIColorUsing(new Color(0.9f, 0.7f, 1f, 0.8f))) {
                                                GUILayout.Label("It is Release-only define.", EditorStyles.miniLabel);
                                            }

                                        }

                                    }) == true) {

                                if (value == true) {

                                    conf.SetEnabled(define.name);
                                    GUI.changed = true;

                                } else {

                                    conf.SetDisabled(define.name);
                                    GUI.changed = true;

                                }

                            }
                        }

                        if (EditorGUI.EndChangeCheck() == true) {

                            conf.isDirty = true;
                            target.configurations[selectedIndex] = conf;
                            EditorUtility.SetDirty(this.target);

                        }

                        EditorGUI.EndDisabledGroup();

                    }, EditorStyles.helpBox);
                    
                }
                GUILayoutExt.Separator(new Color(0f, 0f, 0f, 0.5f), offset: new RectOffset(16, 16, 0, 0));

                this.settingsFoldOut = GUILayoutExt.BeginFoldoutHeaderGroup(this.settingsFoldOut, new GUIContent("Settings"), InitializerEditor.styleFoldout);
                if (this.settingsFoldOut == true) {

                    GUILayoutExt.Padding(2f, 2f, () => {

                        GUILayoutExt.ToggleLeft(
                            ref target.worldSettings.turnOffViews,
                            ref isDirty,
                            "Turn off views module",
                            "If you want to run ME.ECS on server, you don't need to use Views at all. Turn off views module to avoid updating view instances overhead.");

                        GUILayoutExt.ToggleLeft(
                            ref target.worldSettings.useJobsForSystems,
                            ref isDirty,
                            "Use jobs for Systems",
                            "Each system with filter has `jobs` flag which determine AdvanceTick behavior. If checked, jobs will be enabled and AdvanceTick will run asynchronously.");

                        GUILayoutExt.ToggleLeft(
                            ref target.worldSettings.createInstanceForFeatures,
                            ref isDirty,
                            "Create instance copy for Features",
                            "When you add feature into the world, do you need to create copy of feature data at runtime? Turn off this checkbox if you do not want to change features data.");

                        GUILayoutExt.IntFieldLeft(
                            ref target.worldSettings.maxTicksSimulationCount,
                            ref isDirty,
                            "Max ticks per simulation frame",
                            "If simulation ticks count will be over this value, exception will be throw. Zero value = ignore this parameter.");

                        GUILayoutExt.ToggleLeft(
                            ref target.worldSettings.useJobsForViews,
                            ref isDirty,
                            "Use jobs for Views",
                            "Some view providers have jobs implementation. Turn it on to enable them update views inside jobs. Please note that some providers could lose some method calls.");

                        if (this.viewsJobsEditors != null) {

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(10f);
                            {
                                GUILayout.BeginVertical();
                                foreach (var editor in this.viewsJobsEditors) {

                                    GUILayoutExt.Separator();
                                    editor.Value.target = this.target as InitializerBase;
                                    if (editor.Value.OnDrawGUI() == true) {

                                        isDirty = true;

                                    }

                                }

                                GUILayout.EndVertical();
                            }
                            GUILayout.EndHorizontal();

                        }

                    }, EditorStyles.helpBox);

                }
                GUILayoutExt.Separator(new Color(0f, 0f, 0f, 0.5f), offset: new RectOffset(16, 16, 0, 0));
                
                this.settingsDebugFoldOut = GUILayoutExt.BeginFoldoutHeaderGroup(this.settingsDebugFoldOut, new GUIContent("Debug Settings"), InitializerEditor.styleFoldout);
                if (this.settingsDebugFoldOut == true) {
                    
                    GUILayoutExt.Padding(2f, 2f, () => {
                        
                        GUILayoutExt.ToggleLeft(
                            ref target.worldDebugSettings.createGameObjectsRepresentation,
                            ref isDirty,
                            "Create GameObject representation",
                            "Editor-only feature. If checked, all entities will be represented by GameObject with debug information.");

                        {
                        
                            GUILayoutExt.ToggleLeft(
                                ref target.worldDebugSettings.collectStatistic,
                                ref isDirty,
                                "Collect Statistics",
                                "Editor-only feature. If checked, ME.ECS will collect statistics data like entity usage.");

                            if (target.worldDebugSettings.collectStatistic == true) {

                                GUILayout.BeginHorizontal();
                                GUILayout.Space(10f);
                                {
                                    GUILayout.BeginVertical();
                                    var statObj = (ME.ECS.Debug.StatisticsObject)EditorGUILayout.ObjectField("Statistic Object",
                                                                                                            target.worldDebugSettings.statisticsObject,
                                                                                                            typeof(ME.ECS.Debug.StatisticsObject),
                                                                                                            allowSceneObjects: false);
                                    if (target.worldDebugSettings.statisticsObject != statObj) {
                                        
                                        target.worldDebugSettings.statisticsObject = statObj;
                                        isDirty = true;

                                    }
                                    if (statObj == null) {

                                        EditorGUILayout.HelpBox("Object is None, create custom statistic object or create default one.", MessageType.Warning);
                                        if (GUILayout.Button("Create Default") == true) {

                                            statObj = ME.ECS.Debug.StatisticsObject.CreateInstance<ME.ECS.Debug.StatisticsObject>();
                                            var path = AssetDatabase.GetAssetPath(this.target);
                                            var dir = System.IO.Path.GetDirectoryName(path);
                                            path = dir + "/" + this.target.name + "_StatisticObject.asset";
                                            AssetDatabase.CreateAsset(statObj, path);
                                            AssetDatabase.ImportAsset(path);

                                            var so = AssetDatabase.LoadAssetAtPath<ME.ECS.Debug.StatisticsObject>(path);
                                            target.worldDebugSettings.statisticsObject = so;
                                            isDirty = true;

                                        }

                                    }
                                    GUILayout.Space(10f);
                                    GUILayout.EndVertical();
                                }
                                GUILayout.EndHorizontal();

                            }

                        }

                        {
                            
                            GUILayoutExt.ToggleLeft(
                                ref target.worldDebugSettings.showViewsOnScene,
                                ref isDirty,
                                "Show Views in Hierarchy",
                                "Editor-only feature. If checked, views module always show views on scene.");

                            if (this.viewsDebugEditors != null) {

                                GUILayout.BeginHorizontal();
                                GUILayout.Space(10f);
                                {
                                    GUILayout.BeginVertical();
                                    foreach (var editor in this.viewsDebugEditors) {

                                        GUILayoutExt.Separator();
                                        editor.Value.target = this.target as InitializerBase;
                                        if (editor.Value.OnDrawGUI() == true) {

                                            isDirty = true;

                                        }

                                    }
                                    GUILayout.EndVertical();
                                }
                                GUILayout.EndHorizontal();

                            }

                        }

                    }, EditorStyles.helpBox);
                    
                }
                GUILayoutExt.Separator(new Color(0f, 0f, 0f, 0.5f), offset: new RectOffset(16, 16, 0, 0));
                GUILayout.Space(10f);
                
                {

                    var editor = this.GetEditorForTarget(target);
                    var field = editor.serializedObject.GetIterator();
                    editor.serializedObject.Update();
                    var baseClassEnd = false;
                    while (field.NextVisible(true) == true) {

                        if (baseClassEnd == true) {
                            
                            EditorGUILayout.PropertyField(field);

                        }
                        
                        if (field.type == "EndOfBaseClass") {

                            baseClassEnd = true;

                        }
                        
                    }
                    field.Reset();

                    editor.serializedObject.ApplyModifiedProperties();

                }

                if (isDirty == true) {
                    
                    EditorUtility.SetDirty(this.target);
                    AssetDatabase.ForceReserializeAssets(new string[] { AssetDatabase.GetAssetPath(this.target) });

                }

            });
            
            EditorGUILayout.Space();
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying == true || EditorApplication.isPaused == true);
            //InitializerEditor.listCategories.DoLayoutList();
            EditorGUILayout.PropertyField(this.listCategoriesProp);
            EditorGUI.EndDisabledGroup();

            this.serializedObject.ApplyModifiedProperties();

        }

        private System.Collections.Generic.List<string> CollectAllActiveDefines(bool isRelease) {

            var list = new System.Collections.Generic.List<string>();
            foreach (var define in InitializerEditor.defines) {

                if (isRelease == true) {
                    
                    if (/*define.isRelease == true &&*/ define.isActive.Invoke() == true) list.Add(define.define);

                } else {

                    if (define.isActive.Invoke() == true) list.Add(define.define);

                }

            }
            return list;

        }
        
        private void BuildConfiguration(InitializerBase.Configuration configuration) {

            var path = "Assets";
            string file = $"csc-{configuration.name.ToLower()}.gen.rsp";
            
            {
                var mainFile = "csc.gen.rsp";
                var defines = new System.Collections.Generic.Dictionary<string, string>();
                {
                    defines.Add("DEFINES", $"@Assets/{file}");
                }
                ScriptTemplates.Create(path, mainFile, "00-csc-gen.rsp", defines, allowRename: false);
            }
            
            {
                var output = string.Empty;
                foreach (var d in configuration.defines) {

                    if (d.enabled == true) output += $"-define:{d.name}\n";

                }

                var defines = new System.Collections.Generic.Dictionary<string, string>();
                {
                    defines.Add("DEFINES", output);
                }
                ScriptTemplates.Create(path, file, "00-csc-gen.rsp", defines, allowRename: false);
            }

        }

    }

}
