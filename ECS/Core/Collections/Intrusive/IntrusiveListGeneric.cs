
namespace ME.ECS.Collections {

    using System.Collections.Generic;
    
    public struct IntrusiveListGenericNode<T> : IStructComponent {

        public Entity next;
        public Entity prev;
        public T data;

    }

    public interface IIntrusiveListGeneric<T> where T : struct, System.IEquatable<T> {

        int Count { get; }
        
        void Add(in T entityData);
        void AddFirst(in T entityData);
        bool Remove(in T entityData);
        int RemoveAll(in T entityData);
        bool Replace(in T entityData, int index);
        bool Insert(in T entityData, int onIndex);
        void Clear();
        bool RemoveAt(int index);
        int RemoveRange(int from, int to);
        T GetValue(int index);
        bool Contains(in T entityData);
        T GetFirst();
        T GetLast();
        bool RemoveLast();
        bool RemoveFirst();

        IEnumerator<T> GetRange(int from, int to);
        BufferArray<T> ToArray();
        IntrusiveListGeneric<T>.Enumerator GetEnumerator();

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct IntrusiveListGeneric<T> : IIntrusiveListGeneric<T> where T : struct, System.IEquatable<T> {

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private readonly Entity root;
            private Entity head;
            public T Current { get; private set; }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public Enumerator(IntrusiveListGeneric<T> list) {

                this.root = list.root;
                this.head = list.root;
                this.Current = default;
                
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {

                if (this.head.IsAlive() == false) return false;

                this.Current = this.head.GetData<IntrusiveListGenericNode<T>>().data;
                
                this.head = this.head.GetData<IntrusiveListGenericNode<T>>().next;
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
        public Enumerator GetEnumerator() {

            return new Enumerator(this);

        }

        /// <summary>
        /// Put entity data into array.
        /// </summary>
        /// <returns>Buffer array from pool</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArray<T> ToArray() {

            var arr = PoolArray<T>.Spawn(this.count);
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
        public bool Contains(in T entityData) {
            
            if (this.count == 0) return false;

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
        public void Clear() {

            while (this.root.IsAlive() == true) {

                var node = this.root;
                this.root = this.root.GetData<IntrusiveListGenericNode<T>>().next;
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
        public IEnumerator<T> GetRange(int from, int to) {

            while (from < to) {

                var node = this.FindNode(from);
                if (node.IsAlive() == true) {

                    yield return node.GetData<IntrusiveListGenericNode<T>>().data;

                } else {

                    ++from;

                }

            }

        }

        /// <summary>
        /// Remove node at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returns TRUE if data was found</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool RemoveAt(int index) {

            if (this.count == 0) return false;

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                this.RemoveNode(in node);
                return true;

            }

            return false;

        }

        /// <summary>
        /// Remove nodes in range [from..to)
        /// </summary>
        /// <param name="from">Must be exists in list, could not be out of list range</param>
        /// <param name="to">May be out of list range, but greater than from</param>
        /// <returns>Returns count of removed elements</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int RemoveRange(int from, int to) {

            if (this.count == 0) return 0;

            var count = 0;
            var node = this.FindNode(from);
            if (node.IsAlive() == true) {

                while (from < to) {

                    if (node.IsAlive() == true) {
                        
                        var next = node.GetData<IntrusiveListGenericNode<T>>().next;
                        this.RemoveNode(in node);
                        node = next;
                        ++count;
                        ++from;
                        
                    } else {

                        break;

                    }

                }

            }
            
            return count;

        }

        /// <summary>
        /// Get value by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Entity data. Entity.Empty if not found.</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index) {
            
            if (this.count == 0) return default;

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {
                
                return node.GetData<IntrusiveListGenericNode<T>>().data;
                
            }
            
            return default;
            
        }
        
        /// <summary>
        /// Insert data at index onIndex.
        /// </summary>
        /// <param name="entityData"></param>
        /// <param name="onIndex"></param>
        /// <returns>Returns TRUE if successful</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Insert(in T entityData, int onIndex) {

            if (this.count == 0) {
                
                this.Add(in entityData);
                return true;
                
            }

            if (onIndex > this.count) return false;

            if (onIndex == this.count) {
                
                this.Add(in entityData);
                return true;

            }

            IntrusiveListGeneric<T>.InitializeComponents();

            var node = this.FindNode(onIndex);
            if (node.IsAlive() == true) {

                var newNode = IntrusiveListGeneric<T>.CreateNode(in entityData);
                ref var newNodeLink = ref newNode.GetData<IntrusiveListGenericNode<T>>();
                
                ref var link = ref node.GetData<IntrusiveListGenericNode<T>>();
                if (link.prev.IsAlive() == true) {

                    ref var prevLink = ref link.prev.GetData<IntrusiveListGenericNode<T>>();
                    prevLink.next = newNode;
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
        public bool Replace(in T entityData, int index) {

            if (this.count == 0) return false;

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                ref var link = ref node.GetData<IntrusiveListGenericNode<T>>();
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
        public bool Remove(in T entityData) {

            if (this.count == 0) return false;

            var node = this.FindNode(in entityData);
            if (node.IsAlive() == true) {

                this.RemoveNode(in node);
                return true;

            }

            return false;
            
        }

        /// <summary>
        /// Remove all nodes data from list.
        /// </summary>
        /// <param name="entityData"></param>
        /// <returns>Returns count of removed elements</returns>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int RemoveAll(in T entityData) {

            if (this.count == 0) return 0;

            var root = this.root;
            var count = 0;
            do {

                var nextLink = root.GetData<IntrusiveListGenericNode<T>>();
                if (entityData.Equals(nextLink.data) == true) {
                    
                    this.RemoveNode(root);
                    ++count;

                }

                root = nextLink.next;

            } while (root.IsAlive() == true);
            
            return count;
            
        }

        /// <summary>
        /// Add new data at the end of the list.
        /// </summary>
        /// <param name="entityData"></param>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Add(in T entityData) {

            IntrusiveListGeneric<T>.InitializeComponents();
            
            var node = IntrusiveListGeneric<T>.CreateNode(in entityData);
            if (this.count == 0) {

                this.root = node;

            } else {

                ref var nodeLink = ref node.GetData<IntrusiveListGenericNode<T>>();
                ref var headLink = ref this.head.GetData<IntrusiveListGenericNode<T>>();
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
        public void AddFirst(in T entityData) {

            this.Insert(in entityData, 0);

        }

        /// <summary>
        /// Returns first element.
        /// </summary>
        /// <returns>Returns instance, default if not found</returns>
        public T GetFirst() {

            if (this.root.IsAlive() == false) return default;
            
            return this.root.GetData<IntrusiveListGenericNode<T>>().data;

        }

        /// <summary>
        /// Returns last element.
        /// </summary>
        /// <returns>Returns instance, default if not found</returns>
        public T GetLast() {

            if (this.head.IsAlive() == false) return default;
            
            return this.head.GetData<IntrusiveListGenericNode<T>>().data;

        }

        /// <summary>
        /// Returns last element.
        /// </summary>
        /// <returns>Returns TRUE on success</returns>
        public bool RemoveLast() {

            if (this.head.IsAlive() == false) return false;

            this.RemoveNode(in this.head);
            return true;

        }

        /// <summary>
        /// Returns last element.
        /// </summary>
        /// <returns>Returns TRUE on success</returns>
        public bool RemoveFirst() {

            if (this.head.IsAlive() == false) return false;
            
            this.RemoveNode(in this.root);
            return true;

        }

        #region Helpers
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private Entity FindNode(in T entityData) {
            
            if (this.count == 0) return Entity.Empty;
            
            var node = this.root;
            do {

                var nodeEntity = node;
                var nodeLink = node.GetData<IntrusiveListGenericNode<T>>();
                var nodeData = nodeLink.data;
                node = nodeLink.next;
                if (entityData.Equals(nodeData) == true) {
                
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
                var nodeLink = node.GetData<IntrusiveListGenericNode<T>>();
                node = nodeLink.next;
                if (idx == index) {

                    return nodeEntity;

                }

                ++idx;
                if (idx >= this.count) break;

            } while (node.IsAlive() == true);

            return Entity.Empty;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void RemoveNode(in Entity node) {

            var nodeToDestroy = node;
            var link = node.GetData<IntrusiveListGenericNode<T>>();
            if (link.prev.IsAlive() == true) {
                ref var prev = ref link.prev.GetData<IntrusiveListGenericNode<T>>();
                prev.next = link.next;
            }

            if (link.next.IsAlive() == true) {
                ref var next = ref link.next.GetData<IntrusiveListGenericNode<T>>();
                next.prev = link.prev;
            }

            if (nodeToDestroy == this.root) this.root = link.next;
            if (nodeToDestroy == this.head) this.head = link.prev;
            if (this.head == this.root && this.root == nodeToDestroy) {
                    
                this.root = Entity.Empty;
                this.head = Entity.Empty;
                    
            }

            --this.count;
            
            nodeToDestroy.Destroy();
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Entity CreateNode(in T data) {
            
            var node = new Entity("IntrusiveListGenericNode<T>");
            node.ValidateData<IntrusiveListGenericNode<T>>();
            ref var nodeLink = ref node.GetData<IntrusiveListGenericNode<T>>();
            nodeLink.data = data;
            
            return node;

        }

        private static void InitializeComponents() {

            WorldUtilities.InitComponentTypeId<IntrusiveListGenericNode<T>>();
            ComponentInitializer.Init(ref Worlds.currentWorld.GetStructComponents());

        }
        
        private static class ComponentInitializer {
    
            public static void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
                structComponentsContainer.Validate<IntrusiveListGenericNode<T>>(false);
                
            }

        }
        #endregion

    }

}
