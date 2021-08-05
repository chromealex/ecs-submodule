namespace ME.ECS.Collections {

    public interface IDataArray<T> {

        ref readonly T Read(int index);
        T this[int index] { get; set; }
        void Set(int index, T value);
        void Dispose();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [GeneratorIgnoreManagedType]
    public struct DataArray<T> : IDataArray<T> {

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        private class DisposeSentinel : System.IDisposable {

            public BufferArray<T> arr;
            public Tick tick;

            ~DisposeSentinel() {

                this.Dispose();

            }

            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            public void Dispose() {
                
                this.tick = Tick.Invalid;
                PoolClass<DisposeSentinel>.Recycle(this);
                PoolArray<T>.Recycle(ref this.arr);
                
            }
            
        }

        private DisposeSentinel disposeSentinel;
        public bool isCreated;
        public int Length;
        
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public DataArray(int length) {
            
            this.disposeSentinel = PoolClass<DisposeSentinel>.Spawn();
            this.disposeSentinel.arr = PoolArray<T>.Spawn(length);
            this.disposeSentinel.tick = Worlds.currentWorld.GetLastSavedTick();
            this.isCreated = true;
            this.Length = length;

        }

        #region Static Constructors
        public static DataArray<T> From(ListCopyable<T> source) {

            var data = new DataArray<T>(source.Count);
            ArrayUtils.Copy(source.innerArray, 0, ref data.disposeSentinel.arr, 0, source.Count);
            return data;

        }

        public static DataArray<T> From(BufferArray<T> source) {

            var data = new DataArray<T>(source.Length);
            ArrayUtils.Copy(source, 0, ref data.disposeSentinel.arr, 0, source.Length);
            return data;

        }

        public static DataArray<T> From(T[] source) {

            var data = new DataArray<T>(source.Length);
            ArrayUtils.Copy(source, 0, ref data.disposeSentinel.arr, 0, source.Length);
            return data;

        }
        #endregion

        public override int GetHashCode() {
            
            return this.disposeSentinel.arr.GetHashCode();
            
        }

        public T this[int index] {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return this.Read(index);
            }
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            set {
                this.Set(index, value);
            }
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ref readonly T Read(int index) {
            
            if (this.isCreated == false || index >= this.Length) throw new System.IndexOutOfRangeException($"Index: {index} [0..{this.Length}], Tick: {Worlds.currentWorld.GetCurrentTick()}");
            return ref this.disposeSentinel.arr[index];
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Set(int index, T value) {
            
            if (this.isCreated == false || index >= this.Length) throw new System.IndexOutOfRangeException($"Index: {index} [0..{this.Length}], Tick: {Worlds.currentWorld.GetCurrentTick()}");
            if (this.disposeSentinel.tick != Worlds.currentWorld.GetLastSavedTick()) this.CloneInternalArray();
            this.disposeSentinel.arr[index] = value;
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Dispose() {
            
            this.disposeSentinel.Dispose();
            this.isCreated = false;
            this.Length = 0;
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void CloneInternalArray() {

            this.disposeSentinel.tick = Worlds.currentWorld.GetCurrentTick();
            var arr = PoolArray<T>.Spawn(this.Length);
            ArrayUtils.Copy(in this.disposeSentinel.arr, ref arr);
            this.disposeSentinel = PoolClass<DisposeSentinel>.Spawn();
            this.disposeSentinel.arr = arr;
            
        }

    }
    
}