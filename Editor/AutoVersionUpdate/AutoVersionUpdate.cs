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
        
        static AutoVersionUpdateCompilation() {

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
                    var dir = System.IO.Path.GetDirectoryName(filepaths[i]).Substring(0, rootDir.Length);
                    var splitted = dir.Split('/');
                    files[i] = splitted[0] + "." + splitted[1]; //System.IO.Path.GetFileNameWithoutExtension(filepaths[i]);
                }

                var source = package.text;
                var pattern = @"""version"":\s*""(?<major>\d{1,2}).(?<minor>\d{1,2}).(?<build>\d{1,2})""";
                var result = Regex.Replace(source, pattern, AutoVersionUpdateCompilation.UpBuild);
                ChangeLogEditorWindow.Create(files, commitName, currentVersion, (ver) => {
                
                    Debug.Log($"Version up to {ver}");
                    if (currentVersion.StartsWith(ver) == true) {
                        // major/minor not changed
                        System.IO.File.WriteAllText(realPath, result);
                    } else {
                        // version changed
                        newMajorMinorVersion = ver;
                        var newResult = Regex.Replace(source, pattern, AutoVersionUpdateCompilation.SetMajorMinorVersion);
                        System.IO.File.WriteAllText(realPath, newResult);
                    }

                });
                
            }

        }

        private static string newMajorMinorVersion;
        private static string SetMajorMinorVersion(System.Text.RegularExpressions.Match m) {
            return @"""version"": """ + AutoVersionUpdateCompilation.newMajorMinorVersion + ".0";
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