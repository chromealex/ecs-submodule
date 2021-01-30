namespace ME.ECS {

    using Filters;
    using Buffers;
    
    namespace Buffers {

        #if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0>  where T0:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1>  where T0:struct,IStructComponent where T1:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly struct EntityBuffer<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;private readonly DataBuffer<T11> buffer11;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);
this.buffer11 = new DataBuffer<T11>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T11 GetT11(int entityId) { return ref this.buffer11.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;private readonly DataBuffer<T11> buffer11;private readonly DataBuffer<T12> buffer12;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);
this.buffer11 = new DataBuffer<T11>(world, arr, minIdx, maxIdx);
this.buffer12 = new DataBuffer<T12>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T11 GetT11(int entityId) { return ref this.buffer11.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T12 GetT12(int entityId) { return ref this.buffer12.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;private readonly DataBuffer<T11> buffer11;private readonly DataBuffer<T12> buffer12;private readonly DataBuffer<T13> buffer13;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);
this.buffer11 = new DataBuffer<T11>(world, arr, minIdx, maxIdx);
this.buffer12 = new DataBuffer<T12>(world, arr, minIdx, maxIdx);
this.buffer13 = new DataBuffer<T13>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T11 GetT11(int entityId) { return ref this.buffer11.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T12 GetT12(int entityId) { return ref this.buffer12.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T13 GetT13(int entityId) { return ref this.buffer13.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;private readonly DataBuffer<T11> buffer11;private readonly DataBuffer<T12> buffer12;private readonly DataBuffer<T13> buffer13;private readonly DataBuffer<T14> buffer14;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);
this.buffer11 = new DataBuffer<T11>(world, arr, minIdx, maxIdx);
this.buffer12 = new DataBuffer<T12>(world, arr, minIdx, maxIdx);
this.buffer13 = new DataBuffer<T13>(world, arr, minIdx, maxIdx);
this.buffer14 = new DataBuffer<T14>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T11 GetT11(int entityId) { return ref this.buffer11.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T12 GetT12(int entityId) { return ref this.buffer12.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T13 GetT13(int entityId) { return ref this.buffer13.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T14 GetT14(int entityId) { return ref this.buffer14.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;private readonly DataBuffer<T11> buffer11;private readonly DataBuffer<T12> buffer12;private readonly DataBuffer<T13> buffer13;private readonly DataBuffer<T14> buffer14;private readonly DataBuffer<T15> buffer15;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);
this.buffer11 = new DataBuffer<T11>(world, arr, minIdx, maxIdx);
this.buffer12 = new DataBuffer<T12>(world, arr, minIdx, maxIdx);
this.buffer13 = new DataBuffer<T13>(world, arr, minIdx, maxIdx);
this.buffer14 = new DataBuffer<T14>(world, arr, minIdx, maxIdx);
this.buffer15 = new DataBuffer<T15>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T11 GetT11(int entityId) { return ref this.buffer11.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T12 GetT12(int entityId) { return ref this.buffer12.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T13 GetT13(int entityId) { return ref this.buffer13.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T14 GetT14(int entityId) { return ref this.buffer14.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T15 GetT15(int entityId) { return ref this.buffer15.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;private readonly DataBuffer<T11> buffer11;private readonly DataBuffer<T12> buffer12;private readonly DataBuffer<T13> buffer13;private readonly DataBuffer<T14> buffer14;private readonly DataBuffer<T15> buffer15;private readonly DataBuffer<T16> buffer16;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);
this.buffer11 = new DataBuffer<T11>(world, arr, minIdx, maxIdx);
this.buffer12 = new DataBuffer<T12>(world, arr, minIdx, maxIdx);
this.buffer13 = new DataBuffer<T13>(world, arr, minIdx, maxIdx);
this.buffer14 = new DataBuffer<T14>(world, arr, minIdx, maxIdx);
this.buffer15 = new DataBuffer<T15>(world, arr, minIdx, maxIdx);
this.buffer16 = new DataBuffer<T16>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T11 GetT11(int entityId) { return ref this.buffer11.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T12 GetT12(int entityId) { return ref this.buffer12.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T13 GetT13(int entityId) { return ref this.buffer13.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T14 GetT14(int entityId) { return ref this.buffer14.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T15 GetT15(int entityId) { return ref this.buffer15.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T16 GetT16(int entityId) { return ref this.buffer16.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;private readonly DataBuffer<T11> buffer11;private readonly DataBuffer<T12> buffer12;private readonly DataBuffer<T13> buffer13;private readonly DataBuffer<T14> buffer14;private readonly DataBuffer<T15> buffer15;private readonly DataBuffer<T16> buffer16;private readonly DataBuffer<T17> buffer17;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);
this.buffer11 = new DataBuffer<T11>(world, arr, minIdx, maxIdx);
this.buffer12 = new DataBuffer<T12>(world, arr, minIdx, maxIdx);
this.buffer13 = new DataBuffer<T13>(world, arr, minIdx, maxIdx);
this.buffer14 = new DataBuffer<T14>(world, arr, minIdx, maxIdx);
this.buffer15 = new DataBuffer<T15>(world, arr, minIdx, maxIdx);
this.buffer16 = new DataBuffer<T16>(world, arr, minIdx, maxIdx);
this.buffer17 = new DataBuffer<T17>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T11 GetT11(int entityId) { return ref this.buffer11.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T12 GetT12(int entityId) { return ref this.buffer12.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T13 GetT13(int entityId) { return ref this.buffer13.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T14 GetT14(int entityId) { return ref this.buffer14.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T15 GetT15(int entityId) { return ref this.buffer15.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T16 GetT16(int entityId) { return ref this.buffer16.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T17 GetT17(int entityId) { return ref this.buffer17.Get(entityId); }


}
#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {

    private readonly DataBuffer<T0> buffer0;private readonly DataBuffer<T1> buffer1;private readonly DataBuffer<T2> buffer2;private readonly DataBuffer<T3> buffer3;private readonly DataBuffer<T4> buffer4;private readonly DataBuffer<T5> buffer5;private readonly DataBuffer<T6> buffer6;private readonly DataBuffer<T7> buffer7;private readonly DataBuffer<T8> buffer8;private readonly DataBuffer<T9> buffer9;private readonly DataBuffer<T10> buffer10;private readonly DataBuffer<T11> buffer11;private readonly DataBuffer<T12> buffer12;private readonly DataBuffer<T13> buffer13;private readonly DataBuffer<T14> buffer14;private readonly DataBuffer<T15> buffer15;private readonly DataBuffer<T16> buffer16;private readonly DataBuffer<T17> buffer17;private readonly DataBuffer<T18> buffer18;

    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arr, minIdx, maxIdx);
this.buffer1 = new DataBuffer<T1>(world, arr, minIdx, maxIdx);
this.buffer2 = new DataBuffer<T2>(world, arr, minIdx, maxIdx);
this.buffer3 = new DataBuffer<T3>(world, arr, minIdx, maxIdx);
this.buffer4 = new DataBuffer<T4>(world, arr, minIdx, maxIdx);
this.buffer5 = new DataBuffer<T5>(world, arr, minIdx, maxIdx);
this.buffer6 = new DataBuffer<T6>(world, arr, minIdx, maxIdx);
this.buffer7 = new DataBuffer<T7>(world, arr, minIdx, maxIdx);
this.buffer8 = new DataBuffer<T8>(world, arr, minIdx, maxIdx);
this.buffer9 = new DataBuffer<T9>(world, arr, minIdx, maxIdx);
this.buffer10 = new DataBuffer<T10>(world, arr, minIdx, maxIdx);
this.buffer11 = new DataBuffer<T11>(world, arr, minIdx, maxIdx);
this.buffer12 = new DataBuffer<T12>(world, arr, minIdx, maxIdx);
this.buffer13 = new DataBuffer<T13>(world, arr, minIdx, maxIdx);
this.buffer14 = new DataBuffer<T14>(world, arr, minIdx, maxIdx);
this.buffer15 = new DataBuffer<T15>(world, arr, minIdx, maxIdx);
this.buffer16 = new DataBuffer<T16>(world, arr, minIdx, maxIdx);
this.buffer17 = new DataBuffer<T17>(world, arr, minIdx, maxIdx);
this.buffer18 = new DataBuffer<T18>(world, arr, minIdx, maxIdx);

        
    }
    
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T0 GetT0(int entityId) { return ref this.buffer0.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T1 GetT1(int entityId) { return ref this.buffer1.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T2 GetT2(int entityId) { return ref this.buffer2.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T3 GetT3(int entityId) { return ref this.buffer3.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T4 GetT4(int entityId) { return ref this.buffer4.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T5 GetT5(int entityId) { return ref this.buffer5.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T6 GetT6(int entityId) { return ref this.buffer6.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T7 GetT7(int entityId) { return ref this.buffer7.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T8 GetT8(int entityId) { return ref this.buffer8.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T9 GetT9(int entityId) { return ref this.buffer9.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T10 GetT10(int entityId) { return ref this.buffer10.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T11 GetT11(int entityId) { return ref this.buffer11.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T12 GetT12(int entityId) { return ref this.buffer12.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T13 GetT13(int entityId) { return ref this.buffer13.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T14 GetT14(int entityId) { return ref this.buffer14.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T15 GetT15(int entityId) { return ref this.buffer15.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T16 GetT16(int entityId) { return ref this.buffer16.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T17 GetT17(int entityId) { return ref this.buffer17.Get(entityId); }
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)] public ref T18 GetT18(int entityId) { return ref this.buffer18.Get(entityId); }


}


    }
    
    public static class StateFiltersForEachExtensions {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ForEach(this Filter filter, R onEach) {
            filter.GetBounds(out var min, out var max);
            var entities = filter.world.currentState.storage.cache;
            for (int i = min; i <= max; ++i) { var e = entities.arr[i]; onEach.Invoke(in e); }
        }
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ForEach<TCustom0>(this Filter filter, in TCustom0 custom0, RCP0<TCustom0> onEach) {
            filter.GetBounds(out var min, out var max);
            var entities = filter.world.currentState.storage.cache;
            for (int i = min; i <= max; ++i) { var e = entities.arr[i]; onEach.Invoke(in custom0, in e); }
        }
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ForEach<TCustom0, TCustom1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP0<TCustom0, TCustom1> onEach) {
            filter.GetBounds(out var min, out var max);
            var entities = filter.world.currentState.storage.cache;
            for (int i = min; i <= max; ++i) { var e = entities.arr[i]; onEach.Invoke(in custom0, in custom1, in e); }
        }
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ForEach<TCustom0, TCustom1, TCustom2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP0<TCustom0, TCustom1, TCustom2> onEach) {
            filter.GetBounds(out var min, out var max);
            var entities = filter.world.currentState.storage.cache;
            for (int i = min; i <= max; ++i) { var e = entities.arr[i]; onEach.Invoke(in custom0, in custom1, in custom2, in e); }
        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0>(this Filter filter, R<T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0>(this Filter filter, in TCustom0 custom0, RCP1<TCustom0, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP1<TCustom0, TCustom1, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP1<TCustom0, TCustom1, TCustom2, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1>(this Filter filter, R<T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1>(this Filter filter, in TCustom0 custom0, RCP2<TCustom0, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP2<TCustom0, TCustom1, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP2<TCustom0, TCustom1, TCustom2, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2>(this Filter filter, R<T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2>(this Filter filter, in TCustom0 custom0, RCP3<TCustom0, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP3<TCustom0, TCustom1, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP3<TCustom0, TCustom1, TCustom2, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3>(this Filter filter, R<T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, RCP4<TCustom0, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP4<TCustom0, TCustom1, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP4<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4>(this Filter filter, R<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, RCP5<TCustom0, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP5<TCustom0, TCustom1, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP5<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5>(this Filter filter, R<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, RCP6<TCustom0, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP6<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP6<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}

[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, RCP7<TCustom0, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP7<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP7<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, RCP8<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP8<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP8<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, RCP9<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP9<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP9<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, RCP10<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP10<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP10<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, RCP11<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP11<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP11<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, RCP12<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP12<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP12<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, RCP13<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP13<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP13<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, RCP14<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP14<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP14<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, RCP15<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP15<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP15<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, RCP16<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP16<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP16<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, RCP17<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP17<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP17<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, RCP18<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP18<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP18<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, RCP19<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP19<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP19<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}

        
    }

}