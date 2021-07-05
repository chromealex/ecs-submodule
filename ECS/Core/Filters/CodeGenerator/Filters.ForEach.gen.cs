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
public struct FilterBag<T0>  where T0:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);

        
        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();


        this.Dispose();

    }

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }

    #endregion

}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);

        
        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();


        this.Dispose();

    }

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }
public void SetT1(int id, in T1 data) { this.buffer1.Set(id, in data); }
public void SetT1(in T1 data) { this.buffer1.Set(this.filterEntities[this.index], in data); }
public ref T1 GetT1(int id) { return ref this.buffer1.Get(id); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.filterEntities[this.index]); }
public void RemoveT1(int id) { this.buffer1.Remove(id); }
public void RemoveT1() { this.buffer1.Remove(this.filterEntities[this.index]); }
public ref readonly T1 ReadT1(int id) { return ref this.buffer1.Read(id); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.filterEntities[this.index]); }
public bool HasT1(int id) { return this.buffer1.Has(id); }
public bool HasT1() { return this.buffer1.Has(this.filterEntities[this.index]); }

    #endregion

}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max, this.filterEntities);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max, this.filterEntities);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max, this.filterEntities);

        
        if (changedCount > 0) world.UpdateAllFilters();
        
        this.Dispose();
        
    }

    public void Revert() {
        
        this.buffer0.Dispose();
this.buffer1.Dispose();
this.buffer2.Dispose();


        this.Dispose();

    }

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }
public void SetT1(int id, in T1 data) { this.buffer1.Set(id, in data); }
public void SetT1(in T1 data) { this.buffer1.Set(this.filterEntities[this.index], in data); }
public ref T1 GetT1(int id) { return ref this.buffer1.Get(id); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.filterEntities[this.index]); }
public void RemoveT1(int id) { this.buffer1.Remove(id); }
public void RemoveT1() { this.buffer1.Remove(this.filterEntities[this.index]); }
public ref readonly T1 ReadT1(int id) { return ref this.buffer1.Read(id); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.filterEntities[this.index]); }
public bool HasT1(int id) { return this.buffer1.Has(id); }
public bool HasT1() { return this.buffer1.Has(this.filterEntities[this.index]); }
public void SetT2(int id, in T2 data) { this.buffer2.Set(id, in data); }
public void SetT2(in T2 data) { this.buffer2.Set(this.filterEntities[this.index], in data); }
public ref T2 GetT2(int id) { return ref this.buffer2.Get(id); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.filterEntities[this.index]); }
public void RemoveT2(int id) { this.buffer2.Remove(id); }
public void RemoveT2() { this.buffer2.Remove(this.filterEntities[this.index]); }
public ref readonly T2 ReadT2(int id) { return ref this.buffer2.Read(id); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.filterEntities[this.index]); }
public bool HasT2(int id) { return this.buffer2.Has(id); }
public bool HasT2() { return this.buffer2.Has(this.filterEntities[this.index]); }

    #endregion

}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);

        
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

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }
public void SetT1(int id, in T1 data) { this.buffer1.Set(id, in data); }
public void SetT1(in T1 data) { this.buffer1.Set(this.filterEntities[this.index], in data); }
public ref T1 GetT1(int id) { return ref this.buffer1.Get(id); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.filterEntities[this.index]); }
public void RemoveT1(int id) { this.buffer1.Remove(id); }
public void RemoveT1() { this.buffer1.Remove(this.filterEntities[this.index]); }
public ref readonly T1 ReadT1(int id) { return ref this.buffer1.Read(id); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.filterEntities[this.index]); }
public bool HasT1(int id) { return this.buffer1.Has(id); }
public bool HasT1() { return this.buffer1.Has(this.filterEntities[this.index]); }
public void SetT2(int id, in T2 data) { this.buffer2.Set(id, in data); }
public void SetT2(in T2 data) { this.buffer2.Set(this.filterEntities[this.index], in data); }
public ref T2 GetT2(int id) { return ref this.buffer2.Get(id); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.filterEntities[this.index]); }
public void RemoveT2(int id) { this.buffer2.Remove(id); }
public void RemoveT2() { this.buffer2.Remove(this.filterEntities[this.index]); }
public ref readonly T2 ReadT2(int id) { return ref this.buffer2.Read(id); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.filterEntities[this.index]); }
public bool HasT2(int id) { return this.buffer2.Has(id); }
public bool HasT2() { return this.buffer2.Has(this.filterEntities[this.index]); }
public void SetT3(int id, in T3 data) { this.buffer3.Set(id, in data); }
public void SetT3(in T3 data) { this.buffer3.Set(this.filterEntities[this.index], in data); }
public ref T3 GetT3(int id) { return ref this.buffer3.Get(id); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.filterEntities[this.index]); }
public void RemoveT3(int id) { this.buffer3.Remove(id); }
public void RemoveT3() { this.buffer3.Remove(this.filterEntities[this.index]); }
public ref readonly T3 ReadT3(int id) { return ref this.buffer3.Read(id); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.filterEntities[this.index]); }
public bool HasT3(int id) { return this.buffer3.Has(id); }
public bool HasT3() { return this.buffer3.Has(this.filterEntities[this.index]); }

    #endregion

}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);

        
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

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }
public void SetT1(int id, in T1 data) { this.buffer1.Set(id, in data); }
public void SetT1(in T1 data) { this.buffer1.Set(this.filterEntities[this.index], in data); }
public ref T1 GetT1(int id) { return ref this.buffer1.Get(id); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.filterEntities[this.index]); }
public void RemoveT1(int id) { this.buffer1.Remove(id); }
public void RemoveT1() { this.buffer1.Remove(this.filterEntities[this.index]); }
public ref readonly T1 ReadT1(int id) { return ref this.buffer1.Read(id); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.filterEntities[this.index]); }
public bool HasT1(int id) { return this.buffer1.Has(id); }
public bool HasT1() { return this.buffer1.Has(this.filterEntities[this.index]); }
public void SetT2(int id, in T2 data) { this.buffer2.Set(id, in data); }
public void SetT2(in T2 data) { this.buffer2.Set(this.filterEntities[this.index], in data); }
public ref T2 GetT2(int id) { return ref this.buffer2.Get(id); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.filterEntities[this.index]); }
public void RemoveT2(int id) { this.buffer2.Remove(id); }
public void RemoveT2() { this.buffer2.Remove(this.filterEntities[this.index]); }
public ref readonly T2 ReadT2(int id) { return ref this.buffer2.Read(id); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.filterEntities[this.index]); }
public bool HasT2(int id) { return this.buffer2.Has(id); }
public bool HasT2() { return this.buffer2.Has(this.filterEntities[this.index]); }
public void SetT3(int id, in T3 data) { this.buffer3.Set(id, in data); }
public void SetT3(in T3 data) { this.buffer3.Set(this.filterEntities[this.index], in data); }
public ref T3 GetT3(int id) { return ref this.buffer3.Get(id); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.filterEntities[this.index]); }
public void RemoveT3(int id) { this.buffer3.Remove(id); }
public void RemoveT3() { this.buffer3.Remove(this.filterEntities[this.index]); }
public ref readonly T3 ReadT3(int id) { return ref this.buffer3.Read(id); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.filterEntities[this.index]); }
public bool HasT3(int id) { return this.buffer3.Has(id); }
public bool HasT3() { return this.buffer3.Has(this.filterEntities[this.index]); }
public void SetT4(int id, in T4 data) { this.buffer4.Set(id, in data); }
public void SetT4(in T4 data) { this.buffer4.Set(this.filterEntities[this.index], in data); }
public ref T4 GetT4(int id) { return ref this.buffer4.Get(id); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.filterEntities[this.index]); }
public void RemoveT4(int id) { this.buffer4.Remove(id); }
public void RemoveT4() { this.buffer4.Remove(this.filterEntities[this.index]); }
public ref readonly T4 ReadT4(int id) { return ref this.buffer4.Read(id); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.filterEntities[this.index]); }
public bool HasT4(int id) { return this.buffer4.Has(id); }
public bool HasT4() { return this.buffer4.Has(this.filterEntities[this.index]); }

    #endregion

}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);

        
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

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }
public void SetT1(int id, in T1 data) { this.buffer1.Set(id, in data); }
public void SetT1(in T1 data) { this.buffer1.Set(this.filterEntities[this.index], in data); }
public ref T1 GetT1(int id) { return ref this.buffer1.Get(id); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.filterEntities[this.index]); }
public void RemoveT1(int id) { this.buffer1.Remove(id); }
public void RemoveT1() { this.buffer1.Remove(this.filterEntities[this.index]); }
public ref readonly T1 ReadT1(int id) { return ref this.buffer1.Read(id); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.filterEntities[this.index]); }
public bool HasT1(int id) { return this.buffer1.Has(id); }
public bool HasT1() { return this.buffer1.Has(this.filterEntities[this.index]); }
public void SetT2(int id, in T2 data) { this.buffer2.Set(id, in data); }
public void SetT2(in T2 data) { this.buffer2.Set(this.filterEntities[this.index], in data); }
public ref T2 GetT2(int id) { return ref this.buffer2.Get(id); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.filterEntities[this.index]); }
public void RemoveT2(int id) { this.buffer2.Remove(id); }
public void RemoveT2() { this.buffer2.Remove(this.filterEntities[this.index]); }
public ref readonly T2 ReadT2(int id) { return ref this.buffer2.Read(id); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.filterEntities[this.index]); }
public bool HasT2(int id) { return this.buffer2.Has(id); }
public bool HasT2() { return this.buffer2.Has(this.filterEntities[this.index]); }
public void SetT3(int id, in T3 data) { this.buffer3.Set(id, in data); }
public void SetT3(in T3 data) { this.buffer3.Set(this.filterEntities[this.index], in data); }
public ref T3 GetT3(int id) { return ref this.buffer3.Get(id); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.filterEntities[this.index]); }
public void RemoveT3(int id) { this.buffer3.Remove(id); }
public void RemoveT3() { this.buffer3.Remove(this.filterEntities[this.index]); }
public ref readonly T3 ReadT3(int id) { return ref this.buffer3.Read(id); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.filterEntities[this.index]); }
public bool HasT3(int id) { return this.buffer3.Has(id); }
public bool HasT3() { return this.buffer3.Has(this.filterEntities[this.index]); }
public void SetT4(int id, in T4 data) { this.buffer4.Set(id, in data); }
public void SetT4(in T4 data) { this.buffer4.Set(this.filterEntities[this.index], in data); }
public ref T4 GetT4(int id) { return ref this.buffer4.Get(id); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.filterEntities[this.index]); }
public void RemoveT4(int id) { this.buffer4.Remove(id); }
public void RemoveT4() { this.buffer4.Remove(this.filterEntities[this.index]); }
public ref readonly T4 ReadT4(int id) { return ref this.buffer4.Read(id); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.filterEntities[this.index]); }
public bool HasT4(int id) { return this.buffer4.Has(id); }
public bool HasT4() { return this.buffer4.Has(this.filterEntities[this.index]); }
public void SetT5(int id, in T5 data) { this.buffer5.Set(id, in data); }
public void SetT5(in T5 data) { this.buffer5.Set(this.filterEntities[this.index], in data); }
public ref T5 GetT5(int id) { return ref this.buffer5.Get(id); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.filterEntities[this.index]); }
public void RemoveT5(int id) { this.buffer5.Remove(id); }
public void RemoveT5() { this.buffer5.Remove(this.filterEntities[this.index]); }
public ref readonly T5 ReadT5(int id) { return ref this.buffer5.Read(id); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.filterEntities[this.index]); }
public bool HasT5(int id) { return this.buffer5.Has(id); }
public bool HasT5() { return this.buffer5.Has(this.filterEntities[this.index]); }

    #endregion

}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);

        
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

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }
public void SetT1(int id, in T1 data) { this.buffer1.Set(id, in data); }
public void SetT1(in T1 data) { this.buffer1.Set(this.filterEntities[this.index], in data); }
public ref T1 GetT1(int id) { return ref this.buffer1.Get(id); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.filterEntities[this.index]); }
public void RemoveT1(int id) { this.buffer1.Remove(id); }
public void RemoveT1() { this.buffer1.Remove(this.filterEntities[this.index]); }
public ref readonly T1 ReadT1(int id) { return ref this.buffer1.Read(id); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.filterEntities[this.index]); }
public bool HasT1(int id) { return this.buffer1.Has(id); }
public bool HasT1() { return this.buffer1.Has(this.filterEntities[this.index]); }
public void SetT2(int id, in T2 data) { this.buffer2.Set(id, in data); }
public void SetT2(in T2 data) { this.buffer2.Set(this.filterEntities[this.index], in data); }
public ref T2 GetT2(int id) { return ref this.buffer2.Get(id); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.filterEntities[this.index]); }
public void RemoveT2(int id) { this.buffer2.Remove(id); }
public void RemoveT2() { this.buffer2.Remove(this.filterEntities[this.index]); }
public ref readonly T2 ReadT2(int id) { return ref this.buffer2.Read(id); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.filterEntities[this.index]); }
public bool HasT2(int id) { return this.buffer2.Has(id); }
public bool HasT2() { return this.buffer2.Has(this.filterEntities[this.index]); }
public void SetT3(int id, in T3 data) { this.buffer3.Set(id, in data); }
public void SetT3(in T3 data) { this.buffer3.Set(this.filterEntities[this.index], in data); }
public ref T3 GetT3(int id) { return ref this.buffer3.Get(id); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.filterEntities[this.index]); }
public void RemoveT3(int id) { this.buffer3.Remove(id); }
public void RemoveT3() { this.buffer3.Remove(this.filterEntities[this.index]); }
public ref readonly T3 ReadT3(int id) { return ref this.buffer3.Read(id); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.filterEntities[this.index]); }
public bool HasT3(int id) { return this.buffer3.Has(id); }
public bool HasT3() { return this.buffer3.Has(this.filterEntities[this.index]); }
public void SetT4(int id, in T4 data) { this.buffer4.Set(id, in data); }
public void SetT4(in T4 data) { this.buffer4.Set(this.filterEntities[this.index], in data); }
public ref T4 GetT4(int id) { return ref this.buffer4.Get(id); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.filterEntities[this.index]); }
public void RemoveT4(int id) { this.buffer4.Remove(id); }
public void RemoveT4() { this.buffer4.Remove(this.filterEntities[this.index]); }
public ref readonly T4 ReadT4(int id) { return ref this.buffer4.Read(id); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.filterEntities[this.index]); }
public bool HasT4(int id) { return this.buffer4.Has(id); }
public bool HasT4() { return this.buffer4.Has(this.filterEntities[this.index]); }
public void SetT5(int id, in T5 data) { this.buffer5.Set(id, in data); }
public void SetT5(in T5 data) { this.buffer5.Set(this.filterEntities[this.index], in data); }
public ref T5 GetT5(int id) { return ref this.buffer5.Get(id); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.filterEntities[this.index]); }
public void RemoveT5(int id) { this.buffer5.Remove(id); }
public void RemoveT5() { this.buffer5.Remove(this.filterEntities[this.index]); }
public ref readonly T5 ReadT5(int id) { return ref this.buffer5.Read(id); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.filterEntities[this.index]); }
public bool HasT5(int id) { return this.buffer5.Has(id); }
public bool HasT5() { return this.buffer5.Has(this.filterEntities[this.index]); }
public void SetT6(int id, in T6 data) { this.buffer6.Set(id, in data); }
public void SetT6(in T6 data) { this.buffer6.Set(this.filterEntities[this.index], in data); }
public ref T6 GetT6(int id) { return ref this.buffer6.Get(id); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.filterEntities[this.index]); }
public void RemoveT6(int id) { this.buffer6.Remove(id); }
public void RemoveT6() { this.buffer6.Remove(this.filterEntities[this.index]); }
public ref readonly T6 ReadT6(int id) { return ref this.buffer6.Read(id); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.filterEntities[this.index]); }
public bool HasT6(int id) { return this.buffer6.Has(id); }
public bool HasT6() { return this.buffer6.Has(this.filterEntities[this.index]); }

    #endregion

}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);

        
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

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }
public void SetT1(int id, in T1 data) { this.buffer1.Set(id, in data); }
public void SetT1(in T1 data) { this.buffer1.Set(this.filterEntities[this.index], in data); }
public ref T1 GetT1(int id) { return ref this.buffer1.Get(id); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.filterEntities[this.index]); }
public void RemoveT1(int id) { this.buffer1.Remove(id); }
public void RemoveT1() { this.buffer1.Remove(this.filterEntities[this.index]); }
public ref readonly T1 ReadT1(int id) { return ref this.buffer1.Read(id); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.filterEntities[this.index]); }
public bool HasT1(int id) { return this.buffer1.Has(id); }
public bool HasT1() { return this.buffer1.Has(this.filterEntities[this.index]); }
public void SetT2(int id, in T2 data) { this.buffer2.Set(id, in data); }
public void SetT2(in T2 data) { this.buffer2.Set(this.filterEntities[this.index], in data); }
public ref T2 GetT2(int id) { return ref this.buffer2.Get(id); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.filterEntities[this.index]); }
public void RemoveT2(int id) { this.buffer2.Remove(id); }
public void RemoveT2() { this.buffer2.Remove(this.filterEntities[this.index]); }
public ref readonly T2 ReadT2(int id) { return ref this.buffer2.Read(id); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.filterEntities[this.index]); }
public bool HasT2(int id) { return this.buffer2.Has(id); }
public bool HasT2() { return this.buffer2.Has(this.filterEntities[this.index]); }
public void SetT3(int id, in T3 data) { this.buffer3.Set(id, in data); }
public void SetT3(in T3 data) { this.buffer3.Set(this.filterEntities[this.index], in data); }
public ref T3 GetT3(int id) { return ref this.buffer3.Get(id); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.filterEntities[this.index]); }
public void RemoveT3(int id) { this.buffer3.Remove(id); }
public void RemoveT3() { this.buffer3.Remove(this.filterEntities[this.index]); }
public ref readonly T3 ReadT3(int id) { return ref this.buffer3.Read(id); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.filterEntities[this.index]); }
public bool HasT3(int id) { return this.buffer3.Has(id); }
public bool HasT3() { return this.buffer3.Has(this.filterEntities[this.index]); }
public void SetT4(int id, in T4 data) { this.buffer4.Set(id, in data); }
public void SetT4(in T4 data) { this.buffer4.Set(this.filterEntities[this.index], in data); }
public ref T4 GetT4(int id) { return ref this.buffer4.Get(id); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.filterEntities[this.index]); }
public void RemoveT4(int id) { this.buffer4.Remove(id); }
public void RemoveT4() { this.buffer4.Remove(this.filterEntities[this.index]); }
public ref readonly T4 ReadT4(int id) { return ref this.buffer4.Read(id); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.filterEntities[this.index]); }
public bool HasT4(int id) { return this.buffer4.Has(id); }
public bool HasT4() { return this.buffer4.Has(this.filterEntities[this.index]); }
public void SetT5(int id, in T5 data) { this.buffer5.Set(id, in data); }
public void SetT5(in T5 data) { this.buffer5.Set(this.filterEntities[this.index], in data); }
public ref T5 GetT5(int id) { return ref this.buffer5.Get(id); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.filterEntities[this.index]); }
public void RemoveT5(int id) { this.buffer5.Remove(id); }
public void RemoveT5() { this.buffer5.Remove(this.filterEntities[this.index]); }
public ref readonly T5 ReadT5(int id) { return ref this.buffer5.Read(id); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.filterEntities[this.index]); }
public bool HasT5(int id) { return this.buffer5.Has(id); }
public bool HasT5() { return this.buffer5.Has(this.filterEntities[this.index]); }
public void SetT6(int id, in T6 data) { this.buffer6.Set(id, in data); }
public void SetT6(in T6 data) { this.buffer6.Set(this.filterEntities[this.index], in data); }
public ref T6 GetT6(int id) { return ref this.buffer6.Get(id); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.filterEntities[this.index]); }
public void RemoveT6(int id) { this.buffer6.Remove(id); }
public void RemoveT6() { this.buffer6.Remove(this.filterEntities[this.index]); }
public ref readonly T6 ReadT6(int id) { return ref this.buffer6.Read(id); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.filterEntities[this.index]); }
public bool HasT6(int id) { return this.buffer6.Has(id); }
public bool HasT6() { return this.buffer6.Has(this.filterEntities[this.index]); }
public void SetT7(int id, in T7 data) { this.buffer7.Set(id, in data); }
public void SetT7(in T7 data) { this.buffer7.Set(this.filterEntities[this.index], in data); }
public ref T7 GetT7(int id) { return ref this.buffer7.Get(id); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.filterEntities[this.index]); }
public void RemoveT7(int id) { this.buffer7.Remove(id); }
public void RemoveT7() { this.buffer7.Remove(this.filterEntities[this.index]); }
public ref readonly T7 ReadT7(int id) { return ref this.buffer7.Read(id); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.filterEntities[this.index]); }
public bool HasT7(int id) { return this.buffer7.Has(id); }
public bool HasT7() { return this.buffer7.Has(this.filterEntities[this.index]); }

    #endregion

}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {

    public readonly int Length;
    public int index;
    private readonly int max;
    [Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.NativeArrayBurst<int> filterEntities;
    
    private DataBuffer<T0> buffer0;private DataBuffer<T1> buffer1;private DataBuffer<T2> buffer2;private DataBuffer<T3> buffer3;private DataBuffer<T4> buffer4;private DataBuffer<T5> buffer5;private DataBuffer<T6> buffer6;private DataBuffer<T7> buffer7;private DataBuffer<T8> buffer8;

    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {

        var world = filter.world;
        var arrEntities = world.currentState.storage.cache;
        this.index = -1;
        this.Length = filter.Count;
        this.max = filter.Count;
        
        this.filterEntities = new ME.ECS.Collections.NativeArrayBurst<int>(filter.Count, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            
            this.filterEntities[idx++] = entity.id;
            
        }
        
        this.buffer0 = new DataBuffer<T0>(world, arrEntities, allocator);
this.buffer1 = new DataBuffer<T1>(world, arrEntities, allocator);
this.buffer2 = new DataBuffer<T2>(world, arrEntities, allocator);
this.buffer3 = new DataBuffer<T3>(world, arrEntities, allocator);
this.buffer4 = new DataBuffer<T4>(world, arrEntities, allocator);
this.buffer5 = new DataBuffer<T5>(world, arrEntities, allocator);
this.buffer6 = new DataBuffer<T6>(world, arrEntities, allocator);
this.buffer7 = new DataBuffer<T7>(world, arrEntities, allocator);
this.buffer8 = new DataBuffer<T8>(world, arrEntities, allocator);

        
    }

    public int GetEntityIdByIndex(int index) {

        return this.filterEntities[index];

    }
    
    public void Push() {

        var world = Worlds.currentWorld;
        var changedCount = 0;
        changedCount += this.buffer0.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer1.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer2.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer3.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer4.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer5.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer6.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer7.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);
changedCount += this.buffer8.Push(world, world.currentState.storage.cache, this.max,  this.filterEntities);

        
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

    public void Reset() {

        this.index = -1;

    }
    
    public bool MoveNext() {
        
        ++this.index;
        return this.index < this.max;

    }
    
    private void Dispose() {
        
        this.filterEntities.Dispose();
        
    }

    #region API
    public void SetT0(int id, in T0 data) { this.buffer0.Set(id, in data); }
public void SetT0(in T0 data) { this.buffer0.Set(this.filterEntities[this.index], in data); }
public ref T0 GetT0(int id) { return ref this.buffer0.Get(id); }
public ref T0 GetT0() { return ref this.buffer0.Get(this.filterEntities[this.index]); }
public void RemoveT0(int id) { this.buffer0.Remove(id); }
public void RemoveT0() { this.buffer0.Remove(this.filterEntities[this.index]); }
public ref readonly T0 ReadT0(int id) { return ref this.buffer0.Read(id); }
public ref readonly T0 ReadT0() { return ref this.buffer0.Read(this.filterEntities[this.index]); }
public bool HasT0(int id) { return this.buffer0.Has(id); }
public bool HasT0() { return this.buffer0.Has(this.filterEntities[this.index]); }
public void SetT1(int id, in T1 data) { this.buffer1.Set(id, in data); }
public void SetT1(in T1 data) { this.buffer1.Set(this.filterEntities[this.index], in data); }
public ref T1 GetT1(int id) { return ref this.buffer1.Get(id); }
public ref T1 GetT1() { return ref this.buffer1.Get(this.filterEntities[this.index]); }
public void RemoveT1(int id) { this.buffer1.Remove(id); }
public void RemoveT1() { this.buffer1.Remove(this.filterEntities[this.index]); }
public ref readonly T1 ReadT1(int id) { return ref this.buffer1.Read(id); }
public ref readonly T1 ReadT1() { return ref this.buffer1.Read(this.filterEntities[this.index]); }
public bool HasT1(int id) { return this.buffer1.Has(id); }
public bool HasT1() { return this.buffer1.Has(this.filterEntities[this.index]); }
public void SetT2(int id, in T2 data) { this.buffer2.Set(id, in data); }
public void SetT2(in T2 data) { this.buffer2.Set(this.filterEntities[this.index], in data); }
public ref T2 GetT2(int id) { return ref this.buffer2.Get(id); }
public ref T2 GetT2() { return ref this.buffer2.Get(this.filterEntities[this.index]); }
public void RemoveT2(int id) { this.buffer2.Remove(id); }
public void RemoveT2() { this.buffer2.Remove(this.filterEntities[this.index]); }
public ref readonly T2 ReadT2(int id) { return ref this.buffer2.Read(id); }
public ref readonly T2 ReadT2() { return ref this.buffer2.Read(this.filterEntities[this.index]); }
public bool HasT2(int id) { return this.buffer2.Has(id); }
public bool HasT2() { return this.buffer2.Has(this.filterEntities[this.index]); }
public void SetT3(int id, in T3 data) { this.buffer3.Set(id, in data); }
public void SetT3(in T3 data) { this.buffer3.Set(this.filterEntities[this.index], in data); }
public ref T3 GetT3(int id) { return ref this.buffer3.Get(id); }
public ref T3 GetT3() { return ref this.buffer3.Get(this.filterEntities[this.index]); }
public void RemoveT3(int id) { this.buffer3.Remove(id); }
public void RemoveT3() { this.buffer3.Remove(this.filterEntities[this.index]); }
public ref readonly T3 ReadT3(int id) { return ref this.buffer3.Read(id); }
public ref readonly T3 ReadT3() { return ref this.buffer3.Read(this.filterEntities[this.index]); }
public bool HasT3(int id) { return this.buffer3.Has(id); }
public bool HasT3() { return this.buffer3.Has(this.filterEntities[this.index]); }
public void SetT4(int id, in T4 data) { this.buffer4.Set(id, in data); }
public void SetT4(in T4 data) { this.buffer4.Set(this.filterEntities[this.index], in data); }
public ref T4 GetT4(int id) { return ref this.buffer4.Get(id); }
public ref T4 GetT4() { return ref this.buffer4.Get(this.filterEntities[this.index]); }
public void RemoveT4(int id) { this.buffer4.Remove(id); }
public void RemoveT4() { this.buffer4.Remove(this.filterEntities[this.index]); }
public ref readonly T4 ReadT4(int id) { return ref this.buffer4.Read(id); }
public ref readonly T4 ReadT4() { return ref this.buffer4.Read(this.filterEntities[this.index]); }
public bool HasT4(int id) { return this.buffer4.Has(id); }
public bool HasT4() { return this.buffer4.Has(this.filterEntities[this.index]); }
public void SetT5(int id, in T5 data) { this.buffer5.Set(id, in data); }
public void SetT5(in T5 data) { this.buffer5.Set(this.filterEntities[this.index], in data); }
public ref T5 GetT5(int id) { return ref this.buffer5.Get(id); }
public ref T5 GetT5() { return ref this.buffer5.Get(this.filterEntities[this.index]); }
public void RemoveT5(int id) { this.buffer5.Remove(id); }
public void RemoveT5() { this.buffer5.Remove(this.filterEntities[this.index]); }
public ref readonly T5 ReadT5(int id) { return ref this.buffer5.Read(id); }
public ref readonly T5 ReadT5() { return ref this.buffer5.Read(this.filterEntities[this.index]); }
public bool HasT5(int id) { return this.buffer5.Has(id); }
public bool HasT5() { return this.buffer5.Has(this.filterEntities[this.index]); }
public void SetT6(int id, in T6 data) { this.buffer6.Set(id, in data); }
public void SetT6(in T6 data) { this.buffer6.Set(this.filterEntities[this.index], in data); }
public ref T6 GetT6(int id) { return ref this.buffer6.Get(id); }
public ref T6 GetT6() { return ref this.buffer6.Get(this.filterEntities[this.index]); }
public void RemoveT6(int id) { this.buffer6.Remove(id); }
public void RemoveT6() { this.buffer6.Remove(this.filterEntities[this.index]); }
public ref readonly T6 ReadT6(int id) { return ref this.buffer6.Read(id); }
public ref readonly T6 ReadT6() { return ref this.buffer6.Read(this.filterEntities[this.index]); }
public bool HasT6(int id) { return this.buffer6.Has(id); }
public bool HasT6() { return this.buffer6.Has(this.filterEntities[this.index]); }
public void SetT7(int id, in T7 data) { this.buffer7.Set(id, in data); }
public void SetT7(in T7 data) { this.buffer7.Set(this.filterEntities[this.index], in data); }
public ref T7 GetT7(int id) { return ref this.buffer7.Get(id); }
public ref T7 GetT7() { return ref this.buffer7.Get(this.filterEntities[this.index]); }
public void RemoveT7(int id) { this.buffer7.Remove(id); }
public void RemoveT7() { this.buffer7.Remove(this.filterEntities[this.index]); }
public ref readonly T7 ReadT7(int id) { return ref this.buffer7.Read(id); }
public ref readonly T7 ReadT7() { return ref this.buffer7.Read(this.filterEntities[this.index]); }
public bool HasT7(int id) { return this.buffer7.Has(id); }
public bool HasT7() { return this.buffer7.Has(this.filterEntities[this.index]); }
public void SetT8(int id, in T8 data) { this.buffer8.Set(id, in data); }
public void SetT8(in T8 data) { this.buffer8.Set(this.filterEntities[this.index], in data); }
public ref T8 GetT8(int id) { return ref this.buffer8.Get(id); }
public ref T8 GetT8() { return ref this.buffer8.Get(this.filterEntities[this.index]); }
public void RemoveT8(int id) { this.buffer8.Remove(id); }
public void RemoveT8() { this.buffer8.Remove(this.filterEntities[this.index]); }
public ref readonly T8 ReadT8(int id) { return ref this.buffer8.Read(id); }
public ref readonly T8 ReadT8() { return ref this.buffer8.Read(this.filterEntities[this.index]); }
public bool HasT8(int id) { return this.buffer8.Has(id); }
public bool HasT8() { return this.buffer8.Has(this.filterEntities[this.index]); }

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
public static void ForEach<T0>(this Filter filter, R<T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0>(this Filter filter, in TCustom0 custom0, RCP1<TCustom0, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP1<TCustom0, TCustom1, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP1<TCustom0, TCustom1, TCustom2, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0> onEach)  where T0:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP1<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0> onEach)  where T0:struct,IStructComponentBase {
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
public static void ForEach<T0,T1>(this Filter filter, R<T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1>(this Filter filter, in TCustom0 custom0, RCP2<TCustom0, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP2<TCustom0, TCustom1, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP2<TCustom0, TCustom1, TCustom2, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP2<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
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
public static void ForEach<T0,T1,T2>(this Filter filter, R<T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2>(this Filter filter, in TCustom0 custom0, RCP3<TCustom0, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP3<TCustom0, TCustom1, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP3<TCustom0, TCustom1, TCustom2, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP3<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
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
public static void ForEach<T0,T1,T2,T3>(this Filter filter, R<T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, RCP4<TCustom0, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP4<TCustom0, TCustom1, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP4<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP4<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
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
public static void ForEach<T0,T1,T2,T3,T4>(this Filter filter, R<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, RCP5<TCustom0, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP5<TCustom0, TCustom1, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP5<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP5<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
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
public static void ForEach<T0,T1,T2,T3,T4,T5>(this Filter filter, R<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, RCP6<TCustom0, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP6<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP6<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP6<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
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
public static void ForEach<T0,T1,T2,T3,T4,T5,T6>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, RCP7<TCustom0, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP7<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP7<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP7<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
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
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, RCP8<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP8<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP8<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP8<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
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
public static void ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, R<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, RCP9<TCustom0, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, RCP9<TCustom0, TCustom1, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, RCP9<TCustom0, TCustom1, TCustom2, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    filter.GetBounds(out var min, out var max);
    var entities = filter.world.currentState.storage.cache;
    var buffer = new EntityBuffer<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filter.world, entities, min, max);
    for (int i = min; i <= max; ++i) { var e = entities.arr[i]; var id = e.id; onEach.Invoke(in custom0, in custom1, in custom2, in custom3, in custom4, in custom5, in custom6, in custom7, in custom8, in e, ref buffer.GetT0(id), ref buffer.GetT1(id), ref buffer.GetT2(id), ref buffer.GetT3(id), ref buffer.GetT4(id), ref buffer.GetT5(id), ref buffer.GetT6(id), ref buffer.GetT7(id), ref buffer.GetT8(id)); }
}
#if INLINE_METHODS
[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
public static void ForEach<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8>(this Filter filter, in TCustom0 custom0, in TCustom1 custom1, in TCustom2 custom2, in TCustom3 custom3, in TCustom4 custom4, in TCustom5 custom5, in TCustom6 custom6, in TCustom7 custom7, in TCustom8 custom8, in TCustom9 custom9, RCP9<TCustom0, TCustom1, TCustom2, TCustom3, TCustom4, TCustom5, TCustom6, TCustom7, TCustom8, TCustom9, T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
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