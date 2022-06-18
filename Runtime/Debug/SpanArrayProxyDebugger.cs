namespace ME.ECS.Debug {

    public class SpanArrayProxyDebugger<T> where T : unmanaged {

        private ME.ECS.Collections.SpanArray<T> arr;
        
        public SpanArrayProxyDebugger(ME.ECS.Collections.SpanArray<T> arr) {

            this.arr = arr;

        }

        public T[] items {
            get {
                var arr = new T[this.arr.Length];
                for (int i = 0; i < this.arr.Length; ++i) {
                    arr[i] = this.arr[i];
                }
                return arr;
            }
        }

    }

}