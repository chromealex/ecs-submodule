
namespace ME.ECS.Collections {

    using System.Collections.Generic;
    
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
        bool RemoveAll(in Entity entityData);
        bool Replace(in Entity entityData, int index);
        bool Insert(in Entity entityData, int onIndex);
        void Clear(bool destroyData = false);
        bool RemoveAt(int index, bool destroyData = false);
        void RemoveRange(int from, int to, bool destroyData = false);
        Entity GetValue(int index);
        bool Contains(in Entity entityData);

        IEnumerator<Entity> GetRange(int from, int to);
        BufferArray<Entity> ToArray();
        IntrusiveList.IntrusiveListEnumerator GetEnumerator();

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct IntrusiveList : IIntrusiveList {

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public struct IntrusiveListEnumerator : System.Collections.Generic.IEnumerator<Entity> {

            private readonly Entity root;
            private Entity head;
            private readonly int count;
            public Entity Current { get; private set; }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public IntrusiveListEnumerator(IntrusiveList list) {

                this.root = list.root;
                this.head = list.root;
                this.count = list.count;
                this.Current = default;
                
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {

                if (this.head == Entity.Empty) return false;

                this.Current = this.head.GetData<IntrusiveListNode>().data;
                
                this.head = this.head.GetData<IntrusiveListNode>().next;
                if (this.head.IsAlive() == false) return false;
                return true;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

            }

        }

        private Entity root;
        private Entity head;
        private int count;

        public int Count => this.count;

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IntrusiveListEnumerator GetEnumerator() {

            return new IntrusiveListEnumerator(this);

        }

        /// <summary>
        /// Put entity data into array.
        /// </summary>
        /// <returns>Buffer array from pool</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Clear(bool destroyData = false) {

            while (this.root.IsAlive() == true) {

                var node = this.root;
                this.root = this.root.GetData<IntrusiveListNode>().next;
                if (destroyData == true) IntrusiveList.DestroyData(in node);
                node.Destroy();
                
            }

            this.root = Entity.Empty;
            this.head = Entity.Empty;
            this.count = 0;

        }

        /// <summary>
        /// Returns enumeration of nodes in range [from..to)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IEnumerator<Entity> GetRange(int from, int to) {

            while (from < to) {

                var node = this.FindNode(from);
                if (node.IsAlive() == true) {

                    yield return node.GetData<IntrusiveListNode>().data;

                } else {

                    ++from;

                }

            }

        }

        /// <summary>
        /// Remove node at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="destroyData">Destroy also entity data</param>
        /// <returns>Returns TRUE if data was found</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool RemoveAt(int index, bool destroyData = false) {

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                this.RemoveNode(in node, destroyData);
                return true;

            }

            return false;

        }

        /// <summary>
        /// Remove nodes in range [from..to)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="destroyData">Destroy also entity data</param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void RemoveRange(int from, int to, bool destroyData = false) {

            while (from < to) {

                var node = this.FindNode(from);
                if (node.IsAlive() == true) {

                    this.RemoveNode(in node, destroyData);

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
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

                ++this.count;
                
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
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Remove(in Entity entityData) {

            var node = this.FindNode(in entityData);
            if (node.IsAlive() == true) {

                this.RemoveNode(in node, destroyData: false);
                return true;

            }

            return false;
            
        }

        /// <summary>
        /// Remove all nodes data from list.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns TRUE if data was found</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool RemoveAll(in Entity entityData) {

            ref var root = ref this.root;
            var nextLink = root.GetData<IntrusiveListNode>();
            var hasAny = false;
            do {
            
                var result = nextLink.data;
                nextLink = nextLink.next.GetData<IntrusiveListNode>();
                if (result == entityData) {
                    
                    this.RemoveNode(nextLink.prev, destroyData: false);
                    hasAny = true;

                }

            } while (nextLink.next.IsAlive() == true);
            
            return hasAny;
            
        }

        /// <summary>
        /// Add new data at the end of the list.
        /// </summary>
        /// <param name="entityData"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AddFirst(in Entity entityData) {

            this.Insert(in entityData, 0);

        }

        #region Helpers
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private Entity FindNode(in Entity entityData) {
            
            if (this.count == 0) return Entity.Empty;
            
            var node = this.root;
            do {

                var nodeEntity = node;
                var nodeLink = node.GetData<IntrusiveListNode>();
                var nodeData = nodeLink.data;
                if (nodeLink.next.IsAlive() == true) {
                    node = nodeLink.next;
                }
                if (nodeData == entityData) {

                    return nodeEntity;

                }

            } while (node.IsAlive() == true);
            
            return Entity.Empty;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private Entity FindNode(int index) {
            
            var idx = 0;
            var node = this.root;
            do {

                var nodeEntity = node;
                var nodeLink = node.GetData<IntrusiveListNode>();
                if (nodeLink.next.IsAlive() == true) {
                    node = nodeLink.next;
                }
                if (idx == index) {

                    return nodeEntity;

                }

                ++idx;
                if (idx >= this.count) break;

            } while (node.IsAlive() == true);

            return Entity.Empty;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void RemoveNode(in Entity node, bool destroyData) {
            
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

            if (destroyData == true) IntrusiveList.DestroyData(in node);

            --this.count;
            node.Destroy();
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Entity CreateNode(in Entity data) {
            
            var node = new Entity("IntrusiveListNode");
            ref var nodeLink = ref node.GetData<IntrusiveListNode>();
            nodeLink.data = data;

            return node;

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void DestroyData(in Entity node) {
            
            var data = node.GetData<IntrusiveListNode>().data;
            if (data.IsAlive() == true) data.Destroy();

        }
        #endregion

    }

}
