using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

namespace ME.ECSEditor {

    [InitializeOnLoad]
    public static class AutoVersionUpdateCompilation {

        static System.Type m_toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        static System.Type m_guiViewType = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");
        static System.Type m_iWindowBackendType = typeof(Editor).Assembly.GetType("UnityEditor.IWindowBackend");
        static System.Reflection.PropertyInfo m_windowBackend = m_guiViewType.GetProperty("windowBackend",
                                                                                          System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        static System.Reflection.PropertyInfo m_viewVisualTree = m_iWindowBackendType.GetProperty("visualTree",
                                                                                                  System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        static ScriptableObject m_currentToolbar;
        
        internal static UnityEditor.Compilation.Assembly[] assemblies;
        internal static string currentVersion;

        internal static UnityEngine.Networking.UnityWebRequest request;
        internal static bool isInitialized;
        
        static AutoVersionUpdateCompilation() {

            EditorApplication.delayCall += () => SessionState.SetBool("AutoVersionUpdate.initialized", true);

            var con = SessionState.GetString("AutoVersionUpdate.Contributors", string.Empty);
            if (string.IsNullOrEmpty(con) == false) return;
            
            var url = "https://raw.githubusercontent.com/chromealex/ecs/master/Docs/contributors.txt";
            //Debug.Log("Request contributors");
            var www = UnityEngine.Networking.UnityWebRequest.Get(url);
            www.SendWebRequest();
            AutoVersionUpdateCompilation.request = www;
            
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
            
        }

        private static void OnUpdate() {
            
            AutoVersionUpdateCompilation.AddButton();
            
        }

        private static void AddButton() {
            
            #if UNITY_2021_OR_NEWER
            var username = UnityEditor.CloudProjectSettings.userName;
            if (AutoVersionUpdate.IsUserEditor(username, out var commitName) == false) return;
            
            if (m_currentToolbar == null)
            {
                // Find toolbar
                var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);
                m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject) toolbars[0] : null;
                if (m_currentToolbar != null) {
                    var root = m_currentToolbar.GetType().GetField("m_Root", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var rawRoot = root.GetValue(m_currentToolbar);
                    var mRoot = rawRoot as VisualElement;
                    var element = mRoot.Q("ToolbarZoneRightAlign");
                    {
                        var uss = EditorUtilities.Load<StyleSheet>("Editor/AutoVersionUpdate/styles.uss");
                        element.styleSheets.Add(uss);
                        var dropdown = new DropdownField();
                        dropdown.value = "ME.ECS Collab";
                        dropdown.AddToClassList("autoupdate-button");
                        dropdown.RemoveFromClassList("unity-button");
                        dropdown.RemoveFromClassList("unity-base-field");
                        dropdown.Q(className:"unity-base-field__input").RemoveFromClassList("unity-base-field__input");
                        dropdown.RegisterCallback<ClickEvent>(evt => {

                            ChangeLogEditorWindow.CreateAfterCompilation(dropdown.worldBound);

                        });
                        element.Add(dropdown);
                    }
                }
            }
            #endif

        }

        internal static void UpdatePackageVersion(string[] filepaths, string commitName) {
            
            var package = EditorUtilities.Load<TextAsset>("package.json", out var realPath);
            if (package != null) {

                var files = new string[filepaths.Length];
                var rootDir = System.IO.Path.GetDirectoryName(realPath);
                for (int i = 0; i < filepaths.Length; ++i) {
                    var d = System.IO.Path.GetDirectoryName(filepaths[i]);
                    var dir = d.Substring(rootDir.Length, d.Length - rootDir.Length);
                    var splitted = dir.Split('/');
                    if (splitted.Length > 1) {
                        files[i] = splitted[1] + "." + splitted[2]; //System.IO.Path.GetFileNameWithoutExtension(filepaths[i]);
                    } else {
                        files[i] = splitted[0];
                    }
                }

                var source = package.text;
                var pattern = @"""version"":\s*""(?<major>[0-9]+).(?<minor>[0-9]+).(?<build>[0-9]+)""";
                var result = Regex.Replace(source, pattern, AutoVersionUpdateCompilation.UpBuild);
                ChangeLogEditorWindow.Create(files, commitName, currentVersion, realPath, source, result);
                AutoVersionUpdateCompilation.Callback(currentVersion, currentVersion, realPath, source, result);
                
            }

        }

        public static void Callback(string sourceVersion, string ver, string realPath, string source, string text) {

            var sourceVersionCheck = string.Join(".", sourceVersion.Split('.').Take(2).ToArray());
            var verCheck = string.Join(".", ver.Split('.').Take(2).ToArray());
            
            Debug.Log($"Version up {sourceVersion} => {ver}");
            if (sourceVersionCheck == verCheck) {
                // major/minor not changed
                System.IO.File.WriteAllText(realPath, text);
            } else {
                // version changed
                newMajorMinorVersion = ver;
                var pattern = @"""version"":\s*""(?<major>[0-9]+).(?<minor>[0-9]+).(?<build>[0-9]+)""";
                var newResult = Regex.Replace(source, pattern, AutoVersionUpdateCompilation.SetMajorMinorVersion);
                System.IO.File.WriteAllText(realPath, newResult);
            }

        }

        private static string newMajorMinorVersion;
        private static string SetMajorMinorVersion(System.Text.RegularExpressions.Match m) {
            return @"""version"": """ + AutoVersionUpdateCompilation.newMajorMinorVersion + @"""";
        } 

        private static string UpBuild(System.Text.RegularExpressions.Match m) {
            int major = int.Parse(m.Groups["major"].Value);
            int minor = int.Parse(m.Groups["minor"].Value);
            int build = int.Parse(m.Groups["build"].Value) + 1;
            currentVersion = $"{major}.{minor}.{build}";
            return @"""version"": """ + major.ToString() + "." + minor.ToString() + "." + build.ToString() + @"""";
        } 

    }
    
    public class AutoVersionUpdate : AssetPostprocessor {

        internal static bool IsUserEditor(string username, out string commitName) {
            
            // Check if its me ;)
            //var dir = System.IO.Directory.Exists("/Users/aleksandrfeer/Projects");
            //if (dir == true) return true;

            commitName = string.Empty;
            var con = SessionState.GetString("AutoVersionUpdate.Contributors", string.Empty);
            if (string.IsNullOrEmpty(con) == true) {
                
                if (AutoVersionUpdateCompilation.request.isDone == false) return false;
                
                var text = AutoVersionUpdateCompilation.request.downloadHandler.text;
                SessionState.SetString("AutoVersionUpdate.Contributors", con);

                con = text;

            }

            var lines = con.Split('\n');
            for (int i = 0; i < lines.Length; ++i) {
                var spl = lines[i].Split(' ');
                if (username == spl[0]) {
                    if (spl.Length > 1) commitName = spl[1];
                    return true;
                }
            }

            return false;

        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {

            if (SessionState.GetBool("AutoVersionUpdate.initialized", false) == false) return;
            
            var username = UnityEditor.CloudProjectSettings.userName;
            if (IsUserEditor(username, out var commitName) == false) return;
            
            if (AutoVersionUpdateCompilation.assemblies == null) AutoVersionUpdateCompilation.assemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies();

            var assetReimport = new System.Collections.Generic.List<string>();
            var found = false;
            foreach (var asset in importedAssets) {

                if (asset.EndsWith(".cs") == true) {

                    var assembly = AutoVersionUpdateCompilation.assemblies.FirstOrDefault(a => a.name.Contains("ME.ECS") == true && a.sourceFiles.Contains(asset) == true);
                    if (assembly != null) {

                        assetReimport.Add(asset);
                        found = true;
                    
                    }

                }
            
            }
            
            if (found == true) AutoVersionUpdateCompilation.UpdatePackageVersion(assetReimport.ToArray(), commitName);
            
        }

    }

}