namespace ME.ECS.Collections.LowLevel {

    using Unsafe;

    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(ME.ECS.DebugUtils.StackProxyDebugger<>))]
    public struct Stack<T> where T : unmanaged {

        private struct InternalData {

            public MemArrayAllocator<T> array; // Storage for stack elements
            public int size; // Number of items in the stack.
            public int version; // Used to keep enumerator in sync w/ collection.

            public void Dispose(ref MemoryAllocator allocator) {
                
                this.array.Dispose(ref allocator);
                
            }

        }
        
        private const int DEFAULT_CAPACITY = 4;

        [ME.ECS.Serializer.SerializeField]
        private MemArrayAllocator<T> array;
        [ME.ECS.Serializer.SerializeField]
        private int size;
        [ME.ECS.Serializer.SerializeField]
        private int version;
        public bool isCreated => this.array.isCreated;

        public readonly int Count => this.size;

        public Stack(ref MemoryAllocator allocator, int capacity) {
            this = default;
            this.array = new MemArrayAllocator<T>(ref allocator, capacity);
        }

        public void Dispose(ref MemoryAllocator allocator) {
            
            this.array.Dispose(ref allocator);
            this = default;
            
        }

        public void Clear(in MemoryAllocator allocator) {
            this.size = 0;
            this.version++;
        }

        public bool Contains<U>(in MemoryAllocator allocator, U item) where U : System.IEquatable<T> {

            var count = this.size;
            while (count-- > 0) {
                if (item.Equals(this.array[in allocator, count])) {
                    return true;
                }
            }

            return false;

        }

        public readonly Enumerator GetEnumerator(State state) {
            return new Enumerator(this, state);
        }
        
        public readonly Enumerator GetEnumerator() {
            
            return this.GetEnumerator(Worlds.current.GetState());
            
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

        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private readonly Stack<T> stack;
            private readonly State state;
            private int index;
            private readonly int version;
            private T currentElement;

            internal Enumerator(Stack<T> stack, State state) {
                this.stack = stack;
                this.state = state;
                this.version = this.stack.version;
                this.index = -2;
                this.currentElement = default(T);
            }

            public void Dispose() {
                this.index = -1;
            }

            public bool MoveNext() {
                bool retval;
                if (this.version != this.stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) { // First call to enumerator.
                    this.index = this.stack.size - 1;
                    retval = this.index >= 0;
                    if (retval) {
                        this.currentElement = this.stack.array[in this.state.allocator, this.index];
                    }

                    return retval;
                }

                if (this.index == -1) { // End of enumeration.
                    return false;
                }

                retval = --this.index >= 0;
                if (retval) {
                    this.currentElement = this.stack.array[in this.state.allocator, this.index];
                } else {
                    this.currentElement = default(T);
                }

                return retval;
            }

            public T Current {
                get {
                    if (this.index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this.index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    if (this.index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this.index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = -2;
                this.currentElement = default;
            }

        }

        public struct EnumeratorNoState : System.Collections.Generic.IEnumerator<T> {

            private readonly Stack<T> stack;
            private readonly MemoryAllocator allocator;
            private int index;
            private readonly int version;
            private T currentElement;

            internal EnumeratorNoState(Stack<T> stack, in MemoryAllocator allocator) {
                this.stack = stack;
                this.allocator = allocator;
                this.version = this.stack.version;
                this.index = -2;
                this.currentElement = default(T);
            }

            public void Dispose() {
                this.index = -1;
            }

            public bool MoveNext() {
                bool retval;
                if (this.version != this.stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) { // First call to enumerator.
                    this.index = this.stack.size - 1;
                    retval = this.index >= 0;
                    if (retval) {
                        this.currentElement = this.stack.array[in this.allocator, this.index];
                    }

                    return retval;
                }

                if (this.index == -1) { // End of enumeration.
                    return false;
                }

                retval = --this.index >= 0;
                if (retval) {
                    this.currentElement = this.stack.array[in this.allocator, this.index];
                } else {
                    this.currentElement = default(T);
                }

                return retval;
            }

            public T Current {
                get {
                    if (this.index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this.index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    if (this.index == -2) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
                    }

                    if (this.index == -1) {
                        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
                    }

                    return this.currentElement;
                }
            }

            void System.Collections.IEnumerator.Reset() {
                if (this.version != this.stack.version) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = -2;
                this.currentElement = default;
            }

        }

    }

}