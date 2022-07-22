#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
#define EDITOR_ARRAY

namespace ME.ECS.Collections {

    public interface IBufferArraySliced {

        int Length { get; }

    }

    public static class BufferArraySlicedExt {
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArraySliced<T> CopyFrom<T, TCopy>(this in BufferArraySliced<T> src, in BufferArraySliced<T> other, in TCopy copy) where TCopy : IArrayElementCopy<T> where T : struct {

            var data = src.data;
            //var tails = this.tails;
            ArrayUtils.Copy(other.data, ref data, copy);
            //ArrayUtils.Copy(other.tails, ref tails, new ArrayCopy<TCopy>() { elementCopy = copy });
            return new BufferArraySliced<T>(data);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArraySliced<T> CopyFrom<T>(this in BufferArraySliced<T> src, in BufferArraySliced<T> other) where T : struct {

            var data = src.data;
            //var tails = this.tails;
            ArrayUtils.Copy(in other.data, ref data);
            //ArrayUtils.Copy(other.tails, ref tails, new ArrayCopy());
            return new BufferArraySliced<T>(data);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArraySliced<T> Resize<T>(this in BufferArraySliced<T> src, int index, bool resizeWithOffset, out bool result) where T : struct {

            if (index >= src.Length) {

                // Do we need any tail?
                var tails = src.tails;
                // Look into tails
                var ptr = src.data.Length;
                for (int i = 0, length = src.tails.Length; i < length; ++i) {

                    // For each tail determine do we need to resize any tail to store index?
                    var tail = tails.arr[i];
                    ptr += tail.Length;
                    if (index >= ptr) continue;

                    // We have found tail without resize needed
                    // Tail was found - we do not need to resize
                    result = false;
                    return src;

                }

                // Need to add new tail and resize tails container
                var idx = tails.Length;
                var size = src.Length;
                ArrayUtils.Resize(idx, ref tails, resizeWithOffset);
                var bucketSize = index + BufferArraySliced<T>.BUCKET_SIZE - size;
                tails.arr[idx] = PoolArray<T>.Spawn(bucketSize, realSize: false);
                result = true;
                return new BufferArraySliced<T>(src.data, tails);

            }

            // We dont need to resize any
            result = false;
            return src;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArraySliced<T> Merge<T>(this in BufferArraySliced<T> src) where T : struct {

            if (src.tails.isCreated == false || src.tails.Length == 0) {

                return src;

            }

            var arr = PoolArray<T>.Spawn(src.Length);
            if (src.data.isCreated == true) System.Array.Copy(src.data.arr, 0, arr.arr, 0, src.data.Length);
            var ptr = src.data.Length;
            for (int i = 0, length = src.tails.Length; i < length; ++i) {

                ref var tail = ref src.tails.arr[i];
                if (tail.isCreated == false) continue;

                System.Array.Copy(tail.arr, 0, arr.arr, ptr, tail.Length);
                ptr += tail.Length;

            }

            src.Dispose();
            return new BufferArraySliced<T>(arr);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BufferArraySliced<T> Dispose<T>(this in BufferArraySliced<T> src) where T : struct {

            src.data.Dispose();
            for (int i = 0, length = src.tails.Length; i < length; ++i) {

                var tail = src.tails.arr[i];
                src.tails.arr[i] = tail.Dispose();

            }

            src.tails.Dispose();

            return new BufferArraySliced<T>();

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public readonly struct BufferArraySliced<T> : IBufferArraySliced where T : struct {

        internal const int BUCKET_SIZE = 4;

        public readonly BufferArray<T> data;
        public readonly BufferArray<BufferArray<T>> tails;
        public readonly int tailsLength;
        public readonly bool isCreated;

        public int Length {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => this.data.Length + this.tailsLength;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArraySliced(BufferArray<T> arr) {

            this.isCreated = true;
            this.data = arr;
            this.tails = default;
            this.tailsLength = 0;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal BufferArraySliced(BufferArray<T> arr, BufferArray<BufferArray<T>> tails) {

            this.isCreated = true;
            this.data = arr;
            this.tails = tails;
            this.tailsLength = 0;
            for (int i = 0, length = tails.Length; i < length; ++i) {

                var tail = tails.arr[i];
                this.tailsLength += tail.Length;

            }

        }

        public ref T this[int index] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                var data = this.data;
                if (index >= data.Length) {

                    // Look into tails
                    var tails = this.tails;
                    var arr = tails.arr;
                    index -= data.Length;
                    for (int i = 0, length = tails.Length; i < length; ++i) {

                        ref var tail = ref arr[i];
                        var len = tail.Length;
                        if (index < len) return ref tail.arr[index];
                        index -= len;

                    }

                }

                return ref data.arr[index];
            }
        }

    }

}