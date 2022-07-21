namespace ME.ECS.DataConfigs {

    [ComponentGroup("Data Config", GroupColor.Default, -100)]
    [ComponentOrder(1)]
    [ComponentHelp("Stores last applied config")]
    public struct SourceConfig : IComponent, IComponentRuntime {

        public DataConfig config;

    }

    #if !STATIC_API_DISABLED
    [ComponentGroup("Data Config", GroupColor.Default, -100)]
    [ComponentOrder(1)]
    [ComponentHelp("Stores all applied configs except first applied (see SourceConfig component)")]
    public struct SourceConfigs : ICopyable<SourceConfigs>, IComponentRuntime {

        public ME.ECS.Collections.ListCopyable<DataConfig> configs;

        public void CopyFrom(in SourceConfigs other) {
            
            ArrayUtils.Copy(other.configs, ref this.configs);
            
        }

        public void OnRecycle() {
            
            PoolListCopyable<DataConfig>.Recycle(ref this.configs);
            
        }

    }
    #endif

}