namespace ME.ECS.Collections.MemoryAllocator {

    using Collections.V3;
    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(QueueProxy<>))]
    public struct Queue<T> where T : unmanaged {
        
        private const int MINIMUM_GROW = 4;
        private const int GROW_FACTOR = 200;

        [ME.ECS.Serializer.SerializeField]
        private MemArrayAllocator<T> array;
        [ME.ECS.Serializer.SerializeField]
        private int head;
        [ME.ECS.Serializer.SerializeField]
        private int tail;
        [ME.ECS.Serializer.SerializeField]
        private int size;
        [ME.ECS.Serializer.SerializeField]
        private int version;
        public readonly bool isCreated => this.array.isCreated;

        public readonly int Count => this.size;

        public Queue(ref MemoryAllocator allocator, int capacity) {
            this = default;
            this.array = new MemArrayAllocator<T>(ref allocator, capacity);
        }

        public void Dispose(ref MemoryAllocator allocator) {
            
            this.array.Dispose(ref allocator);
            this = default;
            
        }

        public void Clear(in MemoryAllocator allocator) {
            this.head = 0;
            this.tail = 0;
            this.size = 0;
            this.version++;
        }

        public void Enqueue(ref MemoryAllocator allocator, T item) {
            if (this.size == this.array.Length) {
                var newCapacity = (int)((long)this.array.Length * (long)Queue<T>.GROW_FACTOR / 100);
                if (newCapacity < this.array.Length + Queue<T>.MINIMUM_GROW) {
                    newCapacity = this.array.Length + Queue<T>.MINIMUM_GROW;
                }

                this.SetCapacity(ref allocator, newCapacity);
            }

            this.array[in allocator, this.tail] = item;
            this.tail = (this.tail + 1) % this.array.Length;
            this.size++;
            this.version++;
        }

        public readonly Enumerator GetEnumerator(State state) {
            return new Enumerator(this, state);
        }
        
        public readonly Enumerator GetEnumerator() {
            
            return this.GetEnumerator(Worlds.current.GetState());
            
        }

        public EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
            return new EnumeratorNoState(this, in allocator);
        }

        public T Dequeue(in MemoryAllocator allocator) {
            if (this.size == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyQueue);
            }

            var removed = this.array[in allocator, this.head];
            this.array[in allocator, this.head] = default(T);
            this.head = (this.head + 1) % this.array.Length;
            this.size--;
            this.version++;
            return removed;
        }

        public T Peek(in MemoryAllocator allocator) {
            if (this.size == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyQueue);
            }

            return this.array[in allocator, this.head];
        }

        public bool Contains<U>(in MemoryAllocator allocator, U item) where U : System.IEquatable<T> {
            var index = this.head;
            var count = this.size;

            while (count-- > 0) {
                if (item.Equals(this.array[in allocator, index])) {
                    return true;
                }

                index = (index + 1) % this.array.Length;
            }

            return false;
        }

        private T GetElement(in MemoryAllocator allocator, int i) {
            return this.array[in allocator, (this.head + i) % this.array.Length];
        }

        private void SetCapacity(ref MemoryAllocator allocator, int capacity) {
            this.array.Resize(ref allocator, capacity);
            this.head = 0;
            this.tail = this.size == capacity ? 0 : this.size;
            this.version++;
        }

        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private readonly State state;
            private Queue<T> q;
            private int index; // -1 = not started, -2 = ended/disposed
            private readonly int version;
            private T currentElement;

            internal Enumerator(Queue<T> q, State state) {
                this.q = q;
                this.version = this.q.version;
                this.index = -1;
                this.currentElement = default(T);
                this.state = state;
            }

            public void Dispose() {
                this.index = -2;
                this.currentElement = default(T);
            }

            public bool MoveNext() {
                if (this.version != this.q.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) {
                    return false;
                }

                this.index++;

                if (this.index == this.q.size) {
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
                if (this.version != this.q.version) {
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
                this.version = this.q.version;
                this.index = -1;
                this.currentElement = default(T);
                this.allocator = allocator;
            }

            public void Dispose() {
                this.index = -2;
                this.currentElement = default(T);
            }

            public bool MoveNext() {
                if (this.version != this.q.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) {
                    return false;
                }

                this.index++;

                if (this.index == this.q.size) {
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
                if (this.version != this.q.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = -1;
                this.currentElement = default(T);
            }

        }

    }

}