using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace ME.ECSEditor {

    public class ChangeLogEditorWindow : EditorWindow {

        private const int FONT_SIZE = 12;

        private const string linePrefix = "* ";
        
        private string version;
        private string majorMinorVersion;
        private string nextMajorMinorVersion;
        private System.Action<string> callback;
        private string changeLogFile;
        private string changeLogFilePath;
        private string commitName;

        private string changedFilename;
        private string message;
        
        private GUIStyle fixedFontStyle;
        private Font font;
        
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            
            //CreateAfterCompilation();

        }

        public static void Create(string[] changedFilenames, string commitName, string version, string realPath, string source, string result) {

            SessionState.SetString("temp.realPath", realPath);
            SessionState.SetString("temp.source", source);
            SessionState.SetString("temp.result", result);
            SessionState.SetString("temp.commitName", commitName);
            SessionState.SetString("temp.version", version);
            var addFiles = new List<string>();
            var len = SessionState.GetInt("temp.file.length", 0);
            foreach (var changed in changedFilenames) {
                var found = false;
                for (int i = 0; i < len; ++i) {
                    var file = SessionState.GetString("temp.file." + i, string.Empty);
                    if (file == changed) {
                        found = true;
                        break;
                    }
                }
                if (found == false) {
                    addFiles.Add(changed);
                }
            }

            SessionState.SetInt("temp.file.length", len + addFiles.Count);
            for (int i = len; i < len + addFiles.Count; ++i) {
                SessionState.SetString("temp.file." + i, addFiles[i - len]);
            }

        }
        
        public static void CreateAfterCompilation(Rect rect) {

            var len = SessionState.GetInt("temp.file.length", 0);
            if (len == 0) return;
            var realPath = SessionState.GetString("temp.realPath", string.Empty);
            var source = SessionState.GetString("temp.source", string.Empty);
            var result = SessionState.GetString("temp.result", string.Empty);
            var commitName = SessionState.GetString("temp.commitName", string.Empty);
            var version = SessionState.GetString("temp.version", string.Empty);
            
            var v = version.Split('.');
            var majorMinor = v[0] + "." + v[1];
            var nextMajorMinor = v[0] + "." + (int.Parse(v[1]) + 1);

            EditorUtilities.Load<TextAsset>("CHANGELOG.md", out var filePath);
            for (int i = 0; i < len; ++i) {

                var changedFilename = SessionState.GetString("temp.file." + i, string.Empty);
                var win = ChangeLogEditorWindow.CreateInstance<ChangeLogEditorWindow>();
                win.titleContent = new GUIContent("CHANGE LOG");
                win.position = new Rect(Screen.width * 0.5f - 300f, Screen.height * 0.5f - 60f, 600f, 120f);
                win.commitName = (string.IsNullOrEmpty(commitName) == true ? string.Empty : " [" + commitName + "]");
                win.message = changedFilename + ": ";
                win.changedFilename = changedFilename + ": ";
                win.version = version;
                win.callback = (versionToChange) => {
                    AutoVersionUpdateCompilation.Callback(version, versionToChange, realPath, source, result);
                };
                win.nextMajorMinorVersion = nextMajorMinor;
                win.majorMinorVersion = majorMinor;
                win.changeLogFile = System.IO.File.ReadAllText(filePath);
                win.changeLogFilePath = filePath;
                win.ShowAsDropDown(rect, win.position.size);
                win.Repaint();
                win.Focus();
                if (i == 0) break;

            }

        }

        private void CleanupFont() {
            
            if (this.font == null) {
                return;
            }

            Object.DestroyImmediate(this.font, true);
            this.font = null;
            
        }

        public void OnDisable() {
            
            this.CleanupFont();
            
        }

        private void OnGUI() {

            if (this.fixedFontStyle == null || this.font == null) {
                
                this.fixedFontStyle = new GUIStyle(GUI.skin.textArea);
                var fontName = Application.platform == RuntimePlatform.WindowsEditor ? "Consolas" : "Courier";

                this.CleanupFont();

                this.font = Font.CreateDynamicFontFromOSFont(fontName, FONT_SIZE);
                this.fixedFontStyle.normal = EditorStyles.label.normal;
                this.fixedFontStyle.wordWrap = true;
                this.fixedFontStyle.richText = true;
                this.fixedFontStyle.font = this.font;
                this.fixedFontStyle.fontSize = FONT_SIZE;
                
            }
            
            GUILayoutExt.Padding(4f, 4f, () => {
            
                this.message = GUILayout.TextField(this.message, this.fixedFontStyle, GUILayout.MinHeight(60f));

                GUILayout.Space(10f);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button($"CLOSE", GUILayout.MinWidth(100f), GUILayout.Height(40f)) == true) {

                    this.Close();

                }
                GUILayout.Space(2f);
                if (GUILayout.Button($"ADD TO {this.majorMinorVersion}", GUILayout.MinWidth(100f), GUILayout.Height(40f)) == true) {

                    this.Commit();
                    this.Close();

                }
                GUILayout.Space(2f);
                if (GUILayout.Button($"ADD AND UP VERSION TO {this.nextMajorMinorVersion}", GUILayout.MinWidth(100f), GUILayout.Height(40f)) == true) {

                    this.majorMinorVersion = this.nextMajorMinorVersion;
                    this.version = this.majorMinorVersion + ".0";
                    this.Commit(addDate: true);
                    this.Close();

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
    
            });
            
        }

        private void Commit(bool addDate = false) {

            SessionState.EraseInt("temp.file.length");
            if (string.IsNullOrEmpty(this.message) == false &&
                this.message != this.changedFilename) {

                this.message += this.commitName;
                
                var lines = this.changeLogFile.Split('\n').ToList();
                var lineIndex = -1;
                var versionFound = false;
                var withPrefix = true;
                for (int i = 0; i < lines.Count; ++i) {

                    var line = lines[i];
                    if (line.StartsWith("# ") == true) {

                        if (versionFound == false) {

                            // Read version
                            var match = System.Text.RegularExpressions.Regex.Match(line, @"# Version ([0-9]+)\.([0-9]+)");
                            if (match.Groups.Count > 0) {

                                var major = match.Groups[1].Value;
                                var minor = match.Groups[2].Value;
                                if (this.majorMinorVersion == major + "." + minor) {

                                    lineIndex = i;
                                    versionFound = true;
                                    
                                }

                            }

                        } else {

                            break;

                        }

                    } else if (versionFound == true) {

                        if (this.message.StartsWith(this.changedFilename) == true &&
                            line.StartsWith(linePrefix + this.changedFilename) == true) {

                            var resStr = this.message.Substring(this.changedFilename.Length, this.message.Length - this.changedFilename.Length);
                            if (string.IsNullOrEmpty(resStr.Trim()) == false) {

                                var source = line.Substring(linePrefix.Length + this.changedFilename.Length, line.Length - this.changedFilename.Length - linePrefix.Length);
                                if (string.IsNullOrEmpty(source.Trim()) == false) {
                                    lines[i] = "  * " + source.Trim();
                                    lines.Insert(i, linePrefix + this.changedFilename);
                                }

                                this.message = "  * " + resStr.Trim();
                                lineIndex = i;
                                withPrefix = false;
                                break;

                            } else {

                                lineIndex = -2;
                                break;

                            }

                        }
                        
                    }

                }

                if (lineIndex >= 0) {

                    if (withPrefix == true) {

                        // line found
                        lines.Insert(lineIndex + 1, linePrefix + this.message);

                    } else {
                        
                        // line found
                        lines.Insert(lineIndex + 1, this.message);

                    }

                } else if (lineIndex == -1) {

                    if (addDate == true) {
                        
                        lines.Insert(0, System.DateTime.UtcNow.ToString("dd/MM/yyyy"));
                        
                    }

                    // Add version header
                    lines.Insert(0, "");
                    lines.Insert(0, linePrefix + this.message);
                    lines.Insert(0, $"# Version {this.majorMinorVersion}");

                }

                if (lineIndex >= -1) {

                    var str = string.Join("\n", lines);
                    System.IO.File.WriteAllText(this.changeLogFilePath, str);

                }

            }

            //this.changeLogFile.Write(this.GetBytes(this.message));
            //this.changeLogFile.Close();
            this.callback?.Invoke(this.version);
            
        }

    }

}