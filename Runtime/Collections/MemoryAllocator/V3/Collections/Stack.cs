namespace ME.ECS.Collections.MemoryAllocator {

    using ME.ECS.Collections.V3;
    using MemPtr = System.Int64;

    [System.Diagnostics.DebuggerTypeProxyAttribute(typeof(StackProxy<>))]
    public struct Stack<T> where T : unmanaged {

        private struct InternalData {

            public MemArrayAllocator<T> array; // Storage for stack elements
            public int size; // Number of items in the stack.
            public int version; // Used to keep enumerator in sync w/ collection.
            public readonly bool isCreated;

            public void Dispose(ref MemoryAllocator allocator) {
                
                this.array.Dispose(ref allocator);
                
            }

        }
        
        private const int DEFAULT_CAPACITY = 4;

        [ME.ECS.Serializer.SerializeField]
        private readonly MemPtr ptr;
        
        private readonly ref MemArrayAllocator<T> array(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).array;
        private readonly ref int size(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).size;
        private readonly ref int version(in MemoryAllocator allocator) => ref allocator.Ref<InternalData>(this.ptr).version;
        public bool isCreated => this.ptr != 0;

        public readonly int Count(in MemoryAllocator allocator) => this.size(in allocator);

        public Stack(ref MemoryAllocator allocator, int capacity) {
            this.ptr = allocator.AllocData<InternalData>(default);
            this.array(in allocator) = new MemArrayAllocator<T>(ref allocator, capacity);
            this.size(in allocator) = 0;
            this.version(in allocator) = 0;
        }

        public void Dispose(ref MemoryAllocator allocator) {
            
            allocator.Ref<InternalData>(this.ptr).Dispose(ref allocator);
            allocator.Free(this.ptr);
            this = default;
            
        }

        public void Clear(in MemoryAllocator allocator) {
            this.size(in allocator) = 0;
            this.version(in allocator)++;
        }

        public bool Contains<U>(in MemoryAllocator allocator, U item) where U : System.IEquatable<T> {

            var count = this.size(in allocator);
            while (count-- > 0) {
                if (item.Equals(this.array(in allocator)[in allocator, count])) {
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
            if (this.size(in allocator) == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyStack);
            }

            return this.array(in allocator)[in allocator, this.size(in allocator) - 1];
        }

        public T Pop(in MemoryAllocator allocator) {
            if (this.size(in allocator) == 0) {
                ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyStack);
            }

            this.version(in allocator)++;
            var item = this.array(in allocator)[in allocator, --this.size(in allocator)];
            this.array(in allocator)[in allocator, this.size(in allocator)] = default;
            return item;
        }

        public void Push(ref MemoryAllocator allocator, T item) {
            if (this.size(in allocator) == this.array(in allocator).Length(in allocator)) {
                this.array(in allocator).Resize(ref allocator, this.array(in allocator).Length(in allocator) == 0 ? Stack<T>.DEFAULT_CAPACITY : 2 * this.array(in allocator).Length(in allocator));
            }

            this.array(in allocator)[in allocator, this.size(in allocator)++] = item;
            this.version(in allocator)++;
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
                this.version = this.stack.version(in state.allocator);
                this.index = -2;
                this.currentElement = default(T);
            }

            public void Dispose() {
                this.index = -1;
            }

            public bool MoveNext() {
                bool retval;
                if (this.version != this.stack.version(in this.state.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) { // First call to enumerator.
                    this.index = this.stack.size(in this.state.allocator) - 1;
                    retval = this.index >= 0;
                    if (retval) {
                        this.currentElement = this.stack.array(in this.state.allocator)[in this.state.allocator, this.index];
                    }

                    return retval;
                }

                if (this.index == -1) { // End of enumeration.
                    return false;
                }

                retval = --this.index >= 0;
                if (retval) {
                    this.currentElement = this.stack.array(in this.state.allocator)[in this.state.allocator, this.index];
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
                if (this.version != this.stack.version(in this.state.allocator)) {
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
                this.version = this.stack.version(in this.allocator);
                this.index = -2;
                this.currentElement = default(T);
            }

            public void Dispose() {
                this.index = -1;
            }

            public bool MoveNext() {
                bool retval;
                if (this.version != this.stack.version(in this.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                if (this.index == -2) { // First call to enumerator.
                    this.index = this.stack.size(in this.allocator) - 1;
                    retval = this.index >= 0;
                    if (retval) {
                        this.currentElement = this.stack.array(in this.allocator)[in this.allocator, this.index];
                    }

                    return retval;
                }

                if (this.index == -1) { // End of enumeration.
                    return false;
                }

                retval = --this.index >= 0;
                if (retval) {
                    this.currentElement = this.stack.array(in this.allocator)[in this.allocator, this.index];
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
                if (this.version != this.stack.version(in this.allocator)) {
                    ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
                }

                this.index = -2;
                this.currentElement = default;
            }

        }

    }

}