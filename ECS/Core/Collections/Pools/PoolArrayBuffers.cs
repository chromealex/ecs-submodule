namespace ME.ECS.Buffers {

    using System.Threading;
    using ArgumentException = System.ArgumentException;
    using ArgumentNullException = System.ArgumentNullException;
    using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;
    using Array = System.Array;
    using Debugger = System.Diagnostics.Debugger;
    using SpinLock = System.Threading.SpinLock;

    public static class ArrayPools {

        public static System.Collections.Generic.HashSet<ArrayPoolBase> pools = new System.Collections.Generic.HashSet<ArrayPoolBase>();

    }

    public abstract class ArrayPoolBase {

        public abstract void Clear();

    }
    
    public abstract class ArrayPool<T> : ArrayPoolBase {

        private static ArrayPool<T> s_sharedInstance;

        public static ArrayPool<T> Shared {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => Volatile.Read<ArrayPool<T>>(ref ArrayPool<T>.s_sharedInstance) ?? ArrayPool<T>.EnsureSharedCreated();
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static ArrayPool<T> EnsureSharedCreated() {
            Interlocked.CompareExchange<ArrayPool<T>>(ref ArrayPool<T>.s_sharedInstance, ArrayPool<T>.Create(), (ArrayPool<T>)null);
            return ArrayPool<T>.s_sharedInstance;
        }

        public static ArrayPool<T> Create() {
            return (ArrayPool<T>)new DefaultArrayPool<T>();
        }

        public static ArrayPool<T> Create(int maxArrayLength, int maxArraysPerBucket) {
            return (ArrayPool<T>)new DefaultArrayPool<T>(maxArrayLength, maxArraysPerBucket);
        }

        public abstract T[] Rent(int minimumLength);

        public abstract void Return(T[] array, bool clearArray = true);

    }

    internal static class Utilities {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int SelectBucketIndex(int bufferSize) {
            var num1 = (uint)(bufferSize - 1) >> 4;
            var num2 = 0;
            if (num1 > (uint)ushort.MaxValue) {
                num1 >>= 16;
                num2 = 16;
            }

            if (num1 > (uint)byte.MaxValue) {
                num1 >>= 8;
                num2 += 8;
            }

            if (num1 > 15U) {
                num1 >>= 4;
                num2 += 4;
            }

            if (num1 > 3U) {
                num1 >>= 2;
                num2 += 2;
            }

            if (num1 > 1U) {
                num1 >>= 1;
                ++num2;
            }

            return num2 + (int)num1;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int GetMaxSizeForBucket(int binIndex) {
            return 16 << binIndex;
        }

    }

    internal sealed class ArrayPoolEventSource : System.Diagnostics.Tracing.EventSource {

        internal static readonly ArrayPoolEventSource Log = new ArrayPoolEventSource();

        [System.Diagnostics.Tracing.EventAttribute(1, Level = System.Diagnostics.Tracing.EventLevel.Verbose)]
        internal unsafe void BufferRented(int bufferId, int bufferSize, int poolId, int bucketId) {
            var data = stackalloc System.Diagnostics.Tracing.EventSource.EventData[4];
            data->Size = 4;
            data->DataPointer = (System.IntPtr)(void*)&bufferId;
            data[1].Size = 4;
            data[1].DataPointer = (System.IntPtr)(void*)&bufferSize;
            data[2].Size = 4;
            data[2].DataPointer = (System.IntPtr)(void*)&poolId;
            data[3].Size = 4;
            data[3].DataPointer = (System.IntPtr)(void*)&bucketId;
            this.WriteEventCore(1, 4, data);
        }

        [System.Diagnostics.Tracing.EventAttribute(2, Level = System.Diagnostics.Tracing.EventLevel.Informational)]
        internal unsafe void BufferAllocated(
            int bufferId,
            int bufferSize,
            int poolId,
            int bucketId,
            BufferAllocatedReason reason) {
            var data = stackalloc System.Diagnostics.Tracing.EventSource.EventData[5];
            data->Size = 4;
            data->DataPointer = (System.IntPtr)(void*)&bufferId;
            data[1].Size = 4;
            data[1].DataPointer = (System.IntPtr)(void*)&bufferSize;
            data[2].Size = 4;
            data[2].DataPointer = (System.IntPtr)(void*)&poolId;
            data[3].Size = 4;
            data[3].DataPointer = (System.IntPtr)(void*)&bucketId;
            data[4].Size = 4;
            data[4].DataPointer = (System.IntPtr)(void*)&reason;
            this.WriteEventCore(2, 5, data);
        }

        [System.Diagnostics.Tracing.EventAttribute(3, Level = System.Diagnostics.Tracing.EventLevel.Verbose)]
        internal void BufferReturned(int bufferId, int bufferSize, int poolId) {
            this.WriteEvent(3, bufferId, bufferSize, poolId);
        }

        internal enum BufferAllocatedReason {

            Pooled,
            OverMaximumSize,
            PoolExhausted,

        }

    }

    internal sealed class DefaultArrayPool<T> : ArrayPool<T> {

        private const int DefaultMaxArrayLength = 1048576;
        private const int DefaultMaxNumberOfArraysPerBucket = 50;
        private static T[] s_emptyArray;
        private readonly DefaultArrayPool<T>.Bucket[] _buckets;

        internal DefaultArrayPool()
            : this(1048576, 50) { }

        internal DefaultArrayPool(int maxArrayLength, int maxArraysPerBucket) {
            if (maxArrayLength <= 0) {
                throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
            }

            if (maxArraysPerBucket <= 0) {
                throw new ArgumentOutOfRangeException(nameof(maxArraysPerBucket));
            }

            if (maxArrayLength > 1073741824) {
                maxArrayLength = 1073741824;
            } else if (maxArrayLength < 16) {
                maxArrayLength = 16;
            }

            var id = this.Id;
            var bucketArray = new DefaultArrayPool<T>.Bucket[Utilities.SelectBucketIndex(maxArrayLength) + 1];
            for (var binIndex = 0; binIndex < bucketArray.Length; ++binIndex) {
                bucketArray[binIndex] = new DefaultArrayPool<T>.Bucket(Utilities.GetMaxSizeForBucket(binIndex), maxArraysPerBucket, id);
            }

            this._buckets = bucketArray;
        }

        private int Id => this.GetHashCode();

        public override void Clear() {

            for (int i = 0; i < this._buckets.Length; ++i) {
                
                if (this._buckets[i] != null) this._buckets[i].Clear();
                
            }
            
        }

        public override T[] Rent(int minimumLength) {
            if (minimumLength < 0) {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            if (minimumLength == 0) {
                return DefaultArrayPool<T>.s_emptyArray ?? (DefaultArrayPool<T>.s_emptyArray = new T[0]);
            }

            if (ArrayPools.pools.Contains(this) == false) {

                ArrayPools.pools.Add(this);

            }

            var log = ArrayPoolEventSource.Log;
            var index1 = Utilities.SelectBucketIndex(minimumLength);
            T[] objArray1;
            if (index1 < this._buckets.Length) {
                var index2 = index1;
                do {
                    var objArray2 = this._buckets[index2].Rent();
                    if (objArray2 != null) {
                        if (log.IsEnabled()) {
                            log.BufferRented(objArray2.GetHashCode(), objArray2.Length, this.Id, this._buckets[index2].Id);
                        }

                        return objArray2;
                    }
                } while (++index2 < this._buckets.Length && index2 != index1 + 2);

                objArray1 = new T[this._buckets[index1]._bufferLength];
            } else {
                objArray1 = new T[minimumLength];
            }

            if (log.IsEnabled()) {
                var hashCode = objArray1.GetHashCode();
                var bucketId = -1;
                log.BufferRented(hashCode, objArray1.Length, this.Id, bucketId);
                log.BufferAllocated(hashCode, objArray1.Length, this.Id, bucketId,
                                    index1 >= this._buckets.Length
                                        ? ArrayPoolEventSource.BufferAllocatedReason.OverMaximumSize
                                        : ArrayPoolEventSource.BufferAllocatedReason.PoolExhausted);
            }

            return objArray1;
        }

        public override void Return(T[] array, bool clearArray = true) {
            if (array == null) {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length == 0) {
                return;
            }

            var index = Utilities.SelectBucketIndex(array.Length);
            if (index < this._buckets.Length) {
                if (clearArray) {
                    Array.Clear((Array)array, 0, array.Length);
                }

                this._buckets[index].Return(array);
            }

            var log = ArrayPoolEventSource.Log;
            if (!log.IsEnabled()) {
                return;
            }

            log.BufferReturned(array.GetHashCode(), array.Length, this.Id);
        }

        private sealed class Bucket {

            internal readonly int _bufferLength;
            private readonly T[][] _buffers;
            private readonly int _poolId;
            private SpinLock _lock;
            private int _index;

            internal Bucket(int bufferLength, int numberOfBuffers, int poolId) {
                this._lock = new SpinLock(Debugger.IsAttached);
                this._buffers = new T[numberOfBuffers][];
                this._bufferLength = bufferLength;
                this._poolId = poolId;
            }

            internal int Id => this.GetHashCode();

            internal void Clear() {

                for (int i = 0; i < this._buffers.Length; ++i) {

                    this._buffers[i] = null;

                }
                
            }

            internal T[] Rent() {
                var buffers = this._buffers;
                var objArray = (T[])null;
                var lockTaken = false;
                var flag = false;
                try {
                    this._lock.Enter(ref lockTaken);
                    if (this._index < buffers.Length) {
                        objArray = buffers[this._index];
                        buffers[this._index++] = (T[])null;
                        flag = objArray == null;
                    }
                } finally {
                    if (lockTaken) {
                        this._lock.Exit(false);
                    }
                }

                if (flag) {
                    objArray = new T[this._bufferLength];
                    var log = ArrayPoolEventSource.Log;
                    if (log.IsEnabled()) {
                        log.BufferAllocated(objArray.GetHashCode(), this._bufferLength, this._poolId, this.Id, ArrayPoolEventSource.BufferAllocatedReason.Pooled);
                    }
                }

                return objArray;
            }

            internal void Return(T[] array) {
                if (array.Length != this._bufferLength) {
                    //throw new ArgumentException("Array is not from pool", nameof(array));
                    return;
                }

                var lockTaken = false;
                try {
                    this._lock.Enter(ref lockTaken);
                    if (this._index == 0) {
                        return;
                    }

                    this._buffers[--this._index] = array;
                } finally {
                    if (lockTaken) {
                        this._lock.Exit(false);
                    }
                }
            }

        }

    }

}