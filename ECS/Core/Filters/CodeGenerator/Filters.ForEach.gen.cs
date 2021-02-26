#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Filters;
    using Buffers;
    
    namespace Buffers {

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

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
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
    }
    */

}