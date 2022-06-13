namespace ME.ECS.Essentials.GOAP {

    using Collections;
    using Unity.Collections;
    
    public struct Action {

        internal struct Data {

            // For planner
            internal int id;
            internal int parent;
            internal float h;
            internal bool isClosed;
            internal SpanArray<int> neighbours;

            // Public
            public GOAPGroupId groupId;
            public float cost;
            public Precondition preconditions;
            public Effect effects;

            internal readonly bool HasPreconditions(NativeArray<Action.Data> temp, in Action.Data parentAction, NativeHashSet<int> entityState) {
                return this.preconditions.Has(temp, in parentAction, entityState);
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
                
                this.preconditions.Dispose();
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
                preconditions = goapAction.GetPreconditions(allocator),
                effects = goapAction.GetEffects(allocator),
            };

        }

        public void Dispose() {
            
            this.data.Dispose();
            
        }
        
        internal void BuildNeighbours(NativeArray<Action> availableActions) {

            var temp = PoolListCopyable<int>.Spawn(availableActions.Length);

            for (int i = 0; i < availableActions.Length; ++i) {

                var action = availableActions[i];
                if (action.data.id == this.data.id) continue;

                // if action has effects which gives us any of precondition list
                if (this.data.effects.HasAny(action.data.preconditions) == true) {

                    temp.Add(action.data.id);

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