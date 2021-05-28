namespace ME.ECSBurst {

    using System.Runtime.CompilerServices;
    using Unity.Collections.LowLevel.Unsafe;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static unsafe class MemUtilsCuts {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static T mcall<T>(void* methodPtr) {
            
            return System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<T>((System.IntPtr)methodPtr);

        }
    
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref T mref<T>(void* ptr) where T : struct => ref UnsafeUtility.AsRef<T>(ptr);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void free(ref void* ptr, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) {
            
            UnsafeUtility.Free(ptr, allocator);
            ptr = null;
            
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void* pnew<T>(T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            return MemUtils.CreateFromStruct(source, allocator);

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static T* tnew<T>(T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            return MemUtils.Create(source, allocator);

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static unsafe class MemUtils {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static T* Create<T>(T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : unmanaged {

            var ptr = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.CopyStructureToPtr(ref source, ptr);
            return (T*)ptr;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void* CreateFromStruct<T>(T source, Unity.Collections.Allocator allocator = Unity.Collections.Allocator.Persistent) where T : struct {

            var ptr = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator);
            UnsafeUtility.CopyStructureToPtr(ref source, ptr);
            return ptr;

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref T Ref<T>(void* ptr) where T : struct => ref UnsafeUtility.AsRef<T>(ptr);

    }
    
}