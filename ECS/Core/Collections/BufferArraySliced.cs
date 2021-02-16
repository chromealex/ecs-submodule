#define EDITOR_ARRAY
using System.Collections;
using System.Collections.Generic;

namespace ME.ECS.Collections {

    public interface IBufferArraySliced {

        int Length { get; }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public readonly struct BufferArraySliced<T> : IBufferArraySliced {

        private const int BUCKET_SIZE = 4;

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
        private BufferArraySliced(BufferArray<T> arr, BufferArray<BufferArray<T>> tails) {

            this.isCreated = true;
            this.data = arr;
            this.tails = tails;
            this.tailsLength = 0;
            for (int i = 0, length = this.tails.Length; i < length; ++i) {
            
                var tail = tails.arr[i];
                this.tailsLength += tail.Length;

            }

        }

        public ref T this[int index] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                if (this.tails.isCreated == true && index >= this.data.Length) {

                    // Look into tails
                    index -= this.data.Length;
                    for (int i = 0, length = this.tails.Length; i < length; ++i) {

                        ref var tail = ref this.tails.arr[i];
                        var len = tail.arr.Length;
                        if (index >= len) {
                            
                            index -= len;
                            continue;
                            
                        }
                        
                        return ref tail.arr[index];
                        
                    }
                    
                }

                return ref this.data.arr[index];
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        private struct ArrayCopy<TCopy> : IArrayElementCopy<BufferArray<T>> where TCopy : IArrayElementCopyWithIndex<T> {

            public TCopy elementCopy;

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Copy(BufferArray<T> from, ref BufferArray<T> to) {
                
                ArrayUtils.CopyWithIndex(from, ref to, this.elementCopy);
                
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Recycle(BufferArray<T> item) {
                
                ArrayUtils.RecycleWithIndex(ref item, this.elementCopy);
                
            }

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        private struct ArrayCopy : IArrayElementCopy<BufferArray<T>> {
        
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Copy(BufferArray<T> from, ref BufferArray<T> to) {
                
                ArrayUtils.Copy(from, ref to);
                
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Recycle(BufferArray<T> item) {

                item.Dispose();

            }
            
        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArraySliced<T> CopyFrom<TCopy>(in BufferArraySliced<T> other, in TCopy copy) where TCopy : IArrayElementCopyWithIndex<T> {
            
            var data = this.data;
            //var tails = this.tails;
            ArrayUtils.CopyWithIndex(other.data, ref data, copy);
            //ArrayUtils.Copy(other.tails, ref tails, new ArrayCopy<TCopy>() { elementCopy = copy });
            return new BufferArraySliced<T>(data);
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArraySliced<T> CopyFrom(in BufferArraySliced<T> other) {

            var data = this.data;
            //var tails = this.tails;
            ArrayUtils.Copy(in other.data, ref data);
            //ArrayUtils.Copy(other.tails, ref tails, new ArrayCopy());
            return new BufferArraySliced<T>(data);

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArraySliced<T> Resize(int index, bool resizeWithOffset, out bool result) {

            if (index >= this.Length) {

                // Do we need any tail?
                var tails = this.tails;
                // Look into tails
                var ptr = this.data.Length;
                for (int i = 0, length = this.tails.Length; i < length; ++i) {

                    // For each tail determine do we need to resize any tail to store index?
                    var tail = tails.arr[i];
                    ptr += tail.arr.Length;
                    if (index >= ptr) continue;

                    // We have found tail without resize needed
                    // Tail was found - we do not need to resize
                    result = false;
                    return this;

                }

                // Need to add new tail and resize tails container
                var idx = tails.Length;
                var size = this.Length;
                ArrayUtils.Resize(idx, ref tails, resizeWithOffset);
                var bucketSize = index + BufferArraySliced<T>.BUCKET_SIZE - size;
                tails.arr[idx] = PoolArray<T>.Spawn(bucketSize, realSize: true);
                result = true;
                return new BufferArraySliced<T>(this.data, tails);

            }
            
            // We dont need to resize any
            result = false;
            return this;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArraySliced<T> Merge() {

            if (this.tails.isCreated == false) {

                return this;

            }

            var arr = PoolArray<T>.Spawn(this.Length);
            if (this.data.isCreated == true) System.Array.Copy(this.data.arr, 0, arr.arr, 0, this.data.Length);
            var ptr = this.data.Length;
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                ref var tail = ref this.tails.arr[i];
                if (tail.isCreated == false) continue;
                
                System.Array.Copy(tail.arr, 0, arr.arr, ptr, tail.arr.Length);
                ptr += tail.arr.Length;

            }

            this.Dispose();
            return new BufferArraySliced<T>(arr);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public BufferArraySliced<T> Dispose() {
            
            this.data.Dispose();
            for (int i = 0, length = this.tails.Length; i < length; ++i) {

                var tail = this.tails.arr[i];
                this.tails.arr[i] = tail.Dispose();

            }
            this.tails.Dispose();
            
            return new BufferArraySliced<T>();
            
        }

    }
    
}
