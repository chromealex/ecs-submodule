namespace ME.ECS.Name {
    
    [ComponentOrder(-1000)]
    public struct Name : IComponent, IVersioned {

        public string value;

    }

}