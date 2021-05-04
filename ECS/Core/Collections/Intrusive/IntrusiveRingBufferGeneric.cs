#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS.Collections {

    public interface IIntrusiveRingBufferGeneric<T> where T : struct, System.IEquatable<T> {

        int Capacity { get; }
        int Count { get; }

        void Push(in T entityData);
        T GetValue(int index);
        void Clear();
        bool Contains(in T entityData);

        BufferArray<T> ToArray();
        IntrusiveRingBufferGeneric<T>.Enumerator GetEnumerator();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct IntrusiveRingBufferGeneric<T> : IIntrusiveRingBufferGeneric<T> where T : struct, System.IEquatable<T> {

        private const int DEFAULT_CAPACTIY = 4;

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private IntrusiveListGeneric<T>.Enumerator listEnumerator;

            T System.Collections.Generic.IEnumerator<T>.Current => this.listEnumerator.Current;
            public ref T Current => ref this.listEnumerator.Current;

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public Enumerator(IntrusiveRingBufferGeneric<T> hashSet) {

                this.listEnumerator = hashSet.list.GetEnumerator();

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {

                return this.listEnumerator.MoveNext();

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Reset() {

                this.listEnumerator = default;

            }

            object System.Collections.IEnumerator.Current {
                get {
                    throw new AllocationException();
                }
            }

            public void Dispose() { }

        }

        [ME.ECS.Serializer.SerializeField]
        private IntrusiveListGeneric<T> list;
        [ME.ECS.Serializer.SerializeField]
        private int capacity;

        public int Capacity => this.capacity;
        public int Count => this.list.Count;

        public IntrusiveRingBufferGeneric(int capacity) {

            this.capacity = capacity;
            this.list = default;

        }

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

            var arr = PoolArray<T>.Spawn(this.Count);
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

            return this.list.Contains(in entityData);

        }

        /// <summary>
        /// Clear the list.
        /// </summary>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clear() {

            this.list.Clear();

        }

        /// <summary>
        /// Gets the value by index.
        /// </summary>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public T GetValue(int index) {

            var idx = index % (this.capacity <= 0 ? IntrusiveRingBufferGeneric<T>.DEFAULT_CAPACTIY : this.capacity);
            if (idx >= this.list.Count) return default;
            return this.list.GetValue(idx);

        }

        /// <summary>
        /// Add new data at the end of the list.
        /// </summary>
        /// <param name="entityData"></param>
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Push(in T entityData) {

            if (this.list.Count >= (this.capacity <= 0 ? IntrusiveRingBufferGeneric<T>.DEFAULT_CAPACTIY : this.capacity)) {

                this.list.RemoveLast();

            }

            this.list.AddFirst(in entityData);

        }

    }

}