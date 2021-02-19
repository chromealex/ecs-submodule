
namespace ME.ECS.Pathfinding {
    
    using UnityEngine;
    using ME.ECS.Collections;

    public abstract class PathfindingProcessor {

        public abstract Path Run<TMod>(LogLevel pathfindingLogLevel, Vector3 from, Vector3 to, Constraint constraint, Graph graph, TMod pathModifier, int threadIndex = 0) where TMod : IPathModifier;

    }
    
}
