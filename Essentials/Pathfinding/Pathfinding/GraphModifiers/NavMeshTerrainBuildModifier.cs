using UnityEngine;

namespace ME.ECS.Pathfinding {

    public class NavMeshTerrainBuildModifier : GraphModifierBase {

        public Terrain terrain;
        public bool createMesh = true;
        public int area = 0;
        
        public override void ApplyBeforeConnections(Graph graph) {
            
            var navMeshGraph = (NavMeshGraph)graph;
            if (this.createMesh == true) {

                var mesh = MeshUtils.GetMeshFromTerrain(this.terrain);
                navMeshGraph.AddBuildSource(new UnityEngine.AI.NavMeshBuildSource() {
                    area = this.area,
                    shape = UnityEngine.AI.NavMeshBuildSourceShape.Mesh,
                    size = this.terrain.terrainData.size,
                    sourceObject = mesh,
                    transform = Matrix4x4.TRS(this.terrain.transform.position, Quaternion.identity, Vector3.one),
                });

            } else {

                navMeshGraph.AddBuildSource(new UnityEngine.AI.NavMeshBuildSource() {
                    area = this.area,
                    shape = UnityEngine.AI.NavMeshBuildSourceShape.Terrain,
                    size = this.terrain.terrainData.size,
                    sourceObject = this.terrain.terrainData,
                    transform = Matrix4x4.TRS(this.terrain.transform.position, Quaternion.identity, Vector3.one),
                });

            }

        }

        public override void ApplyAfterConnections(Graph graph) {
            
        }

    }

}
