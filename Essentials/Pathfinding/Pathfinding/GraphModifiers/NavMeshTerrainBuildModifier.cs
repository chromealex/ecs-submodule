using UnityEngine;

namespace ME.ECS.Pathfinding {

    public class NavMeshTerrainBuildModifier : GraphModifierBase {

        public Terrain terrain;
        
        public override void ApplyBeforeConnections(Graph graph) {
            
            var navMeshGraph = (NavMeshGraph)graph;
            navMeshGraph.AddBuildSource(new UnityEngine.AI.NavMeshBuildSource() {
                area = 0,
                shape = UnityEngine.AI.NavMeshBuildSourceShape.Terrain,
                size = this.terrain.terrainData.size,
                sourceObject = this.terrain.terrainData,
                transform = Matrix4x4.TRS(this.terrain.transform.position, Quaternion.identity, Vector3.one),
            });

        }

        public override void ApplyAfterConnections(Graph graph) {
            
        }

    }

}
