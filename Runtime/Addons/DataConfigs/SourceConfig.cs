namespace ME.ECS.DataConfigs {

    [ComponentGroup("Data Config", GroupColor.Default, -100)]
    [ComponentOrder(1)]
    [ComponentHelp("Stores last applied config")]
    public struct SourceConfig : IComponent, IComponentRuntime {

        public ConfigId<DataConfig> config;

    }

    #if !STATIC_API_DISABLED
    [ComponentGroup("Data Config", GroupColor.Default, -100)]
    [ComponentOrder(1)]
    [ComponentHelp("Stores all applied configs except first applied (see SourceConfig component)")]
    public struct SourceConfigs : IComponent, IComponentRuntime {

        public ME.ECS.Collections.MemoryAllocator.List<ConfigId<DataConfig>> configs;
        
    }
    #endif

}