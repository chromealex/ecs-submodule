using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {
    
    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections.Generic;

    public struct Key<T1, T2> : System.IEquatable<Key<T1, T2>> {

        public T1 k1;
        public T2 k2;
        
        public Key(T1 item1, T2 item2) {
            
            this.k1 = item1;
            this.k2 = item2;
            
        }

        public bool Equals(Key<T1, T2> other) {
            return EqualityComparer<T1>.Default.Equals(this.k1, other.k1) && EqualityComparer<T2>.Default.Equals(this.k2, other.k2);
        }

        public override bool Equals(object obj) {
            return obj is Key<T1, T2> other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return (EqualityComparer<T1>.Default.GetHashCode(this.k1) * 397) ^ EqualityComparer<T2>.Default.GetHashCode(this.k2);
            }
        }

    }
    
    public class NavMeshGraph : Graph {

        public UnityEngine.Vector3Int size = new Vector3Int(100, 1, 100);
        public Vector3 scale = Vector3.one;
        
        public bool buildFloor;
        [NavMeshArea]
        public int floorArea;
        public float floorHeight = 0.1f;

        public int agentTypeId = 0;
        public float agentClimb = 0f;
        public float agentSlope = 45f;
        public float agentRadius = 0.5f;
        public float agentHeight = 1f;

        public float minRegionArea = 1f;
        public int tileSize = 1;
        public float voxelSize = 0.17f;

        private NavMeshBuildSettings buildSettings;

        private NavMeshData navMeshData;
        private NavMeshDataInstance navMeshDataInstance;
        
        [System.NonSerializedAttribute]
        private List<NavMeshBuildSource> buildSources;
        [System.NonSerializedAttribute]
        private ME.ECS.Collections.DictionaryCopyable<Key<Entity, int>, NavMeshBuildSource> buildSourcesEntities;
        [System.NonSerializedAttribute]
        private List<NavMeshBuildSource> tempSources;
        
        public int lastGraphUpdateHash { get; private set;  }

        public bool drawMesh;
        
        public void AddBuildSource(in NavMeshBuildSource buildSource) {
            
            if (this.buildSources == null) this.buildSources = new List<NavMeshBuildSource>(5000);
            this.buildSources.Add(buildSource);
        
        }

        public void AddBuildSource(in Key<Entity, int> entity, in NavMeshBuildSource buildSource) {

            if (this.buildSourcesEntities == null) this.buildSourcesEntities = new ME.ECS.Collections.DictionaryCopyable<Key<Entity, int>, NavMeshBuildSource>(5000);
            this.buildSourcesEntities.Add(entity, buildSource);
        
        }

        public void RemoveBuildSource(in Key<Entity, int> entity) {

            if (this.buildSourcesEntities == null) return;
            this.buildSourcesEntities.Remove(entity);

        }

        public void AddCurrentNavMeshData() {

            if (this.navMeshData != null) {
                
                if (this.navMeshDataInstance.valid == true) this.navMeshDataInstance.Remove();
                this.navMeshDataInstance = UnityEngine.AI.NavMesh.AddNavMeshData(this.navMeshData, Vector3.zero, Quaternion.identity);
                
            }

        }

        public override void Recycle() {
            
            base.Recycle();
            
            this.OnCleanUp();
            
        }

        protected override void OnCleanUp() {
            
            base.OnCleanUp();
            
            //NavMesh.RemoveAllNavMeshData();
            //NavMesh.RemoveNavMeshData(this.navMeshDataInstance);
            Object.DestroyImmediate(this.navMeshData);
            this.navMeshData = null;
            if (this.navMeshDataInstance.valid == true) this.navMeshDataInstance.Remove();

        }
        
        public bool UpdateGraph(Unity.Collections.NativeArray<NavMeshBuildSource> sources) {

            if (this.navMeshData == null) return false;

            var temp = PoolListCopyable<NavMeshBuildSource>.Spawn(sources.Length);
            temp.AddRange(sources);
            var result = this.UpdateGraph(temp);
            PoolListCopyable<NavMeshBuildSource>.Recycle(ref temp);
            return result;

        }

        public bool UpdateGraph(Unity.Collections.NativeArray<NavMeshBuildSource> sources, Unity.Collections.NativeArray<NavMeshBuildSource> sources2) {

            if (this.navMeshData == null) return false;

            var temp = PoolListCopyable<NavMeshBuildSource>.Spawn(sources.Length + sources2.Length);
            temp.AddRange(sources);
            temp.AddRange(sources2);
            var result = this.UpdateGraph(temp);
            PoolListCopyable<NavMeshBuildSource>.Recycle(ref temp);
            return result;

        }

        public void UpdateGraph() {

            this.UpdateGraph((List<NavMeshBuildSource>)null);

        }

        public bool UpdateGraph(List<NavMeshBuildSource> sources) {

            return this.UpdateGraph(sources, new UnityEngine.Bounds((Vector3)this.graphCenter, this.size));
            
        }

        public bool UpdateGraph(List<NavMeshBuildSource> sources, Bounds bounds) {

            if (this.navMeshData == null) return false;
            
            if (this.tempSources == null) this.tempSources = new List<NavMeshBuildSource>(this.buildSources != null ? this.buildSources.Count : 4);
            this.tempSources.Clear();
            if (this.buildSources != null) this.tempSources.AddRange(this.buildSources);
            if (this.buildSourcesEntities != null && this.buildSourcesEntities.Count > 0) {

                foreach (var kv in this.buildSourcesEntities) {
                    
                    this.tempSources.Add(kv.Value);
                    
                }
                
            }
            if (sources != null) this.tempSources.AddRange(sources);
            if (NavMeshBuilder.UpdateNavMeshData(this.navMeshData, this.buildSettings, this.tempSources, bounds) == false) {

                return false;

            }

            return true;

        }

        public bool UpdateGraph(ME.ECS.Collections.ListCopyable<NavMeshBuildSource> sources) {

            return this.UpdateGraph(sources, new UnityEngine.Bounds((Vector3)this.graphCenter, this.size));
            
        }

        public bool UpdateGraph(ME.ECS.Collections.ListCopyable<NavMeshBuildSource> sources, Bounds bounds) {

            if (this.navMeshData == null) return false;
            
            if (this.tempSources == null) this.tempSources = new List<NavMeshBuildSource>(this.buildSources != null ? this.buildSources.Count : 4);
            this.tempSources.Clear();
            if (this.buildSources != null) this.tempSources.AddRange(this.buildSources);
            if (sources != null) this.tempSources.AddRange(sources);


            var hash = 0;
            for (int i = 0; i < this.tempSources.Count; i++) {
                hash ^= this.tempSources[i].transform.GetHashCode();
            }

            this.lastGraphUpdateHash = hash;
            
            if (NavMeshBuilder.UpdateNavMeshData(this.navMeshData, this.buildSettings, this.tempSources, bounds) == false) {

                return false;

            }

            return true;

        }

        private void BeginBuild() {
            
            if (this.buildSources != null) this.buildSources.Clear();

            if (this.buildFloor == true) {

                this.AddBuildSource(new UnityEngine.AI.NavMeshBuildSource() {
                    area = this.floorArea,
                    shape = UnityEngine.AI.NavMeshBuildSourceShape.Box,
                    size = new Vector3(this.size.x, 0f, this.size.z),
                    transform = Matrix4x4.TRS((Vector3)new float3(this.graphCenter.x, this.floorHeight, this.graphCenter.z), Quaternion.identity, Vector3.one),
                });

            }

        }

        private void EndBuild() {
            
            if (this.buildSources == null) return;
            
            var bounds = new UnityEngine.Bounds((Vector3)this.graphCenter, this.size);
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
            var data = NavMeshBuilder.BuildNavMeshData(buildSettings, this.buildSources, bounds, Vector3.zero, Quaternion.identity);
            this.navMeshData = data;
            this.buildSettings = buildSettings;
            
            Debug.Log("EndBuild: " + this.navMeshData);
            
            this.AddCurrentNavMeshData();
            var t = NavMesh.CalculateTriangulation();
            var hash = 0;
            foreach (var vert in t.vertices) {
                hash ^= (int)(vert.x * 1000000f);
            }
            
            foreach (var vert in t.vertices) {
                hash ^= (int)(vert.y * 1000000f);
            }
            
            foreach (var vert in t.vertices) {
                hash ^= (int)(vert.z * 1000000f);
            }
            
            foreach (var vert in t.indices) {
                hash ^= (int)(vert);
            }
            
            foreach (var vert in t.areas) {
                hash ^= (int)(vert);
            }
            
            Debug.Log("NavMeshGraph Hash: " + hash);

        }

        public override bool ClampPosition(float3 worldPosition, Constraint constraint, out float3 position) {

            if (UnityEngine.AI.NavMesh.SamplePosition((Vector3)worldPosition, out var hit, 1000f, new NavMeshQueryFilter() {
                agentTypeID = this.agentTypeId,
                areaMask = constraint.checkArea == true ? (int)constraint.areaMask : -1,
            }) == true) {

                position = (float3)hit.position;
                return true;

            }

            position = default;
            return false;

        }

        protected override void OnRecycle() {
            
            base.OnRecycle();
            
            this.navMeshDataInstance.Remove();
            Object.DestroyImmediate(this.navMeshData);
            this.navMeshData = null;

        }

        public override void OnCopyFrom(Graph other) {

            var navMeshGraphOther = (NavMeshGraph)other;
            ArrayUtils.Copy(navMeshGraphOther.buildSources, ref this.buildSources);
            ArrayUtils.Copy(navMeshGraphOther.tempSources, ref this.tempSources);
            ArrayUtils.Copy(navMeshGraphOther.buildSourcesEntities, ref this.buildSourcesEntities);
            if (navMeshGraphOther.navMeshData != null) {
                
                this.navMeshData = NavMeshData.Instantiate(navMeshGraphOther.navMeshData);
                if (this.navMeshDataInstance.valid == true) this.navMeshDataInstance.Remove();
                
            } else if (this.navMeshData != null) {
                
                Object.DestroyImmediate(this.navMeshData);
                this.navMeshData = null;
                this.navMeshDataInstance.Remove();
                
            }
            
            this.size = navMeshGraphOther.size;
            this.scale = navMeshGraphOther.scale;
        
            this.buildFloor = navMeshGraphOther.buildFloor;
            this.floorHeight = navMeshGraphOther.floorHeight;
        
            this.agentTypeId = navMeshGraphOther.agentTypeId;
            this.agentClimb = navMeshGraphOther.agentClimb;
            this.agentSlope = navMeshGraphOther.agentSlope;
            this.agentRadius = navMeshGraphOther.agentRadius;
            this.agentHeight = navMeshGraphOther.agentHeight;
        
            this.minRegionArea = navMeshGraphOther.minRegionArea;
            this.tileSize = navMeshGraphOther.tileSize;
            this.voxelSize = navMeshGraphOther.voxelSize;
        
            this.buildSettings = navMeshGraphOther.buildSettings;
        
        }

        public override NodeInfo GetNearest(float3 worldPosition, Constraint constraint) {

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
            Gizmos.DrawWireCube((Vector3)this.graphCenter, this.size);

        }

    }

}
