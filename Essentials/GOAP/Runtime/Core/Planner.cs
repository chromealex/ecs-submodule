#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
using tfloat = sfloat;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
using tfloat = System.Single;
#endif
using ME.ECS.Collections;
using Unity.Collections;
using Unity.Jobs;

namespace ME.ECS.Essentials.GOAP {
    
    using Collections.V3;
    using Collections.MemoryAllocator;

    public struct ActionTemp {

        public Action action;
        public bool canRun;

    }

    public struct Planner {

        public static Plan GetPlan(World world, NativeArray<ActionTemp> actions, Goal goal, in Entity entity) {

            var plan = new Plan() {
                planStatus = PathStatus.NotCalculated,
                cost = 0f,
            };

            plan.planStatus = PathStatus.Processing;

            var entityStateList = world.GetState().structComponents.entitiesIndexer.Get(in world.GetState().allocator, entity.id);
            ref var allocator = ref world.GetState().allocator;
            var entityState = new EquatableHashSet<int>(ref allocator, entityStateList.Count);
            var entityStateData = new NativeHashSet<UnsafeData>(ref allocator, entityStateList.Count);
            
            var e = entityStateList.GetEnumerator(in world.GetState().allocator);
            while (e.MoveNext() == true) {
                var idx = e.Current;
                entityState.Add(ref allocator, idx);
                var reg = world.GetState().structComponents.GetAllRegistries()[idx];
                if (reg is IComponentsBlittable || reg is IComponentsUnmanaged) {
                    var obj = reg.CreateObjectUnsafe(in entity);
                    entityStateData.Add(ref allocator, obj);
                } else {
                    entityStateData.Add(ref allocator, default);
                }
            }
            e.Dispose();
            
            var graph = new NativeArray<Action.Data>(actions.Length, Allocator.TempJob);
            for (int i = 0; i < actions.Length; ++i) {

                ref var action = ref actions.GetRef(i);
                action.action.data.id = i;
                action.action.data.parent = -1;

            }

            for (int i = 0; i < actions.Length; ++i) {

                ref var action = ref actions.GetRef(i);
                if (action.canRun == true) {
                    action.action.BuildNeighbours(actions);
                    graph[i] = action.action.data;
                }

            }

            var bestPath = Planner.GetBestPath(in allocator, graph, goal, entityState, entityStateData);
            if (bestPath.pathStatus == PathStatus.Success) {

                plan.actions = PoolArray<Action>.Spawn(bestPath.actions.Length);
                for (int i = 0; i < plan.actions.Length; ++i) {
                    plan.actions[i] = actions[bestPath.actions[i]].action;
                }
                bestPath.actions.Dispose();

                plan.cost = bestPath.cost;
                plan.planStatus = PathStatus.Success;

            } else {

                plan.planStatus = PathStatus.Failed;

            }

            entityState.Dispose(ref allocator);
            {
                var set = entityStateData.GetEnumerator(in allocator);
                while (set.MoveNext() == true) {
                    set.Current.Dispose(ref allocator);
                }
                set.Dispose();
            }
            entityStateData.Dispose(ref allocator);
            graph.Dispose();

            return plan;

        }

        private static Path GetBestPath(in MemoryAllocator allocator, NativeArray<Action.Data> temp, Goal goal, EquatableHashSet<int> entityState, NativeHashSet<UnsafeData> entityStateData) {

            var result = new NativeArray<Path>(1, Allocator.TempJob);
            new Job() {
                allocator = allocator,
                temp = temp,
                goal = goal,
                entityState = entityState,
                entityStateData = entityStateData,
                result = result,
            }.Run();
            var res = result[0];
            result.Dispose();
            return res;

        }

        private struct Path {

            public tfloat cost;
            public SpanArray<int> actions;
            public PathStatus pathStatus;

        }

        [Unity.Burst.BurstCompileAttribute]
        private struct Job : Unity.Jobs.IJob {

            public MemoryAllocator allocator;
            public NativeArray<Action.Data> temp;
            public Goal goal;
            public EquatableHashSet<int> entityState;
            public NativeHashSet<UnsafeData> entityStateData;
            public NativeArray<Path> result;
            
            public void Execute() {

                var bestPath = new Path() {
                    cost = float.MaxValue,
                };
                for (int i = 0; i < this.temp.Length; ++i) {

                    var action = this.temp[i];
                    if (action.conditions.Has(in this.allocator, this.entityState) == true &&
                        action.conditions.HasData(in this.allocator, this.entityStateData) == true &&
                        action.conditions.HasNoData(in this.allocator, this.entityStateData) == true) {

                        // We have found start action
                        var result = Planner.Traverse(in this.allocator, bestPath.cost, this.temp, this.goal, action, this.entityState);
                        if (result.pathStatus == PathStatus.Success &&
                            result.cost < bestPath.cost) {
                            bestPath = result;
                        }

                    }

                }
                
                this.result[0] = bestPath;

            }

        }
        
        private static Path Traverse(in MemoryAllocator allocator, tfloat prevCost, NativeArray<Action.Data> temp, Goal goal, Action.Data startAction, EquatableHashSet<int> entityState) {

            for (int i = 0; i < temp.Length; ++i) {

                ref var action = ref temp.GetRef(i);
                action.parent = -1;
                action.h = 0f;
                action.isClosed = false;

            }

            var path = new Path() {
                cost = 0f,
                actions = default,
                pathStatus = PathStatus.NotCalculated,
            };

            path.pathStatus = PathStatus.Processing;
            var stack = new PriorityQueueNative<int>(Allocator.Temp, isMinPriorityQueue: true);
            startAction.h = 0f;
            startAction.isClosed = true;
            temp[startAction.id] = startAction;
            stack.Enqueue(0f, startAction.id);
            var max = 10000;
            while (stack.Count > 0) {

                if (--max == 0) {
                    UnityEngine.Debug.LogError("Max break");
                    break;
                }

                var actionIdx = stack.Dequeue();
                ref readonly var action = ref temp.GetRef(actionIdx);
                path.cost += action.cost;

                if (action.HasGoal(goal) == true) {
                    var act = action;
                    var tempResult = new NativeList<int>(10, Allocator.Temp);
                    tempResult.Add(act.id);
                    while (act.parent != -1) {
                        act = temp[act.parent];
                        tempResult.Add(act.id);
                    }
                    path.actions = new SpanArray<int>(tempResult);
                    tempResult.Dispose();

                    for (int i = 0; i < path.actions.Length / 2; ++i) {

                        var ePtr = path.actions[path.actions.Length - 1 - i];
                        path.actions[path.actions.Length - 1 - i] = path.actions[i];
                        path.actions[i] = ePtr;

                    }

                    path.pathStatus = PathStatus.Success;
                    break;
                }

                // get neighbours
                var neighbours = action.GetNeighbours();
                for (int i = 0; i < neighbours.Length; ++i) {

                    var nIdx = neighbours[i];
                    ref var n = ref temp.GetRef(nIdx);
                    var alt = n.cost + action.h;
                    if (path.cost + n.cost >= prevCost) continue;
                    if (n.HasPreconditions(in allocator, temp, in action, entityState) == false) continue;

                    if (n.isClosed == false || alt < n.h) {
                        n.h = alt;
                        if (n.isClosed == false) {
                            n.isClosed = true;
                            n.parent = action.id;
                            stack.Enqueue(alt, n.id);
                        }
                    }

                }

            }

            if (path.pathStatus != PathStatus.Success) path.pathStatus = PathStatus.Failed;

            return path;

        }

    }

}