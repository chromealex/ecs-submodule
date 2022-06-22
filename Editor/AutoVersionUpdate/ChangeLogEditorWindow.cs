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
        
        public static void Create(string[] changedFilenames, string commitName, string version, System.Action<string> callback) {

            var v = version.Split('.');
            var majorMinor = v[0] + "." + v[1];
            var nextMajorMinor = v[0] + "." + (int.Parse(v[1]) + 1);

            for (int i = 0; i < changedFilenames.Length; ++i) {

                var changedFilename = changedFilenames[i];
                EditorUtilities.Load<TextAsset>("CHANGELOG.md", out var filePath);
                var win = ChangeLogEditorWindow.CreateInstance<ChangeLogEditorWindow>();
                win.commitName = (string.IsNullOrEmpty(commitName) == true ? string.Empty : " [" + commitName + "]");
                win.message = changedFilename + ": ";
                win.changedFilename = changedFilename + ": ";
                win.version = version;
                win.callback = callback;
                win.nextMajorMinorVersion = nextMajorMinor;
                win.majorMinorVersion = majorMinor;
                win.changeLogFile = System.IO.File.ReadAllText(filePath);
                win.changeLogFilePath = filePath;
                win.ShowModal();

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
            
            GUILayoutExt.Box(4f, 4f, () => {
            
                this.message = GUILayout.TextField(this.message, this.fixedFontStyle, GUILayout.MinHeight(60f));

                GUILayout.Space(10f);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button($"COMMIT TO {this.majorMinorVersion}", GUILayout.MinWidth(100f), GUILayout.Height(40f)) == true) {

                    this.Commit();

                }
                if (GUILayout.Button($"COMMIT AND UP VERSION TO {this.nextMajorMinorVersion}", GUILayout.MinWidth(100f), GUILayout.Height(40f)) == true) {

                    this.majorMinorVersion = this.nextMajorMinorVersion;
                    this.Commit();

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
    
            });
            
        }

        private void Commit() {

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