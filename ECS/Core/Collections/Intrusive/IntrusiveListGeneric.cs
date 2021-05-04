#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

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
            private int id;

            T System.Collections.Generic.IEnumerator<T>.Current => Worlds.currentWorld.GetData<IntrusiveListGenericNode<T>>(Worlds.currentWorld.GetEntityById(this.id)).data;
            public ref T Current => ref Worlds.currentWorld.GetData<IntrusiveListGenericNode<T>>(Worlds.currentWorld.GetEntityById(this.id)).data;

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public Enumerator(IntrusiveListGeneric<T> list) {

                this.root = list.root;
                this.head = list.root;
                this.id = -1;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {

                if (this.head.IsAlive() == false) return false;

                this.id = this.head.id;
                this.head = this.head.Get<IntrusiveListGenericNode<T>>().next;
                return true;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Reset() {

                this.head = this.root;
                this.id = -1;

            }

            object System.Collections.IEnumerator.Current {
                get {
                    throw new AllocationException();
                }
            }

            public void Dispose() { }

        }

        [ME.ECS.Serializer.SerializeField]
        private Entity root;
        [ME.ECS.Serializer.SerializeField]
        private Entity head;
        [ME.ECS.Serializer.SerializeField]
        private int count;

        public int Count => this.count;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Enumerator GetEnumerator() {

            return new Enumerator(this);

        }

        /// <summary>
        /// Put entity data into array.
        /// </summary>
        /// <returns>Buffer array from pool</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clear() {

            while (this.root.IsAlive() == true) {

                var node = this.root;
                this.root = this.root.Get<IntrusiveListGenericNode<T>>().next;
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public IEnumerator<T> GetRange(int from, int to) {

            while (from < to) {

                var node = this.FindNode(from);
                if (node.IsAlive() == true) {

                    yield return node.Get<IntrusiveListGenericNode<T>>().data;

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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int RemoveRange(int from, int to) {

            if (this.count == 0) return 0;

            var count = 0;
            var node = this.FindNode(from);
            if (node.IsAlive() == true) {

                while (from < to) {

                    if (node.IsAlive() == true) {

                        var next = node.Get<IntrusiveListGenericNode<T>>().next;
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public T GetValue(int index) {

            if (this.count == 0) return default;

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                return node.Get<IntrusiveListGenericNode<T>>().data;

            }

            return default;

        }

        /// <summary>
        /// Insert data at index onIndex.
        /// </summary>
        /// <param name="entityData"></param>
        /// <param name="onIndex"></param>
        /// <returns>Returns TRUE if successful</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
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
                ref var newNodeLink = ref newNode.Get<IntrusiveListGenericNode<T>>();

                ref var link = ref node.Get<IntrusiveListGenericNode<T>>();
                if (link.prev.IsAlive() == true) {

                    ref var prevLink = ref link.prev.Get<IntrusiveListGenericNode<T>>();
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Replace(in T entityData, int index) {

            if (this.count == 0) return false;

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                ref var link = ref node.Get<IntrusiveListGenericNode<T>>();
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int RemoveAll(in T entityData) {

            if (this.count == 0) return 0;

            var root = this.root;
            var count = 0;
            do {

                var nextLink = root.Get<IntrusiveListGenericNode<T>>();
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add(in T entityData) {

            IntrusiveListGeneric<T>.InitializeComponents();

            var node = IntrusiveListGeneric<T>.CreateNode(in entityData);
            if (this.count == 0) {

                this.root = node;

            } else {

                ref var nodeLink = ref node.Get<IntrusiveListGenericNode<T>>();
                ref var headLink = ref this.head.Get<IntrusiveListGenericNode<T>>();
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void AddFirst(in T entityData) {

            this.Insert(in entityData, 0);

        }

        /// <summary>
        /// Returns first element.
        /// </summary>
        /// <returns>Returns instance, default if not found</returns>
        public T GetFirst() {

            if (this.root.IsAlive() == false) return default;

            return this.root.Get<IntrusiveListGenericNode<T>>().data;

        }

        /// <summary>
        /// Returns last element.
        /// </summary>
        /// <returns>Returns instance, default if not found</returns>
        public T GetLast() {

            if (this.head.IsAlive() == false) return default;

            return this.head.Get<IntrusiveListGenericNode<T>>().data;

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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private Entity FindNode(in T entityData) {

            if (this.count == 0) return Entity.Empty;

            var node = this.root;
            do {

                var nodeEntity = node;
                var nodeLink = node.Get<IntrusiveListGenericNode<T>>();
                var nodeData = nodeLink.data;
                node = nodeLink.next;
                if (entityData.Equals(nodeData) == true) {

                    return nodeEntity;

                }

            } while (node.IsAlive() == true);

            return Entity.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private Entity FindNode(int index) {

            var idx = 0;
            var node = this.root;
            do {

                var nodeEntity = node;
                var nodeLink = node.Get<IntrusiveListGenericNode<T>>();
                node = nodeLink.next;
                if (idx == index) {

                    return nodeEntity;

                }

                ++idx;
                if (idx >= this.count) break;

            } while (node.IsAlive() == true);

            return Entity.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void RemoveNode(in Entity node) {

            var nodeToDestroy = node;
            var link = node.Get<IntrusiveListGenericNode<T>>();
            if (link.prev.IsAlive() == true) {
                ref var prev = ref link.prev.Get<IntrusiveListGenericNode<T>>();
                prev.next = link.next;
            }

            if (link.next.IsAlive() == true) {
                ref var next = ref link.next.Get<IntrusiveListGenericNode<T>>();
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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static Entity CreateNode(in T data) {

            var node = new Entity("IntrusiveListGenericNode<T>");
            node.ValidateData<IntrusiveListGenericNode<T>>();
            ref var nodeLink = ref node.Get<IntrusiveListGenericNode<T>>();
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