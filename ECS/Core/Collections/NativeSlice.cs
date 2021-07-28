#if UNITY_EDITOR
//#define COLLECTIONS_CHECKS
#endif

#if NATIVE_ARRAY_BURST
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Internal;

namespace ME.ECS.Collections {

    using Unity.Collections;

    /// <summary>
    ///   <para>Native Slice.</para>
    /// </summary>
    [DebuggerDisplay("Length = {Length}")]
    [NativeContainerSupportsMinMaxWriteRestriction]
    #if COLLECTIONS_CHECKS
    [NativeContainer]
    #endif
    public struct NativeSliceBurst<T> : IEnumerable<T>, IEnumerable, IEquatable<NativeSliceBurst<T>> where T : struct {

        [NativeDisableUnsafePtrRestriction]
        internal unsafe byte* m_Buffer;
        internal int m_Stride;
        internal int m_Length;

        #if COLLECTIONS_CHECKS
        internal int m_MinIndex;
        internal int m_MaxIndex;
        internal AtomicSafetyHandle m_Safety;
        #endif

        public NativeSliceBurst(NativeSliceBurst<T> slice, int start) : this(slice, start, slice.Length - start) { }

        public unsafe NativeSliceBurst(NativeSliceBurst<T> slice, int start, int length) {

            #if COLLECTIONS_CHECKS
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(start), string.Format("Slice start {0} < 0.", (object)start));
            }

            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length), string.Format("Slice length {0} < 0.", (object)length));
            }

            if (start + length > slice.Length) {
                throw new ArgumentException(string.Format("Slice start + length ({0}) range must be <= slice.Length ({1})", (object)(start + length), (object)slice.Length));
            }

            if ((slice.m_MinIndex != 0 || slice.m_MaxIndex != slice.m_Length - 1) &&
                (start < slice.m_MinIndex || slice.m_MaxIndex < start || slice.m_MaxIndex < start + length - 1)) {
                throw new ArgumentException("Slice may not be used on a restricted range slice", nameof(slice));
            }

            this.m_MinIndex = 0;
            this.m_MaxIndex = length - 1;
            this.m_Safety = slice.m_Safety;
            #endif
            this.m_Stride = slice.m_Stride;
            this.m_Buffer = slice.m_Buffer + this.m_Stride * start;
            this.m_Length = length;
        }

        public NativeSliceBurst(NativeArrayBurst<T> array) : this(array, 0, array.Length) { }

        public NativeSliceBurst(NativeArrayBurst<T> array, int start) : this(array, start, array.Length - start) { }

        public static implicit operator NativeSliceBurst<T>(NativeArrayBurst<T> array) {
            return new NativeSliceBurst<T>(array);
        }

        public unsafe NativeSliceBurst(NativeArrayBurst<T> array, int start, int length) {
            #if COLLECTIONS_CHECKS
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(start), string.Format("Slice start {0} < 0.", (object)start));
            }

            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length), string.Format("Slice length {0} < 0.", (object)length));
            }

            if (start + length > array.Length) {
                throw new ArgumentException(string.Format("Slice start + length ({0}) range must be <= array.Length ({1})", (object)(start + length), (object)array.Length));
            }

            if ((array.m_MinIndex != 0 || array.m_MaxIndex != array.m_Length - 1) &&
                (start < array.m_MinIndex || array.m_MaxIndex < start || array.m_MaxIndex < start + length - 1)) {
                throw new ArgumentException("Slice may not be used on a restricted range array", nameof(array));
            }

            this.m_MinIndex = 0;
            this.m_MaxIndex = length - 1;
            this.m_Safety = array.m_Safety;
            #endif
            this.m_Stride = UnsafeUtility.SizeOf<T>();
            this.m_Buffer = (byte*)((IntPtr)array.m_Buffer + this.m_Stride * start);
            this.m_Length = length;
        }

        public unsafe NativeSliceBurst(NativeArray<T> array, int start, int length) {
            #if COLLECTIONS_CHECKS
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(start), string.Format("Slice start {0} < 0.", (object)start));
            }

            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length), string.Format("Slice length {0} < 0.", (object)length));
            }

            if (start + length > array.Length) {
                throw new ArgumentException(string.Format("Slice start + length ({0}) range must be <= array.Length ({1})", (object)(start + length), (object)array.Length));
            }

            if ((array.m_MinIndex != 0 || array.m_MaxIndex != array.m_Length - 1) &&
                (start < array.m_MinIndex || array.m_MaxIndex < start || array.m_MaxIndex < start + length - 1)) {
                throw new ArgumentException("Slice may not be used on a restricted range array", nameof(array));
            }

            this.m_MinIndex = 0;
            this.m_MaxIndex = length - 1;
            this.m_Safety = array.m_Safety;
            #endif
            this.m_Stride = UnsafeUtility.SizeOf<T>();
            this.m_Buffer = (byte*)((IntPtr)array.GetUnsafePtr() + this.m_Stride * start);
            this.m_Length = length;
        }

        public unsafe NativeSliceBurst<U> SliceConvert<U>() where U : struct {
            var num = UnsafeUtility.SizeOf<U>();
            NativeSliceBurst<U> nativeSlice;
            nativeSlice.m_Buffer = this.m_Buffer;
            nativeSlice.m_Stride = num;
            nativeSlice.m_Length = this.m_Length * this.m_Stride / num;

            #if COLLECTIONS_CHECKS
            if (this.m_Stride != UnsafeUtility.SizeOf<T>()) {
                throw new InvalidOperationException("SliceConvert requires that stride matches the size of the source type");
            }

            if (this.m_MinIndex != 0 || this.m_MaxIndex != this.m_Length - 1) {
                throw new InvalidOperationException("SliceConvert may not be used on a restricted range array");
            }

            if ((uint)(this.m_Stride * this.m_Length % num) > 0U) {
                throw new InvalidOperationException("SliceConvert requires that Length * sizeof(T) is a multiple of sizeof(U).");
            }

            nativeSlice.m_MinIndex = 0;
            nativeSlice.m_MaxIndex = nativeSlice.m_Length - 1;
            nativeSlice.m_Safety = this.m_Safety;
            #endif
            return nativeSlice;
        }

        public unsafe NativeSliceBurst<U> SliceWithStride<U>(int offset) where U : struct {
            NativeSliceBurst<U> nativeSlice;
            nativeSlice.m_Buffer = this.m_Buffer + offset;
            nativeSlice.m_Stride = this.m_Stride;
            nativeSlice.m_Length = this.m_Length;

            #if COLLECTIONS_CHECKS
            if (offset < 0) {
                throw new ArgumentOutOfRangeException(nameof(offset), "SliceWithStride offset must be >= 0");
            }

            if (offset + UnsafeUtility.SizeOf<U>() > UnsafeUtility.SizeOf<T>()) {
                throw new ArgumentException("SliceWithStride sizeof(U) + offset must be <= sizeof(T)", nameof(offset));
            }

            nativeSlice.m_MinIndex = this.m_MinIndex;
            nativeSlice.m_MaxIndex = this.m_MaxIndex;
            nativeSlice.m_Safety = this.m_Safety;
            #endif
            return nativeSlice;
        }

        public NativeSliceBurst<U> SliceWithStride<U>() where U : struct {
            return this.SliceWithStride<U>(0);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private unsafe void CheckReadIndex(int index) {
            #if COLLECTIONS_CHECKS
            if (index < this.m_MinIndex || index > this.m_MaxIndex) {
                this.FailOutOfRangeError(index);
            }

            if (this.m_Safety.version == (*(int*)(void*)this.m_Safety.versionNode & -7)) {
                return;
            }

            AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut(this.m_Safety);
            #endif
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private unsafe void CheckWriteIndex(int index) {
            #if COLLECTIONS_CHECKS
            if (index < this.m_MinIndex || index > this.m_MaxIndex) {
                this.FailOutOfRangeError(index);
            }

            if (this.m_Safety.version == (*(int*)(void*)this.m_Safety.versionNode & -6)) {
                return;
            }

            AtomicSafetyHandle.CheckWriteAndThrowNoEarlyOut(this.m_Safety);
            #endif
        }

        public unsafe T this[int index] {
            get {
                this.CheckReadIndex(index);
                return UnsafeUtility.ReadArrayElementWithStride<T>((void*)this.m_Buffer, index, this.m_Stride);
            }
            [WriteAccessRequired]
            set {
                this.CheckWriteIndex(index);
                UnsafeUtility.WriteArrayElementWithStride<T>((void*)this.m_Buffer, index, this.m_Stride, value);
            }
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void FailOutOfRangeError(int index) {
            #if COLLECTIONS_CHECKS
            if (index < this.Length && (this.m_MinIndex != 0 || this.m_MaxIndex != this.Length - 1)) {
                throw new IndexOutOfRangeException(
                    string.Format("Index {0} is out of restricted IJobParallelFor range [{1}...{2}] in ReadWriteBuffer.\n", (object)index, (object)this.m_MinIndex,
                                  (object)this.m_MaxIndex) +
                    "ReadWriteBuffers are restricted to only read & write the element at the job index. You can use double buffering strategies to avoid race conditions due to reading & writing in parallel to the same elements from a job.");
            }

            throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", (object)index, (object)this.Length));
            #endif
        }

        [WriteAccessRequired]
        public unsafe void CopyFrom(NativeSliceBurst<T> slice) {
            if (this.Length != slice.Length) {
                throw new ArgumentException(string.Format("slice.Length ({0}) does not match the Length of this instance ({1}).", (object)slice.Length, (object)this.Length),
                                            nameof(slice));
            }

            UnsafeUtility.MemCpyStride(this.GetUnsafePtr(), this.Stride, slice.GetUnsafeReadOnlyPtr(), slice.Stride, UnsafeUtility.SizeOf<T>(), this.m_Length);
        }

        [WriteAccessRequired]
        public unsafe void CopyFrom(T[] array) {
            if (this.Length != array.Length) {
                throw new ArgumentException(string.Format("array.Length ({0}) does not match the Length of this instance ({1}).", (object)array.Length, (object)this.Length),
                                            nameof(array));
            }

            var gcHandle = GCHandle.Alloc((object)array, GCHandleType.Pinned);
            var num1 = gcHandle.AddrOfPinnedObject();
            var num2 = UnsafeUtility.SizeOf<T>();
            UnsafeUtility.MemCpyStride(this.GetUnsafePtr(), this.Stride, (void*)num1, num2, num2, this.m_Length);
            gcHandle.Free();
        }

        public unsafe void CopyTo(NativeArray<T> array) {
            if (this.Length != array.Length) {
                throw new ArgumentException(string.Format("array.Length ({0}) does not match the Length of this instance ({1}).", (object)array.Length, (object)this.Length),
                                            nameof(array));
            }

            var num = UnsafeUtility.SizeOf<T>();
            UnsafeUtility.MemCpyStride(array.GetUnsafePtr<T>(), num, this.GetUnsafeReadOnlyPtr(), this.Stride, num, this.m_Length);
        }

        public unsafe void CopyTo(T[] array) {
            if (this.Length != array.Length) {
                throw new ArgumentException(string.Format("array.Length ({0}) does not match the Length of this instance ({1}).", (object)array.Length, (object)this.Length),
                                            nameof(array));
            }

            var gcHandle = GCHandle.Alloc((object)array, GCHandleType.Pinned);
            var num1 = gcHandle.AddrOfPinnedObject();
            var num2 = UnsafeUtility.SizeOf<T>();
            UnsafeUtility.MemCpyStride((void*)num1, num2, this.GetUnsafeReadOnlyPtr(), this.Stride, num2, this.m_Length);
            gcHandle.Free();
        }

        public T[] ToArray() {
            var array = new T[this.Length];
            this.CopyTo(array);
            return array;
        }

        public int Stride => this.m_Stride;

        public int Length => this.m_Length;

        public NativeSliceBurst<T>.Enumerator GetEnumerator() {
            return new NativeSliceBurst<T>.Enumerator(ref this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return (IEnumerator<T>)new NativeSliceBurst<T>.Enumerator(ref this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return (IEnumerator)this.GetEnumerator();
        }

        public unsafe bool Equals(NativeSliceBurst<T> other) {
            return this.m_Buffer == other.m_Buffer && this.m_Stride == other.m_Stride && this.m_Length == other.m_Length;
        }

        public override bool Equals(object obj) {
            return obj != null && obj is NativeSliceBurst<T> other && this.Equals(other);
        }

        public override unsafe int GetHashCode() {
            return ((((int)this.m_Buffer * 397) ^ this.m_Stride) * 397) ^ this.m_Length;
        }

        public static bool operator ==(NativeSliceBurst<T> left, NativeSliceBurst<T> right) {
            return left.Equals(right);
        }

        public static bool operator !=(NativeSliceBurst<T> left, NativeSliceBurst<T> right) {
            return !left.Equals(right);
        }

        public unsafe void* GetUnsafePtr() {
            return this.m_Buffer;
        }

        public unsafe void* GetUnsafeReadOnlyPtr() {
            return this.m_Buffer;
        }

        [ExcludeFromDocs]
        public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable {

            private NativeSliceBurst<T> m_Array;
            private int m_Index;

            public Enumerator(ref NativeSliceBurst<T> array) {
                this.m_Array = array;
                this.m_Index = -1;
            }

            public void Dispose() { }

            public bool MoveNext() {
                ++this.m_Index;
                return this.m_Index < this.m_Array.Length;
            }

            public void Reset() {
                this.m_Index = -1;
            }

            public T Current => this.m_Array[this.m_Index];

            object IEnumerator.Current => (object)this.Current;

        }

    }

}
#endif