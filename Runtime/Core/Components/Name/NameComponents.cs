namespace ME.ECS.Name {
    
    [ComponentGroup(typeof(NameComponentConstants.GroupInfo))]
    [ComponentOrder(1)]
    public struct Name : IComponent, IVersioned {

        public Unity.Collections.FixedString64Bytes value;

    }

}