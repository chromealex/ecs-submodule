#define EDITOR_ARRAY
using System.Collections;
using System.Collections.Generic;

namespace ME.ECS.Collections {

    public interface IBufferArray {

        int Count { get; }
        System.Array GetArray();
        IBufferArray Resize(int newSize);

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public readonly struct BufferArray<T> : System.IEquatable<BufferArray<T>>, IBufferArray {

        public static BufferArray<T> Empty = new BufferArray<T>(null, 0);

        #if UNITY_EDITOR && EDITOR_ARRAY
        [System.Serializable]
        public struct EditorArr {

            public T[] data;
            public int Length;
            public int usedLength;
            public bool isCreated;
            
            public ref T this[int index] {
                get {
                    if (this.isCreated == false || index >= this.usedLength) throw new System.IndexOutOfRangeException("Tick: " + Worlds.currentWorld.GetCurrentTick());
                    return ref this.data[index];
                }
            }

            public override int GetHashCode() {
                
                return this.data.GetHashCode();
                
            }

            public override bool Equals(object obj) {
                
                throw new AllocationException();
                
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

        public int Count {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return this.Length;
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IBufferArray Resize(int newSize) {

            var newArr = new T[newSize];
            if (this.arr != null) System.Array.Copy(this.arr, newArr, newSize > this.Length ? this.Length : newSize);
            return new BufferArray<T>(newArr, newSize);

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T instance) {

            if (this.isCreated == false) return -1;
            
            return System.Array.IndexOf(this.arr, instance);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArray<T> RemoveAt(int index) {

            var newLength = this.Length;
            newLength--;
            
            if (index < newLength) {

                System.Array.Copy(this.arr, index + 1, this.arr, index, newLength - index);

            }
            
            return new BufferArray<T>(this.arr, newLength);
            
        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public System.Array GetArray() {

            return this.arr;

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArray<T> RemoveAtUnsorted(ref int index) {

            this.arr[index] = this.arr[this.Length - 1];
            --index;
            return new BufferArray<T>(this.arr, this.Length - 1);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArray<T> Clear() {
            
            PoolArray<T>.Recycle(this);
            return new BufferArray<T>(null, 0);
            
        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() {
            
            return new Enumerator(this);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArray<T> From(ListCopyable<T> arr) {

            var length = arr.Count;
            var buffer = PoolArray<T>.Spawn(length);
            System.Array.Copy(arr.innerArray.arr, buffer.arr, length);
            
            return buffer;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArray<T> From(T[] arr) {

            var length = arr.Length;
            var buffer = PoolArray<T>.Spawn(length);
            ArrayUtils.Copy(new BufferArray<T>(arr, length), ref buffer);
            
            return buffer;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArray<T> From(BufferArray<T> arr) {

            var length = arr.Length;
            var buffer = PoolArray<T>.Spawn(length);
            ArrayUtils.Copy(arr, ref buffer);
            
            return buffer;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArray<T> From(IList<T> arr) {

            var length = arr.Count;
            var buffer = PoolArray<T>.Spawn(length);
            ArrayUtils.Copy(arr, ref buffer);
            
            return buffer;
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal BufferArray(T[] arr, int length, int realLength = -1) {
            
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

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArray<T> Dispose() {
            
            PoolArray<T>.Recycle(this);
            return new BufferArray<T>(null, 0);

        }

        /*public ref T this[int index] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                return ref this.arr[index];
            }
        }*/
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BufferArray<T> e1, BufferArray<T> e2) {

            return e1.arr == e2.arr;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BufferArray<T> e1, BufferArray<T> e2) {

            return !(e1 == e2);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(BufferArray<T> other) {

            return this == other;

        }

        public override bool Equals(object obj) {
            
            throw new AllocationException();
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
                [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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
                [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get {
                    return ref this.bufferArray.arr[this.index];
                }
            }

            #if ECS_COMPILE_IL2CPP_OPTIONS
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
            [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
            #endif
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {

                ++this.index;
                if (this.index >= this.bufferArray.Length) return false;
                return true;

            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Reset() {}

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Dispose() {}
            
        }
        
    }
    
}
