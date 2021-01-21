
namespace ME.ECS.Pathfinding.Features {

    using Collections;
    using ME.ECS.Pathfinding;
    using Pathfinding.Components; using Pathfinding.Modules; using Pathfinding.Systems; using Pathfinding.Markers;
    
    namespace Pathfinding.Components {}
    namespace Pathfinding.Modules {}
    namespace Pathfinding.Systems {}
    namespace Pathfinding.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PathfindingFeature : Feature {

        private ME.ECS.Pathfinding.Pathfinding pathfindingInstance;
        private Entity pathfindingEntity;
        
        public void SetInstance(ME.ECS.Pathfinding.Pathfinding pathfinding) {

            this.pathfindingInstance = pathfinding;
            
        }

        internal ME.ECS.Pathfinding.Pathfinding GetInstance() {

            return this.pathfindingInstance;

        }

        internal Entity GetEntity() {

            return this.pathfindingEntity;

        }
        
        protected override void OnConstruct() {

            this.pathfindingInstance = null;
            
            PathfindingComponentsInitializer.Init(ref this.world.GetStructComponents());
            ComponentsInitializerWorld.Register(PathfindingComponentsInitializer.InitEntity);
            
            var entity = new Entity("Pathfinding");
            entity.SetData(new IsPathfinding());
            this.pathfindingEntity = entity;
            
            this.AddSystem<SetPathfindingInstanceSystem>();
            this.AddSystem<BuildGraphsSystem>();
            this.AddSystem<BuildPathSystem>();
            this.AddSystem<PathfindingUpdateSystem>();

        }

        protected override void OnDeconstruct() {
            
            ComponentsInitializerWorld.UnRegister(PathfindingComponentsInitializer.InitEntity);
            
        }

        public void SetPath(in Entity entity, ME.ECS.Pathfinding.Path path, Constraint constraint, UnityEngine.Vector3 to) {
            
            this.SetPath(in entity, path.nodesModified, path.result, constraint, to);
            
        }

        public void SetPath(in Entity entity, ListCopyable<Node> nodes, PathCompleteState result, Constraint constraint, UnityEngine.Vector3 to) {

            entity.RemoveComponents<ME.ECS.Pathfinding.Features.Pathfinding.Components.Path>();

            var vPath = PoolListCopyable<UnityEngine.Vector3>.Spawn(nodes.Count);
            for (var i = 0; i < nodes.Count; ++i) {

                var node = nodes[i];
                vPath.Add(node.worldPosition);

            }

            if (nodes.Count > 1) {
            
                var currentPos = entity.GetPosition();
                var a = vPath[0];
                var b = vPath[1];
                var target = a + UnityEngine.Vector3.Project(currentPos - a, b - a);
                vPath[0] = target;

            }
            
            var nearestTarget = this.GetNearest(to);
            if (nearestTarget.IsSuitable(constraint) == true) {

                vPath.Add(to);

            }

            var unitPath = entity.AddComponent<ME.ECS.Pathfinding.Features.Pathfinding.Components.Path>();
            unitPath.result = result;
            unitPath.path = ME.ECS.Collections.BufferArray<UnityEngine.Vector3>.From(vPath);
            unitPath.nodes = ME.ECS.Collections.BufferArray<ME.ECS.Pathfinding.Node>.From(nodes);

            entity.SetData(new IsPathBuilt(), ComponentLifetime.NotifyAllSystems);
                
            PoolListCopyable<UnityEngine.Vector3>.Recycle(ref vPath);

        }

        public ME.ECS.Pathfinding.Path CalculatePath(UnityEngine.Vector3 from, UnityEngine.Vector3 to, Constraint constraint) {
            
            var active = this.GetEntity().GetComponent<PathfindingInstance>().pathfinding;
            if (active == null) {

                return default;

            }
            
            var path = active.CalculatePath(from, to, constraint, new ME.ECS.Pathfinding.PathCornersModifier());
            return path;

        }
        
        public void GetNodesInBounds(ListCopyable<Node> output, UnityEngine.Bounds bounds) {
         
            this.pathfindingEntity.GetComponent<PathfindingInstance>().pathfinding.GetNodesInBounds(output, bounds);
            
        }

        public bool BuildNodePhysics(Node node) {

            return this.pathfindingEntity.GetComponent<PathfindingInstance>().pathfinding.BuildNodePhysics(node);

        }
        
        public T GetGraphByIndex<T>(int index) where T : Graph {

            return this.pathfindingEntity.GetComponent<PathfindingInstance>().pathfinding.graphs[index] as T;

        }
        
        public Node GetNearest(UnityEngine.Vector3 worldPosition) {

            return this.pathfindingEntity.GetComponent<PathfindingInstance>().pathfinding.GetNearest(worldPosition, Constraint.Default);

        }

        public Node GetNearest(UnityEngine.Vector3 worldPosition, Constraint constraint) {
            
            return this.pathfindingEntity.GetComponent<PathfindingInstance>().pathfinding.GetNearest(worldPosition, constraint);
            
        }

    }

}