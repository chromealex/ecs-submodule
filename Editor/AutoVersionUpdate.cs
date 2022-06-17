using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace ME.ECSEditor {

    [InitializeOnLoad]
    public class AutoVersionUpdate : AssetPostprocessor {

        private static UnityEditor.Compilation.Assembly[] assemblies;
        
        static AutoVersionUpdate() {
            
            AutoVersionUpdate.assemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies();
            
        }
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {

            // Check if mac os x
            #if !UNITY_EDITOR_OSX
            return;
            #endif
            
            // Check if its me ;)
            var dir = System.IO.Directory.Exists("/Users/aleksandrfeer/Projects");
            if (dir == false) return;
            
            var assetReimport = string.Empty;
            var found = false;
            foreach (var asset in importedAssets) {

                if (asset.EndsWith(".cs") == true) {

                    assetReimport = asset;
                    found = true;
                    break;

                }
                
            }

            if (found == true) {

                UnityEditor.Compilation.CompilationPipeline.assemblyCompilationFinished += (string assemblyPath, UnityEditor.Compilation.CompilerMessage[] messages) => {

                    var assembly = AutoVersionUpdate.assemblies.FirstOrDefault(a => a.outputPath == assemblyPath);
                    if (assembly == null) return;

                    foreach (var src in assembly.sourceFiles) {

                        if (src == assetReimport) {

                            AutoVersionUpdate.UpdatePackageVersion();
                            break;

                        }

                    }

                };

            }

        }

        private static void UpdatePackageVersion() {

            var package = EditorUtilities.Load<TextAsset>("package.json", out var realPath);
            if (package != null) {

                var source = package.text;
                var pattern = @"""version"":\s*""(?<major>\d{1,2}).(?<minor>\d{1,2}).(?<build>\d{1,2})""";      
                var result = Regex.Replace(source, pattern, AutoVersionUpdate.ReplaceEvaluator);
                System.IO.File.WriteAllText(realPath, result);

            }

        }
        
        static string ReplaceEvaluator(System.Text.RegularExpressions.Match m) {
            int major = int.Parse(m.Groups["major"].Value);
            int minor = int.Parse(m.Groups["minor"].Value);
            int build = int.Parse(m.Groups["build"].Value) + 1;
            Debug.Log($"Version up to {major}.{minor}.{build}");
            return @"""version"": """ + major.ToString() + "." + minor.ToString() + "." + build.ToString() + @"""";
        } 

    }

}