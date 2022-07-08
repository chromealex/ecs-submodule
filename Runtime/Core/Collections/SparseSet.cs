namespace ME.ECS.Collections {

    public struct SparseSet<T> where T : struct {

        [ME.ECS.Serializer.SerializeField]
        private BufferArraySliced<T> dense;
        [ME.ECS.Serializer.SerializeField]
        private BufferArray<int> sparse;
        [ME.ECS.Serializer.SerializeField]
        private ListCopyable<int> freeIndexes;

        public bool isCreated;
        public int Length => this.sparse.Length;
        
        public ref T this[int index] => ref this.Get(index);

        public SparseSet(int length) {

            //this.Length = length;
            this.isCreated = true;
            this.dense = new BufferArraySliced<T>(PoolArray<T>.Spawn(length));
            this.sparse = default;
            this.freeIndexes = default;
            this.Validate(length);

        }

        public SparseSet(SparseSet<T> source, int length) {

            //this.Length = length;
            this.isCreated = true;
            this.dense = default;
            this.sparse = default;
            this.freeIndexes = default;
            ArrayUtils.Copy(source.dense, ref this.dense);
            ArrayUtils.Copy(source.sparse, ref this.sparse);

        }

        public T ReadDense(int sparseIndex) {

            return this.dense[sparseIndex];

        }
        
        public ref T GetDense(int sparseIndex) {

            return ref this.dense[sparseIndex];

        }

        public BufferArray<int> GetSparse() {

            return this.sparse;

        }

        public SparseSet<T> CopyFrom(in SparseSet<T> other) {

            this.isCreated = other.isCreated;
            //this.Length = other.Length;
            
            ArrayUtils.Copy(in other.dense, ref this.dense);
            ArrayUtils.Copy(in other.sparse, ref this.sparse);
            ArrayUtils.Copy(other.freeIndexes, ref this.freeIndexes);
            return this;

        }
        
        public SparseSet<T> CopyFrom<TCopy>(in SparseSet<T> other, TCopy copy) where TCopy : IArrayElementCopy<T> {

            this.isCreated = other.isCreated;
            //this.Length = other.Length;
            
            ArrayUtils.Copy(other.dense, ref this.dense, copy);
            ArrayUtils.Copy(in other.sparse, ref this.sparse);
            ArrayUtils.Copy(other.freeIndexes, ref this.freeIndexes);
            return this;

        }
        
        public SparseSet<T> Merge() {

            this.dense = this.dense.Merge();
            return this;

        }
        
        public void Validate(int capacity) {

            if (this.freeIndexes == null) this.freeIndexes = PoolListCopyable<int>.Spawn(10);
            ArrayUtils.Resize(capacity + 1, ref this.sparse);

        }

        public SparseSet<T> Dispose() {
            
            PoolListCopyable<int>.Recycle(ref this.freeIndexes);
            this.sparse = this.sparse.Dispose();
            this.dense = this.dense.Dispose();
            return this;

        }

        public void Set(int fromEntityId, int toEntityId, in T data) {

            for (int i = fromEntityId; i <= toEntityId; ++i) {
                this.Set(i, in data);
            }
            
        }

        public int Set(int entityId, in T data) {

            ref var idx = ref this.sparse[entityId];
            if (idx == 0) {
                if (this.freeIndexes.Count > 0) {
                    idx = this.freeIndexes.Pop();
                } else {
                    idx = this.dense.Length + 1;
                }
                //++this.Length;
            }
            this.dense = this.dense.Resize(idx, true, out _);
            this.dense[idx] = data;
            return idx;

        }

        public ref T Get(int entityId) {
            
            var idx = this.sparse[entityId];
            if (idx == 0) idx = this.Set(entityId, default);
            return ref this.dense[idx];

        }

        public void Remove(int entityId) {
            
            var idx = this.sparse[entityId];
            this.dense[idx] = default;
            this.freeIndexes.Add(idx);
            //--this.Length;
            
        }

        public void Remove(int entityId, int length) {

            for (int i = entityId; i < length; ++i) {
                this.Remove(i);
            }
            
        }

    }

}