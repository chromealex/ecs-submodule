namespace ME.ECS.Collections {

    using System.Collections.Generic;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public class PriorityQueue<T> {

        private struct Node {

            public float priority;
            public T @object;

        }

        private List<Node> queue;
        private int heapSize = -1;
        public bool isMinPriorityQueue;
        private IEqualityComparer<T> comparer;

        public int Count {
            get {
                return this.queue.Count;
            }
        }

        /// <summary>
        /// If min queue or max queue
        /// </summary>
        /// <param name="isMinPriorityQueue"></param>
        public PriorityQueue(int capacity = 4, bool isMinPriorityQueue = false) {
            this.isMinPriorityQueue = isMinPriorityQueue;
            this.queue = new List<Node>(capacity);
            this.comparer = EqualityComparer<T>.Default;
        }

        public void Clear() {
            
            this.queue.Clear();
            this.heapSize = -1;

        }

        /// <summary>
        /// Enqueue the object with priority
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="obj"></param>
        public void Enqueue(float priority, T obj) {
            var node = new Node() { priority = priority, @object = obj };
            this.queue.Add(node);
            this.heapSize++;
            //Maintaining heap
            if (this.isMinPriorityQueue) {
                this.BuildHeapMin(this.heapSize);
            } else {
                this.BuildHeapMax(this.heapSize);
            }
        }

        /// <summary>
        /// Dequeue the object
        /// </summary>
        /// <returns></returns>
        public T Dequeue() {
            if (this.heapSize > -1) {
                var returnVal = this.queue[0].@object;
                this.queue[0] = this.queue[this.heapSize];
                this.queue.RemoveAt(this.heapSize);
                this.heapSize--;
                //Maintaining lowest or highest at root based on min or max queue
                if (this.isMinPriorityQueue) {
                    this.MinHeapify(0);
                } else {
                    this.MaxHeapify(0);
                }

                return returnVal;
            } else {
                throw new System.Exception("Queue is empty");
            }
        }

        /// <summary>
        /// Updating the priority of specific object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="priority"></param>
        public void UpdatePriority(T obj, int priority) {
            var i = 0;
            for (; i <= this.heapSize; i++) {
                var node = this.queue[i];
                if (this.comparer.Equals(node.@object, obj) == true) {
                    node.priority = priority;
                    if (this.isMinPriorityQueue) {
                        this.BuildHeapMin(i);
                        this.MinHeapify(i);
                    } else {
                        this.BuildHeapMax(i);
                        this.MaxHeapify(i);
                    }
                }
            }
        }

        /// <summary>
        /// Maintain max heap
        /// </summary>
        /// <param name="i"></param>
        private void BuildHeapMax(int i) {
            while (i >= 0 && this.queue[(i - 1) / 2].priority < this.queue[i].priority) {
                this.Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        /// <summary>
        /// Maintain min heap
        /// </summary>
        /// <param name="i"></param>
        private void BuildHeapMin(int i) {
            while (i >= 0 && this.queue[(i - 1) / 2].priority > this.queue[i].priority) {
                this.Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        private void MaxHeapify(int i) {
            var left = this.ChildL(i);
            var right = this.ChildR(i);

            var heighst = i;

            if (left <= this.heapSize && this.queue[heighst].priority < this.queue[left].priority) {
                heighst = left;
            }

            if (right <= this.heapSize && this.queue[heighst].priority < this.queue[right].priority) {
                heighst = right;
            }

            if (heighst != i) {
                this.Swap(heighst, i);
                this.MaxHeapify(heighst);
            }
        }

        private void MinHeapify(int i) {
            var left = this.ChildL(i);
            var right = this.ChildR(i);

            var lowest = i;

            if (left <= this.heapSize && this.queue[lowest].priority > this.queue[left].priority) {
                lowest = left;
            }

            if (right <= this.heapSize && this.queue[lowest].priority > this.queue[right].priority) {
                lowest = right;
            }

            if (lowest != i) {
                this.Swap(lowest, i);
                this.MinHeapify(lowest);
            }
        }

        private void Swap(int i, int j) {
            var temp = this.queue[i];
            this.queue[i] = this.queue[j];
            this.queue[j] = temp;
        }

        private int ChildL(int i) {
            return i * 2 + 1;
        }

        private int ChildR(int i) {
            return i * 2 + 2;
        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public struct PriorityQueueNative<T> where T : unmanaged {

        private struct Node {

            public pfloat priority;
            public T @object;

        }

        private Unity.Collections.NativeList<Node> queue;
        private int heapSize;
        public byte isMinPriorityQueue;

        public int Count {
            get {
                return this.queue.Length;
            }
        }

        /// <summary>
        /// If min queue or max queue
        /// </summary>
        /// <param name="isMinPriorityQueue"></param>
        public PriorityQueueNative(Unity.Collections.Allocator allocator, int capacity = 4, bool isMinPriorityQueue = false) {
            this.heapSize = -1;
            this.isMinPriorityQueue = isMinPriorityQueue == true ? (byte)1 : (byte)0;
            this.queue = new Unity.Collections.NativeList<Node>(capacity, allocator);
        }

        public void Dispose() {
            
            this.queue.Dispose();
            
        }

        public void Clear() {
            
            this.queue.Clear();
            this.heapSize = -1;

        }

        /// <summary>
        /// Enqueue the object with priority
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="obj"></param>
        public void Enqueue(float priority, T obj) {
            var node = new Node() { priority = priority, @object = obj };
            this.queue.Add(node);
            this.heapSize++;
            //Maintaining heap
            if (this.isMinPriorityQueue == 1) {
                this.BuildHeapMin(this.heapSize);
            } else {
                this.BuildHeapMax(this.heapSize);
            }
        }

        /// <summary>
        /// Dequeue the object
        /// </summary>
        /// <returns></returns>
        public T Dequeue() {
            if (this.heapSize > -1) {
                var returnVal = this.queue[0].@object;
                this.queue[0] = this.queue[this.heapSize];
                this.queue.RemoveAt(this.heapSize);
                this.heapSize--;
                //Maintaining lowest or highest at root based on min or max queue
                if (this.isMinPriorityQueue == 1) {
                    this.MinHeapify(0);
                } else {
                    this.MaxHeapify(0);
                }

                return returnVal;
            } else {
                this.EmptyException();
                return default;
            }
        }

        [System.Diagnostics.ConditionalAttribute("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void EmptyException() {
            throw new System.Exception("Queue is empty");
        }

        /// <summary>
        /// Updating the priority of specific object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="priority"></param>
        public void UpdatePriority(T obj, int priority) {
            var i = 0;
            var comparer = EqualityComparer<T>.Default;
            for (; i <= this.heapSize; i++) {
                var node = this.queue[i];
                if (comparer.Equals(node.@object, obj) == true) {
                    node.priority = priority;
                    if (this.isMinPriorityQueue == 1) {
                        this.BuildHeapMin(i);
                        this.MinHeapify(i);
                    } else {
                        this.BuildHeapMax(i);
                        this.MaxHeapify(i);
                    }
                }
            }
        }

        /// <summary>
        /// Maintain max heap
        /// </summary>
        /// <param name="i"></param>
        private void BuildHeapMax(int i) {
            while (i >= 0 && this.queue[(i - 1) / 2].priority < this.queue[i].priority) {
                this.Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        /// <summary>
        /// Maintain min heap
        /// </summary>
        /// <param name="i"></param>
        private void BuildHeapMin(int i) {
            while (i >= 0 && this.queue[(i - 1) / 2].priority > this.queue[i].priority) {
                this.Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        private void MaxHeapify(int i) {
            var left = this.ChildL(i);
            var right = this.ChildR(i);

            var heighst = i;

            if (left <= this.heapSize && this.queue[heighst].priority < this.queue[left].priority) {
                heighst = left;
            }

            if (right <= this.heapSize && this.queue[heighst].priority < this.queue[right].priority) {
                heighst = right;
            }

            if (heighst != i) {
                this.Swap(heighst, i);
                this.MaxHeapify(heighst);
            }
        }

        private void MinHeapify(int i) {
            var left = this.ChildL(i);
            var right = this.ChildR(i);

            var lowest = i;

            if (left <= this.heapSize && this.queue[lowest].priority > this.queue[left].priority) {
                lowest = left;
            }

            if (right <= this.heapSize && this.queue[lowest].priority > this.queue[right].priority) {
                lowest = right;
            }

            if (lowest != i) {
                this.Swap(lowest, i);
                this.MinHeapify(lowest);
            }
        }

        private void Swap(int i, int j) {
            var temp = this.queue[i];
            this.queue[i] = this.queue[j];
            this.queue[j] = temp;
        }

        private int ChildL(int i) {
            return i * 2 + 1;
        }

        private int ChildR(int i) {
            return i * 2 + 2;
        }

    }

}