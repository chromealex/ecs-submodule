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
    public partial class InitializerEditor : Editor {

        public static System.Func<InitializerBase.Configuration, InitializerBase.Configuration> buildConfiguration;
        public static System.Func<InitializerBase.DefineInfo[]> getAdditionalDefines;

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

        private static readonly System.Lazy<GUIStyle> styleFoldout = new (() => new GUIStyle(EditorStyles.foldoutHeader) { fixedHeight = 26f });

        private class PrefVar<T> where T: struct {

            private T valueCache;
            private readonly string key;
            private readonly System.Action<string, T> setter;

            public T value {
                get => this.valueCache;
                set {
                    if (this.valueCache.Equals(value) == false) {
                        this.valueCache = value;
                        this.setter(this.key, value);
                    }
                }
            }

            private PrefVar() { }

            public PrefVar(string key, System.Func<string, T, T> getter, System.Action<string, T> setter, T defaultValue) {
                this.key = key;
                this.setter = setter;
                this.valueCache = getter(key, defaultValue);
            }

        }
        
        private class PrefBool : PrefVar<bool> {
            public PrefBool(string key, bool defaultValue) : base(key, EditorPrefs.GetBool, EditorPrefs.SetBool, defaultValue) { }

        }

        private PrefBool settingsFoldOut;
        private PrefBool definesFoldOut;
        private PrefBool settingsDebugFoldOut;
        
        public void OnEnable() {
            
            this.settingsFoldOut = new PrefBool("ME.ECS.InitializerEditor.settingsFoldoutState", false);
            this.definesFoldOut = new PrefBool("ME.ECS.InitializerEditor.definesFoldoutState", false);
            this.settingsDebugFoldOut = new PrefBool("ME.ECS.InitializerEditor.settingsDebugFoldoutState", false);

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
            var defines = InitializerEditor.GetDefines();
            for (int i = 0; i < target.configurations.Count; ++i) {

                var conf = target.configurations[i];
                foreach (var define in defines) {

                    if (conf.Contains(define) == false) {
                        
                        //conf.Remove(define);
                        conf.Add(define);
                        changed = true;
                        
                    } else {
                        
                        if (conf.Add(define) == true) {

                            changed = true;

                        }
                        
                    }

                }
                target.configurations[i] = conf;

            }
            
            if (changed == true) EditorUtility.SetDirty(target);

        }

        private Editor GetEditorForTarget(Object target) {

            return Editor.CreateEditor(target);
            
        }

        public override void OnInspectorGUI() {
            
            this.serializedObject.Update();

            ((Component)this.target).transform.hideFlags = HideFlags.HideInInspector;
            
            GUILayoutExt.CollectEditors<IDebugViewGUIEditor<InitializerBase>, ViewProviderCustomEditorAttribute>(ref this.viewsDebugEditors);
            GUILayoutExt.CollectEditors<IJobsViewGUIEditor<InitializerBase>, ViewProviderCustomEditorAttribute>(ref this.viewsJobsEditors);
            
            var target = this.target as InitializerBase;
            
            GUILayoutExt.Box(15f, 0f, () => {

                var isDirty = false;
                
                this.definesFoldOut.value = GUILayoutExt.BeginFoldoutHeaderGroup(this.definesFoldOut.value, new GUIContent("Defines"), InitializerEditor.styleFoldout.Value);
                if (this.definesFoldOut.value == true) {

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

                            GUILayout.BeginHorizontal();
                            {
                                var value = define.enabled;
                                GUILayout.BeginVertical();
                                {
                                    if (defineInfo.deprecatedVersion != null) {
                                        var style = new GUIStyle(EditorStyles.miniBoldLabel);
                                        style.richText = true;
                                        GUILayout.Label($"<color=yellow>Deprecated, removed after {defineInfo.deprecatedVersion}</color>", style);
                                    }
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
                                GUILayout.EndVertical();

                                using (new GUILayoutExt.GUIAlphaUsing(define.IsActualEnabled(defineInfo) == true ? 1f : 0.5f)) {

                                    GUILayout.BeginHorizontal(GUILayout.Width(80f));
                                    {
                                        GUILayout.BeginHorizontal(GUILayout.Width(40f));
                                        {
                                            var codeSize = defineInfo.codeSize;
                                            var tooltip = "How much generated IL2CPP code with/without this define will take.\n\n";
                                            switch (codeSize) {
                                                case InitializerBase.CodeSize.Light:
                                                    tooltip += "Light - Has static size without generics";
                                                    break;
                                                case InitializerBase.CodeSize.Normal:
                                                    tooltip += "Normal - Depends on components count, but contains not heavy/doesn't contains any generic instructions";
                                                    break;
                                                case InitializerBase.CodeSize.Heavy:
                                                    tooltip += "Heavy - Depends on components count and contains heavy generic instructions";
                                                    break;
                                            }
    
                                            if (codeSize != InitializerBase.CodeSize.Unknown) {
                                                GUILayoutExt.ProgressBar((float)codeSize, (float)InitializerBase.CodeSize.Heavy, Color.black, Color.green, Color.red,
                                                                         new GUIContent("Code Size", tooltip));
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                        GUILayout.BeginHorizontal(GUILayout.Width(40f));
                                        {
                                            var runtimeSpeed = defineInfo.runtimeSpeed;
                                            var tooltip = "How fast this code works.\n\n";
                                            switch (runtimeSpeed) {
                                                case InitializerBase.RuntimeSpeed.Light:
                                                    tooltip += "Light - Has no additional instructions at all at runtime";
                                                    break;
                                                case InitializerBase.RuntimeSpeed.Normal:
                                                    tooltip += "Normal - Has additional light-weight instructions at runtime";
                                                    break;
                                                case InitializerBase.RuntimeSpeed.Heavy:
                                                    tooltip += "Heavy - Has heavy instructions every tick";
                                                    break;
                                            }
    
                                            if (runtimeSpeed != InitializerBase.RuntimeSpeed.Unknown) {
                                                GUILayoutExt.ProgressBar((float)runtimeSpeed, (float)InitializerBase.RuntimeSpeed.Heavy, Color.black, Color.green, Color.red,
                                                                         new GUIContent("Runtime Speed", tooltip));
                                            }
                                        }
                                        GUILayout.EndHorizontal();
                                    }
                                    GUILayout.EndHorizontal();
                                    
                                }
                                
                            }
                            GUILayout.EndHorizontal();
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

                this.settingsFoldOut.value = GUILayoutExt.BeginFoldoutHeaderGroup(this.settingsFoldOut.value, new GUIContent("Settings"), InitializerEditor.styleFoldout.Value);
                if (this.settingsFoldOut.value == true) {

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

                        GUILayoutExt.ToggleLeft(
                            ref target.worldSettings.updateVisualWhileRollback,
                            ref isDirty,
                            "Perform visual update while rollback",
                            "When rollback is performed, do we need to update visual modules?");

                        GUILayoutExt.EnumField(
                            ref target.worldSettings.frameFixType,
                            ref isDirty,
                            "Simulation limitation type",
                            "You can choose right behaviour depends on your game.");

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Space(10);
                            GUILayout.BeginVertical();
                            if (target.worldSettings.frameFixType == FrameFixBehaviour.ExceptionOverTicksPreFrame) {

                                GUILayoutExt.IntFieldLeft(
                                    ref target.worldSettings.frameFixValue,
                                    ref isDirty,
                                    "Max ticks per simulation frame",
                                    "If simulation ticks count will be over this value, exception will be thrown.",
                                    1);

                            } else if (target.worldSettings.frameFixType == FrameFixBehaviour.AsyncOverMillisecondsPerFrame) {

                                GUILayoutExt.IntFieldLeft(
                                    ref target.worldSettings.frameFixValue,
                                    ref isDirty,
                                    "Max ms per simulation frame",
                                    "If simulation frame time in milliseconds will be over this value, value will be clamped and simulation continues at the next simulation frame.",
                                    1);

                            } else if (target.worldSettings.frameFixType == FrameFixBehaviour.AsyncOverTicksPerFrame) {

                                GUILayoutExt.IntFieldLeft(
                                    ref target.worldSettings.frameFixValue,
                                    ref isDirty,
                                    "Max ticks per simulation frame",
                                    "If simulation frame time in milliseconds will be over this value, value will be clamped and simulation continues at the next simulation frame.",
                                    1);

                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();

                        GUILayoutExt.ToggleLeft(
                            ref target.worldSettings.useJobsForViews,
                            ref isDirty,
                            "Use jobs for Views",
                            "Some view providers have jobs implementation. Turn it on to enable them update views inside jobs. Please note that some providers could lose some method calls.");

                        GUILayoutExt.ToggleLeft(
                            ref target.worldSettings.viewsSettings.interpolationState,
                            ref isDirty,
                            "Interpolate view state",
                            "Use additional interpolate state for views");
                        
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
                
                this.settingsDebugFoldOut.value = GUILayoutExt.BeginFoldoutHeaderGroup(this.settingsDebugFoldOut.value, new GUIContent("Debug Settings"), InitializerEditor.styleFoldout.Value);
                if (this.settingsDebugFoldOut.value == true) {
                    
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
                                    var statObj = (ME.ECS.DebugUtils.StatisticsObject)EditorGUILayout.ObjectField("Statistic Object",
                                                                                                                  target.worldDebugSettings.statisticsObject,
                                                                                                                  typeof(ME.ECS.DebugUtils.StatisticsObject),
                                                                                                                  allowSceneObjects: false);
                                    if (target.worldDebugSettings.statisticsObject != statObj) {
                                        
                                        target.worldDebugSettings.statisticsObject = statObj;
                                        isDirty = true;

                                    }
                                    if (statObj == null) {

                                        EditorGUILayout.HelpBox("Object is None, create custom statistic object or create default one.", MessageType.Warning);
                                        if (GUILayout.Button("Create Default") == true) {

                                            statObj = ME.ECS.DebugUtils.StatisticsObject.CreateInstance<ME.ECS.DebugUtils.StatisticsObject>();
                                            var path = AssetDatabase.GetAssetPath(this.target);
                                            var dir = System.IO.Path.GetDirectoryName(path);
                                            path = dir + "/" + this.target.name + "_StatisticObject.asset";
                                            AssetDatabase.CreateAsset(statObj, path);
                                            AssetDatabase.ImportAsset(path);

                                            var so = AssetDatabase.LoadAssetAtPath<ME.ECS.DebugUtils.StatisticsObject>(path);
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
                    EditorApplication.delayCall += () => {
                        AssetDatabase.ForceReserializeAssets(new string[] { AssetDatabase.GetAssetPath(this.target) });
                    };

                }

            });
            
            EditorGUILayout.Space();
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying == true || EditorApplication.isPaused == true);
            //InitializerEditor.listCategories.DoLayoutList();
            this.listCategoriesProp = this.serializedObject.FindProperty("featuresListCategories");
            EditorGUILayout.PropertyField(this.listCategoriesProp);
            EditorGUI.EndDisabledGroup();

            this.serializedObject.ApplyModifiedProperties();

        }

        private System.Collections.Generic.List<string> CollectAllActiveDefines(bool isRelease) {

            var list = new System.Collections.Generic.List<string>();
            foreach (var define in InitializerEditor.GetDefines()) {

                if (isRelease == true) {
                    
                    if (/*define.isRelease == true &&*/ define.isActive.Invoke() == true) list.Add(define.define);

                } else {

                    if (define.isActive.Invoke() == true) list.Add(define.define);

                }

            }
            return list;

        }
        
        private void BuildConfiguration(InitializerBase.Configuration configuration) {

            if (InitializerEditor.buildConfiguration != null) configuration = InitializerEditor.buildConfiguration.Invoke(configuration);
            
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
