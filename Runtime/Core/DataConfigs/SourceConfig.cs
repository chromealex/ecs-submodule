namespace ME.ECS.DataConfigs {

    public struct SourceConfig : IComponent, IComponentRuntime {

        /// <summary>
        /// Last applied config onto entity
        /// </summary>
        public DataConfig config;

    }

}