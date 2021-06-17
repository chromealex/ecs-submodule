using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ME.ECS.DataConfigGenerator {
    
    using ME.ECSEditor;

    [CustomEditor(typeof(DataConfigGeneratorSettings))]
    public class DataConfigGeneratorSettingsEditor : Editor {

        private const int FONT_SIZE = 12;
        
        private SerializedProperty paths;
        private static List<LogItem> logs;

        private GUIStyle fixedFontStyle;
        private Font font;
        private System.Text.StringBuilder logsResult;
        private bool inProgress;

        public void OnEnable() {

            if (DataConfigGeneratorSettingsEditor.logs == null) DataConfigGeneratorSettingsEditor.logs = new List<LogItem>();
            this.paths = this.serializedObject.FindProperty("paths");
            
            this.logsResult = new System.Text.StringBuilder(1000);

            EditorCoroutines.StartCoroutine(this.ProgressSymbol());

        }
        
        private void CleanupFont() {
            
            if (this.font == null) {
                return;
            }

            DataConfigGeneratorSettingsEditor.DestroyImmediate(this.font, true);
            this.font = null;
            
        }

        public void OnDisable() {
            
            this.CleanupFont();
            
        }
        
        public override void OnInspectorGUI() {
            
            if (this.fixedFontStyle == null || this.font == null) {
                
                this.fixedFontStyle = new GUIStyle(GUI.skin.label);
                string fontName;
                if (Application.platform == RuntimePlatform.WindowsEditor)
                    fontName = "Consolas";
                else
                    fontName = "Courier";

                this.CleanupFont();

                this.font = Font.CreateDynamicFontFromOSFont(fontName, DataConfigGeneratorSettingsEditor.FONT_SIZE);
                this.fixedFontStyle.normal = EditorStyles.label.normal;
                this.fixedFontStyle.richText = true;
                this.fixedFontStyle.font = this.font;
                this.fixedFontStyle.fontSize = DataConfigGeneratorSettingsEditor.FONT_SIZE;
                
            }
            
            this.serializedObject.Update();
            
            GUILayoutExt.Box(10f, 10f, () => {

                EditorGUILayout.PropertyField(this.paths);

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUI.BeginDisabledGroup(this.inProgress);
                if (GUILayout.Button("Update All", GUILayout.Width(120f), GUILayout.Height(30f)) == true) {

                    DataConfigGeneratorSettingsEditor.logs.Clear();
                    EditorCoroutines.StartCoroutine(this.LoadAll(this.paths));
                    
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                
            });

            GUILayoutExt.Box(10f, 10f, () => {

                this.logsResult.Clear();
                foreach (var item in DataConfigGeneratorSettingsEditor.logs) {

                    if (item.logType == LogType.Error) {
                     
                        this.logsResult.Append($"<color=#f44>{item.text}</color>");
   
                    } else if (item.logType == LogType.Warning) {
                     
                        this.logsResult.Append($"<color=#ff4>{item.text}</color>");
   
                    } else {

                        this.logsResult.Append(item.text);

                    }

                    this.logsResult.Append("\n");

                }
                
                GUILayout.TextArea(this.logsResult.ToString(), this.fixedFontStyle);

            });

            this.serializedObject.ApplyModifiedProperties();
            
            this.Repaint();

        }

        private string progressSymbol;
        IEnumerator ProgressSymbol() {

            var j = 0;
            while (true) {
                
                this.progressSymbol = "|";
                switch (j) {
                    case 1:
                        this.progressSymbol = "/";
                        break;

                    case 2:
                        this.progressSymbol = "-";
                        break;

                    case 3:
                        this.progressSymbol = "\\";
                        break;
                }

                ++j;
                if (j > 3) j = 0;

                yield return null;

            }
            
        }
        
        IEnumerator GetFileSize(string url, System.Action onLoading, System.Action<long> result) {
            
            UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequest.Head(url);
            uwr.SendWebRequest();
            while (uwr.isDone == false) {
                onLoading.Invoke();
                yield return null;
            }

            string size = uwr.GetResponseHeader("Content-Length");

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                if (result != null)
                    result(-1);
            }
            else
            {
                if (result != null)
                    result(System.Convert.ToInt64(size));
            }
        }

        private IEnumerator LoadAll(SerializedProperty paths) {

            this.inProgress = true;
            var visitedConfigs = new HashSet<DataConfigGenerator.ConfigInfo>();
            var visitedFiles = new HashSet<string>();
        
            var list = new List<DataConfigGenerator>();
            var newConfigs = new List<string>();
            var pathsStr = new string[paths.arraySize];
            var captions = new string[paths.arraySize];
            var versions = new int[paths.arraySize];
            var visitedFilesArr = new string[paths.arraySize];
            for (int i = 0; i < paths.arraySize; ++i) {

                captions[i] = paths.GetArrayElementAtIndex(i).FindPropertyRelative("caption").stringValue;
                pathsStr[i] = paths.GetArrayElementAtIndex(i).FindPropertyRelative("path").stringValue;
                versions[i] = paths.GetArrayElementAtIndex(i).FindPropertyRelative("version").intValue;
                visitedFilesArr[i] = paths.GetArrayElementAtIndex(i).FindPropertyRelative("visitedFiles").stringValue;

            }

            for (int i = 0; i < pathsStr.Length; ++i) {
                
                var path = pathsStr[i];
                var caption = captions[i];
                var currentVersion = versions[i];
                var visitedFilesStr = visitedFilesArr[i];
                DataConfigGeneratorSettingsEditor.logs.Add(LogItem.LogWarning($"=========================="));
                DataConfigGeneratorSettingsEditor.logs.Add(LogItem.LogWarning($"** {caption}"));
                DataConfigGeneratorSettingsEditor.logs.Add(LogItem.LogWarning($"** Version: {currentVersion}"));
                DataConfigGeneratorSettingsEditor.logs.Add(LogItem.LogWarning($"=========================="));

                DataConfigGeneratorSettingsEditor.logs.Add(LogItem.LogWarning($"Receiving data from {path}"));
                DataConfigGeneratorSettingsEditor.logs.Add(LogItem.LogWarning($"Connecting {this.progressSymbol}"));
                var fileSize = 0L;
                var isDone = false;
                EditorCoroutines.StartCoroutine(this.GetFileSize(path, () => {
                    
                    DataConfigGeneratorSettingsEditor.logs[DataConfigGeneratorSettingsEditor.logs.Count - 1] = LogItem.LogWarning($"Connecting {this.progressSymbol}");
                    
                }, (size) => {
                    fileSize = size;
                    isDone = true;
                }));
                while (isDone == false) yield return null; 
                var request = UnityEngine.Networking.UnityWebRequest.Get(path);
                request.SendWebRequest();
                while (request.isDone == false) {

                    yield return null;
                    DataConfigGeneratorSettingsEditor.logs[DataConfigGeneratorSettingsEditor.logs.Count - 1] = LogItem.LogWarning($"Downloading: {request.downloadedBytes}/{fileSize} bytes ({(request.downloadProgress >= 0f ? Mathf.FloorToInt(request.downloadProgress * 100f).ToString() : "0")}%) {this.progressSymbol}");

                }
                
                DataConfigGeneratorSettingsEditor.logs[DataConfigGeneratorSettingsEditor.logs.Count - 1] = LogItem.LogWarning($"Downloaded: {request.downloadedBytes} bytes ({(request.downloadProgress >= 0f ? Mathf.FloorToInt(request.downloadProgress * 100f).ToString() : "0")}%)");

                var isOk = (request.isHttpError == false && request.isNetworkError == false && request.responseCode > 0);
                if (isOk == true) {
                
                    DataConfigGeneratorSettingsEditor.logs.Add(LogItem.LogWarning($"Response: {request.responseCode} OK"));

                } else {

                    DataConfigGeneratorSettingsEditor.logs.Add(LogItem.LogError($"Response error: {request.error}"));
                    
                }
                
                if (isOk == true) {

                    var generator = new DataConfigGenerator(currentVersion, request.downloadHandler.text, visitedConfigs, visitedFiles);
                    if (generator.status == Status.OK) {

                        generator.AddConfigs(newConfigs);
                        generator.Run();
                        newConfigs.AddRange(generator.GetCreatedConfigs());

                        if (paths.serializedObject != null) {
                        
                            paths.serializedObject.Update();
                            paths.GetArrayElementAtIndex(i).FindPropertyRelative("version").intValue = generator.version;
                            paths.GetArrayElementAtIndex(i).FindPropertyRelative("visitedFiles").stringValue = string.Join(";", visitedFiles);
                            paths.serializedObject.ApplyModifiedProperties();
                            
                        }

                        list.Add(generator);

                    } else if (generator.status == Status.UpToDate) {

                        var splitted = visitedFilesStr.Split(';');
                        foreach (var sp in splitted) {
                        
                            visitedFiles.Add(sp);

                        }
                        
                        list.Add(generator);

                    }
                    
                    var logs = generator.GetLogs();
                    DataConfigGeneratorSettingsEditor.logs.AddRange(logs);

                }

            }

            foreach (var generator in list) {

                generator.ClearLogs();
                generator.UpdateConfigs();

                var logs = generator.GetLogs();
                DataConfigGeneratorSettingsEditor.logs.AddRange(logs);

            }
            
            this.inProgress = false;

        }

    }

}