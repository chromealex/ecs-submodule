namespace ME.ECS.DataConfigs {

    [ComponentGroup("Data Config", GroupColor.Default, -100)]
    [ComponentOrder(1)]
    public struct SourceConfig : IComponent, IComponentRuntime {

        /// <summary>
        /// Last applied config onto entity
        /// </summary>
        public DataConfig config;

    }

}