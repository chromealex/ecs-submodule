
using System.Linq;
using ME.ECS;

namespace ME.ECSEditor.BlackBox {
    
    using ME.ECS.BlackBox;
    using UnityEditor;
    using UnityEngine;

    public class BBCustomEditor : System.Attribute {

        public System.Type type;

        public BBCustomEditor(System.Type type) {
            
            this.type = type;
            
        }

    }

    public interface ICustomEditor {

        float GetHeight(SerializedObject obj);
        void OnGUI(Rect rect, SerializedObject obj);

    }

    public class BlackBoxContainerEditor : EditorWindow {

        public const float BOX_PADDING = 10f;
        public const float GRID_SIZE = 20f;

        private static class Styles {

            public const float horizontalSpacing = 100f;
            public const float verticalSpacing = 6f;
            public const float width = 220f;
            public const float nodeHeight = 60f;
            public const float beginEndTickHeight = 10000f;
            public static GUIStyle nodeStyle;
            public static GUIStyle nodeCustomStyle;
            public static GUIStyle containerStyle;
            public static GUIStyle containerCaption;
            public static GUIStyle enterStyle;
            public static GUIStyle exitStyle;
            public static GUIStyle nodeCaption;
            public static GUIStyle beginTickStyle;
            public static GUIStyle endTickStyle;
            public static GUIStyle systemNode;
            public static GUIStyle measureLabel;
            public static Color measureLabelNormal;
            public static Color measureLabelWarning;
            public static Color measureLabelError;
            public static Texture2D connectionTexture;
            public static GUIStyle background;
            public static Color gridColor = new Color(0f, 0f, 0f, 1f);

            public static Texture2D connectionTextureDotted;

            public static GUIStyle fixedFontLabel;
            private static Font fixedFont;

            static Styles() {
                
                Styles.Init();
                
            }

            public static Color GetColorForMeasuring(float tickTime, double ts) {

                var curColor = Styles.measureLabelNormal;
                if (ts > tickTime * 1000f) {

                    curColor = Styles.measureLabelError;

                } else if (ts > tickTime * 1000f * 0.5f) {
                        
                    curColor = Styles.measureLabelWarning;
                        
                }

                return curColor;

            }
            
            public static string ColorToHex(Color color) {

                return ColorUtility.ToHtmlStringRGB(color);
                
            }

            public static void Init() {

                {
                    var t = new Texture2D(4, 4, TextureFormat.RGBA32, false);
                    var arr = new Color32[t.width * t.height];
                    for (int i = 0; i < t.width * t.height; ++i) {
                        arr[i] = new Color(1f, 1f, 1f, 1f);
                    }

                    var alpha = new Color(0f, 0f, 0f, 0f);
                    arr[2] = alpha;
                    arr[3] = alpha;
                    arr[6] = alpha;
                    arr[7] = alpha;
                    arr[10] = alpha;
                    arr[11] = alpha;
                    arr[14] = alpha;
                    arr[15] = alpha;

                    t.SetPixels32(arr);
                    t.Apply();
                    Styles.connectionTextureDotted = t;
                }

                {
                    var t = new Texture2D(3, 3, TextureFormat.RGBA32, false);
                    var arr = new Color32[t.width * t.height];
                    for (int i = 0; i < t.width * t.height; ++i) {
                        arr[i] = new Color(0.3f, 0.5f, 1f, 0.5f);
                    }

                    t.SetPixels32(arr);
                    t.Apply();
                    Styles.connectionTexture = t;
                }

                {
                    Styles.background = new GUIStyle();
                    var t = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    var arr = new Color32[t.width * t.height];
                    for (int i = 0; i < t.width * t.height; ++i) {
                        arr[i] = new Color(0.07f, 0.07f, 0.07f, 1f);
                    }

                    t.SetPixels32(arr);
                    t.Apply();
                    Styles.background.normal.background = t;
                }

                Styles.nodeStyle = new GUIStyle();
                Styles.nodeStyle.normal.background = EditorStyles.miniButton.normal.scaledBackgrounds[0];//EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
                Styles.nodeStyle.border = new RectOffset(12, 12, 12, 12);

                Styles.nodeCustomStyle = new GUIStyle();
                Styles.nodeCustomStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;
                Styles.nodeCustomStyle.border = new RectOffset(12, 12, 12, 12);

                Styles.systemNode = new GUIStyle();
                Styles.systemNode.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node2.png") as Texture2D;
                Styles.systemNode.border = new RectOffset(12, 12, 12, 12);

                Styles.measureLabelNormal = Color.green;
                Styles.measureLabelWarning = Color.yellow;
                Styles.measureLabelError = Color.red;

                Styles.containerCaption = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                Styles.containerCaption.alignment = TextAnchor.UpperCenter;

                Styles.nodeCaption = new GUIStyle(EditorStyles.label);
                Styles.nodeCaption.alignment = TextAnchor.LowerCenter;
                Styles.nodeCaption.padding = new RectOffset(0, 0, 0, 15);
                
                Styles.containerStyle = new GUIStyle(EditorStyles.helpBox);

                Styles.enterStyle = new GUIStyle();
                Styles.enterStyle.normal.background = EditorStyles.miniButton.onNormal.scaledBackgrounds[0];//EditorGUIUtility.Load("builtin skins/darkskin/images/node5.png") as Texture2D;
                Styles.enterStyle.border = new RectOffset(12, 12, 12, 0);

                Styles.exitStyle = new GUIStyle();
                Styles.exitStyle.normal.background = EditorStyles.miniButton.onNormal.scaledBackgrounds[0];//EditorGUIUtility.Load("builtin skins/darkskin/images/node5.png") as Texture2D;
                Styles.exitStyle.border = new RectOffset(12, 12, 0, 12);

                Styles.beginTickStyle = new GUIStyle();
                Styles.beginTickStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3.png") as Texture2D;
                Styles.beginTickStyle.border = new RectOffset(12, 12, 12, 12);

                Styles.endTickStyle = new GUIStyle();
                Styles.endTickStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node4.png") as Texture2D;
                Styles.endTickStyle.border = new RectOffset(12, 12, 12, 12);

                if (Styles.fixedFontLabel == null) {
                    
                    Styles.fixedFontLabel = new GUIStyle(EditorStyles.miniLabel);
                    string fontName;
                    if (Application.platform == RuntimePlatform.WindowsEditor) {
                        fontName = "Consolas";
                    } else {
                        fontName = "Courier";
                    }

                    Styles.CleanupFont();

                    Styles.fixedFont = Font.CreateDynamicFontFromOSFont(fontName, Styles.fixedFontLabel.fontSize);
                    Styles.fixedFontLabel.richText = true;
                    Styles.fixedFontLabel.font = Styles.fixedFont;
                    Styles.fixedFontLabel.fontSize = Styles.fixedFontLabel.fontSize;
                    
                    Styles.measureLabel = new GUIStyle(Styles.fixedFontLabel);
                    Styles.measureLabel.richText = true;
                    Styles.measureLabel.padding = new RectOffset(0, 0, 15, 0);
                    Styles.measureLabel.alignment = TextAnchor.UpperCenter;

                }
                
                //Styles.measureLabel = new GUIStyle(EditorStyles.miniBoldLabel);
                //Styles.measureLabel.richText = true;
                //Styles.measureLabel.padding = new RectOffset(0, 0, 15, 0);
                //Styles.measureLabel.alignment = TextAnchor.UpperCenter;

            }

            public static void CleanupFont() {
                
                if (Styles.fixedFont != null) {
                    
                    Object.DestroyImmediate(Styles.fixedFont, true);
                    Styles.fixedFont = null;
                    
                }
                
            }

        }

        public static BlackBoxContainerEditor active;

        [MenuItem("ME.ECS/Black Box/Container Editor...")]
        public static BlackBoxContainerEditor Open() {
            
            var window = BlackBoxContainerEditor.GetWindow<BlackBoxContainerEditor>();
            window.titleContent = new UnityEngine.GUIContent("Black Box");
            window.Show();
            return window;

        }
        
        [UnityEditor.Callbacks.OnOpenAssetAttribute(1)]
        public static bool OpenAsset(int instanceID, int line) {

            var path = AssetDatabase.GetAssetPath(instanceID);
            {
                var asset = AssetDatabase.LoadMainAssetAtPath(path) as Blueprint;
                if (asset != null) {

                    var window = BlackBoxContainerEditor.Open();
                    window.SetContainer(null);
                    window.SetBlueprint(asset);
                    window.titleContent = new UnityEngine.GUIContent("Black Box [" + asset.name + "]");
                    return true;

                }
            }

            {
                var asset = AssetDatabase.LoadMainAssetAtPath(path) as Container;
                if (asset != null) {

                    var window = BlackBoxContainerEditor.Open();
                    window.SetContainer(asset);
                    window.titleContent = new UnityEngine.GUIContent("Black Box [" + asset.name + "]");
                    return true;

                }
            }

            return false;

        }

        public Container container;
        private SerializedObject containerSerialized;
        public Blueprint blueprint;
        private SerializedObject blueprintSerialized;
        private SerializedObject outputSerialized;
        
        public Rect rect;
        public Rect localRect;
        public Vector2 scrollPosition;

        private float zoom {
            get {
                return EditorPrefs.GetFloat("ME.ECS.BlackBox.ZoomValue", 1f);
            }
            set {
                EditorPrefs.SetFloat("ME.ECS.BlackBox.ZoomValue", value);
            }
        }

        public void Update() {
            
            this.Repaint();
            
        }

        public void OnEnable() {

            this.CollectEditors();

        }

        private void OnGUI() {

            Styles.Init();

            if ((this.blueprint != null && this.blueprintSerialized == null) || (this.container != null && this.containerSerialized == null)) {

                if (this.container != null) {
                    
                    this.SetContainer(this.container);
                    return;
                    
                }

                if (this.blueprint != null) {
                    
                    this.SetBlueprint(this.blueprint);
                    return;
                    
                }

            }
            
            if (this.blueprint == null || this.blueprintSerialized == null) {
                
                this.DrawContainerChooser();
                return;
                
            }

            /*if (Event.current.shift == true) {
                Debug.Log(Compiler.Make(this.container));
            }*/
            
            BlackBoxContainerEditor.active = this;
            this.blueprintSerialized = new SerializedObject(this.blueprint);

            this.rect = this.position;
            this.localRect = new Rect(0f, 0f, this.rect.width, this.rect.height);
            
            this.connections.Clear();
            this.DrawBackground();
            EditorZoomArea.Begin(this.zoom, this.localRect);
            {
                this.DrawGrid();
                this.DrawGraph();
                this.DrawCurrentConnection();
                this.DrawConnections();
            }
            EditorZoomArea.End();

            this.ProcessEvents(Event.current);
            this.DrawSelectionBox();
            this.DrawMiniMap();
            
            BlackBoxContainerEditor.active = null;

        }

        private readonly System.Collections.Generic.Dictionary<System.Type, ICustomEditor> customEditors = new System.Collections.Generic.Dictionary<System.Type, ICustomEditor>();

        public ICustomEditor GetEditor(System.Type type) {

            if (this.customEditors.TryGetValue(type, out var ed) == true) return ed;
            return null;

        }
        
        public void CollectEditors() {

            var searchAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            this.customEditors.Clear();
            foreach (var asm in searchAssemblies) {

                var types = asm.GetTypes();
                foreach (var type in types) {

                    var attrs = type.GetCustomAttributes(typeof(BBCustomEditor), inherit: true);
                    if (attrs.Length > 0) {

                        if (attrs[0] is BBCustomEditor attr) {

                            if (typeof(ICustomEditor).IsAssignableFrom(type) == true) {

                                var editor = (ICustomEditor)System.Activator.CreateInstance(type);
                                if (this.customEditors.ContainsKey(attr.type) == false) {

                                    this.customEditors.Add(attr.type, editor);

                                }

                            }

                        }

                    }

                }

            }
            
        }

        private void DrawMiniMap() {

            if (Event.current.alt == true ||
                Event.current.button == 2) {

                using (new GUILayoutExt.GUIAlphaUsing(0.3f)) {
                    
                    using (new GUILayoutExt.HandlesAlphaUsing(0.5f)) {

                        var aspect = this.localRect.width / this.localRect.height;
                        var size = 120f;
                        var rect = new Rect(0f, 0f, size * aspect, size / aspect);
                        EditorGUI.DrawRect(rect, Color.black);

                        var minMaxRect = new Rect();
                        for (int i = 0; i < this.blueprint.boxes.Length; ++i) {

                            var item = this.blueprint.boxes[i];
                            var r = item.rect;
                            r.position -= this.scrollPosition;
                            if (minMaxRect.xMin > r.xMin) minMaxRect.xMin = r.xMin;
                            if (minMaxRect.xMax < r.xMax) minMaxRect.xMax = r.xMax;
                            if (minMaxRect.yMin > r.yMin) minMaxRect.yMin = r.yMin;
                            if (minMaxRect.yMax < r.yMax) minMaxRect.yMax = r.yMax;

                        }

                        var factor = Mathf.Min(rect.width / minMaxRect.width, rect.height / minMaxRect.height);
                        var offsetX = -minMaxRect.xMin;
                        var offsetY = -minMaxRect.yMin;

                        foreach (var item in this.blueprint.boxes) {

                            var r = item.rect;
                            r.position = new Vector2((item.position.x + offsetX) * factor, (item.position.y + offsetY) * factor);
                            r.size *= factor;
                            EditorGUI.DrawRect(r, Color.red);

                        }

                        var connectionColor = Color.white;
                        foreach (var item in this.connections) {

                            var fromPos = (item.@from - this.scrollPosition).XY() * factor;
                            var toPos = (item.to - this.scrollPosition).XY() * factor;

                            Handles.DrawBezier(fromPos,
                                               toPos,
                                               this.GetTangent(fromPos, toPos),
                                               this.GetTangent(toPos, fromPos),
                                               connectionColor,
                                               Texture2D.whiteTexture,
                                               1f);

                        }

                        var rView = new Rect(-this.scrollPosition * factor, this.localRect.size);
                        //rView.position += new Vector2(0f, this.localRect.height * 0.5f) * factor;
                        rView.size *= factor * (1f / this.zoom);
                        EditorGUI.DrawRect(rView, Color.blue);

                    }

                }
                
            }
            
        }

        private void DrawRootNode(Vector2 offset) {

            var so = this.container != null ? this.containerSerialized : this.blueprintSerialized;
            var editor = this.GetEditor(so.targetObject.GetType());
            var width = (this.container != null ? 260f : 80f);
            this.DrawNode(true, BlackBoxContainerEditor.BOX_PADDING, so, false, new Rect(offset.x, offset.y, width, 200f), "Input", (r) => {

                var h = 0f;
                if (editor != null) {

                    h = editor.GetHeight(so);

                } else {

                    if (this.container != null) {

                        var iterBlueprint = this.blueprintSerialized.GetIterator();
                        iterBlueprint.NextVisible(true);
                        iterBlueprint.NextVisible(false);
                        do {
                            h += EditorGUI.GetPropertyHeight(iterBlueprint, includeChildren: true);
                        } while (iterBlueprint.NextVisible(false) == true);

                    }

                    var iter = so.GetIterator();
                    iter.NextVisible(true);
                    iter.NextVisible(false);
                    do {
                        h += EditorGUI.GetPropertyHeight(iter, includeChildren: true);
                    } while (iter.NextVisible(false) == true);

                }

                return h;
                
            }, (r) => {

                if (editor != null) {

                    editor.OnGUI(r, so);

                } else {

                    if (this.container != null) {

                        this.blueprintSerialized.Update();

                        var iterBlueprint = this.blueprintSerialized.GetIterator();
                        iterBlueprint.NextVisible(true);
                        iterBlueprint.NextVisible(false);
                        do {
                            var h = EditorGUI.GetPropertyHeight(iterBlueprint, includeChildren: true);
                            r.height = h;
                            EditorGUI.PropertyField(r, iterBlueprint, includeChildren: true);
                            r.y += h;
                        } while (iterBlueprint.NextVisible(false) == true);

                        this.blueprintSerialized.ApplyModifiedProperties();

                    }

                    so.Update();

                    var iter = so.GetIterator();
                    iter.NextVisible(true);
                    iter.NextVisible(false);
                    do {
                        var h = EditorGUI.GetPropertyHeight(iter, includeChildren: true);
                        r.height = h;
                        EditorGUI.PropertyField(r, iter, includeChildren: true);
                        r.y += h;
                    } while (iter.NextVisible(false) == true);

                    so.ApplyModifiedProperties();

                }

            });

        }

        private void DrawExitNode(Vector2 offset) {

            if (this.container != null) return;
            if (this.blueprint.outputItem.box == null) return;

            var style = new GUIStyle("Box");
            var tex = Resources.Load<Texture2D>("BlackBox/BoxExit");
            style.normal.background = tex;
            style.normal.scaledBackgrounds = new Texture2D[] { tex };

            var styleHeader = new GUIStyle("Box");
            styleHeader.fontStyle = FontStyle.Bold;
            var texHeader = Resources.Load<Texture2D>("BlackBox/BoxExitHeader");
            styleHeader.normal.background = texHeader;
            styleHeader.normal.scaledBackgrounds = new Texture2D[] { texHeader };

            var so = this.outputSerialized;
            var blueprint = this.blueprintSerialized;
            var boxObj = (so.targetObject as Box);
            var width = boxObj.width;
            var padding = boxObj.padding;
            var item = blueprint.FindProperty("outputItem");
            var position = item.FindPropertyRelative("position");
            var rectProp = item.FindPropertyRelative("rect");
            var pos = position.vector2Value;
            var editor = this.GetEditor(so.targetObject.GetType());
            var rect = this.DrawNode(styleHeader, style, false, padding, so, true, new Rect(pos.x + offset.x, pos.y + offset.y, width, 200f), "Output", (r) => {

                var h = 0f;
                if (editor != null) {

                    h = editor.GetHeight(so);

                } else {

                    if (this.container != null) {

                        var iterBlueprint = this.blueprintSerialized.GetIterator();
                        iterBlueprint.NextVisible(true);
                        iterBlueprint.NextVisible(false);
                        do {
                            h += EditorGUI.GetPropertyHeight(iterBlueprint, includeChildren: true);
                        } while (iterBlueprint.NextVisible(false) == true);

                    }

                    var iter = so.GetIterator();
                    iter.NextVisible(true);
                    iter.NextVisible(false);
                    do {
                        h += EditorGUI.GetPropertyHeight(iter, includeChildren: true);
                    } while (iter.NextVisible(false) == true);

                }

                return h;
                
            }, (r) => {

                if (editor != null) {

                    editor.OnGUI(r, so);

                } else {

                    if (this.container != null) {

                        this.blueprintSerialized.Update();

                        var iterBlueprint = this.blueprintSerialized.GetIterator();
                        iterBlueprint.NextVisible(true);
                        iterBlueprint.NextVisible(false);
                        do {
                            var h = EditorGUI.GetPropertyHeight(iterBlueprint, includeChildren: true);
                            r.height = h;
                            EditorGUI.PropertyField(r, iterBlueprint, includeChildren: true);
                            r.y += h;
                        } while (iterBlueprint.NextVisible(false) == true);

                        this.blueprintSerialized.ApplyModifiedProperties();

                    }

                    so.Update();

                    var iter = so.GetIterator();
                    iter.NextVisible(true);
                    iter.NextVisible(false);
                    do {
                        var h = EditorGUI.GetPropertyHeight(iter, includeChildren: true);
                        r.height = h;
                        EditorGUI.PropertyField(r, iter, includeChildren: true);
                        r.y += h;
                    } while (iter.NextVisible(false) == true);

                    so.ApplyModifiedProperties();

                }

            });

            if (rectProp.rectValue != rect) {

                this.blueprintSerialized.Update();
                rectProp.rectValue = rect;
                this.blueprintSerialized.ApplyModifiedProperties();

            }

        }

        private void DrawGraph() {

            var offset = this.scrollPosition;

            this.DrawRootNode(offset);
            this.DrawExitNode(offset);
            
            var boxes = this.blueprintSerialized.FindProperty("boxes");
            for (int i = 0; i < boxes.arraySize; ++i) {
                
                var item = boxes.GetArrayElementAtIndex(i);
                var position = item.FindPropertyRelative("position");
                var rectProp = item.FindPropertyRelative("rect");
                var box = item.FindPropertyRelative("box");
                if (box.objectReferenceValue == null) continue;
                var so = new SerializedObject(box.objectReferenceValue);
                var boxObj = (box.objectReferenceValue as Box);
                var customEditor = this.GetEditor(boxObj.GetType());
                
                var width = boxObj.width;
                var padding = boxObj.padding;
                var pos = position.vector2Value;
                var rect = this.DrawNode(false, padding, so, true, new Rect(pos.x + offset.x, pos.y + offset.y, width, 200f), box.objectReferenceValue.name, (r) => {

                    var h = 0f;
                    if (customEditor != null) {
                        
                        h = customEditor.GetHeight(so);
                        
                    } else {

                        var iter = so.GetIterator();
                        iter.NextVisible(true);
                        while (iter.NextVisible(false) == true) {
                            h += EditorGUI.GetPropertyHeight(iter, includeChildren: true);
                        }

                    }

                    return h;

                }, (r) => {

                    so.Update();
                    
                    if (customEditor != null) {
                        
                        customEditor.OnGUI(r, so);
                        
                    } else {

                        var iter = so.GetIterator();
                        iter.NextVisible(true);
                        while (iter.NextVisible(false) == true) {
                            var h = EditorGUI.GetPropertyHeight(iter, includeChildren: true);
                            r.height = h;
                            EditorGUI.PropertyField(r, iter, includeChildren: true);
                            r.y += h;
                        }

                    }

                    so.ApplyModifiedProperties();
                    
                });

                if (rectProp.rectValue != rect) {

                    this.blueprintSerialized.Update();
                    rectProp.rectValue = rect;
                    this.blueprintSerialized.ApplyModifiedProperties();

                }

            }

        }

        private Vector2 SnapToGrid(Vector2 pos) {
            
            return new Vector2(Mathf.Round(pos.x / GRID_SIZE) * GRID_SIZE, Mathf.Round(pos.y / GRID_SIZE) * GRID_SIZE);
            
        }

        private Rect DrawNode(bool isRoot, float padding, SerializedObject so, bool drawLinkIn, Rect rect, string caption, System.Func<Rect, float> getHeight, System.Action<Rect> onDraw) {

            var style = new GUIStyle("Box");
            var tex = (isRoot == true ? Resources.Load<Texture2D>("BlackBox/BoxRoot") : Resources.Load<Texture2D>("BlackBox/Box"));
            style.normal.background = tex;
            style.normal.scaledBackgrounds = new Texture2D[] { tex };

            var styleHeader = new GUIStyle("Box");
            styleHeader.fontStyle = FontStyle.Bold;
            var texHeader = (isRoot == true ? Resources.Load<Texture2D>("BlackBox/BoxRootHeader") : Resources.Load<Texture2D>("BlackBox/BoxHeader"));
            styleHeader.normal.background = texHeader;
            styleHeader.normal.scaledBackgrounds = new Texture2D[] { texHeader };
            //style.border = new RectOffset(6, 6, 6, 6);

            return this.DrawNode(styleHeader, style, isRoot, padding, so, drawLinkIn, rect, caption, getHeight, onDraw);

        }
        
        private Rect DrawNode(GUIStyle headerStyle, GUIStyle bodyStyle, bool avoidDraggable, float padding, SerializedObject so, bool drawLinkIn, Rect rect, string caption, System.Func<Rect, float> getHeight, System.Action<Rect> onDraw) {

            var rectHeader = rect;
            rectHeader.height = 26f;
            var height = getHeight.Invoke(rect);
            rect.height = height + padding * 2f + rectHeader.height;
            
            var drawSelection = false;
            if (this.dragBeginHeader == true) {
                
                var box = so.targetObject as Box;
                if (box == this.dragBeginSo) {

                    drawSelection = true;
                    
                }
                
            }

            drawSelection |= this.selected.Contains(so.targetObject);
            if (drawSelection == true) {
                
                var styleHighlight = new GUIStyle("Box");
                var texHighlight = Resources.Load<Texture2D>("BlackBox/BoxHighlight");
                styleHighlight.normal.background = texHighlight;
                styleHighlight.normal.scaledBackgrounds = new Texture2D[] { texHighlight };

                var sizeDelta = new Vector2(8f, 8f);
                GUI.Box(new Rect(this.SnapToGrid(rect.position - this.scrollPosition) + this.scrollPosition - sizeDelta * 0.5f, rect.size + sizeDelta), string.Empty, styleHighlight);

            }

            GUI.Box(rect, string.Empty, bodyStyle);
            EditorGUI.LabelField(rectHeader, caption, headerStyle);

            if (drawLinkIn == true) this.DrawLink(so, rectHeader.x, rectHeader.y, drawIn: true);
            
            onDraw.Invoke(new Rect(rect.x + padding, rect.y + padding + rectHeader.height, rect.width - padding * 2f, rect.height));

            if (avoidDraggable == false) {

                if (so != null &&
                    rectHeader.Contains(Event.current.mousePosition) == true &&
                    Event.current.type == EventType.MouseDown &&
                    Event.current.button == 0) {

                    this.dragBeginHeader = true;
                    var box = so.targetObject as Box;
                    this.dragBeginSo = box;

                    Event.current.Use();

                } else if (this.dragBeginHeader == true &&
                           Event.current.type == EventType.MouseDrag) {

                    var box = so.targetObject as Box;
                    if (box == this.dragBeginSo) {

                        var delta = Event.current.delta;
                        if (this.selected.Count > 0) {

                            foreach (var selection in this.selected) {

                                ref var r = ref this.blueprint.GetItem(selection);
                                r.position += delta;
                                r.rect.position += delta;

                            }
                            
                        } else {

                            this.blueprint.UpdatePosition(box, rect.x - this.scrollPosition.x + delta.x, rect.y - this.scrollPosition.y + delta.y);
                            
                        }

                        EditorUtility.SetDirty(this.blueprint);
                        Event.current.Use();

                    }

                } else if (this.dragBeginHeader == true &&
                           Event.current.type == EventType.MouseUp) {

                    var box = so.targetObject as Box;
                    if (box == this.dragBeginSo) {

                        if (this.selected.Count > 0) {

                            foreach (var selection in this.selected) {
                                
                                ref var r = ref this.blueprint.GetItem(selection);
                                r.position = this.SnapToGrid(r.position);
                                r.rect.position = r.position;
                                Event.current.Use();

                            }

                        } else {

                            var p = new Vector2(rect.x - this.scrollPosition.x, rect.y - this.scrollPosition.y);
                            p = this.SnapToGrid(p);
                            if (p != this.blueprint.GetItem(box).position) {

                                this.blueprint.UpdatePosition(box, p.x, p.y);
                                Event.current.Use();

                            }

                        }

                        EditorUtility.SetDirty(this.blueprint);
                        this.dragBeginHeader = false;

                    }

                }

                if (this.dragBeginHeader == false &&
                    rectHeader.Contains(Event.current.mousePosition) == true &&
                    Event.current.type == EventType.MouseUp &&
                    Event.current.button == 1) {

                    this.DrawNodeContextMenu(so);
                    Event.current.Use();
                    
                }

            }
            
            return rect;

        }

        private bool dragBeginHeader;
        private bool dragBeginConnection;
        private Box dragBeginSo;

        private class CurrentPair {

            public LinkParameters linkParameters;
            public Vector2 fromPos;
            public string from;
            public SerializedObject fromSo;
            public SerializedObject to;
            public bool closed;

        }
        private static CurrentPair currentPair;

        private void DrawNodeContextMenu(SerializedObject box) {
            
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Select Node"), false, () => {

                var boxObj = box.targetObject as Box;
                EditorGUIUtility.PingObject(boxObj);
                
            });
            menu.AddItem(new GUIContent("Select Script"), false, () => {

                var boxObj = box.targetObject as Box;
                var so = new SerializedObject(boxObj);
                var iter = so.GetIterator();
                iter.NextVisible(true);
                var txt = iter.objectReferenceValue;
                EditorGUIUtility.PingObject(txt);
                
            });
            menu.AddItem(new GUIContent("Delete"), false, () => {

                var boxObj = box.targetObject as Box;
                this.blueprint.Remove(boxObj);
                AssetDatabase.RemoveObjectFromAsset(boxObj);
                EditorUtility.SetDirty(this.blueprint);

            });
            menu.ShowAsContext();
            
        }
        
        private void DrawCurrentConnection() {

            if (BlackBoxContainerEditor.currentPair != null) {

                var fromPos = BlackBoxContainerEditor.currentPair.fromPos;
                var toPos = (Event.current.mousePosition).XY();
                this.DrawConnection(fromPos, toPos, BlackBoxContainerEditor.currentPair.linkParameters);
                
            }
            
        }

        public struct LinkParameters {

            public bool inLink;
            public bool isWrong;
            public System.Type boxType;

        }
        
        public void DrawLink(object prop, float x, float y, bool drawIn, LinkParameters linkParameters = default) {

            var rect = new Rect(x, y, 18f, 18f);
            var linkIcon = (drawIn == true ? Resources.Load<Texture2D>("BlackBox/BoxLink-In") : Resources.Load<Texture2D>("BlackBox/BoxLink-Out"));
            GUI.DrawTexture(rect, linkIcon);
            var halfOffset = new Vector2(rect.width * 0.5f, rect.height * 0.5f);
            var fromPos = new Vector3(x + halfOffset.x, y + halfOffset.y, 0f);

            if (prop is SerializedProperty property) {

                var to = property.objectReferenceValue as Box;
                if (to != null && to == true) {

                    var boxInfo = this.blueprint.GetItem(to);
                    if (boxInfo.box != null) {

                        var toPos = (boxInfo.position + this.scrollPosition + halfOffset).XY();
                        this.DrawConnection(fromPos, toPos, linkParameters);

                    }

                }

            }

            if (prop is SerializedProperty &&
                rect.Contains(Event.current.mousePosition) == true &&
                Event.current.type == EventType.MouseDown &&
                this.dragBeginHeader == false &&
                drawIn == false) {
                
                this.dragBeginConnection = true;
                
                if (BlackBoxContainerEditor.currentPair == null) {

                    BlackBoxContainerEditor.currentPair = new CurrentPair {
                        linkParameters = linkParameters,
                        @from = (prop as SerializedProperty).propertyPath,
                        fromSo = (prop as SerializedProperty).serializedObject,
                        fromPos = fromPos,
                    };

                }
                
                Event.current.Use();

            } else if (prop is SerializedProperty &&
                Event.current.type == EventType.MouseDrag &&
                this.dragBeginConnection == true) {

                Event.current.Use();

            } else if (prop is SerializedObject &&
                       rect.Contains(Event.current.mousePosition) == true &&
                       Event.current.type == EventType.MouseUp &&
                       this.dragBeginConnection == true) {

                var isUsed = false;
                var pair = BlackBoxContainerEditor.currentPair;
                if (pair != null) {

                    var obj = prop as SerializedObject;
                    var type = obj.targetObject.GetType();
                    if ((linkParameters.boxType == null) ||
                        (type.IsAssignableFrom(linkParameters.boxType) == true)) {

                        pair.to = obj;
                        pair.closed = true;
                        isUsed = true;

                    }

                }

                if (isUsed == true) {

                    this.dragBeginConnection = false;
                    Event.current.Use();

                }

            }
            
        }

        private Vector3 GetTangent(Vector3 from, Vector3 to) {

            if (Mathf.Abs(from.x - to.x) > Mathf.Abs(from.y - to.y)) {

                to.y = from.y;

            } else {
                
                to.x = from.x;

            }
            
            return from + (to - from) * 0.5f;
            
        }
        
        private void DrawContainerChooser() {

            EditorGUI.LabelField(this.localRect, "Double-click on any Container or Blueprint", EditorStyles.centeredGreyMiniLabel);
            
            /*
            this.SetContainer(EditorGUILayout.ObjectField(new UnityEngine.GUIContent("Container"), this.container, typeof(Container), allowSceneObjects: false) as Container);
            this.SetBlueprint(EditorGUILayout.ObjectField(new UnityEngine.GUIContent("Blueprint"), this.blueprint, typeof(Blueprint), allowSceneObjects: false) as Blueprint);
            */
            
        }

        private void SetContainer(Container container) {

            this.container = container;
            if (this.container != null) {
                this.containerSerialized = new SerializedObject(this.container);
            } else {
                this.containerSerialized = null;
                this.blueprint = null;
                this.blueprintSerialized = null;
            }
            if (container != null && container.blueprint == null) {

                var blueprint = Blueprint.CreateInstance<Blueprint>();
                blueprint.name = "MainBlueprint";
                AssetDatabase.AddObjectToAsset(blueprint, container);
                container.blueprint = blueprint;
                EditorUtility.SetDirty(container);

            }
            if (container != null) this.SetBlueprint(container.blueprint);

        }

        private void SetBlueprint(Blueprint container) {

            this.blueprint = container;
            if (this.blueprint != null) {
                
                this.blueprintSerialized = new SerializedObject(this.blueprint);

                if (this.container == null) {

                    if (this.blueprint.outputItem.box == null) {

                        var output = Blueprint.CreateInstance<OutputVariable>();
                        output.name = "Output";
                        AssetDatabase.AddObjectToAsset(output, this.blueprint);
                        this.blueprint.outputItem = new Blueprint.Item() {
                            box = output,
                            position = new Vector2(100f, 100f),
                            rect = new Rect(300f, 0f, 200f, 200f),
                        };
                        EditorUtility.SetDirty(this.blueprint);

                    }

                }

                if (this.blueprint.outputItem.box != null) this.outputSerialized = new SerializedObject(this.blueprint.outputItem.box);

            }
            
        }

        private void Pair(string from, SerializedObject fromSo, SerializedObject to) {

            if (to.targetObject == fromSo.targetObject) {
                
                this.ShowNotification(new GUIContent("Couldn't pair object to itself"));
                return;
                
            }
            var box = to.targetObject as Box;

            fromSo.Update();
            var prop = fromSo.FindProperty(from);
            prop.objectReferenceValue = box;
            fromSo.ApplyModifiedProperties();

        }

        private void UnPair(string from, SerializedObject fromSo) {

            fromSo.Update();
            var prop = fromSo.FindProperty(from);
            prop.objectReferenceValue = null;
            fromSo.ApplyModifiedProperties();

        }

        private void ProcessEvents(Event e) {

            if (BlackBoxContainerEditor.currentPair != null &&
                BlackBoxContainerEditor.currentPair.closed == true) {
                
                var pair = BlackBoxContainerEditor.currentPair;
                this.Pair(pair.@from, pair.fromSo, pair.to);
                BlackBoxContainerEditor.currentPair = null;
                this.dragBeginConnection = false;

            }
            
            switch (e.type) {

                case EventType.MouseDown:
                    
                    var hasAnySelection = false;
                    foreach (var selected in this.selected) {

                        var item = this.blueprint.GetItem(selected);
                        if (item.rect.Contains(e.mousePosition) == true) {

                            hasAnySelection = true;
                            break;

                        }

                    }
                    
                    if (hasAnySelection == false) this.selected.Clear();

                    this.CreateSelectionBox(e.mousePosition);
                    break;

                case EventType.MouseUp:

                    this.RemoveSelectionBox();
                    
                    if (BlackBoxContainerEditor.currentPair != null &&
                        BlackBoxContainerEditor.currentPair.closed == false) {

                        var pair = BlackBoxContainerEditor.currentPair;
                        this.UnPair(pair.@from, pair.fromSo);
                        BlackBoxContainerEditor.currentPair = null;
                        this.dragBeginConnection = false;

                    }

                    if (e.button == 1) {

                        this.DrawContextMenuAt(e.mousePosition);

                    }
                    break;
                
                case EventType.MouseDrag:
                    if ((e.button == 0 && e.alt == true) || e.button == 2) {
                        this.OnDrag(e.delta);
                    } else if (e.button == 0 && e.alt == false) {
                        this.UpdateSelectionBox(e.mousePosition);
                    }
                    break;
                
                case EventType.ScrollWheel:
                    this.OnZoom(e.delta);
                    break;
                
            }
            
        }

        private void DrawSelectionBox() {

            if (this.selectionBox != null) {

                var point = this.selectionBox.point;
                var size = this.selectionBox.size;
                if (size.x < 0f) {

                    point.x += size.x;
                    size.x = -size.x;

                }
                if (size.y < 0f) {

                    point.y += size.y;
                    size.y = -size.y;

                }

                var rect = new Rect(point, size);

                var color = Color.white;
                const float borderSize = 1f;
                using (new GUILayoutExt.GUIAlphaUsing(0.1f)) {

                    EditorGUI.DrawRect(rect, color);
                    EditorGUI.DrawRect(new Rect(point + new Vector2(borderSize, 0f), new Vector2(size.x - borderSize * 2f, borderSize)), color);
                    EditorGUI.DrawRect(new Rect(point, new Vector2(borderSize, size.y)), color);
                    EditorGUI.DrawRect(new Rect(point + new Vector2(borderSize, size.y - borderSize), new Vector2(size.x - borderSize * 2f, borderSize)), color);
                    EditorGUI.DrawRect(new Rect(point + new Vector2(size.x - borderSize, 0f), new Vector2(borderSize, size.y)), color);

                }

                {

                    rect = rect.ScaleSizeBy(1f / this.zoom, this.localRect.TopLeft());
                    this.selected.Clear();
                    foreach (var item in this.blueprint.boxes) {

                        if (rect.Overlaps(item.rect) == true) {

                            this.selected.Add(item.box);

                        }
                        
                    }

                }

            }
            
        }

        private readonly System.Collections.Generic.HashSet<Box> selected = new System.Collections.Generic.HashSet<Box>();
        
        private class SelectionBox {

            public Vector2 point;
            public Vector2 size;

        }

        private SelectionBox selectionBox;

        private void RemoveSelectionBox() {
            
            this.selectionBox = null;
            
        }
        
        private void CreateSelectionBox(Vector2 position) {
            
            if (this.selectionBox == null) this.selectionBox = new SelectionBox();
            this.selectionBox.point = position;

        }

        private void UpdateSelectionBox(Vector2 position) {
            
            if (this.selectionBox == null) this.selectionBox = new SelectionBox();
            this.selectionBox.size = position - this.selectionBox.point;

        }

        private struct Connection {

            public LinkParameters linkParameters;
            public Vector2 from;
            public Vector2 to;

        }
        
        private System.Collections.Generic.List<Connection> connections = new System.Collections.Generic.List<Connection>();

        private void DrawConnection(Vector2 fromPos, Vector2 toPos, LinkParameters linkParameters = default) {

            this.connections.Add(new Connection() {
                linkParameters = linkParameters,
                from = fromPos,
                to = toPos,
            });
            
        }

        private void DrawConnections() {

            var color = new Color(0.6f, 0.6f, 1f, 1f);
            var wrongColor = new Color(1f, 0.6f, 0.6f, 1f);
            var shadowColor = Color.black;
            const float arrowAngle = 20f;
            
            foreach (var item in this.connections) {

                var fromPos = item.@from.XY();
                var toPos = item.to.XY();

                var isDotted = false;
                if (item.linkParameters.inLink == true) {

                    var from = fromPos;
                    fromPos = toPos;
                    toPos = from;
                    isDotted = true;

                }

                var connectionColor = color;
                if (item.linkParameters.isWrong == true) {

                    connectionColor = wrongColor;

                }

                var toArrow = this.GetTangent(toPos, fromPos) - toPos;
                if (isDotted == true) {

                    toArrow = fromPos - toPos;
                    using (new GUILayoutExt.HandlesColorUsing(shadowColor)) {

                        Handles.DrawDottedLine(fromPos, toPos, 4f);

                    }

                } else {

                    Handles.DrawBezier(fromPos,
                                       toPos,
                                       this.GetTangent(fromPos, toPos),
                                       this.GetTangent(toPos, fromPos),
                                       shadowColor,
                                       Texture2D.whiteTexture,
                                       4f);

                }

                using (new GUILayoutExt.HandlesColorUsing(shadowColor)) {
                    
                    Handles.DrawSolidDisc(fromPos, Vector3.back, 4f);
                    
                    Handles.DrawSolidDisc(toPos, Vector3.back, 4f);
                    
                    Handles.DrawSolidArc(toPos, Vector3.back, toArrow, arrowAngle * 0.5f, 16f);
                    Handles.DrawSolidArc(toPos, Vector3.back, toArrow, -arrowAngle * 0.5f, 16f);
                    
                }

                if (isDotted == true) {

                    using (new GUILayoutExt.HandlesColorUsing(connectionColor)) {

                        Handles.DrawDottedLine(fromPos, toPos, 2f);

                    }

                } else {

                    Handles.DrawBezier(fromPos,
                                       toPos,
                                       this.GetTangent(fromPos, toPos),
                                       this.GetTangent(toPos, fromPos),
                                       connectionColor,
                                       Texture2D.whiteTexture,
                                       2f);

                }

                using (new GUILayoutExt.HandlesColorUsing(connectionColor)) {
                    
                    Handles.DrawSolidDisc(fromPos, Vector3.back, 3f);
                    
                    Handles.DrawSolidDisc(toPos, Vector3.back, 3f);
                    
                    Handles.DrawSolidArc(toPos, Vector3.back, toArrow, arrowAngle, 14f);
                    Handles.DrawSolidArc(toPos, Vector3.back, toArrow, -arrowAngle, 14f);
                    
                }
                
            }

        }

        private void DrawContextMenuAt(Vector2 position) {
            
            var menu = new GenericMenu();
            var types = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.IsAbstract == false && typeof(Box).IsAssignableFrom(y))).ToArray();
            foreach (var type in types) {

                if (type.GetCustomAttributes(typeof(HideInMenuAttribute), true).Length > 0) continue;
                
                var categoryPath = string.Empty;
                var category = type.GetCustomAttributes(typeof(CategoryAttribute), true);
                if (category.Length == 1) {

                    categoryPath = ((CategoryAttribute)category[0]).path + "/";

                }

                var name = GUILayoutExt.GetStringCamelCaseSpace(type.Name);
                menu.AddItem(new GUIContent(categoryPath + name), false, () => {

                    var instance = ScriptableObject.CreateInstance(type);
                    instance.name = name;
                    AssetDatabase.AddObjectToAsset(instance, this.blueprint);
                    this.blueprint.Add((Box)instance, this.SnapToGrid(position / this.zoom - this.scrollPosition));
                    EditorUtility.SetDirty(this.blueprint);

                });
                
            }

            menu.ShowAsContext();
            
        }

        private void OnZoom(Vector2 delta) {

            Vector2 screenCoordsMousePos = Event.current.mousePosition;
            Vector2 zoomCoordsMousePos = this.ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
            float zoomDelta = -delta.y / 150.0f;
            float oldZoom = this.zoom;
            this.zoom += zoomDelta;
            this.zoom = Mathf.Clamp(this.zoom, 0.2f, 1f);
            this.scrollPosition -= (zoomCoordsMousePos - this.scrollPosition) - (oldZoom / this.zoom) * (zoomCoordsMousePos - this.scrollPosition);

        }
        
        private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords) {

            return (screenCoords - this.localRect.TopLeft()) / this.zoom + this.scrollPosition;
            
        }
        
        private void OnDrag(Vector2 delta) {

            this.scrollPosition += delta / this.zoom;

        }

        private void DrawBackground() {
            
            //GUI.DrawTexture(this.localRect, Styles.background.normal.background);
            var style = new GUIStyle("CurveEditorBackground");
            GUI.Box(this.localRect, string.Empty, style);

        }
        
        private void DrawGrid() {
            
            this.DrawGrid(this.localRect, GRID_SIZE, 0.2f, Styles.gridColor);
            this.DrawGrid(this.localRect, GRID_SIZE * 5f, 0.4f, Styles.gridColor);
            
        }

        private void DrawGrid(Rect rect, float gridSpacing, float gridOpacity, Color gridColor) {

            var widthDivs = Mathf.CeilToInt(rect.width / gridSpacing / this.zoom);
            var heightDivs = Mathf.CeilToInt(rect.height / gridSpacing / this.zoom);

            Handles.BeginGUI();
            var oldColor = Handles.color;
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            var newOffset = new Vector3((this.scrollPosition.x + rect.x) % gridSpacing, (this.scrollPosition.y + rect.y) % gridSpacing, 0);

            for (var i = 0; i < widthDivs; ++i) {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, rect.height / this.zoom, 0f) + newOffset);
            }

            for (var j = 0; j < heightDivs; ++j) {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(rect.width / this.zoom, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = oldColor;
            Handles.EndGUI();
            
        }

    }

}
