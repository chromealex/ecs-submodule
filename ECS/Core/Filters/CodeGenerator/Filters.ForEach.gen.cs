#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Filters;
    using Buffers;
    using ME.ECS.Collections;
    
    using Unity.Burst;
    using Unity.Jobs;
    using Unity.Collections.LowLevel.Unsafe;
    using System.Runtime.InteropServices;

    namespace Buffers {

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0> {
    public byte containsT0;
    public byte opsT0;
    public Entity entity;
    public T0 t0;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0>  where T0:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }
    #endregion

}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0,T1> {
    public byte containsT0;public byte containsT1;
    public byte opsT0;public byte opsT1;
    public Entity entity;
    public T0 t0;public T1 t1;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0,T1>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
regT1.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0,T1>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0,T1>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,containsT1 = regT1.components[entity.id].state,
opsT1 = 0,
t1 = AllComponentTypes<T1>.isTag == false ? regT1.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}{
    var op = data.opsT1;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT1);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT1, in data.t1);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 = 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }
    #endregion

}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0,T1,T2> {
    public byte containsT0;public byte containsT1;public byte containsT2;
    public byte opsT0;public byte opsT1;public byte opsT2;
    public Entity entity;
    public T0 t0;public T1 t1;public T2 t2;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
regT1.Merge();var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];
regT2.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0,T1,T2>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,containsT1 = regT1.components[entity.id].state,
opsT1 = 0,
t1 = AllComponentTypes<T1>.isTag == false ? regT1.components[entity.id].data : default,containsT2 = regT2.components[entity.id].state,
opsT2 = 0,
t2 = AllComponentTypes<T2>.isTag == false ? regT2.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}{
    var op = data.opsT1;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT1);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT1, in data.t1);
            ++changedCount;
        }
    }
}{
    var op = data.opsT2;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT2);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT2, in data.t2);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 = 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 = 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }
    #endregion

}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0,T1,T2,T3> {
    public byte containsT0;public byte containsT1;public byte containsT2;public byte containsT3;
    public byte opsT0;public byte opsT1;public byte opsT2;public byte opsT3;
    public Entity entity;
    public T0 t0;public T1 t1;public T2 t2;public T3 t3;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
regT1.Merge();var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];
regT2.Merge();var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];
regT3.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0,T1,T2,T3>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,containsT1 = regT1.components[entity.id].state,
opsT1 = 0,
t1 = AllComponentTypes<T1>.isTag == false ? regT1.components[entity.id].data : default,containsT2 = regT2.components[entity.id].state,
opsT2 = 0,
t2 = AllComponentTypes<T2>.isTag == false ? regT2.components[entity.id].data : default,containsT3 = regT3.components[entity.id].state,
opsT3 = 0,
t3 = AllComponentTypes<T3>.isTag == false ? regT3.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}{
    var op = data.opsT1;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT1);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT1, in data.t1);
            ++changedCount;
        }
    }
}{
    var op = data.opsT2;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT2);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT2, in data.t2);
            ++changedCount;
        }
    }
}{
    var op = data.opsT3;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT3);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT3, in data.t3);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 = 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 = 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 = 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }
    #endregion

}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0,T1,T2,T3,T4> {
    public byte containsT0;public byte containsT1;public byte containsT2;public byte containsT3;public byte containsT4;
    public byte opsT0;public byte opsT1;public byte opsT2;public byte opsT3;public byte opsT4;
    public Entity entity;
    public T0 t0;public T1 t1;public T2 t2;public T3 t3;public T4 t4;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
regT1.Merge();var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];
regT2.Merge();var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];
regT3.Merge();var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];
regT4.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0,T1,T2,T3,T4>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,containsT1 = regT1.components[entity.id].state,
opsT1 = 0,
t1 = AllComponentTypes<T1>.isTag == false ? regT1.components[entity.id].data : default,containsT2 = regT2.components[entity.id].state,
opsT2 = 0,
t2 = AllComponentTypes<T2>.isTag == false ? regT2.components[entity.id].data : default,containsT3 = regT3.components[entity.id].state,
opsT3 = 0,
t3 = AllComponentTypes<T3>.isTag == false ? regT3.components[entity.id].data : default,containsT4 = regT4.components[entity.id].state,
opsT4 = 0,
t4 = AllComponentTypes<T4>.isTag == false ? regT4.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}{
    var op = data.opsT1;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT1);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT1, in data.t1);
            ++changedCount;
        }
    }
}{
    var op = data.opsT2;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT2);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT2, in data.t2);
            ++changedCount;
        }
    }
}{
    var op = data.opsT3;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT3);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT3, in data.t3);
            ++changedCount;
        }
    }
}{
    var op = data.opsT4;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT4);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT4, in data.t4);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 = 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 = 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 = 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 = 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }
    #endregion

}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0,T1,T2,T3,T4,T5> {
    public byte containsT0;public byte containsT1;public byte containsT2;public byte containsT3;public byte containsT4;public byte containsT5;
    public byte opsT0;public byte opsT1;public byte opsT2;public byte opsT3;public byte opsT4;public byte opsT5;
    public Entity entity;
    public T0 t0;public T1 t1;public T2 t2;public T3 t3;public T4 t4;public T5 t5;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4,T5>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
regT1.Merge();var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];
regT2.Merge();var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];
regT3.Merge();var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];
regT4.Merge();var regT5 = (StructComponents<T5>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T5>()];
regT5.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4,T5>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0,T1,T2,T3,T4,T5>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,containsT1 = regT1.components[entity.id].state,
opsT1 = 0,
t1 = AllComponentTypes<T1>.isTag == false ? regT1.components[entity.id].data : default,containsT2 = regT2.components[entity.id].state,
opsT2 = 0,
t2 = AllComponentTypes<T2>.isTag == false ? regT2.components[entity.id].data : default,containsT3 = regT3.components[entity.id].state,
opsT3 = 0,
t3 = AllComponentTypes<T3>.isTag == false ? regT3.components[entity.id].data : default,containsT4 = regT4.components[entity.id].state,
opsT4 = 0,
t4 = AllComponentTypes<T4>.isTag == false ? regT4.components[entity.id].data : default,containsT5 = regT5.components[entity.id].state,
opsT5 = 0,
t5 = AllComponentTypes<T5>.isTag == false ? regT5.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];var regT5 = (StructComponents<T5>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T5>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}{
    var op = data.opsT1;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT1);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT1, in data.t1);
            ++changedCount;
        }
    }
}{
    var op = data.opsT2;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT2);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT2, in data.t2);
            ++changedCount;
        }
    }
}{
    var op = data.opsT3;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT3);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT3, in data.t3);
            ++changedCount;
        }
    }
}{
    var op = data.opsT4;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT4);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT4, in data.t4);
            ++changedCount;
        }
    }
}{
    var op = data.opsT5;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT5);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT5, in data.t5);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 = 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 = 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 = 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 = 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }public void RemoveT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 = 0x4; data.containsT5 = 0; }
public void Set(int index, in T5 component) { ref var data = ref this.arr.GetRef(index); data.t5 = component; data.opsT5 = 0x2; data.containsT5 = 1; }
public ref T5 GetT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 = 0x2; return ref data.t5; }
public ref readonly T5 ReadT5(int index) { return ref this.arr.GetRefRead(index).t5; }
public bool HasT5(int index) { return this.arr.GetRefRead(index).containsT5 > 0; }
    #endregion

}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0,T1,T2,T3,T4,T5,T6> {
    public byte containsT0;public byte containsT1;public byte containsT2;public byte containsT3;public byte containsT4;public byte containsT5;public byte containsT6;
    public byte opsT0;public byte opsT1;public byte opsT2;public byte opsT3;public byte opsT4;public byte opsT5;public byte opsT6;
    public Entity entity;
    public T0 t0;public T1 t1;public T2 t2;public T3 t3;public T4 t4;public T5 t5;public T6 t6;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4,T5,T6>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
regT1.Merge();var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];
regT2.Merge();var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];
regT3.Merge();var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];
regT4.Merge();var regT5 = (StructComponents<T5>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T5>()];
regT5.Merge();var regT6 = (StructComponents<T6>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T6>()];
regT6.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4,T5,T6>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0,T1,T2,T3,T4,T5,T6>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,containsT1 = regT1.components[entity.id].state,
opsT1 = 0,
t1 = AllComponentTypes<T1>.isTag == false ? regT1.components[entity.id].data : default,containsT2 = regT2.components[entity.id].state,
opsT2 = 0,
t2 = AllComponentTypes<T2>.isTag == false ? regT2.components[entity.id].data : default,containsT3 = regT3.components[entity.id].state,
opsT3 = 0,
t3 = AllComponentTypes<T3>.isTag == false ? regT3.components[entity.id].data : default,containsT4 = regT4.components[entity.id].state,
opsT4 = 0,
t4 = AllComponentTypes<T4>.isTag == false ? regT4.components[entity.id].data : default,containsT5 = regT5.components[entity.id].state,
opsT5 = 0,
t5 = AllComponentTypes<T5>.isTag == false ? regT5.components[entity.id].data : default,containsT6 = regT6.components[entity.id].state,
opsT6 = 0,
t6 = AllComponentTypes<T6>.isTag == false ? regT6.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];var regT5 = (StructComponents<T5>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T5>()];var regT6 = (StructComponents<T6>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T6>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}{
    var op = data.opsT1;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT1);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT1, in data.t1);
            ++changedCount;
        }
    }
}{
    var op = data.opsT2;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT2);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT2, in data.t2);
            ++changedCount;
        }
    }
}{
    var op = data.opsT3;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT3);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT3, in data.t3);
            ++changedCount;
        }
    }
}{
    var op = data.opsT4;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT4);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT4, in data.t4);
            ++changedCount;
        }
    }
}{
    var op = data.opsT5;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT5);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT5, in data.t5);
            ++changedCount;
        }
    }
}{
    var op = data.opsT6;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT6);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT6, in data.t6);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 = 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 = 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 = 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 = 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }public void RemoveT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 = 0x4; data.containsT5 = 0; }
public void Set(int index, in T5 component) { ref var data = ref this.arr.GetRef(index); data.t5 = component; data.opsT5 = 0x2; data.containsT5 = 1; }
public ref T5 GetT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 = 0x2; return ref data.t5; }
public ref readonly T5 ReadT5(int index) { return ref this.arr.GetRefRead(index).t5; }
public bool HasT5(int index) { return this.arr.GetRefRead(index).containsT5 > 0; }public void RemoveT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 = 0x4; data.containsT6 = 0; }
public void Set(int index, in T6 component) { ref var data = ref this.arr.GetRef(index); data.t6 = component; data.opsT6 = 0x2; data.containsT6 = 1; }
public ref T6 GetT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 = 0x2; return ref data.t6; }
public ref readonly T6 ReadT6(int index) { return ref this.arr.GetRefRead(index).t6; }
public bool HasT6(int index) { return this.arr.GetRefRead(index).containsT6 > 0; }
    #endregion

}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0,T1,T2,T3,T4,T5,T6,T7> {
    public byte containsT0;public byte containsT1;public byte containsT2;public byte containsT3;public byte containsT4;public byte containsT5;public byte containsT6;public byte containsT7;
    public byte opsT0;public byte opsT1;public byte opsT2;public byte opsT3;public byte opsT4;public byte opsT5;public byte opsT6;public byte opsT7;
    public Entity entity;
    public T0 t0;public T1 t1;public T2 t2;public T3 t3;public T4 t4;public T5 t5;public T6 t6;public T7 t7;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4,T5,T6,T7>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
regT1.Merge();var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];
regT2.Merge();var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];
regT3.Merge();var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];
regT4.Merge();var regT5 = (StructComponents<T5>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T5>()];
regT5.Merge();var regT6 = (StructComponents<T6>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T6>()];
regT6.Merge();var regT7 = (StructComponents<T7>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T7>()];
regT7.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4,T5,T6,T7>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0,T1,T2,T3,T4,T5,T6,T7>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,containsT1 = regT1.components[entity.id].state,
opsT1 = 0,
t1 = AllComponentTypes<T1>.isTag == false ? regT1.components[entity.id].data : default,containsT2 = regT2.components[entity.id].state,
opsT2 = 0,
t2 = AllComponentTypes<T2>.isTag == false ? regT2.components[entity.id].data : default,containsT3 = regT3.components[entity.id].state,
opsT3 = 0,
t3 = AllComponentTypes<T3>.isTag == false ? regT3.components[entity.id].data : default,containsT4 = regT4.components[entity.id].state,
opsT4 = 0,
t4 = AllComponentTypes<T4>.isTag == false ? regT4.components[entity.id].data : default,containsT5 = regT5.components[entity.id].state,
opsT5 = 0,
t5 = AllComponentTypes<T5>.isTag == false ? regT5.components[entity.id].data : default,containsT6 = regT6.components[entity.id].state,
opsT6 = 0,
t6 = AllComponentTypes<T6>.isTag == false ? regT6.components[entity.id].data : default,containsT7 = regT7.components[entity.id].state,
opsT7 = 0,
t7 = AllComponentTypes<T7>.isTag == false ? regT7.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];var regT5 = (StructComponents<T5>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T5>()];var regT6 = (StructComponents<T6>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T6>()];var regT7 = (StructComponents<T7>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T7>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}{
    var op = data.opsT1;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT1);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT1, in data.t1);
            ++changedCount;
        }
    }
}{
    var op = data.opsT2;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT2);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT2, in data.t2);
            ++changedCount;
        }
    }
}{
    var op = data.opsT3;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT3);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT3, in data.t3);
            ++changedCount;
        }
    }
}{
    var op = data.opsT4;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT4);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT4, in data.t4);
            ++changedCount;
        }
    }
}{
    var op = data.opsT5;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT5);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT5, in data.t5);
            ++changedCount;
        }
    }
}{
    var op = data.opsT6;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT6);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT6, in data.t6);
            ++changedCount;
        }
    }
}{
    var op = data.opsT7;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT7);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT7, in data.t7);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 = 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 = 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 = 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 = 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }public void RemoveT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 = 0x4; data.containsT5 = 0; }
public void Set(int index, in T5 component) { ref var data = ref this.arr.GetRef(index); data.t5 = component; data.opsT5 = 0x2; data.containsT5 = 1; }
public ref T5 GetT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 = 0x2; return ref data.t5; }
public ref readonly T5 ReadT5(int index) { return ref this.arr.GetRefRead(index).t5; }
public bool HasT5(int index) { return this.arr.GetRefRead(index).containsT5 > 0; }public void RemoveT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 = 0x4; data.containsT6 = 0; }
public void Set(int index, in T6 component) { ref var data = ref this.arr.GetRef(index); data.t6 = component; data.opsT6 = 0x2; data.containsT6 = 1; }
public ref T6 GetT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 = 0x2; return ref data.t6; }
public ref readonly T6 ReadT6(int index) { return ref this.arr.GetRefRead(index).t6; }
public bool HasT6(int index) { return this.arr.GetRefRead(index).containsT6 > 0; }public void RemoveT7(int index) { ref var data = ref this.arr.GetRef(index); data.opsT7 = 0x4; data.containsT7 = 0; }
public void Set(int index, in T7 component) { ref var data = ref this.arr.GetRef(index); data.t7 = component; data.opsT7 = 0x2; data.containsT7 = 1; }
public ref T7 GetT7(int index) { ref var data = ref this.arr.GetRef(index); data.opsT7 = 0x2; return ref data.t7; }
public ref readonly T7 ReadT7(int index) { return ref this.arr.GetRefRead(index).t7; }
public bool HasT7(int index) { return this.arr.GetRefRead(index).containsT7 > 0; }
    #endregion

}

[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct DataBufferStruct<T0,T1,T2,T3,T4,T5,T6,T7,T8> {
    public byte containsT0;public byte containsT1;public byte containsT2;public byte containsT3;public byte containsT4;public byte containsT5;public byte containsT6;public byte containsT7;public byte containsT8;
    public byte opsT0;public byte opsT1;public byte opsT2;public byte opsT3;public byte opsT4;public byte opsT5;public byte opsT6;public byte opsT7;public byte opsT8;
    public Entity entity;
    public T0 t0;public T1 t1;public T2 t2;public T3 t3;public T4 t4;public T5 t5;public T6 t6;public T7 t7;public T8 t8;
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {

    public readonly int Length;
    [Unity.Collections.NativeDisableParallelForRestriction] private Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4,T5,T6,T7,T8>> arr;
    
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        var world = filter.world;
        this.Length = filter.Count;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];
regT0.Merge();var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];
regT1.Merge();var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];
regT2.Merge();var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];
regT3.Merge();var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];
regT4.Merge();var regT5 = (StructComponents<T5>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T5>()];
regT5.Merge();var regT6 = (StructComponents<T6>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T6>()];
regT6.Merge();var regT7 = (StructComponents<T7>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T7>()];
regT7.Merge();var regT8 = (StructComponents<T8>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T8>()];
regT8.Merge();
        this.arr = new Unity.Collections.NativeArray<DataBufferStruct<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(this.Length, allocator);
        var idx = 0;
        foreach (var entity in filter) {
            this.arr[idx] = new DataBufferStruct<T0,T1,T2,T3,T4,T5,T6,T7,T8>() {
                entity = entity,
                containsT0 = regT0.components[entity.id].state,
opsT0 = 0,
t0 = AllComponentTypes<T0>.isTag == false ? regT0.components[entity.id].data : default,containsT1 = regT1.components[entity.id].state,
opsT1 = 0,
t1 = AllComponentTypes<T1>.isTag == false ? regT1.components[entity.id].data : default,containsT2 = regT2.components[entity.id].state,
opsT2 = 0,
t2 = AllComponentTypes<T2>.isTag == false ? regT2.components[entity.id].data : default,containsT3 = regT3.components[entity.id].state,
opsT3 = 0,
t3 = AllComponentTypes<T3>.isTag == false ? regT3.components[entity.id].data : default,containsT4 = regT4.components[entity.id].state,
opsT4 = 0,
t4 = AllComponentTypes<T4>.isTag == false ? regT4.components[entity.id].data : default,containsT5 = regT5.components[entity.id].state,
opsT5 = 0,
t5 = AllComponentTypes<T5>.isTag == false ? regT5.components[entity.id].data : default,containsT6 = regT6.components[entity.id].state,
opsT6 = 0,
t6 = AllComponentTypes<T6>.isTag == false ? regT6.components[entity.id].data : default,containsT7 = regT7.components[entity.id].state,
opsT7 = 0,
t7 = AllComponentTypes<T7>.isTag == false ? regT7.components[entity.id].data : default,containsT8 = regT8.components[entity.id].state,
opsT8 = 0,
t8 = AllComponentTypes<T8>.isTag == false ? regT8.components[entity.id].data : default,
            };
            ++idx;
        }
    }

    public void Push() {
        var world = Worlds.currentWorld;
        var changedCount = 0;
        var regT0 = (StructComponents<T0>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T0>()];var regT1 = (StructComponents<T1>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T1>()];var regT2 = (StructComponents<T2>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T2>()];var regT3 = (StructComponents<T3>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T3>()];var regT4 = (StructComponents<T4>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T4>()];var regT5 = (StructComponents<T5>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T5>()];var regT6 = (StructComponents<T6>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T6>()];var regT7 = (StructComponents<T7>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T7>()];var regT8 = (StructComponents<T8>)world.currentState.structComponents.list.arr[WorldUtilities.GetAllComponentTypeId<T8>()];
        for (int i = 0; i < this.Length; ++i) {
            ref readonly var data = ref this.arr.GetRefRead(i);
            {
    var op = data.opsT0;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT0);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT0, in data.t0);
            ++changedCount;
        }
    }
}{
    var op = data.opsT1;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT1);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT1, in data.t1);
            ++changedCount;
        }
    }
}{
    var op = data.opsT2;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT2);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT2, in data.t2);
            ++changedCount;
        }
    }
}{
    var op = data.opsT3;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT3);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT3, in data.t3);
            ++changedCount;
        }
    }
}{
    var op = data.opsT4;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT4);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT4, in data.t4);
            ++changedCount;
        }
    }
}{
    var op = data.opsT5;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT5);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT5, in data.t5);
            ++changedCount;
        }
    }
}{
    var op = data.opsT6;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT6);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT6, in data.t6);
            ++changedCount;
        }
    }
}{
    var op = data.opsT7;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT7);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT7, in data.t7);
            ++changedCount;
        }
    }
}{
    var op = data.opsT8;
    if (op != 0) {
        if ((op & 0x4) != 0) {
            DataBufferUtils.PushRemove_INTERNAL(world, in data.entity, regT8);
            ++changedCount;
        } else if ((op & 0x2) != 0) {
            DataBufferUtils.PushSet_INTERNAL(world, in data.entity, regT8, in data.t8);
            ++changedCount;
        }
    }
}
        }
        //if (changedCount > 0) world.UpdateAllFilters();
        this.Dispose();
    }
    
    public int GetEntityId(int index) => this.arr[index].entity.id;

    public ref readonly Entity GetEntity(int index) => ref this.arr.GetRefRead(index).entity;

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 = 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 = 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 = 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 = 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 = 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 = 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 = 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 = 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 = 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 = 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }public void RemoveT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 = 0x4; data.containsT5 = 0; }
public void Set(int index, in T5 component) { ref var data = ref this.arr.GetRef(index); data.t5 = component; data.opsT5 = 0x2; data.containsT5 = 1; }
public ref T5 GetT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 = 0x2; return ref data.t5; }
public ref readonly T5 ReadT5(int index) { return ref this.arr.GetRefRead(index).t5; }
public bool HasT5(int index) { return this.arr.GetRefRead(index).containsT5 > 0; }public void RemoveT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 = 0x4; data.containsT6 = 0; }
public void Set(int index, in T6 component) { ref var data = ref this.arr.GetRef(index); data.t6 = component; data.opsT6 = 0x2; data.containsT6 = 1; }
public ref T6 GetT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 = 0x2; return ref data.t6; }
public ref readonly T6 ReadT6(int index) { return ref this.arr.GetRefRead(index).t6; }
public bool HasT6(int index) { return this.arr.GetRefRead(index).containsT6 > 0; }public void RemoveT7(int index) { ref var data = ref this.arr.GetRef(index); data.opsT7 = 0x4; data.containsT7 = 0; }
public void Set(int index, in T7 component) { ref var data = ref this.arr.GetRef(index); data.t7 = component; data.opsT7 = 0x2; data.containsT7 = 1; }
public ref T7 GetT7(int index) { ref var data = ref this.arr.GetRef(index); data.opsT7 = 0x2; return ref data.t7; }
public ref readonly T7 ReadT7(int index) { return ref this.arr.GetRefRead(index).t7; }
public bool HasT7(int index) { return this.arr.GetRefRead(index).containsT7 > 0; }public void RemoveT8(int index) { ref var data = ref this.arr.GetRef(index); data.opsT8 = 0x4; data.containsT8 = 0; }
public void Set(int index, in T8 component) { ref var data = ref this.arr.GetRef(index); data.t8 = component; data.opsT8 = 0x2; data.containsT8 = 1; }
public ref T8 GetT8(int index) { ref var data = ref this.arr.GetRef(index); data.opsT8 = 0x2; return ref data.t8; }
public ref readonly T8 ReadT8(int index) { return ref this.arr.GetRefRead(index).t8; }
public bool HasT8(int index) { return this.arr.GetRefRead(index).containsT8 > 0; }
    #endregion

}



    }
    
    public static class FiltersForEachExtensions {

        internal static class JobsW<T0>  where T0:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDW<T0>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDW<T0>> ForEach<T0>(this in Filter filter, ME.ECS.Filters.FDW<T0> onEach)  where T0:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDW<T0>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDW<T0>> task, in Filter filterInternal, ME.ECS.Filters.FDW<T0> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsW<T0>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsW<T0>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i));
            bag.Push();

        }

    });
    
}

internal static class JobsR<T0>  where T0:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDR<T0>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDR<T0>> ForEach<T0>(this in Filter filter, ME.ECS.Filters.FDR<T0> onEach)  where T0:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDR<T0>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDR<T0>> task, in Filter filterInternal, ME.ECS.Filters.FDR<T0> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsR<T0>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsR<T0>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i));
            bag.Push();

        }

    });
    
}

internal static class JobsWW<T0,T1>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDWW<T0,T1>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWW<T0,T1>> ForEach<T0,T1>(this in Filter filter, ME.ECS.Filters.FDWW<T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDWW<T0,T1>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDWW<T0,T1>> task, in Filter filterInternal, ME.ECS.Filters.FDWW<T0,T1> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsWW<T0,T1>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsWW<T0,T1>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRW<T0,T1>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRW<T0,T1>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRW<T0,T1>> ForEach<T0,T1>(this in Filter filter, ME.ECS.Filters.FDRW<T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRW<T0,T1>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRW<T0,T1>> task, in Filter filterInternal, ME.ECS.Filters.FDRW<T0,T1> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRW<T0,T1>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRW<T0,T1>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRR<T0,T1>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRR<T0,T1>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRR<T0,T1>> ForEach<T0,T1>(this in Filter filter, ME.ECS.Filters.FDRR<T0,T1> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRR<T0,T1>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRR<T0,T1>> task, in Filter filterInternal, ME.ECS.Filters.FDRR<T0,T1> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRR<T0,T1>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRR<T0,T1>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i));
            bag.Push();

        }

    });
    
}

internal static class JobsWWW<T0,T1,T2>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDWWW<T0,T1,T2>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWW<T0,T1,T2>> ForEach<T0,T1,T2>(this in Filter filter, ME.ECS.Filters.FDWWW<T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWW<T0,T1,T2>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWW<T0,T1,T2>> task, in Filter filterInternal, ME.ECS.Filters.FDWWW<T0,T1,T2> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsWWW<T0,T1,T2>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsWWW<T0,T1,T2>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRWW<T0,T1,T2>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRWW<T0,T1,T2>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWW<T0,T1,T2>> ForEach<T0,T1,T2>(this in Filter filter, ME.ECS.Filters.FDRWW<T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWW<T0,T1,T2>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWW<T0,T1,T2>> task, in Filter filterInternal, ME.ECS.Filters.FDRWW<T0,T1,T2> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRWW<T0,T1,T2>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRWW<T0,T1,T2>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRW<T0,T1,T2>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRW<T0,T1,T2>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRW<T0,T1,T2>> ForEach<T0,T1,T2>(this in Filter filter, ME.ECS.Filters.FDRRW<T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRW<T0,T1,T2>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRW<T0,T1,T2>> task, in Filter filterInternal, ME.ECS.Filters.FDRRW<T0,T1,T2> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRW<T0,T1,T2>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRW<T0,T1,T2>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRR<T0,T1,T2>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRR<T0,T1,T2>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRR<T0,T1,T2>> ForEach<T0,T1,T2>(this in Filter filter, ME.ECS.Filters.FDRRR<T0,T1,T2> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRR<T0,T1,T2>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRR<T0,T1,T2>> task, in Filter filterInternal, ME.ECS.Filters.FDRRR<T0,T1,T2> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRR<T0,T1,T2>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRR<T0,T1,T2>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i));
            bag.Push();

        }

    });
    
}

internal static class JobsWWWW<T0,T1,T2,T3>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDWWWW<T0,T1,T2,T3>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWW<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDWWWW<T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWW<T0,T1,T2,T3>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWW<T0,T1,T2,T3>> task, in Filter filterInternal, ME.ECS.Filters.FDWWWW<T0,T1,T2,T3> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsWWWW<T0,T1,T2,T3>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsWWWW<T0,T1,T2,T3>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRWWW<T0,T1,T2,T3>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRWWW<T0,T1,T2,T3>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWW<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDRWWW<T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWW<T0,T1,T2,T3>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWW<T0,T1,T2,T3>> task, in Filter filterInternal, ME.ECS.Filters.FDRWWW<T0,T1,T2,T3> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRWWW<T0,T1,T2,T3>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRWWW<T0,T1,T2,T3>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRWW<T0,T1,T2,T3>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRWW<T0,T1,T2,T3>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWW<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDRRWW<T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWW<T0,T1,T2,T3>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWW<T0,T1,T2,T3>> task, in Filter filterInternal, ME.ECS.Filters.FDRRWW<T0,T1,T2,T3> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRWW<T0,T1,T2,T3>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRWW<T0,T1,T2,T3>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRW<T0,T1,T2,T3>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRW<T0,T1,T2,T3>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRW<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDRRRW<T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRW<T0,T1,T2,T3>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRW<T0,T1,T2,T3>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRW<T0,T1,T2,T3> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRW<T0,T1,T2,T3>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRW<T0,T1,T2,T3>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRR<T0,T1,T2,T3>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRR<T0,T1,T2,T3>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRR<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDRRRR<T0,T1,T2,T3> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRR<T0,T1,T2,T3>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRR<T0,T1,T2,T3>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRR<T0,T1,T2,T3> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRR<T0,T1,T2,T3>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRR<T0,T1,T2,T3>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i));
            bag.Push();

        }

    });
    
}

internal static class JobsWWWWW<T0,T1,T2,T3,T4>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDWWWWW<T0,T1,T2,T3,T4>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDWWWWW<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWW<T0,T1,T2,T3,T4>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWW<T0,T1,T2,T3,T4>> task, in Filter filterInternal, ME.ECS.Filters.FDWWWWW<T0,T1,T2,T3,T4> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsWWWWW<T0,T1,T2,T3,T4>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsWWWWW<T0,T1,T2,T3,T4>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRWWWW<T0,T1,T2,T3,T4>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRWWWW<T0,T1,T2,T3,T4>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRWWWW<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWW<T0,T1,T2,T3,T4>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWW<T0,T1,T2,T3,T4>> task, in Filter filterInternal, ME.ECS.Filters.FDRWWWW<T0,T1,T2,T3,T4> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRWWWW<T0,T1,T2,T3,T4>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRWWWW<T0,T1,T2,T3,T4>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRWWW<T0,T1,T2,T3,T4>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRWWW<T0,T1,T2,T3,T4>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRRWWW<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWW<T0,T1,T2,T3,T4>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWW<T0,T1,T2,T3,T4>> task, in Filter filterInternal, ME.ECS.Filters.FDRRWWW<T0,T1,T2,T3,T4> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRWWW<T0,T1,T2,T3,T4>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRWWW<T0,T1,T2,T3,T4>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRWW<T0,T1,T2,T3,T4>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRWW<T0,T1,T2,T3,T4>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRRRWW<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWW<T0,T1,T2,T3,T4>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWW<T0,T1,T2,T3,T4>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRWW<T0,T1,T2,T3,T4> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRWW<T0,T1,T2,T3,T4>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRWW<T0,T1,T2,T3,T4>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRW<T0,T1,T2,T3,T4>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRW<T0,T1,T2,T3,T4>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRRRRW<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRW<T0,T1,T2,T3,T4>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRW<T0,T1,T2,T3,T4>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRW<T0,T1,T2,T3,T4> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRW<T0,T1,T2,T3,T4>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRW<T0,T1,T2,T3,T4>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRR<T0,T1,T2,T3,T4>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRR<T0,T1,T2,T3,T4>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRR<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRRRRR<T0,T1,T2,T3,T4> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRR<T0,T1,T2,T3,T4>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRR<T0,T1,T2,T3,T4>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRR<T0,T1,T2,T3,T4> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRR<T0,T1,T2,T3,T4>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRR<T0,T1,T2,T3,T4>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i));
            bag.Push();

        }

    });
    
}

internal static class JobsWWWWWW<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDWWWWWW<T0,T1,T2,T3,T4,T5>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDWWWWWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWW<T0,T1,T2,T3,T4,T5>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWW<T0,T1,T2,T3,T4,T5>> task, in Filter filterInternal, ME.ECS.Filters.FDWWWWWW<T0,T1,T2,T3,T4,T5> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsWWWWWW<T0,T1,T2,T3,T4,T5>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsWWWWWW<T0,T1,T2,T3,T4,T5>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRWWWWW<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRWWWWW<T0,T1,T2,T3,T4,T5>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRWWWWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWW<T0,T1,T2,T3,T4,T5>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWW<T0,T1,T2,T3,T4,T5>> task, in Filter filterInternal, ME.ECS.Filters.FDRWWWWW<T0,T1,T2,T3,T4,T5> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRWWWWW<T0,T1,T2,T3,T4,T5>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRWWWWW<T0,T1,T2,T3,T4,T5>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRWWWW<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRWWWW<T0,T1,T2,T3,T4,T5>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRWWWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWW<T0,T1,T2,T3,T4,T5>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWW<T0,T1,T2,T3,T4,T5>> task, in Filter filterInternal, ME.ECS.Filters.FDRRWWWW<T0,T1,T2,T3,T4,T5> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRWWWW<T0,T1,T2,T3,T4,T5>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRWWWW<T0,T1,T2,T3,T4,T5>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRWWW<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRWWW<T0,T1,T2,T3,T4,T5>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRRWWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWW<T0,T1,T2,T3,T4,T5>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWW<T0,T1,T2,T3,T4,T5>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRWWW<T0,T1,T2,T3,T4,T5> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRWWW<T0,T1,T2,T3,T4,T5>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRWWW<T0,T1,T2,T3,T4,T5>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRWW<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRWW<T0,T1,T2,T3,T4,T5>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRRRWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWW<T0,T1,T2,T3,T4,T5>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWW<T0,T1,T2,T3,T4,T5>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRWW<T0,T1,T2,T3,T4,T5> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRWW<T0,T1,T2,T3,T4,T5>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRWW<T0,T1,T2,T3,T4,T5>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRW<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRW<T0,T1,T2,T3,T4,T5>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),ref bag.GetT5(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRRRRW<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRW<T0,T1,T2,T3,T4,T5>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRW<T0,T1,T2,T3,T4,T5>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRW<T0,T1,T2,T3,T4,T5> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRW<T0,T1,T2,T3,T4,T5>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRW<T0,T1,T2,T3,T4,T5>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),ref bag.GetT5(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRR<T0,T1,T2,T3,T4,T5>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRR<T0,T1,T2,T3,T4,T5>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRR<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRRRRR<T0,T1,T2,T3,T4,T5> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRR<T0,T1,T2,T3,T4,T5>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRR<T0,T1,T2,T3,T4,T5>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRR<T0,T1,T2,T3,T4,T5> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRR<T0,T1,T2,T3,T4,T5>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRR<T0,T1,T2,T3,T4,T5>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i));
            bag.Push();

        }

    });
    
}

internal static class JobsWWWWWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6>> task, in Filter filterInternal, ME.ECS.Filters.FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsWWWWWWW<T0,T1,T2,T3,T4,T5,T6>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsWWWWWWW<T0,T1,T2,T3,T4,T5,T6>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRWWWWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6>> task, in Filter filterInternal, ME.ECS.Filters.FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRWWWWWW<T0,T1,T2,T3,T4,T5,T6>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRWWWWWW<T0,T1,T2,T3,T4,T5,T6>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRWWWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6>> task, in Filter filterInternal, ME.ECS.Filters.FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRWWWWW<T0,T1,T2,T3,T4,T5,T6>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRWWWWW<T0,T1,T2,T3,T4,T5,T6>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRWWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRWWWW<T0,T1,T2,T3,T4,T5,T6>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRWWWW<T0,T1,T2,T3,T4,T5,T6>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRWWW<T0,T1,T2,T3,T4,T5,T6>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRWWW<T0,T1,T2,T3,T4,T5,T6>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRWW<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRWW<T0,T1,T2,T3,T4,T5,T6>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRWW<T0,T1,T2,T3,T4,T5,T6>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRW<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),ref bag.GetT6(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRW<T0,T1,T2,T3,T4,T5,T6>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRW<T0,T1,T2,T3,T4,T5,T6>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),ref bag.GetT6(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRR<T0,T1,T2,T3,T4,T5,T6>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRR<T0,T1,T2,T3,T4,T5,T6>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRR<T0,T1,T2,T3,T4,T5,T6>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i));
            bag.Push();

        }

    });
    
}

internal static class JobsWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),ref bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),ref bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),in bag.GetT7(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),in bag.GetT7(i));
            bag.Push();

        }

    });
    
}

internal static class JobsWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), ref bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),ref bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),ref bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),ref bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),ref bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),ref bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),ref bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),ref bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),in bag.GetT7(i),ref bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),in bag.GetT7(i),ref bag.GetT8(i));
            bag.Push();

        }

    });
    
}

internal static class JobsRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
    public unsafe struct Job : IJob {

        [NativeDisableUnsafePtrRestriction]
        public System.IntPtr fn;
        public FunctionPointer<ForEachUtils.InternalDelegate> func;
        public FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag;

        public void Execute() {
            var ptr = UnsafeUtility.AddressOf(ref this.bag);
            this.func.Invoke((void*)this.fn, ptr);
            UnsafeUtility.CopyPtrToStructure(ptr, out this.bag);
        }
    
        [BurstCompile(FloatPrecision.High, FloatMode.Deterministic, CompileSynchronously = true)]
        public static void Run(void* fn, void* bagPtr) {
            UnsafeUtility.CopyPtrToStructure(bagPtr, out FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> bag);
            var del = new FunctionPointer<ME.ECS.Filters.FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>>((System.IntPtr)fn);
            for (int i = 0; i < bag.Length; ++i) {
                del.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),in bag.GetT7(i),in bag.GetT8(i));
            }
            UnsafeUtility.CopyStructureToPtr(ref bag, bagPtr);
        }

    }

}

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:struct,IStructComponentBase where T1:struct,IStructComponentBase where T2:struct,IStructComponentBase where T3:struct,IStructComponentBase where T4:struct,IStructComponentBase where T5:struct,IStructComponentBase where T6:struct,IStructComponentBase where T7:struct,IStructComponentBase where T8:struct,IStructComponentBase {
    
    return new ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>>(in filter, onEach, (in ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>> task, in Filter filterInternal, ME.ECS.Filters.FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEachInternal) => {

        if (task.withBurst == true) {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.TempJob);
            var handle = GCHandle.Alloc(onEachInternal);
            var job = new JobsRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job() {
                fn = Marshal.GetFunctionPointerForDelegate(onEachInternal),
                func = BurstCompiler.CompileFunctionPointer<ForEachUtils.InternalDelegate>(JobsRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>.Job.Run),
                bag = bag,
            };
            job.Schedule().Complete();
            handle.Free();
            bag.Push();

        } else {

            var bag = new FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8>(filterInternal, Unity.Collections.Allocator.Persistent);
            for (int i = 0; i < bag.Length; ++i) onEachInternal.Invoke(in bag.GetEntity(i), in bag.GetT0(i),in bag.GetT1(i),in bag.GetT2(i),in bag.GetT3(i),in bag.GetT4(i),in bag.GetT5(i),in bag.GetT6(i),in bag.GetT7(i),in bag.GetT8(i));
            bag.Push();

        }

    });
    
}



    }

}