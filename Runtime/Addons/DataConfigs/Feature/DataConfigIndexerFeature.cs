namespace ME.ECS.DataConfigs {

    using System.Collections.Generic;

    public class DataConfigIndexerFeature : Feature {
        
        public List<DataConfig> configs = new List<DataConfig>(10);
        
        protected override void OnConstruct() {

        }

        protected override void OnDeconstruct() {
            
        }

        public T GetData<T>(ConfigId<T> id) where T : DataConfig {

            return this.Get(id.id) as T;

        }

        public ConfigId<T> RegisterConfig<T>(T config) where T : DataConfig {

            for (var i = 0; i < this.configs.Count; ++i) {
                var item = this.configs[i];
                if (item == config) {
                    return new ConfigId<T>(i + 1);
                }
            }

            var id = this.configs.Count + 1;
            this.configs.Add(config);
            return new ConfigId<T>(id);

        }

        public DataConfig Get(int id) {

            var index = id - 1;
            if (index < 0 || index >= this.configs.Count) return null;
            return this.configs[index];
            
        }

        public int AddOrGet(DataConfig config) {

            if (config == null) return 0;
            
            var nullIndex = -1;
            for (var i = 0; i < this.configs.Count; ++i) {
                var item = this.configs[i];
                if (item == config) {
                    return i + 1;
                }

                if (item == null) {
                    nullIndex = i;
                }
            }

            if (nullIndex >= 0) {
                this.configs[nullIndex] = config;
                return nullIndex + 1;
            }
            
            var id = this.configs.Count + 1;
            this.configs.Add(config);
            return id;

        }

    }

}