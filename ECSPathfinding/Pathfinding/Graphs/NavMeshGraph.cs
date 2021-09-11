namespace ME.ECS.Pathfinding {
    
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;

    public class NavMeshGraph : Graph {

        public UnityEngine.Vector3Int size = new Vector3Int(100, 1, 100);
        public Vector3 scale = Vector3.one;
        
        public bool buildFloor;
        public float floorHeight = 0.1f;

        public int agentTypeId = 0;
        public float agentClimb = 0f;
        public float agentSlope = 45f;
        public float agentRadius = 0.5f;
        public float agentHeight = 1f;

        public float minRegionArea = 1f;
        public int tileSize = 1;
        public float voxelSize = 0.17f;

        public bool drawMesh;

        private NavMeshData navMeshData;
        private NavMeshDataInstance navMeshDataInstance;
        private NavMeshBuildSettings buildSettings;
        private UnityEngine.Experimental.AI.NavMeshQuery query;
        
        private List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>(5000);
        private List<NavMeshBuildSource> tempSources = new List<NavMeshBuildSource>(1000);

        public void AddBuildSource(in NavMeshBuildSource buildSource) {
            
            this.buildSources.Add(buildSource);
        
        }

        public override void Recycle() {
            
            base.Recycle();
            
            this.OnCleanUp();
            
        }

        protected override void OnCleanUp() {
            
            base.OnCleanUp();
            
            NavMesh.RemoveAllNavMeshData();
            
            Object.DestroyImmediate(this.navMeshData);
            this.navMeshData = null;
            if (this.navMeshDataInstance.valid == true) this.navMeshDataInstance.Remove();

        }

        public void UpdateGraph() {

            this.UpdateGraph(null);

        }

        public bool UpdateGraph(List<NavMeshBuildSource> sources) {
            
            this.tempSources.Clear();
            this.tempSources.AddRange(this.buildSources);
            if (sources != null) this.tempSources.AddRange(sources);
            var bounds = new UnityEngine.Bounds(this.graphCenter, this.size);
            if (NavMeshBuilder.UpdateNavMeshData(this.navMeshData, this.buildSettings, this.tempSources, bounds) == false) {

                return false;

            }

            return true;

        }

        private void BeginBuild() {
            
            this.buildSources.Clear();

            if (this.buildFloor == true) {

                this.AddBuildSource(new UnityEngine.AI.NavMeshBuildSource() {
                    area = 0,
                    shape = UnityEngine.AI.NavMeshBuildSourceShape.Box,
                    size = new Vector3(this.size.x, 0f, this.size.z),
                    transform = Matrix4x4.TRS(new Vector3(0f, this.floorHeight, 0f), Quaternion.identity, Vector3.one),
                });

            }

        }

        private void EndBuild() {
            
            if (this.navMeshDataInstance.valid == true) this.navMeshDataInstance.Remove();
            
            var bounds = new UnityEngine.Bounds(this.graphCenter, this.size);
            var buildSettings = UnityEngine.AI.NavMesh.GetSettingsByID(this.agentTypeId);
            buildSettings.agentRadius = this.agentRadius;
            buildSettings.agentHeight = this.agentHeight;
            buildSettings.agentTypeID = this.agentTypeId;
            buildSettings.agentClimb = this.agentClimb;
            buildSettings.minRegionArea = this.minRegionArea;
            buildSettings.overrideTileSize = true;
            buildSettings.tileSize = this.tileSize;
            buildSettings.overrideVoxelSize = true;
            buildSettings.voxelSize = this.voxelSize;
            var data = NavMeshBuilder.BuildNavMeshData(buildSettings, this.buildSources, bounds, this.graphCenter, Quaternion.identity);
            this.navMeshData = data;
            this.buildSettings = buildSettings;
            
            this.navMeshDataInstance = UnityEngine.AI.NavMesh.AddNavMeshData(this.navMeshData, this.graphCenter, Quaternion.identity);
            var t = NavMesh.CalculateTriangulation();
            var hash = 0;
            foreach (var vert in t.vertices) {
                hash ^= (int)(vert.x * 1000000f);
            }
            Debug.Log("NAV MESH HASH X: " + hash);

            hash = 0;
            foreach (var vert in t.vertices) {
                hash ^= (int)(vert.y * 1000000f);
            }
            Debug.Log("NAV MESH HASH Y: " + hash);

            hash = 0;
            foreach (var vert in t.vertices) {
                hash ^= (int)(vert.z * 1000000f);
            }
            Debug.Log("NAV MESH HASH Z: " + hash);

            hash = 0;
            foreach (var vert in t.indices) {
                hash ^= (int)(vert);
            }
            Debug.Log("NAV MESH HASH 2: " + hash);

            hash = 0;
            foreach (var vert in t.areas) {
                hash ^= (int)(vert);
            }
            Debug.Log("NAV MESH HASH 3: " + hash);

        }

        public override bool ClampPosition(Vector3 worldPosition, Constraint constraint, out Vector3 position) {

            if (UnityEngine.AI.NavMesh.SamplePosition(worldPosition, out var hit, 1000f, new NavMeshQueryFilter() {
                agentTypeID = this.agentTypeId,
                areaMask = constraint.checkArea == true ? (int)constraint.areaMask : -1,
            }) == true) {

                position = hit.position;
                return true;

            }

            position = default;
            return false;

        }

        protected override void OnRecycle() {
            
            base.OnRecycle();
            
            this.navMeshDataInstance.Remove();
            Object.DestroyImmediate(this.navMeshData);
            
        }

        public override void OnCopyFrom(Graph other) {
            throw new System.NotImplementedException();
        }

        public override NodeInfo GetNearest(UnityEngine.Vector3 worldPosition, Constraint constraint) {

            if (this.ClampPosition(worldPosition, constraint, out var pos) == true) {

                return new NodeInfo() {
                    graph = this,
                    worldPosition = pos,
                };

            }
            
            return new NodeInfo() {
                graph = this,
            };

        }

        public override void GetNodesInBounds(ME.ECS.Collections.ListCopyable<Node> output, UnityEngine.Bounds bounds, Constraint constraint) {
            throw new System.NotImplementedException();
        }

        protected override void Validate() {
        }

        protected override void BuildNodes() {
            
            this.BeginBuild();
            
        }

        protected override void RunModifiersBeforeConnections() {
            
            base.RunModifiersBeforeConnections();
            
        }

        protected override void BuildConnections() {
            
            this.EndBuild();
            
        }

        protected override void RunModifiersAfterConnections() {
            
            base.RunModifiersAfterConnections();
            
        }

        protected override void DrawGizmos() {

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(this.graphCenter, this.size);

        }

    }

}