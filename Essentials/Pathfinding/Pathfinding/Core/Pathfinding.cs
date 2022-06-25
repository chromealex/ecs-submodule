using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ME.ECS.Mathematics;

namespace ME.ECS.Pathfinding {
    
    using ME.ECS.Collections;
    using Unity.Jobs;

    public struct PathTask {

        public Entity entity;
        public float3 from;
        public float3 to;
        public bool alignToGraphNodes;
        public Constraint constraint;
        public bool burstEnabled;
        public bool cacheEnabled;
        public bool isValid;

    }

    public struct GraphUpdateObject {

        public long graphMask;
        
        public float3 center;
        
        public bool checkSize;
        public float3 size;
        
        public bool checkRadius;
        public float radius;

        public bool changePenalty;
        public int penaltyDelta;

        public bool changeWalkability;
        public bool walkable;

        public Bounds GetBounds() {

            if (this.checkRadius == true) {

                return new Bounds((Vector3)this.center, new Vector3(this.radius * 2f, this.radius * 2f, this.radius * 2f));

            } else if (this.checkSize == true) {

                return new Bounds((Vector3)this.center, (Vector3)this.size);

            }

            return new Bounds((Vector3)this.center, Vector3.zero);

        }

        public void Apply(Node node) {

            if (this.changePenalty == true) {

                node.penalty += this.penaltyDelta;

            }

            if (this.changeWalkability == true) {

                node.walkable = this.walkable;

            }

        }

    }
    
    [ExecuteInEditMode]
    #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class Pathfinding : MonoBehaviour {

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        [System.Serializable]
        public sealed class ModificatorItem {

            public bool enabled;
            public GraphModifierBase modifier;
            
        }

        public const int THREADS_COUNT = 8;

        public static IPathfindingProcessor defaultProcessor = new PathfindingNavMeshProcessor();
        public List<Graph> graphs;

        public LogLevel logLevel;

        public bool clonePathfinding = false;
        
        private HashSet<GraphDynamicModifier> dynamicModifiersContains = new HashSet<GraphDynamicModifier>();
        private HashSet<GraphDynamicModifier> dynamicModifiersList = new HashSet<GraphDynamicModifier>();

        private struct CopyGraph : IArrayElementCopy<Graph> {

            public void Copy(in Graph @from, ref Graph to) {
                
                to.CopyFrom(from);
                
            }

            public void Recycle(ref Graph item) {

                item.Recycle();
                item = null;

            }

        }

        public Pathfinding Clone() {

            var instance = Object.Instantiate(this);
            for (int i = 0; i < this.graphs.Count; ++i) {

                this.graphs[i].pathfindingLogLevel = instance.logLevel;

            }
            instance.CopyFrom(this);
            return instance;

        }

        public void Recycle() {

            this.ClearCache();
            
            this.OnRecycle();
            
            if (this != null && this.gameObject != null) {
                
                Object.Destroy(this.gameObject);
                
            }
            
        }

        private void ClearCache() {

            PathfindingFlowFieldProcessor.ClearCache();

        }

        private void OnRecycle() {

            this.clonePathfinding = false;
            this.logLevel = default;

            if (this.graphs != null) {

                for (int i = 0; i < this.graphs.Count; ++i) {

                    this.graphs[i].Recycle();

                }
                
                PoolList<Graph>.Recycle(ref this.graphs);
                
            }

        }
        
        public void CopyFrom(Pathfinding other) {

            this.clonePathfinding = other.clonePathfinding;
            this.logLevel = other.logLevel;

            ArrayUtils.Copy(other.graphs, ref this.graphs, new CopyGraph());
            
        }
        
        public bool HasLogLevel(LogLevel level) {

            return (this.logLevel & level) != 0;

        }
        
        public void RegisterDynamic(GraphDynamicModifier modifier) {

            if (this.dynamicModifiersContains.Contains(modifier) == false) {

                if (this.dynamicModifiersContains.Add(modifier) == true) {

                    this.dynamicModifiersList.Add(modifier);
                    modifier.ApplyForced();
                    this.BuildAreas();
                    
                }
                
            }

        }

        public void UnRegisterDynamic(GraphDynamicModifier modifier) {

            if (this.dynamicModifiersContains.Contains(modifier) == true) {

                if (this.dynamicModifiersContains.Remove(modifier) == true) {

                    this.dynamicModifiersList.Remove(modifier);
                    modifier.ApplyForced(disabled: true);
                    this.BuildAreas();

                }
                
            }
            
        }

        public void AdvanceTick(float deltaTime) {

            var anyUpdated = false;
            foreach (var mod in this.dynamicModifiersList) {
                
                anyUpdated |= mod.Apply();
                
            }

            if (anyUpdated == true) {
                
                this.BuildAreas();

            }
            
        }

        public void UpdateGraphs(GraphUpdateObject graphUpdateObject) {
            
            if (this.graphs != null) {

                for (int i = 0; i < this.graphs.Count; ++i) {

                    if (graphUpdateObject.graphMask >= 0 && (graphUpdateObject.graphMask & (1 << this.graphs[i].index)) == 0) continue;

                    var graph = this.graphs[i];
                    graph.UpdateGraph(graphUpdateObject);

                }

            }
            
        }

        public float3 ClampPosition(float3 worldPosition, Constraint constraint) {
            
            float3 nearest = default;
            if (this.graphs != null) {

                sfloat dist = sfloat.MaxValue;
                for (int i = 0; i < this.graphs.Count; ++i) {

                    if (constraint.graphMask >= 0 && (constraint.graphMask & (1 << this.graphs[i].index)) == 0) continue;

                    if (this.graphs[i].ClampPosition(worldPosition, constraint, out var node) == true) {

                        var d = math.distancesq(node, worldPosition);
                        if (d < dist) {

                            dist = d;
                            nearest = node;

                        }

                    }

                }

            }

            return nearest;

        }

        public NodeInfo GetNearest(float3 worldPosition, Constraint constraint) {

            NodeInfo nearest = default;
            if (this.graphs != null) {

                sfloat dist = sfloat.MaxValue;
                for (int i = 0; i < this.graphs.Count; ++i) {

                    if (constraint.graphMask >= 0 && (constraint.graphMask & (1 << this.graphs[i].index)) == 0) continue;
                    
                    var node = this.graphs[i].GetNearest(worldPosition, constraint);
                    var d = math.distancesq(node.worldPosition, worldPosition);
                    if (d < dist) {

                        dist = d;
                        nearest = node;

                    }

                }

            }

            return nearest;

        }

        public Path CalculatePath(float3 from, float3 to) {

            var constraint = Constraint.Default;
            return this.CalculatePath(from, to, constraint);
            
        }

        public Path CalculatePath(float3 from, float3 to, Constraint constraint) {

            return this.CalculatePath(from, to, constraint, new PathModifierEmpty());
            
        }

        public Path CalculatePath<TMod>(float3 from, float3 to, TMod pathModifier) where TMod : struct, IPathModifier {

            var constraint = Constraint.Default;
            return this.CalculatePath(from, to, constraint, pathModifier);
            
        }

        public Path CalculatePath<TMod>(float3 from, float3 to, Constraint constraint, TMod pathModifier, int threadIndex = 0, bool cacheEnabled = false, bool burstEnabled = false) where TMod : struct, IPathModifier {

            var graph = this.GetNearest(from, constraint).graph;
            return this.CalculatePath_INTERNAL(Pathfinding.defaultProcessor, from, to, constraint, graph, pathModifier, threadIndex, cacheEnabled: cacheEnabled, burstEnabled: burstEnabled);
            
        }

        public Path CalculatePath<TMod>(float3 from, float3 to, Constraint constraint, Graph graph, TMod pathModifier, int threadIndex = 0, bool cacheEnabled = false) where TMod : struct, IPathModifier {

            return this.CalculatePath_INTERNAL(Pathfinding.defaultProcessor, from, to, constraint, graph, pathModifier, threadIndex, cacheEnabled: cacheEnabled);

        }

        public Path CalculatePath<TProcessor>(float3 from, float3 to, Constraint constraint) where TProcessor : struct, IPathfindingProcessor {

            return this.CalculatePath<PathModifierEmpty, TProcessor>(from, to, constraint, new PathModifierEmpty());
            
        }

        public Path CalculatePath<TMod, TProcessor>(float3 from, float3 to, TMod pathModifier) where TMod : struct, IPathModifier where TProcessor : struct, IPathfindingProcessor {

            var constraint = Constraint.Default;
            return this.CalculatePath<TMod, TProcessor>(from, to, constraint, pathModifier);
            
        }

        public Path CalculatePath<TMod, TProcessor>(float3 from, float3 to, Constraint constraint, TMod pathModifier, int threadIndex = 0) where TMod : struct, IPathModifier where TProcessor : struct, IPathfindingProcessor {

            var graph = this.GetNearest(from, constraint).graph;
            return this.CalculatePath<TMod, TProcessor>(from, to, constraint, graph, pathModifier, threadIndex);
            
        }

        public Path CalculatePath<TMod, TProcessor>(float3 from, float3 to, Constraint constraint, Graph graph, TMod pathModifier, int threadIndex = 0, bool burstEnabled = false) where TMod : struct, IPathModifier where TProcessor : struct, IPathfindingProcessor {

            return this.CalculatePath_INTERNAL(new TProcessor(), from, to, constraint, graph, pathModifier, threadIndex, burstEnabled);

        }

        internal Path CalculatePath_INTERNAL<TMod, TProcessor>(TProcessor processor, float3 from, float3 to, Constraint constraint, Graph graph, TMod pathModifier, int threadIndex = 0, bool burstEnabled = true, bool cacheEnabled = false) where TMod : struct, IPathModifier where TProcessor : IPathfindingProcessor {

            return processor.Run(this.logLevel, from, to, constraint, graph, pathModifier, threadIndex, burstEnabled, cacheEnabled);

        }

        public void BuildAreas() {
            
            if (this.graphs != null) {

                for (int i = 0; i < this.graphs.Count; ++i) {

                    this.graphs[i].BuildAreas();

                }

            }
            
        }
        
        public void GetNodesInBounds(ListCopyable<Node> result, Bounds bounds, Constraint constraint) {
            
            if (this.graphs != null) {

                for (int i = 0; i < this.graphs.Count; ++i) {

                    if (constraint.graphMask >= 0 && (constraint.graphMask & (1 << this.graphs[i].index)) == 0) continue;

                    this.graphs[i].GetNodesInBounds(result, bounds, constraint);

                }

            }
            
        }
        
        public void BuildAll() {

            if (this.graphs != null) {

                for (int i = 0; i < this.graphs.Count; ++i) {

                    this.graphs[i].DoCleanUp();

                }

                for (int i = 0; i < this.graphs.Count; ++i) {

                    this.graphs[i].DoBuild();

                }

            }

        }

        public void OnDrawGizmos() {

            if (this.graphs != null) {

                for (int i = 0; i < this.graphs.Count; ++i) {

                    this.graphs[i].DoDrawGizmos();

                }

            }

        }

        private struct RunTasksJob<TMod, TProcessor> : Unity.Jobs.IJobParallelFor where TMod : struct, IPathModifier where TProcessor : struct, IPathfindingProcessor {

            public Unity.Collections.NativeArray<PathTask> arr;

            public void Execute(int index) {

                var item = this.arr[index];
                if (item.isValid == true) {

                    var instance = Pathfinding.pathfinding;
                    var graph = instance.GetNearest(item.from, item.constraint).graph;
                    Pathfinding.results.arr[index] = instance.CalculatePath_INTERNAL(new TProcessor(), item.@from, item.to, item.constraint, graph, new TMod(), index, item.burstEnabled, item.cacheEnabled);
                    
                }
                this.arr[index] = item;

            }

        }

        private static Pathfinding pathfinding;
        private static BufferArray<Path> results;
        public void RunTasks<TMod, TProcessor>(Unity.Collections.NativeArray<PathTask> tasks, ref BufferArray<Path> results) where TMod : struct, IPathModifier where TProcessor : struct, IPathfindingProcessor {

            ArrayUtils.Resize(tasks.Length, ref Pathfinding.results);
            
            Pathfinding.pathfinding = this;

            var job = new RunTasksJob<TMod, TProcessor>() {
                arr = tasks,
            };
            
            for (int i = 0; i < tasks.Length; ++i) {

                job.Execute(i);

            }
            
            /*var jobHandle = job.Schedule(tasks.Length, 64);
            jobHandle.Complete();
            
            for (int i = 0; i < tasks.Length; ++i) {

                var item = tasks[i];
                if (item.burstEnabled == true) {
                    
                    item.isValid = true;
                    tasks[i] = item;

                }
                
            }*/

            results = Pathfinding.results;

            /*
            for (int i = 0; i < tasks.Count; ++i) {

                var task = tasks[i];
                task.result = this.CalculatePath(task.@from, task.to, task.constraint, task.pathCornersModifier);
                tasks[i] = task;

            }*/

        }
        
        public PathTask CalculatePathTask(Entity entity, float3 requestFrom, float3 requestTo, bool alignToGraphNodes, Constraint constraint, bool burstEnabled, bool cacheEnabled) {

            return new PathTask() {
                entity = entity,
                from = requestFrom,
                to = requestTo,
                alignToGraphNodes = alignToGraphNodes,
                constraint = constraint,
                burstEnabled = burstEnabled,
                cacheEnabled = cacheEnabled,
                isValid = true,
            };
            
        }

    }

}
