namespace ME.ECS.Views.Features {

    using System.Collections.Generic;
    
    public class ViewIndexerFeature : Feature {

        public List<ViewBase> configs = new List<ViewBase>(10);
        
        protected override void OnConstruct() {

        }

        protected override void OnDeconstruct() {
            
        }

        public T GetData<T>(ViewId<T> id) where T : ViewBase {

            return this.Get(id.id) as T;

        }

        public ViewId<T> RegisterConfig<T>(T config) where T : ViewBase {

            for (var i = 0; i < this.configs.Count; ++i) {
                var item = this.configs[i];
                if (item == config) {
                    return new ViewId<T>(i + 1);
                }
            }

            var id = this.configs.Count + 1;
            this.configs.Add(config);
            return new ViewId<T>(id);

        }

        public ViewBase Get(int id) {

            var index = id - 1;
            if (index < 0 || index >= this.configs.Count) return null;
            return this.configs[index];
            
        }

        public int AddOrGet(ViewBase config) {

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