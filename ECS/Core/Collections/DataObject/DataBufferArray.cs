#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS.Collections {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct DataBufferArrayProvider<T> : IDataObjectProvider<BufferArray<T>> {

        public void Clone(BufferArray<T> from, ref BufferArray<T> to) {

            to = PoolArray<T>.Spawn(from.Length);
            ArrayUtils.Copy(in from, ref to);

        }

        public void Recycle(ref BufferArray<T> value) {

            if (value != null) {
                PoolArray<T>.Recycle(ref value);
            }

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [GeneratorIgnoreManagedType]
    public struct DataBufferArray<T> : IDataObject<BufferArray<T>> {

        [ME.ECS.Serializer.SerializeField]
        private DataObject<BufferArray<T>, DataBufferArrayProvider<T>> dataObject;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataBufferArray(int length = 0) {

            this.dataObject = new DataObject<BufferArray<T>, DataBufferArrayProvider<T>>(PoolArray<T>.Spawn(length));

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataBufferArray(BufferArray<T> data) {

            this.dataObject = new DataObject<BufferArray<T>, DataBufferArrayProvider<T>>(data);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly BufferArray<T> Read() {

            return ref this.dataObject.Read();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref BufferArray<T> Get() {

            return ref this.dataObject.Get();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(BufferArray<T> value) {

            this.dataObject.Set(value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {

            this.dataObject.Dispose();

        }

    }

}
