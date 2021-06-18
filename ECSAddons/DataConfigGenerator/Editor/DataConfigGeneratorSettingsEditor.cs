using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ME.ECS.DataConfigGenerator {
    
    using ME.ECSEditor;

    [CustomEditor(typeof(DataConfigGeneratorSettings))]
    public class DataConfigGeneratorSettingsEditor : Editor {

        private const int FONT_SIZE = 12;
        private static List<LogItem> logs;

        private SerializedProperty paths;
        private UnityEditorInternal.ReorderableList list;

        private GUIStyle versionStyle;
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
                var fontName = Application.platform == RuntimePlatform.WindowsEditor ? "Consolas" : "Courier";

                this.CleanupFont();

                this.font = Font.CreateDynamicFontFromOSFont(fontName, DataConfigGeneratorSettingsEditor.FONT_SIZE);
                this.fixedFontStyle.normal = EditorStyles.label.normal;
                this.fixedFontStyle.wordWrap = true;
                this.fixedFontStyle.richText = true;
                this.fixedFontStyle.font = this.font;
                this.fixedFontStyle.fontSize = DataConfigGeneratorSettingsEditor.FONT_SIZE;
                
            }

            if (this.versionStyle == null) {

                this.versionStyle = new GUIStyle(EditorStyles.miniBoldLabel);
                this.versionStyle.alignment = TextAnchor.MiddleRight;

            }


            this.serializedObject.Update();
            
            GUILayoutExt.Box(10f, 10f, () => {

                if (this.list == null) {
                
                    var items = this.paths;
                    const float offset = 4f;
                    const float padding = 4f;
                    this.list = new UnityEditorInternal.ReorderableList(this.serializedObject, items, true, false, true, true);
                    this.list.drawHeaderCallback = (rect) => {
                    
                        GUI.Label(rect, "Google Sheets");
                    
                    };
                    this.list.onAddCallback = (list) => {

                        items.arraySize = items.arraySize + 1;
                        var prop = items.GetArrayElementAtIndex(items.arraySize - 1);
                        prop.FindPropertyRelative("version").intValue = -1;
                        prop.FindPropertyRelative("caption").stringValue = string.Empty;
                        prop.FindPropertyRelative("path").stringValue = string.Empty;
                        prop.FindPropertyRelative("visitedFiles").stringValue = string.Empty;
                        
                    };
                    this.list.elementHeightCallback = index => {
                        
                        var prop = items.GetArrayElementAtIndex(index);
                        return offset * 2f + padding + EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("caption")) + EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("path"));
                        
                    }; 
                    this.list.drawElementCallback = (rect, index, active, focused) => {

                        var prop = items.GetArrayElementAtIndex(index);

                        var captionRect = new Rect(rect);
                        captionRect.y += offset;
                        captionRect.height = EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("caption"));
                        var pathRect = new Rect(rect);
                        pathRect.y += captionRect.height + padding;
                        pathRect.height = EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("path"));
                        var versionRect = new Rect(pathRect);
                        versionRect.height = EditorGUIUtility.singleLineHeight;
                        
                        EditorGUI.PropertyField(captionRect, prop.FindPropertyRelative("caption"));
                        EditorGUI.PropertyField(pathRect, prop.FindPropertyRelative("path"));
                        EditorGUI.LabelField(versionRect, $"Version: {prop.FindPropertyRelative("version").intValue}", this.versionStyle);
                        
                    };
                
                }
            
                this.list.DoLayoutList();

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

                this.logPosition = this.ScrollableSelectableLabel(this.logPosition, this.logsResult.ToString(), this.fixedFontStyle);
                //var rect = GUILayoutUtility.GetLastRect();
                //GUILayout.TextArea(this.logsResult.ToString(), this.fixedFontStyle, GUILayout.ExpandWidth(false), GUILayout.Width(rect.width));

            });

            this.serializedObject.ApplyModifiedProperties();
            
            this.Repaint();

        }

        private Vector3 logPosition;

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
        
        private Vector3 ScrollableSelectableLabel(Vector3 position, string text, GUIStyle style) {
            // Extract scroll position and width from position vector.
            Vector2 scrollPos = new Vector2(position.x, position.y);
            float width = position.z;
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            // Calculate height of text.
            float pixelHeight = style.CalcHeight(new GUIContent(text), width);
            EditorGUILayout.SelectableLabel(text, style, GUILayout.MinHeight(pixelHeight));
            // Update the width on repaint, based on width of the SelectableLabel's rectangle.
            if (Event.current.type == EventType.Repaint)
            {
                width = GUILayoutUtility.GetLastRect().width;
            }
            GUILayout.EndScrollView();
            // Put scroll position and width back into the Vector3 used to track position.
            return new Vector3(scrollPos.x, scrollPos.y, width);
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