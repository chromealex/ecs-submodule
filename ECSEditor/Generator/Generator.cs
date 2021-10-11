using System.Collections.Generic;
using System.Linq;
using Mono.Reflection;
using UnityEditor;

namespace ME.ECSEditor {

    public class Generator : AssetPostprocessor {

        private static string MENU_ITEM_AUTO;
        private static string CONTENT_ITEM;
        private static string CONTENT_ITEM2;
        private static string CONTENT_ITEM3;
        private static string FILE_NAME;
        private static string TEMPLATE;
        private static System.Type SEARCH_TYPE;
        private static string PREFS_KEY;
        private static string DIRECTORY_CONTAINS;
        private static bool AUTO_COMPILE_DEFAULT;

        protected static void Set(string MENU_ITEM_AUTO, string CONTENT_ITEM, string FILE_NAME, string TEMPLATE, System.Type SEARCH_TYPE, string PREFS_KEY,
                                  string DIRECTORY_CONTAINS, bool AUTO_COMPILE_DEFAULT, string CONTENT_ITEM2 = null, string CONTENT_ITEM3 = null) {

            Generator.MENU_ITEM_AUTO = MENU_ITEM_AUTO;
            Generator.CONTENT_ITEM = CONTENT_ITEM;
            Generator.CONTENT_ITEM2 = CONTENT_ITEM2;
            Generator.CONTENT_ITEM3 = CONTENT_ITEM3;
            Generator.FILE_NAME = FILE_NAME;
            Generator.TEMPLATE = TEMPLATE;
            Generator.SEARCH_TYPE = SEARCH_TYPE;
            Generator.PREFS_KEY = PREFS_KEY;
            Generator.DIRECTORY_CONTAINS = DIRECTORY_CONTAINS;
            Generator.AUTO_COMPILE_DEFAULT = AUTO_COMPILE_DEFAULT;

        }

        protected static void OnPostprocessAllAssetsGen(string[] importedAssets, string[] deletedAssets,
                                                        string[] movedAssets, string[] movedFromAssetPaths) {

            foreach (var importedAsset in deletedAssets) {

                if (importedAsset.EndsWith(".cs") == true && importedAsset.EndsWith(Generator.FILE_NAME) == false && importedAsset.Contains(Generator.DIRECTORY_CONTAINS) == true) {

                    Generator.OnAfterAssemblyReload(true);
                    return;

                }

            }

        }

        public static bool IsPrevTimeValid() {

            var time = EditorPrefs.GetFloat("ME.ECS.Generator.lastGenTime", 0f);
            var dt = (EditorApplication.timeSinceStartup - time);
            if (dt < 0f) {
                return true;
            }
            return dt > 10f;

        }

        public static void ApplyTime() {

            EditorPrefs.SetFloat("ME.ECS.Generator.lastGenTime", (float)EditorApplication.timeSinceStartup);
            
        }

        protected static void OnAfterAssemblyReload(bool delete) {

            if (Generator.IsPrevTimeValid() == false) {
                return;
            }

            if (Generator.IsAutoGenerateEnabled() == false) {
                return;
            }

            if (EditorApplication.isCompiling == true) {
                return;
            }

            var asms = UnityEditor.AssetDatabase.FindAssets("t:asmdef");
            foreach (var asm in asms) {

                var asmPath = UnityEditor.AssetDatabase.GUIDToAssetPath(asm);
                var asmNamePath = System.IO.Path.GetDirectoryName(asmPath).Replace("\\", "/");
                if (System.IO.Directory.Exists(asmNamePath) == false) continue;

                if (delete == true) {

                    var fullDir = asmNamePath + System.IO.Path.DirectorySeparatorChar + Generator.FILE_NAME;
                    if (System.IO.File.Exists(fullDir) == true) {

                        AssetDatabase.DeleteAsset(fullDir);
                        AssetDatabase.ImportAsset(fullDir, ImportAssetOptions.ForceUpdate);
                        AssetDatabase.Refresh();

                    }

                } else {

                    Generator.CompileDirectory(asmNamePath);

                }

            }
            
            //ME.ECS.DataConfigs.DataConfig.OnScriptsReloaded();

        }

        protected static bool AutoGenerateValidate() {

            var isEnabled = Generator.IsAutoGenerateEnabled();
            Menu.SetChecked(Generator.MENU_ITEM_AUTO, isEnabled);
            return true;

        }

        protected static bool IsAutoGenerateEnabled() {

            return EditorPrefs.GetBool(Generator.PREFS_KEY, Generator.AUTO_COMPILE_DEFAULT);

        }

        protected static void AutoGenerateCheck() {

            EditorPrefs.SetBool(Generator.PREFS_KEY, !Generator.IsAutoGenerateEnabled());
            //Menu.SetChecked(Generator.MENU_ITEM_AUTO, Generator.AutoGenerateValidate());

        }

        public static bool IsValidToCompile() {
            
            var obj = Selection.activeObject;
            if (obj == null) return false;
            
            string path = null;
            if (obj != null) {

                path = AssetDatabase.GetAssetPath(obj);
                if (System.IO.File.Exists(path) == true) {
                    path = System.IO.Path.GetDirectoryName(path);
                }

            }

            if (string.IsNullOrEmpty(path) == true) {
                path = "Assets/";
            }
            
            var asms = UnityEditor.AssetDatabase.FindAssets("t:asmdef", new[] { path });
            if (asms.Length == 0) return false;

            return true;

        }

        public static void Compile() {

            string path = null;
            var obj = Selection.activeObject;
            if (obj != null) {

                path = AssetDatabase.GetAssetPath(obj);
                if (System.IO.File.Exists(path) == true) {
                    path = System.IO.Path.GetDirectoryName(path);
                }

            }

            if (string.IsNullOrEmpty(path) == true) {
                path = "Assets/";
            }

            Generator.CompileDirectory(path);

        }

        [System.Serializable]
        public struct AsmDefAsset {

            public string name;
            public string[] references;

        }
        
        private static string[] GetReferences(UnityEditorInternal.AssemblyDefinitionAsset asset) {
            
            var data = UnityEngine.JsonUtility.FromJson<AsmDefAsset>(asset.text);
            var allAssembliesGUIDs = AssetDatabase.FindAssets("t:asmdef");
            if (data.references != null) {
                
                for (int i = 0; i < data.references.Length; ++i) {
                    
                    if (data.references[i].StartsWith("GUID:") == true) {
                    
                        data.references[i] = data.references[i].Replace("GUID:", string.Empty);
                        
                    } else {
                        
                        foreach (var assemblyGUID in allAssembliesGUIDs) {
                            
                            var asmdefPath = AssetDatabase.GUIDToAssetPath(assemblyGUID);
                            var definition = AssetDatabase.LoadAssetAtPath<UnityEditorInternal.AssemblyDefinitionAsset>(asmdefPath);
                            var smdefData = UnityEngine.JsonUtility.FromJson<AsmDefAsset>(definition.text);
                            if (smdefData.name == data.references[i]) {
                                
                                data.references[i] = assemblyGUID;
                                break;
                                
                            }
                            
                        }
                        
                    }
                    
                }
                
            }
            return data.references;
            
        }
        
        public static void CompileDirectory(string dir) {

            if (System.IO.Directory.Exists(dir) == false) return;
            
            var itemStr = Generator.CONTENT_ITEM;
            var itemStr2 = Generator.CONTENT_ITEM2;
            var itemStr3 = Generator.CONTENT_ITEM3;

            var splittedMain = dir.Split(System.IO.Path.DirectorySeparatorChar);
            var asmNameMain  = splittedMain[splittedMain.Length - 1];

            var listEntities = new List<System.Type>();
            var asms = UnityEditor.AssetDatabase.FindAssets("t:asmdef", new[] { dir });
            foreach (var asm in asms) {

                var output = string.Empty;
                var output2 = string.Empty;
                var output3 = string.Empty;
                listEntities.Clear();

                var asmPath = UnityEditor.AssetDatabase.GUIDToAssetPath(asm);
                var asmNamePath = System.IO.Path.GetDirectoryName(asmPath);
                if (System.IO.Directory.Exists(asmNamePath) == false) continue;

                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditorInternal.AssemblyDefinitionAsset>(asmPath);
                asmNameMain = asset.name;

                var splitted = asmNamePath.Split(System.IO.Path.DirectorySeparatorChar);
                var asmName  = splitted[splitted.Length - 1];

                var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                var allAsms = new HashSet<string>();
                System.Reflection.Assembly mainAsm = null;
                
                void Collect(UnityEditorInternal.AssemblyDefinitionAsset asmInner) {
                    
                    var refs = Generator.GetReferences(asmInner);
                    if (refs != null) {

                        foreach (var rGuid in refs) {

                            if (string.IsNullOrEmpty(rGuid) == true) continue;

                            var asmPathR = UnityEditor.AssetDatabase.GUIDToAssetPath(rGuid);
                            var assetR = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditorInternal.AssemblyDefinitionAsset>(asmPathR);
                            if (assetR == null) continue;

                            if (allAsms.Contains(assetR.name) == false) {

                                allAsms.Add(assetR.name);

                            }

                        }

                    }

                }

                foreach (var assembly in assemblies) {

                    if (assembly.GetName().Name == asmNameMain) {

                        mainAsm = assembly;
                        
                        allAsms.Add(mainAsm.GetName().Name);

                        Collect(asset);
                        break;

                    }

                }

                if (mainAsm == null) {
                    
                    //UnityEngine.Debug.LogWarning("Assembly with the name " + asmNameMain + " was not found in directory " + dir);
                    return;

                }

                /*ComponentIndexGeneratorData componentIndex = null;
                if (System.IO.Directory.Exists(asmNamePath + "/gen") == true) {

                    componentIndex = ComponentIndexGeneratorData.Generate(asmNamePath);
                    if (componentIndex == null) {
	                    
	                    UnityEngine.Debug.LogError($"Error while creating ComponentIndexGeneratorData in {asmNamePath}/gen");
	                    return;
	                    
                    }
                    componentIndex.ResetCurrent();

                } else {

                    return;

                }*/

                foreach (var assembly in assemblies) {

                    if (allAsms.Contains(assembly.GetName().Name) == true && assembly.GetName().Name != "ECSAssembly") {

                        var allTypes = assembly.GetTypes();
                        foreach (var type in allTypes) {

                            if (type.IsInterface == true) continue;

                            var interfaces = type.GetInterfaces();
                            foreach (var @interface in interfaces) {

                                if (@interface.IsAssignableFrom(Generator.SEARCH_TYPE) == true) {

                                    if (listEntities.Contains(type) == false) {
                                        
                                        listEntities.Add(type);
                                        //componentIndex.SetStruct(type);
                                        
                                    }

                                }

                            }

                        }

                        //break;

                    }

                }

                listEntities = listEntities.OrderBy(x => {
                    
                    var attrs = x.GetCustomAttributes(typeof(ME.ECS.ComponentOrderAttribute), false);
                    if (attrs.Length > 0) {

                        return (attrs[0] as ME.ECS.ComponentOrderAttribute).order;

                    }
                    
                    return 0;
                    
                }).ThenBy(x => x.FullName).ToList();

                var linesOutput = new List<string>(100);
                var linesOutput2 = new List<string>(100);
                var linesOutput3 = new List<string>(100);
                for (var i = 0; i < listEntities.Count; ++i) {
                    
                    var type = listEntities[i];
                    
                    var entityType = type.FullName.Replace("+", ".");
                    var hasFields = type.GetFields(System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Length > 0;
                    var isCopyable = typeof(ME.ECS.IStructCopyableBase).IsAssignableFrom(type);
                    var isStatic = typeof(ME.ECS.IComponentStatic).IsAssignableFrom(type);
                    var isDisposable = typeof(ME.ECS.IComponentDisposable).IsAssignableFrom(type);
                    var isOneShot = typeof(ME.ECS.IComponentOneShot).IsAssignableFrom(type);
                    var isShared = typeof(ME.ECS.IComponentShared).IsAssignableFrom(type);
                    var isVersioned = typeof(ME.ECS.IVersioned).IsAssignableFrom(type);
                    var isVersionedNoState = typeof(ME.ECS.IVersionedNoState).IsAssignableFrom(type);

                    if (isCopyable == false && hasFields == true && isStatic == false) {
                        
                        // Check for managed types
                        if (Generator.HasManagedTypes(type, out var failedFieldInfo) == true) {
                            
                            UnityEngine.Debug.LogError($"[ME.ECS] Generator for type `{type}` failed because it is not blittable (field `{failedFieldInfo.Name}`). Use IStructCopyable to create manual copy.");
                            
                        }

                    }
                    
                    var resItem = itemStr;
                    resItem = resItem.Replace("#ISTAG#", hasFields == true ? "false" : "true");
                    resItem = resItem.Replace("#ISSHARED#", isShared == true ? "true" : "false");
                    resItem = resItem.Replace("#TYPENAME#", entityType);
                    resItem = resItem.Replace("#COPYABLE#", isCopyable == true ? "Copyable" : "");
                    resItem = resItem.Replace("#DISPOSABLE#", isDisposable == true ? "Disposable" : "");
                    resItem = resItem.Replace("#ONESHOT#", isOneShot == true ? "OneShot" : "");
                    resItem = resItem.Replace("\r\n", "\n");
                    
                    /*
                    resItem = resItem.Replace("#PROJECTNAME#", asmName);
                    resItem = resItem.Replace("#STATENAME#", asmName + "State");
                    resItem = resItem.Replace("#ISVERSIONED#", isVersioned == true ? "true" : "false");
                    resItem = resItem.Replace("#ISVERSIONED_NOSTATE#", isVersionedNoState == true ? "true" : "false");
                    resItem = resItem.Replace("#ISCOPYABLE#", isCopyable == true ? "true" : "false");
                    resItem = resItem.Replace("#ISDISPOSABLE#", isDisposable == true ? "true" : "false");
                    */
                    
                    linesOutput.Add(resItem);

                    if (itemStr2 != null) {

                        var resItem2 = itemStr2;
                        resItem2 = resItem2.Replace("#TYPENAME#", entityType);
                        resItem2 = resItem2.Replace("#ISTAG#", hasFields == true ? "false" : "true");
                        resItem2 = resItem2.Replace("#ISSHARED#", isShared == true ? "true" : "false");
                        resItem2 = resItem2.Replace("#COPYABLE#", isCopyable == true ? "Copyable" : "");
                        resItem2 = resItem2.Replace("#DISPOSABLE#", isDisposable == true ? "Disposable" : "");
                        resItem2 = resItem2.Replace("#ONESHOT#", isOneShot == true ? "OneShot" : "");
                        resItem2 = resItem2.Replace("\r\n", "\n");
                        
                        /*
                        resItem2 = resItem2.Replace("#PROJECTNAME#", asmName);
                        resItem2 = resItem2.Replace("#STATENAME#", asmName + "State");
                        resItem2 = resItem2.Replace("#ISVERSIONED#", isVersioned == true ? "true" : "false");
                        resItem2 = resItem2.Replace("#ISVERSIONED_NOSTATE#", isVersionedNoState == true ? "true" : "false");
                        resItem2 = resItem2.Replace("#ISCOPYABLE#", isCopyable == true ? "true" : "false");
                        resItem2 = resItem2.Replace("#ISDISPOSABLE#", isDisposable == true ? "true" : "false");
                        */
                        
                        linesOutput2.Add(resItem2);

                    }

                    if (itemStr3 != null) {

                        var resItem3 = itemStr3;
                        resItem3 = resItem3.Replace("#TYPENAME#", entityType);
                        resItem3 = resItem3.Replace("#ISTAG#", hasFields == true ? "false" : "true");
                        resItem3 = resItem3.Replace("#ISSHARED#", isShared == true ? "true" : "false");
                        resItem3 = resItem3.Replace("#ISCOPYABLE#", isCopyable == true ? "true" : "false");
                        resItem3 = resItem3.Replace("#ISDISPOSABLE#", isDisposable == true ? "true" : "false");
                        resItem3 = resItem3.Replace("#ISONESHOT#", isOneShot == true ? "true" : "false");
                        resItem3 = resItem3.Replace("#ISVERSIONED#", isVersioned == true ? "true" : "false");
                        resItem3 = resItem3.Replace("#ISVERSIONED_NOSTATE#", isVersionedNoState == true ? "true" : "false");
                        resItem3 = resItem3.Replace("\r\n", "\n");

                        /*
                        resItem3 = resItem3.Replace("#PROJECTNAME#", asmName);
                        resItem3 = resItem3.Replace("#STATENAME#", asmName + "State");
                        resItem3 = resItem3.Replace("#COPYABLE#", isCopyable == true ? "Copyable" : "");
                        resItem3 = resItem3.Replace("#DISPOSABLE#", isDisposable == true ? "Disposable" : "");
                        */

                        linesOutput3.Add(resItem3);

                    }
                    
                }

                output = string.Join(string.Empty, linesOutput);
                output2 = string.Join(string.Empty, linesOutput2);
                output3 = string.Join(string.Empty, linesOutput3);
                
                if (ME.ECSEditor.ScriptTemplates.Create(asmNamePath, Generator.FILE_NAME, Generator.TEMPLATE,
                                                        new Dictionary<string, string>() { { "CONTENT", output }, { "CONTENT2", output2 }, { "CONTENT3", output3 } }, false) == true) {

                    UnityEngine.Debug.Log($"{Generator.FILE_NAME} successfully refreshed at path {asmNamePath}");

                }

            }

            Generator.ApplyTime();

        }

        private static bool HasManagedTypes(System.Type type, out System.Reflection.FieldInfo failedFieldInfo) {

            failedFieldInfo = null;
            //if (Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable(type) == true) return false;

            var fields = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var field in fields) {

                var attrs = field.GetCustomAttributes(typeof(ME.ECS.GeneratorIgnoreManagedType), true);
                if (attrs.Length > 0) continue;

                var itemType = field.FieldType;
                if (itemType == typeof(string)) continue;
                
                attrs = itemType.GetCustomAttributes(typeof(ME.ECS.GeneratorIgnoreManagedType), true);
                if (attrs.Length > 0) continue;
                
                if (itemType.IsClass == true ||
                    itemType.IsInterface == true ||
                    itemType.IsArrayOrList() == true) {
                    
                    if (typeof(UnityEngine.Object).IsAssignableFrom(itemType) == true) continue;

                    failedFieldInfo = field;
                    return true;
                    
                } else {

                    if (Generator.HasManagedTypes(itemType, out failedFieldInfo) == true) return true;

                }

            }
            
            return false;

        }

    }

}
