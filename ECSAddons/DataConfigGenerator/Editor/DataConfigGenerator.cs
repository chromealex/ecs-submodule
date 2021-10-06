using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ME.ECS.DataConfigGenerator {

    using Parsers;

    public interface IParser {

        bool IsValid(System.Type fieldType);
        bool Parse(string data, System.Type fieldType, out object result);

    }

    internal interface IDefaultParser { }

    public struct LogItem {

        public enum LogItemType {

            Log,
            Warning,
            Error,
            System,

        }
        
        public string text;
        public LogItemType logType;

        public static LogItem Log(string text) {
            
            return new LogItem() {
                logType = LogItemType.Log,
                text = text,
            };

        }

        public static LogItem LogWarning(string text) {
            
            return new LogItem() {
                logType = LogItemType.Warning,
                text = text,
            };

        }

        public static LogItem LogError(string text) {
            
            return new LogItem() {
                logType = LogItemType.Error,
                text = text,
            };
            
        }

        public static LogItem LogSystem(string text) {
            
            return new LogItem() {
                logType = LogItemType.System,
                text = text,
            };
            
        }

    }

    public enum Status {

        Undefined = 0,
        UpToDate,
        Error,
        OK,

    }

    public struct DefaultGeneratorBehaviour : IGeneratorBehaviour {

        public IDataConfigGenerator generator { get; set; }

        public ME.ECS.DataConfigs.DataConfig CreateConfigInstance(ConfigInfo configInfo) {

            if (string.IsNullOrEmpty(configInfo.comment) == false) {

                System.Type typeFound = null;
                foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies()) {
                    
                    var type = asm.GetTypes().FirstOrDefault(y => y.Name == configInfo.comment && typeof(ME.ECS.DataConfigs.DataConfig).IsAssignableFrom(y) == true);
                    if (type != null) {

                        typeFound = type;
                        break;
                        
                    }

                };

                if (typeFound != null) {
                    
                    return (ME.ECS.DataConfigs.DataConfig)ME.ECS.DataConfigs.DataConfig.CreateInstance(typeFound);
                    
                }

            }
            
            return ME.ECS.DataConfigs.DataConfig.CreateInstance<ME.ECS.DataConfigs.DataConfig>();
            
        }

        public void DeleteConfigAsset(string path) {
            
            UnityEditor.AssetDatabase.DeleteAsset(path);

        }

        public object CreateComponentInstance(ME.ECS.DataConfigs.DataConfig config, ComponentInfo componentInfo, bool allValuesAreNull) {

            var field = config.GetType().GetField(componentInfo.name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (field != null) return config;
            
            var componentType = this.generator.GetComponentType(componentInfo);
            if (componentType == null) {
                
                if (allValuesAreNull == false) this.generator.LogError($"Component type `{componentInfo.name}` was not found");
                return null;
                
            }
            var instance = System.Activator.CreateInstance(componentType);
            return instance;

        }

        public void ParseComponentField(ref bool allValuesAreNull, object instance, System.Type componentType, ComponentInfo componentInfo, string fieldName, string data) {
            
            var field = componentType.GetField(fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (field == null) {

                this.generator.LogError($"Field `{fieldName}` was not found in component `{componentInfo.name}`");
                return;

            }

            var dataStr = data;
            if (string.IsNullOrEmpty(dataStr) == false) {

                allValuesAreNull = false;

            } else {

                return;

            }

            var result = this.generator.TryToConvert(dataStr, componentType, field.Name, field.FieldType, out var val);
            if (result == false) {

                this.generator.LogError($"Data `{dataStr}` couldn't been parsed because parser was not found for type `{field.FieldType}`. If you need to store custom struct type - use JSON format.");

            } else {

                field.SetValue(instance, val);

            }

        }

    }
    
    public class DataConfigGenerator : IDataConfigGenerator {

        /*[UnityEditor.MenuItem("ME.ECS/Tests/DataConfig Gen Test")]
        public static void Test() {

            new DataConfigGenerator(System.IO.File.ReadAllText("Assets/ECS-submodule/ECSAddons/DataConfigGenerator/Test.csv"));

        }*/

        public string configsDirectory = "Assets/DataConfigs";
        public IGeneratorBehaviour behaviour;
        public static List<string> projectConfigs;
        private List<string> allConfigs;
        private List<LogItem> logs;
        public Status status;
        public int version;

        public DataConfigGenerator(int currentVersion, string csvData, HashSet<ConfigInfo> visitedConfigs = null, HashSet<string> visitedFiles = null) {

            this.visitedConfigs = visitedConfigs;
            this.visitedFiles = visitedFiles;
            this.logs = new List<LogItem>();

            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            this.allConfigs = UnityEditor.AssetDatabase.FindAssets("t:DataConfig").Where(x => {

                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(x);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfig>(path);
                if (asset is ME.ECS.DataConfigs.DataConfigTemplate) return false;
                return true;

            }).Select(UnityEditor.AssetDatabase.GUIDToAssetPath).ToList();

            var reader = new CsvReader(new System.IO.StringReader(csvData));
            if (reader.Read() == false) {
                
                this.status = Status.Error;
                return;
                
            }

            var destinationDirectory = this.configsDirectory;
            if (System.IO.Directory.Exists(destinationDirectory) == false) {

                System.IO.Directory.CreateDirectory(destinationDirectory);

            }

            int.TryParse(reader[0], out var version);
            if (version > currentVersion) {

                this.version = version;
                if (currentVersion >= 0) {
                    this.Log($"Update version: {currentVersion} => {version}");
                } else {
                    this.Log($"Update version to {version}");
                }

            } else {

                this.Log($"Sheet is up to date (version: {currentVersion}).");
                this.status = Status.UpToDate;
                return;

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
                        last.length = item.offset - last.offset;
                        components[prevOffset] = last;

                    }

                    components.Add(i, item);
                    prevOffset = i;
                    //UnityEngine.Debug.Log("Component: " + name);

                } else {

                    this.LogError($"Duplicate entry `{name}`");

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

                var comment = reader[0];
                var templates = reader[1];
                var configName = reader[2];
                var item = new ConfigInfo {
                    comment = comment,
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

            this.configInfos = configs;
            
            this.status = Status.OK;

        }

        private List<ConfigInfo> configInfos;
        private Dictionary<string, ConfigInfo> createdConfigs;
        private HashSet<ConfigInfo> visitedConfigs;
        private HashSet<string> visitedFiles;

        public string[] GetCreatedConfigs() {

            if (this.createdConfigs == null) return new string[0];
            return this.createdConfigs.Keys.ToArray();

        }

        public HashSet<ConfigInfo> GetVisitedConfigs() {

            return this.visitedConfigs;

        }

        public HashSet<string> GetVisitedFiles() {

            return this.visitedFiles;

        }

        public void UpdateConfigs() {

            if (this.createdConfigs != null) {

                foreach (var kv in this.createdConfigs) {

                    // Update new config
                    var config = kv.Value;
                    this.Log($"Updating config `{config.name}`");
                    this.UpdateConfig(kv.Key, kv.Value);
                    this.Log($"Updated config `{config.name}`");

                }

            }

            var configFiles = System.IO.Directory.GetFiles(this.configsDirectory, "*.asset");
            // Configs to delete
            foreach (var file in configFiles) {

                var fileFix = file.Replace("\\", "/");
                if (this.visitedFiles.Contains(fileFix) == false) {
                    
                    // Delete config
                    //UnityEngine.Debug.LogWarning($"DO: Delete file {file}");
                    this.Log($"Deleting config `{fileFix}`");
                    this.DeleteConfig(fileFix);
                    this.Log($"Deleted config `{fileFix}`");
                    
                }

            }

        }
        
        public void Run() {
            
            if (this.behaviour == null) this.behaviour = new DefaultGeneratorBehaviour();
            this.behaviour.generator = this;

            var configFiles = System.IO.Directory.GetFiles(this.configsDirectory, "*.asset");
            var visitedFiles = this.visitedFiles ?? new HashSet<string>();
            var visitedConfigs = this.visitedConfigs ?? new HashSet<ConfigInfo>();
            // Configs to update
            foreach (var config in this.configInfos) {

                foreach (var file in configFiles) {

                    var fileFix = file.Replace("\\", "/");
                    if (System.IO.Path.GetFileNameWithoutExtension(fileFix) == config.name) {
                        
                        // Update config
                        //UnityEngine.Debug.LogWarning($"DO: Update config {config.name}");
                        this.Log($"Updating config `{config.name}`");
                        this.UpdateConfig(fileFix, config);
                        this.Log($"Updated config `{config.name}`");
                        
                        visitedFiles.Add(fileFix);
                        visitedConfigs.Add(config);

                    }

                }

            }

            // Configs to create
            var created = new Dictionary<string, ConfigInfo>();
            foreach (var config in this.configInfos) {

                if (visitedConfigs.Contains(config) == false) {
                    
                    // Create config
                    //UnityEngine.Debug.LogWarning($"DO: Create config {config.name}");
                    this.Log($"Creating config `{config.name}`");
                    var path = this.CreateConfig(config);
                    this.Log($"Created config `{config.name}`");
                    created.Add(path, config);
                    
                    visitedFiles.Add(path);
                    visitedConfigs.Add(config);
                    
                }
                
            }
            
            this.createdConfigs = created;
            this.visitedConfigs = visitedConfigs;
            this.visitedFiles = visitedFiles;
            
        }

        public System.Type GetComponentType(ComponentInfo info) {

            var asms = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms) {

                var type = asm.GetTypes()
                               .Where(x => typeof(IStructComponentBase).IsAssignableFrom(x))
                               .Where(x => x.FullName.EndsWith(info.name)).OrderByDescending(x => x.Name).FirstOrDefault();
                if (type != null) return type;

            }

            return null;

        }

        public static bool TryToParse(string prefix, string data, out string postfix) {

            if (data.StartsWith(prefix) == true) {
                    
                postfix = data.Substring(prefix.Length, data.Length - prefix.Length);
                return true;
                
            }

            postfix = default;
            return false;

        }

        public static ME.ECS.DataConfigs.DataConfig GetConfig(string name) {

            foreach (var config in DataConfigGenerator.projectConfigs) {

                if (System.IO.Path.GetFileNameWithoutExtension(config) == name) {

                    return UnityEditor.AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfig>(config);

                }
                
            }

            return null;

        }

        internal static List<IParser> parsers = new List<IParser>();
        public static void CollectParsers(DataConfigGenerator generator) {

            if (DataConfigGenerator.parsers.Count > 0) return;
            
            var defaultParsers = new List<IParser>();
            var asms = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms) {

                var types = asm.GetTypes();
                foreach (var type in types) {

                    if (type.IsValueType == true && typeof(IParser).IsAssignableFrom(type) == true) {

                        try {
                            var obj = System.Activator.CreateInstance(type);
                            if (obj is IDefaultParser) {
                                defaultParsers.Add((IParser)obj);
                            } else {
                                DataConfigGenerator.parsers.Add((IParser)obj);
                            }
                        } catch (System.Exception ex) {
                            generator.LogError($"Parser initialization failed: {ex.Message}");
                        }

                    }
                    
                }

            }

            DataConfigGenerator.parsers.AddRange(defaultParsers);

        }

        private bool TryToFindParser(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {

            DataConfigGenerator.CollectParsers(this);

            foreach (var parser in DataConfigGenerator.parsers) {

                if (parser.IsValid(fieldType) == true) {

                    try {
                        if (parser.Parse(data, fieldType, out result) == true) return true;
                    } catch (System.Exception ex) {
                        this.LogError($"Parser `{parser}` failed with exception: {ex.Message}");
                    }

                }
                
            }

            result = null;
            return false;

        }

        public bool TryToConvert(string data, System.Type componentType, string fieldName, System.Type fieldType, out object result) {

            DataConfigGenerator.projectConfigs = this.allConfigs;

            if (fieldType == typeof(int)) {
                
                result = int.Parse(data);
                return true;
                
            } else if (fieldType == typeof(uint)) {
                
                result = uint.Parse(data);
                return true;
                
            } else if (fieldType == typeof(float)) {

                data = data.Replace(",", ".");
                result = float.Parse(data, System.Globalization.CultureInfo.InvariantCulture);
                return true;
                
            } else if (fieldType == typeof(double)) {
                
                data = data.Replace(",", ".");
                result = double.Parse(data, System.Globalization.CultureInfo.InvariantCulture);
                return true;
                
            } else if (fieldType == typeof(long)) {
                
                result = long.Parse(data);
                return true;
                
            } else if (fieldType == typeof(ulong)) {
                
                result = ulong.Parse(data);
                return true;
                
            } else if (fieldType == typeof(short)) {
                
                result = short.Parse(data);
                return true;
                
            } else if (fieldType == typeof(ushort)) {
                
                result = ushort.Parse(data);
                return true;
                
            } else if (fieldType == typeof(byte)) {
                
                result = byte.Parse(data);
                return true;
                
            } else if (fieldType == typeof(sbyte)) {
                             
                result = sbyte.Parse(data);
                return true;
                 
            } else if (fieldType == typeof(string)) {
                
                result = data;
                return true;
                
            }

            if (this.TryToFindParser(data, componentType, fieldName, fieldType, out result) == true) {
                
                return true;
                
            }

            if (data.StartsWith("{") == true || data.StartsWith("[") == true) {

                // Custom json-struct
                result = JSONParser.FromJson(data, fieldType);
                return true;

            }

            result = null;
            return false;

        }

        public void UpdateConfig(string path, ConfigInfo configInfo) {

            var config = UnityEditor.AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfig>(path);
            config.name = configInfo.name;
            config.structComponents = new IStructComponentBase[configInfo.data.Count];
            
            var i = 0;
            foreach (var kv in configInfo.data) {

                var componentInfo = kv.Key;
                var data = kv.Value;
                
                bool isTag = componentInfo.fields.Count == 1 && string.IsNullOrEmpty(componentInfo.fields[0]) == true;
                var allValuesAreNull = true;
                for (int j = 0; j < componentInfo.fields.Count; ++j) {
                        
                    if (string.IsNullOrEmpty(data[j]) == false) {

                        allValuesAreNull = false;
                        break;

                    }

                }

                var instance = this.behaviour.CreateComponentInstance(config, componentInfo, allValuesAreNull);
                if (instance == null) continue;
                
                if (isTag == false) {

                    for (int j = 0; j < componentInfo.fields.Count; ++j) {

                        var fieldName = componentInfo.fields[j];
                        this.behaviour.ParseComponentField(ref allValuesAreNull, instance, instance.GetType(), componentInfo, fieldName, data[j]);
                        
                    }

                }

                if (instance is IStructComponentBase) {

                    if (allValuesAreNull == true) {

                        var list = config.structComponents.ToList();
                        list.RemoveAt(i);
                        config.structComponents = list.ToArray();
                        --i;

                    } else {

                        config.structComponents[i] = (IStructComponentBase)instance;

                    }

                }

                ++i;

            }
            
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
                            var guid = UnityEditor.AssetDatabase.AssetPathToGUID(templatePath);
                            if (config.templates == null) config.templates = new string[0];
                            System.Array.Resize(ref config.templates, config.templates.Length + 1);
                            config.templates[config.templates.Length - 1] = guid;
                            config.AddTemplate(t);
                            found = true;
                            break;
                            
                        }
                        
                    }

                    if (found == false) {
                        
                        this.LogWarning($"Template `{templateName}` was not found");
                        
                    }
                    
                }
                
            }

            config.Save();
            
        }

        public void DeleteConfig(string path) {
            
            this.behaviour.DeleteConfigAsset(path);
            
        }

        public string CreateConfig(ConfigInfo configInfo) {

            var config = this.behaviour.CreateConfigInstance(configInfo);
            config.name = configInfo.name;
            var path = $"{this.configsDirectory}/{config.name}.asset";
            UnityEditor.AssetDatabase.CreateAsset(config, path);
            UnityEditor.AssetDatabase.ImportAsset(path);
            return path;

        }

        public void AddConfig(string path) {
            
            this.allConfigs.Add(path);
            
        }

        public void AddConfigs(IList<string> paths) {
            
            this.allConfigs.AddRange(paths);
            
        }

        public List<LogItem> GetLogs() {

            return this.logs;

        }

        public void ClearLogs() {
            
            this.logs.Clear();
            
        }

        public void LogError(string text) {
            
            this.logs.Add(LogItem.LogError(text));
            
        }
        
        public void Log(string text) {
            
            this.logs.Add(LogItem.Log(text));
            
        }

        public void LogWarning(string text) {
            
            this.logs.Add(LogItem.LogWarning(text));
            
        }

    }

}