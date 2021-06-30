#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Pathfinding {

    using ME.ECS.Collections;

    public enum BuildingState : byte {

        NotBuilt = 0,
        Building,
        Built,

    }

    public struct NodeInfo {

        public Vector3 worldPosition;
        public Graph graph;
        public Node node;

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class Graph : MonoBehaviour {

        public LogLevel pathfindingLogLevel;

        public int index;
        public string graphName;

        public Vector3 graphCenter;

        public BuildingState buildingState;
        public ListCopyable<Node> nodes;
        public List<Pathfinding.ModificatorItem> modifiers = new List<Pathfinding.ModificatorItem>();

        public float minPenalty { get; private set; }
        public float maxPenalty { get; private set; }

        public float minHeight { get; private set; }
        public float maxHeight { get; private set; }

        public void SetMinMaxHeight(float min, float max) {
            
            this.minHeight = min;
            this.maxHeight = max;
            
        }
        
        protected virtual void OnRecycle() {

            this.pathfindingLogLevel = default;
            this.index = default;
            this.graphName = default;
            this.graphCenter = default;
            this.buildingState = default;
            this.minPenalty = default;
            this.maxPenalty = default;
            this.minHeight = default;
            this.maxHeight = default;

        }

        public virtual void Recycle() {
            
            this.OnRecycle();

        }

        public void CopyFrom(Graph other) {

            this.pathfindingLogLevel = other.pathfindingLogLevel;
            this.index = other.index;
            this.graphName = other.graphName;
            this.graphCenter = other.graphCenter;
            this.buildingState = other.buildingState;
            this.minPenalty = other.minPenalty;
            this.maxPenalty = other.maxPenalty;
            this.minHeight = other.minHeight;
            this.maxHeight = other.maxHeight;

            this.OnCopyFrom(other);

        }

        public abstract void OnCopyFrom(Graph other);

        public virtual float GetNodeMinDistance() {

            return 1f;

        }

        public virtual T AddNode<T>() where T : Node, new() {

            var node = PoolClass<T>.Spawn();
            node.graph = this;
            node.index = this.nodes.Count;
            this.nodes.Add(node);

            return node;

        }

        public virtual void RemoveNode(ref Node node, bool bruteForceConnections = false) {

            if (bruteForceConnections == true) {

                // Brute force all connections from all nodes and remove them to this node
                for (int i = 0; i < this.nodes.Count; ++i) {

                    var connections = this.nodes[i].GetConnections();
                    for (int j = 0; j < connections.Length; ++j) {

                        var connection = connections[j];
                        if (connection.index == node.index) {

                            connections[j] = Node.Connection.NoConnection;

                        }

                    }

                }

            } else {

                // Remove all connections to this node from neighbours only
                var connections = node.GetConnections();
                for (int i = 0; i < connections.Length; ++i) {

                    var connection = connections[i];
                    if (connection.index >= 0) {

                        var connectedTo = this.nodes[connection.index].GetConnections();
                        for (int j = 0; j < connectedTo.Length; ++j) {

                            if (connectedTo[j].index == node.index) {

                                connectedTo[j] = Node.Connection.NoConnection;

                            }

                        }

                    }

                }

            }

            // Remove node from list
            this.nodes.RemoveAt(node.index);

        }

        public virtual T GetNodeByIndex<T>(int index) where T : Node {

            if (index < 0 || index >= this.nodes.Count) return null;

            return (T)this.nodes[index];

        }

        public virtual void UpdateGraph(GraphUpdateObject graphUpdateObject) {

            var bounds = graphUpdateObject.GetBounds();
            var nodes = PoolListCopyable<Node>.Spawn(10);
            this.GetNodesInBounds(nodes, bounds, Constraint.Empty);
            for (int i = 0, cnt = nodes.Count; i < cnt; ++i) {

                var node = nodes[i];
                if (graphUpdateObject.checkRadius == true) {
                    
                    if ((node.worldPosition - graphUpdateObject.center).sqrMagnitude > graphUpdateObject.radius * graphUpdateObject.radius) continue;
                    
                }
                
                graphUpdateObject.Apply(node);

            }
            PoolListCopyable<Node>.Recycle(ref nodes);
            
        }

        public NodeInfo GetNearest(Vector3 worldPosition) {

            return this.GetNearest(worldPosition, Constraint.Default);

        }

        public abstract NodeInfo GetNearest(Vector3 worldPosition, Constraint constraint);

        public abstract bool ClampPosition(Vector3 worldPosition, Constraint constraint, out Vector3 position);

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public abstract void GetNodesInBounds(ListCopyable<Node> output, Bounds bounds, Constraint constraint);

        private Dictionary<int, Color> areaColors = new Dictionary<int, Color>();

        protected Color GetAreaColor(int area) {

            if (this.areaColors.TryGetValue(area, out var color) == false) {

                color = this.GetSColor();
                color.a = 0.04f;
                this.areaColors.Add(area, color);
                return color;

            } else {

                return this.areaColors[area];

            }

        }

        protected void FloodFillAreas(Node rootNode, int area) {

            var list = PoolQueue<Node>.Spawn(this.nodes.Count);
            list.Enqueue(rootNode);
            while (list.Count > 0) {

                var node = list.Dequeue();

                var connections = node.GetConnections();
                for (int j = 0; j < connections.Length; ++j) {

                    var connection = connections[j];
                    if (connection.index >= 0) {

                        var nb = this.nodes[connection.index];
                        if (nb.area == 0 && nb.walkable == true) {

                            nb.area = area;
                            //this.FloodFillAreas(nb, area);
                            list.Enqueue(nb);

                        }

                    }

                }

            }

            PoolQueue<Node>.Recycle(ref list);

        }

        protected Color GetSColor() {

            var rgb = new Vector3Int();
            rgb[0] = Random.Range(0, 256); // red
            rgb[1] = Random.Range(0, 256); // green
            rgb[2] = Random.Range(0, 256); // blue

            int max, min;
            if (rgb[0] > rgb[1]) {

                max = (rgb[0] > rgb[2]) ? 0 : 2;
                min = (rgb[1] < rgb[2]) ? 1 : 2;

            } else {

                max = (rgb[1] > rgb[2]) ? 1 : 2;
                int notmax = 1 + max % 2;
                min = (rgb[0] < rgb[notmax]) ? 0 : notmax;

            }

            rgb[max] = 255;
            rgb[min] = 0;

            return new Color32((byte)rgb[0], (byte)rgb[1], (byte)rgb[2], 255);

        }

        protected Color GetPenaltyColor(float penalty) {

            var min = this.minPenalty;
            var max = this.maxPenalty;

            var from = new Color(0f, 1f, 0f, 0.05f);
            var to = new Color(1f, 0f, 0f, 0.05f);

            var t = Mathf.Clamp01((penalty - min) / (min == max ? 1f : max - min));
            return Color.Lerp(from, to, t);

        }

        protected Color GetHeightColor(float height) {

            var min = this.minHeight;
            var max = this.maxHeight;

            var from = new Color(0f, 0f, 0f, 0.05f);
            var to = new Color(1f, 1f, 1f, 0.05f);

            var t = Mathf.Clamp01((height - min) / (min == max ? 1f : max - min));
            return Color.Lerp(from, to, t);

        }

        public void DoCleanUp() {
            
            this.OnCleanUp();
            
        }

        protected virtual void OnCleanUp() { }

        public void DoBuild() {

            var pathfindingLogLevel = this.pathfindingLogLevel;

            System.Diagnostics.Stopwatch sw = null;
            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) sw = System.Diagnostics.Stopwatch.StartNew();

            this.buildingState = BuildingState.Building;

            this.minPenalty = float.MaxValue;
            this.maxPenalty = float.MinValue;

            this.minHeight = float.MaxValue;
            this.maxHeight = float.MinValue;

            System.Diagnostics.Stopwatch swBuildNodes = null;
            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swBuildNodes = System.Diagnostics.Stopwatch.StartNew();

            this.Validate();
            this.BuildNodes();

            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swBuildNodes.Stop();

            System.Diagnostics.Stopwatch swBeforeConnections = null;
            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swBeforeConnections = System.Diagnostics.Stopwatch.StartNew();

            this.RunModifiersBeforeConnections();

            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swBeforeConnections.Stop();

            System.Diagnostics.Stopwatch swBuildConnections = null;
            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swBuildConnections = System.Diagnostics.Stopwatch.StartNew();

            for (var i = 0; i < this.nodes.Count; ++i) {

                var p = this.nodes[i].penalty;
                if (p < this.minPenalty) this.minPenalty = p;
                if (p > this.maxPenalty) this.maxPenalty = p;

                var h = this.nodes[i].height;
                if (h < this.minHeight) this.minHeight = h;
                if (h > this.maxHeight) this.maxHeight = h;

            }

            this.BuildConnections();

            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swBuildConnections.Stop();

            System.Diagnostics.Stopwatch swAfterConnections = null;
            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swAfterConnections = System.Diagnostics.Stopwatch.StartNew();

            this.RunModifiersAfterConnections();

            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swAfterConnections.Stop();

            System.Diagnostics.Stopwatch swBuildAreas = null;
            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swBuildAreas = System.Diagnostics.Stopwatch.StartNew();

            this.BuildAreas();

            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) swBuildAreas.Stop();

            this.buildingState = BuildingState.Built;

            if ((pathfindingLogLevel & LogLevel.GraphBuild) != 0) {

                Logger.Log(string.Format(
                               "Graph built {0} nodes in {1}ms:\nBuild Nodes: {2}ms\nBefore Connections: {3}ms\nBuild Connections: {4}ms\nAfter Connections: {5}ms\nBuild Areas: {6}ms",
                               this.nodes.Count, sw.ElapsedMilliseconds, swBuildNodes.ElapsedMilliseconds, swBeforeConnections.ElapsedMilliseconds,
                               swBuildConnections.ElapsedMilliseconds, swAfterConnections.ElapsedMilliseconds, swBuildAreas.ElapsedMilliseconds));

            }

        }

        protected abstract void Validate();
        protected abstract void BuildNodes();
        protected abstract void BuildConnections();
        
        protected virtual void RunModifiersAfterConnections() {

            for (var i = 0; i < this.modifiers.Count; ++i) {

                if (this.modifiers[i].enabled == true) this.modifiers[i].modifier.ApplyAfterConnections(this);

            }

        }

        protected virtual void RunModifiersBeforeConnections() {

            for (var i = 0; i < this.modifiers.Count; ++i) {

                if (this.modifiers[i].enabled == true) this.modifiers[i].modifier.ApplyBeforeConnections(this);

            }

        }

        public virtual void BuildAreas() {

            var area = 0;
            for (int i = 0; i < this.nodes.Count; ++i) {

                this.nodes[i].area = 0;

            }

            for (int i = 0; i < this.nodes.Count; ++i) {

                var node = this.nodes[i];
                if (node.walkable == true && node.area == 0) {

                    var currentArea = ++area;
                    node.area = currentArea;
                    this.FloodFillAreas(node, currentArea);

                }

            }

        }

        public void DoDrawGizmos() {

            this.DrawGizmos();

        }

        protected abstract void DrawGizmos();

    }

}