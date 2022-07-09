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

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct BufferArrayEnumerator<T> : IEnumerator<T> {

        private readonly BufferArray<T> bufferArray;
        private int index;

        public BufferArrayEnumerator(BufferArray<T> bufferArray) {

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

    public static class BufferArrayExt {
    
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArray<T> Clamp<T>(this in BufferArray<T> src, int length) {

            var delta = src.Length - length;
            if (delta > 0) System.Array.Clear(src.arr, length, delta);
            return new BufferArray<T>(src.arr, length);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArray<T> Clamp<T, TCopy>(this in BufferArray<T> src, int length, TCopy copy) where TCopy : IArrayElementCopy<T> {

            for (int i = length; i < src.Length; ++i) {
                
                copy.Recycle(ref src.arr[i]);
                
            }
            return new BufferArray<T>(src.arr, length);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static IBufferArray Resize<T>(this in BufferArray<T> src, int newSize) {

            var newArr = new T[newSize];
            if (src.arr != null) System.Array.Copy(src.arr, newArr, newSize > src.Length ? src.Length : newSize);
            return new BufferArray<T>(newArr, newSize);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static int IndexOf<T>(this in BufferArray<T> src, T instance) {

            if (src.isCreated == false) return -1;

            return System.Array.IndexOf(src.arr, instance);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> RemoveAt<T>(this in BufferArray<T> src, int index) {

            var newLength = src.Length;
            newLength--;

            if (index < newLength) {

                System.Array.Copy(src.arr, index + 1, src.arr, index, newLength - index);

            }

            return new BufferArray<T>(src.arr, newLength);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> RemoveAtUnsorted<T>(this in BufferArray<T> src, ref int index) {

            src.arr[index] = src.arr[src.Length - 1];
            --index;
            return new BufferArray<T>(src.arr, src.Length - 1);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> Clear<T>(this in BufferArray<T> src) {

            PoolArray<T>.Recycle(src);
            return new BufferArray<T>(null, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static BufferArray<T> Dispose<T>(this in BufferArray<T> src) {

            PoolArray<T>.Recycle(src);
            return new BufferArray<T>(null, 0);

        }

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
                [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get {
                    if (this.isCreated == false || index < 0 || index >= this.usedLength) throw new System.IndexOutOfRangeException($"Index: {index} [0..{this.usedLength}], Tick: {Worlds.currentWorld.GetCurrentTick()}");
                    return ref this.data[index];
                }
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public override int GetHashCode() {

                if (this.data == null) return 0;
                return this.data.GetHashCode();

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public readonly bool Equals(EditorArr obj) {

                return this.data == obj.data;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public override bool Equals(object obj) {

                if (obj is EditorArr ent) {
                
                    return this.Equals(ent);
                
                }

                return false;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static implicit operator T[](EditorArr item) {

                return item.data;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static bool operator ==(EditorArr v1, object obj) {

                if (obj is EditorArr arr && arr.data == v1.data) return true;
                if (v1.data == obj) return true;
                return false;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                if (this.isCreated == false || index >= this.Length) throw new System.IndexOutOfRangeException($"Index: {index} [0..{this.Length}], Tick: {Worlds.currentWorld.GetCurrentTick()}");
                return ref this.arr[index];
            }
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
        public BufferArray(T[] arr, int length, int realLength) {

            this.Length = length;
            this.isCreated = (length > 0 && arr != null);

            #if UNITY_EDITOR && EDITOR_ARRAY
            {
                var len = (realLength >= 0 ? realLength : length);
                if (arr != null && arr.Length < len) {
                    throw new System.Exception($"BufferArray try to being created with arr.Length < used_length ({arr.Length} < {len})");
                }
            }
            this.arr.data = arr;
            this.arr.Length = (arr != null ? arr.Length : 0);
            this.arr.usedLength = (realLength >= 0 ? realLength : length);
            this.arr.isCreated = this.isCreated;
            #else
            this.arr = arr;
            #endif

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public BufferArray(T[] arr, int length) : this(arr, length, -1) { }

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
        public BufferArrayEnumerator<T> GetEnumerator() {

            return new BufferArrayEnumerator<T>(this);

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

            return e1.arr.Equals(e2.arr);

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
        public static BufferArray<T> From(ListCopyable<T> arr) {

            var length = arr.Count;
            var buffer = PoolArray<T>.Spawn(length);
            System.Array.Copy(arr.innerArray, buffer.arr, length);

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

    }

}