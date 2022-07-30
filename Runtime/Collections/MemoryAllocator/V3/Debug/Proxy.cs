namespace ME.ECS.Collections.V3 {
    
    using Collections.MemoryAllocator;

    public static class StaticAllocatorProxy {

        public static MemoryAllocator defaultAllocator;
        public static ref MemoryAllocator allocator {
            get {
                if (Worlds.current == null || Worlds.current.currentState == null) {
                    return ref StaticAllocatorProxy.defaultAllocator;
                }

                return ref Worlds.current.currentState.allocator;
            }

        }

    }

    public class MemArrayAllocatorProxy<T> where T : unmanaged {

        private MemArrayAllocator<T> arr;
        
        public MemArrayAllocatorProxy(MemArrayAllocator<T> arr) {

            this.arr = arr;

        }

        public T[] items {
            get {
                var arr = new T[this.arr.Length(StaticAllocatorProxy.allocator)];
                for (int i = 0; i < this.arr.Length(StaticAllocatorProxy.allocator); ++i) {
                    arr[i] = this.arr[in StaticAllocatorProxy.allocator, i];
                }

                return arr;
            }
        }

    }

    public class ListProxy<T> where T : unmanaged {

        private List<T> arr;
        
        public ListProxy(List<T> arr) {

            this.arr = arr;

        }

        public int Capacity {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return 0;
                return this.arr.Capacity(StaticAllocatorProxy.allocator);
            }
        }

        public int Count {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return 0;
                return this.arr.Count(StaticAllocatorProxy.allocator);
            }
        }

        public T[] items {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return null;
                var arr = new T[this.arr.Count(StaticAllocatorProxy.allocator)];
                for (int i = 0; i < this.arr.Count(StaticAllocatorProxy.allocator); ++i) {
                    arr[i] = this.arr[in StaticAllocatorProxy.allocator, i];
                }

                return arr;
            }
        }

    }

    public class HashSetProxy<T> where T : unmanaged {

        private HashSet<T> arr;
        
        public HashSetProxy(HashSet<T> arr) {

            this.arr = arr;

        }

        public int Count {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return 0;
                return this.arr.Count(StaticAllocatorProxy.allocator);
            }
        }
        
        public MemArrayAllocator<int> buckets => this.arr.buckets(StaticAllocatorProxy.allocator);
        public MemArrayAllocator<HashSet<T>.Slot> slots => this.arr.slots(StaticAllocatorProxy.allocator);
        public int count => this.arr.count(StaticAllocatorProxy.allocator);
        public int version => this.arr.version(StaticAllocatorProxy.allocator);
        public int freeList => this.arr.freeList(StaticAllocatorProxy.allocator);
        public int lastIndex => this.arr.lastIndex(StaticAllocatorProxy.allocator);

        public T[] items {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return null;
                var arr = new T[this.arr.Count(StaticAllocatorProxy.allocator)];
                var i = 0;
                var e = this.arr.GetEnumerator(in StaticAllocatorProxy.allocator);
                while (e.MoveNext() == true) {
                    arr[i++] = e.Current;
                }
                e.Dispose();
                
                return arr;
            }
        }

    }

    public class DictionaryProxy<K, V> where K : unmanaged where V : unmanaged {

        private Dictionary<K, V> arr;
        
        public DictionaryProxy(Dictionary<K, V> arr) {

            this.arr = arr;

        }

        public int Count {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return 0;
                return this.arr.Count(StaticAllocatorProxy.allocator);
            }
        }

        public MemArrayAllocator<int> buckets => this.arr.buckets(StaticAllocatorProxy.allocator);
        public MemArrayAllocator<Dictionary<K, V>.Entry> entries => this.arr.entries(StaticAllocatorProxy.allocator);
        public int count => this.arr.count(StaticAllocatorProxy.allocator);
        public int version => this.arr.version(StaticAllocatorProxy.allocator);
        public int freeList => this.arr.freeList(StaticAllocatorProxy.allocator);
        public int freeCount => this.arr.freeCount(StaticAllocatorProxy.allocator);

        public System.Collections.Generic.KeyValuePair<K, V>[] items {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return null;
                var arr = new System.Collections.Generic.KeyValuePair<K, V>[this.arr.Count(StaticAllocatorProxy.allocator)];
                var i = 0;
                var e = this.arr.GetEnumerator(in StaticAllocatorProxy.allocator);
                while (e.MoveNext() == true) {
                    arr[i++] = e.Current;
                }
                e.Dispose();
                
                return arr;
            }
        }

    }

    public class StackProxy<T> where T : unmanaged {

        private Stack<T> arr;
        
        public StackProxy(Stack<T> arr) {

            this.arr = arr;

        }

        public int Count {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return 0;
                return this.arr.Count(StaticAllocatorProxy.allocator);
            }
        }

        public T[] items {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return null;
                var arr = new T[this.arr.Count(StaticAllocatorProxy.allocator)];
                var i = 0;
                var e = this.arr.GetEnumerator(in StaticAllocatorProxy.allocator);
                while (e.MoveNext() == true) {
                    arr[i++] = e.Current;
                }
                e.Dispose();
                
                return arr;
            }
        }

    }

    public class QueueProxy<T> where T : unmanaged {

        private Queue<T> arr;
        
        public QueueProxy(Queue<T> arr) {

            this.arr = arr;

        }

        public int Count {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return 0;
                return this.arr.Count(StaticAllocatorProxy.allocator);
            }
        }

        public T[] items {
            get {
                if (StaticAllocatorProxy.allocator.isValid == false) return null;
                var arr = new T[this.arr.Count(StaticAllocatorProxy.allocator)];
                var i = 0;
                var e = this.arr.GetEnumerator(in StaticAllocatorProxy.allocator);
                while (e.MoveNext() == true) {
                    arr[i++] = e.Current;
                }
                e.Dispose();
                
                return arr;
            }
        }

    }

}