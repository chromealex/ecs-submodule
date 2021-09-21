namespace ME.ECS.Collections {

    public interface IListCopyableBase {

        void Add(object obj);
        void Clear();

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public sealed class ListCopyable<T> : IPoolableSpawn, IPoolableRecycle, IListCopyableBase, System.Collections.Generic.IEnumerable<T> {

        private const int DefaultCapacity = 8;
        private static bool isValueType;
        
        [ME.ECS.Serializer.SerializeField]
        public BufferArray<T> innerArray;
        [ME.ECS.Serializer.SerializeField]
        public int Count { get; private set; } //Also the index of the next element to be added
        [ME.ECS.Serializer.SerializeField]
        public int Capacity = ListCopyable<T>.DefaultCapacity;

        public void CopyFrom(ListCopyable<T> other) {

            this.Count = other.Count;
            this.Capacity = other.Capacity;
            ArrayUtils.Copy(other.innerArray, ref this.innerArray);

        }

        public void OnSpawn() {

            this.innerArray = PoolArray<T>.Spawn(this.Capacity);
            this.Capacity = 0;
            this.Initialize();

        }

        public void OnRecycle() {

            PoolArray<T>.Recycle(ref this.innerArray);
            this.Capacity = 0;
            this.Count = 0;

        }
        
        public ListCopyable(BufferArray<T> startArray) {
            this.innerArray = startArray;
            this.Count = this.innerArray.Length;
            this.Capacity = this.innerArray.Length;
        }

        public ListCopyable(int startCapacity) {
            this.Capacity = startCapacity;
            this.innerArray = PoolArray<T>.Spawn(this.Capacity);

            this.Initialize();
        }

        public ListCopyable() {
            this.Initialize();
        }

        private void Initialize() {

            this.Count = 0;
            ListCopyable<T>.isValueType = typeof(T).IsValueType;
            
        }

        public int IndexOf(T item) {
            return this.innerArray.IndexOf(item);
        }

        public void RemoveRange(int index, int count) {
            
            if (index < 0 || index >= this.Count || count < 0) {
                throw new System.ArgumentOutOfRangeException();
            }

            var resetIndex = index;
            var copyIndex = index + count;
            var copyCount = this.Count - copyIndex;
            if (copyCount < 0) {
                throw new System.ArgumentOutOfRangeException();
            }

            if (copyCount > 0) {
                System.Array.Copy(this.innerArray.arr, copyIndex, this.innerArray.arr, index, copyCount);
                resetIndex += copyCount;
            }

            System.Array.Clear(this.innerArray.arr, resetIndex, this.Count - resetIndex);
            this.Count -= count;
            
        }

        public void TrimLeft(int count) {
            
            if (count > this.Count || count < 0) {
                throw new System.ArgumentOutOfRangeException();
            }

            this.Count -= count;
            if (this.Count > 0) {
                System.Array.Copy(this.innerArray.arr, count, this.innerArray.arr, 0, this.Count);
            }

            var clearCount = this.Count > count ? this.Count : count;
            System.Array.Clear(this.innerArray.arr, this.Count, clearCount);
            
        }

        public void TrimRight(int count) {
            
            if (count > this.Count || count < 0) {
                throw new System.ArgumentOutOfRangeException();
            }

            this.Count -= count;
            System.Array.Clear(this.innerArray.arr, this.Count, count);
            
        }

        void IListCopyableBase.Add(object obj) {
            
            this.Add((T)obj);
            
        }
        
        public void Add(T item) {
            this.EnsureCapacity(this.Count + 1);
            this.innerArray.arr[this.Count++] = item;

        }

        public void AddRange(BufferArray<T> items) {
            var arrayLength = items.Count;
            this.EnsureCapacity(this.Count + arrayLength + 1);
            System.Array.Copy(items.arr, 0, this.innerArray.arr, this.Count, arrayLength);
            this.Count += arrayLength;
            /*for (var i = 0; i < arrayLength; i++) {
                this.innerArray.arr[this.Count++] = items.arr[i];
            }*/
        }

        public void AddRange(ListCopyable<T> items) {
            var arrayLength = items.Count;
            this.EnsureCapacity(this.Count + arrayLength + 1);
            System.Array.Copy(items.innerArray.arr, 0, this.innerArray.arr, this.Count, arrayLength);
            this.Count += arrayLength;
            /*for (var i = 0; i < arrayLength; i++) {
                this.innerArray.arr[this.Count++] = items[i];
            }*/
        }

        public void AddRange(System.Collections.Generic.IList<T> items) {
            var arrayLength = items.Count;
            this.EnsureCapacity(this.Count + arrayLength + 1);
            for (var i = 0; i < arrayLength; i++) {
                this.innerArray.arr[this.Count++] = items[i];
            }
        }

        public void AddRange(System.Collections.Generic.IList<T> items, int startIndex, int count) {
            this.EnsureCapacity(this.Count + count + 1);
            for (var i = 0; i < count; i++) {
                this.innerArray.arr[this.Count++] = items[i + startIndex];
            }
        }

        public bool Remove(T item) {

            if (this.Count == 0) return false;
            var index = System.Array.IndexOf(this.innerArray.arr, item, 0, this.Count);
            if (index >= 0) {
                this.RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index) {
            
            this.Count--;
            if (index < this.Count) System.Array.Copy(this.innerArray.arr, index + 1, this.innerArray.arr, index, this.Count - index);
            this.innerArray.arr[this.Count] = default(T);

        }

        public void RemoveAtFast(int index) {
            
            this.Count--;
            this.innerArray.arr[index] = this.innerArray.arr[this.Count];
            this.innerArray.arr[this.Count] = default;
            
        }

        public BufferArray<T> GetArray() {

            return this.innerArray;

        }
        
        public T[] ToArray() {
            var retArray = new T[this.Count];
            System.Array.Copy(this.innerArray.arr, 0, retArray, 0, this.Count);
            return retArray;
        }

        public BufferArray<T> ToBufferArray() {
            var retArray = PoolArray<T>.Spawn(this.Count);
            System.Array.Copy(this.innerArray.arr, 0, retArray.arr, 0, this.Count);
            return retArray;
        }

        public bool Contains(T item) {
            if (this.Count == 0) return false;
            return System.Array.IndexOf(this.innerArray.arr, item, 0, this.Count) != -1;
        }

        public void Reverse() {
            
            System.Array.Reverse(this.innerArray.arr,0,this.Count);
            /*var highCount = this.Count / 2;
            var reverseCount = this.Count - 1;
            for (var i = 0; i < highCount; i++) {
                var swapItem = this.innerArray.arr[i];
                this.innerArray.arr[i] = this.innerArray.arr[reverseCount];
                this.innerArray.arr[reverseCount] = swapItem;

                --reverseCount;
            }*/
        }

        public void EnsureCapacity(int min) {
            if (this.Capacity < min) {
                this.Capacity *= 2;
                if (this.Capacity < min) {
                    this.Capacity = min;
                }

                ArrayUtils.Resize(this.Capacity - 1, ref this.innerArray, resizeWithOffset: false);

            }
        }

        public ref T this[int index] {
            get {
                return ref this.innerArray.arr[index];
            }
            /*set {
                this.innerArray.arr[index] = value;
            }*/
        }

        public void Clear() {
            
            if (ListCopyable<T>.isValueType == false) {

                System.Array.Clear(this.innerArray.arr, 0, this.Capacity);

            }

            this.Count = 0;

        }

        /// <summary>
        /// Marks elements for overwriting. Note: this list will still keep references to objects.
        /// </summary>
        public void FastClear() {
            this.Count = 0;
        }

        public void CopyTo(ListCopyable<T> target) {
            System.Array.Copy(this.innerArray.arr, 0, target.innerArray.arr, 0, this.Count);
            target.Count = this.Count;
            target.Capacity = this.Capacity;
        }

        public T[] TrimmedArray {
            get {
                var ret = new T[this.Count];
                System.Array.Copy(this.innerArray.arr, ret, this.Count);
                return ret;
            }
        }

        public override string ToString() {
            if (this.Count <= 0) {
                return base.ToString();
            }

            var output = string.Empty;
            for (var i = 0; i < this.Count - 1; i++) {
                output += this.innerArray.arr[i] + ", ";
            }

            return base.ToString() + ": " + output + this.innerArray.arr[this.Count - 1];
        }

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() {
            for (var i = 0; i < this.Count; i++) {
                yield return this.innerArray.arr[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            for (var i = 0; i < this.Count; i++) {
                yield return this.innerArray.arr[i];
            }
        }

    }

}