using System.Collections.Generic;
using ME.ECS.Collections;
using Unity.Collections;
using Unity.Jobs;

namespace ME.ECS.Essentials.GOAP {

    public struct Planner {

        public static Plan GetPlan(World world, NativeArray<Action> actions, Goal goal, in Entity entity) {

            var plan = new Plan() {
                planStatus = PathStatus.NotCalculated,
                cost = 0f,
            };

            plan.planStatus = PathStatus.Processing;

            var entityStateList = world.GetState().structComponents.entitiesIndexer.Get(entity.id);
            var entityState = new NativeHashSet<int>(entityStateList.Count, Allocator.TempJob);
            foreach (var item in entityStateList) {
                entityState.Add(item);
            }
            
            var entityStateData = new NativeHashSet<UnsafeData>(entityStateList.Count, Allocator.TempJob);
            foreach (var idx in entityStateList) {
                var reg = world.GetState().structComponents.GetAllRegistries()[idx];
                if (reg is IComponentsBlittable) {
                    var obj = reg.CreateObjectUnsafe(in entity);
                    entityStateData.Add(obj);
                } else {
                    entityStateData.Add(default);
                }
            }
            
            var temp = new NativeArray<Action.Data>(actions.Length, Allocator.TempJob);
            for (int i = 0; i < actions.Length; ++i) {

                ref var action = ref actions.GetRef(i);
                action.data.id = i;
                action.data.parent = -1;

            }

            for (int i = 0; i < actions.Length; ++i) {

                ref var action = ref actions.GetRef(i);
                action.BuildNeighbours(actions);
                temp[i] = action.data;

            }

            var bestPath = Planner.GetBestPath(temp, goal, entityState, entityStateData);
            if (bestPath.pathStatus == PathStatus.Success) {

                plan.actions = PoolArray<Action>.Spawn(bestPath.actions.Length);
                for (int i = 0; i < plan.actions.Length; ++i) {
                    plan.actions[i] = actions[bestPath.actions[i]];
                }
                bestPath.actions.Dispose();

                plan.cost = bestPath.cost;
                plan.planStatus = PathStatus.Success;

            } else {

                plan.planStatus = PathStatus.Failed;

            }

            entityState.Dispose();
            foreach (var item in entityStateData) {
                item.Dispose();
            }
            entityStateData.Dispose();
            temp.Dispose();

            return plan;

        }

        private static Path GetBestPath(NativeArray<Action.Data> temp, Goal goal, NativeHashSet<int> entityState, NativeHashSet<UnsafeData> entityStateData) {

            var result = new NativeArray<Path>(1, Allocator.TempJob);
            new Job() {
                temp = temp,
                goal = goal,
                entityState = entityState,
                entityStateData = entityStateData,
                result = result,
            }.Schedule().Complete();
            var res = result[0];
            result.Dispose();
            return res;

        }

        private struct Path {

            public float cost;
            public SpanArray<int> actions;
            public PathStatus pathStatus;

        }

        [Unity.Burst.BurstCompileAttribute]
        private struct Job : Unity.Jobs.IJob {

            public NativeArray<Action.Data> temp;
            public Goal goal;
            public NativeHashSet<int> entityState;
            public NativeHashSet<UnsafeData> entityStateData;
            public NativeArray<Path> result;
            
            public void Execute() {

                var bestPath = new Path() {
                    cost = float.MaxValue,
                };
                for (int i = 0; i < this.temp.Length; ++i) {

                    var action = this.temp[i];
                    if (action.preconditions.Has(this.entityState) == true &&
                        action.preconditions.HasData(this.entityStateData) == true &&
                        action.preconditions.HasNoData(this.entityStateData) == true) {

                        // We have found start action
                        var result = Planner.Traverse(bestPath.cost, this.temp, this.goal, action, this.entityState);
                        if (result.pathStatus == PathStatus.Success &&
                            result.cost < bestPath.cost) {
                            bestPath = result;
                        }

                    }

                }
                
                this.result[0] = bestPath;

            }

        }
        
        private static Path Traverse(float prevCost, NativeArray<Action.Data> temp, Goal goal, Action.Data startAction, NativeHashSet<int> entityState) {

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
                    if (n.HasPreconditions(temp, in action, entityState) == false) continue;

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