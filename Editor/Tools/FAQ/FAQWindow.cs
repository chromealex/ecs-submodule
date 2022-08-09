using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ME.ECSEditor;
using UnityEngine.UIElements;

public class FAQWindow : EditorWindow {

    private bool isUpdated;
    
    [MenuItem("ME.ECS/ℚ F.A.Q.", priority = 10004)]
    public static void Open() {

        var win = QuickStartWindow.CreateInstance<FAQWindow>();
        win.titleContent = new GUIContent("F.A.Q.");
        win.position = new Rect(100f, 100f, 600f, 800f);
        win.minSize = new Vector2(600f, 800f);
        win.maxSize = win.minSize;
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
        
    }

    private void UpdateData(string text) {

        this.container.Clear();
        var lines = text.Split('\n');
        Foldout foldout = null;
        for (int i = 0; i < lines.Length; ++i) {

            var line = lines[i];
            if (line.StartsWith("### ") == true) {
                
                // Begin Q-A
                var v = new Foldout();
                v.value = false;
                v.text = line.Substring(4);
                this.container.Add(v);
                foldout = v;

            } else if (foldout != null) {

                if (line.Trim().Length > 0) {

                    var match = System.Text.RegularExpressions.Regex.Match(line, "<answer>(.*?)</answer>");
                    var val = match.Groups[1].Value;
                    var label = new Label();
                    label.AddToClassList("answer");
                    label.text = val;
                    foldout.Add(label);

                }

            }
            
        }
        
    }

    private UnityEngine.Networking.UnityWebRequest request;
    private VisualElement container;
    public void OnEnable() {

        this.prevTime = EditorApplication.timeSinceStartup;
        
        var styles = EditorUtilities.Load<StyleSheet>("Editor/Tools/FAQ/EditorResources/FAQ-Styles.uss", isRequired: true);
        var domMain = EditorUtilities.Load<VisualTreeAsset>("Editor/Tools/FAQ/EditorResources/FAQ-Main.uxml", isRequired: true);

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

        var url = "https://raw.githubusercontent.com/chromealex/ecs/master/Docs/FAQ.md";
        var www = UnityEngine.Networking.UnityWebRequest.Get(url);
        www.SendWebRequest();
        this.request = www;
        this.isUpdated = false;

    }

}
