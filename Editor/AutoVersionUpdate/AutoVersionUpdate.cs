using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace ME.ECSEditor {

    [InitializeOnLoad]
    public static class AutoVersionUpdateCompilation {

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

        private static bool IsUserEditor(string username, out string commitName) {
            
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