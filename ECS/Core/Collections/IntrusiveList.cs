
namespace ME.ECS.Collections {

    public struct IntrusiveListNode : IStructComponent {

        public Entity next;
        public Entity prev;
        public Entity data;

    }

    public interface IIntrusiveList {

        int Count { get; }
        
        void Add(in Entity entityData);
        void AddFirst(in Entity entityData);
        bool Remove(in Entity entityData);
        bool Replace(in Entity entityData, int index);
        bool Insert(in Entity entityData, int onIndex);
        void Clear();
        bool RemoveAt(int index);
        void RemoveRange(int from, int to);
        Entity GetValue(int index);
        bool Contains(in Entity entityData);

        BufferArray<Entity> ToArray();
        IntrusiveList.IntrusiveListEnumerator GetEnumerator();

    }
    
    public struct IntrusiveList : IIntrusiveList {

        public struct IntrusiveListEnumerator : System.Collections.Generic.IEnumerator<Entity> {

            private Entity root;
            private Entity head;
            private int count;
            public Entity Current { get; private set; }

            public IntrusiveListEnumerator(IntrusiveList list) {

                this.root = list.root;
                this.head = list.root;
                this.count = list.count;
                this.Current = default;
                
            }

            public bool MoveNext() {

                if (this.head == Entity.Empty) return false;

                this.Current = this.head.GetData<IntrusiveListNode>().data;
                
                this.head = this.head.GetData<IntrusiveListNode>().next;
                if (this.head.IsAlive() == false) return false;
                return true;

            }

            public void Reset() {
                
                this.head = this.root;
                this.Current = default;
                
            }

            object System.Collections.IEnumerator.Current {
                get {
                    throw new AllocationException();
                }
            }

            public void Dispose() {

                this.head = default;
                this.count = default;
                this.Current = default;

            }

        }

        private Entity root;
        private Entity head;
        private int count;

        public int Count => this.count;

        public IntrusiveListEnumerator GetEnumerator() {

            return new IntrusiveListEnumerator(this);

        }

        /// <summary>
        /// Put entity data into array.
        /// </summary>
        /// <returns>Buffer array from pool</returns>
        public BufferArray<Entity> ToArray() {

            var arr = PoolArray<Entity>.Spawn(this.count);
            var i = 0;
            foreach (var entity in this) {
                
                arr.arr[i++] = entity;
                
            }

            return arr;

        }

        /// <summary>
        /// Find an element.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        public bool Contains(in Entity entityData) {
            
            var node = this.FindNode(in entityData);
            if (node.IsAlive() == true) {

                return true;

            }
            
            return false;

        }
        
        /// <summary>
        /// Clear the list.
        /// </summary>
        public void Clear() {

            while (this.root.IsAlive() == true) {

                var node = this.root;
                this.root = this.root.GetData<IntrusiveListNode>().next;
                node.Destroy();
                
            }

            this.root = Entity.Empty;
            this.head = Entity.Empty;
            this.count = 0;

        }
        
        /// <summary>
        /// Remove node at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returns TRUE if data was found</returns>
        public bool RemoveAt(int index) {

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                this.RemoveNode(in node);
                return true;

            }

            return false;

        }

        /// <summary>
        /// Remove nodes in range [from..to]
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void RemoveRange(int from, int to) {

            while (from <= to) {

                var node = this.FindNode(from);
                if (node.IsAlive() == true) {

                    this.RemoveNode(in node);

                } else {

                    ++from;

                }

            }

        }

        /// <summary>
        /// Get value by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Entity data. Entity.Empty if not found.</returns>
        public Entity GetValue(int index) {
            
            var node = this.FindNode(index);
            if (node.IsAlive() == true) {
                
                return node.GetData<IntrusiveListNode>().data;
                
            }
            
            return Entity.Empty;
            
        }
        
        /// <summary>
        /// Insert data at index onIndex.
        /// </summary>
        /// <param name="entityData"></param>
        /// <param name="onIndex"></param>
        /// <returns>Returns TRUE if successful</returns>
        public bool Insert(in Entity entityData, int onIndex) {

            if (this.count == 0) {
                
                this.Add(in entityData);
                return true;
                
            }

            if (onIndex > this.count) return false;

            if (onIndex == this.count) {
                
                this.Add(in entityData);
                return true;

            }

            var node = this.FindNode(onIndex);
            if (node.IsAlive() == true) {

                var newNode = IntrusiveList.CreateNode(in entityData);
                ref var newNodeLink = ref newNode.GetData<IntrusiveListNode>();
                
                var link = node.GetData<IntrusiveListNode>();
                if (link.prev.IsAlive() == true) {

                    link.prev.GetData<IntrusiveListNode>().next = newNode;
                    newNodeLink.prev = link.prev;

                } else {

                    this.root = newNode;

                }

                newNodeLink.next = node;
                link.prev = newNode;
                
                return true;

            }

            return false;
            
        }

        /// <summary>
        /// Replace data by index.
        /// </summary>
        /// <param name="entityData"></param>
        /// <param name="index"></param>
        /// <returns>Returns TRUE if data was found</returns>
        public bool Replace(in Entity entityData, int index) {

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                ref var link = ref node.GetData<IntrusiveListNode>();
                link.data = entityData;
                return true;

            }

            return false;

        }

        /// <summary>
        /// Remove data from list.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        public bool Remove(in Entity entityData) {

            var node = this.FindNode(in entityData);
            if (node.IsAlive() == true) {

                this.RemoveNode(in node);
                return true;

            }

            return false;
            
        }
        
        /// <summary>
        /// Add new data at the end of the list.
        /// </summary>
        /// <param name="entityData"></param>
        public void Add(in Entity entityData) {

            var node = IntrusiveList.CreateNode(in entityData);
            if (this.count == 0) {

                this.root = node;

            } else {

                ref var nodeLink = ref node.GetData<IntrusiveListNode>();
                ref var headLink = ref this.head.GetData<IntrusiveListNode>();
                headLink.next = node;
                nodeLink.prev = this.head;

            }

            this.head = node;
            ++this.count;

        }

        /// <summary>
        /// Add new data at the end of the list.
        /// </summary>
        /// <param name="entityData"></param>
        public void AddFirst(in Entity entityData) {

            this.Insert(in entityData, 0);

        }

        #region Helpers
        private Entity FindNode(in Entity entityData) {
            
            ref var root = ref this.root;
            var nextLink = root.GetData<IntrusiveListNode>();
            var result = nextLink.next;
            while (nextLink.data != entityData && nextLink.next.IsAlive() == true) {
                
                result = nextLink.next;
                nextLink = nextLink.next.GetData<IntrusiveListNode>();
                
            }

            return result;

        }

        private Entity FindNode(int index) {
            
            var idx = 0;
            ref var root = ref this.root;
            var nextLink = root.GetData<IntrusiveListNode>();
            var result = nextLink.next;
            while (idx != index && nextLink.next.IsAlive() == true) {
                
                result = nextLink.next;
                nextLink = nextLink.next.GetData<IntrusiveListNode>();
                ++idx;

            }

            return result;

        }

        private static Entity CreateNode(in Entity data) {
            
            var node = new Entity("IntrusiveListNode");
            ref var nodeLink = ref node.GetData<IntrusiveListNode>();
            nodeLink.data = data;

            return node;

        }
        
        private void RemoveNode(in Entity node) {
            
            var link = node.GetData<IntrusiveListNode>();
            if (link.prev.IsAlive() == true) {
                ref var prev = ref link.prev.GetData<IntrusiveListNode>();
                prev.next = link.next;
            }

            if (link.next.IsAlive() == true) {
                ref var next = ref link.next.GetData<IntrusiveListNode>();
                next.prev = link.prev;
            }

            if (node == this.root) this.root = link.next;
            if (node == this.head) this.head = link.prev;
            if (this.head == this.root && this.root == node) {
                    
                this.root = Entity.Empty;
                this.head = Entity.Empty;
                    
            }
                
            node.Destroy();
            
        }
        #endregion

    }

}
