using System.Linq;
using ME.ECS.Extensions;

namespace ME.ECSEditor {

    using UnityEngine;
    using UnityEditor;
    using ME.ECS;

    [UnityEditor.CustomEditor(typeof(ME.ECS.DataConfigs.DataConfig), true)]
    [CanEditMultipleObjects]
    public class DataConfigEditor : Editor {

        private struct Registry {

            public IStructComponentBase data;
            public int index;

        }

        private static readonly WorldsViewerEditor.WorldEditor multipleWorldEditor = new WorldsViewerEditor.WorldEditor();
        private static readonly System.Collections.Generic.Dictionary<Object, WorldsViewerEditor.WorldEditor> worldEditors = new System.Collections.Generic.Dictionary<Object, WorldsViewerEditor.WorldEditor>();

        private SerializedProperty sharedGroupId;
        private SerializedProperty componentsProperty;
        
        protected virtual void OnEnable() {

            this.sharedGroupId = this.serializedObject.FindProperty("sharedGroupId");
            this.componentsProperty = this.serializedObject.FindProperty("structComponents");
            
            foreach (var target in this.targets) {

                var config = (ME.ECS.DataConfigs.DataConfig)target;
                if (config.templates == null) config.templates = new string[0];
                foreach (var guid in config.templates) {

                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(path) == true) continue;
                    
                    var template = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
                    if (template != null) template.Use(config);

                }

            }
            
        }

        private bool CanMove(ME.ECS.DataConfigs.DataConfig dataConfig, int from, int to) {

            var arr = dataConfig.structComponents;
            if (to < 0 || to >= arr.Length) return false;

            return true;

        }
        
        private void MoveElement(ME.ECS.DataConfigs.DataConfig dataConfig, int from, int to) {
            
            var arr = dataConfig.structComponents;
            var old = arr[to];
            arr[to] = arr[from];
            arr[from] = old;

            GUILayoutExt.DropCachedFields();

            this.Save(dataConfig);

        }

        private string search;
        public override void OnInspectorGUI() {
            
            var dataConfig = (ME.ECS.DataConfigs.DataConfig)this.target;
            if (dataConfig is ME.ECS.DataConfigs.DataConfigTemplate == false) {

                foreach (var target in this.targets) {

                    var dc = (ME.ECS.DataConfigs.DataConfig)target;
                    if (dc.sharedGroupId == 0) {

                        dc.sharedGroupId = ME.ECS.MathUtils.GetHash(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(dc)));
                        this.Save(dc);

                    }

                }

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                this.serializedObject.Update();
                var sharedIdLabelStyle = new GUIStyle(EditorStyles.miniBoldLabel);
                sharedIdLabelStyle.alignment = TextAnchor.MiddleRight;
                EditorGUILayout.LabelField("Shared ID:", sharedIdLabelStyle);
                EditorGUILayout.PropertyField(this.sharedGroupId, new GUIContent(string.Empty), GUILayout.Width(100f));
                this.serializedObject.ApplyModifiedProperties();
                GUILayout.EndHorizontal();

            }

            this.search = GUILayoutExt.SearchField("Search", this.search);
            
            {
                var style = new GUIStyle(EditorStyles.toolbar);
                style.fixedHeight = 0f;
                style.stretchHeight = true;

                var backStyle = new GUIStyle(EditorStyles.label);
                backStyle.normal.background = Texture2D.whiteTexture;

                var slice = new ME.ECS.DataConfigs.DataConfigSlice();
                var isMultiple = false;
                if (this.targets.Length > 1) {

                    slice = ME.ECS.DataConfigs.DataConfigSlice.Distinct(this.targets.Cast<ME.ECS.DataConfigs.DataConfig>().ToArray());
                    isMultiple = true;

                } else {

                    var config = (ME.ECS.DataConfigs.DataConfig)this.target;
                    slice = new ME.ECS.DataConfigs.DataConfigSlice() {
                        configs = new[] {
                            config
                        },
                        structComponentsTypes = config.structComponents.Where(x => x != null).Select(x => x.GetType()).ToArray(),
                    };

                }

                var usedComponentsAll = new System.Collections.Generic.HashSet<System.Type>();
                foreach (var cfg in slice.configs) {

                    var componentTypes = cfg.GetStructComponentTypes();
                    foreach (var cType in componentTypes) {

                        if (usedComponentsAll.Contains(cType) == false) usedComponentsAll.Add(cType);

                    }

                    if (DataConfigEditor.worldEditors.TryGetValue(cfg, out var worldEditor) == false) {

                        worldEditor = new WorldsViewerEditor.WorldEditor();
                        DataConfigEditor.worldEditors.Add(cfg, worldEditor);

                    }

                }

                if (isMultiple == true) {

                    GUILayoutExt.DrawHeader("The Same Components:");

                    GUILayoutExt.Padding(4f, () => {

                        var kz = 0;
                        for (int i = 0; i < slice.structComponentsTypes.Length; ++i) {

                            var type = slice.structComponentsTypes[i];
                            var component = slice.configs[0].GetByType(slice.configs[0].structComponents, type);
                            if (GUILayoutExt.IsSearchValid(component, this.search) == false) continue;
                            var components = slice.configs.Select(x => x.GetByType(x.structComponents, type)).ToArray();

                            var backColor = GUI.backgroundColor;
                            GUI.backgroundColor = new Color(1f, 1f, 1f, kz++ % 2 == 0 ? 0f : 0.05f);

                            GUILayout.BeginVertical(backStyle);
                            {
                                GUI.backgroundColor = backColor;
                                var editor = WorldsViewerEditor.GetEditor(components);
                                if (editor != null) {

                                    EditorGUI.BeginChangeCheck();
                                    editor.OnDrawGUI();
                                    if (EditorGUI.EndChangeCheck() == true) {

                                        slice.Set(components);
                                        this.Save(slice.configs);

                                    }

                                } else {

                                    var componentName = GUILayoutExt.GetStringCamelCaseSpace(component.GetType().Name);
                                    var fieldsCount = GUILayoutExt.GetFieldsCount(component);
                                    if (fieldsCount == 0) {

                                        EditorGUI.BeginDisabledGroup(true);
                                        EditorGUILayout.Toggle(componentName, true);
                                        EditorGUI.EndDisabledGroup();

                                    } else if (fieldsCount == 1) {

                                        var changed = GUILayoutExt.DrawFields(DataConfigEditor.multipleWorldEditor, components, componentName);
                                        if (changed == true) {

                                            slice.Set(components);
                                            this.Save(slice.configs);

                                        }

                                    } else {

                                        GUILayout.BeginHorizontal();
                                        {
                                            GUILayout.Space(18f);
                                            GUILayout.BeginVertical();
                                            {

                                                var key = "ME.ECS.WorldsViewerEditor.FoldoutTypes." + component.GetType().FullName;
                                                var foldout = EditorPrefs.GetBool(key, true);
                                                GUILayoutExt.FoldOut(ref foldout, componentName, () => {

                                                    var changed = GUILayoutExt.DrawFields(DataConfigEditor.multipleWorldEditor, components);
                                                    if (changed == true) {

                                                        slice.Set(components);
                                                        this.Save(slice.configs);

                                                    }

                                                });
                                                EditorPrefs.SetBool(key, foldout);

                                            }
                                            GUILayout.EndVertical();
                                        }
                                        GUILayout.EndHorizontal();

                                    }

                                }

                                GUILayoutExt.DrawComponentHelp(component.GetType());
                                this.DrawShared(component);
                                
                            }
                            GUILayout.EndVertical();

                            GUILayoutExt.Separator();

                        }

                    });

                    GUILayoutExt.DrawAddComponentMenu(usedComponentsAll, (addType, isUsed) => {

                        foreach (var dataConfigInner in slice.configs) {

                            if (isUsed == true) {

                                this.OnRemoveComponent(addType);
                                usedComponentsAll.Remove(addType);
                                for (int i = 0; i < dataConfigInner.structComponents.Length; ++i) {

                                    if (dataConfigInner.structComponents[i].GetType() == addType) {

                                        var list = dataConfigInner.structComponents.ToList();
                                        list.RemoveAt(i);
                                        dataConfigInner.structComponents = list.ToArray();
                                        //dataConfigInner.OnScriptLoad();
                                        this.Save(dataConfigInner);
                                        break;

                                    }

                                }

                            } else {

                                usedComponentsAll.Add(addType);
                                System.Array.Resize(ref dataConfigInner.structComponents, dataConfigInner.structComponents.Length + 1);
                                dataConfigInner.structComponents[dataConfigInner.structComponents.Length - 1] = (IStructComponentBase)System.Activator.CreateInstance(addType);
                                //dataConfigInner.OnScriptLoad();
                                this.Save(dataConfigInner);
                                this.OnAddComponent(addType);

                            }

                        }

                    });

                    return;

                }

                GUILayoutExt.DrawHeader("Add Struct Components:");
                GUILayoutExt.Separator();

                GUILayoutExt.Padding(4f, () => {

                    var usedComponents = new System.Collections.Generic.HashSet<System.Type>();

                    this.serializedObject.Update();
                    
                    if (GUILayoutExt.DrawFieldsSingle(this.search, this.target, DataConfigEditor.multipleWorldEditor, dataConfig.structComponents,
                                                      (index, component, prop) => {
                                                          
                                                          GUILayout.BeginVertical();
                                                          
                                                      },
                                                      (index, component, prop) => {

                        if (component == null) {

                            GUILayout.EndVertical();
                            GUILayoutExt.Separator();
                            return;

                        }

                        usedComponents.Add(component.GetType());
                                                          
                        GUILayoutExt.DrawComponentHelp(component.GetType());
                        this.DrawComponentTemplatesUsage(dataConfig, component);
                        this.DrawShared(component);
                        
                        GUILayout.EndVertical();
                        
                        {
                            
                            var lastRect = GUILayoutUtility.GetLastRect();
                            if (Event.current.type == EventType.ContextClick && lastRect.Contains(Event.current.mousePosition) == true) {

                                var menu = new GenericMenu();
                                if (this.CanMove(dataConfig, index, index - 1) == true) {
                                    menu.AddItem(new GUIContent("Move Up"), false, () => { this.MoveElement(dataConfig, index, index - 1); });
                                } else {
                                    menu.AddDisabledItem(new GUIContent("Move Up"));
                                }

                                if (this.CanMove(dataConfig, index, index + 1) == true) {
                                    menu.AddItem(new GUIContent("Move Down"), false, () => { this.MoveElement(dataConfig, index, index + 1); });
                                } else {
                                    menu.AddDisabledItem(new GUIContent("Move Down"));
                                }

                                menu.AddItem(new GUIContent("Delete"), false, () => {

                                    var list = dataConfig.structComponents.ToList();
                                    this.OnRemoveComponent(list[index].GetType());
                                    list.RemoveAt(index);
                                    dataConfig.structComponents = list.ToArray();
                                    //dataConfig.OnScriptLoad();
                                    this.Save(dataConfig);

                                });

                                this.OnComponentMenu(menu, index);

                                menu.ShowAsContext();

                            }

                        }
                        
                        GUILayoutExt.Separator();

                    }, (index, component) => {
                                                          
                                                          this.Save(dataConfig);

                                                      }) == true) {
            
                        this.Save(dataConfig);
            
                    }
                    
                    GUILayoutExt.DrawAddComponentMenu(usedComponents, (addType, isUsed) => {

                        if (isUsed == true) {

                            this.OnRemoveComponent(addType);
                            usedComponents.Remove(addType);
                            for (int i = 0; i < dataConfig.structComponents.Length; ++i) {

                                if (dataConfig.structComponents[i].GetType() == addType) {

                                    var list = dataConfig.structComponents.ToList();
                                    list.RemoveAt(i);
                                    dataConfig.structComponents = list.ToArray();
                                    //dataConfig.OnScriptLoad();
                                    this.Save(dataConfig);
                                    break;

                                }

                            }

                        } else {

                            usedComponents.Add(addType);
                            System.Array.Resize(ref dataConfig.structComponents, dataConfig.structComponents.Length + 1);
                            dataConfig.structComponents[dataConfig.structComponents.Length - 1] = (IStructComponentBase)System.Activator.CreateInstance(addType);
                            //dataConfig.OnScriptLoad();
                            this.Save(dataConfig);
                            this.OnAddComponent(addType);

                        }

                    });

                    this.serializedObject.ApplyModifiedProperties();
                    
                });
                
                GUILayoutExt.DrawHeader("Remove Struct Components:");
                GUILayoutExt.Separator();

                // Remove struct components
                GUILayoutExt.Padding(4f, () => {

                    var usedComponents = new System.Collections.Generic.HashSet<System.Type>();

                    var kz = 0;
                    var registries = dataConfig.removeStructComponents;
                    for (int i = 0; i < registries.Length; ++i) {

                        var registry = registries[i];
                        if (GUILayoutExt.IsSearchValid(registry, this.search) == false) continue;
                        var type = registry.GetType();

                        usedComponents.Add(type);

                        var backColor = GUI.backgroundColor;
                        GUI.backgroundColor = new Color(1f, 1f, 1f, kz++ % 2 == 0 ? 0f : 0.05f);

                        GUILayout.BeginVertical(backStyle);
                        {
                            GUI.backgroundColor = backColor;
                            var componentName = GUILayoutExt.GetStringCamelCaseSpace(type.Name);

                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.Toggle(componentName, true);
                            EditorGUI.EndDisabledGroup();

                            GUILayoutExt.DrawComponentHelp(type);
                            this.DrawComponentTemplatesUsage(dataConfig, dataConfig.removeStructComponents[i]);

                        }
                        GUILayout.EndVertical();

                        GUILayoutExt.Separator();

                    }

                    GUILayoutExt.DrawAddComponentMenu(usedComponents, (addType, isUsed) => {

                        if (isUsed == true) {

                            this.OnRemoveComponentFromRemoveList(addType);
                            usedComponents.Remove(addType);
                            for (int i = 0; i < dataConfig.removeStructComponents.Length; ++i) {

                                if (dataConfig.removeStructComponents[i].GetType() == addType) {

                                    var list = dataConfig.removeStructComponents.ToList();
                                    list.RemoveAt(i);
                                    dataConfig.removeStructComponents = list.ToArray();
                                    //dataConfig.OnScriptLoad();
                                    this.Save(dataConfig);
                                    break;

                                }

                            }

                        } else {

                            usedComponents.Add(addType);
                            System.Array.Resize(ref dataConfig.removeStructComponents, dataConfig.removeStructComponents.Length + 1);
                            dataConfig.removeStructComponents[dataConfig.removeStructComponents.Length - 1] = (IStructComponentBase)System.Activator.CreateInstance(addType);
                            //dataConfig.OnScriptLoad();
                            this.Save(dataConfig);
                            this.OnAddComponentFromRemoveList(addType);

                        }

                    });

                });

                if ((dataConfig is ME.ECS.DataConfigs.DataConfigTemplate) == false) this.DrawTemplates(dataConfig);

            }
            
        }

        private void DrawShared(IStructComponentBase component) {
            
            GUILayout.BeginHorizontal();
            {
                var styleLabel = new GUIStyle(EditorStyles.miniLabel);
                styleLabel.richText = true;
                if (component is IComponentShared) {

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.LabelField(new GUIContent("<color=#7f7><i>Shared</i></color>"), styleLabel);
                    EditorGUI.EndDisabledGroup();

                    GUILayout.Space(10f);

                }

                if (component is IComponentStatic) {

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.LabelField(new GUIContent("<color=#7ff><i>Static</i></color>"), styleLabel);
                    EditorGUI.EndDisabledGroup();

                }
            }
            GUILayout.EndHorizontal();

        }

        protected virtual void OnComponentMenu(GenericMenu menu, int index) { }
        protected virtual void OnAddComponentFromRemoveList(System.Type type) { }
        protected virtual void OnRemoveComponentFromRemoveList(System.Type type) { }
        protected virtual void OnAddComponent(System.Type type) { }
        protected virtual void OnRemoveComponent(System.Type type) { }

        private void DrawTemplates(ME.ECS.DataConfigs.DataConfig dataConfig) {

            GUILayoutExt.DrawHeader("Used Templates:");
            GUILayoutExt.Separator();

            var usedComponents = new System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate>();
            if (dataConfig.templates != null) {

                var rect = new Rect(0f, 0f, EditorGUIUtility.currentViewWidth, 1000f);
                var style = new GUIStyle("AssetLabel Partial");
                var buttonRects = EditorGUIUtility.GetFlowLayoutedRects(rect, style, 4f, 4f, dataConfig.templates.Select(x => {
                    
                    var guid = x;
                    if (string.IsNullOrEmpty(guid) == true) return string.Empty;
                    
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(path) == true) return string.Empty;
                    
                    var template = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
                    if (template == null) return string.Empty;

                    return template.name;

                }).ToList());
                GUILayout.BeginHorizontal();
                GUILayout.EndHorizontal();
                var areaRect = GUILayoutUtility.GetLastRect();
                for (int i = 0; i < buttonRects.Count; ++i) areaRect.height = Mathf.Max(0f, buttonRects[i].yMax);

                GUILayoutUtility.GetRect(areaRect.width, areaRect.height);
                
                GUI.BeginGroup(areaRect);
                for (int i = 0; i < dataConfig.templates.Length; ++i) {

                    var guid = dataConfig.templates[i];
                    if (string.IsNullOrEmpty(guid) == true) continue;

                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(path) == true) continue;

                    var template = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
                    if (template == null) continue;

                    if (usedComponents.Contains(template) == false) usedComponents.Add(template);
                    
                }

                for (int i = 0; i < dataConfig.templates.Length; ++i) {

                    var guid = dataConfig.templates[i];
                    if (string.IsNullOrEmpty(guid) == true) continue;
                    
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(path) == true) continue;
                    
                    var template = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
                    if (template == null) continue;

                    if (GUI.Button(buttonRects[i], template.name, style) == true) {
                        
                        EditorGUIUtility.PingObject(template);
                        //this.RemoveTemplate(dataConfig, template, usedComponents);
                        
                    }

                }
                GUI.EndGroup();

            }

            GUILayoutExt.DrawManageDataConfigTemplateMenu(usedComponents, (template, isUsed) => {

                var path = AssetDatabase.GetAssetPath(template);
                var guid = AssetDatabase.AssetPathToGUID(path);
                if (string.IsNullOrEmpty(guid) == true) return;
                
                if (isUsed == true) {

                    usedComponents.Remove(template);
                    for (int i = 0; i < dataConfig.templates.Length; ++i) {

                        if (dataConfig.templates[i] == guid) {

                            this.RemoveTemplate(dataConfig, template, usedComponents);
                            break;

                        }

                    }

                } else {

                    usedComponents.Add(template);
                    if (dataConfig.templates == null) dataConfig.templates = new string[0];
                    System.Array.Resize(ref dataConfig.templates, dataConfig.templates.Length + 1);
                    dataConfig.templates[dataConfig.templates.Length - 1] = guid;
                    dataConfig.AddTemplate(template);
                    //dataConfig.OnScriptLoad();
                    this.Save(dataConfig);
                    
                }

            });

        }

        private void DrawComponentTemplatesUsage(ME.ECS.DataConfigs.DataConfig dataConfig, object component) {

            if (dataConfig.templates != null && dataConfig.templates.Length > 1) {
                
                GUILayout.BeginHorizontal();
                var list = new System.Collections.Generic.List<ME.ECS.DataConfigs.DataConfigTemplate>();
                for (int i = 0; i < dataConfig.templates.Length; ++i) {

                    var guid = dataConfig.templates[i];
                    if (string.IsNullOrEmpty(guid) == true) continue;
                    
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(path) == true) continue;
                    
                    var template = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
                    if (template == null) continue;

                    if (template.HasByType(template.structComponents, component) == true) {

                        list.Add(template);

                    }

                }

                if (list.Count > 1) {

                    var style = new GUIStyle("AssetLabel Partial");
                    foreach (var item in list) {

                        GUILayout.Label(item.name, style);

                    }

                }
                GUILayout.EndHorizontal();
                
            }
            
        }
        
        private void RemoveTemplate(ME.ECS.DataConfigs.DataConfig dataConfig, ME.ECS.DataConfigs.DataConfigTemplate template, System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate> allTemplates) {
            
            var path = AssetDatabase.GetAssetPath(template);
            var guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid) == true) return;

            var list = dataConfig.templates.ToList();
            list.Remove(guid);
            dataConfig.templates = list.ToArray();
            dataConfig.RemoveTemplate(template, allTemplates);
            //dataConfig.OnScriptLoad();
            this.Save(dataConfig);
            
        }

        protected void Save(ME.ECS.DataConfigs.DataConfig dataConfig, bool dirtyOnly = false) {
            
            dataConfig.Save(dirtyOnly);
            
        }

        protected void Save(ME.ECS.DataConfigs.DataConfig[] dataConfigs, bool dirtyOnly = false) {

            foreach (var dataConfig in dataConfigs) dataConfig.Save(dirtyOnly);

        }

    }

}
