using ME.ECS;

namespace ME.ECS.Essentials.GOAP.Modules {
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    
    public sealed class GOAPModule : IModule {

        private ME.ECS.Collections.DictionaryCopyable<GOAPGroup, GOAPGroupId> groupToGroupId;
        private ME.ECS.Collections.DictionaryCopyable<GOAPGroupId, GOAPGroup> groupIdToGroup;
        private GOAPGroupId nextGroupId;

        public World world { get; set; }
        public void OnConstruct() {
            
            this.groupToGroupId = PoolDictionaryCopyable<GOAPGroup, GOAPGroupId>.Spawn(10);
            this.groupIdToGroup = PoolDictionaryCopyable<GOAPGroupId, GOAPGroup>.Spawn(10);
            
        }

        public void OnDeconstruct() {

            foreach (var kv in this.groupIdToGroup) {

                kv.Value.Dispose();

            }
            
            PoolDictionaryCopyable<GOAPGroup, GOAPGroupId>.Recycle(ref this.groupToGroupId);
            PoolDictionaryCopyable<GOAPGroupId, GOAPGroup>.Recycle(ref this.groupIdToGroup);
            
        }

        public GOAPGroup GetGroupById(GOAPGroupId groupId) {

            if (this.groupIdToGroup.TryGetValue(groupId, out var group) == true) {

                return group;

            }

            return null;

        }

        public GOAPAction GetAction(GOAPGroupId groupId, int index) {

            if (index < 0) return null;
            if (this.groupIdToGroup.TryGetValue(groupId, out var group) == true) {

                if (index >= group.actions.Length) return null;
                return group.actions[index];

            }

            return null;

        }

        public Unity.Collections.NativeArray<ActionTemp> GetGroupActions(in Entity entity, GOAPGroupId groupId, Unity.Collections.Allocator allocator) {
            
            if (this.groupIdToGroup.TryGetValue(groupId, out var group) == true) {
                
                var arr = new Unity.Collections.NativeArray<ActionTemp>(group.actions.Length, allocator);
                for (int i = 0; i < group.actions.Length; ++i) {

                    var goapAction = group.actions[i];
                    arr[i] = new ActionTemp() {
                        action = new Action(entity, groupId, goapAction, allocator),
                        canRun = goapAction.CanRunPrepare(in entity),
                    };

                }
                
                return arr;

            }

            return default;

        }

        public GOAPGroupId RegisterGroup(GOAPGroup group) {

            if (this.groupToGroupId.TryGetValue(group, out var id) == true) {

                return id;

            }
            
            id = ++this.nextGroupId;
            this.groupToGroupId.Add(group, id);
            this.groupIdToGroup.Add(id, group);
            group.DoAwake();

            return id;

        }

    }

}
