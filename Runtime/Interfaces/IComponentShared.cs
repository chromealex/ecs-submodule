namespace ME.ECS {

    #if !SHARED_COMPONENTS_DISABLED
    public interface IComponentShared : IComponentBase { }

    public interface ISharedGroups {

        System.Collections.Generic.ICollection<uint> GetGroups();

    }
    #endif

}