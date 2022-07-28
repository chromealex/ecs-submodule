namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;

    public struct Stack<T> where T : unmanaged {

        private const int DEFAULT_CAPACITY = 4;

        private MemArrayAllocator<T> array; // Storage for stack elements
        private int size; // Number of items in the stack.
        private int version; // Used to keep enumerator in sync w/ collection.
        public readonly bool isCreated;

        public Stack(ref MemoryAllocator allocator, int capacity) {
            this.array = new MemArrayAllocator<T>(ref allocator, capacity);
            this.size = 0;
            this.version = 0;
            this.isCreated = true;
        }

        public int Count => this.size;

        public void Clear(in MemoryAllocator allocator) {
            this.size = 0;
            this.version++;
        }

        public bool Contains(in MemoryAllocator allocator, T item) {

            var count = this.size;
            var c = System.Collections.Generic.EqualityComparer<T>.Default;
            while (count-- > 0) {
                if (c.Equals(this.array[in allocator, count], item)) {
                    return true;
                }
            }

            return false;

        }

        public readonly Enumerator GetEnumerator(State state) {
            return new Enumerator(this, state);
        }

        public readonly EnumeratorNoState GetEnumerator(in MemoryAllocator allocator) {
            return new EnumeratorNoState(this, in allocator);
        }

        public readonly T Peek(in MemoryAllocator allocator) {
            if (this.size == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyStack);
            }

            return this.array[in allocator, this.size - 1];
        }

        public T Pop(in MemoryAllocator allocator) {
            if (this.size == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyStack);
            }

            this.version++;
            var item = this.array[in allocator, --this.size];
            this.array[in allocator, this.size] = default;
            return item;
        }

        public void Push(ref MemoryAllocator allocator, T item) {
            if (this.size == this.array.Length) {
                this.array.Resize(ref allocator, this.array.Length == 0 ? Stack<T>.DEFAULT_CAPACITY : 2 * this.array.Length);
            }

            this.array[in allocator, this.size++] = item;
            this.version++;
        }

        public void Dispose(ref MemoryAllocator allocator) {
            
            this.array.Dispose(ref allocator);
            this = default;
            
        }

        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private Stack<T> _stack;
            private State state;
            private int _index;
            private int _version;
            private T currentElement;

            internal Enumerator(Stack<T> stack, State state) {
                this._stack = stack;
                this.state = state;
                this._version = this._stack.version;
                this._index = -2;
                this.currentElement = default(T);
            }

            public void Dispose() {
                this._index = -1;
            }

            public bool MoveNext() {
                bool retval;
                if (this._version != this._stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this._index == -2) { // First call to enumerator.
                    this._index = this._stack.size - 1;
                    retval = this._index >= 0;
                    if (retval) {
                        this.currentElement = this._stack.array[in this.state.allocator, this._index];
                    }

                    return retval;
                }

                if (this._index == -1) { // End of enumeration.
                    return false;
                }

                retval = --this._index >= 0;
                if (retval) {
                    this.currentElement = this._stack.array[in this.state.allocator, this._index];
                } else {
                    this.currentElement = default(T);
                }

                return retval;
            }

            public T Current {
                get {
                    if (this._index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this._index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    if (this._index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this._index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            void System.Collections.IEnumerator.Reset() {
                if (this._version != this._stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this._index = -2;
                this.currentElement = default;
            }

        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<T> {

            private Stack<T> _stack;
            private MemoryAllocator allocator;
            private int _index;
            private int _version;
            private T currentElement;

            internal EnumeratorNoState(Stack<T> stack, in MemoryAllocator allocator) {
                this._stack = stack;
                this.allocator = allocator;
                this._version = this._stack.version;
                this._index = -2;
                this.currentElement = default(T);
            }

            public void Dispose() {
                this._index = -1;
            }

            public bool MoveNext() {
                bool retval;
                if (this._version != this._stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this._index == -2) { // First call to enumerator.
                    this._index = this._stack.size - 1;
                    retval = this._index >= 0;
                    if (retval) {
                        this.currentElement = this._stack.array[in this.allocator, this._index];
                    }

                    return retval;
                }

                if (this._index == -1) { // End of enumeration.
                    return false;
                }

                retval = --this._index >= 0;
                if (retval) {
                    this.currentElement = this._stack.array[in this.allocator, this._index];
                } else {
                    this.currentElement = default(T);
                }

                return retval;
            }

            public T Current {
                get {
                    if (this._index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this._index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    if (this._index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this._index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            void System.Collections.IEnumerator.Reset() {
                if (this._version != this._stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this._index = -2;
                this.currentElement = default;
            }

        }

    }

}