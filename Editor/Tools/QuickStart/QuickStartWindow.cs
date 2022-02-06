using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ME.ECSEditor;
using UnityEngine.UIElements;

public class QuickStartWindow : EditorWindow {

    [MenuItem("ME.ECS/Quick Start")]
    public static void Open() {

        var win = QuickStartWindow.CreateInstance<QuickStartWindow>();
        win.titleContent = new GUIContent("Quick Start");
        win.position = new Rect(100f, 100f, 600f, 800f);
        win.minSize = new Vector2(600f, 800f);
        win.maxSize = win.minSize;
        win.ShowUtility();
        //win.Show();

    }

    private int GetStep() {

        return EditorPrefs.GetInt("ME.ECS.QuickStart.Step", 0);

    }

    private void SetStep(int index) {
        
        EditorPrefs.SetInt("ME.ECS.QuickStart.Step", index);
        
    }

    private void OnGUI() {
        
        if (Event.current != null && Event.current.keyCode == KeyCode.Escape) {
            
            this.Close();
            
        }
        
    }

    private void UpdateSteps() {

        var step = this.GetStep();
        var blocks = this.rootVisualElement.Query(className: "block").ToList();
        for (var i = 0; i < blocks.Count; ++i) {
            
            var block = blocks[i];
            block.RemoveFromClassList("complete");
            block.RemoveFromClassList("inactive");
            block.RemoveFromClassList("active");

            if (i == step) {
                
                block.AddToClassList("active");

            } else if (i < step) {
                
                block.AddToClassList("complete");
                
            } else {
                
                block.AddToClassList("inactive");
                
            }
            
        }
        
    }
    
    public void OnEnable() {

        var styles = EditorUtilities.Load<StyleSheet>("Editor/Tools/QuickStart/EditorResources/QuickStart-Styles.uss", isRequired: true);
        var domMain = EditorUtilities.Load<VisualTreeAsset>("Editor/Tools/QuickStart/EditorResources/QuickStart-Main.uxml", isRequired: true);

        this.rootVisualElement.styleSheets.Add(styles);
        this.rootVisualElement.Add(domMain.CloneTree());

        var mainView = this.rootVisualElement.Q<ScrollView>(className: "main-scroll-view");
        var footer = mainView.Q(className: "main-footer");
        var startAgain = footer.Q<Button>();
        startAgain.clicked += () => {
            this.SetStep(0);
            this.UpdateSteps();
            mainView.verticalScroller.value = 0f;
        };
        mainView.RemoveFromClassList("dark");
        mainView.RemoveFromClassList("light");
        if (EditorGUIUtility.isProSkin == true) {
            mainView.AddToClassList("dark");
        } else {
            mainView.AddToClassList("light");
        }

        var buttons = this.rootVisualElement.Query<Button>();
        var list = buttons.ToList();
        for (int i = 0; i < list.Count; ++i) {

            var index = i;
            var button = list[i];
            button.clicked += () => {
                if (index == this.GetStep()) {
                    this.SetStep(index + 1);
                    this.UpdateSteps();
                }
            };
            
        }

        this.UpdateSteps();

    }

}
