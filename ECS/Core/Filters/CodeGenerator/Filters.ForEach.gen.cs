#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Filters;
    using Buffers;
    
    namespace Buffers {

        /*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0>  where T0:struct,IStructComponent {

    private DataBuffer<T0> buffer0;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0>  where T0:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1>  where T0:struct,IStructComponent where T1:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
[Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
public struct FilterBag<T0,T1>  where T0:struct,IStructComponent where T1:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer11.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();
this.buffer11.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer11.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer12.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();
this.buffer11.Dispose();
this.buffer12.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer11.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer12.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer13.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();
this.buffer11.Dispose();
this.buffer12.Dispose();
this.buffer13.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer11.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer12.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer13.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer14.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();
this.buffer11.Dispose();
this.buffer12.Dispose();
this.buffer13.Dispose();
this.buffer14.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;private DataBuffer<T15> buffer15;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);
this.buffer15 = new DataBuffer<T15>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }
public void SetT15(in T15 data) { this.buffer15.Set(this.index, in data); }
public ref T15 GetT15() { return ref this.buffer15.Get(this.index); }
public void RemoveT15() { this.buffer15.Remove(this.index); }
public ref readonly T15 ReadT15() { return ref this.buffer15.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;private DataBuffer<T15> buffer15;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);
this.buffer15 = new DataBuffer<T15>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer11.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer12.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer13.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer14.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer15.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();
this.buffer11.Dispose();
this.buffer12.Dispose();
this.buffer13.Dispose();
this.buffer14.Dispose();
this.buffer15.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }
public void SetT15(in T15 data) { this.buffer15.Set(this.index, in data); }
public ref T15 GetT15() { return ref this.buffer15.Get(this.index); }
public void RemoveT15() { this.buffer15.Remove(this.index); }
public ref readonly T15 ReadT15() { return ref this.buffer15.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;private DataBuffer<T15> buffer15;private DataBuffer<T16> buffer16;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);
this.buffer15 = new DataBuffer<T15>(world, arrEntities, min, max, max - min, allocator);
this.buffer16 = new DataBuffer<T16>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }
public void SetT15(in T15 data) { this.buffer15.Set(this.index, in data); }
public ref T15 GetT15() { return ref this.buffer15.Get(this.index); }
public void RemoveT15() { this.buffer15.Remove(this.index); }
public ref readonly T15 ReadT15() { return ref this.buffer15.Read(this.index); }
public void SetT16(in T16 data) { this.buffer16.Set(this.index, in data); }
public ref T16 GetT16() { return ref this.buffer16.Get(this.index); }
public void RemoveT16() { this.buffer16.Remove(this.index); }
public ref readonly T16 ReadT16() { return ref this.buffer16.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;private DataBuffer<T15> buffer15;private DataBuffer<T16> buffer16;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);
this.buffer15 = new DataBuffer<T15>(world, arrEntities, min, max, max - min, allocator);
this.buffer16 = new DataBuffer<T16>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer11.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer12.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer13.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer14.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer15.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer16.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();
this.buffer11.Dispose();
this.buffer12.Dispose();
this.buffer13.Dispose();
this.buffer14.Dispose();
this.buffer15.Dispose();
this.buffer16.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }
public void SetT15(in T15 data) { this.buffer15.Set(this.index, in data); }
public ref T15 GetT15() { return ref this.buffer15.Get(this.index); }
public void RemoveT15() { this.buffer15.Remove(this.index); }
public ref readonly T15 ReadT15() { return ref this.buffer15.Read(this.index); }
public void SetT16(in T16 data) { this.buffer16.Set(this.index, in data); }
public ref T16 GetT16() { return ref this.buffer16.Get(this.index); }
public void RemoveT16() { this.buffer16.Remove(this.index); }
public ref readonly T16 ReadT16() { return ref this.buffer16.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;private DataBuffer<T15> buffer15;private DataBuffer<T16> buffer16;private DataBuffer<T17> buffer17;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);
this.buffer15 = new DataBuffer<T15>(world, arrEntities, min, max, max - min, allocator);
this.buffer16 = new DataBuffer<T16>(world, arrEntities, min, max, max - min, allocator);
this.buffer17 = new DataBuffer<T17>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }
public void SetT15(in T15 data) { this.buffer15.Set(this.index, in data); }
public ref T15 GetT15() { return ref this.buffer15.Get(this.index); }
public void RemoveT15() { this.buffer15.Remove(this.index); }
public ref readonly T15 ReadT15() { return ref this.buffer15.Read(this.index); }
public void SetT16(in T16 data) { this.buffer16.Set(this.index, in data); }
public ref T16 GetT16() { return ref this.buffer16.Get(this.index); }
public void RemoveT16() { this.buffer16.Remove(this.index); }
public ref readonly T16 ReadT16() { return ref this.buffer16.Read(this.index); }
public void SetT17(in T17 data) { this.buffer17.Set(this.index, in data); }
public ref T17 GetT17() { return ref this.buffer17.Get(this.index); }
public void RemoveT17() { this.buffer17.Remove(this.index); }
public ref readonly T17 ReadT17() { return ref this.buffer17.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;private DataBuffer<T15> buffer15;private DataBuffer<T16> buffer16;private DataBuffer<T17> buffer17;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);
this.buffer15 = new DataBuffer<T15>(world, arrEntities, min, max, max - min, allocator);
this.buffer16 = new DataBuffer<T16>(world, arrEntities, min, max, max - min, allocator);
this.buffer17 = new DataBuffer<T17>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer11.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer12.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer13.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer14.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer15.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer16.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer17.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();
this.buffer11.Dispose();
this.buffer12.Dispose();
this.buffer13.Dispose();
this.buffer14.Dispose();
this.buffer15.Dispose();
this.buffer16.Dispose();
this.buffer17.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }
public void SetT15(in T15 data) { this.buffer15.Set(this.index, in data); }
public ref T15 GetT15() { return ref this.buffer15.Get(this.index); }
public void RemoveT15() { this.buffer15.Remove(this.index); }
public ref readonly T15 ReadT15() { return ref this.buffer15.Read(this.index); }
public void SetT16(in T16 data) { this.buffer16.Set(this.index, in data); }
public ref T16 GetT16() { return ref this.buffer16.Get(this.index); }
public void RemoveT16() { this.buffer16.Remove(this.index); }
public ref readonly T16 ReadT16() { return ref this.buffer16.Read(this.index); }
public void SetT17(in T17 data) { this.buffer17.Set(this.index, in data); }
public ref T17 GetT17() { return ref this.buffer17.Get(this.index); }
public void RemoveT17() { this.buffer17.Remove(this.index); }
public ref readonly T17 ReadT17() { return ref this.buffer17.Read(this.index); }

    #endregion

}

/*#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public readonly ref struct EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {

    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;private DataBuffer<T15> buffer15;private DataBuffer<T16> buffer16;private DataBuffer<T17> buffer17;private DataBuffer<T18> buffer18;

    #if INLINE_METHODS
    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    #endif
    public EntityBuffer(World world, ME.ECS.Collections.BufferArray<Entity> arr, int minIdx, int maxIdx) {
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);
this.buffer15 = new DataBuffer<T15>(world, arrEntities, min, max, max - min, allocator);
this.buffer16 = new DataBuffer<T16>(world, arrEntities, min, max, max - min, allocator);
this.buffer17 = new DataBuffer<T17>(world, arrEntities, min, max, max - min, allocator);
this.buffer18 = new DataBuffer<T18>(world, arrEntities, min, max, max - min, allocator);

        
    }
    
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }
public void SetT15(in T15 data) { this.buffer15.Set(this.index, in data); }
public ref T15 GetT15() { return ref this.buffer15.Get(this.index); }
public void RemoveT15() { this.buffer15.Remove(this.index); }
public ref readonly T15 ReadT15() { return ref this.buffer15.Read(this.index); }
public void SetT16(in T16 data) { this.buffer16.Set(this.index, in data); }
public ref T16 GetT16() { return ref this.buffer16.Get(this.index); }
public void RemoveT16() { this.buffer16.Remove(this.index); }
public ref readonly T16 ReadT16() { return ref this.buffer16.Read(this.index); }
public void SetT17(in T17 data) { this.buffer17.Set(this.index, in data); }
public ref T17 GetT17() { return ref this.buffer17.Get(this.index); }
public void RemoveT17() { this.buffer17.Remove(this.index); }
public ref readonly T17 ReadT17() { return ref this.buffer17.Read(this.index); }
public void SetT18(in T18 data) { this.buffer18.Set(this.index, in data); }
public ref T18 GetT18() { return ref this.buffer18.Get(this.index); }
public void RemoveT18() { this.buffer18.Remove(this.index); }
public ref readonly T18 ReadT18() { return ref this.buffer18.Read(this.index); }


}*/

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {

    public readonly int Length;
    private int index;
    private readonly int max;
    private Unity.Collections.NativeArray<bool> inFilter;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;private DataBuffer<T9> buffer9;private DataBuffer<T10> buffer10;private DataBuffer<T11> buffer11;private DataBuffer<T12> buffer12;private DataBuffer<T13> buffer13;private DataBuffer<T14> buffer14;private DataBuffer<T15> buffer15;private DataBuffer<T16> buffer16;private DataBuffer<T17> buffer17;private DataBuffer<T18> buffer18;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        filter.GetBounds(out var min, out var max);
        if (min > max) {

            min = 0;
            max = -1;

        }

        if (min < 0) min = 0;
        ++max;
        if (max >= arrEntities.Length) max = arrEntities.Length - 1;
        this.index = min - 1;
        this.Length = filter.Count;
        this.max = max;
        
        this.inFilter = new Unity.Collections.NativeArray<bool>(world.GetFilter(filter.id).dataContains.arr, allocator);
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, min, max, max - min, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, min, max, max - min, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, min, max, max - min, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, min, max, max - min, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, min, max, max - min, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, min, max, max - min, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, min, max, max - min, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, min, max, max - min, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, min, max, max - min, allocator);
this.buffer9 = new DataBuffer<T9>(world, arrEntities, min, max, max - min, allocator);
this.buffer10 = new DataBuffer<T10>(world, arrEntities, min, max, max - min, allocator);
this.buffer11 = new DataBuffer<T11>(world, arrEntities, min, max, max - min, allocator);
this.buffer12 = new DataBuffer<T12>(world, arrEntities, min, max, max - min, allocator);
this.buffer13 = new DataBuffer<T13>(world, arrEntities, min, max, max - min, allocator);
this.buffer14 = new DataBuffer<T14>(world, arrEntities, min, max, max - min, allocator);
this.buffer15 = new DataBuffer<T15>(world, arrEntities, min, max, max - min, allocator);
this.buffer16 = new DataBuffer<T16>(world, arrEntities, min, max, max - min, allocator);
this.buffer17 = new DataBuffer<T17>(world, arrEntities, min, max, max - min, allocator);
this.buffer18 = new DataBuffer<T18>(world, arrEntities, min, max, max - min, allocator);

        
    }

    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer9.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer10.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer11.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer12.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer13.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer14.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer15.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer16.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer17.Push(world, world.currentState.storage.cache, this.max, this.inFilter);
changedCount += this.buffer18.Push(world, world.currentState.storage.cache, this.max, this.inFilter);

        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();
this.buffer3.Dispose();
this.buffer4.Dispose();
this.buffer5.Dispose();
this.buffer6.Dispose();
this.buffer7.Dispose();
this.buffer8.Dispose();
this.buffer9.Dispose();
this.buffer10.Dispose();
this.buffer11.Dispose();
this.buffer12.Dispose();
this.buffer13.Dispose();
this.buffer14.Dispose();
this.buffer15.Dispose();
this.buffer16.Dispose();
this.buffer17.Dispose();
this.buffer18.Dispose();

        this.Dispose();

    }

    public bool MoveNext() {
        
        ++this.index;
        while (this.index <= this.max && this.inFilter[this.index] == false) {

            ++this.index;

        }

        return this.index <= this.max;

    }
    
    private void Dispose() {
        
        this.inFilter.Dispose();
        
    }

    #region API
    public void SetT0(in T0 data) { this.buffer0.Set(this.index, in data); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.index); }
public void RemoveT0() { this.buffer0.Remove(this.index); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.index); }
public void SetT1(in T1 data) { this.buffer1.Set(this.index, in data); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.index); }
public void RemoveT1() { this.buffer1.Remove(this.index); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.index); }
public void SetT2(in T2 data) { this.buffer2.Set(this.index, in data); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.index); }
public void RemoveT2() { this.buffer2.Remove(this.index); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.index); }
public void SetT3(in T3 data) { this.buffer3.Set(this.index, in data); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.index); }
public void RemoveT3() { this.buffer3.Remove(this.index); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.index); }
public void SetT4(in T4 data) { this.buffer4.Set(this.index, in data); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.index); }
public void RemoveT4() { this.buffer4.Remove(this.index); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.index); }
public void SetT5(in T5 data) { this.buffer5.Set(this.index, in data); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.index); }
public void RemoveT5() { this.buffer5.Remove(this.index); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.index); }
public void SetT6(in T6 data) { this.buffer6.Set(this.index, in data); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.index); }
public void RemoveT6() { this.buffer6.Remove(this.index); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.index); }
public void SetT7(in T7 data) { this.buffer7.Set(this.index, in data); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.index); }
public void RemoveT7() { this.buffer7.Remove(this.index); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.index); }
public void SetT8(in T8 data) { this.buffer8.Set(this.index, in data); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.index); }
public void RemoveT8() { this.buffer8.Remove(this.index); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.index); }
public void SetT9(in T9 data) { this.buffer9.Set(this.index, in data); }
public ref T9 GetT9() { return ref this.buffer9.Get(this.index); }
public void RemoveT9() { this.buffer9.Remove(this.index); }
public ref readonly T9 ReadT9() { return ref this.buffer9.Read(this.index); }
public void SetT10(in T10 data) { this.buffer10.Set(this.index, in data); }
public ref T10 GetT10() { return ref this.buffer10.Get(this.index); }
public void RemoveT10() { this.buffer10.Remove(this.index); }
public ref readonly T10 ReadT10() { return ref this.buffer10.Read(this.index); }
public void SetT11(in T11 data) { this.buffer11.Set(this.index, in data); }
public ref T11 GetT11() { return ref this.buffer11.Get(this.index); }
public void RemoveT11() { this.buffer11.Remove(this.index); }
public ref readonly T11 ReadT11() { return ref this.buffer11.Read(this.index); }
public void SetT12(in T12 data) { this.buffer12.Set(this.index, in data); }
public ref T12 GetT12() { return ref this.buffer12.Get(this.index); }
public void RemoveT12() { this.buffer12.Remove(this.index); }
public ref readonly T12 ReadT12() { return ref this.buffer12.Read(this.index); }
public void SetT13(in T13 data) { this.buffer13.Set(this.index, in data); }
public ref T13 GetT13() { return ref this.buffer13.Get(this.index); }
public void RemoveT13() { this.buffer13.Remove(this.index); }
public ref readonly T13 ReadT13() { return ref this.buffer13.Read(this.index); }
public void SetT14(in T14 data) { this.buffer14.Set(this.index, in data); }
public ref T14 GetT14() { return ref this.buffer14.Get(this.index); }
public void RemoveT14() { this.buffer14.Remove(this.index); }
public ref readonly T14 ReadT14() { return ref this.buffer14.Read(this.index); }
public void SetT15(in T15 data) { this.buffer15.Set(this.index, in data); }
public ref T15 GetT15() { return ref this.buffer15.Get(this.index); }
public void RemoveT15() { this.buffer15.Remove(this.index); }
public ref readonly T15 ReadT15() { return ref this.buffer15.Read(this.index); }
public void SetT16(in T16 data) { this.buffer16.Set(this.index, in data); }
public ref T16 GetT16() { return ref this.buffer16.Get(this.index); }
public void RemoveT16() { this.buffer16.Remove(this.index); }
public ref readonly T16 ReadT16() { return ref this.buffer16.Read(this.index); }
public void SetT17(in T17 data) { this.buffer17.Set(this.index, in data); }
public ref T17 GetT17() { return ref this.buffer17.Get(this.index); }
public void RemoveT17() { this.buffer17.Remove(this.index); }
public ref readonly T17 ReadT17() { return ref this.buffer17.Read(this.index); }
public void SetT18(in T18 data) { this.buffer18.Set(this.index, in data); }
public ref T18 GetT18() { return ref this.buffer18.Get(this.index); }
public void RemoveT18() { this.buffer18.Remove(this.index); }
public ref readonly T18 ReadT18() { return ref this.buffer18.Read(this.index); }

    #endregion

}



    }
    
    /*
    public static class StateFiltersForEachExtensions {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ForEach(this Filter filter, R onEach) {
            filter.GetBounds(out var min, out var max);
            var entities = filter.world.currentState.storage.cache;
            for (int i = min; i <= max; ++i) { var e = entities.arr[i]; onEach.Invoke(in e); }
        }
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ForEach<TCustom0>(this Filter filter, in TCustom0 custom0, RCP0<TCustom0> onEach) {
            filter.GetBounds(out var min, out var max);
            var entities = filter.world.currentState.storage.cache;
            for (int i = min; i <= max; ++i) { var e = entities.arr[i]; onEach.Invoke(in custom0, in e); }
        }
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ForEach<TCustom0, TCustom1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP0<TCustom0, TCustom1> onEach) {
            filter.GetBounds(out var min, out var max);
            var entities = filter.world.currentState.storage.cache;
            for (int i = min; i <= max; ++i) { var e = entities.arr[i]; onEach.Invoke(in custom0, in custom1, in e); }
        }
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ForEach<TCustom0, TCustom1, TCustom2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP0<TCustom0, TCustom1, TCustom2> onEach) {
            filter.GetBounds(out var min, out var max);
            var entities = filter.world.currentState.storage.cache;
            for (int i = min; i <= max; ++i) { var e = entities.arr[i]; onEach.Invoke(in custom0, in custom1, in custom2, in e); }
        }
        */
        /*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0>(this Filter filter, R<T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0>(this Filter filter, in TCustom0 custom0, RCP1<TCustom0, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP1<TCustom0, TCustom1, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP1<TCustom0, TCustom1, TCustom2, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0> onEach)  where T0:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1>(this Filter filter, R<T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1>(this Filter filter, in TCustom0 custom0, RCP2<TCustom0, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP2<TCustom0, TCustom1, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP2<TCustom0, TCustom1, TCustom2, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2>(this Filter filter, R<T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2>(this Filter filter, in TCustom0 custom0, RCP3<TCustom0, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP3<TCustom0, TCustom1, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP3<TCustom0, TCustom1, TCustom2, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3>(this Filter filter, R<T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, RCP4<TCustom0, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP4<TCustom0, TCustom1, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP4<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4>(this Filter filter, R<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, RCP5<TCustom0, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP5<TCustom0, TCustom1, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP5<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5>(this Filter filter, R<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, RCP6<TCustom0, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP6<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP6<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, RCP7<TCustom0, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP7<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP7<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, RCP8<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP8<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP8<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, RCP9<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP9<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP9<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, RCP10<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP10<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP10<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP10<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, RCP11<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP11<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP11<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP11<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, RCP12<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP12<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP12<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP12<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, RCP13<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP13<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP13<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP13<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, RCP14<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP14<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP14<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP14<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, RCP15<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP15<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP15<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP15<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, RCP16<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP16<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP16<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP16<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, RCP17<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP17<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP17<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP17<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, RCP18<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP18<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP18<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP18<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id)); }
}
*/
/*
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, RCP19<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP19<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP19<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP19<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18> onEach)  where T0:struct,IStructComponent where T1:struct,IStructComponent where T2:struct,IStructComponent where T3:struct,IStructComponent where T4:struct,IStructComponent where T5:struct,IStructComponent where T6:struct,IStructComponent where T7:struct,IStructComponent where T8:struct,IStructComponent where T9:struct,IStructComponent where T10:struct,IStructComponent where T11:struct,IStructComponent where T12:struct,IStructComponent where T13:struct,IStructComponent where T14:struct,IStructComponent where T15:struct,IStructComponent where T16:struct,IStructComponent where T17:struct,IStructComponent where T18:struct,IStructComponent {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in custom9, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id), ref buffer.GetT9(id), ref buffer.GetT10(id), ref buffer.GetT11(id), ref buffer.GetT12(id), ref buffer.GetT13(id), ref buffer.GetT14(id), ref buffer.GetT15(id), ref buffer.GetT16(id), ref buffer.GetT17(id), ref buffer.GetT18(id)); }
}
*/

        /*
    }
    */

}