﻿namespace ME.ECSEditor {

    using UnityEngine;
    using UnityEditor;
    using ME.ECS;
    
    [UnityEditor.CustomEditor(typeof(ME.ECS.Debug.EntityDebugComponent), true)]
    public class EntityDebugComponentEditor : Editor {

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

        }

    }

}
