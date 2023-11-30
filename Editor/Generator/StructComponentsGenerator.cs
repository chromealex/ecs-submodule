using System.Collections.Generic;
using UnityEditor;

namespace ME.ECSEditor {

    [InitializeOnLoad]
    public class StructComponentsGenerator : Generator {

        private const string FILE_NAME = "gen/compiler.gen.structcomponents.cs";
        private const string MENU_ITEM = "ME.ECS/Generators/Struct Components/Compile...";
        private const string MENU_ITEM_AUTO = "ME.ECS/Generators/Struct Components/Auto Compile";
        private const string PREFS_KEY = "ME.ECS.Compiler.Gen.StructComponents.Auto";
        private const string TEMPLATE = "00-StructComponents";
        private const string DIRECTORY_CONTAINS = "/Components/";
        private static readonly System.Type SEARCH_TYPE = typeof(ME.ECS.IComponentBase);
        private const string CONTENT_ITEM = @"
            #CONTAINER#.Validate#BLITTABLE##COPYABLE##ONESHOT##TAG##UNMANAGED##DISPOSABLE#<#TYPENAME#>(#ALLOCATOR_FIRST_TYPE##ISTAG#);";
        private const string CONTENT_ITEM2 = @"
            entity.ValidateData#BLITTABLE##COPYABLE##ONESHOT##TAG##UNMANAGED##DISPOSABLE#<#TYPENAME#>(#ISTAG#);";
        private const string CONTENT_ITEM3 = @"
            WorldUtilities.InitComponentTypeId<#TYPENAME#>(#ISTAG#, #ISSIMPLE#, #ISBLITTABLE#, #ISDISPOSABLE#, #ISCOPYABLE#, #ISVERSIONED#, #ISVERSIONED_NOSTATE#, #ISSHARED#, #ISONESHOT#, #COPYABLE_UNMANAGED#);";
        private const bool AUTO_COMPILE_DEFAULT = true;

        static StructComponentsGenerator() {

            AssemblyReloadEvents.afterAssemblyReload += StructComponentsGenerator.OnAfterAssemblyReload;
            UnityEditor.Compilation.CompilationPipeline.assemblyCompilationFinished += StructComponentsGenerator.AssemblyCompilationFinishedEventHandler;
            
            if (StructComponentsGenerator.IsFirstLaunch() == true) {

                StructComponentsGenerator.SetFirstLaunch();
                StructComponentsGenerator.OnAfterAssemblyReload();

            }

        }

        private static void AssemblyCompilationFinishedEventHandler(string output, UnityEditor.Compilation.CompilerMessage[] messages) {

            var filename = System.IO.Path.GetFileName(StructComponentsGenerator.FILE_NAME);
            foreach (var message in messages) {
                
                if (message.type != UnityEditor.Compilation.CompilerMessageType.Error || message.file.Contains(filename) == false) continue;
                Generator.OnAfterAssemblyReload(true, true);
                return;
                
            }
            
        }

        public static void Init() {
            
            Generator.Set(
                StructComponentsGenerator.MENU_ITEM_AUTO,
                StructComponentsGenerator.CONTENT_ITEM,
                StructComponentsGenerator.FILE_NAME,
                StructComponentsGenerator.TEMPLATE,
                StructComponentsGenerator.SEARCH_TYPE,
                StructComponentsGenerator.PREFS_KEY,
                StructComponentsGenerator.DIRECTORY_CONTAINS,
                StructComponentsGenerator.AUTO_COMPILE_DEFAULT,
                StructComponentsGenerator.CONTENT_ITEM2,
                StructComponentsGenerator.CONTENT_ITEM3);
            
        }

        public static bool IsFirstLaunch() {

            return SessionState.GetBool("StructComponentsGenerator.IsFirstLaunch", true);

        }

        public static void SetFirstLaunch() {

            SessionState.SetBool("StructComponentsGenerator.IsFirstLaunch", false);

        }

        public static void OnAfterAssemblyReload() {
            
            StructComponentsGenerator.Init();
            Generator.OnAfterAssemblyReload(false);

        }

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {

            StructComponentsGenerator.Init();
            Generator.OnPostprocessAllAssetsGen(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
            
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {

            if (EditorApplication.isPlayingOrWillChangePlaymode == true) return;
            
            StructComponentsGenerator.Init();
            Generator.OnAfterAssemblyReload(false);
            
        }

        [UnityEditor.MenuItem(StructComponentsGenerator.MENU_ITEM_AUTO, true)]
        public static bool AutoGenerateValidateFunc() {
            
            StructComponentsGenerator.Init();
            return Generator.AutoGenerateValidate();

        }

        [UnityEditor.MenuItem(StructComponentsGenerator.MENU_ITEM_AUTO)]
        public static void AutoGenerate() {
            
            StructComponentsGenerator.Init();
            Generator.AutoGenerateCheck();
            
        }

        [UnityEditor.MenuItem(StructComponentsGenerator.MENU_ITEM, true)]
        public static bool CompileMenuValidate() {
            
            StructComponentsGenerator.Init();
            return Generator.IsValidToCompile();

        }

        [UnityEditor.MenuItem(StructComponentsGenerator.MENU_ITEM)]
        public static void CompileMenu() {
            
            StructComponentsGenerator.Init();
            Generator.Compile();
            
        }

    }

}