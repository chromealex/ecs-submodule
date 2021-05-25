#if UNITY_EDITOR
//#define COLLECTIONS_CHECKS
#endif
using System.Diagnostics;
using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;

namespace ME.ECSBurst.Collections {

    // Marks our struct as a NativeContainer.
    // If ENABLE_UNITY_COLLECTIONS_CHECKS is enabled,
    // it is required that m_Safety & m_DisposeSentinel are declared, with exactly these names.
    #if COLLECTIONS_CHECKS
    [NativeContainer]
    #endif
    // The [NativeContainerSupportsMinMaxWriteRestriction] enables
    // a common jobification pattern where an IJobParallelFor is split into ranges
    // And the job is only allowed to access the index range being Executed by that worker thread.
    // Effectively limiting access of the array to the specific index passed into the Execute(int index) method
    // This attribute requires m_MinIndex & m_MaxIndex to exist.
    // and the container is expected to perform out of bounds checks against it.
    // m_MinIndex & m_MaxIndex will be set by the job scheduler before Execute is called on the worker thread.
    [NativeContainerSupportsMinMaxWriteRestriction]
    // It is recommended to always implement a Debugger proxy
    // to visualize the contents of the array in VisualStudio and other tools.
    [DebuggerDisplay("Length = {Length}")]
    [DebuggerTypeProxy(typeof(NativeArrayBurstDebugView<>))]
    public unsafe struct NativeArrayBurst<T> : IDisposable where T : struct {

        [NativeDisableUnsafePtrRestriction]
        internal void* m_Buffer;
        internal int m_Length;

        #if COLLECTIONS_CHECKS
        internal int m_MinIndex;
        internal int m_MaxIndex;
        internal AtomicSafetyHandle m_Safety;
        [NativeSetClassTypeToNullOnSchedule]
        internal DisposeSentinel m_DisposeSentinel;
        #endif

        internal Allocator m_AllocatorLabel;

        public void* GetUnsafePtr() {

            return this.m_Buffer;

        }

        public NativeArrayBurst(NativeArray<T> array, Allocator allocator) {
        
            NativeArrayBurst<T>.Allocate(array.Length, allocator, out this);
            ArrayUtils.Copy(array, ref this);
            
        }

        public NativeArrayBurst(NativeArrayBurst<T> array, Allocator allocator) {
        
            NativeArrayBurst<T>.Allocate(array.Length, allocator, out this);
            ArrayUtils.Copy(array, ref this);
            
        }

        public NativeArrayBurst(T[] array, Allocator allocator) {
            if (array == null)
                throw new ArgumentNullException(nameof (array));
            NativeArrayBurst<T>.Allocate(array.Length, allocator, out this);
            NativeArrayBurst<T>.Copy(array, this);
        }

        public NativeArrayBurst(int length, Allocator allocator) {
            
            NativeArrayBurst<T>.Allocate(length, allocator, out this);
            
        }
        
        private static unsafe void Allocate(int length, Allocator allocator, out NativeArrayBurst<T> array) {
            long totalSize = (long)((ulong)UnsafeUtility.SizeOf<T>() * (ulong)length);

            #if COLLECTIONS_CHECKS
            // Native allocation is only valid for Temp, Job and Persistent
            if (allocator <= Allocator.None) throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", "allocator");
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be >= 0");
            if (!UnsafeUtility.IsBlittable<T>()) throw new ArgumentException(string.Format("{0} used in NativeArrayBurst<{0}> must be blittable", typeof(T)));
            #endif

            array.m_Buffer = UnsafeUtility.Malloc(totalSize, UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.MemClear(array.m_Buffer, totalSize);

            array.m_Length = length;
            array.m_AllocatorLabel = allocator;

            #if COLLECTIONS_CHECKS
            array.m_MinIndex = 0;
            array.m_MaxIndex = length - 1;
            DisposeSentinel.Create(out array.m_Safety, out array.m_DisposeSentinel, 1, allocator);
            #endif
        }

        public int Length {
            get {
                return m_Length;
            }
        }

        public unsafe ref T this[int index] {
            get {
                #if COLLECTIONS_CHECKS
                // If the container is currently not allowed to read from the buffer
                // then this will throw an exception.
                // This handles all cases, from already disposed containers
                // to safe multithreaded access.
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety);

                // Perform out of range checks based on
                // the NativeContainerSupportsMinMaxWriteRestriction policy
                if (index < m_MinIndex || index > m_MaxIndex) FailOutOfRangeError(index);
                #endif
                // Read the element from the allocated native memory
                //return UnsafeUtility.ReadArrayElement<T>(m_Buffer, index);
                return ref this.GetRef(index);
            }
            /*[WriteAccessRequired]
            set {
                #if COLLECTIONS_CHECKS
                // If the container is currently not allowed to write to the buffer
                // then this will throw an exception.
                // This handles all cases, from already disposed containers
                // to safe multithreaded access.
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);

                // Perform out of range checks based on
                // the NativeContainerSupportsMinMaxWriteRestriction policy
                if (index < m_MinIndex || index > m_MaxIndex) FailOutOfRangeError(index);
                #endif
                // Writes value to the allocated native memory
                UnsafeUtility.WriteArrayElement(m_Buffer, index, value);
            }*/
        }
        
        [BurstCompatible(GenericTypeArguments = new [] { typeof(int), typeof(int) })]
        public static int IndexOf<TI, UI>(void* ptr, int length, UI value) where TI : struct, IEquatable<UI>
        {
            for (int i = 0; i != length; i++)
            {
                if (UnsafeUtility.ReadArrayElement<TI>(ptr, i).Equals(value))
                    return i;
            }
            return -1;
        }

        [BurstCompatible(GenericTypeArguments = new[] { typeof(int) }), System.Diagnostics.Contracts.PureAttribute]
        public int IndexOf<TI>(TI value) where TI : struct, IComparable<TI>
        {
            for (int i = 0; i != this.Length; i++)
            {
                if (UnsafeUtility.ReadArrayElement<TI>(this.m_Buffer, i).CompareTo(value) == 0)
                    return i;
            }
            return -1;
        }

        public static void Copy(T[] src, NativeArrayBurst<T> dst) {
            #if COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(dst.m_Safety);
            if (src.Length != dst.Length)
                throw new ArgumentException("source and destination length must be the same");
            #endif
            NativeArrayBurst<T>.Copy(src, 0, dst, 0, src.Length);
        }

        public static void Copy(T[] src, NativeArrayBurst<T> dst, int length) => NativeArrayBurst<T>.Copy(src, 0, dst, 0, length);

        public static void Copy(
            T[] src,
            int srcIndex,
            NativeArrayBurst<T> dst,
            int dstIndex,
            int length)
        {
            #if COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(dst.m_Safety);
            #endif
            if (src == null)
                throw new ArgumentNullException(nameof (src));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof (length), "length must be equal or greater than zero.");
            if (srcIndex < 0 || srcIndex > src.Length || srcIndex == src.Length && (uint) src.Length > 0U)
                throw new ArgumentOutOfRangeException(nameof (srcIndex), "srcIndex is outside the range of valid indexes for the source array.");
            if (dstIndex < 0 || dstIndex > dst.Length || dstIndex == dst.Length && dst.Length > 0)
                throw new ArgumentOutOfRangeException(nameof (dstIndex), "dstIndex is outside the range of valid indexes for the destination NativeArray.");
            if (srcIndex + length > src.Length)
                throw new ArgumentException("length is greater than the number of elements from srcIndex to the end of the source array.", nameof (length));
            if (dstIndex + length > dst.Length)
                throw new ArgumentException("length is greater than the number of elements from dstIndex to the end of the destination NativeArray.", nameof (length));
            var gcHandle = System.Runtime.InteropServices.GCHandle.Alloc(src, System.Runtime.InteropServices.GCHandleType.Pinned);
            IntPtr num = gcHandle.AddrOfPinnedObject();
            UnsafeUtility.MemCpy((void*) ((IntPtr) dst.m_Buffer + dstIndex * UnsafeUtility.SizeOf<T>()), (void*) ((IntPtr) (void*) num + srcIndex * UnsafeUtility.SizeOf<T>()), (long) (length * UnsafeUtility.SizeOf<T>()));
            gcHandle.Free();
        }

        public T[] ToArray() {
            #if COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
            #endif

            var array = new T[Length];
            for (var i = 0; i < Length; i++) array[i] = UnsafeUtility.ReadArrayElement<T>(m_Buffer, i);
            return array;
        }

        public bool IsCreated {
            get {
                return (IntPtr)m_Buffer != IntPtr.Zero;
            }
        }

        [WriteAccessRequired]
        public void Dispose() {
            #if COLLECTIONS_CHECKS
            DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
            #endif

            UnsafeUtility.Free(m_Buffer, m_AllocatorLabel);
            m_Buffer = (void*)null;
            m_Length = 0;
        }

        #if COLLECTIONS_CHECKS
        private void FailOutOfRangeError(int index) {
            if (index < Length && (m_MinIndex != 0 || m_MaxIndex != Length - 1))
                throw new IndexOutOfRangeException(string.Format(
                                                       "Index {0} is out of restricted IJobParallelFor range [{1}...{2}] in ReadWriteBuffer.\n" +
                                                       "ReadWriteBuffers are restricted to only read & write the element at the job index. " +
                                                       "You can use double buffering strategies to avoid race conditions due to " +
                                                       "reading & writing in parallel to the same elements from a job.",
                                                       index, m_MinIndex, m_MaxIndex));

            throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", index, Length));
        }

        #endif

    }

    // Visualizes the custom array in the C# debugger
    internal sealed class NativeArrayBurstDebugView<T> where T : struct {

        private NativeArrayBurst<T> m_Array;

        public NativeArrayBurstDebugView(NativeArrayBurst<T> array) {
            m_Array = array;
        }

        public T[] Items {
            get {
                return m_Array.ToArray();
            }
        }

    }

}