namespace ME.ECS.DebugUtils {
    
    using Collections.LowLevel;
    using Collections.LowLevel.Unsafe;

    public static class StaticAllocatorProxyDebugger {

        public static MemoryAllocator defaultAllocator;
        public static ref MemoryAllocator allocator {
            get {
                if (Worlds.current == null || Worlds.current.currentState == null) {
                    return ref StaticAllocatorProxyDebugger.defaultAllocator;
                }

                return ref Worlds.current.currentState.allocator;
            }

        }

    }

    public sealed class MemArrayAllocatorProxyDebugger<T> where T : unmanaged {

        private MemArrayAllocator<T> arr;
        
        public MemArrayAllocatorProxyDebugger(MemArrayAllocator<T> arr) {

            this.arr = arr;

        }

        public T[] items {
            get {
                var arr = new T[this.arr.Length];
                for (int i = 0; i < this.arr.Length; ++i) {
                    arr[i] = this.arr[in StaticAllocatorProxyDebugger.allocator, i];
                }

                return arr;
            }
        }

    }
    
    public sealed class ListProxyDebugger<T> where T : unmanaged {

        private List<T> arr;
        
        public ListProxyDebugger(List<T> arr) {

            this.arr = arr;

        }

        public int Capacity {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return 0;
                return this.arr.Capacity(StaticAllocatorProxyDebugger.allocator);
            }
        }

        public int Count {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return 0;
                return this.arr.Count;
            }
        }

        public T[] items {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return null;
                var arr = new T[this.arr.Count];
                for (int i = 0; i < this.arr.Count; ++i) {
                    arr[i] = this.arr[in StaticAllocatorProxyDebugger.allocator, i];
                }

                return arr;
            }
        }

    }

    public sealed class HashSetProxyDebugger<T> where T : unmanaged {

        private HashSet<T> arr;
        
        public HashSetProxyDebugger(HashSet<T> arr) {

            this.arr = arr;

        }

        public int Count {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return 0;
                return this.arr.Count;
            }
        }
        
        public MemArrayAllocator<int> buckets => this.arr.buckets;
        public MemArrayAllocator<HashSet<T>.Slot> slots => this.arr.slots;
        public int count => this.arr.count;
        public int version => this.arr.version;
        public int freeList => this.arr.freeList;
        public int lastIndex => this.arr.lastIndex;

        public T[] items {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return null;
                var arr = new T[this.arr.Count];
                var i = 0;
                var e = this.arr.GetEnumerator(in StaticAllocatorProxyDebugger.allocator);
                while (e.MoveNext() == true) {
                    arr[i++] = e.Current;
                }
                e.Dispose();
                
                return arr;
            }
        }

    }

    public sealed class DictionaryProxyDebugger<K, V> where K : unmanaged where V : unmanaged {

        private Dictionary<K, V> arr;
        
        public DictionaryProxyDebugger(Dictionary<K, V> arr) {

            this.arr = arr;

        }

        public int Count {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return 0;
                return this.arr.Count;
            }
        }

        public MemArrayAllocator<int> buckets => this.arr.buckets;
        public MemArrayAllocator<Dictionary<K, V>.Entry> entries => this.arr.entries;
        public int count => this.arr.count;
        public int version => this.arr.version;
        public int freeList => this.arr.freeList;
        public int freeCount => this.arr.freeCount;

        public System.Collections.Generic.KeyValuePair<K, V>[] items {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return null;
                var arr = new System.Collections.Generic.KeyValuePair<K, V>[this.arr.Count];
                var i = 0;
                var e = this.arr.GetEnumerator(in StaticAllocatorProxyDebugger.allocator);
                while (e.MoveNext() == true) {
                    arr[i++] = e.Current;
                }
                e.Dispose();
                
                return arr;
            }
        }

    }

    public sealed class StackProxyDebugger<T> where T : unmanaged {

        private Stack<T> arr;
        
        public StackProxyDebugger(Stack<T> arr) {

            this.arr = arr;

        }

        public int Count {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return 0;
                return this.arr.Count;
            }
        }

        public T[] items {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return null;
                var arr = new T[this.arr.Count];
                var i = 0;
                var e = this.arr.GetEnumerator(in StaticAllocatorProxyDebugger.allocator);
                while (e.MoveNext() == true) {
                    arr[i++] = e.Current;
                }
                e.Dispose();
                
                return arr;
            }
        }

    }

    public class QueueProxyDebugger<T> where T : unmanaged {

        private Queue<T> arr;
        
        public QueueProxyDebugger(Queue<T> arr) {

            this.arr = arr;

        }

        public int Count {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return 0;
                return this.arr.Count;
            }
        }

        public T[] items {
            get {
                if (StaticAllocatorProxyDebugger.allocator.isValid == false) return null;
                var arr = new T[this.arr.Count];
                var i = 0;
                var e = this.arr.GetEnumerator(in StaticAllocatorProxyDebugger.allocator);
                while (e.MoveNext() == true) {
                    arr[i++] = e.Current;
                }
                e.Dispose();
                
                return arr;
            }
        }

    }

}