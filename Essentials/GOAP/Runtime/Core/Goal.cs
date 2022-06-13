namespace ME.ECS.Essentials.GOAP {

    [System.Serializable]
    public struct GoalData {

        public ComponentData component;

    }

    public struct Goal {

        internal int hasComponent;

        public static Goal Create<T>() where T : struct, IComponent {
            
            return new Goal() { hasComponent = AllComponentTypes<T>.typeId };
            
        }

        public static Goal Create(GoalData data) {

            var component = data.component.component;
            if (ComponentTypesRegistry.allTypeId.TryGetValue(component.GetType(), out var index) == true) {
                
                return new Goal() { hasComponent = index };

            }

            return default;
            
        }

        public bool Has(in Entity entity) {

            return Worlds.current.HasDataBit(in entity, this.hasComponent);

        }

    }

}