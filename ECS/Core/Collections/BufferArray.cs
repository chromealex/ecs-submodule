#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
#define EDITOR_ARRAY
using System.Collections;
using System.Collections.Generic;

namespace ME.ECS.Collections {

    public interface IBufferArray {

        int Count { get; }
        System.Array GetArray();
        IBufferArray Resize(int newSize);

    }

    /// <summary>
    /// BufferArray<T> for array pool
    /// Note: Beware of readonly instruction - it will readonly in build, but in editor it is non-readonly because of PropertyDrawer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public readonly struct BufferArray<T> : System.IEquatable<BufferArray<T>>, IBufferArray {

        public static BufferArray<T> Empty => new BufferArray<T>(null, 0);

        #if UNITY_EDITOR && EDITOR_ARRAY
        [System.Serializable]
        public struct EditorArr {

            public T[] data;
            public int Length;
            public int usedLength;
            public bool isCreated;

            public ref T this[int index] {
                get {
                    if (this.isCreated == false || index >= this.usedLength) throw new System.IndexOutOfRangeException($"Index: {index} [0..{this.usedLength}], Tick: {Worlds.currentWorld.GetCurrentTick()}");
                    return ref this.data[index];
                }
            }

            public override int GetHashCode() {

                if (this.data == null) return 0;
                return this.data.GetHashCode();

            }

            public bool Equals(EditorArr obj) {

                return this.data == obj.data;

            }

            public override bool Equals(object obj) {

                if (obj is EditorArr ent) {
                
                    return this.Equals(ent);
                
                }

                return false;

            }

            public static implicit operator T[](EditorArr item) {

                return item.data;

            }

            public static bool operator ==(EditorArr v1, object obj) {

                if (obj is EditorArr arr && arr.data == v1.data) return true;
                if (v1.data == obj) return true;
                return false;

            }

            public static bool operator !=(EditorArr v1, object obj) {

                return !(v1 == obj);

            }

        }

        public readonly EditorArr arr;
        #else
        public readonly T[] arr;
        #endif
        public readonly int Length;
        public readonly bool isCreated;

        public ref T this[int index] {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                if (this.isCreated == false || index >= this.Length) throw new System.IndexOutOfRangeException($"Index: {index} [0..{this.Length}], Tick: {Worlds.currentWorld.GetCurrentTick()}");
                return ref this.arr[index];
            }
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal BufferArray(T[] arr, int length) : this(arr, length, -1) { }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal BufferArray(T[] arr, int length, int realLength) {

            this.Length = length;
            this.isCreated = (length > 0 && arr != null);

            #if UNITY_EDITOR && EDITOR_ARRAY
            this.arr.data = arr;
            this.arr.Length = (arr != null ? arr.Length : 0);
            this.arr.usedLength = (realLength >= 0 ? realLength : length);
            this.arr.isCreated = this.isCreated;
            #else
            this.arr = arr;
            #endif

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

            var newArr = new T[newSize];
            if (this.arr != null) System.Array.Copy(this.arr, newArr, newSize > this.Length ? this.Length : newSize);
            return new BufferArray<T>(newArr, newSize);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public int IndexOf(T instance) {

            if (this.isCreated == false) return -1;

            return System.Array.IndexOf(this.arr, instance);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public BufferArray<T> RemoveAt(int index) {

            var newLength = this.Length;
            newLength--;

            if (index < newLength) {

                System.Array.Copy(this.arr, index + 1, this.arr, index, newLength - index);

            }

            return new BufferArray<T>(this.arr, newLength);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public System.Array GetArray() {

            return this.arr;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public BufferArray<T> RemoveAtUnsorted(ref int index) {

            this.arr[index] = this.arr[this.Length - 1];
            --index;
            return new BufferArray<T>(this.arr, this.Length - 1);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public BufferArray<T> Clear() {

            PoolArray<T>.Recycle(this);
            return new BufferArray<T>(null, 0);

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
        public static BufferArray<T> From(ListCopyable<T> arr) {

            var length = arr.Count;
            var buffer = PoolArray<T>.Spawn(length);
            System.Array.Copy(arr.innerArray.arr, buffer.arr, length);

            return buffer;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> From(T[] arr) {

            var length = arr.Length;
            var buffer = PoolArray<T>.Spawn(length);
            ArrayUtils.Copy(new BufferArray<T>(arr, length), ref buffer);

            return buffer;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> From(BufferArray<T> arr) {

            var length = arr.Length;
            var buffer = PoolArray<T>.Spawn(length);
            ArrayUtils.Copy(arr, ref buffer);

            return buffer;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> From(IList<T> arr) {

            var length = arr.Count;
            var buffer = PoolArray<T>.Spawn(length);
            ArrayUtils.Copy(arr, ref buffer);

            return buffer;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public BufferArray<T> Dispose() {

            PoolArray<T>.Recycle(this);
            return new BufferArray<T>(null, 0);

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
        public static bool operator ==(BufferArray<T> e1, BufferArray<T> e2) {

            return e1.arr == e2.arr;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool operator !=(BufferArray<T> e1, BufferArray<T> e2) {

            return !(e1 == e2);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Equals(BufferArray<T> other) {

            return this == other;

        }

        public override bool Equals(object other) {
            
            if (other is BufferArray<T> ent) {
                
                return this.Equals(ent);
                
            }
            
            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public override int GetHashCode() {

            if (this.arr == null) return 0;
            return this.arr.GetHashCode();

        }

        public override string ToString() {

            var content = string.Empty;
            for (int i = 0; i < this.Length; ++i) {
                content += "[" + i + "] " + this.arr[i] + "\n";
            }

            return "BufferArray<>[" + this.Length + "]:\n" + content;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public struct Enumerator : IEnumerator<T> {

            private readonly BufferArray<T> bufferArray;
            private int index;

            public Enumerator(BufferArray<T> bufferArray) {

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
            public ref T Current {
                #if INLINE_METHODS
                [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                #endif
                get {
                    return ref this.bufferArray.arr[this.index];
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