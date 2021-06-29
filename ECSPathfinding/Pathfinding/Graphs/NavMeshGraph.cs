namespace ME.ECS.Pathfinding {
    
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;

    public class NavMeshGraph : Graph {

        public UnityEngine.Vector3Int size = new Vector3Int(100, 1, 100);
        public Vector3 scale = Vector3.one;
        public bool buildFloor;

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
        
        private List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();
        private Dictionary<Entity, NavMeshBuildSource> obstacleSources = new Dictionary<Entity, NavMeshBuildSource>(1000);
        private List<NavMeshBuildSource> tempSources = new List<NavMeshBuildSource>(1000);

        public void AddBuildSource(in NavMeshBuildSource buildSource) {
            
            this.buildSources.Add(buildSource);
        
        }

        public void RemoveObstacle(Entity entity) {
            
            this.obstacleSources.Remove(entity);
            
        }

        public void AddObstacle(Entity entity, Vector3 position, Quaternion rotation, NavMeshBuildSourceShape shape, Vector3 size) {
            
            this.obstacleSources.Add(entity, new NavMeshBuildSource() {
                shape = shape,
                size = size,
                area = 1 << 1,
                transform = Matrix4x4.TRS(position, rotation, Vector3.one),
            });

        }

        public void UpdateGraph() {

            this.UpdateGraph(null);

        }

        public bool UpdateGraph(List<NavMeshBuildSource> sources) {
            
            this.tempSources.Clear();
            if (sources != null) this.tempSources.AddRange(sources);
            this.tempSources.AddRange(this.buildSources);
            foreach (var kv in this.obstacleSources) {

                if (kv.Key.IsAlive() == true) {
                    
                    this.tempSources.Add(kv.Value);
                    
                }
                
            }
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
                    size = new Vector3(this.size.x * this.scale.x, 0f, this.size.z * this.scale.z),
                    transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one),
                });

            }

        }

        private void EndBuild() {
            
            NavMesh.RemoveAllNavMeshData();
            
            var bounds = new UnityEngine.Bounds(this.graphCenter, new Vector3(this.size.x * this.scale.x, this.size.y * this.scale.y, this.size.z * this.scale.z));
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
            
        }

        public override bool ClampPosition(Vector3 worldPosition, Constraint constraint, out Vector3 position) {

            if (UnityEngine.AI.NavMesh.SamplePosition(worldPosition, out var hit, 1000f, (int)constraint.areaMask) == true) {

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