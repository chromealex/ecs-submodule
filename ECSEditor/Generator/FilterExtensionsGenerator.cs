using System.Collections.Generic;
using UnityEditor;

namespace ME.ECSEditor {

    public class FilterExtensionsGenerator : AssetPostprocessor {

        [UnityEditor.MenuItem("ME.ECS/Generators/Generate Filters...")]
        public static void GenerateFilters() {
            
            var asms = UnityEditor.AssetDatabase.FindAssets("t:asmdef ECSAssembly");
            foreach (var asm in asms) {

                var asset = UnityEditor.AssetDatabase.GUIDToAssetPath(asm);
                var dir = System.IO.Path.GetDirectoryName(asset) + "/Core/Filters/CodeGenerator";
                if (System.IO.Directory.Exists(dir) == false) continue;

                var outputDelegates = string.Empty;
                var outputForEach = string.Empty;
                var buffers = string.Empty;
                
                var count = 20;
                for (int j = 1; j < count; ++j) {

                    var itemsType = "T0";
                    for (int i = 1; i < j; ++i) {

                        itemsType += ",T" + i.ToString();

                    }

                    var items = "ref T0 t0";
                    for (int i = 1; i < j; ++i) {

                        items += ", ref T" + i.ToString() + " t" + i.ToString();

                    }

                    {
                        var res = UnityEngine.Resources.Load<UnityEngine.TextAsset>("00-FilterExtensionsDelegateItem").text;
                        res = res.Replace("#ITEMS_TYPE#", itemsType);
                        res = res.Replace("#ITEMS#", items);
                        res = res.Replace("#INDEX#", j.ToString());
                        outputDelegates += res + "\n";
                    }
                    
                    {
                    
                        var itemsWhere = " where T0:struct,IStructComponent";
                        for (int i = 1; i < j; ++i) {

                            itemsWhere += " where T" + i.ToString() + ":struct,IStructComponent";

                        }

                        var itemsGet = "ref buffer.GetT0(id)";
                        for (int i = 1; i < j; ++i) {

                            itemsGet += ", ref buffer.GetT" + i.ToString() + "(id)";

                        }

                        var res = UnityEngine.Resources.Load<UnityEngine.TextAsset>("00-FilterExtensionsForEachItem").text;
                        res = res.Replace("#ITEMS_TYPE#", itemsType);
                        res = res.Replace("#ITEMS_WHERE#", itemsWhere);
                        res = res.Replace("#ITEMS_GET#", itemsGet);
                        res = res.Replace("#INDEX#", j.ToString());
                        outputForEach += res + "\n";
                        
                    }
                    
                    {
                    
                        var itemsMethods = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            itemsMethods += "[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T" + i.ToString() + " GetT" + i.ToString() + "(int entityId) { return ref this.buffer" + i.ToString() + ".Get(entityId); }\n";

                        }
                        
                        var itemsInit = string.Empty;
                        for (int i = 0; i < j; ++i) {

                            itemsInit += "this.buffer" + i.ToString() + " = new DataBuffer<T" + i.ToString() + ">(world, arr, minIdx, maxIdx);\n";

                        }
                        
                        var itemsWhere = " where T0:struct,IStructComponent";
                        for (int i = 1; i < j; ++i) {

                            itemsWhere += " where T" + i.ToString() + ":struct,IStructComponent";

                        }
        
                        var itemsBuffer = "private readonly DataBuffer<T0> buffer0;";
                        for (int i = 1; i < j; ++i) {

                            itemsBuffer += "private readonly DataBuffer<T" + i.ToString() + "> buffer" + i.ToString() + ";";

                        }
        
                        var res = UnityEngine.Resources.Load<UnityEngine.TextAsset>("00-FilterExtensionsBufferItem").text;
                        res = res.Replace("#ITEMS_TYPE#", itemsType);
                        res = res.Replace("#ITEMS_WHERE#", itemsWhere);
                        res = res.Replace("#ITEMS_METHODS#", itemsMethods);
                        res = res.Replace("#ITEMS_INIT#", itemsInit);
                        res = res.Replace("#ITEMS_BUFFER#", itemsBuffer);
                        res = res.Replace("#INDEX#", j.ToString());
                        buffers += res + "\n";
                        
                    }

                }
                
                if (string.IsNullOrEmpty(outputDelegates) == false) ME.ECSEditor.ScriptTemplates.Create(dir, "Filters.Delegates.gen.cs", "00-FilterExtensionsDelegates", new Dictionary<string, string>() { { "CONTENT", outputDelegates } }, allowRename: false);
                if (string.IsNullOrEmpty(outputForEach) == false) ME.ECSEditor.ScriptTemplates.Create(dir, "Filters.ForEach.gen.cs", "00-FilterExtensionsForEach", new Dictionary<string, string>() { { "CONTENT", outputForEach }, { "CONTENT_BUFFERS", buffers } }, allowRename: false);
                
            }
            
        }

    }

}