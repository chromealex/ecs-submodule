using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {

    public class Seeker : MonoBehaviour {

        public Pathfinding pathfinding;
        public Graph graph;
        public PathModifierSeeker modifier;

        public struct Modifier : IPathModifier {

            public PathModifierSeeker mod;

            public Path Run(Path path, Constraint constraint) {

                return this.mod.Run(path, constraint);

            }

            public bool IsWalkable(int index, Constraint constraint) {
            
                return this.mod.IsWalkable(index, constraint);

            }

        }
        
        public Path CalculatePath(float3 from, float3 to, Constraint constraint) {
            
            return this.pathfinding.CalculatePath(from, to, constraint, this.graph, new Modifier() { mod = this.modifier });
            
        }
        
    }

}
