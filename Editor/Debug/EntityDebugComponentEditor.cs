using UnityEditor.UIElements;

namespace ME.ECSEditor {

    using UnityEngine;
    using UnityEditor;
    using ME.ECS;
    using UnityEngine.UIElements;
    
    [UnityEditor.CustomEditor(typeof(ME.ECS.Debug.EntityDebugComponent), true)]
    public class EntityDebugComponentEditor : Editor, IEditorContainer {

        public class TempDataObject : MonoBehaviour {

            public ME.ECS.Debug.EntityProxyDebugger.ComponentData[] components;
            #if !SHARED_COMPONENTS_DISABLED
            public ME.ECS.Debug.EntityProxyDebugger.ComponentData[] sharedComponents;
            #endif

        }
        
        public GameObject go;
        public TempDataObject temp;
        private ME.ECS.Debug.EntityProxyDebugger debug;

        private readonly System.Collections.Generic.Dictionary<int, UnityEditor.UIElements.PropertyField> fieldsCacheComponents = new System.Collections.Generic.Dictionary<int, PropertyField>();
        private VisualElement rootElement;
        private VisualElement componentsContainer;
        private VisualElement content;
        private bool componentsDirty;
        
        #if !SHARED_COMPONENTS_DISABLED
        private readonly System.Collections.Generic.Dictionary<int, UnityEditor.UIElements.PropertyField> fieldsCacheSharedComponents = new System.Collections.Generic.Dictionary<int, PropertyField>();
        private bool sharedComponentsDirty;
        private VisualElement sharedComponentsContainer;
        #endif
        
        private VisualElement entityContainer;
        private VisualElement entityContainerWarning;
        private Label entityId;
        private Label entityGen;
        private Label entityVersion;

        public void Save() {
            
        }

        public void OnComponentMenu(GenericMenu menu, int index) {
            
        }

        public void OnEnable() {

            var target = this.target as ME.ECS.Debug.EntityDebugComponent;
            this.go = new GameObject("Temp", typeof(TempDataObject));
            this.go.transform.SetParent(target.transform);

            target.transform.hideFlags = HideFlags.HideInInspector;

            this.temp = this.go.GetComponent<TempDataObject>();

        }

        public void OnDisable() {
            
            GameObject.DestroyImmediate(this.go);
            this.go = null;

        }

        public void Update() {
            
            var target = this.target as ME.ECS.Debug.EntityDebugComponent;
            var entity = this.UpdateEntity(target.entity);
            if (entity != Entity.Empty) {
                
                this.debug.SetEntity(entity);
                this.UpdateComponents();
                #if !SHARED_COMPONENTS_DISABLED
                this.UpdateSharedComponents();
                #endif

                if (this.componentsDirty == true) {
                    this.componentsDirty = false;
                    this.RegisterComponentsCallbacks();
                }
                
                #if !SHARED_COMPONENTS_DISABLED
                if (this.sharedComponentsDirty == true) {
                    this.sharedComponentsDirty = false;
                    this.RegisterSharedComponentsCallbacks();
                }
                #endif
                
            } else {
                
                this.rootElement.Clear();
                var element = new Label("No active world found");
                element.AddToClassList("world-not-found");
                this.rootElement.Add(element);

            }

        }

        private Entity UpdateEntity(Entity entity) {

            var target = this.target as ME.ECS.Debug.EntityDebugComponent;
            if (target.world != null && target.world.isActive == true) {

                var currentEntity = target.world.GetEntityById(entity.id);
                if (currentEntity.IsAlive() == true && currentEntity == entity) {

                    this.entityContainer.RemoveFromClassList("entity-changed");
                    this.entityContainer.AddToClassList("entity-current");
                    this.entityContainerWarning.AddToClassList("hidden");

                    if (this.content.childCount == 0) {

                        this.BuildLists(this.content);
                    
                    }

                } else {
                    
                    this.entityContainer.RemoveFromClassList("entity-current");
                    this.entityContainer.AddToClassList("entity-changed");
                    this.entityContainerWarning.RemoveFromClassList("hidden");
                    
                    this.content.Clear();
                    entity = currentEntity;
                    
                }
                
                this.entityId.text = currentEntity.id.ToString();
                this.entityGen.text = currentEntity.generation.ToString();
                this.entityVersion.text = currentEntity.GetVersion().ToString();
                
            } else {
                
                entity = Entity.Empty;
                
            }

            return entity;

        }

        #if !SHARED_COMPONENTS_DISABLED
        private void UpdateSharedComponents() {
            
            var components = this.debug.GetSharedComponentsList();
            if (components.Length != this.temp.sharedComponents.Length) {

                //this.RebuildSharedComponents(this.sharedComponentsContainer.contentContainer);
                
                if (Selection.activeObject == this.target) {

                    Selection.activeObject = null;
                    Selection.activeObject = this.target;

                }

                return;

            }

            for (int i = 0; i < this.temp.sharedComponents.Length; ++i) {

                if (components[i].dataIndex != this.temp.sharedComponents[i].dataIndex) {

                    //this.RebuildSharedComponents(this.sharedComponentsContainer.contentContainer);
                    
                    if (Selection.activeObject == this.target) {

                        Selection.activeObject = null;
                        Selection.activeObject = this.target;

                    }

                    return;
                    
                }
                
            }

            this.temp.sharedComponents = components;

        }
        #endif

        private void UpdateComponents() {
            
            var components = this.debug.GetComponentsList();
            if (components.Length != this.temp.components.Length) {

                if (Selection.activeObject == this.target) {

                    Selection.activeObject = null;
                    Selection.activeObject = this.target;

                }

                //this.RebuildComponents(this.componentsContainer.contentContainer);
                //this.rootElement = this.CreateInspectorGUI();
                //this.temp.components = components;
                return;

            }

            for (int i = 0; i < this.temp.components.Length; ++i) {

                if (components[i].dataIndex != this.temp.components[i].dataIndex) {

                    if (Selection.activeObject == this.target) {

                        Selection.activeObject = null;
                        Selection.activeObject = this.target;

                    }

                    //this.RebuildComponents(this.componentsContainer.contentContainer);
                    //this.rootElement = this.CreateInspectorGUI();
                    //this.temp.components = components;
                    return;
                    
                }
                
            }
            
            this.temp.components = components;

        }

        private void RegisterComponentsCallbacks() {
            
            foreach (var item in this.fieldsCacheComponents) {
                
                var element = item.Value;
                this.RegisterEvents(item.Key, element, isShared: false);

            }

        }

        #if !SHARED_COMPONENTS_DISABLED
        private void RegisterSharedComponentsCallbacks() {
            
            foreach (var item in this.fieldsCacheSharedComponents) {
                
                var element = item.Value;
                this.RegisterEvents(item.Key, element, isShared: true);

            }

        }
        #endif

        private void RegisterEvents(int index, PropertyField element, bool isShared) {
            
            var build = element.Query().Class(PropertyField.ussClassName).Build();
            build.ForEach(x => {
                    
                var prop = (PropertyField)x;
                prop.userData = index;
                #if !SHARED_COMPONENTS_DISABLED
                if (isShared == true) {
                    prop.UnregisterCallback<SerializedPropertyChangeEvent>(this.OnEventPropertyChangedShared);
                    prop.RegisterCallback<SerializedPropertyChangeEvent>(this.OnEventPropertyChangedShared);
                } else
                #endif
                {
                    prop.UnregisterCallback<SerializedPropertyChangeEvent>(this.OnEventPropertyChanged);
                    prop.RegisterCallback<SerializedPropertyChangeEvent>(this.OnEventPropertyChanged);
                }

            });
            
        }

        private void OnEventPropertyChanged(SerializedPropertyChangeEvent evtProp) {

            var idx = (int)((PropertyField)evtProp.currentTarget).userData;
            var comp = this.temp.components[idx];
            this.debug.SetComponent(comp.dataIndex, comp.data);
            
        }

        #if !SHARED_COMPONENTS_DISABLED
        private void OnEventPropertyChangedShared(SerializedPropertyChangeEvent evtProp) {
            
            var idx = (int)((PropertyField)evtProp.currentTarget).userData;
            var comp = this.temp.components[idx];
            this.debug.SetSharedComponent(comp.dataIndex, comp.data, comp.groupId);
            
        }
        #endif

        public override VisualElement CreateInspectorGUI() {
            
            var container = new VisualElement();
            container.styleSheets.Add(EditorUtilities.Load<StyleSheet>("Editor/Core/DataConfigs/styles.uss", isRequired: true));
            this.rootElement = container;
            
            var target = this.target as ME.ECS.Debug.EntityDebugComponent;
            if (target.world != null && target.world.isActive == true) {

                this.debug = new ME.ECS.Debug.EntityProxyDebugger(target.entity, target.world);
                this.temp.components = this.debug.GetComponentsList();
                #if !SHARED_COMPONENTS_DISABLED
                this.temp.sharedComponents = this.debug.GetSharedComponentsList();
                #endif
                container.schedule.Execute(this.Update).Every((long)(target.world.GetTickTime() * 1000f));
                
                var searchField = new ToolbarSearchField();
                searchField.AddToClassList("search-field");
                searchField.RegisterValueChangedCallback((evt) => {

                    var search = evt.newValue.ToLower();
                    DataConfigEditor.Search(search, this.componentsContainer);
                    #if !SHARED_COMPONENTS_DISABLED
                    DataConfigEditor.Search(search, this.sharedComponentsContainer);
                    #endif
                
                });
                container.Add(searchField);

                {
                    var entityElement = new VisualElement();
                    this.entityContainer = entityElement;
                    entityElement.name = "EntityContainer";
                    entityElement.AddToClassList("entity-container");
                    var id = new Label("ID:");
                    id.AddToClassList("entity-container-item");
                    id.AddToClassList("entity-container-item-label");
                    entityElement.Add(id);
                    var idValue = new Label();
                    idValue.AddToClassList("entity-container-item");
                    idValue.AddToClassList("entity-container-item-value");
                    idValue.name = "EntityId";
                    this.entityId = idValue;
                    entityElement.Add(idValue);
                    var gen = new Label("Generation:");
                    gen.AddToClassList("entity-container-item");
                    gen.AddToClassList("entity-container-item-label");
                    entityElement.Add(gen);
                    var genValue = new Label();
                    genValue.AddToClassList("entity-container-item");
                    genValue.AddToClassList("entity-container-item-value");
                    genValue.name = "EntityGen";
                    this.entityGen = genValue;
                    entityElement.Add(genValue);
                    var version = new Label("Version:");
                    version.AddToClassList("entity-container-item");
                    version.AddToClassList("entity-container-item-label");
                    entityElement.Add(version);
                    var versionValue = new Label();
                    versionValue.AddToClassList("entity-container-item");
                    versionValue.AddToClassList("entity-container-item-value");
                    versionValue.name = "EntityVersion";
                    this.entityVersion = versionValue;
                    entityElement.Add(versionValue);

                    container.Add(entityElement);
                    
                    var changedWarning = new Label("Selected entity is no longer available: generation has been changed.");
                    this.entityContainerWarning = changedWarning;
                    changedWarning.name = "EntityChanged";
                    changedWarning.AddToClassList("entity-changed-warning");
                    changedWarning.AddToClassList("hidden");
                    container.Add(changedWarning);
                    
                }

                this.content = new VisualElement();
                this.content.name = "Content";
                container.Add(this.content);
                
                if (target.entity.IsAlive() == true) {

                    this.BuildLists(this.content);
                    
                }
                
            } else {
                
                var element = new Label("No active world found");
                element.AddToClassList("world-not-found");
                this.rootElement.Add(element);
                
            }

            return this.rootElement;
            
        }

        public void BuildLists(VisualElement container) {
            
            this.componentsContainer = new VisualElement();
            this.RebuildComponents(this.componentsContainer.contentContainer);
            container.Add(this.componentsContainer);

            #if !SHARED_COMPONENTS_DISABLED
            this.sharedComponentsContainer = new VisualElement();
            this.RebuildSharedComponents(this.sharedComponentsContainer.contentContainer);
            container.Add(this.sharedComponentsContainer);
            #endif

        }

        private void RebuildComponents(VisualElement container) {
            
            container.Clear();

            var header = new Label("Components:");
            header.AddToClassList("header");
            container.Add(header);
            
            var element = new VisualElement();
            var so = new SerializedObject(this.temp);
            foreach (var fields in this.fieldsCacheComponents) {
                fields.Value.Unbind();
            }
            var source = so.FindProperty("components");
            this.fieldsCacheComponents.Clear();
            DataConfigEditor.BuildInspectorPropertiesElement("data",
                                                             this,
                                                             null,
                                                             source,
                                                             element,
                                                             noFields: false,
                                                             onBuild: (index, propElement) => {
                                                              
                                                                 this.fieldsCacheComponents.Add(index, propElement);
                                                              
                                                             },
                                                             drawGroups: true);

            /*
            this.fieldsCacheComponents.Clear();
            var prop = so.FindProperty("components");
            for (int i = 0; i < prop.arraySize; ++i) {

                var propField = prop.GetArrayElementAtIndex(i);
                var dataField = propField.FindPropertyRelative("data");
                var field = new PropertyField(dataField);
                field.BindProperty(dataField);
                element.Add(field);
                this.fieldsCacheComponents.Add(i, field);

            }*/
            
            this.componentsDirty = true;
            
            container.Add(element);
            
        }

        #if !SHARED_COMPONENTS_DISABLED
        private void RebuildSharedComponents(VisualElement container) {
            
            container.Clear();

            var header = new Label("Shared Components:");
            header.AddToClassList("header");
            container.Add(header);
            
            var element = new VisualElement();
            var so = new SerializedObject(this.temp);
            var source = so.FindProperty("sharedComponents");
            this.fieldsCacheSharedComponents.Clear();
            DataConfigEditor.BuildInspectorPropertiesElement("data",
                                                             this,
                                                             null,
                                                             source,
                                                             element,
                                                             noFields: false,
                                                             onBuild: (index, propElement) => {
                                                              
                                                                 this.fieldsCacheSharedComponents.Add(index, propElement);
                                                              
                                                             });
            
            this.sharedComponentsDirty = true;
            
            container.Add(element);
            
        }
        #endif

        /*
        private static readonly System.Collections.Generic.Dictionary<World, WorldsViewerEditor.WorldEditor> worldEditors = new System.Collections.Generic.Dictionary<World, WorldsViewerEditor.WorldEditor>();

        private string search;
        
        public override void OnInspectorGUI() {

            var target = this.target as ME.ECS.Debug.EntityDebugComponent;
            if (target.world != null && target.world.isActive == true) {

                var currentEntity = GUILayoutExt.DrawEntitySelection(target.world, in target.entity, checkAlive: true, drawSelectButton: false);
                if (currentEntity.IsAlive() == true && target.entity.IsAlive() == true) {

                    if (EntityDebugComponentEditor.worldEditors.TryGetValue(target.world, out var worldEditor) == false) {

                        worldEditor = new WorldsViewerEditor.WorldEditor();
                        worldEditor.world = target.world;
                        EntityDebugComponentEditor.worldEditors.Add(target.world, worldEditor);

                    }

                    this.search = GUILayoutExt.SearchField("Search", this.search);

                    WorldsViewerEditor.DrawEntity(this.search, target.entity, worldEditor, worldEditor.GetEntitiesStorage(), worldEditor.GetStructComponentsStorage(), worldEditor.GetModules());
                    this.Repaint();

                }
                
            } else {

                if (Worlds.currentWorld == null) {
                    
                    GUILayout.Label("No running worlds found", EditorStyles.centeredGreyMiniLabel);
                    
                } else {

                    GUILayoutExt.DrawAddEntityMenu(target);

                }

            }

        }*/

    }

}
