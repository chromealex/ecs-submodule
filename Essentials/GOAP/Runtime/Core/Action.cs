#if FIXED_POINT_MATH
using math = ME.ECS.Mathematics.math;
using float3 = ME.ECS.Mathematics.float3;
using tfloat = sfloat;
#else
using math = Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;
using tfloat = System.Single;
#endif

namespace ME.ECS.Essentials.GOAP {

    using Collections;
    using Unity.Collections;
    using Collections.V3;
    using Collections.MemoryAllocator;
    
    public struct Action {

        internal struct Data {

            // For planner
            internal int id;
            internal int parent;
            internal tfloat h;
            internal bool isClosed;
            internal SpanArray<int> neighbours;

            // Public
            public GOAPGroupId groupId;
            public tfloat cost;
            public Condition conditions;
            public Effect effects;

            internal readonly bool HasPreconditions(in MemoryAllocator allocator, NativeArray<Action.Data> temp, in Action.Data parentAction, EquatableHashSet<int> entityState) {
                return this.conditions.Has(in allocator, temp, in parentAction, entityState);
            }

            internal readonly SpanArray<int> GetNeighbours() {
                return this.neighbours;
            }

            internal readonly bool HasGoal(Goal goal) {

                // this method should check if goal exists in action's effect
                for (int i = 0; i < this.effects.hasComponents.Length; ++i) {

                    if (this.effects.hasComponents[i] == goal.hasComponent) return true;

                }

                return false;

            }

            public void Dispose() {
                
                this.conditions.Dispose();
                this.effects.Dispose();
                this.neighbours.Dispose();
                
            }

        }

        // For planner
        internal Data data;

        public Action(in Entity agent, GOAPGroupId groupId, GOAPAction goapAction, Allocator allocator) {
            
            this.data = new Data() {
                groupId = groupId,
                cost = goapAction.GetCost(in agent),
                conditions = goapAction.GetPreconditions(allocator),
                effects = goapAction.GetEffects(allocator),
            };

        }

        public void Dispose() {
            
            this.data.Dispose();
            
        }
        
        internal void BuildNeighbours(NativeArray<ActionTemp> availableActions) {

            var temp = PoolListCopyable<int>.Spawn(availableActions.Length);

            for (int i = 0; i < availableActions.Length; ++i) {

                var action = availableActions[i];
                if (action.canRun == false) continue;
                if (action.action.data.id == this.data.id) continue;

                // if action has effects which gives us any of precondition list
                if (this.data.effects.HasAny(action.action.data.conditions) == true) {

                    temp.Add(action.action.data.id);

                }

            }
            
            this.data.neighbours = new SpanArray<int>(temp.Count);
            for (int i = 0; i < temp.Count; ++i) {
                this.data.neighbours[i] = temp[i];
            }
            PoolListCopyable<int>.Recycle(ref temp);

        }

    }

}