using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ME.ECSEditor;
using UnityEngine.UIElements;

public class AddonsWindow : EditorWindow {

    private bool isUpdated;
    
    [MenuItem("ME.ECS/Add-ons...", priority = 10005)]
    public static void Open() {

        var win = QuickStartWindow.CreateInstance<AddonsWindow>();
        win.titleContent = new GUIContent("Add-ons");
        win.position = new Rect(100f, 100f, 600f, 800f);
        win.minSize = new Vector2(600f, 800f);
        win.maxSize = win.minSize;
        win.LoadPackages();
        win.ShowUtility();
        //win.Show();

    }

    private void OnGUI() {
        
        if (Event.current != null && Event.current.keyCode == KeyCode.Escape) {
            
            this.Close();
            
        }
        
    }

    private float alpha1 = 0f;
    private float alpha2 = 0.3f;
    private float alpha3 = 0.7f;
    private double prevTime;
    public void Update() {

        if (this.request != null) {

            if (this.isUpdated == false) {

                var mainView = this.rootVisualElement.Q<ScrollView>(className: "main-scroll-view");
                var loader = mainView.Q(className: "loader");
                loader.RemoveFromClassList("invisible");
                loader.AddToClassList("visible");

            }

            if (this.request.isDone == true && this.alpha1 >= 1f && string.IsNullOrEmpty(this.request.error) == true) {

                if (this.isUpdated == false) {
                    
                    var mainView = this.rootVisualElement.Q<ScrollView>(className: "main-scroll-view");
                    var loader = mainView.Q(className: "loader");
                    loader.RemoveFromClassList("visible");
                    loader.AddToClassList("invisible");
                    
                    this.isUpdated = true;
                    this.UpdateData(this.request.downloadHandler.text);

                }

            } else if (string.IsNullOrEmpty(this.request.error) == false) {
                
                Debug.LogWarning(this.request.error);
                
            } else {
                
                var dt = (float)(EditorApplication.timeSinceStartup - this.prevTime);
                this.prevTime = EditorApplication.timeSinceStartup;
                
                this.alpha1 += dt;
                this.alpha2 += dt;
                this.alpha3 += dt;
                
                var mainView = this.rootVisualElement.Q<ScrollView>(className: "main-scroll-view");
                var loader = mainView.Q(className: "loader");
                loader.RemoveFromClassList("invisible");
                loader.AddToClassList("visible");
                {
                    var item1 = loader.Q(className: "item1");
                    item1.style.opacity = Mathf.Repeat(this.alpha1, 1f);
                }
                {
                    var item2 = loader.Q(className: "item2");
                    item2.style.opacity = Mathf.Repeat(this.alpha2, 1f);
                }
                {
                    var item3 = loader.Q(className: "item3");
                    item3.style.opacity = Mathf.Repeat(this.alpha3, 1f);
                }

            }

        }

        this.LoadPackagesUpdate();
        this.UpdateRequests();

    }

    private void UpdateData(string text) {

        this.container.Clear();
        var lines = text.Split('\n');
        VisualElement container = null;
        for (int i = 0; i < lines.Length; ++i) {

            var line = lines[i];
            if (line.StartsWith("| <h3>") == true) {
                
                var match = System.Text.RegularExpressions.Regex.Match(line, "<h3>(.*?)</h3>");
                var val = match.Groups[1].Value;
                
                container = new VisualElement();
                container.AddToClassList("item");
                this.container.Add(container);

                var v = new Label();
                v.AddToClassList("caption");
                v.text = val;
                container.Add(v);

            } else {

                if (line.Trim().Length > 0) {

                    var match = System.Text.RegularExpressions.Regex.Match(line, @"\|(.*?)\|");
                    var val = match.Groups[1].Value;
                    if (val.Contains("<br>") == false) continue;

                    var splitted = val.Split(new string[1] { "<br>" }, System.StringSplitOptions.None);
                    
                    var label = new Label();
                    label.AddToClassList("description");
                    label.text = splitted[1];
                    container.Add(label);

                    var bottom = new VisualElement();
                    bottom.AddToClassList("bottom");
                    container.Add(bottom);

                    var url = splitted[2].Trim();
                    {
                        var currentVersion = new Label();
                        currentVersion.AddToClassList("current-version");
                        
                        var version = new Label();
                        version.AddToClassList("version");
                        this.GetVersion(container, version, currentVersion, url);
                        bottom.Add(version);
                        bottom.Add(currentVersion);
                    }
                    {
                        var button = new Button(() => { Application.OpenURL(url); });
                        button.AddToClassList("button");
                        var icon = new Image();
                        icon.image = EditorUtilities.Load<Texture>("Editor/Tools/Addons/EditorResources/github-icon.png");
                        button.Add(icon);
                        var buttonLabel = new Label("github");
                        button.Add(buttonLabel);
                        bottom.Add(button);
                    }

                }

            }
            
        }
        
    }

    private struct Item {

        public UnityEngine.Networking.UnityWebRequest request;
        public System.Action<string> callback;

    }

    [System.Serializable]
    private struct PackageInfo {

        public string name;
        public string version;

    }

    [System.Serializable]
    private struct ManifestInfo {

        public Dictionary<string, string> dependencies;

    }

    private List<Item> requests = new List<Item>();
    private void GetVersion(VisualElement container, Label label, Label currentVersion, string url) {
        
        currentVersion.text = "Checking installed version...";
        
        var www = UnityEngine.Networking.UnityWebRequest.Get(url.Replace("github.com", "raw.githubusercontent.com") + "/main/package.json");
        www.SendWebRequest();
        this.requests.Add(new Item() {
            request = www,
            callback = (data) => {
                var json = JsonUtility.FromJson<PackageInfo>(data);
                label.text = json.version;
                if (this.HasInstalledPackage(json, out var installedInfo) == true) {
                    if (installedInfo.version != json.version) {
                        currentVersion.AddToClassList("new-version-available");
                        container.AddToClassList("new-version-available");
                        currentVersion.text = $"Installed version: {installedInfo.version} (new version available)";
                    } else {
                        currentVersion.AddToClassList("up-to-date");
                        container.AddToClassList("up-to-date");
                        currentVersion.text = $"Installed version: {installedInfo.version} (up to date)";
                    }
                } else {
                    currentVersion.AddToClassList("not-installed");
                    container.AddToClassList("not-installed");
                    currentVersion.text = "Not installed";
                }
            },
        });
        
    }

    private List<PackageInfo> installedPackages = new List<PackageInfo>();
    private UnityEditor.PackageManager.Requests.ListRequest requestPackages;

    private void LoadPackagesUpdate() {

        if (this.requestPackages != null && this.requestPackages.IsCompleted == true) {

            var list = this.requestPackages.Result;
            if (list != null) {
                foreach (var item in list) {
                    this.installedPackages.Add(new PackageInfo() {
                        name = item.name,
                        version = item.version,
                    });
                }
            }
            this.requestPackages = null;

        }
        
    }
    private void LoadPackages() {

        this.requestPackages = UnityEditor.PackageManager.Client.List(true, false);
        
        var packages = AssetDatabase.FindAssets("package");
        foreach (var guid in packages) {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (System.IO.Path.GetExtension(path) == ".json") {
                var json = JsonUtility.FromJson<PackageInfo>(System.IO.File.ReadAllText(path));
                this.installedPackages.Add(json);
            }
        }

    }

    private bool HasInstalledPackage(PackageInfo refPackage, out PackageInfo packageInfo) {
        foreach (var item in this.installedPackages) {
            if (item.name == refPackage.name) {
                packageInfo = item;
                return true;
            }
        }
        packageInfo = default;
        return false;
    }

    private void UpdateRequests() {

        for (int i = this.requests.Count - 1; i >= 0; --i) {

            var req = this.requests[i];
            if (req.request.isDone == true && string.IsNullOrEmpty(req.request.error) == true) {
                
                req.callback.Invoke(req.request.downloadHandler.text);
                this.requests.RemoveAt(i);
                
            } else if (string.IsNullOrEmpty(req.request.error) == false) {
                
                Debug.LogWarning(req.request.error);
                this.requests.RemoveAt(i);
                
            }

        }
        
    }
    
    private UnityEngine.Networking.UnityWebRequest request;
    private VisualElement container;
    public void OnEnable() {

        this.prevTime = EditorApplication.timeSinceStartup;
        
        var styles = EditorUtilities.Load<StyleSheet>("Editor/Tools/Addons/EditorResources/Addons-Styles.uss", isRequired: true);
        var domMain = EditorUtilities.Load<VisualTreeAsset>("Editor/Tools/Addons/EditorResources/Addons-Main.uxml", isRequired: true);

        this.rootVisualElement.styleSheets.Add(styles);
        this.rootVisualElement.Add(domMain.CloneTree());

        var mainView = this.rootVisualElement.Q<ScrollView>(className: "main-scroll-view");
        this.container = mainView.Q("content");
        mainView.RemoveFromClassList("dark");
        mainView.RemoveFromClassList("light");
        if (EditorGUIUtility.isProSkin == true) {
            mainView.AddToClassList("dark");
        } else {
            mainView.AddToClassList("light");
        }

        var url = "https://raw.githubusercontent.com/chromealex/ecs-submodule/master/Addons.md";
        var www = UnityEngine.Networking.UnityWebRequest.Get(url);
        www.SendWebRequest();
        this.request = www;
        this.isUpdated = false;

    }

}
