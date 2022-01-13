using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ME.ECS.Pathfinding.Editor {

    [GraphCustomEditor(typeof(NavMeshGraph))]
    public class NavMeshGraphEditor : GraphEditor, IGraphGUIEditor<Graph> {

        public bool showGrid {
            get => EditorPrefs.GetBool("ME.ECS.Pathfinding.Graph." + this.target.index + ".showGrid", true);
            set => EditorPrefs.SetBool("ME.ECS.Pathfinding.Graph." + this.target.index + ".showGrid", value);
        }

        public bool showAgent {
            get => EditorPrefs.GetBool("ME.ECS.Pathfinding.Graph." + this.target.index + ".showAgent", true);
            set => EditorPrefs.SetBool("ME.ECS.Pathfinding.Graph." + this.target.index + ".showAgent", value);
        }

        public bool showEditor {
            get => EditorPrefs.GetBool("ME.ECS.Pathfinding.Graph." + this.target.index + ".showEditor", true);
            set => EditorPrefs.SetBool("ME.ECS.Pathfinding.Graph." + this.target.index + ".showEditor", value);
        }

        new public NavMeshGraph target => (NavMeshGraph)base.target;

        new public T GetTarget<T>() {
            return (T)(object)this.target;
        }

        public override bool OnDrawGUI() {

            base.OnDrawGUI();

            var state = this.showGrid;
            ME.ECSEditor.GUILayoutExt.FoldOut(ref state, "Graph",
                                              () => {
                                              
                                                  ME.ECSEditor.GUILayoutExt.Box(6f, 2f, () => {
                                                      
                                                      this.target.size = EditorGUILayout.Vector3IntField("Size", this.target.size);
                                                      this.target.scale = EditorGUILayout.Vector3Field("Scale", this.target.scale);
                                                      this.target.buildFloor = EditorGUILayout.Toggle("Build Floor", this.target.buildFloor);
                                                      this.target.floorArea = NavMeshAreaPropertyDrawer.GUILayout("Floor Area", this.target.floorArea);
                                                      this.target.floorHeight = EditorGUILayout.FloatField("Floor Height", this.target.floorHeight);

                                                  });
                                                      
                                              });
            this.showGrid = state;

            state = this.showAgent;
            ME.ECSEditor.GUILayoutExt.FoldOut(ref state, "Agent", () => {
                
                ME.ECSEditor.GUILayoutExt.Box(6f, 2f, () => {

                    const float diagramHeight = 80.0f;
                    var agentDiagramRect = EditorGUILayout.GetControlRect(false, diagramHeight);
                    UnityEditor.AI.NavMeshEditorHelpers.DrawAgentDiagram(agentDiagramRect, this.target.agentRadius, this.target.agentHeight, this.target.agentClimb,
                                                                         this.target.agentSlope);
                    this.target.agentTypeId = NavMeshGraphEditor.AgentTypePopup("Agent Type", this.target.agentTypeId);
                    this.target.agentClimb = EditorGUILayout.FloatField("Climb", this.target.agentClimb);
                    this.target.agentHeight = EditorGUILayout.FloatField("Height", this.target.agentHeight);
                    this.target.agentRadius = EditorGUILayout.FloatField("Radius", this.target.agentRadius);
                    this.target.agentSlope = EditorGUILayout.FloatField("Slope", this.target.agentSlope);
                    
                    this.target.voxelSize = EditorGUILayout.FloatField("Voxel Size", this.target.voxelSize);
                    this.target.tileSize = EditorGUILayout.IntField("Tile Size", this.target.tileSize);
                    this.target.minRegionArea = EditorGUILayout.FloatField("Min Region Area", this.target.minRegionArea);

                });
                
            });
            this.showAgent = state;

            state = this.showEditor;
            ME.ECSEditor.GUILayoutExt.FoldOut(ref state, "Editor", () => {
                
                ME.ECSEditor.GUILayoutExt.Box(6f, 2f, () => {
                    
                    var drawMesh = EditorGUILayout.Toggle("Draw Mesh", this.target.drawMesh);
                    if (drawMesh != this.target.drawMesh) {

                        this.target.drawMesh = drawMesh;
                        if (drawMesh == true) {

                            ++UnityEditor.AI.NavMeshVisualizationSettings.showNavigation;

                        } else {
                            
                            --UnityEditor.AI.NavMeshVisualizationSettings.showNavigation;

                        }

                    }

                });
                
            });
            this.showEditor = state;

            return false;

        }

        private static int AgentTypePopup(string labelName, int agentTypeID) {
            
            var index = -1;
            var count = UnityEngine.AI.NavMesh.GetSettingsCount();
            var agentTypeNames = new string[count + 2];
            for (var i = 0; i < count; i++) {
                var id = UnityEngine.AI.NavMesh.GetSettingsByIndex(i).agentTypeID;
                var name = UnityEngine.AI.NavMesh.GetSettingsNameFromID(id);
                agentTypeNames[i] = name;
                if (id == agentTypeID) {
                    index = i;
                }
            }

            agentTypeNames[count] = "";
            agentTypeNames[count + 1] = "Open Agent Settings...";

            var validAgentType = index != -1;
            if (!validAgentType) {
                EditorGUILayout.HelpBox("Agent Type invalid.", MessageType.Warning);
            }

            var rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            
            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(rect, labelName, index, agentTypeNames);
            if (EditorGUI.EndChangeCheck()) {
                if (index >= 0 && index < count) {
                    var id = UnityEngine.AI.NavMesh.GetSettingsByIndex(index).agentTypeID;
                    agentTypeID = id;
                } else if (index == count + 1) {
                    UnityEditor.AI.NavMeshEditorHelpers.OpenAgentSettings(-1);
                }
            }

            return agentTypeID;

        }

    }

}