using UnityEngine;

namespace ME.ECS.Pathfinding {

    public class NavMeshComponentBuildModifier : GraphModifierBase {

        public MeshFilter component;
        public int area = 0;
        
        public override void ApplyBeforeConnections(Graph graph) {
            
            var navMeshGraph = (NavMeshGraph)graph;
            var mesh = this.component.sharedMesh;
            navMeshGraph.AddBuildSource(new UnityEngine.AI.NavMeshBuildSource() {
                area = this.area,
                shape = UnityEngine.AI.NavMeshBuildSourceShape.Mesh,
                sourceObject = mesh,
                transform = Matrix4x4.TRS(this.component.transform.position, this.component.transform.rotation, this.component.transform.localScale),
            });

        }

        public override void ApplyAfterConnections(Graph graph) {
            
        }

    }

}
