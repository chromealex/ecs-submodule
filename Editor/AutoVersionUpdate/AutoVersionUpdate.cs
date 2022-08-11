using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

namespace ME.ECSEditor {

    [InitializeOnLoad]
    public static class AutoVersionUpdateCompilation {

        static System.Type m_toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        static System.Type m_guiViewType = typeof(Editor).Assembly.GetType("UnityEditor.GUIView");
        #if UNITY_2020_1_OR_NEWER
        static System.Type m_iWindowBackendType = typeof(Editor).Assembly.GetType("UnityEditor.IWindowBackend");
        static System.Reflection.PropertyInfo m_windowBackend = m_guiViewType.GetProperty("windowBackend",
                                                                                          System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        static System.Reflection.PropertyInfo m_viewVisualTree = m_iWindowBackendType.GetProperty("visualTree",
                                                                                                  System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        #else
		static PropertyInfo m_viewVisualTree = m_guiViewType.GetProperty("visualTree",
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        #endif
        static System.Reflection.FieldInfo m_imguiContainerOnGui = typeof(IMGUIContainer).GetField("m_OnGUIHandler",
                                                                                                   System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        static ScriptableObject m_currentToolbar;
        
        internal static UnityEditor.Compilation.Assembly[] assemblies;
        internal static string currentVersion;

        internal static UnityEngine.Networking.UnityWebRequest request;
        internal static bool isInitialized;
        
        static AutoVersionUpdateCompilation() {

            EditorApplication.delayCall += () => SessionState.SetBool("AutoVersionUpdate.initialized", true);

            var con = SessionState.GetString("AutoVersionUpdate.Contributors", string.Empty);
            if (string.IsNullOrEmpty(con) == false) return;
            
            var url = "https://raw.githubusercontent.com/chromealex/ecs/master/Docs/contributors.txt";
            //Debug.Log("Request contributors");
            var www = UnityEngine.Networking.UnityWebRequest.Get(url);
            www.SendWebRequest();
            AutoVersionUpdateCompilation.request = www;
            
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
            
        }

        private static void OnUpdate() {
            
            AutoVersionUpdateCompilation.AddButton();
            
        }

        private static void AddButton() {
            
            var username = UnityEditor.CloudProjectSettings.userName;
            if (AutoVersionUpdate.IsUserEditor(username, out var commitName) == false) return;
            
            if (m_currentToolbar == null)
            {
                // Find toolbar
                var toolbars = Resources.FindObjectsOfTypeAll(m_toolbarType);
                m_currentToolbar = toolbars.Length > 0 ? (ScriptableObject) toolbars[0] : null;
                if (m_currentToolbar != null) {
                    #if UNITY_2021_OR_NEWER
                    var root = m_currentToolbar.GetType().GetField("m_Root", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var rawRoot = root.GetValue(m_currentToolbar);
                    var mRoot = rawRoot as VisualElement;
                    var element = mRoot.Q("ToolbarZoneRightAlign");
                    DrawButton(element);
                    #else
                    #if UNITY_2020_1_OR_NEWER
                    var windowBackend = m_windowBackend.GetValue(m_currentToolbar);
                    // Get it's visual tree
                    var visualTree = (VisualElement) m_viewVisualTree.GetValue(windowBackend, null);
                    #else
			        // Get it's visual tree
			        var visualTree = (VisualElement) m_viewVisualTree.GetValue(m_currentToolbar, null);
                    #endif

                    // Get first child which 'happens' to be toolbar IMGUIContainer
                    var container = (IMGUIContainer) visualTree[0];

                    // (Re)attach handler
                    var handler = (System.Action) m_imguiContainerOnGui.GetValue(container);
                    handler -= OnGUI;
                    handler += OnGUI;
                    m_imguiContainerOnGui.SetValue(container, handler);
					        
                    #endif
                }
            }
            
        }

        #if UNITY_2019_3_OR_NEWER
        public const float space = 8;
        #else
		public const float space = 10;
        #endif
        public const float largeSpace = 20;
        public const float buttonWidth = 32;
        public const float dropdownWidth = 80;
        #if UNITY_2019_1_OR_NEWER
        public const float playPauseStopWidth = 140;
        #else
		public const float playPauseStopWidth = 100;
        #endif

        static int m_toolCount;
        static GUIStyle m_commandStyle = null;
        
        static void OnGUI()
		{
            System.Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
			
            #if UNITY_2019_1_OR_NEWER
            string fieldName = "k_ToolCount";
            #else
			string fieldName = "s_ShownToolIcons";
            #endif
			
            System.Reflection.FieldInfo toolIcons = toolbarType.GetField(fieldName,
                                                                         System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			
            #if UNITY_2019_3_OR_NEWER
            m_toolCount = toolIcons != null ? ((int) toolIcons.GetValue(null)) : 8;
            #elif UNITY_2019_1_OR_NEWER
			m_toolCount = toolIcons != null ? ((int) toolIcons.GetValue(null)) : 7;
            #elif UNITY_2018_1_OR_NEWER
			m_toolCount = toolIcons != null ? ((Array) toolIcons.GetValue(null)).Length : 6;
            #else
			m_toolCount = toolIcons != null ? ((Array) toolIcons.GetValue(null)).Length : 5;
            #endif

			// Create two containers, left and right
			// Screen is whole toolbar

			if (m_commandStyle == null)
			{
				m_commandStyle = new GUIStyle("CommandLeft");
			}

			var screenWidth = EditorGUIUtility.currentViewWidth;

			// Following calculations match code reflected from Toolbar.OldOnGUI()
			float playButtonsPosition = Mathf.RoundToInt ((screenWidth - playPauseStopWidth) / 2);

			Rect leftRect = new Rect(0, 0, screenWidth, Screen.height);
			leftRect.xMin += space; // Spacing left
			leftRect.xMin += buttonWidth * m_toolCount; // Tool buttons
#if UNITY_2019_3_OR_NEWER
			leftRect.xMin += space; // Spacing between tools and pivot
#else
			leftRect.xMin += largeSpace; // Spacing between tools and pivot
#endif
			leftRect.xMin += 64 * 2; // Pivot buttons
			leftRect.xMax = playButtonsPosition;

			Rect rightRect = new Rect(0, 0, screenWidth, Screen.height);
			rightRect.xMin = playButtonsPosition;
			rightRect.xMin += m_commandStyle.fixedWidth * 3; // Play buttons
			rightRect.xMax = screenWidth;
			rightRect.xMax -= space; // Spacing right
			rightRect.xMax -= dropdownWidth; // Layout
			rightRect.xMax -= space; // Spacing between layout and layers
			rightRect.xMax -= dropdownWidth; // Layers
#if UNITY_2019_3_OR_NEWER
			rightRect.xMax -= space; // Spacing between layers and account
#else
			rightRect.xMax -= largeSpace; // Spacing between layers and account
#endif
			rightRect.xMax -= dropdownWidth; // Account
			rightRect.xMax -= space; // Spacing between account and cloud
			rightRect.xMax -= buttonWidth; // Cloud
			rightRect.xMax -= space; // Spacing between cloud and collab
			rightRect.xMax -= 78; // Colab

			// Add spacing around existing controls
			leftRect.xMin += space;
			leftRect.xMax -= space;
			rightRect.xMin += space;
			rightRect.xMax -= space;

			// Add top and bottom margins
#if UNITY_2019_3_OR_NEWER
			leftRect.y = 4;
			leftRect.height = 22;
			rightRect.y = 4;
			rightRect.height = 22;
#else
			leftRect.y = 5;
			leftRect.height = 24;
			rightRect.y = 5;
			rightRect.height = 24;
#endif

			if (rightRect.width > 0)
			{
				GUILayout.BeginArea(rightRect);
				GUILayout.BeginHorizontal();

                {

                    if (GUILayout.Button("ME.ECS Collab", EditorStyles.toolbarPopup, GUILayout.Width(100f)) == true) {
                        
                        ChangeLogEditorWindow.CreateAfterCompilation(rightRect);

                    }

                }

				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}
        
        #if UNITY_2021_OR_NEWER
        private static void DrawButton(VisualElement element) {
            
            var uss = EditorUtilities.Load<StyleSheet>("Editor/AutoVersionUpdate/styles.uss");
            element.styleSheets.Add(uss);
            var dropdown = new DropdownField();
            dropdown.value = "ME.ECS Collab";
            dropdown.AddToClassList("autoupdate-button");
            dropdown.RemoveFromClassList("unity-button");
            dropdown.RemoveFromClassList("unity-base-field");
            dropdown.Q(className:"unity-base-field__input").RemoveFromClassList("unity-base-field__input");
            dropdown.RegisterCallback<ClickEvent>(evt => {

                ChangeLogEditorWindow.CreateAfterCompilation(dropdown.worldBound);

            });
            element.Add(dropdown);
            
        }
        #endif

        internal static void UpdatePackageVersion(string packagePath, string[] filepaths, string commitName) {

            var package = AssetDatabase.LoadAssetAtPath<TextAsset>(packagePath);//EditorUtilities.Load<TextAsset>("package.json", out var realPath);
            var realPath = packagePath;
            if (package != null) {

                var files = new string[filepaths.Length];
                var rootDir = System.IO.Path.GetDirectoryName(realPath);
                for (int i = 0; i < filepaths.Length; ++i) {
                
                    var d = System.IO.Path.GetDirectoryName(filepaths[i]);
                    var dir = d.Substring(rootDir.Length, d.Length - rootDir.Length);
                    var splitted = dir.Split('/');
                    if (splitted.Length > 2) {
                        files[i] = splitted[1] + "." + splitted[2]; //System.IO.Path.GetFileNameWithoutExtension(filepaths[i]);
                    } else {
                        files[i] = splitted[0];
                    }
                    
                }

                Debug.Log(realPath);
                var source = package.text;
                var pattern = @"""version"":\s*""(?<major>[0-9]+).(?<minor>[0-9]+).(?<build>[0-9]+)""";
                var result = Regex.Replace(source, pattern, AutoVersionUpdateCompilation.UpBuild);
                ChangeLogEditorWindow.Create(files, commitName, currentVersion, realPath, source, result);
                AutoVersionUpdateCompilation.Callback(currentVersion, currentVersion, realPath, source, result);
                
            }

        }

        public static void Callback(string sourceVersion, string ver, string realPath, string source, string text) {

            var sourceVersionCheck = string.Join(".", sourceVersion.Split('.').Take(2).ToArray());
            var verCheck = string.Join(".", ver.Split('.').Take(2).ToArray());
            
            Debug.Log($"{realPath}: Version up {sourceVersion} => {ver}");
            if (sourceVersionCheck == verCheck) {
                // major/minor not changed
                System.IO.File.WriteAllText(realPath, text);
            } else {
                // version changed
                newMajorMinorVersion = ver;
                var pattern = @"""version"":\s*""(?<major>[0-9]+).(?<minor>[0-9]+).(?<build>[0-9]+)""";
                var newResult = Regex.Replace(source, pattern, AutoVersionUpdateCompilation.SetMajorMinorVersion);
                System.IO.File.WriteAllText(realPath, newResult);
            }

        }

        private static string newMajorMinorVersion;
        private static string SetMajorMinorVersion(System.Text.RegularExpressions.Match m) {
            return @"""version"": """ + AutoVersionUpdateCompilation.newMajorMinorVersion + @"""";
        } 

        private static string UpBuild(System.Text.RegularExpressions.Match m) {
            int major = int.Parse(m.Groups["major"].Value);
            int minor = int.Parse(m.Groups["minor"].Value);
            int build = int.Parse(m.Groups["build"].Value) + 1;
            currentVersion = $"{major}.{minor}.{build}";
            return @"""version"": """ + major.ToString() + "." + minor.ToString() + "." + build.ToString() + @"""";
        } 

    }
    
    public class AutoVersionUpdate : AssetPostprocessor {

        internal static bool IsUserEditor(string username, out string commitName) {
            
            // Check if its me ;)
            //var dir = System.IO.Directory.Exists("/Users/aleksandrfeer/Projects");
            //if (dir == true) return true;

            commitName = string.Empty;
            var con = SessionState.GetString("AutoVersionUpdate.Contributors", string.Empty);
            if (string.IsNullOrEmpty(con) == true) {
                
                if (AutoVersionUpdateCompilation.request.isDone == false) return false;
                
                var text = AutoVersionUpdateCompilation.request.downloadHandler.text;
                SessionState.SetString("AutoVersionUpdate.Contributors", con);

                con = text;

            }

            var lines = con.Split('\n');
            for (int i = 0; i < lines.Length; ++i) {
                var spl = lines[i].Split(' ');
                if (username == spl[0]) {
                    if (spl.Length > 1) commitName = spl[1];
                    return true;
                }
            }

            return false;

        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {

            if (SessionState.GetBool("AutoVersionUpdate.initialized", false) == false) return;
            
            var username = UnityEditor.CloudProjectSettings.userName;
            if (IsUserEditor(username, out var commitName) == false) return;
            
            if (AutoVersionUpdateCompilation.assemblies == null) AutoVersionUpdateCompilation.assemblies = UnityEditor.Compilation.CompilationPipeline.GetAssemblies();

            var packagePath = string.Empty;
            var assetReimport = new System.Collections.Generic.List<string>();
            var found = false;
            foreach (var asset in importedAssets) {

                if (asset.EndsWith(".cs") == true) {

                    var assembly = AutoVersionUpdateCompilation.assemblies.FirstOrDefault(a => a.name.Contains("ME.ECS") == true && a.sourceFiles.Contains(asset) == true);
                    if (assembly != null) {

                        var srcDir = System.IO.Path.GetDirectoryName(asset);
                        packagePath = GetPackageFilePath(srcDir);
                        
                        assetReimport.Add(asset);
                        found = true;
                    
                    }

                }
            
            }
            
            if (found == true) AutoVersionUpdateCompilation.UpdatePackageVersion(packagePath, assetReimport.ToArray(), commitName);
            
        }

        private static string GetPackageFilePath(string path) {
            
            var result = System.IO.Path.Combine(path, "package.json");
            if (System.IO.File.Exists(result) == true) return result;

            var prevPath = path;
            var splitted = path.Split('/');
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < splitted.Length - 1; ++i) {
                sb.Append(splitted[i]);
                if (i < splitted.Length - 2) sb.Append('/');
            }
            path = sb.ToString();
            
            //path = new System.IO.DirectoryInfo(path).Parent.ToString();
            if (string.IsNullOrEmpty(path) == true || path.Length <= 1) return string.Empty;
            if (prevPath.Length == path.Length) return string.Empty;
            
            return GetPackageFilePath(path);
        }

    }

}