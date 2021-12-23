using ME.ECS.Collections;
using ME.ECS.Extensions;

namespace ME.ECS {

    using System.Runtime.CompilerServices;
    using Unity.Collections.LowLevel.Unsafe;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static unsafe class MemUtilsCuts {

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static T mcall<T>(void* methodPtr) {
            
            return System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<T>((System.IntPtr)methodPtr);

        }
    
        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref T mref<T>(void* ptr) where T : struct => ref UnsafeUtility.AsRef<T>(ptr);

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void free(ref void* ptr, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) {
            
            //UnsafeUtility.Free(ptr, allocator);
            System.Runtime.InteropServices.Marshal.FreeHGlobal((System.IntPtr)ptr);
            ptr = null;
            
        }

        public static void free<T>(ref T* ptr, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {
            
            //UnsafeUtility.Free(ptr, allocator);
            System.Runtime.InteropServices.Marshal.FreeHGlobal((System.IntPtr)ptr);
            ptr = null;
            
        }

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void* pnew<T>(ref T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            return MemUtils.CreateFromStruct(ref source, allocator);

        }

        public static void* pnew<T>(Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            return MemUtils.CreateFromStruct<T>(allocator);

        }

        public static void* pnew<T>(ref void* ptr, ref T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            return MemUtils.CreateFromStruct(ref ptr, ref source, allocator);

        }

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static T* tnew<T>(ref T* ptr, ref T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            return MemUtils.Create(ref ptr, ref source, allocator);

        }

        public static T* tnew<T>(ref T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            return MemUtils.Create(ref source, allocator);

        }

        public static T* tnew<T>(Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            return MemUtils.Create<T>(allocator);

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static unsafe class MemUtils {

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static T* Create<T>(ref T* ptr, ref T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            var size = UnsafeUtility.SizeOf<T>();
            ptr = (T*)System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            //ptr = (T*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.CopyStructureToPtr(ref source, ptr);
            return ptr;

        }

        public static T* Create<T>(ref T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            var size = UnsafeUtility.SizeOf<T>();
            var ptr = (T*)System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            //var ptr = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.CopyStructureToPtr(ref source, ptr);
            return ptr;

        }

        public static T* Create<T>(Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            var size = UnsafeUtility.SizeOf<T>();
            var ptr = (T*)System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            //var ptr = UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.MemClear(ptr, size);
            return ptr;

        }

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void* CreateFromStruct<T>(ref T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            var size = UnsafeUtility.SizeOf<T>();
            var ptr = (void*)System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            //var ptr = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.CopyStructureToPtr(ref source, ptr);
            return ptr;

        }
        public static void* CreateFromStruct<T>(Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            var size = UnsafeUtility.SizeOf<T>();
            //var ptr = (void*)System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            var ptr = UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.MemClear(ptr, size);
            return ptr;

        }

        public static void* CreateFromStruct<T>(ref void* ptr, ref T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            var size = UnsafeUtility.SizeOf<T>();
            ptr = (void*)System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            //ptr = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.CopyStructureToPtr(ref source, ptr);
            return ptr;

        }

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref T Ref<T>(void* ptr) where T : struct => ref UnsafeUtility.AsRef<T>(ptr);

    }
    
}

namespace ME.ECS {
    
    using Unity.Collections.LowLevel.Unsafe;
    using System.Runtime.InteropServices;
    using Collections;
    using static MemUtilsCuts;

    [StructLayout(LayoutKind.Sequential)]
    public struct StructComponentsItem<T> where T : struct {

        private NativeBufferArray<bool> dataExists;
        private NativeBufferArray<T> data;

        public bool Has(int entityId) {

            return this.dataExists[entityId];

        }

        public ref T Get(int entityId) {

            this.dataExists[entityId] = true;
            return ref this.data[entityId];

        }

        public ref readonly T Read(int entityId) {

            return ref this.data[entityId];

        }

        public void Set(int entityId, T data) {
            
            this.data[entityId] = data;
            this.dataExists[entityId] = true;

        }

        public bool Remove(int entityId) {
            
            ref var state = ref this.dataExists[entityId];
            var prevState = state;
            this.data[entityId] = default;
            state = false;
            return prevState;

        }

        public void Validate(int entityId) {

            var mode = Unity.Collections.NativeLeakDetection.Mode;
            Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled; 
            NativeArrayUtils.Resize(entityId, ref this.data);
            NativeArrayUtils.Resize(entityId, ref this.dataExists);
            Unity.Collections.NativeLeakDetection.Mode = mode;

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct StructComponentsItemUnknown {

        public NativeBufferArray<bool> dataExists;
        public NativeBufferArray<byte> data;
        
        public void Dispose() {

            if (this.data.isCreated == true) {
                this.data.Dispose();
            }

            if (this.dataExists.isCreated == true) {
                this.dataExists.Dispose();
            }

        }

    }

    public unsafe struct UnmanagedComponentsStorage {

        public NativeBufferArray<System.IntPtr> list;
        
        public void Initialize() { }

        public void Dispose() {

            for (int i = 0; i < this.list.Length; ++i) {

                var ptr = (void*)this.list[i];
                if (ptr == null) continue;
                ref var item = ref mref<StructComponentsItemUnknown>(ptr);
                item.Dispose();

            }
            if (this.list.isCreated == true) this.list.Dispose();

        }

        public void RemoveAll(int entityId) {

            var entId = ArrayUtils.AssumePositive(entityId);
            for (int i = 0; i < this.list.Length; ++i) {
                
                var ptr = (void*)this.list[i];
                if (ptr == null) continue;
                ref var item = ref mref<StructComponentsItemUnknown>(ptr);
                item.dataExists[entId] = false;

            }
            
        }

        public void Validate<T>(int entityId) where T : struct {

            var entId = ArrayUtils.AssumePositive(entityId);
            var id = WorldUtilities.GetAllComponentTypeId<T>();
            NativeArrayUtils.Resize(id, ref this.list);
            
            for (int i = 0; i < this.list.Length; ++i) {

                var ptr = (void*)this.list[i];
                if (ptr == null) continue;
                ref var item = ref mref<StructComponentsItem<T>>(ptr);
                item.Validate(entId);

            }

        }

        public void Validate<T>() where T : struct {

            var id = WorldUtilities.GetAllComponentTypeId<T>();
            NativeArrayUtils.Resize(id, ref this.list);

            if (this.list[id] == System.IntPtr.Zero) {

                this.list[id] = (System.IntPtr)pnew<StructComponentsItem<T>>();
                
            }
            
        }

        public bool Remove<T>(int entityId) where T : struct {

            var entId = ArrayUtils.AssumePositive(entityId);
            var id = WorldUtilities.GetAllComponentTypeId<T>();
            //this.Validate<T>(entId);
            var ptr = this.list[id];
            ref var item = ref mref<StructComponentsItem<T>>((void*)ptr);
            return item.Remove(entId);

        }

        public bool Has<T>(int entityId) where T : struct {

            var entId = ArrayUtils.AssumePositive(entityId);
            var id = WorldUtilities.GetAllComponentTypeId<T>();
            //this.Validate<T>(entId);
            var ptr = this.list[id];
            ref var item = ref mref<StructComponentsItem<T>>((void*)ptr);
            return item.Has(entId);

        }

        public void Set<T>(int entityId, T data) where T : struct {

            var entId = ArrayUtils.AssumePositive(entityId);
            var id = WorldUtilities.GetAllComponentTypeId<T>();
            //this.Validate<T>(entId);
            var ptr = this.list[id];
            ref var item = ref mref<StructComponentsItem<T>>((void*)ptr);
            item.Set(entId, data);

        }

        public ref T Get<T>(int entityId) where T : struct {
            
            var entId = ArrayUtils.AssumePositive(entityId);
            var id = WorldUtilities.GetAllComponentTypeId<T>();
            //this.Validate<T>(entId);
            var ptr = this.list[id];
            ref var item = ref mref<StructComponentsItem<T>>((void*)ptr);
            return ref item.Get(entId);

        }

        public ref readonly T Read<T>(int entityId) where T : struct {
            
            var entId = ArrayUtils.AssumePositive(entityId);
            var id = WorldUtilities.GetAllComponentTypeId<T>();
            //this.Validate<T>(entId);
            var ptr = this.list[id];
            ref var item = ref mref<StructComponentsItem<T>>((void*)ptr);
            return ref item.Read(entId);

        }

    }

}