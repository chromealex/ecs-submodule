#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS.Collections {

    using System.Collections.Generic;

    public interface IIntrusiveList {

        int Count { get; }

        void Add(in Entity entityData);
        void AddFirst(in Entity entityData);
        bool Remove(in Entity entityData);
        int RemoveAll(in Entity entityData);
        bool Replace(in Entity entityData, int index);
        bool Insert(in Entity entityData, int onIndex);
        void Clear(bool destroyData = false);
        bool RemoveAt(int index, bool destroyData = false);
        int RemoveRange(int from, int to, bool destroyData = false);
        Entity GetValue(int index);
        bool Contains(in Entity entityData);
        Entity GetFirst();
        Entity GetLast();
        bool RemoveLast(bool destroyData = false);
        bool RemoveFirst(bool destroyData = false);

        IEnumerator<Entity> GetRange(int from, int to);
        void GetRange(int from, int to, ListCopyable<Entity> results);
        BufferArray<Entity> ToArray();
        IntrusiveList.Enumerator GetEnumerator();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public struct IntrusiveList : IIntrusiveList {

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public struct Enumerator : System.Collections.Generic.IEnumerator<Entity> {

            private readonly Entity root;
            private Entity head;
            private int id;
            
            Entity System.Collections.Generic.IEnumerator<Entity>.Current => Worlds.currentWorld.ReadData<IntrusiveListNode>(Worlds.currentWorld.GetEntityById(this.id)).data;
            public ref readonly Entity Current => ref Worlds.currentWorld.ReadData<IntrusiveListNode>(Worlds.currentWorld.GetEntityById(this.id)).data;

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public Enumerator(IntrusiveList list) {

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

                this.head = this.head.Read<IntrusiveListNode>().next;
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

        [ME.ECS.Serializer.SerializeFieldAttribute]
        private Entity data;

        private Entity root {
            readonly get {
                if (this.data == Entity.Null) return Entity.Null;
                return this.data.Read<IntrusiveData>().root;
            }
            set {
                this.ValidateData();
                this.data.Get<IntrusiveData>().root = value;
            }
        }

        private Entity head {
            readonly get {
                if (this.data == Entity.Null) return Entity.Null;
                return this.data.Read<IntrusiveData>().head;
            }
            set {
                this.ValidateData();
                this.data.Get<IntrusiveData>().head = value;
            }
        }

        private int count {
            readonly get {
                if (this.data == Entity.Null) return 0;
                return this.data.Read<IntrusiveData>().count;   
            }
            set {
                this.ValidateData();
                this.data.Get<IntrusiveData>().count = value;
            }
        }

        public readonly int Count => this.count;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void ValidateData() {

            if (this.data == Entity.Null) {
                WorldUtilities.InitComponentTypeId<ME.ECS.Collections.IntrusiveData>(false, isBlittable: true);
                this.data = new Entity(EntityFlag.None);
                this.data.ValidateDataUnmanaged<IntrusiveData>();
                this.data.Set(new IntrusiveData());
            }
            
        }

        public void Dispose() {

            this.Clear();
            if (this.data != Entity.Null) this.data.Destroy();
            this.data = Entity.Null;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly Enumerator GetEnumerator() {

            return new Enumerator(this);

        }

        /// <summary>
        /// Put entity data into array.
        /// </summary>
        /// <returns>Buffer array from pool</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly BufferArray<Entity> ToArray() {

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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly bool Contains(in Entity entityData) {

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
        public void Clear(bool destroyData = false) {

            while (this.root.IsAlive() == true) {

                var node = this.root;
                this.root = this.root.Read<IntrusiveListNode>().next;
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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly IEnumerator<Entity> GetRange(int from, int to) {

            while (from < to) {

                var node = this.FindNode(from);
                if (node.IsAlive() == true) {

                    yield return node.Read<IntrusiveListNode>().data;

                } else {

                    ++from;

                }

            }

        }

        /// <summary>
        /// Returns enumeration of nodes in range [from..to)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="results"></param>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public readonly void GetRange(int from, int to, ListCopyable<Entity> results) {

            while (from < to) {

                var node = this.FindNode(from);
                if (node.IsAlive() == true) {

                    results.Add(node.Get<IntrusiveListNode>().data);

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
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool RemoveAt(int index, bool destroyData = false) {

            if (this.count == 0) return false;

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
        /// <param name="from">Must be exists in list, could not be out of list range</param>
        /// <param name="to">May be out of list range, but greater than from</param>
        /// <param name="destroyData">Destroy also entity data</param>
        /// <returns>Returns count of removed elements</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int RemoveRange(int from, int to, bool destroyData = false) {

            if (this.count == 0) return 0;

            var count = 0;
            var node = this.FindNode(from);
            if (node.IsAlive() == true) {

                while (from < to) {

                    if (node.IsAlive() == true) {

                        var next = node.Read<IntrusiveListNode>().next;
                        this.RemoveNode(in node, destroyData);
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
        public readonly Entity GetValue(int index) {

            if (this.count == 0) return Entity.Empty;

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                return node.Read<IntrusiveListNode>().data;

            }

            return Entity.Empty;

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
                ref var newNodeLink = ref newNode.Get<IntrusiveListNode>();

                var link = node.Read<IntrusiveListNode>();
                if (link.prev.IsAlive() == true) {

                    link.prev.Get<IntrusiveListNode>().next = newNode;
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
        public bool Replace(in Entity entityData, int index) {

            if (this.count == 0) return false;

            var node = this.FindNode(index);
            if (node.IsAlive() == true) {

                ref var link = ref node.Get<IntrusiveListNode>();
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
        public bool Remove(in Entity entityData) {

            if (this.count == 0) return false;

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
        /// <returns>Returns count of removed elements</returns>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int RemoveAll(in Entity entityData) {

            if (this.count == 0) return 0;

            var root = this.root;
            var count = 0;
            do {

                var nextLink = root.Read<IntrusiveListNode>();
                if (nextLink.data == entityData) {

                    this.RemoveNode(root, destroyData: false);
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
        public void Add(in Entity entityData) {

            var node = IntrusiveList.CreateNode(in entityData);
            if (this.count == 0) {

                this.root = node;

            } else {

                ref var nodeLink = ref node.Get<IntrusiveListNode>();
                ref var headLink = ref this.head.Get<IntrusiveListNode>();
                headLink.next = node;
                nodeLink.prev = this.head;

            }

            this.head = node;
            ++this.count;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add(in Entity entityData, out Entity node) {

            node = IntrusiveList.CreateNode(in entityData);
            if (this.count == 0) {

                this.root = node;

            } else {

                ref var nodeLink = ref node.Get<IntrusiveListNode>();
                ref var headLink = ref this.head.Get<IntrusiveListNode>();
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
        public void AddFirst(in Entity entityData) {

            this.Insert(in entityData, 0);

        }

        /// <summary>
        /// Returns first element.
        /// </summary>
        /// <returns>Returns instance, default if not found</returns>
        public readonly Entity GetFirst() {

            if (this.root.IsAlive() == false) return Entity.Empty;

            return this.root.Read<IntrusiveListNode>().data;

        }

        /// <summary>
        /// Returns last element.
        /// </summary>
        /// <returns>Returns instance, default if not found</returns>
        public readonly Entity GetLast() {

            if (this.head.IsAlive() == false) return Entity.Empty;

            return this.head.Read<IntrusiveListNode>().data;

        }

        /// <summary>
        /// Returns last element.
        /// </summary>
        /// <returns>Returns TRUE on success</returns>
        public bool RemoveLast(bool destroyData = false) {

            if (this.head.IsAlive() == false) return false;

            this.RemoveNode(this.head, destroyData);
            return true;

        }

        /// <summary>
        /// Returns last element.
        /// </summary>
        /// <returns>Returns TRUE on success</returns>
        public bool RemoveFirst(bool destroyData = false) {

            if (this.head.IsAlive() == false) return false;

            this.RemoveNode(this.root, destroyData);
            return true;

        }

        #region Helpers
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private readonly Entity FindNode(in Entity entityData) {

            if (this.count == 0) return Entity.Empty;

            var node = this.root;
            do {

                var nodeEntity = node;
                ref readonly var nodeLink = ref node.Read<IntrusiveListNode>();
                var nodeData = nodeLink.data;
                node = nodeLink.next;
                if (nodeData == entityData) {

                    return nodeEntity;

                }

            } while (node.IsAlive() == true);

            return Entity.Empty;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private readonly Entity FindNode(int index) {

            var idx = 0;
            var node = this.root;
            do {

                var nodeEntity = node;
                ref readonly var nodeLink = ref node.Read<IntrusiveListNode>();
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
        private void RemoveNode(in Entity node, bool destroyData) {

            var link = node.Read<IntrusiveListNode>();
            if (link.prev.IsAlive() == true) {
                link.prev.Get<IntrusiveListNode>().next = link.next;
            }

            if (link.next.IsAlive() == true) {
                link.next.Get<IntrusiveListNode>().prev = link.prev;
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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static Entity CreateNode(in Entity data) {

            var node = new Entity(EntityFlag.None);
            node.Get<IntrusiveListNode>().data = data;
            return node;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static void DestroyData(in Entity node) {

            var data = node.Read<IntrusiveListNode>().data;
            if (data.IsAlive() == true) data.Destroy();

        }
        #endregion

    }

}