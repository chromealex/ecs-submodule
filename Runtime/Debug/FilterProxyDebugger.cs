
namespace ME.ECS.Debug {

    public class FilterProxyDebugger {

        private Filter filter;
        
        public FilterProxyDebugger(Filter filter) {

            this.filter = filter;

        }
        
        public int id {
            get { return this.filter.id; }
        }

        public int Count {
            get { return this.filter.Count; }
        }

        public Entity[] entities {
            
            get {
            
                var list = new System.Collections.Generic.List<Entity>();
                foreach (var entity in this.filter) {

                    list.Add(entity);

                }

                return list.ToArray();
                
            }
            
        }

    }

}