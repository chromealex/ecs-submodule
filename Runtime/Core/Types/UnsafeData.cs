using Unity.Collections.LowLevel.Unsafe;

namespace ME.ECS {
    
    using MemPtr = System.Int64;

    public unsafe struct UnsafeData : ME.ECS.Collections.MemoryAllocator.IEquatableAllocator<UnsafeData> {

        public static System.Reflection.MethodInfo setMethodInfo = typeof(UnsafeData)
                                                                   .GetMethod("Set", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        public MemPtr data;
        public int sizeOf;
        public int alignOf;
        public int typeId;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public UnsafeData Set<T>(ref ME.ECS.Collections.V3.MemoryAllocator allocator, T data) where T : unmanaged {

            return this.SetAsUnmanaged(ref allocator, data);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        internal UnsafeData SetAsUnmanaged<T>(ref ME.ECS.Collections.V3.MemoryAllocator allocator, T data) where T : struct {

            this.typeId = AllComponentTypes<T>.typeId;

            if (this.data != 0) allocator.Free(this.data);
            
            this.sizeOf = UnsafeUtility.SizeOf<T>();
            this.alignOf = UnsafeUtility.AlignOf<T>();
            this.data = allocator.AllocUnmanaged<T>();
            allocator.RefUnmanaged<T>(this.data) = data;
            
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref T Get<T>(ref ME.ECS.Collections.V3.MemoryAllocator allocator) where T : struct {

            return ref allocator.RefUnmanaged<T>(this.data);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public T Read<T>(in ME.ECS.Collections.V3.MemoryAllocator allocator) where T : struct {

            return allocator.RefUnmanaged<T>(this.data);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose(ref ME.ECS.Collections.V3.MemoryAllocator allocator) {

            if (this.data != 0) allocator.Free(this.data);
            this = default;
            
        }

        public bool Equals(in ME.ECS.Collections.V3.MemoryAllocator allocator, UnsafeData other) {
            return this.sizeOf == other.sizeOf &&
                   this.alignOf == other.alignOf &&
                   this.typeId == other.typeId &&
                   this.EqualsData(in allocator, this.data, other.data);
        }

        private bool EqualsData(in ME.ECS.Collections.V3.MemoryAllocator allocator, MemPtr ptr1, MemPtr ptr2) {

            for (int i = 0; i < this.sizeOf; ++i) {
                if (allocator.GetUnsafePtr(ptr1 + i) != allocator.GetUnsafePtr(ptr2 + i)) {
                    return false;
                }
            }
            return true;

        }

        public int GetHash(in ME.ECS.Collections.V3.MemoryAllocator allocator) {
            unchecked {
                var hashCode = this.data.GetHashCode();
                hashCode = (hashCode * 397) ^ this.sizeOf;
                hashCode = (hashCode * 397) ^ this.alignOf;
                hashCode = (hashCode * 397) ^ this.typeId;
                return hashCode;
            }
        }

    }

    public unsafe struct UnsafeDataPtr {

        public void* data;
        public int sizeOf;
        public int alignOf;
        public int typeId;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public UnsafeDataPtr Set<T>(T data) where T : unmanaged {

            this.typeId = AllComponentTypes<T>.typeId;
            
            if (this.data != null) {
                
                NativeArrayUtils.Dispose(ref this.data);
                
            }

            this.sizeOf = UnsafeUtility.SizeOf<T>();
            this.alignOf = UnsafeUtility.AlignOf<T>();
            this.data = UnsafeUtility.Malloc(this.sizeOf, this.alignOf, Unity.Collections.Allocator.Persistent);
            Unity.Collections.LowLevel.Unsafe.UnsafeUtility.WriteArrayElement((void*)this.data, 0, data);
            
            return this;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref T Get<T>() where T : struct {

            return ref Unity.Collections.LowLevel.Unsafe.UnsafeUtility.ArrayElementAsRef<T>((void*)this.data, 0);

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
        public void Dispose() {
            
            if (this.data != null) {

                this.typeId = default;
                this.sizeOf = default;
                this.alignOf = default;
                NativeArrayUtils.Dispose(ref this.data);
                
            }
            
        }

    }

}