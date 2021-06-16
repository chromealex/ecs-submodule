using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ME.ECS.DataConfigGenerator {

    using Parsers;
    
    public class DataConfigGenerator {

        [UnityEditor.MenuItem("ME.ECS/Tests/DataConfig Gen Test")]
        public static void Test() {

            new DataConfigGenerator(System.IO.File.ReadAllText("Assets/ECS-submodule/ECSAddons/DataConfigGenerator/Test.csv"));

        }

        public struct ComponentInfo : System.IEquatable<ComponentInfo> {

            public string name;
            public int offset;
            public int length;
            public List<string> fields;

            public bool Equals(ComponentInfo other) {

                return other.name == this.name;

            }

        }

        public struct ConfigInfo : System.IEquatable<ConfigInfo> {

            public string name;
            public string[] templates;
            public Dictionary<ComponentInfo, List<string>> data;

            public bool Equals(ConfigInfo other) {

                return other.name == this.name;

            }
            
        }

        public string configsDirectory = "Assets/DataConfigs";
        private List<string> allConfigs;
        
        public DataConfigGenerator(string csvData) {

            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            
            this.allConfigs = UnityEditor.AssetDatabase.FindAssets("t:DataConfig").Where(x => {

                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(x);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfig>(path);
                if (asset is ME.ECS.DataConfigs.DataConfigTemplate) return false;
                return true;

            }).Select(UnityEditor.AssetDatabase.GUIDToAssetPath).ToList();

            var reader = new CsvReader(new System.IO.StringReader(csvData));
            if (reader.Read() == false) return;

            var destinationDirectory = this.configsDirectory;
            if (System.IO.Directory.Exists(destinationDirectory) == false) {

                System.IO.Directory.CreateDirectory(destinationDirectory);

            }

            //var fieldsCount = reader.FieldsCount;
            //UnityEngine.Debug.Log("Fields count: " + fieldsCount);
            
            var components = new Dictionary<int, ComponentInfo>();
            var prevOffset = -1;
            for (int i = 3; i < reader.FieldsCount; ++i) {

                var name = reader[i];
                if (string.IsNullOrEmpty(name) == true) continue;
                
                var item = new ComponentInfo() {
                    name = name,
                    offset = i,
                    length = reader.FieldsCount - i,
                    fields = new List<string>(),
                };
                if (components.ContainsValue(item) == false) {

                    if (prevOffset >= 0) {

                        var last = components[prevOffset];
                        last.length = i - item.offset + 1;
                        components[prevOffset] = last;

                    }

                    components.Add(i, item);
                    prevOffset = i;
                    //UnityEngine.Debug.Log("Component: " + name);

                } else {
                    
                    UnityEngine.Debug.LogWarning($"Duplicate entry `{name}`");
                    
                }
                
            }

            // This line contains component field names
            reader.Read();
            foreach (var kv in components) {

                var info = kv.Value;
                //UnityEngine.Debug.Log("Read fields for: " + info.name + ", length: " + info.length);
                for (int j = 0; j < info.length; ++j) {
                
                    var fieldName = reader[info.offset + j];
                    info.fields.Add(fieldName);
                    //UnityEngine.Debug.Log("Field name: " + fieldName);
    
                }

            }
            
            // Other lines contain data configs
            var configs = new List<ConfigInfo>();
            while (reader.Read() == true) {

                var templates = reader[1];
                var configName = reader[2];
                var item = new ConfigInfo {
                    name = configName,
                    templates = templates.Split(','),
                    data = new Dictionary<ComponentInfo, List<string>>(),
                };
                foreach (var kv in components) {

                    var componentInfo = kv.Value;
                    var list = new List<string>();
                    
                    //UnityEngine.Debug.Log("Add component: " + componentInfo.name);
                    for (int j = 0; j < componentInfo.length; ++j) {

                        var data = reader[componentInfo.offset + j];
                        list.Add(data);
                        //UnityEngine.Debug.Log("Set component field: " + componentInfo.fields[j] + " value " + data);
                        
                    }
                    
                    item.data.Add(componentInfo, list);

                }
                
                configs.Add(item);
                
            }

            var configFiles = System.IO.Directory.GetFiles(destinationDirectory, "*.asset");
            var visitedFiles = new HashSet<string>();
            var visitedConfigs = new HashSet<ConfigInfo>();
            // Configs to update
            foreach (var config in configs) {

                foreach (var file in configFiles) {

                    if (System.IO.Path.GetFileNameWithoutExtension(file) == config.name) {
                        
                        // Update config
                        UnityEngine.Debug.LogWarning($"DO: Update config {config.name}");
                        this.UpdateConfig(file, config);
                        
                        visitedFiles.Add(file);
                        visitedConfigs.Add(config);

                    }

                }

            }

            // Configs to create
            var created = new Dictionary<string, ConfigInfo>();
            foreach (var config in configs) {

                if (visitedConfigs.Contains(config) == false) {
                    
                    // Create config
                    UnityEngine.Debug.LogWarning($"DO: Create config {config.name}");
                    created.Add(this.CreateConfig(config), config);
                    
                    visitedConfigs.Add(config);
                    
                }
                
            }
            
            foreach (var kv in created) {

                // Update new config
                this.UpdateConfig(kv.Key, kv.Value);
                
            }

            // Configs to delete
            foreach (var file in configFiles) {

                if (visitedFiles.Contains(file) == false) {
                    
                    // Delete config
                    UnityEngine.Debug.LogWarning($"DO: Delete file {file}");
                    this.DeleteConfig(file);
                    
                }

            }

        }

        private System.Type GetComponentType(ComponentInfo info) {

            var asms = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms) {

                var type = asm.GetTypes()
                               .Where(x => typeof(IStructComponentBase).IsAssignableFrom(x))
                               .FirstOrDefault(x => x.FullName.EndsWith(info.name));
                if (type != null) return type;

            }

            return null;

        }

        private bool TryToParse(string prefix, string data, out string postfix) {

            if (data.StartsWith(prefix) == true) {
                    
                postfix = data.Substring(prefix.Length, data.Length - prefix.Length);
                return true;
                    
            }

            postfix = default;
            return false;

        }

        private ME.ECS.DataConfigs.DataConfig GetConfig(string name) {

            foreach (var config in this.allConfigs) {

                if (System.IO.Path.GetFileNameWithoutExtension(config) == name) {

                    return UnityEditor.AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfig>(config);

                }
                
            }

            return null;

        }

        private object Convert(string data, System.Type fieldType) {

            if (typeof(Object).IsAssignableFrom(fieldType) == true) {
                
                // Try to find reference
                if (this.TryToParse("config://", data, out var configName) == true) {

                    return this.GetConfig(configName);

                }
                
                if (this.TryToParse("view://", data, out var viewPath) == true) {

                    return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(viewPath).GetComponent<ME.ECS.Views.ViewBase>();

                }

                if (this.TryToParse("go://", data, out var goPath) == true) {

                    return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(goPath);

                }

                if (this.TryToParse("component://", data, out var componentPath) == true) {

                    return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(componentPath).GetComponent(fieldType);

                }

            } else if (fieldType == typeof(int)) {
                
                return int.Parse(data);
                
            } else if (fieldType == typeof(uint)) {
                
                return uint.Parse(data);
                
            } else if (fieldType == typeof(float)) {
                
                return float.Parse(data);
                
            } else if (fieldType == typeof(double)) {
                
                return double.Parse(data);
                
            } else if (fieldType == typeof(long)) {
                
                return long.Parse(data);
                
            } else if (fieldType == typeof(ulong)) {
                
                return ulong.Parse(data);
                
            } else if (fieldType == typeof(short)) {
                
                return short.Parse(data);
                
            } else if (fieldType == typeof(ushort)) {
                
                return ushort.Parse(data);
                
            } else if (fieldType == typeof(byte)) {
                
                return byte.Parse(data);
                
            } else if (fieldType == typeof(sbyte)) {
                             
                return sbyte.Parse(data);
                 
            } else if (fieldType == typeof(string)) {
                
                return data;
                
            } else if (fieldType == typeof(Vector2)) {
                
                var prs = data.Split(';');
                return new Vector2(float.Parse(prs[0]), float.Parse(prs[1]));
                
            } else if (fieldType == typeof(Vector3)) {
                
                var prs = data.Split(';');
                return new Vector3(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2]));
                
            } else if (fieldType == typeof(Vector4)) {
                
                var prs = data.Split(';');
                return new Vector4(float.Parse(prs[0]), float.Parse(prs[1]), float.Parse(prs[2]), float.Parse(prs[3]));
                
            } else if (fieldType == typeof(Vector2Int)) {
                
                var prs = data.Split(';');
                return new Vector2Int(int.Parse(prs[0]), int.Parse(prs[1]));
                
            } else if (fieldType == typeof(Vector3Int)) {
                
                var prs = data.Split(';');
                return new Vector3Int(int.Parse(prs[0]), int.Parse(prs[1]), int.Parse(prs[2]));
                
            }

            if (data.StartsWith("{") == true) {

                // Custom json-struct
                return JsonUtility.FromJson(data, fieldType);

            }
            
            UnityEngine.Debug.LogError($"Type {fieldType} couldn't been parsed because of deserializer not found. If you need to store custom struct type - use JSON format.");

            return null;

        }

        public void UpdateConfig(string path, ConfigInfo configInfo) {

            var config = UnityEditor.AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfig>(path);
            config.name = configInfo.name;
            config.structComponents = new IStructComponentBase[configInfo.data.Count];
            var templates = configInfo.templates;
            if (templates != null) {
                
                var templatePaths = UnityEditor.AssetDatabase.FindAssets("t:DataConfigTemplate").Select(UnityEditor.AssetDatabase.GUIDToAssetPath).ToArray();
                foreach (var template in templates) {
                
                    var templateName = template.Trim();
                    if (string.IsNullOrEmpty(templateName) == true) continue;
                    
                    var found = false;
                    foreach (var templatePath in templatePaths) {

                        if (System.IO.Path.GetFileNameWithoutExtension(templatePath) == templateName) {

                            var t = UnityEditor.AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(templatePath);
                            config.AddTemplate(t);
                            found = true;
                            break;
                            
                        }
                        
                    }

                    if (found == false) {
                        
                        UnityEngine.Debug.LogError($"Template was not found with name {templateName}");
                        
                    }
                    
                }
                
            }

            var i = 0;
            foreach (var kv in configInfo.data) {

                var componentInfo = kv.Key;
                var data = kv.Value;
                var componentType = this.GetComponentType(componentInfo);
                if (componentType == null) {
                    
                    UnityEngine.Debug.LogError($"Component type not found with name {componentInfo.name}");
                    continue;
                    
                }
                var instance = System.Activator.CreateInstance(componentType);
                for (int j = 0; j < componentInfo.fields.Count; ++j) {

                    var fieldName = componentInfo.fields[j];
                    var field = componentType.GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                    if (field == null) {

                        UnityEngine.Debug.LogError($"Field not found with name {fieldName}");
                        continue;
                        
                    }
                    field.SetValue(instance, this.Convert(data[j], field.FieldType));

                }
                config.structComponents[i] = (IStructComponentBase)instance;
                ++i;

            }
            
            config.Save();
            
        }

        public void DeleteConfig(string path) {
            
            UnityEditor.AssetDatabase.DeleteAsset(path);
            
        }

        public string CreateConfig(ConfigInfo configInfo) {

            var config = ME.ECS.DataConfigs.DataConfig.CreateInstance<ME.ECS.DataConfigs.DataConfig>();
            config.name = configInfo.name;
            var path = $"{this.configsDirectory}/{config.name}.asset";
            UnityEditor.AssetDatabase.CreateAsset(config, path);
            UnityEditor.AssetDatabase.ImportAsset(path);
            return path;

        }

    }

}