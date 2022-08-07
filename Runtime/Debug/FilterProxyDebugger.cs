
namespace ME.ECS.DebugUtils {

    public class FilterProxyDebugger {

        private Filter filter;
        
        public FilterProxyDebugger(Filter filter) {

            this.filter = filter;

        }
        
        public int id => this.filter.id;
        public int Count => this.filter.Count;
        
        public Entity[] entities {
            
            get {
            
                var arr = this.filter.ToArray();
                var arrResult = arr.ToArray();
                arr.Dispose();
                return arrResult;
                
            }
            
        }

    }

}