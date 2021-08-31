using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.CodeDom.Compiler;

namespace ME.ECSEditor {
    
    using ME.ECS;

    public interface ICustomFieldEditor : IGUIEditorBase {

	    bool DrawGUI(string caption, object instance, int instanceArrIndex, System.Reflection.FieldInfo fieldInfo, ref object value, bool typeCheckOnly, bool hasMultipleDifferentValues);

    }

    public class CustomFieldEditorAttribute : CustomEditorAttribute {

	    public CustomFieldEditorAttribute(System.Type type) : base(type, 0) {}

    }
    
    public abstract class CustomEditorAttribute : System.Attribute {

        public System.Type type;
        public int order;

        protected CustomEditorAttribute(System.Type type, int order = 0) {

            this.type = type;
            this.order = order;

        }

    }

    public class ComponentCustomEditorAttribute : CustomEditorAttribute {

	    public ComponentCustomEditorAttribute(System.Type type, int order = 0) : base(type, order) {}

    }

    public static class GUILayoutExt {

	    public struct HandlesAlphaUsing : IDisposable {

		    private Color oldColor;

		    public HandlesAlphaUsing(float alpha) {

			    this.oldColor = Handles.color;
			    var c = this.oldColor;
			    c.a = alpha;
			    Handles.color = c;

		    }
		    
		    public void Dispose() {

			    Handles.color = this.oldColor;

		    }

	    }
	    
	    public struct HandlesColorUsing : IDisposable {

		    private Color oldColor;

		    public HandlesColorUsing(Color color) {

			    this.oldColor = Handles.color;
			    Handles.color = color;

		    }
		    
		    public void Dispose() {

			    Handles.color = this.oldColor;

		    }

	    }

	    public struct GUIColorUsing : IDisposable {

		    private Color oldColor;

		    public GUIColorUsing(Color color) {

			    this.oldColor = GUI.color;
			    GUI.color = color;

		    }
		    
		    public void Dispose() {

			    GUI.color = this.oldColor;

		    }

	    }

	    public struct GUIAlphaUsing : IDisposable {

		    private Color oldColor;

		    public GUIAlphaUsing(float alpha) {

			    this.oldColor = GUI.color;
			    var c = this.oldColor;
			    c.a = alpha;
			    GUI.color = c;

		    }
		    
		    public void Dispose() {

			    GUI.color = this.oldColor;

		    }

	    }

	    public struct GUIBackgroundAlphaUsing : IDisposable {

		    private Color oldColor;

		    public GUIBackgroundAlphaUsing(float alpha) {

			    this.oldColor = GUI.backgroundColor;
			    var c = this.oldColor;
			    c.a = alpha;
			    GUI.backgroundColor = c;

		    }
		    
		    public void Dispose() {

			    GUI.backgroundColor = this.oldColor;

		    }

	    }

	    public struct GUIBackgroundColorUsing : IDisposable {

		    private Color oldColor;

		    public GUIBackgroundColorUsing(Color color) {

			    this.oldColor = GUI.backgroundColor;
			    GUI.backgroundColor = color;

		    }
		    
		    public void Dispose() {

			    GUI.backgroundColor = this.oldColor;

		    }

	    }

	    public static void DrawGradient(float height, Color from, Color to, string labelFrom, string labelTo) {
	        
		    var tex = new Texture2D(2, 1, TextureFormat.RGBA32, false);
		    tex.filterMode = FilterMode.Bilinear;
		    tex.wrapMode = TextureWrapMode.Clamp;
		    tex.SetPixel(0, 0, from);
		    tex.SetPixel(1, 0, to);
		    tex.Apply();
		    
		    Rect rect = EditorGUILayout.GetControlRect(false, height);
		    rect.height = height;
		    EditorGUI.DrawTextureTransparent(rect, tex, ScaleMode.StretchToFill);
		    
		    GUILayout.BeginHorizontal();
		    {
			    GUILayout.Label(labelFrom);
			    GUILayout.FlexibleSpace();
			    GUILayout.Label(labelTo);
		    }
		    GUILayout.EndHorizontal();
		    
	    }

	    public static void ProgressBar(float value, float max, bool drawLabel = false) {
		    
		    GUILayoutExt.ProgressBar(value, max, new Color(0f, 0f, 0f, 0.3f), new Color32(104, 148, 192, 255), drawLabel);
		    
	    }

	    public static void ProgressBar(float value, float max, Color back, Color fill, bool drawLabel = false) {

		    var progress = value / max;
		    var lineHeight = (drawLabel == true ? 8f : 4f);
		    Rect rect = EditorGUILayout.GetControlRect(false, lineHeight);
		    rect.height = lineHeight;
		    var fillRect = rect;
		    fillRect.width = progress * rect.width;
		    EditorGUI.DrawRect(rect, back);
		    EditorGUI.DrawRect(fillRect, fill);

		    if (drawLabel == true) {
			    
			    EditorGUI.LabelField(rect, string.Format("{0}/{1}", value, max), EditorStyles.centeredGreyMiniLabel);
			    
		    }

	    }

	    public static Entity DrawEntitySelection(World world, in Entity entity, bool checkAlive, bool drawSelectButton = true) {
		    
		    var currentEntity = world.GetEntityById(entity.id);
		    if (checkAlive == true && entity.IsAlive() == false) {

			    EditorGUILayout.HelpBox("This entity version is already in pool, the list of components has been changed.", MessageType.Warning);
			    if (currentEntity.generation > 0) {
                        
				    GUILayout.Label("New entity: " + currentEntity.ToSmallString());
                        
			    } else {
                        
				    GUILayout.Label("New entity is not active");
                        
			    }

		    }

		    if (drawSelectButton == true && currentEntity.generation > 0) {

			    UnityEngine.GUILayout.BeginHorizontal();
			    UnityEngine.GUILayout.FlexibleSpace();
			    if (UnityEngine.GUILayout.Button("Select Entity", UnityEngine.GUILayout.Width(150f)) == true) {

				    WorldsViewerEditor.SelectEntity(currentEntity);

			    }
			    UnityEngine.GUILayout.FlexibleSpace();
			    UnityEngine.GUILayout.EndHorizontal();

		    }

		    return currentEntity;

	    }
	    
	    public static void DrawAddEntityMenu(ME.ECS.Debug.EntityDebugComponent entityDebugComponent) {
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 12;
            style.fixedWidth = 230;
            style.fixedHeight = 23;
 
            var rect = GUILayoutUtility.GetLastRect();
 
            if (GUILayout.Button("Assign Entity", style)) {
                
                rect.y += 26f;
                rect.x += rect.width;
                rect.width = style.fixedWidth;
                //AddEquipmentBehaviourWindow.Show(rect, entity, usedComponents);

                //var lastRect = GUILayoutUtility.GetLastRect();
                //lastRect.height = 200f;
                var v2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
                rect.x = v2.x;
                rect.y = v2.y;
                rect.height = 320f;
                
                var popup = new Popup() {
	                title = "Entities",
	                autoHeight = false,
	                autoClose = true,
	                screenRect = rect,
	                searchText = string.Empty,
	                separator = '.',
	                
                };
                
                var worldEditor = new WorldsViewerEditor.WorldEditor();
                worldEditor.world = Worlds.currentWorld;
                
                var allEntities = PoolListCopyable<Entity>.Spawn(worldEditor.world.GetState().storage.AliveCount);
                if (worldEditor.world.ForEachEntity(allEntities) == true) {

	                for (int i = 0; i < allEntities.Count; ++i) {

		                var entity = allEntities[i];
		                var name = entity.Has<ME.ECS.Name.Name>() == true ? entity.Read<ME.ECS.Name.Name>().value : "Unnamed";
		                popup.Item(string.Format("{0} ({1})", name, entity), () => {
		                
			                entityDebugComponent.world = worldEditor.world;
			                entityDebugComponent.entity = entity;
		                
		                });

	                }

                }
                PoolListCopyable<Entity>.Recycle(ref allEntities);

                popup.Show();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
 
        }
	    
	    private static System.Type[] allStructComponentsWithoutRuntime;
	    private static System.Type[] allStructComponents;

	    public static void DrawManageDataConfigTemplateMenu(System.Collections.Generic.HashSet<ME.ECS.DataConfigs.DataConfigTemplate> usedComponents, System.Action<ME.ECS.DataConfigs.DataConfigTemplate, bool> onAdd) {
		    
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 12;
            style.fixedWidth = 230;
            style.fixedHeight = 23;
 
            var rect = GUILayoutUtility.GetLastRect();
 
            if (GUILayout.Button("Manage Templates", style)) {
                
                rect.y += 26f;
                rect.x += rect.width;
                rect.width = style.fixedWidth;
                
                var v2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
                rect.x = v2.x;
                rect.y = v2.y;
                rect.height = 320f;
                
                var popup = new Popup() {
	                title = "Components",
	                autoHeight = false,
	                screenRect = rect,
	                searchText = string.Empty,
	                separator = '.',
	                
                };
                var arr = AssetDatabase.FindAssets("t:DataConfigTemplate");
                foreach (var guid in arr) {

	                var path = AssetDatabase.GUIDToAssetPath(guid);
	                var template = AssetDatabase.LoadAssetAtPath<ME.ECS.DataConfigs.DataConfigTemplate>(path);
	                var isUsed = usedComponents.Contains(template);
	                var caption = template.name;

	                System.Action<PopupWindowAnim.PopupItem> onItemSelect = (item) => {
		                
		                isUsed = usedComponents.Contains(template);
		                onAdd.Invoke(template, isUsed);
		                
		                isUsed = usedComponents.Contains(template);
		                var tex = isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0];
		                item.image = tex;
		                
	                };
	                
	                if (isUsed == true) popup.Item("Used." + caption, isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0], onItemSelect, searchable: false);
	                popup.Item(caption, isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0], onItemSelect);

                }
                popup.Show();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
 
	    }

	    public static void DrawAddComponentMenu(Rect rect, System.Collections.Generic.HashSet<System.Type> usedComponents, System.Action<System.Type, bool> onAdd, bool showRuntime) {
		    
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 12;
            style.fixedWidth = 130f;
            style.fixedHeight = 23f;

            var offset = (rect.width - style.fixedWidth) * 0.5f;
            rect.width = style.fixedWidth;
            rect.height = style.fixedHeight;
            rect.x += offset;
            if (GUI.Button(rect, "Edit Components", style)) {
                
                rect.y += 26f;
                rect.x += rect.width;
                rect.width = style.fixedWidth;
                //AddEquipmentBehaviourWindow.Show(rect, entity, usedComponents);

                if (GUILayoutExt.allStructComponents == null) {

	                GUILayoutExt.allStructComponents = AppDomain.CurrentDomain.GetAssemblies()
	                                                            .SelectMany(x => x.GetTypes())
	                                                            .Where(x => 
		                                                                   x.IsValueType == true &&
		                                                                   typeof(IStructComponentBase).IsAssignableFrom(x) == true
	                                                            )
	                                                            .ToArray();

                }

                if (GUILayoutExt.allStructComponentsWithoutRuntime == null) {

	                GUILayoutExt.allStructComponentsWithoutRuntime = AppDomain.CurrentDomain.GetAssemblies()
	                                                                          .SelectMany(x => x.GetTypes())
	                                                                          .Where(x => 
		                                                                                 x.IsValueType == true &&
		                                                                                 typeof(IStructComponentBase).IsAssignableFrom(x) == true &&
		                                                                                 typeof(IComponentRuntime).IsAssignableFrom(x) == false
	                                                                          )
	                                                                          .ToArray();

                }

                //var lastRect = GUILayoutUtility.GetLastRect();
                //lastRect.height = 200f;
                var v2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
                rect.x = v2.x - rect.width;
                rect.y = v2.y;
                rect.width = 230f;
                rect.height = 320f;
                
                var popup = new Popup() {
	                title = "Components",
	                autoHeight = false,
	                screenRect = rect,
	                searchText = string.Empty,
	                separator = '.',
	                
                };
                var arr = showRuntime == true ? GUILayoutExt.allStructComponents : GUILayoutExt.allStructComponentsWithoutRuntime;
                foreach (var type in arr) {

	                var isUsed = usedComponents.Contains(type);

	                var addType = type;
	                var name = type.FullName;
	                var fixName = string.Empty;

	                if (name.StartsWith("ME.ECS") == true) {
		                
		                var spName = name.Split('.');
		                var p1 = spName[spName.Length - 2];
		                var p2 = spName[spName.Length - 1];
		                if (p1 == p2) {
			                
			                fixName = "ECS." + p2;

		                } else {

			                fixName = "ECS." + p1 + "." + p2;

		                }

	                } else {

		                //var spName = name.Split('.');
		                //var component = spName[spName.Length - 1];
		                var spName = name.Split(new[] { ".Features." }, StringSplitOptions.RemoveEmptyEntries);
		                //var rootName = spName[0];
		                name = spName[spName.Length - 1];
		                /*spName = name.Split(new[] { ".Components." }, StringSplitOptions.RemoveEmptyEntries);
		                var feature = spName[0];
						fixName = rootName + "." + feature + "." + component;*/
		                fixName = name;

	                }
	                
	                System.Action<PopupWindowAnim.PopupItem> onItemSelect = (item) => {
		                
		                isUsed = usedComponents.Contains(type);
		                onAdd.Invoke(addType, isUsed);
		                
		                isUsed = usedComponents.Contains(type);
		                var tex = isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0];
		                item.image = tex;
		                
	                };
	                
	                if (isUsed == true) popup.Item("Used." + type.Name, isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0], onItemSelect, searchable: false);
	                popup.Item(fixName, isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0], onItemSelect);

                }
                popup.Show();

            }
 
	    }

	    public static void DrawAddComponentMenu(System.Collections.Generic.HashSet<System.Type> usedComponents, System.Action<System.Type, bool> onAdd, bool showRuntime) {
		    
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 12;
            style.fixedWidth = 230;
            style.fixedHeight = 23;
 
            var rect = GUILayoutUtility.GetLastRect();
 
            if (GUILayout.Button("Edit Components", style)) {
                
                rect.y += 26f;
                rect.x += rect.width;
                rect.width = style.fixedWidth;
                //AddEquipmentBehaviourWindow.Show(rect, entity, usedComponents);

                if (GUILayoutExt.allStructComponents == null) {

	                GUILayoutExt.allStructComponents = AppDomain.CurrentDomain.GetAssemblies()
	                                                            .SelectMany(x => x.GetTypes())
	                                                            .Where(x => 
		                                                                   x.IsValueType == true &&
		                                                                   typeof(IStructComponentBase).IsAssignableFrom(x) == true
	                                                            )
	                                                            .ToArray();

                }

                if (GUILayoutExt.allStructComponentsWithoutRuntime == null) {

	                GUILayoutExt.allStructComponentsWithoutRuntime = AppDomain.CurrentDomain.GetAssemblies()
	                                                                          .SelectMany(x => x.GetTypes())
	                                                                          .Where(x => 
		                                                                                 x.IsValueType == true &&
		                                                                                 typeof(IStructComponentBase).IsAssignableFrom(x) == true &&
		                                                                                 typeof(IComponentRuntime).IsAssignableFrom(x) == false
	                                                                          )
	                                                                          .ToArray();

                }

                //var lastRect = GUILayoutUtility.GetLastRect();
                //lastRect.height = 200f;
                var v2 = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
                rect.x = v2.x;
                rect.y = v2.y;
                rect.height = 320f;
                
                var popup = new Popup() {
	                title = "Components",
	                autoHeight = false,
	                screenRect = rect,
	                searchText = string.Empty,
	                separator = '.',
	                
                };
                var arr = showRuntime == true ? GUILayoutExt.allStructComponents : GUILayoutExt.allStructComponentsWithoutRuntime;
                foreach (var type in arr) {

	                var isUsed = usedComponents.Contains(type);

	                var addType = type;
	                var name = type.FullName;
	                var fixName = string.Empty;

	                if (name.StartsWith("ME.ECS") == true) {
		                
		                var spName = name.Split('.');
		                var p1 = spName[spName.Length - 2];
		                var p2 = spName[spName.Length - 1];
		                if (p1 == p2) {
			                
			                fixName = "ECS." + p2;

		                } else {

			                fixName = "ECS." + p1 + "." + p2;

		                }

	                } else {

		                //var spName = name.Split('.');
		                //var component = spName[spName.Length - 1];
		                var spName = name.Split(new[] { ".Features." }, StringSplitOptions.RemoveEmptyEntries);
		                //var rootName = spName[0];
		                name = spName[spName.Length - 1];
		                /*spName = name.Split(new[] { ".Components." }, StringSplitOptions.RemoveEmptyEntries);
		                var feature = spName[0];
						fixName = rootName + "." + feature + "." + component;*/
		                fixName = name;

	                }
	                
	                System.Action<PopupWindowAnim.PopupItem> onItemSelect = (item) => {
		                
		                isUsed = usedComponents.Contains(type);
		                onAdd.Invoke(addType, isUsed);
		                
		                isUsed = usedComponents.Contains(type);
		                var tex = isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0];
		                item.image = tex;
		                
	                };
	                
	                if (isUsed == true) popup.Item("Used." + type.Name, isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0], onItemSelect, searchable: false);
	                popup.Item(fixName, isUsed == true ? EditorStyles.toggle.onNormal.scaledBackgrounds[0] : EditorStyles.toggle.normal.scaledBackgrounds[0], onItemSelect);

                }
                popup.Show();

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
 
	    }

	    public static void DrawAddComponentMenu(Entity entity, System.Collections.Generic.HashSet<System.Type> usedComponents, IStructComponentsContainer componentsStructStorage) {
            
		    GUILayoutExt.DrawAddComponentMenu(usedComponents, (addType, isUsed) => {
			    
			    var registries = componentsStructStorage.GetAllRegistries();
			    for (int i = 0; i < registries.Length; ++i) {
		                
				    var registry = registries.arr[i];
				    if (registry == null) continue;
				    if (registry.HasType(addType) == true) {

					    if (isUsed == true) {

						    usedComponents.Remove(addType);
						    registry.RemoveObject(entity);
						    Worlds.currentWorld.RemoveComponentFromFilter(entity);

					    } else {
				                
						    usedComponents.Add(addType);
						    registry.SetObject(entity, (IStructComponentBase)System.Activator.CreateInstance(addType));
						    Worlds.currentWorld.AddComponentToFilter(entity);

					    }

					    break;

				    }
		                
			    }

		    }, showRuntime: true);
		    
        }

	    public static void CollectEditors<TEditor, TAttribute>(ref System.Collections.Generic.Dictionary<System.Type, TEditor> dic,
	                                                           System.Reflection.Assembly searchAssembly = null)
		    where TEditor : IGUIEditorBase where TAttribute : CustomEditorAttribute {

		    if (dic != null) return;
		    
		    var assembly = (searchAssembly == null ? System.Reflection.Assembly.GetExecutingAssembly() : searchAssembly);
		    CollectEditors<TEditor, TAttribute>(ref dic, new [] { assembly });
		    
	    }

	    public static void CollectEditorsAll<TEditor, TAttribute>(ref System.Collections.Generic.Dictionary<System.Type, TEditor> dic) where TEditor : IGUIEditorBase where TAttribute : CustomEditorAttribute {
		    
		    if (dic == null) CollectEditors<TEditor, TAttribute>(ref dic, System.AppDomain.CurrentDomain.GetAssemblies());
		    
	    }

	    public static void CollectEditors<TEditor, TAttribute>(ref System.Collections.Generic.Dictionary<System.Type, TEditor> dic, System.Reflection.Assembly[] searchAssemblies) where TEditor : IGUIEditorBase where TAttribute : CustomEditorAttribute {

            if (dic == null) {

                dic = new System.Collections.Generic.Dictionary<System.Type, TEditor>();

                foreach (var asm in searchAssemblies) {

	                var types = asm.GetTypes();
	                foreach (var type in types) {

		                var attrs = type.GetCustomAttributes(typeof(TAttribute), inherit: true);
		                if (attrs.Length > 0) {

			                if (attrs[0] is TAttribute attr) {

				                if (typeof(TEditor).IsAssignableFrom(type) == true) {

					                var editor = (TEditor)System.Activator.CreateInstance(type);
					                if (dic.ContainsKey(attr.type) == false) {

						                dic.Add(attr.type, editor);

					                }

				                }

			                }

		                }

	                }

                }

            }

        }
        
	    public static bool ToggleLeft(ref bool state, ref bool isDirty, string caption, string text, System.Action onDrawBeforeDescription = null) {

		    var labelRich = new GUIStyle(EditorStyles.label);
		    labelRich.richText = true;

		    var isLocalDirty = false;
		    var flag = EditorGUILayout.ToggleLeft(caption, state, labelRich);
		    if (flag != state) {

			    isLocalDirty = true;
			    isDirty = true;
			    state = flag;
                        
		    }
		    onDrawBeforeDescription?.Invoke();
		    if (string.IsNullOrEmpty(text) == false) GUILayoutExt.SmallLabel(text);
		    EditorGUILayout.Space();

		    return isLocalDirty;

	    }

	    public static bool IntFieldLeft(ref int state, ref bool isDirty, string caption, string text) {

		    var labelRich = new GUIStyle(EditorStyles.label);
		    labelRich.richText = true;

		    var isLocalDirty = false;
		    GUILayout.BeginHorizontal();
		    var flag = EditorGUILayout.IntField(state, labelRich);
		    EditorGUILayout.LabelField(caption);
		    GUILayout.EndHorizontal();
		    if (flag != state) {

			    isLocalDirty = true;
			    isDirty = true;
			    state = flag;
                        
		    }
		    if (string.IsNullOrEmpty(text) == false) GUILayoutExt.SmallLabel(text);
		    EditorGUILayout.Space();

		    return isLocalDirty;

	    }

        public static LayerMask DrawLayerMaskField(string label, LayerMask layerMask) {

	        System.Collections.Generic.List<string> layers = new System.Collections.Generic.List<string>();
	        System.Collections.Generic.List<int> layerNumbers = new System.Collections.Generic.List<int>();

	        for (int i = 0; i < 32; i++) {
		        string layerName = LayerMask.LayerToName(i);
		        if (layerName != "") {
			        layers.Add(layerName);
			        layerNumbers.Add(i);
		        }
	        }
	        int maskWithoutEmpty = 0;
	        for (int i = 0; i < layerNumbers.Count; i++) {
		        if (((1 << layerNumbers[i]) & layerMask.value) > 0)
			        maskWithoutEmpty |= (1 << i);
	        }
	        maskWithoutEmpty = EditorGUILayout.MaskField( label, maskWithoutEmpty, layers.ToArray());
	        int mask = 0;
	        for (int i = 0; i < layerNumbers.Count; i++) {
		        if ((maskWithoutEmpty & (1 << i)) > 0)
			        mask |= (1 << layerNumbers[i]);
	        }
	        layerMask.value = mask;
	        return layerMask;

        }
        
        public static void DrawHeader(string caption) {

            var backStyle = new GUIStyle(EditorStyles.label);
            backStyle.normal.background = Texture2D.whiteTexture;
            
            var backColor = GUI.backgroundColor;
            GUILayout.Space(4f);
            GUI.backgroundColor = new Color(0f, 0f, 0f, 0.07f);
            GUILayoutExt.Separator(new Color(0f, 0f, 0f, 0.4f), 1f);
            GUILayoutExt.Padding(
                8f, 4f,
                () => {
                    
                    GUILayout.Label(caption, EditorStyles.boldLabel);
                    
                }, backStyle);
            GUILayoutExt.Separator(new Color(0f, 0f, 0f, 0.4f), 1f);
            GUI.backgroundColor = backColor;

        }

        public static void SmallLabel(string text) {

            var labelRich = new GUIStyle(EditorStyles.miniLabel);
            labelRich.richText = true;
            labelRich.wordWrap = true;

            var oldColor = GUI.color;
            var c = oldColor;
            c.a = 0.5f;
            GUI.color = c;
            
            EditorGUILayout.LabelField(text, labelRich);

            GUI.color = oldColor;

        }

        public static int Pages(int count, int page, int elementsOnPage, System.Action<int, int> onDraw, System.Action<int> onPageElementsChanged, System.Action onDrawHeader = null) {

            var from = page * elementsOnPage;
            var to = from + elementsOnPage;
            if (from < 0) from = 0;
            if (to > count) to = count;
            var pages = Mathf.CeilToInt(count / (float)elementsOnPage) - 1;
            
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (onDrawHeader != null) onDrawHeader.Invoke();
                
                GUILayout.FlexibleSpace();
                
                GUILayout.BeginHorizontal();
                {

                    GUILayout.Label("On page:", EditorStyles.toolbarButton);
                    if (GUILayout.Button(elementsOnPage.ToString(), EditorStyles.toolbarDropDown, GUILayout.MinWidth(30f)) == true) {

                        var items = new[] { 10, 20, 30, 40, 50, 100 };
                        var menu = new GenericMenu();
                        for (int i = 0; i < items.Length; ++i) {

                            var idx = i;
                            menu.AddItem(new GUIContent(items[i].ToString()), items[i] == elementsOnPage, () => { onPageElementsChanged.Invoke(items[idx]); });

                        }

                        //menu.DropDown(GUILayoutUtility.GetLastRect());
                        menu.ShowAsContext();

                    }

                    EditorGUI.BeginDisabledGroup(page <= 0);
                    if (GUILayout.Button("◄", EditorStyles.toolbarButton) == true) {

                        --page;

                    }

                    EditorGUI.EndDisabledGroup();

                    var pageStr = GUILayout.TextField((page + 1).ToString(), EditorStyles.toolbarTextField, GUILayout.MinWidth(20f));
                    if (int.TryParse(pageStr, out var res) == true) {

                        page = res - 1;

                    }
                    GUILayout.Label("/", EditorStyles.toolbarButton);
                    GUILayout.Label(string.Format("{0}", pages + 1), EditorStyles.toolbarButton, GUILayout.MinWidth(20f));

                    EditorGUI.BeginDisabledGroup(page >= pages);
                    if (GUILayout.Button("►", EditorStyles.toolbarButton) == true) {

                        ++page;

                    }

                    EditorGUI.EndDisabledGroup();

                }
                GUILayout.EndHorizontal();
                
                if (page < 0) page = 0;
                if (page > pages) page = pages;

            }
            GUILayout.EndHorizontal();
            
            onDraw.Invoke(from, to);

            return page;

        }

        public static bool IsFirstFieldHasChilds(object instance) {
	        
	        var fields = instance.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
	        if (fields.Length == 1) {

		        var field = fields[0];
		        if (GUILayoutExt.IsDefaultEditorType(field.FieldType) == true) return false;
		        if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType) == true) return false;
		        ME.ECSEditor.GUILayoutExt.CollectEditorsAll<ICustomFieldEditor, CustomFieldEditorAttribute>(ref GUILayoutExt.customFieldEditors);
		        if (GUILayoutExt.customFieldEditors.TryGetValue(field.FieldType, out _) == true) return false;
		        if (field.FieldType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Length > 1) return true;
		        
		        return true;

	        }

	        return false;

        }
        
        public static int GetFieldsCount(object instance) {
            
            var fields = instance.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            return fields.Length;

        }

        public class TempObject : MonoBehaviour {

	        [SerializeReference]
	        public object data;

        }

        public struct FieldsSingleCache {

	        public GameObject temp;
	        public TempObject[] comps;
	        public SerializedObject[] objs;

        }
        private static readonly System.Collections.Generic.Dictionary<int, FieldsSingleCache> fieldsSingleCache = new System.Collections.Generic.Dictionary<int, FieldsSingleCache>();

        private static int GetFieldSingleCacheKey(object cacheKey, IStructComponentBase[] instances) {
	        
	        var key = cacheKey.GetHashCode();
	        key ^= instances.Length;
	        return key;

        }

        public static void DrawObjectAsPropertyField<T>(T obj, GUIContent label) {

	        var temp = new GameObject("Temp");
	        temp.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
	        
	        var comp = temp.AddComponent<TempObject>();
	        comp.data = obj;

	        var so = new SerializedObject(comp);
	        var it = so.FindProperty("data");
	        so.Update();
	        EditorGUILayout.PropertyField(it, label, true);
	        so.ApplyModifiedProperties();
	        
	        GameObject.DestroyImmediate(temp);

        }

        public static float GetObjectAsPropertyFieldHeight<T>(T obj, GUIContent label) {
	        
	        var temp = new GameObject("Temp");
	        temp.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
	        
	        var comp = temp.AddComponent<TempObject>();
	        comp.data = obj;

	        var so = new SerializedObject(comp);
	        var it = so.FindProperty("data");
	        var h = EditorGUI.GetPropertyHeight(it, label, true);
	        
	        GameObject.DestroyImmediate(temp);
	        return h;

        }

        public static void DrawObjectAsPropertyField<T>(Rect rect, T obj, GUIContent label) {

	        var temp = new GameObject("Temp");
	        temp.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
	        
	        var comp = temp.AddComponent<TempObject>();
	        comp.data = obj;

	        var so = new SerializedObject(comp);
	        var it = so.FindProperty("data");
	        so.Update();
	        EditorGUI.PropertyField(rect, it, label, true);
	        so.ApplyModifiedProperties();
	        
	        GameObject.DestroyImmediate(temp);

        }

        public static void DrawObjectAsPropertyFieldAll<T>(Rect rect, T obj, GUIContent label) {

	        var temp = new GameObject("Temp");
	        temp.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
	        
	        var comp = temp.AddComponent<TempObject>();
	        comp.data = obj;

	        var so = new SerializedObject(comp);
	        var it = so.FindProperty("data");
	        so.Update();
	        while (it.Next(true) == true) {

		        var h = EditorGUI.GetPropertyHeight(it, label);
		        rect.height = h;
		        EditorGUI.PropertyField(rect, it, label);
		        rect.y += h;

	        }
	        so.ApplyModifiedProperties();
	        
	        GameObject.DestroyImmediate(temp);

        }

        public static string SearchField(string label, string value) {

	        if (value == null) value = string.Empty;
	        return EditorGUILayout.TextField(label, value, EditorStyles.toolbarSearchField);

        }

        public static bool IsSearchValid(IStructComponentBase component, string search) {

	        if (string.IsNullOrEmpty(search) == false) {

		        var splitted = search.Split(' ');
		        for (int i = 0; i < splitted.Length; ++i) {

			        var type = component.GetType();
			        if (type.FullName.ToLower().Contains(splitted[i].ToLower()) == true) return true;

		        }

		        return false;

	        }

	        return true;
	        
        }

        public static void DropCachedFields() {
	        
	        foreach (var kv in GUILayoutExt.fieldsSingleCache) {
				        
		        GameObject.DestroyImmediate(kv.Value.temp);
				        
	        }
			        
	        GUILayoutExt.fieldsSingleCache.Clear();
	        
        }

        public static bool DrawFieldsSingle(string search, object cacheKey, WorldsViewerEditor.WorldEditor world, IStructComponentBase[] instances, System.Action<int, IStructComponentBase, SerializedProperty> onPropertyBegin, System.Action<int, IStructComponentBase, SerializedProperty> onPropertyEnd, System.Action<int, IStructComponentBase> onPropertyChanged = null) {

	        SerializedObject[] objs = null;
	        var key = GUILayoutExt.GetFieldSingleCacheKey(cacheKey, instances);
	        {
		        if (GUILayoutExt.fieldsSingleCache.Count > 100) {

			        GUILayoutExt.DropCachedFields();

		        }
		        if (GUILayoutExt.fieldsSingleCache.TryGetValue(key, out var cache) == false ||
		            instances.Length != cache.objs.Length) {

			        var temp = new GameObject("Temp");
			        temp.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
			        cache.temp = temp;
			        cache.objs = new SerializedObject[instances.Length];
			        cache.comps = new TempObject[instances.Length];
			        for (int i = 0; i < instances.Length; ++i) {

				        var comp = temp.AddComponent<TempObject>();
				        comp.data = instances[i];

				        cache.comps[i] = comp;
				        cache.objs[i] = new SerializedObject(comp);

			        }

			        if (GUILayoutExt.fieldsSingleCache.ContainsKey(key) == true) {
				        
				        GUILayoutExt.fieldsSingleCache[key] = cache;
				        
			        } else {

				        GUILayoutExt.fieldsSingleCache.Add(key, cache);

			        }

			        objs = cache.objs;

		        } else {

			        for (int i = 0; i < cache.objs.Length; ++i) {

				        cache.objs[i] = new SerializedObject(cache.comps[i]);

			        }
			        objs = cache.objs;

		        }
	        }

	        var changed = false;
	        {

		        const float minHeight = 24f;
		        var backStyle = new GUIStyle(EditorStyles.label);
		        backStyle.normal.background = Texture2D.whiteTexture;

		        var k = 0;
		        for (var index = 0; index < objs.Length; index++) {

			        var component = instances[index];
			        if (GUILayoutExt.IsSearchValid(component, search) == false) continue;

			        EditorGUI.BeginChangeCheck();

			        ++k;
			        {
				        using (new GUIBackgroundColorUsing(new Color(1f, 1f, 1f, k % 2 == 0 ? 0f : 0.05f))) {

					        GUILayout.BeginVertical(backStyle, GUILayout.MinHeight(minHeight));

				        }

				        if (component == null) {

					        onPropertyBegin.Invoke(index, null, null);

					        EditorGUI.BeginDisabledGroup(true);
					        var styleLabel = new GUIStyle(EditorStyles.label);
					        styleLabel.richText = true;
					        EditorGUILayout.LabelField(new GUIContent("<color=#f77><i>MISSING</i></color>"), styleLabel);
					        EditorGUI.EndDisabledGroup();

					        onPropertyEnd.Invoke(index, null, null);

				        } else {

					        var label = GUILayoutExt.GetStringCamelCaseSpace(instances[index].GetType().Name);

					        var obj = objs[index];
					        var it = obj.FindProperty("data");

					        onPropertyBegin.Invoke(index, component, it);

					        obj.Update();

					        var fieldsCount = GUILayoutExt.GetFieldsCount(component);
					        if (fieldsCount == 0) {

						        EditorGUI.BeginDisabledGroup(true);
						        EditorGUILayout.Toggle(label, true);
						        EditorGUI.EndDisabledGroup();

					        } else if (EditorUtilities.GetPropertyChildCount(it) == 1 ||
					                   EditorUtilities.GetPropertyHeight(it, true, new GUIContent(label)) <= minHeight) {

						        if (EditorUtilities.GetPropertyChildCount(it) > 1 || it.NextVisible(true) == true) {

							        EditorGUILayout.PropertyField(it, new GUIContent(label), true);

						        } else {

							        EditorGUILayout.LabelField(label);

						        }

					        } else {

						        var foldKey = "ME.ECS.WorldsViewerEditor.FoldoutTypes." + component.GetType().FullName;
						        var state = world.IsFoldOutCustom(foldKey);
						        GUILayoutExt.FoldOut(ref state, label, () => {

							        ++EditorGUI.indentLevel;

							        var enterChildren = true;
							        while (it.NextVisible(enterChildren) == true) {

								        EditorGUILayout.PropertyField(it, true);
								        enterChildren = false;

							        }

							        --EditorGUI.indentLevel;

						        });
						        world.SetFoldOutCustom(foldKey, state);

					        }

					        obj.ApplyModifiedProperties();

					        onPropertyEnd.Invoke(index, component, it);

				        }

				        GUILayout.EndVertical();

			        }

			        if (EditorGUI.EndChangeCheck() == true) {

				        changed = true;
				        onPropertyChanged?.Invoke(index, component);

			        }

		        }

		        /*if (changed == true)*/ {

			        for (var index = 0; index < objs.Length; index++) {

				        instances[index] = (IStructComponentBase)((TempObject)objs[index].targetObject).data;

			        }

		        }

	        }

	        return changed;
	        
        }

        public static bool DrawFields(WorldsViewerEditor.WorldEditor world, object[] instances, string customName = null) {

	        var temp = new GameObject("Temp");
	        foreach (var instance in instances) {
		        
		        var comp = temp.AddComponent<TempObject>();
		        comp.data = instance;

	        }
	        
	        var comps = temp.GetComponents<TempObject>();
	        var obj = new SerializedObject(comps);

	        obj.Update();
	        var changed = false;
	        var it = obj.FindProperty("data");
	        it.NextVisible(true);
	        EditorGUI.BeginChangeCheck();
	        EditorGUILayout.PropertyField(it, new GUIContent(customName), includeChildren: true);
	        if (EditorGUI.EndChangeCheck() == true) {

		        changed = true;

	        }
	        
	        /*while (it.NextVisible(true) == true) {

		        var depth = EditorGUI.indentLevel;
		        EditorGUI.indentLevel = it.depth - 1;
		        EditorGUI.BeginChangeCheck();
		        EditorGUILayout.PropertyField(it);
		        if (EditorGUI.EndChangeCheck() == true) {

			        changed = true;

		        }
		        EditorGUI.indentLevel = depth;

		        //GUILayoutExt.DrawComponentHelp(System.Type.GetType(it.managedReferenceFullTypename));

	        }*/
	        obj.ApplyModifiedProperties();
	        
	        if (changed == true) {

		        for (int i = 0; i < comps.Length; ++i) {

			        instances[i] = comps[i].data;

		        }
		        
	        }
	        
	        GameObject.DestroyImmediate(temp);
	        
	        return changed;
	        
	        /*
	        var objType = instances[0].GetType();
	        var changed = false;
	        var fields = objType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
	        if (fields.Length > 0) {

		        foreach (var field in fields) {

			        var isDiff = false;
			        var prevVal = string.Empty;
			        foreach (var instance in instances) {
				    
				        var val = field.GetValue(instance);
				        if (val.ToString() != prevVal) {

					        prevVal = val.ToString();
					        isDiff = true;

				        }

			        }

			        var value = field.GetValue(instances[0]);
			        var oldValue = value;
			        var isEditable = GUILayoutExt.PropertyField(world, field.Name, field, field.FieldType, ref value, typeCheckOnly: true, hasMultipleDifferentValues: isDiff);
			        EditorGUI.BeginDisabledGroup(disabled: (isEditable == false));
			        if (GUILayoutExt.PropertyField(world, customName != null ? customName : field.Name, field, field.FieldType, ref value, typeCheckOnly: false, hasMultipleDifferentValues: isDiff) == true) {

				        if (oldValue != value) {

					        foreach (var instance in instances) field.SetValue(instance, value);
					        changed = true;

				        }

			        }

			        EditorGUI.EndDisabledGroup();
			        
		        }

	        }

	        GUILayoutExt.DrawComponentHelp(objType);
            
	        return changed;
	        */

        }

        public static bool DrawFields(WorldsViewerEditor.WorldEditor world, object instance, string customName = null) {

	        return GUILayoutExt.DrawFields(world, ref instance, customName);

        }

        public static bool DrawFields(WorldsViewerEditor.WorldEditor world, ref object instance, string customName = null) {

            //var padding = 2f;
            //var margin = 1f;
            //var cellHeight = 24f;
            //var tableStyle = new GUIStyle("Box");

            var objType = instance.GetType();
            var changed = false;
            var fields = objType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (fields.Length > 0) {

                /*GUILayout.BeginHorizontal();
                {
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Field", EditorStyles.miniBoldLabel); },
                                     tableStyle, GUILayout.Width(fieldWidth),
                                     GUILayout.Height(cellHeight));
                    GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.TableCaption("Value", EditorStyles.miniBoldLabel); },
                                     tableStyle, GUILayout.ExpandWidth(true),
                                     GUILayout.Height(cellHeight));
                }
                GUILayout.EndHorizontal();*/

                foreach (var field in fields) {

                    //GUILayout.BeginHorizontal();
                    {
                        //GUILayoutExt.Box(padding, margin, () => { GUILayoutExt.DataLabel(field.Name); }, tableStyle,
                        //                 GUILayout.Width(fieldWidth), GUILayout.Height(cellHeight));
                        //GUILayoutExt.Box(padding, margin, () => {

                        //GUILayoutExt.DataLabel(field.Name);
                        //var lastRect = GUILayoutUtility.GetLastRect();
                        var value = field.GetValue(instance);
                        //var oldValue = value;
                        var isEditable = GUILayoutExt.PropertyField(world, field.Name, instance, -1, field, field.FieldType, ref value, typeCheckOnly: true, hasMultipleDifferentValues: false);
                        EditorGUI.BeginDisabledGroup(disabled: (isEditable == false));
                        if (GUILayoutExt.PropertyField(world, customName != null ? customName : field.Name, instance, -1, field, field.FieldType, ref value, typeCheckOnly: false, hasMultipleDifferentValues: false) == true) {

                            //if (oldValue != value) {

                                field.SetValue(instance, value);
                                changed = true;

                            //}

                        }
                        EditorGUI.EndDisabledGroup();

                        //GUILayout.EndHorizontal();

                        //}, tableStyle, GUILayout.ExpandWidth(true), GUILayout.Height(cellHeight));
                    }
                    //GUILayout.EndHorizontal();

                }

            }

            return changed;

        }

        public static void DrawComponentHelp(System.Type type) {
	        
	        var helps = type.GetCustomAttributes(typeof(ComponentHelpAttribute), false);
	        if (helps.Length > 0) {

		        using (new GUIAlphaUsing(0.6f)) {

			        var style = new GUIStyle(EditorStyles.miniLabel);
			        style.wordWrap = true;
			        GUILayout.Label(((ComponentHelpAttribute)helps[0]).comment, style);

		        }

	        }

        }

        public static void Icon(string[] paths, float width = 32f, float height = 32f) {

	        var icon = new GUIStyle();
	        for (int i = 0; i < paths.Length; ++i) {

		        var path = paths[i];
		        if (System.IO.File.Exists(path) == false) continue;
		        
		        icon.normal.background = UnityEditor.Experimental.EditorResources.Load<Texture2D>(path, false);

	        }

	        EditorGUILayout.LabelField(string.Empty, icon, GUILayout.Width(width), GUILayout.Height(height));

        }

        private static bool HasBaseType(this System.Type type, System.Type baseType) {

	        return baseType.IsAssignableFrom(type);

        }

        private static bool HasInterface(this System.Type type, System.Type interfaceType) {

	        return interfaceType.IsAssignableFrom(type);
	        
        }

        public static string GetStringCamelCaseSpace(string caption) {

	        if (string.IsNullOrEmpty(caption) == true) return string.Empty;
	        var str = System.Text.RegularExpressions.Regex.Replace(caption, "[A-Z]", " $0").Trim();
	        return char.ToUpper(str[0]) + str.Substring(1);

        }
        
        public static System.Array ResizeArray(System.Array oldArray, int newSize, System.Type arrType) {
	        
	        int oldSize = (oldArray != null ? oldArray.Length : 0);
	        System.Type elementType = arrType.GetElementType();
	        System.Array newArray = System.Array.CreateInstance(elementType, newSize);
	        int preserveLength = System.Math.Min(oldSize, newSize);
	        if (preserveLength > 0) {
		        System.Array.Copy (oldArray, newArray, preserveLength);
	        }
	        return newArray;
	        
        }

        public static bool IsDefaultEditorType(System.Type type) {

	        if (type.IsPrimitive == true || type.IsEnum == true) return true;
	        if (type == typeof(Entity)) return true;
	        if (type == typeof(Color)) return true;
	        if (type == typeof(Color32)) return true;
	        if (type == typeof(Vector2)) return true;
	        if (type == typeof(Vector3)) return true;
	        if (type == typeof(FPVector2)) return true;
	        if (type == typeof(FPVector3)) return true;
	        if (type == typeof(Vector4)) return true;
	        if (type == typeof(Quaternion)) return true;
	        if (type == typeof(FPQuaternion)) return true;
	        if (type == typeof(pfloat)) return true;
	        return false;

        }
        
        private static System.Collections.Generic.Dictionary<System.Type, ICustomFieldEditor> customFieldEditors = null;
        public static bool PropertyField(WorldsViewerEditor.WorldEditor world, string caption, object instance, int instanceArrIndex, System.Reflection.FieldInfo fieldInfo, System.Type type, ref object value, bool typeCheckOnly, bool hasMultipleDifferentValues) {

            if (typeCheckOnly == false && value == null && type.IsValueType == false && type.IsArray == false && type.HasBaseType(typeof(UnityEngine.Object)) == false && type.HasBaseType(typeof(string)) == false) {

                EditorGUILayout.LabelField("Null");
                return false;

            }

            caption = GUILayoutExt.GetStringCamelCaseSpace(caption);
            
            EditorGUI.showMixedValue = hasMultipleDifferentValues;

            ME.ECSEditor.GUILayoutExt.CollectEditorsAll<ICustomFieldEditor, CustomFieldEditorAttribute>(ref GUILayoutExt.customFieldEditors);
            if (GUILayoutExt.customFieldEditors.TryGetValue(type, out var editor) == true) {

	            return editor.DrawGUI(caption, instance, instanceArrIndex, fieldInfo, ref value, typeCheckOnly, hasMultipleDifferentValues);

            } else if (type.IsEnum == true) {

	            if (typeCheckOnly == false) {

		            var attrs = fieldInfo.GetCustomAttributes(typeof(IsBitmask), true);
		            if (attrs.Length == 0) {
				        
			            value = EditorGUILayout.EnumPopup(caption, (Enum)value);
    
		            } else {
			            
			            value = EditorGUILayout.EnumFlagsField(caption, (Enum)value);

		            }
		            
	            }

            } else if (type.HasInterface(typeof(ME.ECS.Collections.IBufferArray)) == true ||
                       type.IsArray == true ||
                       type.HasInterface(typeof(ME.ECS.Collections.IStackArray)) == true)
            {

	            if (typeCheckOnly == false) {

		            if (type.HasInterface(typeof(ME.ECS.Collections.IStackArray)) == true) {
			            
			            var arr = (ME.ECS.Collections.IStackArray)value;
			            var state = world.IsFoldOutCustom(type);
			            GUILayoutExt.FoldOut(ref state, string.Format("{0} [{1}]", caption, arr.Length), () => {

				            for (int i = 0; i < arr.Length; ++i) {

					            if (i > 0) GUILayoutExt.Separator();
					            var arrValue = arr[i];
					            object v = default;
					            var isEditable = GUILayoutExt.PropertyField(world, null, instance, i, fieldInfo, arrValue.GetType(), ref v, typeCheckOnly: true, hasMultipleDifferentValues: hasMultipleDifferentValues);
					            EditorGUI.BeginDisabledGroup(disabled: (isEditable == false));
					            GUILayoutExt.PropertyField(world, "Element [" + i.ToString() + "]", instance, i, fieldInfo, arrValue.GetType(), ref arrValue, typeCheckOnly: false, hasMultipleDifferentValues: hasMultipleDifferentValues);
					            EditorGUI.EndDisabledGroup();
					            arr[i] = arrValue;

				            }
			            
			            });
			            world.SetFoldOutCustom(type, state);
			            value = arr;

		            } else if (type.HasInterface(typeof(ME.ECS.Collections.IBufferArray)) == true) {
			            
			            var arr = (ME.ECS.Collections.IBufferArray)value;
			            var state = world.IsFoldOutCustom(type);
			            GUILayoutExt.FoldOut(ref state, string.Format("{0} [{1}]", caption, arr.Count), () => {

				            var array = arr.GetArray();
				            for (int i = 0; i < arr.Count; ++i) {

					            if (i > 0) GUILayoutExt.Separator();
					            var arrValue = array.GetValue(i);
					            object v = default;
					            var isEditable = GUILayoutExt.PropertyField(world, null, instance, i, fieldInfo, arrValue.GetType(), ref v, typeCheckOnly: true, hasMultipleDifferentValues: hasMultipleDifferentValues);
					            EditorGUI.BeginDisabledGroup(disabled: (isEditable == false));
					            GUILayoutExt.PropertyField(world, "Element [" + i.ToString() + "]", instance, i, fieldInfo, arrValue.GetType(), ref arrValue, typeCheckOnly: false, hasMultipleDifferentValues: hasMultipleDifferentValues);
					            EditorGUI.EndDisabledGroup();
					            array.SetValue(arrValue, i);

				            }
			            
			            });
			            world.SetFoldOutCustom(type, state);
			            value = arr;

		            } else if (type.IsArray == true) {

			            var arr = (System.Array)value;
			            if (arr == null) arr = GUILayoutExt.ResizeArray(arr, 0, type);
			            var state = world.IsFoldOutCustom(type);
			            GUILayoutExt.FoldOut(ref state, string.Format("{0} [{1}]", caption, arr.Length), () => {

				            var size = EditorGUILayout.IntField("Size", arr.Length);
				            if (size != arr.Length) {

					            arr = GUILayoutExt.ResizeArray(arr, size, type);

				            }
				            
				            var array = arr;
				            for (int i = 0; i < arr.Length; ++i) {

					            if (i > 0) GUILayoutExt.Separator();
					            var arrValue = array.GetValue(i);
					            if (arrValue == null) arrValue = System.Activator.CreateInstance(array.GetType().GetElementType());
					            object v = default;
					            var isEditable = GUILayoutExt.PropertyField(world, null, array, i, fieldInfo, arrValue.GetType(), ref v, typeCheckOnly: true, hasMultipleDifferentValues: hasMultipleDifferentValues);
					            EditorGUI.BeginDisabledGroup(disabled: (isEditable == false));
					            GUILayoutExt.PropertyField(world, "Element [" + i.ToString() + "]", array, i, fieldInfo, arrValue.GetType(), ref arrValue, typeCheckOnly: false, hasMultipleDifferentValues: hasMultipleDifferentValues);
					            EditorGUI.EndDisabledGroup();
					            array.SetValue(arrValue, i);

				            }

			            });
			            world.SetFoldOutCustom(type, state);
			            value = arr;

		            }

	            }
	            
            } else if (type == typeof(Entity)) {

	            if (typeCheckOnly == false) {

		            var entity = (Entity)value;
		            GUILayout.BeginHorizontal();
		            var buttonWidth = 50f;
		            EditorGUILayout.LabelField(caption, GUILayout.Width(EditorGUIUtility.labelWidth));
		            if (entity == Entity.Empty) {
			            
			            GUILayout.Label("Empty");
			            
		            } else {
			            
			            var customName = (entity.IsAlive() == true ? entity.Read<ME.ECS.Name.Name>().value : string.Empty);
			            GUILayout.BeginVertical();
			            GUILayout.Label(string.IsNullOrEmpty(customName) == false ? customName : "Unnamed");
			            GUILayout.Label(entity.ToSmallString(), EditorStyles.miniLabel);
			            GUILayout.EndVertical();
			            
		            }

		            GUILayout.FlexibleSpace();
		            EditorGUI.BeginDisabledGroup(entity == Entity.Empty);
		            if (GUILayout.Button("Select", GUILayout.Width(buttonWidth)) == true) {

			            WorldsViewerEditor.SelectEntity(entity);

		            }
		            EditorGUI.EndDisabledGroup();
		            GUILayout.EndHorizontal();

	            }

            } else if (type == typeof(Color)) {

	            if (typeCheckOnly == false) {
	                
		            value = EditorGUILayout.ColorField(caption, (Color)value);
		            GUILayout.BeginHorizontal();
		            {
			            var c = (Color32)(Color)value;
			            GUILayout.Label("Raw", EditorStyles.miniLabel, GUILayout.Width(EditorGUIUtility.labelWidth));
			            c.r = (byte)EditorGUILayout.IntField(c.r, EditorStyles.miniTextField);
			            c.g = (byte)EditorGUILayout.IntField(c.g, EditorStyles.miniTextField);
			            c.b = (byte)EditorGUILayout.IntField(c.b, EditorStyles.miniTextField);
			            c.a = (byte)EditorGUILayout.IntField(c.a, EditorStyles.miniTextField);
			            value = (Color)(Color32)c;
		            }
		            GUILayout.EndHorizontal();

	            }
                
            } else if (type == typeof(Color32)) {

	            if (typeCheckOnly == false) {

		            value = EditorGUILayout.ColorField(caption, (Color32)value);
		            GUILayout.BeginHorizontal();
		            {
			            var c = (Color32)value;
			            GUILayout.Label("Raw", EditorStyles.miniLabel, GUILayout.Width(EditorGUIUtility.labelWidth));
			            c.r = (byte)EditorGUILayout.IntField(c.r, EditorStyles.miniTextField);
			            c.g = (byte)EditorGUILayout.IntField(c.g, EditorStyles.miniTextField);
			            c.b = (byte)EditorGUILayout.IntField(c.b, EditorStyles.miniTextField);
			            c.a = (byte)EditorGUILayout.IntField(c.a, EditorStyles.miniTextField);
			            value = c;
		            }
		            GUILayout.EndHorizontal();

	            }
                
            } else if (type == typeof(Vector2)) {

	            if (typeCheckOnly == false) {

		            value = EditorGUILayout.Vector2Field(caption, (Vector2)value);

	            }

            } else if (type == typeof(Vector3)) {

	            if (typeCheckOnly == false) {

		            value = EditorGUILayout.Vector3Field(caption, (Vector3)value);

	            }

            }  else if (type == typeof(FPVector2)) {

	            if (typeCheckOnly == false) {

		            value = (FPVector2)EditorGUILayout.Vector2Field(caption, (FPVector2)value);

	            }

            } else if (type == typeof(FPVector3)) {

	            if (typeCheckOnly == false) {

		            value = (FPVector3)EditorGUILayout.Vector3Field(caption, (FPVector3)value);

	            }

            } else if (type == typeof(Vector4)) {

                if (typeCheckOnly == false) {

                    value = EditorGUILayout.Vector4Field(caption, (Vector4)value);

                }

            } else if (type == typeof(Quaternion)) {

	            if (typeCheckOnly == false) {

		            value = Quaternion.Euler(EditorGUILayout.Vector3Field(caption, ((Quaternion)value).eulerAngles));

	            }

            } else if (type == typeof(FPQuaternion)) {

	            if (typeCheckOnly == false) {

		            value = (FPQuaternion)Quaternion.Euler(EditorGUILayout.Vector3Field(caption, ((FPQuaternion)value).eulerAngles));

	            }

            } else if (type == typeof(pfloat)) {

	            if (typeCheckOnly == false) {

		            value = (pfloat)EditorGUILayout.FloatField(caption, (float)(pfloat)value);

	            }

            } else if (type == typeof(bool)) {

	            if (typeCheckOnly == false) {

		            value = EditorGUILayout.Toggle(caption, (bool)value);

	            }

            } else if (type == typeof(int)) {

                if (typeCheckOnly == false) {

                    value = EditorGUILayout.IntField(caption, (int)value);

                }

            } else if (type == typeof(float)) {

                if (typeCheckOnly == false) {

                    value = EditorGUILayout.FloatField(caption, (float)value);

                }

            } else if (type == typeof(double)) {

                if (typeCheckOnly == false) {

                    value = EditorGUILayout.DoubleField(caption, (double)value);

                }

            } else if (type == typeof(long)) {

                if (typeCheckOnly == false) {

                    value = EditorGUILayout.LongField(caption, (long)value);

                }

            } else if (type.HasBaseType(typeof(UnityEngine.Object)) == true) {

	            if (typeCheckOnly == false) {

		            var obj = (UnityEngine.Object)value;
		            obj = EditorGUILayout.ObjectField(caption, obj, type, allowSceneObjects: true);
		            value = obj;

	            }

            } else if (type == typeof(string)) {

	            if (typeCheckOnly == false) {

		            if (string.IsNullOrEmpty((string)value) == true) value = string.Empty;
		            var str = value.ToString();
		            if (str.Contains("\n") == true) {

			            value = EditorGUILayout.TextArea(str);

		            } else {

			            value = EditorGUILayout.TextField(caption, str);

		            }

	            }

            } else {

	            if (typeCheckOnly == false) {

		            ++EditorGUI.indentLevel;
		            GUILayoutExt.DrawFields(world, ref value);
		            --EditorGUI.indentLevel;
		            
		            /*
		            var str = value.ToString();
		            if (str.Contains("\n") == true) {

			            EditorGUILayout.TextArea(str);

		            } else {

			            EditorGUILayout.TextField(caption, str);

		            }*/

	            }

	            EditorGUI.showMixedValue = false;

	            return true;

            }

            EditorGUI.showMixedValue = false;
            
            return true;

        }

        public static void DataLabel(string content, params GUILayoutOption[] options) {

            var style = new GUIStyle(EditorStyles.label);
            var rect = GUILayoutUtility.GetRect(new GUIContent(content), style, options);
            style.richText = true;
            style.stretchHeight = false;
            style.fixedHeight = 0f;
            EditorGUI.SelectableLabel(rect, content, style);

        }

        public static string GetTypeLabel(System.Type type) {

            var output = type.Name;
            var sOutput = output.Split('`');
            if (sOutput.Length > 0) {

                output = sOutput[0];

            }

            var genericTypes = type.GenericTypeArguments;
            if (genericTypes != null && genericTypes.Length > 0) {

                var sTypes = string.Empty;
                for (int i = 0; i < genericTypes.Length; ++i) {

                    sTypes += (i > 0 ? ", " : string.Empty) + genericTypes[i].Name;

                }

                output += "<" + sTypes + ">";

            }

            return output;

        }

        public static void TypeLabel(System.Type type, params GUILayoutOption[] options) {

            GUILayoutExt.DataLabel(GUILayoutExt.GetTypeLabel(type), options);

        }

        public static void Separator(float lineHeight = 0.5f) {
            
            GUILayoutExt.Separator(new Color(0.1f, 0.1f, 0.1f, 0.2f), lineHeight);
            
        }

        public static void Separator(Color color, float lineHeight = 0.5f) {

            Rect rect = EditorGUILayout.GetControlRect(false, lineHeight);
            rect.height = lineHeight;
            //rect.width += 4f;
            //rect.x -= 2f;
            rect.y -= lineHeight * 0.5f;
            EditorGUI.DrawRect(rect, color);

        }

        public static void TableCaption(string content, GUIStyle style) {

            style = new GUIStyle(style);
            style.alignment = TextAnchor.MiddleCenter;
            style.stretchWidth = true;
            style.stretchHeight = true;

            GUILayout.Label(content, style);

        }

        private static int foldOutLevel;

        public static void FoldOut(ref bool state, string content, System.Action onContent, GUIStyle style = null, System.Action<Rect> onHeader = null) {

            if (style == null) {

                style = new GUIStyle(EditorStyles.foldoutHeader);
                style.fixedWidth = 0f;
                style.stretchWidth = true;

                if (GUILayoutExt.foldOutLevel == 0) {

                    style.fixedHeight = 24f;
                    style.richText = true;
                    content = "<b>" + content + "</b>";

                } else {

                    style.fixedHeight = 16f;
                    style.richText = true;

                }

            }

            ++GUILayoutExt.foldOutLevel;
            state = GUILayoutExt.BeginFoldoutHeaderGroup(state, new GUIContent(content), style, menuAction: onHeader);
            if (state == true) {

	            GUILayout.BeginHorizontal();
	            {
		            GUILayout.Space(10f);
		            GUILayout.BeginVertical();
		            onContent.Invoke();
		            GUILayout.EndVertical();
	            }
	            GUILayout.EndHorizontal();

            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            --GUILayoutExt.foldOutLevel;

        }

        public static bool BeginFoldoutHeaderGroup(
            bool foldout,
            GUIContent content,
            GUIStyle style = null,
            System.Action<Rect> menuAction = null,
            GUIStyle menuIcon = null) {

            return GUILayoutExt.BeginFoldoutHeaderGroup(GUILayoutUtility.GetRect(content, style), foldout, content, style, menuAction, menuIcon);

        }

        public static bool BeginFoldoutHeaderGroup(
            Rect position,
            bool foldout,
            GUIContent content,
            GUIStyle style = null,
            System.Action<Rect> menuAction = null,
            GUIStyle menuIcon = null) {

	        using (new GUIBackgroundAlphaUsing(0.35f)) {
		        //if (EditorGUIUtility.hierarchyMode) position.xMin -= (float)(EditorStyles.inspectorDefaultMargins.padding.left - EditorStyles.inspectorDefaultMargins.padding.right);
		        if (style == null) style = EditorStyles.foldoutHeader;
		        Rect position1 = new Rect() {
			        x = (float)((double)position.xMax - (double)style.padding.right - 16.0),
			        y = position.y + (float)style.padding.top,
			        size = Vector2.one * 16f
		        };
		        bool isHover = position1.Contains(Event.current.mousePosition);
		        bool isActive = isHover && Event.current.type == EventType.MouseDown && Event.current.button == 0;
		        if (menuAction != null && isActive) {
			        menuAction(position1);
			        Event.current.Use();
		        }

		        foldout = GUI.Toggle(position, foldout, content, style);
		        if (menuAction != null && Event.current.type == EventType.Repaint) {
			        if (menuIcon == null) menuIcon = EditorStyles.foldoutHeaderIcon;
			        menuIcon.Draw(position1, isHover, isActive, false, false);
		        }
	        }

	        return foldout;
        }

        public static void Box(float padding, float margin, System.Action onContent, GUIStyle style = null, params GUILayoutOption[] options) {

            GUILayoutExt.Padding(margin, () => {

                if (style == null) {

                    style = "GroupBox";

                } else {

                    style = new GUIStyle(style);

                }

                style.padding = new RectOffset();
                style.margin = new RectOffset();

                GUILayout.BeginVertical(style, options);
                {

                    GUILayoutExt.Padding(padding, onContent);

                }
                GUILayout.EndVertical();

            }, options);

        }

        public static void Padding(float padding, System.Action onContent, params GUILayoutOption[] options) {

            GUILayoutExt.Padding(padding, padding, onContent, options);

        }

        public static void Padding(float paddingX, float paddingY, System.Action onContent, params GUILayoutOption[] options) {

            GUILayoutExt.Padding(paddingX, paddingY, onContent, GUIStyle.none, options);

        }

        public static void Padding(float paddingX, float paddingY, System.Action onContent, GUIStyle style, params GUILayoutOption[] options) {

	        GUILayout.BeginHorizontal(style, options);
	        {
		        GUILayout.Space(paddingX);
	            GUILayout.BeginVertical(options);
	            {
	                GUILayout.Space(paddingY);
                    {
                        onContent.Invoke();
                    }
	                GUILayout.Space(paddingY);
	            }
	            GUILayout.EndVertical();
	            GUILayout.Space(paddingX);
	        }
	        GUILayout.EndHorizontal();

        }

    }

    public class PopupWindowAnim : EditorWindow {

		private const float defaultWidth = 150;
		private const float defaultHeight = 250;
		private const float elementHeight = 20;
		
		/// <summary> Прямоугольник, в котором будет отображен попап </summary>
		public Rect screenRect;
		
		/// <summary> Указывает, что является разделителем в пути </summary>
		public char separator = '/';
		
		/// <summary> Позволяет использовать/убирать поиск </summary>
		public bool useSearch = true;
		
		/// <summary> Название рута </summary>
		public new string title = "Menu";

		public new string name { get { return title; } set { title = value; } }
		
		/// <summary> Стили, используемые для визуализации попапа </summary>
		private static Styles styles;
		
		//Поиск
		/// <summary> Строка поиска </summary>
		public string searchText = "";

		/// <summary> Активен ли поиск? </summary>
		private bool hasSearch { get { return useSearch && !string.IsNullOrEmpty(searchText); } }
		
		//Анимация
		private float _anim;
		private int _animTarget = 1;
		private long _lastTime;
		
		//Элементы
		/// <summary> Список конечных элементов (до вызова Show) </summary>
		private System.Collections.Generic.List<PopupItem> submenu = new System.Collections.Generic.List<PopupItem>();
		/// <summary> Хранит контекст элементов (нужно при заполнении попапа) </summary>
		private System.Collections.Generic.List<string> folderStack = new System.Collections.Generic.List<string>();
		/// <summary> Список элементов (после вызова Show) </summary>
		private Element[] _tree;
		/// <summary> Список элементов, подходящих под условия поиска </summary>
		private Element[] _treeSearch;
		/// <summary> Хранит контексты элементов (после вызова Show) </summary>
		private System.Collections.Generic.List<GroupElement> _stack = new System.Collections.Generic.List<GroupElement>();
		/// <summary> Указывает, нуждается ли выбор нового элемента в прокрутке </summary>
		private bool scrollToSelected;
		
		private Element[] activeTree { get { return (!hasSearch ? _tree : _treeSearch); } }

		private GroupElement activeParent { get { return _stack[(_stack.Count - 2) + _animTarget]; } }

		private Element activeElement {
			get {
				if (activeTree == null)
					return null;
				var childs = GetChildren(activeTree, activeParent);
				if (childs.Count == 0)
					return null;
				return childs[activeParent.selectedIndex];
			}
		}
		
		/// <summary> Создание окна </summary>
		public static PopupWindowAnim Create(Rect screenRect, bool useSearch = true) {
			var popup = CreateInstance<PopupWindowAnim>();
			popup.screenRect = screenRect;
			popup.useSearch = useSearch;
			return popup;
		}
		
		/// <summary> Создание окна </summary>
		public static PopupWindowAnim CreateByPos(Vector2 pos, bool useSearch = true) {
			return Create(new Rect(pos.x, pos.y, defaultWidth, defaultHeight), useSearch);
		}
		
		/// <summary> Создание окна </summary>
		public static PopupWindowAnim CreateByPos(Vector2 pos, float width, bool useSearch = true) {
			return Create(new Rect(pos.x, pos.y, width, defaultHeight), useSearch);
		}
		
		/// <summary> Создание окна. Вызывается из OnGUI()! </summary>
		public static PopupWindowAnim CreateBySize(Vector2 size, bool useSearch = true) {
			var screenPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			return Create(new Rect(screenPos.x, screenPos.y, size.x, size.y), useSearch);
		}
		
		/// <summary> Создание окна. Вызывается из OnGUI()! </summary>
		public static PopupWindowAnim Create(float width, bool useSearch = true) {
			return CreateBySize(new Vector2(width, defaultHeight), useSearch);
		}
		
		/// <summary> Создание окна. Вызывается из OnGUI()! </summary>
		public static PopupWindowAnim Create(bool useSearch = true) {
			return CreateBySize(new Vector2(defaultWidth, defaultHeight), useSearch);
		}
		
		/// <summary> Отображает попап </summary>
		public new void Show() {
			if (submenu.Count == 0)
				DestroyImmediate(this);
			else
				Init();
		}
		
		/// <summary> Отображает попап </summary>
		public void ShowAsDropDown() {
			Show();
		}
		
		public void SetHeightByElementCount(int elementCount) {
			screenRect.height = elementCount * elementHeight + (useSearch ? 30f : 0f) + 26f;
		}
		
		public void SetHeightByElementCount() {
			SetHeightByElementCount(maxElementCount);
		}
		
		public bool autoHeight;
		public bool autoClose;
		
		public void BeginRoot(string folderName) {
			var previous = folderStack.Count != 0 ? folderStack[folderStack.Count - 1] : "";
			if (string.IsNullOrEmpty(folderName))
				folderName = "<Noname>";
			if (!string.IsNullOrEmpty(previous))
				folderStack.Add(previous + separator + folderName);
			else
				folderStack.Add(folderName);
		}
		
		public void EndRoot() {
			if (folderStack.Count > 0)
				folderStack.RemoveAt(folderStack.Count - 1);
			else
				throw new Exception("Excess call EndFolder()");
		}
		
		public void EndRootAll() {
			while (folderStack.Count > 0)
				folderStack.RemoveAt(folderStack.Count - 1);
		}
		
		public void Item(string title, Texture2D image, Action action) {
			var folder = "";
			if (folderStack.Count > 0)
				folder = folderStack[folderStack.Count - 1] ?? "";
			submenu.Add(string.IsNullOrEmpty(folder)
				            ? new PopupItem(this.title + separator + title, action) { image = image }
				            : new PopupItem(this.title + separator + folder + separator + title, action) { image = image });
		}

		public void Item(string title, Texture2D image, Action<PopupItem> action, bool searchable) {
			var folder = "";
			if (folderStack.Count > 0)
				folder = folderStack[folderStack.Count - 1] ?? "";
			submenu.Add(string.IsNullOrEmpty(folder)
			            ? new PopupItem(this.title + separator + title, action) { image = image, searchable = searchable }
			: new PopupItem(this.title + separator + folder + separator + title, action) { image = image, searchable = searchable });
		}
		
		public void Item(string title, Action action) {
			var folder = "";
			if (folderStack.Count > 0)
				folder = folderStack[folderStack.Count - 1] ?? "";
			submenu.Add(string.IsNullOrEmpty(folder)
			            ? new PopupItem(this.title + separator + title, action)
			            : new PopupItem(this.title + separator + folder + separator + title, action));
		}
		
		public void Item(string title) {
			var folder = "";
			if (folderStack.Count > 0)
				folder = folderStack[folderStack.Count - 1] ?? "";
			submenu.Add(string.IsNullOrEmpty(folder)
			            ? new PopupItem(this.title + separator + title, () => { })
			            : new PopupItem(this.title + separator + folder + separator + title, () => { }));
		}
		
		public void ItemByPath(string path, Texture2D image, Action action) {
			if (string.IsNullOrEmpty(path))
				path = "<Noname>";
			submenu.Add(new PopupItem(title + separator + path, action) { image = image });
		}
		
		public void ItemByPath(string path, Action action) {
			if (string.IsNullOrEmpty(path))
				path = "<Noname>";
			submenu.Add(new PopupItem(title + separator + path, action));
		}
		
		public void ItemByPath(string path) {
			if (string.IsNullOrEmpty(path))
				path = "<Noname>";
			submenu.Add(new PopupItem(title + separator + path, () => { }));
		}
		
		private void Init() {
			CreateComponentTree();
			if (autoHeight)
				SetHeightByElementCount();
			ShowAsDropDown(new Rect(screenRect.x, screenRect.y, 1, 1), new Vector2(screenRect.width, screenRect.height));
			Focus();
			wantsMouseMove = true;
		}
		
		private void CreateComponentTree() {

			var list = new System.Collections.Generic.List<string>();
			var elements = new System.Collections.Generic.List<Element>();

			this.submenu = this.submenu.OrderBy(x => x.path).ToList();
			
			for (int i = 0; i < submenu.Count; i++) {

				var submenuItem = submenu[i];
				string menuPath = submenuItem.path;
				var separators = new[] { separator };
				var pathParts = menuPath.Split(separators);

				while (pathParts.Length - 1 < list.Count) {

					list.RemoveAt(list.Count - 1);

				}

				while (list.Count > 0 && pathParts[list.Count - 1] != list[list.Count - 1]) {

					list.RemoveAt(list.Count - 1);

				}

				while (pathParts.Length - 1 > list.Count) {

					elements.Add(new GroupElement(list.Count, pathParts[list.Count]));
					list.Add(pathParts[list.Count]);

				}

				elements.Add(new CallElement(list.Count, pathParts[pathParts.Length - 1], submenuItem));

			}

			_tree = elements.ToArray();
			for (int i = 0; i < _tree.Length; i++) {
				var elChilds = GetChildren(_tree, _tree[i]);
				if (elChilds.Count > maxElementCount)
					maxElementCount = elChilds.Count;
			}
			if (_stack.Count == 0) {
				_stack.Add(_tree[0] as GroupElement);
				goto to_research;
			}
			var parent = _tree[0] as GroupElement;
			var level = 0;
			to_startCycle:
			var stackElement = _stack[level];
			_stack[level] = parent;
			if (_stack[level] != null) {
				_stack[level].selectedIndex = stackElement.selectedIndex;
				_stack[level].scroll = stackElement.scroll;
			}
			level++;
			if (level != _stack.Count) {
				var childs = GetChildren(activeTree, parent);
				var child = childs.FirstOrDefault(x => _stack[level].name == x.name);
				if (child is GroupElement)
					parent = child as GroupElement;
				else
					while (_stack.Count > level)
						_stack.RemoveAt(level);
				goto to_startCycle;
			}
			to_research:
			s_DirtyList = false;
			RebuildSearch();
		}
		
		private int maxElementCount = 1;
		private static bool s_DirtyList = true;

		private void RebuildSearch() {
			if (!hasSearch) {
				_treeSearch = null;
				if (_stack[_stack.Count - 1].name == "Search") {
					_stack.Clear();
					_stack.Add(_tree[0] as GroupElement);
				}
				_animTarget = 1;
				_lastTime = DateTime.Now.Ticks;
			}
			else {
				var separatorSearch = new[] { ' ', separator };
				var searchLowerWords = searchText.ToLower().Split(separatorSearch);
				var firstElements = new System.Collections.Generic.List<Element>();
				var otherElements = new System.Collections.Generic.List<Element>();
				foreach (var element in _tree) {
					if (!(element is CallElement))
						continue;
					if (element.searchable == false) continue;
					var elementNameShortLower = element.name.ToLower().Replace(" ", string.Empty);
					var itsSearchableItem = true;
					var firstContainsFlag = false;
					for (int i = 0; i < searchLowerWords.Length; i++) {
						var searchLowerWord = searchLowerWords[i];
						if (elementNameShortLower.Contains(searchLowerWord)) {
							if (i == 0 && elementNameShortLower.StartsWith(searchLowerWord))
								firstContainsFlag = true;
						}
						else {
							itsSearchableItem = false;
							break;
						}
					}
					if (itsSearchableItem) {
						if (firstContainsFlag)
							firstElements.Add(element);
						else
							otherElements.Add(element);
					}
				}
				firstElements.Sort();
				otherElements.Sort();
				
				var searchElements = new System.Collections.Generic.List<Element>
				{ new GroupElement(0, "Search") };
				searchElements.AddRange(firstElements);
				searchElements.AddRange(otherElements);
				//            searchElements.Add(_tree[_tree.Length - 1]);
				_treeSearch = searchElements.ToArray();
				_stack.Clear();
				_stack.Add(_treeSearch[0] as GroupElement);
				if (GetChildren(activeTree, activeParent).Count >= 1)
					activeParent.selectedIndex = 0;
				else
					activeParent.selectedIndex = -1;
			}
		}
		
		public void OnGUI() {
			if (_tree == null) {
				Close();
				return; 
			}
			//Создание стиля
			if (styles == null)
				styles = new Styles();
			//Фон
			if (s_DirtyList)
				CreateComponentTree();
			HandleKeyboard();
			GUI.Label(new Rect(0, 0, position.width, position.height), GUIContent.none, styles.background);
			
			//Поиск
			if (useSearch) {
				GUILayout.Space(7f);
				var rectSearch = GUILayoutUtility.GetRect(10f, 20f);
				rectSearch.x += 8f;
				rectSearch.width -= 16f;
				EditorGUI.FocusTextInControl("ComponentSearch");
				GUI.SetNextControlName("ComponentSearch");
				if (SearchField(rectSearch, ref searchText))
					RebuildSearch();
			}
			
			//Элементы
			ListGUI(activeTree, _anim, GetElementRelative(0), GetElementRelative(-1));
			if (_anim < 1f && _stack.Count > 1)
				ListGUI(activeTree, _anim + 1f, GetElementRelative(-1), GetElementRelative(-2));
			if (_anim != _animTarget && Event.current.type == EventType.Repaint) {
				var ticks = DateTime.Now.Ticks;
				var coef = (ticks - _lastTime) / 1E+07f;
				_lastTime = ticks;
				_anim = Mathf.MoveTowards(_anim, _animTarget, coef * 4f);
				if (_animTarget == 0 && _anim == 0f) {
					_anim = 1f;
					_animTarget = 1;
					_stack.RemoveAt(_stack.Count - 1);
				}
				Repaint();
			}
		}
		
		private void HandleKeyboard() {
			Event current = Event.current;
			if (current.type == EventType.KeyDown) {
				if (current.keyCode == KeyCode.DownArrow) {
					activeParent.selectedIndex++;
					activeParent.selectedIndex = Mathf.Min(activeParent.selectedIndex,
					                                       GetChildren(activeTree, activeParent).Count - 1);
					scrollToSelected = true;
					current.Use();
				}
				if (current.keyCode == KeyCode.UpArrow) {
					GroupElement element2 = activeParent;
					element2.selectedIndex--;
					activeParent.selectedIndex = Mathf.Max(activeParent.selectedIndex, 0);
					scrollToSelected = true;
					current.Use();
				}
				if (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter) {
					GoToChild(activeElement, true);
					current.Use();
				}
				if (!hasSearch) {
					if (current.keyCode == KeyCode.LeftArrow || current.keyCode == KeyCode.Backspace) {
						GoToParent();
						current.Use();
					}
					if (current.keyCode == KeyCode.RightArrow) {
						GoToChild(activeElement, false);
						current.Use();
					}
					if (current.keyCode == KeyCode.Escape) {
						Close();
						current.Use();
					}
				}
			}
		}
		
		private static bool SearchField(Rect position, ref string text) {
			var rectField = position;
			rectField.width -= 15f;
			var startText = text;
			text = GUI.TextField(rectField, startText ?? "", styles.searchTextField);
			
			var rectCancel = position;
			rectCancel.x += position.width - 15f;
			rectCancel.width = 15f;
			var styleCancel = text == "" ? styles.searchCancelButtonEmpty : styles.searchCancelButton;
			if (GUI.Button(rectCancel, GUIContent.none, styleCancel) && text != "") {
				text = "";
				GUIUtility.keyboardControl = 0;
			}
			return startText != text;
		}
		
		private void ListGUI(Element[] tree, float anim, GroupElement parent, GroupElement grandParent) {
			anim = Mathf.Floor(anim) + Mathf.SmoothStep(0f, 1f, Mathf.Repeat(anim, 1f));
			Rect rectArea = position;
			rectArea.x = position.width * (1f - anim) + 1f;
			rectArea.y = useSearch ? 30f : 0;
			rectArea.height -= useSearch ? 30f : 0;
			rectArea.width -= 2f;
			GUILayout.BeginArea(rectArea);
			{
				var rectHeader = GUILayoutUtility.GetRect(10f, 25f);
				var nameHeader = parent.name;
				GUI.Label(rectHeader, nameHeader, styles.header);
				if (grandParent != null) {
					var rectHeaderBackArrow = new Rect(rectHeader.x + 4f, rectHeader.y + 7f, 13f, 13f);
					if (Event.current.type == EventType.Repaint)
						styles.leftArrow.Draw(rectHeaderBackArrow, false, false, false, false);
					if (Event.current.type == EventType.MouseDown && rectHeader.Contains(Event.current.mousePosition)) {
						GoToParent();
						Event.current.Use();
					}
				}
				ListGUI(tree, parent);
			}
			GUILayout.EndArea();
		}
		
		private void ListGUI(Element[] tree, GroupElement parent) {
			parent.scroll = GUILayout.BeginScrollView(parent.scroll, new GUILayoutOption[0]);
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
			var children = GetChildren(tree, parent);
			var rect = new Rect();
			for (int i = 0; i < children.Count; i++) {
				var e = children[i];
				var options = new[] { GUILayout.ExpandWidth(true) };
				var rectElement = GUILayoutUtility.GetRect(16f, elementHeight, options);
				if ((Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDown) 
					&& parent.selectedIndex != i && rectElement.Contains(Event.current.mousePosition)) {
					parent.selectedIndex = i;
					Repaint();
				}
				bool on = false;
				if (i == parent.selectedIndex) {
					on = true;
					rect = rectElement;
				}
				if (Event.current.type == EventType.Repaint) {
					(e.content.image != null ? styles.componentItem : styles.groupItem).Draw(rectElement, e.content, false, false, on, on);
					if (!(e is CallElement)) {
						var rectElementForwardArrow = new Rect(rectElement.x + rectElement.width - 13f, rectElement.y + 4f, 13f, 13f);
						styles.rightArrow.Draw(rectElementForwardArrow, false, false, false, false);
					}
				}
				if (Event.current.type == EventType.MouseDown && rectElement.Contains(Event.current.mousePosition)) {
					Event.current.Use();
					parent.selectedIndex = i;
					GoToChild(e, true);
				}
			}
			EditorGUIUtility.SetIconSize(Vector2.zero);
			GUILayout.EndScrollView();
			if (scrollToSelected && Event.current.type == EventType.Repaint) {
				scrollToSelected = false;
				var lastRect = GUILayoutUtility.GetLastRect();
				if ((rect.yMax - lastRect.height) > parent.scroll.y) {
					parent.scroll.y = rect.yMax - lastRect.height;
					Repaint();
				}
				if (rect.y < parent.scroll.y) {
					parent.scroll.y = rect.y;
					Repaint();
				}
			}
		}
		
		private void GoToParent() {
			if (_stack.Count <= 1) 
				return;
			_animTarget = 0;
			_lastTime = DateTime.Now.Ticks;
		}
		
		private void GoToChild(Element e, bool addIfComponent) {
			var element = e as CallElement;
			if (element != null) {
				if (!addIfComponent) 
					return;
				element.action();
				if (this.autoClose == true) Close();
			}
			else if (!hasSearch) {
					_lastTime = DateTime.Now.Ticks;
					if (_animTarget == 0)
						_animTarget = 1;
					else if (_anim == 1f) {
							_anim = 0f;
							_stack.Add(e as GroupElement);
						}
				}
		}
		
		private System.Collections.Generic.List<Element> GetChildren(Element[] tree, Element parent) {
			var list = new System.Collections.Generic.List<Element>();
			var num = -1;
			var index = 0;
			while (index < tree.Length) {
				if (tree[index] == parent) {
					num = parent.level + 1;
					index++;
					break;
				}
				index++;
			}
			if (num == -1) 
				return list;
			while (index < tree.Length) {
				var item = tree[index];
				if (item.level < num)
					return list;
				if (item.level <= num || hasSearch)
					list.Add(item);
				index++;
			}
			return list;
		}
		
		private GroupElement GetElementRelative(int rel) {
			int num = (_stack.Count + rel) - 1;
			return num < 0 ? null : _stack[num];
		}
		
		
		private class CallElement : Element {
			public Action action;
			
			public CallElement(int level, string name, PopupItem item) {
				base.level = level;
				content = new GUIContent(name, item.image);
				action = () => {
					item.action();
					content = new GUIContent(name, item.image);
				};
				this.searchable = item.searchable;
			}
		}
		
		[Serializable]
		private class GroupElement : Element {
			public Vector2 scroll;
			public int selectedIndex;
			
			public GroupElement(int level, string name) {
				this.level = level;
				content = new GUIContent(name);
				this.searchable = true;
			}
		}
		
		private class Element : IComparable {
			public GUIContent content;
			public int level;
			public bool searchable;
			
			public string name { get { return content.text; } }
			
			public int CompareTo(object o) {
				return String.Compare(name, ((Element)o).name, StringComparison.Ordinal);
			}
		}
		
		private class Styles {
			public GUIStyle searchTextField = "SearchTextField";
			public GUIStyle searchCancelButton = "SearchCancelButton";
			public GUIStyle searchCancelButtonEmpty = "SearchCancelButtonEmpty";
			public GUIStyle background = "grey_border";
			public GUIStyle componentItem = new GUIStyle("PR Label");
			public GUIStyle groupItem;
			public GUIStyle header = new GUIStyle("In BigTitle");
			public GUIStyle leftArrow = "AC LeftArrow";
			public GUIStyle rightArrow = "AC RightArrow";
			
			public Styles() {
				header.font = EditorStyles.boldLabel.font;
				header.richText = true;
				componentItem.alignment = TextAnchor.MiddleLeft;
				componentItem.padding.left -= 15;
				componentItem.fixedHeight = 20f;
				componentItem.richText = true;
				groupItem = new GUIStyle(componentItem);
				groupItem.padding.left += 0x11;
				groupItem.richText = true;
			}
		}
		
		public class PopupItem {
			public PopupItem(string path, Action action) {
				this.path = path;
				this.action = action;
			}
			
			public PopupItem(string path, Action<PopupItem> action) {
				this.path = path;
				this.action = () => { action(this); };
			}

			public string path;
			public Texture2D image;
			public Action action;
			public bool searchable;

		}
	}
    
    public class Popup {
		/// <summary> Окно, которое связано с попапом </summary>
		internal PopupWindowAnim window;
		/// <summary> Прямоугольник, в котором будет отображен попап </summary>
		public Rect screenRect { get { return window.screenRect; } set { window.screenRect = value; } }
		
		/// <summary> Указывает, что является разделителем в пути </summary>
		public char separator { get { return window.separator; } set { window.separator = value; } }
		
		/// <summary> Позволяет использовать/убирать поиск </summary>
		public bool useSearch { get { return window.useSearch; } set { window.useSearch = value; } }

		/// <summary> Название рута </summary>
		public string title { get { return window.title; } set { window.title = value; } }

		/// <summary> Название рута </summary>
		public string searchText { get { return window.searchText; } set { window.searchText = value; } }

		/// <summary> Автоматически установить размер по высоте, узнав максимальное количество видимых элементов </summary>
		public bool autoHeight { get { return window.autoHeight; } set { window.autoHeight = value; } }
		public bool autoClose { get { return window.autoClose; } set { window.autoClose = value; } }
		
		/// <summary> Создание окна </summary>
		public Popup(Rect screenRect, bool useSearch = true, string title = "Menu", char separator = '/') {
			window = PopupWindowAnim.Create(screenRect, useSearch);
			this.title = title;
			this.separator = separator;
		}
		
		/// <summary> Создание окна </summary>
		public Popup(Vector2 size, bool useSearch = true, string title = "Menu", char separator = '/') {
			window = PopupWindowAnim.CreateBySize(size, useSearch);
			this.title = title;
			this.separator = separator;
		}
		
		/// <summary> Создание окна </summary>
		public Popup(float width, bool useSearch = true, string title = "Menu", char separator = '/', bool autoHeight = true) {
			window = PopupWindowAnim.Create(width, useSearch);
			this.title = title;
			this.separator = separator;
			this.autoHeight = autoHeight;
		}
		
		/// <summary> Создание окна </summary>
		public Popup(bool useSearch = true, string title = "Menu", char separator = '/', bool autoHeight = true) {
			window = PopupWindowAnim.Create(useSearch);
			this.title = title;
			this.separator = separator;
			this.autoHeight = autoHeight;
		}
		
		public void BeginFolder(string folderName) {
			window.BeginRoot(folderName);
		}
		
		public void EndFolder() {
			window.EndRoot();
		}
		
		public void EndFolderAll() {
			window.EndRootAll();
		}
		
		public void Item(string name) {
			window.Item(name);
		}
		
		public void Item(string name, Action action) {
			window.Item(name, action);
		}
		
		public void Item(string name, Texture2D image, Action action) {
			window.Item(name, image, action);
		}

		public void Item(string name, Texture2D image, Action<PopupWindowAnim.PopupItem> action, bool searchable = true) {
			window.Item(name, image, action, searchable);
		}

		public void ItemByPath(string path) {
			window.ItemByPath(path);
		}
		
		public void ItemByPath(string path, Action action) {
			window.ItemByPath(path, action);
		}
		
		public void ItemByPath(string path, Texture2D image, Action action) {
			window.ItemByPath(path, image, action);
		}
		
		public void Show() {
			window.Show();
		}
		
		public static void DrawInt(GUIContent label, string selected, System.Action<int> onResult, GUIContent[] options, int[] keys) {
			
			DrawInt_INTERNAL(new Rect(), selected, label, onResult, options, keys, true);
			
		}

		public static void DrawInt(Rect rect, string selected, GUIContent label, System.Action<int> onResult, GUIContent[] options, int[] keys) {

			DrawInt_INTERNAL(rect, selected, label, onResult, options, keys, false);

		}

		private static void DrawInt_INTERNAL(Rect rect, string selected, GUIContent label, System.Action<int> onResult, GUIContent[] options, int[] keys, bool layout) {

			var state = false;
			if (layout == true) {

				GUILayout.BeginHorizontal();
				if (label != null) GUILayout.Label(label);
				if (GUILayout.Button(selected, EditorStyles.popup) == true) {
					
					state = true;
					
				}
				GUILayout.EndHorizontal();

			} else {
				
				if (label != null) rect = EditorGUI.PrefixLabel(rect, label);
				if (GUI.Button(rect, selected, EditorStyles.popup) == true) {
					
					state = true;
					
				}
				
			}
			
			if (state == true) {

				Popup popup = null;
				if (layout == true) {

					popup = new Popup() { title = (label == null ? string.Empty : label.text), screenRect = new Rect(rect.x, rect.y + rect.height, rect.width, 200f) };
					
				} else {
					
					Vector2 vector = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
					rect.x = vector.x;
					rect.y = vector.y;
					
					popup = new Popup() { title = (label == null ? string.Empty : label.text), screenRect = new Rect(rect.x, rect.y + rect.height, rect.width, 200f) };
					
				}
				
				for (int i = 0; i < options.Length; ++i) {
					
					var option = options[i];
					var result = keys[i];
					popup.ItemByPath(option.text, () => {
						
						onResult(result);
						
					});
					
				}
				
				popup.Show();

			}

		}

	}
    
}
