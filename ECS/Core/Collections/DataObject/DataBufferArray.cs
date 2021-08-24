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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clone(BufferArray<T> from, ref BufferArray<T> to) {

            to = PoolArray<T>.Spawn(from.Length);
            ArrayUtils.Copy(in from, ref to);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
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
    public struct NativeDataBufferArrayProvider<T> : IDataObjectProvider<NativeBufferArray<T>> where T : struct {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clone(NativeBufferArray<T> from, ref NativeBufferArray<T> to) {

            to = NativeBufferArray<T>.From(from);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Recycle(ref NativeBufferArray<T> value) {

            if (value != null) {
                value.Dispose();
            }

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [GeneratorIgnoreManagedType]
    public struct NativeDataBufferArray<T> : IDataObject<NativeBufferArray<T>> where T : struct {

        public int Length;

        [ME.ECS.Serializer.SerializeField]
        private DataObject<NativeBufferArray<T>, NativeDataBufferArrayProvider<T>> dataObject;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsCreated() => this.dataObject.IsCreated();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeDataBufferArray(int length = 0) {

            var mode = Unity.Collections.NativeLeakDetection.Mode;
            Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
            this.dataObject = new DataObject<NativeBufferArray<T>, NativeDataBufferArrayProvider<T>>(new NativeBufferArray<T>(length));
            Unity.Collections.NativeLeakDetection.Mode = mode;
            this.Length = length;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeDataBufferArray(BufferArray<T> data) {

            var mode = Unity.Collections.NativeLeakDetection.Mode;
            Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
            this.dataObject = new DataObject<NativeBufferArray<T>, NativeDataBufferArrayProvider<T>>(new NativeBufferArray<T>(data));
            Unity.Collections.NativeLeakDetection.Mode = mode;
            this.Length = data.Length;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public NativeDataBufferArray(NativeBufferArray<T> data) {

            this.dataObject = new DataObject<NativeBufferArray<T>, NativeDataBufferArrayProvider<T>>(data);
            this.Length = data.Length;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public 
            #if CSHARP_8_OR_NEWER
            readonly
            #endif
            ref readonly NativeBufferArray<T> Read() {

            return ref this.dataObject.Read();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref NativeBufferArray<T> Get() {

            return ref this.dataObject.Get();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(NativeBufferArray<T> value) {

            this.dataObject.Set(value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {

            this.dataObject.Dispose();

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [GeneratorIgnoreManagedType]
    public struct DataBufferArray<T> : IDataObject<BufferArray<T>> {

        public int Length;

        [ME.ECS.Serializer.SerializeField]
        private DataObject<BufferArray<T>, DataBufferArrayProvider<T>> dataObject;

        public bool IsCreated() => this.dataObject.IsCreated();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataBufferArray(int length = 0) {

            this.dataObject = new DataObject<BufferArray<T>, DataBufferArrayProvider<T>>(PoolArray<T>.Spawn(length));
            this.Length = length;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataBufferArray(BufferArray<T> data) {

            this.dataObject = new DataObject<BufferArray<T>, DataBufferArrayProvider<T>>(data);
            this.Length = data.Length;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public 
            #if CSHARP_8_OR_NEWER
            readonly
            #endif
            ref readonly BufferArray<T> Read() {

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
