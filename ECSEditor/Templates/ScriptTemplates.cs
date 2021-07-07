using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.Collections.Generic;

namespace ME.ECSEditor {

    public static class ScriptTemplates {

        public const int CREATE_MENU_PRIORITY = 90;
        
        private const int CREATE_PROJECT_PRIORITY = ScriptTemplates.CREATE_MENU_PRIORITY + 100;
        
        private const int CREATE_FEATURE_PRIORITY = ScriptTemplates.CREATE_MENU_PRIORITY + 11;
        private const int CREATE_FEATURE_COMPLEX_PRIORITY = ScriptTemplates.CREATE_MENU_PRIORITY + 12;
        
        private const int CREATE_SYSTEM_PRIORITY = ScriptTemplates.CREATE_MENU_PRIORITY + 13;
        private const int CREATE_SYSTEM_FILTER_PRIORITY = ScriptTemplates.CREATE_MENU_PRIORITY + 14;
        private const int CREATE_MODULE_PRIORITY = ScriptTemplates.CREATE_MENU_PRIORITY + 15;
        private const int CREATE_MARKER_PRIORITY = ScriptTemplates.CREATE_MENU_PRIORITY + 17;
        
        private const int CREATE_COMPONENT_STRUCT_PRIORITY = ScriptTemplates.CREATE_MENU_PRIORITY + 40;

        internal class DoCreateScriptAsset : EndNameEditAction {

            private System.Action<Object> onCreated;
            
            private static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile) {

                var str1 = resourceFile.Replace("#NOTRIM#", "");
                var withoutExtension = System.IO.Path.GetFileNameWithoutExtension(pathName);
                var str2 = str1.Replace("#NAME#", withoutExtension);
                var str3 = withoutExtension.Replace(" ", "");
                var str4 = str2.Replace("#SCRIPTNAME#", str3);
                string templateContent;
                if (char.IsUpper(str3, 0)) {
                    var newValue = char.ToLower(str3[0]).ToString() + str3.Substring(1);
                    templateContent = str4.Replace("#SCRIPTNAME_LOWER#", newValue);
                } else {
                    var newValue = "my" + (object)char.ToUpper(str3[0]) + str3.Substring(1);
                    templateContent = str4.Replace("#SCRIPTNAME_LOWER#", newValue);
                }

                return DoCreateScriptAsset.CreateScriptAssetWithContent(pathName, templateContent);

            }

            public EndNameEditAction SetCallback(System.Action<Object> onCreated) {

                this.onCreated = onCreated;
                return this;

            }

            private static string SetLineEndings(string content, LineEndingsMode lineEndingsMode) {

                string replacement;
                switch (lineEndingsMode) {
                    case LineEndingsMode.OSNative:
                        replacement = Application.platform != RuntimePlatform.WindowsEditor ? "\n" : "\r\n";
                        break;

                    case LineEndingsMode.Unix:
                        replacement = "\n";
                        break;

                    case LineEndingsMode.Windows:
                        replacement = "\r\n";
                        break;

                    default:
                        replacement = "\n";
                        break;
                }

                content = System.Text.RegularExpressions.Regex.Replace(content, "\\r\\n?|\\n", replacement);
                return content;

            }

            private static UnityEngine.Object CreateScriptAssetWithContent(string pathName, string templateContent) {

                templateContent = DoCreateScriptAsset.SetLineEndings(templateContent, EditorSettings.lineEndingsForNewScripts);
                var fullPath = System.IO.Path.GetFullPath(pathName);
                var utF8Encoding = new System.Text.UTF8Encoding(true);
                System.IO.File.WriteAllText(fullPath, templateContent, (System.Text.Encoding)utF8Encoding);
                AssetDatabase.ImportAsset(pathName);
                return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));

            }

            public override void Action(int instanceId, string pathName, string resourceFile) {

                var instance = DoCreateScriptAsset.CreateScriptAssetFromTemplate(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(instance);
                if (this.onCreated != null) this.onCreated.Invoke(instance);

            }

        }

        private static Texture2D scriptIcon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

        internal static void Create(string fileName, string templateName, System.Collections.Generic.Dictionary<string, string> customDefines = null, bool allowRename = true, System.Action<Object> onCreated = null) {

            var obj = ScriptTemplates.GetSelectedDirectory();
            var path = AssetDatabase.GetAssetPath(obj);
            if (System.IO.File.Exists(path) == true) {
                path = System.IO.Path.GetDirectoryName(path);
            }

            if (string.IsNullOrEmpty(path) == true) {
                path = "Assets/";
            }
            
            ScriptTemplates.Create(path, fileName, templateName, customDefines, allowRename, onCreated);
            
        }
        
        internal static bool Create(string path, string fileName, string templateName, System.Collections.Generic.Dictionary<string, string> customDefines = null, bool allowRename = true, System.Action<Object> onCreated = null) {

            var templateAsset = EditorUtilities.Load<TextAsset>($"ECSEditor/Templates/EditorResources/{templateName}.txt", true);
            var content = templateAsset.text;
            if (customDefines != null) {

                foreach (var def in customDefines) {

                    content = content.Replace("#" + def.Key + "#", def.Value);

                }

            }

            var stateTypeStr = "StateClassType";
            if (content.Contains("#STATENAME#") == true) {

                var projectName = path.Split('/');
                var type = typeof(ME.ECS.State);
                var types = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                                  .Where(p => p.IsClass == true && type.IsAssignableFrom(p) && projectName.Contains(p.Name.Replace("State", string.Empty))).ToArray();
                if (types.Length > 0) {

                    var stateType = types[0];
                    stateTypeStr = stateType.Name;

                }

            }

            var @namespace = path.Replace("Assets/", "").Replace("/", ".").Replace("\\", ".");
            content = content.Replace(@"#NAMESPACE#", @namespace);
            content = content.Replace(@"#PROJECTNAME#", @namespace.Split('.')[0]);
            content = content.Replace(@"#STATENAME#", stateTypeStr);
            content = content.Replace(@"#REFERENCES#", string.Empty);

            if (allowRename == true) {

                var defaultNewFileName = fileName;
                var image = ScriptTemplates.scriptIcon;
                ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
            ScriptableObject.CreateInstance<DoCreateScriptAsset>().SetCallback((instance) => {
             
                        if (onCreated != null) onCreated.Invoke(instance);

                    }),
                    defaultNewFileName,
                    image,
                    content);

            } else {

                var fullDir = path + "/" + fileName;
                if (System.IO.File.Exists(fullDir) == true) {

                    var contentExists = System.IO.File.ReadAllText(fullDir);
                    if (contentExists == content) return false;

                }
                
                var withoutExtension = System.IO.Path.GetFileNameWithoutExtension(fullDir);
                withoutExtension = withoutExtension.Replace(" ", "");
                content = content.Replace("#SCRIPTNAME#", withoutExtension);

                var dir = System.IO.Path.GetDirectoryName(fullDir);
                if (System.IO.Directory.Exists(dir) == false) return false;
                
                System.IO.File.WriteAllText(fullDir, content);
                AssetDatabase.ImportAsset(fullDir, ImportAssetOptions.ForceSynchronousImport);

                if (onCreated != null) onCreated.Invoke(AssetDatabase.LoadAssetAtPath<Object>(fullDir));

            }
            
            return true;

        }

        internal static void CreateEmptyDirectory(string path, string dir) {

            var fullDir = path + "/" + dir;
            System.IO.Directory.CreateDirectory(fullDir);
            System.IO.File.WriteAllText(fullDir + "/.dummy", string.Empty);
            AssetDatabase.ImportAsset(fullDir);

        }

        internal static void CreatePrefab(string path, string name, string guid) {
            
            var prefabPath = path + "/" + name + ".prefab";
            
            var content = @"%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!1 &6009573824331432541
GameObject:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  serializedVersion: 6
  m_Component:
  - component: {fileID: 6971210752520958191}
  - component: {fileID: 4958016069329523290}
  m_Layer: 0
  m_Name: GameObject
  m_TagString: Untagged
  m_Icon: {fileID: 0}
  m_NavMeshLayer: 0
  m_StaticEditorFlags: 0
  m_IsActive: 1
--- !u!4 &6971210752520958191
Transform:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 6009573824331432541}
  m_LocalRotation: {x: 0, y: 0, z: 0, w: 1}
  m_LocalPosition: {x: -0.114168406, y: 0, z: 2.8584266}
  m_LocalScale: {x: 1, y: 1, z: 1}
  m_Children: []
  m_Father: {fileID: 0}
  m_RootOrder: 0
  m_LocalEulerAnglesHint: {x: 0, y: 0, z: 0}
--- !u!114 &4958016069329523290
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {fileID: 0}
  m_PrefabInstance: {fileID: 0}
  m_PrefabAsset: {fileID: 0}
  m_GameObject: {fileID: 6009573824331432541}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {fileID: 11500000, guid: #GUID#, type: 3}
  m_Name: 
  m_EditorClassIdentifier:
";
            
            System.IO.File.WriteAllText(prefabPath, content.Replace("#GUID#", guid));
            AssetDatabase.ImportAsset(prefabPath);
            
        }

        private static string GetDirectoryFromAsset(Object obj) {
            
            var path = AssetDatabase.GetAssetPath(obj);
            if (System.IO.File.Exists(path) == true) {
                path = System.IO.Path.GetDirectoryName(path);
            }

            if (string.IsNullOrEmpty(path) == true) {
                path = "Assets";
            }

            return path;

        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/Initialize Project", priority = ScriptTemplates.CREATE_PROJECT_PRIORITY)]
        public static void CreateProject() {

            var obj = ScriptTemplates.GetSelectedDirectory();
            var path = ScriptTemplates.GetDirectoryFromAsset(obj);
            if (System.IO.Directory.GetFiles(path).Length > 0) {
                
                Debug.LogError($"Directory {path} is not empty! Target directory should be empty to start project initialization.");
                return;

            }

            var projectName = System.IO.Path.GetFileName(path);
            projectName = projectName.Replace(".", "");
            projectName = projectName.Replace(" ", "");
            projectName = projectName.Replace("_", "");
            var stateName = projectName + "State";
            var defines = new Dictionary<string, string>() { { "STATENAME", stateName }, { "PROJECTNAME", projectName } };
            
            ScriptTemplates.CreateEmptyDirectory(path, "Modules");
            ScriptTemplates.CreateEmptyDirectory(path, "Systems");
            ScriptTemplates.CreateEmptyDirectory(path, "Components");
            ScriptTemplates.CreateEmptyDirectory(path, "Markers");
            ScriptTemplates.CreateEmptyDirectory(path, "Features");
            ScriptTemplates.CreateEmptyDirectory(path, "Views");
            ScriptTemplates.CreateEmptyDirectory(path, "Generator");

            ScriptTemplates.Create(path, projectName + "State.cs", "00-StateTemplate", defines, allowRename: false);
            ScriptTemplates.Create(path, "AssemblyInfo.cs", "00-AssemblyInfo", defines, allowRename: false);
            
            ScriptTemplates.CreateEmptyDirectory(path + "/Generator", "gen");
            var definesGen = defines.ToDictionary(x => x.Key, x => x.Value);
            definesGen["PROJECTNAME"] = projectName + ".gen";
            ScriptTemplates.Create(path, projectName + ".asmdef", "00-asmdef", defines, allowRename: false);
            var refGuid = AssetDatabase.AssetPathToGUID(path + "/" + projectName + ".asmdef");
            definesGen.Add("REFERENCES", @",""GUID:" + refGuid + @"""");
            ScriptTemplates.Create(path + "/Generator", projectName + ".gen.asmdef", "00-asmdef", definesGen, allowRename: false);
            ScriptTemplates.Create(path + "/Generator", projectName + "Initializer.cs", "00-InitializerTemplate", defines, allowRename: false, onCreated: (asset) => {
                
                var assetPath = AssetDatabase.GetAssetPath(asset);
                var meta = assetPath + ".meta";
                var text = System.IO.File.ReadAllText(meta);
                text = text.Replace("icon: {instanceID: 0}", "icon: {fileID: 2800000, guid: 16b72cc483a6c4dbda2dc209982c422c, type: 3}");
                System.IO.File.WriteAllText(meta, text);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                
                var guid = AssetDatabase.AssetPathToGUID(path + "/Generator/" + projectName + "Initializer.cs");
                if (string.IsNullOrEmpty(guid) == false) ScriptTemplates.CreatePrefab(path, projectName + "Initializer", guid);

            });
            ScriptTemplates.Create(path + "/Generator", "csc.rsp", "00-csc.rsp", defines, allowRename: false);
            
            ScriptTemplates.Create(path, "Modules/FPSModule.cs", "00-FPSModuleTemplate", defines, allowRename: false);
            ScriptTemplates.Create(path, "Modules/NetworkModule.cs", "00-NetworkModuleTemplate", defines, allowRename: false);
            ScriptTemplates.Create(path, "Modules/StatesHistoryModule.cs", "00-StatesHistoryModuleTemplate", defines, allowRename: false);
            
            ScriptTemplates.Create(path, "csc.rsp", "00-csc.rsp", defines, allowRename: false);
            ScriptTemplates.Create("Assets", "csc.gen.rsp", "00-csc-gen-default.rsp", defines, allowRename: false);

        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/Module", priority = ScriptTemplates.CREATE_MODULE_PRIORITY)]
        public static void CreateModule() {

            var obj = ScriptTemplates.GetSelectedDirectory();
            if (obj != null) {

                if (ScriptTemplates.GetFeature(obj, out var featureName) == true) {
                    
                    ScriptTemplates.Create("New Module.cs", "01-ModuleFeatureTemplate", new Dictionary<string, string>() {
                        { "FEATURE", featureName },
                    });
                    return;
                    
                }
                
            }

            ScriptTemplates.Create("New Module.cs", "01-ModuleTemplate");

        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/System", priority = ScriptTemplates.CREATE_SYSTEM_PRIORITY)]
        public static void CreateSystem() {

            var obj = ScriptTemplates.GetSelectedDirectory();
            if (obj != null) {

                if (ScriptTemplates.GetFeature(obj, out var featureName) == true) {
                    
                    ScriptTemplates.Create("New System.cs", "11-SystemFeatureTemplate", new Dictionary<string, string>() {
                        { "FEATURE", featureName },
                    });
                    return;
                    
                }
                
            }

            ScriptTemplates.Create("New System.cs", "11-SystemTemplate");

        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/System with Filter", priority = ScriptTemplates.CREATE_SYSTEM_FILTER_PRIORITY)]
        public static void CreateSystemFilter() {

            var obj = ScriptTemplates.GetSelectedDirectory();
            if (obj != null) {

                if (ScriptTemplates.GetFeature(obj, out var featureName) == true) {
                    
                    ScriptTemplates.Create("New System with Filter.cs", "12-SystemFilterFeatureTemplate", new Dictionary<string, string>() {
                        { "FEATURE", featureName },
                    });
                    return;
                    
                }
                
            }

            ScriptTemplates.Create("New System with Filter.cs", "12-SystemFilterTemplate");

        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/Marker", priority = ScriptTemplates.CREATE_MARKER_PRIORITY)]
        public static void CreateMarker() {

            ScriptTemplates.Create("New Marker.cs", "51-MarkerTemplate");

        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/Feature", priority = ScriptTemplates.CREATE_FEATURE_PRIORITY)]
        public static void CreateFeature() {

            ScriptTemplates.Create("New Feature.cs", "61-FeatureTemplate");

        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {

            ScriptTemplates.CreateFeatureComplex_AfterCompilation();

        }

        private static void CreateFeatureComplex_AfterCompilation() {
            
            var waitForCompilation = EditorPrefs.GetBool("Temp.EditorWaitCompilation.CreateFeatureComplex");
            if (waitForCompilation == true) {
                
                EditorPrefs.DeleteKey("Temp.EditorWaitCompilation.CreateFeatureComplex");
                
                var dir = EditorPrefs.GetString("Temp.EditorWaitCompilation.CreateFeatureComplex.Dir");
                var assetName = EditorPrefs.GetString("Temp.EditorWaitCompilation.CreateFeatureComplex.Name");
                var assetPath = EditorPrefs.GetString("Temp.EditorWaitCompilation.CreateFeatureComplex.ScriptPath");
                var newAssetPath = EditorPrefs.GetString("Temp.EditorWaitCompilation.CreateFeatureComplex.NewScriptPath");
            
                AssetDatabase.MoveAsset(assetPath, newAssetPath);
                AssetDatabase.ImportAsset(newAssetPath, ImportAssetOptions.ForceSynchronousImport);

                var guid = AssetDatabase.AssetPathToGUID(newAssetPath);
                var defs = new Dictionary<string, string>() {
                    { "GUID", guid },
                };
                ScriptTemplates.Create(dir, assetName + "Feature.asset", "63-FeatureAsset", allowRename: false, customDefines: defs);

                ScriptTemplates.CreateEmptyDirectory(dir, "Modules");
                ScriptTemplates.CreateEmptyDirectory(dir, "Systems");
                ScriptTemplates.CreateEmptyDirectory(dir, "Components");
                ScriptTemplates.CreateEmptyDirectory(dir, "Markers");
                ScriptTemplates.CreateEmptyDirectory(dir, "Views");

            }
            
        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/Feature (Complex)", priority = ScriptTemplates.CREATE_FEATURE_COMPLEX_PRIORITY)]
        public static void CreateFeatureComplex() {

            ScriptTemplates.Create("New Feature.cs", "61-FeatureTemplate", onCreated: (asset) => {

                var path = ScriptTemplates.GetDirectoryFromAsset(asset);
                var assetName = asset.name;
                if (assetName.EndsWith("Feature") == true) assetName = assetName.Replace("Feature", string.Empty);
                ScriptTemplates.CreateEmptyDirectory(path, assetName);
                var dir = path + "/" + assetName;
                var newAssetPath = dir + "/" + assetName + "Feature.cs";

                EditorPrefs.SetBool("Temp.EditorWaitCompilation.CreateFeatureComplex", true);
                EditorPrefs.SetString("Temp.EditorWaitCompilation.CreateFeatureComplex.Dir", dir);
                EditorPrefs.SetString("Temp.EditorWaitCompilation.CreateFeatureComplex.Name", assetName);
                EditorPrefs.SetString("Temp.EditorWaitCompilation.CreateFeatureComplex.ScriptPath", AssetDatabase.GetAssetPath(asset));
                EditorPrefs.SetString("Temp.EditorWaitCompilation.CreateFeatureComplex.NewScriptPath", newAssetPath);
                
                //ScriptTemplates.CreateEmptyDirectory(dir, "Data");
                //ScriptTemplates.Create(dir + "/Data", "Feature" + assetName + "Data.cs", "62-FeatureData", allowRename: false);

                /*var featureName = assetName;
                var projectGuid = string.Empty;
                var projectPath = path.Replace("Assets/", "");
                var projectName = projectPath.Split('/')[0];
                var asmDefs = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset");
                foreach (var asmGuid in asmDefs) {

                    var asmPath = AssetDatabase.GUIDToAssetPath(asmGuid);
                    var asm = AssetDatabase.LoadAssetAtPath<UnityEditorInternal.AssemblyDefinitionAsset>(asmPath);
                    if (asm.name == projectName) {

                        projectGuid = asmGuid;
                        break;

                    }

                }

                var defs = new Dictionary<string, string>() {
                    { "PROJECTNAME", featureName },
                    { "REFERENCES", @",""GUID:" + projectGuid + @"""" }
                };
                ScriptTemplates.Create(dir, featureName + ".asmdef", "00-asmdef", customDefines: defs, allowRename: false);
                */
                
            });

        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/Component", priority = ScriptTemplates.CREATE_COMPONENT_STRUCT_PRIORITY)]
        public static void CreateStructComponent() {

            ScriptTemplates.Create("New Component.cs", "37-ComponentStructTemplate");

        }

        [UnityEditor.MenuItem("Assets/Create/ME.ECS/Component (Copyable)", priority = ScriptTemplates.CREATE_COMPONENT_STRUCT_PRIORITY)]
        public static void CreateStructCopyableComponent() {

            ScriptTemplates.Create("New Component.cs", "38-ComponentStructCopyableTemplate");

        }

        private static Object GetSelectedDirectory() {

            var guids = Selection.assetGUIDs;
            if (guids.Length == 0) return null;
            
            return AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guids[0]));

        }

        private static bool GetFeature(Object selected, out string featureName) {
            
            var path = AssetDatabase.GetAssetPath(selected);
            if (selected == null || string.IsNullOrEmpty(path) == true) {
                
                featureName = null;
                return false;
                
            }
            
            var dir = System.IO.Path.GetDirectoryName(path);

            if (dir.EndsWith("Systems") == true) {

                dir = dir.Remove(dir.Length - 7);

            }

            featureName = string.Empty;
            if (string.IsNullOrEmpty(dir) == false) {

                var files = System.IO.Directory.GetFiles(dir);
                foreach (var file in files) {

                    var ext = System.IO.Path.GetFileName(file);
                    var filename = System.IO.Path.GetFileNameWithoutExtension(file);
                    if (filename.EndsWith("Feature") == true && ext.EndsWith(".cs") == true) {

                        featureName = filename;
                        break;

                    }

                }

            }

            if (string.IsNullOrEmpty(featureName) == false) {
                    
                return true;
                    
            }

            return false;

        }

    }

}