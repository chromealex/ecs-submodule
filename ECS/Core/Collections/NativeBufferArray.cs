#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
#define EDITOR_ARRAY
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS.Collections {
    
    using Unity.Collections;

    public static class NativeBufferArrayExt {

        [BurstCompatible(GenericTypeArguments = new[] { typeof(int), typeof(int) })]
        public static int IndexOf<T, U>(this NativeBufferArray<T> array, U value) where T : struct, System.IEquatable<U> {

            if (array.isCreated == false) return -1;
            return array.arr.IndexOf(value);
            
        }

    }

    /// <summary>
    /// NativeBufferArray<T> for native array
    /// Note: Beware of readonly instruction - it will readonly in build, but in editor it is non-readonly because of PropertyDrawer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public struct NativeBufferArray<T> : System.IEquatable<NativeBufferArray<T>>, IBufferArray where T : struct {

        public static NativeBufferArray<T> Empty = new NativeBufferArray<T>();

        internal NativeArray<T> arr;
        public readonly int Length;
        public bool isCreated => this.arr.IsCreated;

        public unsafe System.IntPtr GetUnsafePtr() {
            return (System.IntPtr)this.arr.GetUnsafePtr();
        }

        public unsafe System.IntPtr GetUnsafeReadOnlyPtr() {
            return (System.IntPtr)this.arr.GetUnsafeReadOnlyPtr();
        }

        [System.Diagnostics.ConditionalAttribute("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void CheckBounds(int index) {
            if (this.isCreated == false || index >= this.Length) throw new System.IndexOutOfRangeException($"Index: {index} [0..{this.Length}], Tick: {Worlds.currentWorld.GetCurrentTick()}");
        }

        public ref T this[int index] {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                this.CheckBounds(index);
                return ref this.arr.GetRef(index);
            }
        }

        public ref readonly T Read(int index) {
            this.CheckBounds(index);
            return ref this.arr.GetRefRead(index);
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(T[] arr, int length) : this(arr, length, -1) { }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(NativeArray<T> arr, int length) : this(arr, length, -1) { }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(NativeArray<T> arr, int length, int realLength) {

            this.Length = length;
            this.arr = arr;
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(T[] arr, int length, int realLength) {

            this.Length = length;
            this.arr = new NativeArray<T>(arr, Allocator.Persistent);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(int length) {

            this.Length = length;
            this.arr = new NativeArray<T>(length, Allocator.Persistent);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(T[] arr) {

            this.Length = arr.Length;
            this.arr = new NativeArray<T>(arr, Allocator.Persistent);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(NativeArray<T> arr) {

            this.Length = arr.Length;
            this.arr = new NativeArray<T>(arr, Allocator.Persistent);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(NativeBufferArray<T> arr) {

            this.Length = arr.Length;
            this.arr = new NativeArray<T>(arr.arr, Allocator.Persistent);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(BufferArray<T> arr) {

            this.Length = arr.Length;
            this.arr = new NativeArray<T>(arr.arr, Allocator.Persistent);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal NativeBufferArray(ListCopyable<T> arr) {

            this.Length = arr.Count;
            this.arr = new NativeArray<T>(arr.innerArray.arr, Allocator.Persistent);
            
        }

        public static NativeBufferArray<T> From(NativeArray<T> arr) {

            return new NativeBufferArray<T>(arr);

        }

        public static NativeBufferArray<T> From(NativeBufferArray<T> arr) {

            return new NativeBufferArray<T>(arr);

        }

        public static NativeBufferArray<T> From(ListCopyable<T> arr) {

            return new NativeBufferArray<T>(arr);

        }

        public static NativeBufferArray<T> From(T[] arr) {

            return new NativeBufferArray<T>(arr);

        }

        public int Count {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return this.Length;
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public IBufferArray Resize(int newSize) {

            var newArr = new NativeArray<T>(newSize, Allocator.Persistent);
            if (this.arr.IsCreated == true) NativeArrayUtils.Copy(this.arr, ref newArr, newSize > this.Length ? this.Length : newSize);
            return new NativeBufferArray<T>(newArr, newSize);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray<T> RemoveAt(int index) {

            var newLength = this.Length;
            newLength--;

            var arr = this.arr;
            if (index < newLength) {

                NativeArrayUtils.Copy(in this.arr, index + 1, ref arr, index, newLength - index);
            
            }

            return new NativeBufferArray<T>(arr, newLength);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public System.Array GetArray() {

            if (this.isCreated == false) return null;
            return this.arr.ToArray();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray<T> RemoveAtUnsorted(ref int index) {

            var arr = this.arr;
            arr[index] = arr[this.Length - 1];
            --index;
            return new NativeBufferArray<T>(arr, this.Length - 1);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clear() {

            NativeArrayUtils.Clear(this.arr);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clear(int index, int length) {

            NativeArrayUtils.Clear(this.arr, index, length);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public Enumerator GetEnumerator() {

            return new Enumerator(this);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray<T> Dispose() {

            if (this.isCreated == true) this.arr.Dispose();
            return NativeBufferArray<T>.Empty;

        }

        /*public ref T this[int index] {
            #if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
            get {
                return ref this.arr[index];
            }
        }*/

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe bool operator ==(NativeBufferArray<T> e1, NativeBufferArray<T> e2) {

            return e1.arr.GetUnsafeReadOnlyPtr() == e2.arr.GetUnsafeReadOnlyPtr() && e1.arr.Length == e2.arr.Length;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool operator !=(NativeBufferArray<T> e1, NativeBufferArray<T> e2) {

            return !(e1 == e2);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Equals(NativeBufferArray<T> other) {

            return this == other;

        }

        public override bool Equals(object obj) {

            if (obj is NativeBufferArray<T> ent) {
                
                if (ent.isCreated == false && this.isCreated == false) return true;
                if (ent.isCreated != this.isCreated) return false;
                return this.Equals(ent);
                
            }
            
            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override int GetHashCode() {

            return this.arr.GetHashCode();

        }

        public override string ToString() {

            var content = string.Empty;
            for (int i = 0; i < this.Length; ++i) {
                content += "[" + i + "] " + this.arr[i] + "\n";
            }

            return "NativeBufferArray<>[" + this.Length + "]:\n" + content;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public struct Enumerator : IEnumerator<T> {

            private readonly NativeBufferArray<T> bufferArray;
            private int index;

            public Enumerator(NativeBufferArray<T> bufferArray) {

                this.bufferArray = bufferArray;
                this.index = -1;

            }

            object IEnumerator.Current {
                get {
                    throw new AllocationException();
                }
            }

            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            T IEnumerator<T>.Current {
                #if INLINE_METHODS
                [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                #endif
                get {
                    return this.bufferArray.arr[this.index];
                }
            }

            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            public ref readonly T Current {
                #if INLINE_METHODS
                [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                #endif
                get {
                    return ref this.bufferArray.arr.GetRefRead(this.index);
                }
            }

            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public bool MoveNext() {

                ++this.index;
                if (this.index >= this.bufferArray.Length) return false;
                return true;

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Reset() { }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Dispose() { }

        }

    }

}