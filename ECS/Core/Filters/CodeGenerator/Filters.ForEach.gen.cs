#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Filters;
    using Buffers;
    using ME.ECS.Collections;
    
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,
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
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,containsT1 = regT1.componentsStates[entity.id],
opsT1 = 0,
t1 = regT1.components.Length > 0 ? regT1.components.data.arr[entity.id] : default,
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

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 |= 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x2; return ref data.t1; }
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,containsT1 = regT1.componentsStates[entity.id],
opsT1 = 0,
t1 = regT1.components.Length > 0 ? regT1.components.data.arr[entity.id] : default,containsT2 = regT2.componentsStates[entity.id],
opsT2 = 0,
t2 = regT2.components.Length > 0 ? regT2.components.data.arr[entity.id] : default,
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

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 |= 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 |= 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x2; return ref data.t2; }
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,containsT1 = regT1.componentsStates[entity.id],
opsT1 = 0,
t1 = regT1.components.Length > 0 ? regT1.components.data.arr[entity.id] : default,containsT2 = regT2.componentsStates[entity.id],
opsT2 = 0,
t2 = regT2.components.Length > 0 ? regT2.components.data.arr[entity.id] : default,containsT3 = regT3.componentsStates[entity.id],
opsT3 = 0,
t3 = regT3.components.Length > 0 ? regT3.components.data.arr[entity.id] : default,
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

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 |= 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 |= 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 |= 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x2; return ref data.t3; }
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,containsT1 = regT1.componentsStates[entity.id],
opsT1 = 0,
t1 = regT1.components.Length > 0 ? regT1.components.data.arr[entity.id] : default,containsT2 = regT2.componentsStates[entity.id],
opsT2 = 0,
t2 = regT2.components.Length > 0 ? regT2.components.data.arr[entity.id] : default,containsT3 = regT3.componentsStates[entity.id],
opsT3 = 0,
t3 = regT3.components.Length > 0 ? regT3.components.data.arr[entity.id] : default,containsT4 = regT4.componentsStates[entity.id],
opsT4 = 0,
t4 = regT4.components.Length > 0 ? regT4.components.data.arr[entity.id] : default,
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

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 |= 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 |= 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 |= 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 |= 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x2; return ref data.t4; }
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,containsT1 = regT1.componentsStates[entity.id],
opsT1 = 0,
t1 = regT1.components.Length > 0 ? regT1.components.data.arr[entity.id] : default,containsT2 = regT2.componentsStates[entity.id],
opsT2 = 0,
t2 = regT2.components.Length > 0 ? regT2.components.data.arr[entity.id] : default,containsT3 = regT3.componentsStates[entity.id],
opsT3 = 0,
t3 = regT3.components.Length > 0 ? regT3.components.data.arr[entity.id] : default,containsT4 = regT4.componentsStates[entity.id],
opsT4 = 0,
t4 = regT4.components.Length > 0 ? regT4.components.data.arr[entity.id] : default,containsT5 = regT5.componentsStates[entity.id],
opsT5 = 0,
t5 = regT5.components.Length > 0 ? regT5.components.data.arr[entity.id] : default,
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

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 |= 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 |= 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 |= 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 |= 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }public void RemoveT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 |= 0x4; data.containsT5 = 0; }
public void Set(int index, in T5 component) { ref var data = ref this.arr.GetRef(index); data.t5 = component; data.opsT5 |= 0x2; data.containsT5 = 1; }
public ref T5 GetT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 |= 0x2; return ref data.t5; }
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,containsT1 = regT1.componentsStates[entity.id],
opsT1 = 0,
t1 = regT1.components.Length > 0 ? regT1.components.data.arr[entity.id] : default,containsT2 = regT2.componentsStates[entity.id],
opsT2 = 0,
t2 = regT2.components.Length > 0 ? regT2.components.data.arr[entity.id] : default,containsT3 = regT3.componentsStates[entity.id],
opsT3 = 0,
t3 = regT3.components.Length > 0 ? regT3.components.data.arr[entity.id] : default,containsT4 = regT4.componentsStates[entity.id],
opsT4 = 0,
t4 = regT4.components.Length > 0 ? regT4.components.data.arr[entity.id] : default,containsT5 = regT5.componentsStates[entity.id],
opsT5 = 0,
t5 = regT5.components.Length > 0 ? regT5.components.data.arr[entity.id] : default,containsT6 = regT6.componentsStates[entity.id],
opsT6 = 0,
t6 = regT6.components.Length > 0 ? regT6.components.data.arr[entity.id] : default,
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

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 |= 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 |= 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 |= 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 |= 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }public void RemoveT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 |= 0x4; data.containsT5 = 0; }
public void Set(int index, in T5 component) { ref var data = ref this.arr.GetRef(index); data.t5 = component; data.opsT5 |= 0x2; data.containsT5 = 1; }
public ref T5 GetT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 |= 0x2; return ref data.t5; }
public ref readonly T5 ReadT5(int index) { return ref this.arr.GetRefRead(index).t5; }
public bool HasT5(int index) { return this.arr.GetRefRead(index).containsT5 > 0; }public void RemoveT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 |= 0x4; data.containsT6 = 0; }
public void Set(int index, in T6 component) { ref var data = ref this.arr.GetRef(index); data.t6 = component; data.opsT6 |= 0x2; data.containsT6 = 1; }
public ref T6 GetT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 |= 0x2; return ref data.t6; }
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,containsT1 = regT1.componentsStates[entity.id],
opsT1 = 0,
t1 = regT1.components.Length > 0 ? regT1.components.data.arr[entity.id] : default,containsT2 = regT2.componentsStates[entity.id],
opsT2 = 0,
t2 = regT2.components.Length > 0 ? regT2.components.data.arr[entity.id] : default,containsT3 = regT3.componentsStates[entity.id],
opsT3 = 0,
t3 = regT3.components.Length > 0 ? regT3.components.data.arr[entity.id] : default,containsT4 = regT4.componentsStates[entity.id],
opsT4 = 0,
t4 = regT4.components.Length > 0 ? regT4.components.data.arr[entity.id] : default,containsT5 = regT5.componentsStates[entity.id],
opsT5 = 0,
t5 = regT5.components.Length > 0 ? regT5.components.data.arr[entity.id] : default,containsT6 = regT6.componentsStates[entity.id],
opsT6 = 0,
t6 = regT6.components.Length > 0 ? regT6.components.data.arr[entity.id] : default,containsT7 = regT7.componentsStates[entity.id],
opsT7 = 0,
t7 = regT7.components.Length > 0 ? regT7.components.data.arr[entity.id] : default,
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

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 |= 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 |= 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 |= 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 |= 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }public void RemoveT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 |= 0x4; data.containsT5 = 0; }
public void Set(int index, in T5 component) { ref var data = ref this.arr.GetRef(index); data.t5 = component; data.opsT5 |= 0x2; data.containsT5 = 1; }
public ref T5 GetT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 |= 0x2; return ref data.t5; }
public ref readonly T5 ReadT5(int index) { return ref this.arr.GetRefRead(index).t5; }
public bool HasT5(int index) { return this.arr.GetRefRead(index).containsT5 > 0; }public void RemoveT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 |= 0x4; data.containsT6 = 0; }
public void Set(int index, in T6 component) { ref var data = ref this.arr.GetRef(index); data.t6 = component; data.opsT6 |= 0x2; data.containsT6 = 1; }
public ref T6 GetT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 |= 0x2; return ref data.t6; }
public ref readonly T6 ReadT6(int index) { return ref this.arr.GetRefRead(index).t6; }
public bool HasT6(int index) { return this.arr.GetRefRead(index).containsT6 > 0; }public void RemoveT7(int index) { ref var data = ref this.arr.GetRef(index); data.opsT7 |= 0x4; data.containsT7 = 0; }
public void Set(int index, in T7 component) { ref var data = ref this.arr.GetRef(index); data.t7 = component; data.opsT7 |= 0x2; data.containsT7 = 1; }
public ref T7 GetT7(int index) { ref var data = ref this.arr.GetRef(index); data.opsT7 |= 0x2; return ref data.t7; }
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
                containsT0 = regT0.componentsStates[entity.id],
opsT0 = 0,
t0 = regT0.components.Length > 0 ? regT0.components.data.arr[entity.id] : default,containsT1 = regT1.componentsStates[entity.id],
opsT1 = 0,
t1 = regT1.components.Length > 0 ? regT1.components.data.arr[entity.id] : default,containsT2 = regT2.componentsStates[entity.id],
opsT2 = 0,
t2 = regT2.components.Length > 0 ? regT2.components.data.arr[entity.id] : default,containsT3 = regT3.componentsStates[entity.id],
opsT3 = 0,
t3 = regT3.components.Length > 0 ? regT3.components.data.arr[entity.id] : default,containsT4 = regT4.componentsStates[entity.id],
opsT4 = 0,
t4 = regT4.components.Length > 0 ? regT4.components.data.arr[entity.id] : default,containsT5 = regT5.componentsStates[entity.id],
opsT5 = 0,
t5 = regT5.components.Length > 0 ? regT5.components.data.arr[entity.id] : default,containsT6 = regT6.componentsStates[entity.id],
opsT6 = 0,
t6 = regT6.components.Length > 0 ? regT6.components.data.arr[entity.id] : default,containsT7 = regT7.componentsStates[entity.id],
opsT7 = 0,
t7 = regT7.components.Length > 0 ? regT7.components.data.arr[entity.id] : default,containsT8 = regT8.componentsStates[entity.id],
opsT8 = 0,
t8 = regT8.components.Length > 0 ? regT8.components.data.arr[entity.id] : default,
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

    public void Revert() => this.Dispose();

    private void Dispose() => this.arr.Dispose();

    #region API
    public void RemoveT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x4; data.containsT0 = 0; }
public void Set(int index, in T0 component) { ref var data = ref this.arr.GetRef(index); data.t0 = component; data.opsT0 |= 0x2; data.containsT0 = 1; }
public ref T0 GetT0(int index) { ref var data = ref this.arr.GetRef(index); data.opsT0 |= 0x2; return ref data.t0; }
public ref readonly T0 ReadT0(int index) { return ref this.arr.GetRefRead(index).t0; }
public bool HasT0(int index) { return this.arr.GetRefRead(index).containsT0 > 0; }public void RemoveT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x4; data.containsT1 = 0; }
public void Set(int index, in T1 component) { ref var data = ref this.arr.GetRef(index); data.t1 = component; data.opsT1 |= 0x2; data.containsT1 = 1; }
public ref T1 GetT1(int index) { ref var data = ref this.arr.GetRef(index); data.opsT1 |= 0x2; return ref data.t1; }
public ref readonly T1 ReadT1(int index) { return ref this.arr.GetRefRead(index).t1; }
public bool HasT1(int index) { return this.arr.GetRefRead(index).containsT1 > 0; }public void RemoveT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x4; data.containsT2 = 0; }
public void Set(int index, in T2 component) { ref var data = ref this.arr.GetRef(index); data.t2 = component; data.opsT2 |= 0x2; data.containsT2 = 1; }
public ref T2 GetT2(int index) { ref var data = ref this.arr.GetRef(index); data.opsT2 |= 0x2; return ref data.t2; }
public ref readonly T2 ReadT2(int index) { return ref this.arr.GetRefRead(index).t2; }
public bool HasT2(int index) { return this.arr.GetRefRead(index).containsT2 > 0; }public void RemoveT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x4; data.containsT3 = 0; }
public void Set(int index, in T3 component) { ref var data = ref this.arr.GetRef(index); data.t3 = component; data.opsT3 |= 0x2; data.containsT3 = 1; }
public ref T3 GetT3(int index) { ref var data = ref this.arr.GetRef(index); data.opsT3 |= 0x2; return ref data.t3; }
public ref readonly T3 ReadT3(int index) { return ref this.arr.GetRefRead(index).t3; }
public bool HasT3(int index) { return this.arr.GetRefRead(index).containsT3 > 0; }public void RemoveT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x4; data.containsT4 = 0; }
public void Set(int index, in T4 component) { ref var data = ref this.arr.GetRef(index); data.t4 = component; data.opsT4 |= 0x2; data.containsT4 = 1; }
public ref T4 GetT4(int index) { ref var data = ref this.arr.GetRef(index); data.opsT4 |= 0x2; return ref data.t4; }
public ref readonly T4 ReadT4(int index) { return ref this.arr.GetRefRead(index).t4; }
public bool HasT4(int index) { return this.arr.GetRefRead(index).containsT4 > 0; }public void RemoveT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 |= 0x4; data.containsT5 = 0; }
public void Set(int index, in T5 component) { ref var data = ref this.arr.GetRef(index); data.t5 = component; data.opsT5 |= 0x2; data.containsT5 = 1; }
public ref T5 GetT5(int index) { ref var data = ref this.arr.GetRef(index); data.opsT5 |= 0x2; return ref data.t5; }
public ref readonly T5 ReadT5(int index) { return ref this.arr.GetRefRead(index).t5; }
public bool HasT5(int index) { return this.arr.GetRefRead(index).containsT5 > 0; }public void RemoveT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 |= 0x4; data.containsT6 = 0; }
public void Set(int index, in T6 component) { ref var data = ref this.arr.GetRef(index); data.t6 = component; data.opsT6 |= 0x2; data.containsT6 = 1; }
public ref T6 GetT6(int index) { ref var data = ref this.arr.GetRef(index); data.opsT6 |= 0x2; return ref data.t6; }
public ref readonly T6 ReadT6(int index) { return ref this.arr.GetRefRead(index).t6; }
public bool HasT6(int index) { return this.arr.GetRefRead(index).containsT6 > 0; }public void RemoveT7(int index) { ref var data = ref this.arr.GetRef(index); data.opsT7 |= 0x4; data.containsT7 = 0; }
public void Set(int index, in T7 component) { ref var data = ref this.arr.GetRef(index); data.t7 = component; data.opsT7 |= 0x2; data.containsT7 = 1; }
public ref T7 GetT7(int index) { ref var data = ref this.arr.GetRef(index); data.opsT7 |= 0x2; return ref data.t7; }
public ref readonly T7 ReadT7(int index) { return ref this.arr.GetRefRead(index).t7; }
public bool HasT7(int index) { return this.arr.GetRefRead(index).containsT7 > 0; }public void RemoveT8(int index) { ref var data = ref this.arr.GetRef(index); data.opsT8 |= 0x4; data.containsT8 = 0; }
public void Set(int index, in T8 component) { ref var data = ref this.arr.GetRef(index); data.t8 = component; data.opsT8 |= 0x2; data.containsT8 = 1; }
public ref T8 GetT8(int index) { ref var data = ref this.arr.GetRef(index); data.opsT8 |= 0x2; return ref data.t8; }
public ref readonly T8 ReadT8(int index) { return ref this.arr.GetRefRead(index).t8; }
public bool HasT8(int index) { return this.arr.GetRefRead(index).containsT8 > 0; }
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