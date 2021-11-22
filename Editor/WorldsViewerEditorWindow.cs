using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace ME.ECSEditor {

    public class WorldsViewerEditorWindow : EditorWindow {

        //[MenuItem("ME.ECS/Worlds Viewer 2...")]
        public static void ShowWorldViewer() {

            var win = WorldsViewerEditorWindow.CreateInstance<WorldsViewerEditorWindow>();
            win.titleContent = new GUIContent("Worlds Viewer");
            win.Show(immediateDisplay: false);

        }

        public void OnEnable() {

            var styles = EditorUtilities.Load<StyleSheet>("Editor/Core/DataConfigs/styles.uss", isRequired: true);
            var worldsViewerStyles = EditorUtilities.Load<StyleSheet>("Editor/WorldsViewer/EditorResources/styles.uss", isRequired: true);
            var domMain = EditorUtilities.Load<VisualTreeAsset>("Editor/WorldsViewer/EditorResources/Main.uxml", isRequired: true);
            var domWorld = EditorUtilities.Load<VisualTreeAsset>("Editor/WorldsViewer/EditorResources/World.uxml", isRequired: true);
            var domWorldContent = EditorUtilities.Load<VisualTreeAsset>("Editor/WorldsViewer/EditorResources/World-Content.uxml", isRequired: true);
            var domSystem = EditorUtilities.Load<VisualTreeAsset>("Editor/WorldsViewer/EditorResources/System.uxml", isRequired: true);
            var domModule = EditorUtilities.Load<VisualTreeAsset>("Editor/WorldsViewer/EditorResources/Module.uxml", isRequired: true);

            this.rootVisualElement.styleSheets.Add(styles);
            this.rootVisualElement.styleSheets.Add(worldsViewerStyles);
            this.rootVisualElement.Add(domMain.CloneTree());

            var worldsContainer = this.rootVisualElement.Q("Worlds");
            for (int i = 0; i < 10; ++i) {

                var item = domWorld.CloneTree();
                worldsContainer.Add(item);
                
                var systemsContainer = item.Q(className: "systems");
                for (int j = 0; j < 20; ++j) {
                
                    systemsContainer.Add(domSystem.CloneTree());
                    
                }

                var modulesContainer = item.Q(className: "modules");
                for (int j = 0; j < 20; ++j) {
                
                    modulesContainer.Add(domModule.CloneTree());
                    
                }

            }

            var contentContainer = this.rootVisualElement.Q("Content");
            contentContainer.Add(domWorldContent.CloneTree());

        }

    }

}