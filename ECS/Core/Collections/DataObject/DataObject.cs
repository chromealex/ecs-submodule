#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS.Collections {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct DataObjectDefaultProvider<T> : IDataObjectProvider<T> where T : struct {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Clone(T from, ref T to) {

            to = from;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Recycle(ref T value) {

            value = default;

        }

    }

    public interface IDataObjectProvider<T> {

        void Clone(T from, ref T to);
        void Recycle(ref T value);

    }

    public interface IDataObject<T> {

        ref readonly T Read();
        ref T Get();
        void Set(T value);
        void Dispose();
        bool IsCreated();

    }

    public interface IDisposeSentinel {
        
        object GetData();
        void SetData(object data);
        Tick GetTick();
        void SetTick(Tick tick);
        
    }

    public class DisposeSentinel<T, TProvider> : System.IDisposable, IDisposeSentinel where TProvider : struct, IDataObjectProvider<T> {

        public T data;
        public Tick tick;

        object IDisposeSentinel.GetData() {
            return this.data;
        }

        void IDisposeSentinel.SetData(object data) {
            this.data = (T)data; 
        }

        Tick IDisposeSentinel.GetTick() {
            return this.tick;
        }

        void IDisposeSentinel.SetTick(Tick tick) {
            this.tick = tick;
        }

        ~DisposeSentinel() {

            if (Unity.Collections.NativeLeakDetection.Mode != Unity.Collections.NativeLeakDetectionMode.Disabled) UnityEngine.Debug.Log($"Object collected. Suppress and reused. Data: {this.data}");
            this.Dispose();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {

            this.tick = Tick.Invalid;
            PoolClass<DisposeSentinel<T, TProvider>>.Recycle(this);
            default(TProvider).Recycle(ref this.data);

        }
    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [GeneratorIgnoreManagedType]
    public struct DataObject<T, TProvider> : IDataObject<T> where TProvider : struct, IDataObjectProvider<T> {

        [ME.ECS.Serializer.SerializeField]
        private DisposeSentinel<T, TProvider> disposeSentinel;
        [ME.ECS.Serializer.SerializeField]
        private bool isCreated;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsCreated() => this.isCreated;

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataObject(T data) {

            this.disposeSentinel = PoolClass<DisposeSentinel<T, TProvider>>.Spawn();
            this.disposeSentinel.data = data;
            this.disposeSentinel.tick = Worlds.currentWorld.GetLastSavedTick();
            this.isCreated = true;
            
        }

        public override int GetHashCode() {
            
            if (this.isCreated == false) return 0;
            return this.disposeSentinel.data.GetHashCode();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public
            #if CSHARP_8_OR_NEWER
            readonly
            #endif
            ref readonly T Read() {

            if (this.isCreated == false) {
                throw new System.Exception($"Try to read collection that has been already disposed. Tick: {Worlds.currentWorld.GetCurrentTick()}");
            }

            return ref this.disposeSentinel.data;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref T Get() {

            if (this.isCreated == false) {
                throw new System.Exception($"Try to read collection that has been already disposed. Tick: {Worlds.currentWorld.GetCurrentTick()}");
            }

            if (this.disposeSentinel.tick == Tick.Invalid || this.disposeSentinel.tick != Worlds.currentWorld.GetLastSavedTick()) {
                this.CloneInternalArray();
            }

            return ref this.disposeSentinel.data;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(T value) {

            if (this.isCreated == false) {
                throw new System.Exception($"Try to read collection that has been already disposed. Tick: {Worlds.currentWorld.GetCurrentTick()}");
            }

            if (this.disposeSentinel.tick == Tick.Invalid || this.disposeSentinel.tick != Worlds.currentWorld.GetLastSavedTick()) {
                this.CloneInternalArray();
            } else {
                default(TProvider).Recycle(ref this.disposeSentinel.data);
            }
            this.disposeSentinel.data = value;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {

            this.disposeSentinel.Dispose();
            this.isCreated = false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void CloneInternalArray() {

            var lastTick = Worlds.currentWorld.GetLastSavedTick();
            this.disposeSentinel.tick = lastTick == Tick.Invalid ? Tick.Zero : lastTick;
            var previousData = this.disposeSentinel.data;
            this.disposeSentinel = PoolClass<DisposeSentinel<T, TProvider>>.Spawn();
            default(TProvider).Clone(previousData, ref this.disposeSentinel.data);

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [GeneratorIgnoreManagedType]
    public struct DataObject<T> : IDataObject<T> where T : struct {

        private DataObject<T, DataObjectDefaultProvider<T>> dataObject;
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool IsCreated() => this.dataObject.IsCreated();

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataObject(T data) {

            this.dataObject = new DataObject<T, DataObjectDefaultProvider<T>>(data);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly T Read() {

            return ref this.dataObject.Read();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref T Get() {

            return ref this.dataObject.Get();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(T value) {

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
