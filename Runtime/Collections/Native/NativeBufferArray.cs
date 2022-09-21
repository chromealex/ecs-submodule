#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
#define EDITOR_ARRAY
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS.Collections {
    
    using Unity.Collections;

    public interface INativeBufferArray : IBufferArray {

        

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct NativeBufferArrayEnumerator<T> : IEnumerator<T> where T : struct {

        private readonly NativeBufferArray<T> bufferArray;
        private int index;

        public NativeBufferArrayEnumerator(NativeBufferArray<T> bufferArray) {

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

    public static class NativeBufferArrayExt {
        
        [BurstCompatible(GenericTypeArguments = new[] { typeof(int), typeof(int) })]
        public static int IndexOf<T, U>(this NativeBufferArray<T> array, U value) where T : struct, System.IEquatable<U> {

            if (array.isCreated == false) return -1;
            return array.arr.IndexOf(value);
            
        }

        public static unsafe void* GetUnsafePtr<T>(this ref NativeBufferArray<T> arr) where T : struct {
            return arr.arr.GetUnsafePtr();
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static unsafe void* GetUnsafeReadOnlyPtr<T>(this ref NativeBufferArray<T> arr) where T : struct {
            return arr.arr.GetUnsafeReadOnlyPtr();
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ref readonly T Read<T>(this ref NativeBufferArray<T> arr, int index) where T : struct {
            arr.CheckBounds(index);
            return ref arr.arr.GetRefRead(index);
        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static IBufferArray Resize<T>(this ref NativeBufferArray<T> arr, int newSize) where T : struct {

            var newArr = new NativeArray<T>(newSize, Allocator.Persistent);
            if (arr.arr.IsCreated == true) NativeArrayUtils.Copy(arr.arr, ref newArr, newSize > arr.Length ? arr.Length : newSize);
            return new NativeBufferArray<T>(newArr, newSize);

        }

        public static NativeBufferArray<T> Resize<T>(this ref NativeBufferArray<T> arr, int index, bool resizeWithOffset, out bool result) where T : struct {

            var newSize = index + 1;
            result = false;
            if (newSize > arr.Length) {

                if (newSize > arr.arr.Length) {

                    NativeArrayUtils.Resize(newSize, ref arr.arr, Allocator.Persistent, resizeWithOffset);

                }
                
                result = true;
                return new NativeBufferArray<T>(arr.arr, newSize);

            }

            return arr;

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static NativeBufferArray<T> RemoveAt<T>(this ref NativeBufferArray<T> src, int index) where T : struct {

            var newLength = src.Length;
            newLength--;

            var arr = src.arr;
            if (index < newLength) {

                NativeArrayUtils.Copy(in src.arr, index + 1, ref arr, index, newLength - index);
            
            }

            return new NativeBufferArray<T>(arr, newLength);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static NativeBufferArray<T> RemoveAtUnsorted<T>(this ref NativeBufferArray<T> src, ref int index) where T : struct {

            var arr = src.arr;
            arr[index] = arr[src.Length - 1];
            --index;
            return new NativeBufferArray<T>(arr, src.Length - 1);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Clear<T>(this ref NativeBufferArray<T> arr) where T : struct {

            NativeArrayUtils.Clear(arr.arr);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void Clear<T>(this ref NativeBufferArray<T> arr, int index, int length) where T : struct {

            NativeArrayUtils.Clear(arr.arr, index, length);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static NativeBufferArray<T> Dispose<T>(this ref NativeBufferArray<T> arr) where T : struct {

            if (arr.isCreated == true) arr.arr.Dispose();
            return NativeBufferArray<T>.Empty;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static NativeBufferArray<T> Clamp<T>(this ref NativeBufferArray<T> arr, int length) where T : struct {

            var delta = arr.Length - length;
            if (delta > 0) NativeArrayUtils.Clear(arr.arr, length, delta);
            return new NativeBufferArray<T>(arr.arr, length);

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static NativeBufferArray<T> Clamp<T, TCopy>(this ref NativeBufferArray<T> arr, int length, TCopy copy) where TCopy : IArrayElementCopy<T> where T : struct {

            for (int i = length; i < arr.Length; ++i) {
                
                copy.Recycle(ref arr.arr.GetRef(i));
                
            }
            return new NativeBufferArray<T>(arr.arr, length);

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
    public struct NativeBufferArray<T> : System.IEquatable<NativeBufferArray<T>>, INativeBufferArray where T : struct {

        public static NativeBufferArray<T> Empty = new NativeBufferArray<T>();

        [ME.ECS.Serializer.SerializeFieldAttribute]
        [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
        internal NativeArray<T> arr;
        [ME.ECS.Serializer.SerializeFieldAttribute]
        public readonly int Length;
        public readonly bool isCreated => this.arr.IsCreated;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public System.Array GetArray() {

            if (this.isCreated == false) return null;
            return this.arr.ToArray();

        }

        [System.Diagnostics.ConditionalAttribute("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        internal readonly void CheckBounds(int index) {
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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArrayEnumerator<T> GetEnumerator() {

            return new NativeBufferArrayEnumerator<T>(this);

        }
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray(NativeArray<T> arr, int length) {
            
            this.Length = length;
            this.arr = arr;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray(T[] arr, int length, int realSize) {

            this.Length = length;
            if (arr == null) {
                this.arr = default;
            } else {
                this.arr = new NativeArray<T>(arr, Allocator.Persistent);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray(int length, Unity.Collections.NativeArrayOptions options = Unity.Collections.NativeArrayOptions.ClearMemory) {

            this.Length = length;
            this.arr = new NativeArray<T>(length, Allocator.Persistent, options);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray(T[] arr) {

            this.Length = arr.Length;
            this.arr = new NativeArray<T>(arr, Allocator.Persistent);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray(NativeArray<T> arr) {

            this.Length = arr.Length;
            this.arr = new NativeArray<T>(arr, Allocator.Persistent);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray(NativeBufferArray<T> arr) {

            this.Length = arr.Length;
            this.arr = new NativeArray<T>(arr.arr, Allocator.Persistent);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray(BufferArray<T> arr) {

            this.Length = arr.Length;
            this.arr = new NativeArray<T>(arr.arr, Allocator.Persistent);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeBufferArray(ListCopyable<T> arr) {

            this.Length = arr.Count;
            this.arr = new NativeArray<T>(arr.innerArray, Allocator.Persistent);
            
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

    }

}