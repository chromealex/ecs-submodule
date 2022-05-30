using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {
    
    using UnityEngine;
    using ME.ECS.Collections;

    public interface IPathfindingProcessor {

        Path Run<TMod>(LogLevel pathfindingLogLevel, float3 from, float3 to, Constraint constraint, Graph graph, TMod pathModifier, int threadIndex = 0, bool burstEnabled = true, bool cacheEnabled = false) where TMod : struct, IPathModifier;

    }
    
}
