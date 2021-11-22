
namespace ME.ECS.Pathfinding.Features.PathfindingAstar.Systems {

    using ME.ECS.Pathfinding.Features.Pathfinding.Components;

    #pragma warning disable
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class BuildSystem : ISystemFilter, IAdvanceTickPost, IAdvanceTickPre {

        private PathfindingFeature pathfindingFeature;
        private Unity.Collections.NativeArray<PathTask> pathTasks;
        private int idx;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct() {

            this.pathfindingFeature = this.world.GetFeature<PathfindingFeature>();

        }
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-BuildPathSystem")
                         .With<CalculatePath>()
                         .Push();
            
        }

        void IAdvanceTickPre.AdvanceTickPre(in float deltaTime) {
            
            if (this.pathTasks.IsCreated == true) this.pathTasks.Dispose();
            this.idx = 0;

        }

        void IAdvanceTickPost.AdvanceTickPost(in float deltaTime) {

            if (this.pathfindingFeature.GetEntity().Has<PathfindingInstance>() == false) return;
            
            var instance = this.pathfindingFeature.GetEntity().Get<PathfindingInstance>();
            var active = instance.pathfinding;
            if (active == null) return;

            if (this.pathTasks.IsCreated == true) {

                ME.ECS.Collections.BufferArray<ME.ECS.Pathfinding.Path> results = default;
                active.RunTasks<PathCornersModifier, PathfindingAstarProcessor>(this.pathTasks, ref results);
                //UnityEngine.Debug.Log("Calc paths: " + this.idx);
                for (int i = 0; i < this.idx; ++i) {

                    var task = this.pathTasks[i];
                    if (task.isValid == false) continue;
                    
                    var path = results.arr[i];
                    //UnityEngine.Debug.Log("Path build: " + i + " :: " + path.result);
                    if (path.result == ME.ECS.Pathfinding.PathCompleteState.Complete ||
                        path.result == ME.ECS.Pathfinding.PathCompleteState.CompletePartial) {

                        this.pathfindingFeature.SetPathAstar(in task.entity, path, task.constraint, task.from, task.to, task.alignToGraphNodes);

                    } else {

                        task.entity.Remove<Path>();

                    }

                    path.Recycle();

                }

                this.pathTasks.Dispose();

            }
            
        }
        
        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {
            
            if (this.pathfindingFeature.GetEntity().Has<PathfindingInstance>() == false) return;

            var instance = this.pathfindingFeature.GetEntity().Get<PathfindingInstance>();
            var active = instance.pathfinding;
            if (active == null) return;

            //entity.Remove<Path>();
            
            ref readonly var request = ref entity.Read<CalculatePath>();
            if (request.pathType == PathType.AStar) {

                //UnityEngine.Debug.LogWarning("REQUEST PATH: " + request.@from.ToStringDec() + " to " + request.to.ToStringDec());
                var constraint = request.constraint;
                NativeArrayUtils.Resize(this.idx, ref this.pathTasks);
                this.pathTasks[this.idx++] = active.CalculatePathTask(entity, request.from, request.to, request.alignToGraphNodes, constraint, request.burstEnabled, request.cacheEnabled);

                entity.Remove<CalculatePath>();

            }

        }
    
    }
    
}