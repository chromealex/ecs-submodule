using System.Linq;

namespace ME.ECSEditor {

    using UnityEngine;
    using UnityEditor;
    using ME.ECS;
    using ME.ECS.DataConfigs;
    using UnityEngine.UIElements;

    [UnityEditor.CustomEditor(typeof(DataConfigTemplate), true)]
    [CanEditMultipleObjects]
    public class DataConfigTemplateEditor : DataConfigEditor {

        private SerializedProperty editorComment;
        
        protected override void OnEnable() {

            base.OnEnable();

            this.editorComment = this.serializedObject.FindProperty("editorComment");

        }

        public override void OnComponentMenu(GenericMenu menu, int index) {
            
            base.OnComponentMenu(menu, index);
            
            menu.AddItem(new GUIContent("Update value in configs"), false, () => {

                foreach (var target in this.targets) {
                
                    ((DataConfigTemplate)target).UpdateValue(index);

                }

            });
            
        }

        public override void OnAddComponent(System.Type type) {
            
            base.OnAddComponent(type);

            foreach (var target in this.targets) {
                
                ((DataConfigTemplate)target).OnAddComponent(type);

            }
            
            this.Save();
            
        }

        public override void OnRemoveComponent(System.Type type) {
            
            base.OnRemoveComponent(type);
            
            foreach (var target in this.targets) {
                
                ((DataConfigTemplate)target).OnRemoveComponent(type);

            }

            this.Save();

        }

        public override void OnAddComponentFromRemoveList(System.Type type) {
            
            base.OnAddComponentFromRemoveList(type);
            
            foreach (var target in this.targets) {
                
                ((DataConfigTemplate)target).OnAddComponentRemoveList(type);

            }

            this.Save();
            
        }

        public override void OnRemoveComponentFromRemoveList(System.Type type) {
            
            base.OnRemoveComponentFromRemoveList(type);
            
            foreach (var target in this.targets) {
                
                ((DataConfigTemplate)target).OnRemoveComponentRemoveList(type);

            }

            this.Save();

        }

        public override VisualElement CreateInspectorGUI() {
            
            var container = base.CreateInspectorGUI();
            var comment = new UnityEditor.UIElements.PropertyField(this.editorComment);
            comment.AddToClassList("comment-field");
            container.Insert(0, comment);
            return container;

        }

        public override void BuildUsedTemplates(UnityEngine.UIElements.VisualElement container, SerializedObject so) {
            
            var header = new Label("Used in Data Configs:");
            header.AddToClassList("header");
            container.Add(header);

            var templatesContainer = new VisualElement();
            container.Add(templatesContainer);
            var source = so.FindProperty("usedIn");

            var usedComponents = new System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfig>();
            BuildTemplates(usedComponents, templatesContainer, source);
            
        }

        private static void BuildTemplates(System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfig> usedComponents, VisualElement templatesContainer, SerializedProperty source) {
            
            source = source.Copy();
            templatesContainer.Clear();
            templatesContainer.AddToClassList("templates-container");
                
            for (int i = 0; i < source.arraySize; ++i) {
                    
                var elem = source.GetArrayElementAtIndex(i);
                var config = elem.objectReferenceValue;
                if (config == null) continue;
                    
                if (usedComponents.Contains(config) == false) usedComponents.Add((DataConfig)config);
                    
                var button = new Button(() => {
                        
                    EditorGUIUtility.PingObject(config);

                });
                button.AddToClassList("template-button");
                button.text = config.name;
                templatesContainer.Add(button);

            }

        }

        /*public override void OnInspectorGUI() {
            
            this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.editorComment);
            this.serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
            
            if (this.targets.Length == 1) {

                var target = (DataConfigTemplate)this.target;
                if (target.usedIn.Count > 0) {

                    GUILayoutExt.Separator(6f);
                    GUILayoutExt.DrawHeader("Used by Configs:");
                    GUILayoutExt.Separator();

                    var rect = new Rect(0f, 0f, EditorGUIUtility.currentViewWidth, 1000f);
                    var style = new GUIStyle("AssetLabel Partial");
                    var buttonRects = EditorGUIUtility.GetFlowLayoutedRects(rect, style, 4f, 4f, target.usedIn.Select(x => {

                        var config = x;
                        if (config == null) return string.Empty;

                        return config.name;

                    }).ToList());
                    GUILayout.BeginHorizontal();
                    GUILayout.EndHorizontal();
                    var areaRect = GUILayoutUtility.GetLastRect();
                    for (int i = 0; i < buttonRects.Count; ++i) areaRect.height = Mathf.Max(0f, buttonRects[i].yMax);

                    GUILayoutUtility.GetRect(areaRect.width, areaRect.height);

                    GUI.BeginGroup(areaRect);
                    for (int i = 0; i < target.usedIn.Count; ++i) {

                        var config = target.usedIn[i];
                        if (config == null) continue;

                        if (GUI.Button(buttonRects[i], config.name, style) == true) {

                            EditorGUIUtility.PingObject(config);

                        }

                    }

                    GUI.EndGroup();

                }

            }
            
        }*/

    }

}