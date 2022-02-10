using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS {

    public unsafe struct UnsafeData {

        public System.IntPtr data;
        public int sizeOf;
        public int alignOf;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public UnsafeData Set<T>(T data) where T : struct {

            if (this.data != System.IntPtr.Zero) {
                
                NativeArrayUtils.Dispose(ref this.data);
                
            }
            
            this.sizeOf = UnsafeUtility.SizeOf<T>();
            this.alignOf = UnsafeUtility.AlignOf<T>();
            this.data = (System.IntPtr)UnsafeUtility.Malloc(this.sizeOf, this.alignOf, Unity.Collections.Allocator.Persistent);
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.WriteArrayElement((void*)this.data, 0, data);
            
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public T Read<T>() where T : struct {

            return Unity.Collections.LowLevel.Unsafe.UnsafeUtility.ReadArrayElement<T>((void*)this.data, 0);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void CopyFrom(in UnsafeData other) {

            this.sizeOf = other.sizeOf;
            this.alignOf = other.alignOf;
            NativeArrayUtils.Copy(other.data, ref this.data, this.sizeOf, this.alignOf);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {
            
            if (this.data != System.IntPtr.Zero) {

                this.sizeOf = default;
                this.alignOf = default;
                NativeArrayUtils.Dispose(ref this.data);
                
            }
            
        }

    }

}