using System.Collections.Generic;
using UnityEditor;

namespace ME.ECSEditor {

    public class FilterExtensionsGenerator : AssetPostprocessor {

        [UnityEditor.MenuItem("ME.ECS/Generators/Generate Filters...")]
        public static void GenerateFilters() {
            
            var asms = UnityEditor.AssetDatabase.FindAssets("t:asmdef ECSAssembly");
            foreach (var asm in asms) {

                var asset = UnityEditor.AssetDatabase.GUIDToAssetPath(asm);
                var dir = $"{System.IO.Path.GetDirectoryName(asset)}/Core/Filters/CodeGenerator";
                if (System.IO.Directory.Exists(dir) == false) continue;

                var outputDelegates = string.Empty;
                var outputForEach = string.Empty;
                var buffers = string.Empty;
                
                var count = 10;
                for (int j = 1; j < count; ++j) {

                    var itemsType = "T0";
                    for (int i = 1; i < j; ++i) {

                        itemsType += $",T{i}";

                    }

                    var items = "ref T0 t0";
                    for (int i = 1; i < j; ++i) {

                        items += $", ref T{i} t{i}";

                    }

                    {
                        var res = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsDelegateItem.txt", isRequired: true).text;
                        res = res.Replace("#ITEMS_TYPE#", itemsType);
                        res = res.Replace("#ITEMS#", items);
                        res = res.Replace("#INDEX#", j.ToString());
                        outputDelegates += $"{res}\n";
                    }
                    
                    {
                    
                        var itemsWhere = " where T0:struct,IStructComponentBase";
                        for (int i = 1; i < j; ++i) {

                            itemsWhere += $" where T{i}:struct,IStructComponentBase";

                        }

                        var itemsGet = "ref buffer.GetT0(id)";
                        for (int i = 1; i < j; ++i) {

                            itemsGet += $", ref buffer.GetT{i}(id)";

                        }

                        var res = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsForEachItem.txt", isRequired: true).text;
                        res = res.Replace("#ITEMS_TYPE#", itemsType);
                        res = res.Replace("#ITEMS_WHERE#", itemsWhere);
                        res = res.Replace("#ITEMS_GET#", itemsGet);
                        res = res.Replace("#INDEX#", j.ToString());
                        outputForEach += $"{res}\n";
                        
                    }
                    
                    {
                    
                        var itemMethods = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferMethods.txt", isRequired: true).text;
                        var itemsMethods = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            var text = itemMethods;
                            text = text.Replace("#INDEX#", i.ToString());
                            itemsMethods += text;

                        }
                        
                        var dataBufferContains = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferDataBufferContains.txt", isRequired: true).text;
                        var dataBufferContainsOutput = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            var text = dataBufferContains;
                            text = text.Replace("#INDEX#", i.ToString());
                            dataBufferContainsOutput += text;

                        }

                        var dataBufferOps = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferDataBufferOps.txt", isRequired: true).text;
                        var dataBufferOpsOutput = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            var text = dataBufferOps;
                            text = text.Replace("#INDEX#", i.ToString());
                            dataBufferOpsOutput += text;

                        }

                        var dataBufferData = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferDataBufferData.txt", isRequired: true).text;
                        var dataBufferDataOutput = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            var text = dataBufferData;
                            text = text.Replace("#INDEX#", i.ToString());
                            dataBufferDataOutput += text;

                        }

                        var regsInit = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferRegsInit.txt", isRequired: true).text;
                        var regsInitOutput = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            var text = regsInit;
                            text = text.Replace("#INDEX#", i.ToString());
                            regsInitOutput += text;

                        }

                        var regsInitFill = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferRegsInitFill.txt", isRequired: true).text;
                        var regsInitFillOutput = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            var text = regsInitFill;
                            text = text.Replace("#INDEX#", i.ToString());
                            regsInitFillOutput += text;

                        }

                        var pushRegsInit = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferPushRegsInit.txt", isRequired: true).text;
                        var pushRegsInitOutput = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            var text = pushRegsInit;
                            text = text.Replace("#INDEX#", i.ToString());
                            pushRegsInitOutput += text;

                        }

                        var pushOps = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferPushOps.txt", isRequired: true).text;
                        var pushOpsOutput = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            var text = pushOps;
                            text = text.Replace("#INDEX#", i.ToString());
                            pushOpsOutput += text;

                        }

                        var itemsWhere = " where T0:struct,IStructComponentBase";
                        for (int i = 1; i < j; ++i) {

                            itemsWhere += $" where T{i}:struct,IStructComponentBase";

                        }
        
                        var res = EditorUtilities.Load<UnityEngine.TextAsset>("ECSEditor/Templates/EditorResources/00-FilterExtensionsBufferItem.txt", isRequired: true).text;
                        res = res.Replace("#ITEMS_TYPE#", itemsType);
                        res = res.Replace("#ITEMS_WHERE#", itemsWhere);
                        res = res.Replace("#ITEMS_METHODS#", itemsMethods);
                        
                        res = res.Replace("#DATABUFFER_CONTAINS#", dataBufferContainsOutput);
                        res = res.Replace("#DATABUFFER_OPS#", dataBufferOpsOutput);
                        res = res.Replace("#DATABUFFER_DATA#", dataBufferDataOutput);
                        res = res.Replace("#REGS_INIT#", regsInitOutput);
                        res = res.Replace("#REGS_FILL#", regsInitFillOutput);
                        res = res.Replace("#PUSH_REGS_INIT#", pushRegsInitOutput);
                        res = res.Replace("#PUSH_OPS#", pushOpsOutput);
                        
                        res = res.Replace("#INDEX#", j.ToString());
                        buffers += $"{res}\n";
                        
                    }

                }
                
                if (string.IsNullOrEmpty(outputDelegates) == false) ME.ECSEditor.ScriptTemplates.Create(dir, "Filters.Delegates.gen.cs", "00-FilterExtensionsDelegates", new Dictionary<string, string>() { { "CONTENT", outputDelegates } }, allowRename: false);
                if (string.IsNullOrEmpty(outputForEach) == false) ME.ECSEditor.ScriptTemplates.Create(dir, "Filters.ForEach.gen.cs", "00-FilterExtensionsForEach", new Dictionary<string, string>() { { "CONTENT", outputForEach }, { "CONTENT_BUFFERS", buffers } }, allowRename: false);
                
            }
            
        }

    }

}