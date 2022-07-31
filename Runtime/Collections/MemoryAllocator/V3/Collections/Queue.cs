namespace ME.ECS.Collections.MemoryAllocator {

    using Collections.V3;
    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(QueueProxy<>))]
    public struct Queue<T>: System.Collections.Generic.IEnumerable<T> where T : unmanaged {

        private struct InternalData {

            public MemArrayAllocator<T> array;
            public int head; // First valid element in the queue
            public int tail; // Last valid element in the queue
            public int size; // Number of elements.
            public int version;

            public void Dispose(ref MemoryAllocator allocator) {
                
                this.array.Dispose(ref allocator);
                this = default;

            }

        }
        
        private const int MINIMUM_GROW = 4;
        private const int GROW_FACTOR = 200;

        [ME.ECS.Serializer.SerializeField]
        private readonly MemPtr ptr;

        private readonly ref MemArrayAllocator<T> array(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).array;
        private readonly ref int head(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).head;
        private readonly ref int tail(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).tail;
        private readonly ref int size(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).size;
        private readonly ref int version(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).version;
        public readonly bool isCreated => this.ptr != 0;

        public readonly int Count(in MemoryAllocator allocator) => this.size(in allocator);

        public Queue(ref MemoryAllocator allocator, int capacity) {
            this.ptr = allocator.AllocData<InternalData>(default);
            this.array(in allocator) = new MemArrayAllocator<T>(ref allocator, capacity);
            this.head(in allocator) = 0;
            this.tail(in allocator) = 0;
            this.size(in allocator) = 0;
            this.version(in allocator) = 0;
        }

        public void Dispose(ref MemoryAllocator allocator) {
            
            allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;
            
        }

        public void Clear(in MemoryAllocator allocator) {
            this.head(in allocator) = 0;
            this.tail(in allocator) = 0;
            this.size(in allocator) = 0;
            this.version(in allocator)++;
        }

        public void Enqueue(ref MemoryAllocator allocator, T item) {
            if (this.size(in allocator) == this.array(in allocator).Length(in allocator)) {
                var newCapacity = (int)((long)this.array(in allocator).Length(in allocator) * (long)Queue<T>.GROW_FACTOR / 100);
                if (newCapacity < this.array(in allocator).Length(in allocator) + Queue<T>.MINIMUM_GROW) {
                    newCapacity = this.array(in allocator).Length(in allocator) + Queue<T>.MINIMUM_GROW;
                }

                this.SetCapacity(ref allocator, newCapacity);
            }

            this.array(in allocator)[in allocator, this.tail(in allocator)] = item;
            this.tail(in allocator) = (this.tail(in allocator) + 1) % this.array(in allocator).Length(in allocator);
            this.size(in allocator)++;
            this.version(in allocator)++;
        }

        public readonly Enumerator GetEnumerator(State state) {
            return new Enumerator(this, state);
        }
        
        
        public readonly System.Collections.Generic.IEnumerator<T> GetEnumerator() {
            
            return GetEnumerator(Worlds.current.GetState());
            
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            
            return GetEnumerator();
            
        }

        public EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
            return new EnumeratorNoState(this, in allocator);
        }

        public T Dequeue(in MemoryAllocator allocator) {
            if (this.size(in allocator) == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyQueue);
            }

            var removed = this.array(in allocator)[in allocator, this.head(in allocator)];
            this.array(in allocator)[in allocator, this.head(in allocator)] = default(T);
            this.head(in allocator) = (this.head(in allocator) + 1) % this.array(in allocator).Length(in allocator);
            this.size(in allocator)--;
            this.version(in allocator)++;
            return removed;
        }

        public T Peek(in MemoryAllocator allocator) {
            if (this.size(in allocator) == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyQueue);
            }

            return this.array(in allocator)[in allocator, this.head(in allocator)];
        }

        public bool Contains<U>(in MemoryAllocator allocator, U item) where U : System.IEquatable<T> {
            var index = this.head(in allocator);
            var count = this.size(in allocator);

            while (count-- > 0) {
                if (item.Equals(this.array(in allocator)[in allocator, index])) {
                    return true;
                }

                index = (index + 1) % this.array(in allocator).Length(in allocator);
            }

            return false;
        }

        private T GetElement(in MemoryAllocator allocator, int i) {
            return this.array(in allocator)[in allocator, (this.head(in allocator) + i) % this.array(in allocator).Length(in allocator)];
        }

        private void SetCapacity(ref MemoryAllocator allocator, int capacity) {
            this.array(in allocator).Resize(ref allocator, capacity);
            this.head(in allocator) = 0;
            this.tail(in allocator) = this.size(in allocator) == capacity ? 0 : this.size(in allocator);
            this.version(in allocator)++;
        }

        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private readonly State state;
            private Queue<T> q;
            private int index; // -1 = not started, -2 = ended/disposed
            private readonly int version;
            private T currentElement;

            internal Enumerator(Queue<T> q, State state) {
                this.q = q;
                this.version = this.q.version(in state.allocator);
                this.index = -1;
                this.currentElement = default(T);
                this.state = state;
            }

            public void Dispose() {
                this.index = -2;
                this.currentElement = default(T);
            }

            public bool MoveNext() {
                if (this.version != this.q.version(in this.state.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) {
                    return false;
                }

                this.index++;

                if (this.index == this.q.size(in this.state.allocator)) {
                    this.index = -2;
                    this.currentElement = default(T);
                    return false;
                }

                this.currentElement = this.q.GetElement(in this.state.allocator, this.index);
                return true;
            }

            public T Current {
                get {
                    if (this.index < 0) {
                        if (this.index == -1) {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                        } else {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                        }
                    }

                    return this.currentElement;
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    if (this.index < 0) {
                        if (this.index == -1) {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                        } else {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                        }
                    }

                    return this.currentElement;
                }
            }

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.q.version(in this.state.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = -1;
                this.currentElement = default(T);
            }

        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<T> {

            private readonly MemoryAllocator allocator;
            private Queue<T> q;
            private int index; // -1 = not started, -2 = ended/disposed
            private readonly int version;
            private T currentElement;

            internal EnumeratorNoState(Queue<T> q, in MemoryAllocator allocator) {
                this.q = q;
                this.version = this.q.version(in allocator);
                this.index = -1;
                this.currentElement = default(T);
                this.allocator = allocator;
            }

            public void Dispose() {
                this.index = -2;
                this.currentElement = default(T);
            }

            public bool MoveNext() {
                if (this.version != this.q.version(in this.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) {
                    return false;
                }

                this.index++;

                if (this.index == this.q.size(in this.allocator)) {
                    this.index = -2;
                    this.currentElement = default(T);
                    return false;
                }

                this.currentElement = this.q.GetElement(in this.allocator, this.index);
                return true;
            }

            public T Current {
                get {
                    if (this.index < 0) {
                        if (this.index == -1) {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                        } else {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                        }
                    }

                    return this.currentElement;
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    if (this.index < 0) {
                        if (this.index == -1) {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                        } else {
                            ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                        }
                    }

                    return this.currentElement;
                }
            }

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.q.version(in this.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = -1;
                this.currentElement = default(T);
            }

        }

    }

}