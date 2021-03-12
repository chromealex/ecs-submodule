
using System.Linq;

namespace ME.ECSEditor.BlackBox {
    
    using ME.ECS;
    using ME.ECS.BlackBox;
    
    public static class Compiler {

        public static string Make(Container container) {
            
            return new ContainerCompiler(container).Make();
            
        }

        public static string GetClassVariable(string fieldName) {
            
            return $@"[^\w]({fieldName})[^\w]";

        }

        public static FieldItemCompilerInfo GetFieldItemInfo(Box node, string val, System.Reflection.FieldInfo fieldInfo, string publicFieldName, string fieldName) {

            if (typeof(ME.ECS.Views.IView).IsAssignableFrom(fieldInfo.FieldType) == true) {
                
                val = System.Text.RegularExpressions.Regex.Replace(val, Compiler.GetClassVariable(fieldName), new System.Text.RegularExpressions.MatchEvaluator((match) => {

                    var target = match.Groups[0];
                    var r = target.Value;
                    return r.Replace(fieldName, $"{publicFieldName}");

                }));
                
                var list = new System.Collections.Generic.List<FieldItemInfo>();
                var allFields = node.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                foreach (var field in allFields) {

                    var innerFieldName = field.Name;
                    var innerPrivateFieldName = $"{publicFieldName}__{innerFieldName}";
                    var replace = $@"{innerFieldName}";
                    var replaceRaw = $@"{innerFieldName}";
                    val = System.Text.RegularExpressions.Regex.Replace(val, Compiler.GetClassVariable(replace), new System.Text.RegularExpressions.MatchEvaluator((match) => {

                        var target = match.Groups[0];
                        var r = target.Value;
                        return r.Replace(replaceRaw, $"{innerPrivateFieldName}");

                    }));
                    
                    list.Add(new FieldItemInfo() {
                        name = innerPrivateFieldName,
                        type = field.FieldType,
                        isPrivate = true,
                    });

                }
                
                return new FieldItemCompilerInfo() {
                    customExecuteCode = val,
                    innerFields = list,
                    fieldItemInfo = new FieldItemInfo() {
                        name = publicFieldName,
                        type = fieldInfo.FieldType,
                    },
                };
                
            }
            
            if (fieldInfo.FieldType == typeof(BlueprintInfo)) {
                
                var replace = $@"{fieldName}\.link";
                var replaceRaw = $@"{fieldName}.link";
                val = System.Text.RegularExpressions.Regex.Replace(val, Compiler.GetClassVariable(replace), new System.Text.RegularExpressions.MatchEvaluator((match) => {

                    var target = match.Groups[0];
                    var r = target.Value;
                    return r.Replace(replaceRaw, $"{publicFieldName}");

                }));
                
                return new FieldItemCompilerInfo() {
                    customExecuteCode = val,
                    fieldItemInfo = new FieldItemInfo() {
                        name = publicFieldName,
                        type = typeof(Blueprint),
                    },
                };
                
            }

            return new FieldItemCompilerInfo() {
                customExecuteCode = string.Empty,
                fieldItemInfo = new FieldItemInfo() {
                    name = publicFieldName,
                    type = fieldInfo.FieldType,
                },
            };

        }

    }

    public struct FieldItemCompilerInfo {

        public string customExecuteCode;
        public FieldItemInfo fieldItemInfo;
        public System.Collections.Generic.List<FieldItemInfo> innerFields;

    }
    
    public struct FieldItemInfo {

        public System.Type type;
        public string name;
        public bool isPrivate;

        public string ToFieldString() {

            return $"{(this.isPrivate == true ? "internal" : "public")} {this.type.FullName.Replace("+", ".")} {this.name};";

        }

        public string ToPropertyString() {

            return $"internal ref {this.type.FullName.Replace("+", ".")} {this.name} {{ get {{ return ref this.feature.{this.name}; }} }}";

        }

    }
    
    public class ContainerCompiler {

        private Container container;

        public ContainerCompiler(Container container) {

            this.container = container;

        }

        public string Make() {

            var system = "public sealed class #NAME#Feature : FeatureBase {\n" +
                         "#PUBLIC_FIELDS#\n" +
                         "protected override void OnConstruct() { #ON_CONSTRUCT# }\n" +
                         "protected override void OnDeconstruct() {}\n" +
                         "}\n" +
                         "public sealed class #NAME#System : ISystemFilter {\n" +
                         "#PUBLIC_PROPERTIES#\n" +
                         "private #NAME#Feature feature;\n" +
                         "public World world { get; set; }\n" +
                         "void ISystemBase.OnConstruct() { this.feature = this.world.GetFeature<#NAME#Feature>(); }\n" +
                         "void ISystemBase.OnDeconstruct() {}\n" +
                         "bool ISystemFilter.jobs => false;\n" +
                         "int ISystemFilter.jobsBatchCount => 64;\n" +
                         "Filter ISystemFilter.filter { get; set; }\n" +
                         "Filter ISystemFilter.CreateFilter() {\n" +
                         "    return #MAIN_FILTER#;\n" +
                         "}\n" +
                         "void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {" +
                         "#ADVANCE_TICK#\n" +
                         "}\n" +
                         "}\n";

            var name = this.container.name.Replace(" ", string.Empty);
            var fields = new System.Collections.Generic.List<FieldItemInfo>();
            var mainFilterStr = new FilterCompiler(this.container.inputFilter).Make();
            
            var output = system.Replace("#NAME#", name);
            output = output.Replace("#MAIN_FILTER#", mainFilterStr);
            
            var compiledBlueprint = new BlueprintCompiler(this.container.blueprint, false).Make((field) => {
                
                if (fields.Any(x => x.name == field.name) == false) fields.Add(field);
                
            });
            
            var compiledBlueprintOnConstruct = new BlueprintCompiler(this.container.blueprint, true).Make((field) => {
                
                if (fields.Any(x => x.name == field.name) == false) fields.Add(field);
                
            });
            output = output.Replace("#PUBLIC_FIELDS#", string.Join("\n", fields.Select(x => x.ToFieldString()).ToArray()));
            output = output.Replace("#PUBLIC_PROPERTIES#", string.Join("\n", fields.Select(x => x.ToPropertyString()).ToArray()));
            output = output.Replace("#ADVANCE_TICK#", compiledBlueprint);
            output = output.Replace("#ON_CONSTRUCT#", compiledBlueprintOnConstruct);

            return output;

        }

    }

    public class BlueprintCompiler {

        private Blueprint blueprint;
        private int funcIndex;
        private bool onConstruct;

        public BlueprintCompiler(Blueprint blueprint, bool onConstruct) {

            this.blueprint = blueprint;
            this.funcIndex = 0;
            this.onConstruct = onConstruct;

        }

        public string Make(System.Action<FieldItemInfo> onField) {

            var output = string.Empty;
            var node = this.blueprint.root;
            var rootIndex = -1;
            for (int i = 0; i < this.blueprint.boxes.Length; ++i) {
                if (this.blueprint.boxes[i].box == node) {
                    rootIndex = i;
                    break;
                }
            }

            if (rootIndex >= 0) {

                if (this.onConstruct == false) output += $"\ngoto __STEP{rootIndex}; // {node.name} \n";
                for (int i = 0; i < this.blueprint.boxes.Length; ++i) {

                    output += new NodeCompiler(this.blueprint, this.blueprint.boxes[i].box, this.onConstruct).MakeDefinition(ref this.funcIndex, onField);

                }

                if (this.onConstruct == false) output += $"\n__EXIT:\n";

            }

            /*
            var prov = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp");
            var cp = new System.CodeDom.Compiler.CompilerParameters();
            cp.GenerateInMemory = true;
            cp.TreatWarningsAsErrors = false;
            var cr = prov.CompileAssemblyFromSource(cp, txt.text);
            if(cr.Errors.Count > 0)
            {
                // Display compilation errors.
                output += "Errors building:";
                foreach(var ce in cr.Errors)
                {
                    output += "  {0}" + ce.ToString();
                }
            }
            else
            {
                // Display a successful compilation message.
                output += "Source built successfully.";
            }*/
            return output;
            
        }

    }

    public class NodeCompiler {

        private Blueprint blueprint;
        private Box node;
        private bool onConstruct;

        public NodeCompiler(Blueprint blueprint, Box node, bool onConstruct) {

            this.blueprint = blueprint;
            this.node = node;
            this.onConstruct = onConstruct;

        }

        public string MakeDefinition(ref int funcIndex, System.Action<FieldItemInfo> onField) {
            
            var output = string.Empty;
            var so = new UnityEditor.SerializedObject(this.node);
            var iter = so.GetIterator();
            iter.NextVisible(true);
            var txt = (UnityEngine.TextAsset)iter.objectReferenceValue;

            var pattern = "Box\\s+?Execute.*?\\{((?:[^\\{\\}]|(?<open>\\{)|(?<-open>\\}))+(?(open)(?!)))\\}";
            if (this.onConstruct == true) {

                pattern = "void\\s+?OnCreate.*?\\{((?:[^\\{\\}]|(?<open>\\{)|(?<-open>\\}))+(?(open)(?!)))\\}";

            }
            var matches = System.Text.RegularExpressions.Regex.Match(txt.text, pattern, System.Text.RegularExpressions.RegexOptions.Singleline);
            var result = matches.Groups[1];
            var val = result.Value;

            // Replace return
            if (this.onConstruct == false) {

                val = System.Text.RegularExpressions.Regex.Replace(val, @"return\s+(.*?)\s*;", new System.Text.RegularExpressions.MatchEvaluator((match) => {

                    var target = match.Groups[1];
                    var r = target.Value;
                    var rs = r.Split('.');
                    var fieldName = rs[rs.Length - 1];
                    var prop = so.FindProperty(fieldName);
                    if (prop == null) {
                        return $"\ngoto __EXIT;\n";
                    }

                    var nextNode = prop.objectReferenceValue as Box;
                    if (nextNode != null) {

                        for (int i = 0; i < this.blueprint.boxes.Length; ++i) {

                            if (this.blueprint.boxes[i].box == nextNode) {

                                return $"\ngoto __STEP{i}; // {nextNode.name}\n";

                            }

                        }

                    }

                    return $"\ngoto __EXIT;\n";

                }));

            }

            // Search for public serialized fields
            var index = funcIndex;
            while (iter.NextVisible(false) == true) {
                
                var fieldName = iter.name;
                var publicFieldName = $"{fieldName}__step{index}";
                var fieldInfo = this.node.GetType().GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                var itemCompiler = Compiler.GetFieldItemInfo(this.node, val, fieldInfo, publicFieldName, fieldName);
                var addField = false;
                if (string.IsNullOrEmpty(itemCompiler.customExecuteCode) == true) {

                    var replacedCount = 0;
                    val = System.Text.RegularExpressions.Regex.Replace(val, Compiler.GetClassVariable(fieldName), new System.Text.RegularExpressions.MatchEvaluator((match) => {

                        ++replacedCount;
                        var target = match.Groups[0];
                        var r = target.Value;
                        return r.Replace(fieldName, $"{publicFieldName}");

                    }));
                    addField = replacedCount > 0;

                    var allFields = node.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    foreach (var field in allFields) {

                        var innerFieldName = field.Name;
                        var innerPrivateFieldName = $"{publicFieldName}__{innerFieldName}";
                        var replace = $@"{innerFieldName}";
                        var replaceRaw = $@"{innerFieldName}";
                        val = System.Text.RegularExpressions.Regex.Replace(val, Compiler.GetClassVariable(replace), new System.Text.RegularExpressions.MatchEvaluator((match) => {

                            var target = match.Groups[0];
                            var r = target.Value;
                            return r.Replace(replaceRaw, $"{innerPrivateFieldName}");

                        }));
                    
                        onField.Invoke(new FieldItemInfo() {
                            name = innerPrivateFieldName,
                            type = field.FieldType,
                            isPrivate = true,
                        });

                    }

                } else {

                    val = itemCompiler.customExecuteCode;
                    addField = true;

                }

                if (addField == true) {

                    onField.Invoke(itemCompiler.fieldItemInfo);
                    if (itemCompiler.innerFields != null) {
                        
                        foreach (var item in itemCompiler.innerFields) onField.Invoke(item);
                        
                    }

                }

            }

            if (this.onConstruct == false) {

                var innerFunc = new FunctionCompiler(funcIndex++, val);
                output += "\n";
                output += innerFunc.MakeDefinition();
                output += "\n";

            } else {
                
                output += val;
                funcIndex++;
                
            }

            return output;
            
        }

        public string Make() {
            
            /*
            var output = string.Empty;
            var so = new UnityEditor.SerializedObject(this.node);
            var iter = so.GetIterator();
            iter.NextVisible(true);
            var txt = (UnityEngine.TextAsset)iter.objectReferenceValue;
            UnityEngine.Debug.Log("IN " + txt.text);
            
            var matches = System.Text.RegularExpressions.Regex.Match(txt.text, "Box\\s+?Execute.*?\\{((?:[^\\{\\}]|(?<open>\\{)|(?<-open>\\}))+(?(open)(?!)))\\}", System.Text.RegularExpressions.RegexOptions.Singleline);
            var result = matches.Groups[1];
            var val = result.Value;
            
            val = System.Text.RegularExpressions.Regex.Replace(val, @"return\s+(.*?)\s*;", new System.Text.RegularExpressions.MatchEvaluator((match) => {
                
                var target = match.Groups[1];
                var r = target.Value;
                var rs = r.Split('.');
                var fieldName = rs[rs.Length - 1];
                var prop = so.FindProperty(fieldName);
                if (prop == null) {
                    UnityEngine.Debug.Log(fieldName);
                    return "\ngoto __EXIT;\n";
                }
                var nextNode = prop.objectReferenceValue as Box;
                if (nextNode != null) {

                    //output += new NodeCompiler(nextNode).Make(ref funcIndex);
                    for (int i = 0; i < this.blueprint.boxes.Length; ++i) {

                        if (this.blueprint.boxes[i].box == nextNode) {

                            return $"\ngoto __STEP{i};\n";

                        }
                        
                    }

                }

                return "\ngoto __EXIT;\n";
                
            }));
            
            var innerFunc = new FunctionCompiler(funcIndex++, val);
            output += "\n";
            output += innerFunc.MakeCall();
            output += "\n";
            output += innerFunc.MakeDefinition();
            output += "\n";*/

            return null;

        }

    }

    public class FunctionCompiler {

        private int index;
        private string body;

        public FunctionCompiler(int index, string body) {

            this.index = index;
            this.body = body;

        }

        public string MakeDefinition() {

            return $"__STEP{this.index}:{{{this.body}}}";

        }

        public string MakeCall() {

            return $"goto __STEP{this.index};";

        }

    }

    public class FilterCompiler {

        private FilterDataTypes filter;

        public FilterCompiler(FilterDataTypes filter) {

            this.filter = filter;

        }

        public string Make() {

            var typesWith = string.Join("", this.filter.with.Select(x => $".With<{x.GetType().FullName}>()").ToArray());
            var typesWithout = string.Join("", this.filter.without.Select(x => $".Without<{x.GetType().FullName}>()").ToArray());
            var filter = $"Filter.Create(){typesWith}{typesWithout}.Push()";
            return filter;
            
        }

    }

}