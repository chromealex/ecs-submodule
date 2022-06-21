using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace ME.ECSEditor {

    [InitializeOnLoad]
    public static class AutoVersionUpdateCompilation {

        private static UnityEditor.Compilation.Assembly[] assemblies;

        static AutoVersionUpdateCompilation() {
            UnityEditor.Compilation.CompilationPipeline.assemblyCompilationFinished += AssemblyCompilationFinished;
        }

        private static void AssemblyCompilationFinished(string assemblyPath, UnityEditor.Compilation.CompilerMessage[] messages) {
            
            // Check if mac os x
            #if !UNITY_EDITOR_OSX
            return;
            #endif
            
            // Check if its me ;)
            var dir = System.IO.Directory.Exists("/Users/aleksandrfeer/Projects");
            if (dir == false) return;

            var found = EditorPrefs.GetBool("LastCompilation.AutoVersionUpdate.Result", false);
            var assetReimport = EditorPrefs.GetString("LastCompilation.AutoVersionUpdate.File", string.Empty);

            if (AutoVersionUpdateCompilation.assemblies == null) AutoVersionUpdateCompilation.assemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies();
            
            if (found == true) {

                var assembly = AutoVersionUpdateCompilation.assemblies.FirstOrDefault(a => a.outputPath == assemblyPath);
                if (assembly == null) return;

                foreach (var src in assembly.sourceFiles) {

                    if (src == assetReimport) {

                        AutoVersionUpdateCompilation.UpdatePackageVersion();
                        break;

                    }

                }

            }
            
        }

        private static void UpdatePackageVersion() {

            var package = EditorUtilities.Load<TextAsset>("package.json", out var realPath);
            if (package != null) {

                var source = package.text;
                var pattern = @"""version"":\s*""(?<major>\d{1,2}).(?<minor>\d{1,2}).(?<build>\d{1|2|3|4})""";      
                var result = Regex.Replace(source, pattern, AutoVersionUpdateCompilation.ReplaceEvaluator);
                System.IO.File.WriteAllText(realPath, result);

            }

        }
        
        private static string ReplaceEvaluator(System.Text.RegularExpressions.Match m) {
            int major = int.Parse(m.Groups["major"].Value);
            int minor = int.Parse(m.Groups["minor"].Value);
            int build = int.Parse(m.Groups["build"].Value) + 1;
            Debug.Log($"Version up to {major}.{minor}.{build}");
            return @"""version"": """ + major.ToString() + "." + minor.ToString() + "." + build.ToString() + @"""";
        } 

    }
    
    public class AutoVersionUpdate : AssetPostprocessor {

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

            EditorPrefs.SetBool("LastCompilation.AutoVersionUpdate.Result", found);
            EditorPrefs.SetString("LastCompilation.AutoVersionUpdate.File", assetReimport);
            
        }

    }

}