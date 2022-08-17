#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Filters;
    using Buffers;
    using ME.ECS.Collections;
    using ME.ECS.Collections.V3;
    
    using Unity.Burst;
    using Unity.Jobs;
    using Unity.Collections.LowLevel.Unsafe;
    using System.Runtime.InteropServices;

    public interface IFilterBag {
        int Count { get; }
        void BeginForEachIndex(int chunkIndex);
        void EndForEachIndex();
    }

    public unsafe struct Ptr {

        public Collections.MemoryAllocator.SparseSetData value;

    }

    public struct Op {

        public int entityIndex;
        public int componentId; // -1 = entity
        public byte code; // 1 - change, 2 - remove

    }
    
    public struct Ops {

        public int Length => this.items.Length;
        private Unity.Collections.NativeArray<Op> items;
        private Unity.Collections.NativeArray<bool> exist;
        
        public Ops(int length) {
            
            this.items = new Unity.Collections.NativeArray<Op>(length, Unity.Collections.Allocator.Temp);
            this.exist = new Unity.Collections.NativeArray<bool>(length, Unity.Collections.Allocator.Temp);
            
        }

        public void Write(Op op) {
            
            this.items[op.entityIndex] = op;
            this.exist[op.entityIndex] = true;

        }

        public bool Read(int index, out Op op) {

            if (this.exist[index] == true) {
                op = this.items[index];
                return true;
            }

            op = default;
            return false;

        }

        public void BeginForEachIndex(int index) {
            
        }

        public void EndForEachIndex() {
            
        }

        public void Dispose() {

            this.items.Dispose();
            this.exist.Dispose();

        }

    }
    
    namespace Buffers {

        #if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0> : IFilterBag  where T0:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0,T1> : IFilterBag  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T1>* regs1;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops1;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);
ref var regs1 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T1>>(allRegs.items[in memAllocator, AllComponentTypes<T1>.typeId]);
regs1.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);
if (regs1.components.Has(ref memAllocator, entityId).state == 0) regs1.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);this.regs1 = (UnmanagedComponentsStorage.Item<T1>*)UnsafeUtility.AddressOf(ref regs1);
this.ops1 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops1.Length; ++i) {
    if (this.ops1.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T1>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1, this.ReadT1(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  
this.ops1.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT1(int index) {
    if (this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T1 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs1->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T1 GetT1(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
    return ref this.regs1->components.GetPtr(this.allocator, entityId).data;
}
public T1 ReadT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT1(int index) => this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0,T1,T2> : IFilterBag  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T1>* regs1;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops1;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T2>* regs2;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops2;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);
ref var regs1 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T1>>(allRegs.items[in memAllocator, AllComponentTypes<T1>.typeId]);
regs1.Merge(ref memAllocator);
ref var regs2 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T2>>(allRegs.items[in memAllocator, AllComponentTypes<T2>.typeId]);
regs2.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);
if (regs1.components.Has(ref memAllocator, entityId).state == 0) regs1.components.Set(ref memAllocator, entityId, default);
if (regs2.components.Has(ref memAllocator, entityId).state == 0) regs2.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);this.regs1 = (UnmanagedComponentsStorage.Item<T1>*)UnsafeUtility.AddressOf(ref regs1);
this.ops1 = new Ops(this.Length);this.regs2 = (UnmanagedComponentsStorage.Item<T2>*)UnsafeUtility.AddressOf(ref regs2);
this.ops2 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops1.Length; ++i) {
    if (this.ops1.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T1>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1, this.ReadT1(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops2.Length; ++i) {
    if (this.ops2.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T2>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2, this.ReadT2(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  
this.ops1.Dispose();  
this.ops2.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT1(int index) {
    if (this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T1 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs1->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T1 GetT1(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
    return ref this.regs1->components.GetPtr(this.allocator, entityId).data;
}
public T1 ReadT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT1(int index) => this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT2(int index) {
    if (this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T2 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs2->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T2 GetT2(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
    return ref this.regs2->components.GetPtr(this.allocator, entityId).data;
}
public T2 ReadT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT2(int index) => this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0,T1,T2,T3> : IFilterBag  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T1>* regs1;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops1;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T2>* regs2;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops2;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T3>* regs3;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops3;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);
ref var regs1 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T1>>(allRegs.items[in memAllocator, AllComponentTypes<T1>.typeId]);
regs1.Merge(ref memAllocator);
ref var regs2 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T2>>(allRegs.items[in memAllocator, AllComponentTypes<T2>.typeId]);
regs2.Merge(ref memAllocator);
ref var regs3 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T3>>(allRegs.items[in memAllocator, AllComponentTypes<T3>.typeId]);
regs3.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);
if (regs1.components.Has(ref memAllocator, entityId).state == 0) regs1.components.Set(ref memAllocator, entityId, default);
if (regs2.components.Has(ref memAllocator, entityId).state == 0) regs2.components.Set(ref memAllocator, entityId, default);
if (regs3.components.Has(ref memAllocator, entityId).state == 0) regs3.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);this.regs1 = (UnmanagedComponentsStorage.Item<T1>*)UnsafeUtility.AddressOf(ref regs1);
this.ops1 = new Ops(this.Length);this.regs2 = (UnmanagedComponentsStorage.Item<T2>*)UnsafeUtility.AddressOf(ref regs2);
this.ops2 = new Ops(this.Length);this.regs3 = (UnmanagedComponentsStorage.Item<T3>*)UnsafeUtility.AddressOf(ref regs3);
this.ops3 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops1.Length; ++i) {
    if (this.ops1.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T1>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1, this.ReadT1(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops2.Length; ++i) {
    if (this.ops2.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T2>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2, this.ReadT2(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops3.Length; ++i) {
    if (this.ops3.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T3>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3, this.ReadT3(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  
this.ops1.Dispose();  
this.ops2.Dispose();  
this.ops3.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT1(int index) {
    if (this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T1 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs1->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T1 GetT1(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
    return ref this.regs1->components.GetPtr(this.allocator, entityId).data;
}
public T1 ReadT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT1(int index) => this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT2(int index) {
    if (this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T2 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs2->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T2 GetT2(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
    return ref this.regs2->components.GetPtr(this.allocator, entityId).data;
}
public T2 ReadT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT2(int index) => this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT3(int index) {
    if (this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T3 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs3->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T3 GetT3(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
    return ref this.regs3->components.GetPtr(this.allocator, entityId).data;
}
public T3 ReadT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT3(int index) => this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0,T1,T2,T3,T4> : IFilterBag  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T1>* regs1;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops1;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T2>* regs2;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops2;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T3>* regs3;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops3;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T4>* regs4;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops4;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);
ref var regs1 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T1>>(allRegs.items[in memAllocator, AllComponentTypes<T1>.typeId]);
regs1.Merge(ref memAllocator);
ref var regs2 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T2>>(allRegs.items[in memAllocator, AllComponentTypes<T2>.typeId]);
regs2.Merge(ref memAllocator);
ref var regs3 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T3>>(allRegs.items[in memAllocator, AllComponentTypes<T3>.typeId]);
regs3.Merge(ref memAllocator);
ref var regs4 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T4>>(allRegs.items[in memAllocator, AllComponentTypes<T4>.typeId]);
regs4.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);
if (regs1.components.Has(ref memAllocator, entityId).state == 0) regs1.components.Set(ref memAllocator, entityId, default);
if (regs2.components.Has(ref memAllocator, entityId).state == 0) regs2.components.Set(ref memAllocator, entityId, default);
if (regs3.components.Has(ref memAllocator, entityId).state == 0) regs3.components.Set(ref memAllocator, entityId, default);
if (regs4.components.Has(ref memAllocator, entityId).state == 0) regs4.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);this.regs1 = (UnmanagedComponentsStorage.Item<T1>*)UnsafeUtility.AddressOf(ref regs1);
this.ops1 = new Ops(this.Length);this.regs2 = (UnmanagedComponentsStorage.Item<T2>*)UnsafeUtility.AddressOf(ref regs2);
this.ops2 = new Ops(this.Length);this.regs3 = (UnmanagedComponentsStorage.Item<T3>*)UnsafeUtility.AddressOf(ref regs3);
this.ops3 = new Ops(this.Length);this.regs4 = (UnmanagedComponentsStorage.Item<T4>*)UnsafeUtility.AddressOf(ref regs4);
this.ops4 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops1.Length; ++i) {
    if (this.ops1.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T1>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1, this.ReadT1(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops2.Length; ++i) {
    if (this.ops2.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T2>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2, this.ReadT2(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops3.Length; ++i) {
    if (this.ops3.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T3>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3, this.ReadT3(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops4.Length; ++i) {
    if (this.ops4.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T4>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4, this.ReadT4(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  
this.ops1.Dispose();  
this.ops2.Dispose();  
this.ops3.Dispose();  
this.ops4.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT1(int index) {
    if (this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T1 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs1->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T1 GetT1(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
    return ref this.regs1->components.GetPtr(this.allocator, entityId).data;
}
public T1 ReadT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT1(int index) => this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT2(int index) {
    if (this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T2 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs2->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T2 GetT2(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
    return ref this.regs2->components.GetPtr(this.allocator, entityId).data;
}
public T2 ReadT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT2(int index) => this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT3(int index) {
    if (this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T3 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs3->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T3 GetT3(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
    return ref this.regs3->components.GetPtr(this.allocator, entityId).data;
}
public T3 ReadT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT3(int index) => this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT4(int index) {
    if (this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T4 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs4->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T4 GetT4(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
    return ref this.regs4->components.GetPtr(this.allocator, entityId).data;
}
public T4 ReadT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT4(int index) => this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0,T1,T2,T3,T4,T5> : IFilterBag  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T1>* regs1;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops1;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T2>* regs2;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops2;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T3>* regs3;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops3;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T4>* regs4;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops4;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T5>* regs5;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops5;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);
ref var regs1 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T1>>(allRegs.items[in memAllocator, AllComponentTypes<T1>.typeId]);
regs1.Merge(ref memAllocator);
ref var regs2 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T2>>(allRegs.items[in memAllocator, AllComponentTypes<T2>.typeId]);
regs2.Merge(ref memAllocator);
ref var regs3 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T3>>(allRegs.items[in memAllocator, AllComponentTypes<T3>.typeId]);
regs3.Merge(ref memAllocator);
ref var regs4 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T4>>(allRegs.items[in memAllocator, AllComponentTypes<T4>.typeId]);
regs4.Merge(ref memAllocator);
ref var regs5 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T5>>(allRegs.items[in memAllocator, AllComponentTypes<T5>.typeId]);
regs5.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);
if (regs1.components.Has(ref memAllocator, entityId).state == 0) regs1.components.Set(ref memAllocator, entityId, default);
if (regs2.components.Has(ref memAllocator, entityId).state == 0) regs2.components.Set(ref memAllocator, entityId, default);
if (regs3.components.Has(ref memAllocator, entityId).state == 0) regs3.components.Set(ref memAllocator, entityId, default);
if (regs4.components.Has(ref memAllocator, entityId).state == 0) regs4.components.Set(ref memAllocator, entityId, default);
if (regs5.components.Has(ref memAllocator, entityId).state == 0) regs5.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);this.regs1 = (UnmanagedComponentsStorage.Item<T1>*)UnsafeUtility.AddressOf(ref regs1);
this.ops1 = new Ops(this.Length);this.regs2 = (UnmanagedComponentsStorage.Item<T2>*)UnsafeUtility.AddressOf(ref regs2);
this.ops2 = new Ops(this.Length);this.regs3 = (UnmanagedComponentsStorage.Item<T3>*)UnsafeUtility.AddressOf(ref regs3);
this.ops3 = new Ops(this.Length);this.regs4 = (UnmanagedComponentsStorage.Item<T4>*)UnsafeUtility.AddressOf(ref regs4);
this.ops4 = new Ops(this.Length);this.regs5 = (UnmanagedComponentsStorage.Item<T5>*)UnsafeUtility.AddressOf(ref regs5);
this.ops5 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops1.Length; ++i) {
    if (this.ops1.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T1>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1, this.ReadT1(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops2.Length; ++i) {
    if (this.ops2.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T2>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2, this.ReadT2(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops3.Length; ++i) {
    if (this.ops3.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T3>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3, this.ReadT3(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops4.Length; ++i) {
    if (this.ops4.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T4>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4, this.ReadT4(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops5.Length; ++i) {
    if (this.ops5.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T5>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs5, this.ReadT5(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs5);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  
this.ops1.Dispose();  
this.ops2.Dispose();  
this.ops3.Dispose();  
this.ops4.Dispose();  
this.ops5.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT1(int index) {
    if (this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T1 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs1->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T1 GetT1(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
    return ref this.regs1->components.GetPtr(this.allocator, entityId).data;
}
public T1 ReadT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT1(int index) => this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT2(int index) {
    if (this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T2 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs2->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T2 GetT2(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
    return ref this.regs2->components.GetPtr(this.allocator, entityId).data;
}
public T2 ReadT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT2(int index) => this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT3(int index) {
    if (this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T3 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs3->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T3 GetT3(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
    return ref this.regs3->components.GetPtr(this.allocator, entityId).data;
}
public T3 ReadT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT3(int index) => this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT4(int index) {
    if (this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T4 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs4->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T4 GetT4(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
    return ref this.regs4->components.GetPtr(this.allocator, entityId).data;
}
public T4 ReadT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT4(int index) => this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT5(int index) {
    if (this.regs5->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T5 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs5->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs5->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T5 GetT5(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs5->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 1, });
    return ref this.regs5->components.GetPtr(this.allocator, entityId).data;
}
public T5 ReadT5(int index) => this.regs5->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT5(int index) => this.regs5->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT5(int index) => this.regs5->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0,T1,T2,T3,T4,T5,T6> : IFilterBag  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T1>* regs1;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops1;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T2>* regs2;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops2;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T3>* regs3;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops3;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T4>* regs4;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops4;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T5>* regs5;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops5;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T6>* regs6;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops6;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);
ref var regs1 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T1>>(allRegs.items[in memAllocator, AllComponentTypes<T1>.typeId]);
regs1.Merge(ref memAllocator);
ref var regs2 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T2>>(allRegs.items[in memAllocator, AllComponentTypes<T2>.typeId]);
regs2.Merge(ref memAllocator);
ref var regs3 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T3>>(allRegs.items[in memAllocator, AllComponentTypes<T3>.typeId]);
regs3.Merge(ref memAllocator);
ref var regs4 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T4>>(allRegs.items[in memAllocator, AllComponentTypes<T4>.typeId]);
regs4.Merge(ref memAllocator);
ref var regs5 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T5>>(allRegs.items[in memAllocator, AllComponentTypes<T5>.typeId]);
regs5.Merge(ref memAllocator);
ref var regs6 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T6>>(allRegs.items[in memAllocator, AllComponentTypes<T6>.typeId]);
regs6.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);
if (regs1.components.Has(ref memAllocator, entityId).state == 0) regs1.components.Set(ref memAllocator, entityId, default);
if (regs2.components.Has(ref memAllocator, entityId).state == 0) regs2.components.Set(ref memAllocator, entityId, default);
if (regs3.components.Has(ref memAllocator, entityId).state == 0) regs3.components.Set(ref memAllocator, entityId, default);
if (regs4.components.Has(ref memAllocator, entityId).state == 0) regs4.components.Set(ref memAllocator, entityId, default);
if (regs5.components.Has(ref memAllocator, entityId).state == 0) regs5.components.Set(ref memAllocator, entityId, default);
if (regs6.components.Has(ref memAllocator, entityId).state == 0) regs6.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);this.regs1 = (UnmanagedComponentsStorage.Item<T1>*)UnsafeUtility.AddressOf(ref regs1);
this.ops1 = new Ops(this.Length);this.regs2 = (UnmanagedComponentsStorage.Item<T2>*)UnsafeUtility.AddressOf(ref regs2);
this.ops2 = new Ops(this.Length);this.regs3 = (UnmanagedComponentsStorage.Item<T3>*)UnsafeUtility.AddressOf(ref regs3);
this.ops3 = new Ops(this.Length);this.regs4 = (UnmanagedComponentsStorage.Item<T4>*)UnsafeUtility.AddressOf(ref regs4);
this.ops4 = new Ops(this.Length);this.regs5 = (UnmanagedComponentsStorage.Item<T5>*)UnsafeUtility.AddressOf(ref regs5);
this.ops5 = new Ops(this.Length);this.regs6 = (UnmanagedComponentsStorage.Item<T6>*)UnsafeUtility.AddressOf(ref regs6);
this.ops6 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops1.Length; ++i) {
    if (this.ops1.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T1>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1, this.ReadT1(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops2.Length; ++i) {
    if (this.ops2.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T2>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2, this.ReadT2(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops3.Length; ++i) {
    if (this.ops3.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T3>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3, this.ReadT3(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops4.Length; ++i) {
    if (this.ops4.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T4>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4, this.ReadT4(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops5.Length; ++i) {
    if (this.ops5.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T5>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs5, this.ReadT5(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs5);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops6.Length; ++i) {
    if (this.ops6.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T6>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs6, this.ReadT6(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs6);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  
this.ops1.Dispose();  
this.ops2.Dispose();  
this.ops3.Dispose();  
this.ops4.Dispose();  
this.ops5.Dispose();  
this.ops6.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT1(int index) {
    if (this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T1 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs1->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T1 GetT1(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
    return ref this.regs1->components.GetPtr(this.allocator, entityId).data;
}
public T1 ReadT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT1(int index) => this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT2(int index) {
    if (this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T2 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs2->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T2 GetT2(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
    return ref this.regs2->components.GetPtr(this.allocator, entityId).data;
}
public T2 ReadT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT2(int index) => this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT3(int index) {
    if (this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T3 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs3->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T3 GetT3(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
    return ref this.regs3->components.GetPtr(this.allocator, entityId).data;
}
public T3 ReadT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT3(int index) => this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT4(int index) {
    if (this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T4 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs4->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T4 GetT4(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
    return ref this.regs4->components.GetPtr(this.allocator, entityId).data;
}
public T4 ReadT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT4(int index) => this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT5(int index) {
    if (this.regs5->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T5 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs5->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs5->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T5 GetT5(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs5->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 1, });
    return ref this.regs5->components.GetPtr(this.allocator, entityId).data;
}
public T5 ReadT5(int index) => this.regs5->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT5(int index) => this.regs5->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT5(int index) => this.regs5->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT6(int index) {
    if (this.regs6->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T6 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs6->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs6->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T6 GetT6(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs6->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 1, });
    return ref this.regs6->components.GetPtr(this.allocator, entityId).data;
}
public T6 ReadT6(int index) => this.regs6->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT6(int index) => this.regs6->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT6(int index) => this.regs6->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7> : IFilterBag  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T1>* regs1;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops1;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T2>* regs2;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops2;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T3>* regs3;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops3;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T4>* regs4;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops4;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T5>* regs5;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops5;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T6>* regs6;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops6;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T7>* regs7;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops7;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);
ref var regs1 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T1>>(allRegs.items[in memAllocator, AllComponentTypes<T1>.typeId]);
regs1.Merge(ref memAllocator);
ref var regs2 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T2>>(allRegs.items[in memAllocator, AllComponentTypes<T2>.typeId]);
regs2.Merge(ref memAllocator);
ref var regs3 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T3>>(allRegs.items[in memAllocator, AllComponentTypes<T3>.typeId]);
regs3.Merge(ref memAllocator);
ref var regs4 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T4>>(allRegs.items[in memAllocator, AllComponentTypes<T4>.typeId]);
regs4.Merge(ref memAllocator);
ref var regs5 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T5>>(allRegs.items[in memAllocator, AllComponentTypes<T5>.typeId]);
regs5.Merge(ref memAllocator);
ref var regs6 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T6>>(allRegs.items[in memAllocator, AllComponentTypes<T6>.typeId]);
regs6.Merge(ref memAllocator);
ref var regs7 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T7>>(allRegs.items[in memAllocator, AllComponentTypes<T7>.typeId]);
regs7.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);
if (regs1.components.Has(ref memAllocator, entityId).state == 0) regs1.components.Set(ref memAllocator, entityId, default);
if (regs2.components.Has(ref memAllocator, entityId).state == 0) regs2.components.Set(ref memAllocator, entityId, default);
if (regs3.components.Has(ref memAllocator, entityId).state == 0) regs3.components.Set(ref memAllocator, entityId, default);
if (regs4.components.Has(ref memAllocator, entityId).state == 0) regs4.components.Set(ref memAllocator, entityId, default);
if (regs5.components.Has(ref memAllocator, entityId).state == 0) regs5.components.Set(ref memAllocator, entityId, default);
if (regs6.components.Has(ref memAllocator, entityId).state == 0) regs6.components.Set(ref memAllocator, entityId, default);
if (regs7.components.Has(ref memAllocator, entityId).state == 0) regs7.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);this.regs1 = (UnmanagedComponentsStorage.Item<T1>*)UnsafeUtility.AddressOf(ref regs1);
this.ops1 = new Ops(this.Length);this.regs2 = (UnmanagedComponentsStorage.Item<T2>*)UnsafeUtility.AddressOf(ref regs2);
this.ops2 = new Ops(this.Length);this.regs3 = (UnmanagedComponentsStorage.Item<T3>*)UnsafeUtility.AddressOf(ref regs3);
this.ops3 = new Ops(this.Length);this.regs4 = (UnmanagedComponentsStorage.Item<T4>*)UnsafeUtility.AddressOf(ref regs4);
this.ops4 = new Ops(this.Length);this.regs5 = (UnmanagedComponentsStorage.Item<T5>*)UnsafeUtility.AddressOf(ref regs5);
this.ops5 = new Ops(this.Length);this.regs6 = (UnmanagedComponentsStorage.Item<T6>*)UnsafeUtility.AddressOf(ref regs6);
this.ops6 = new Ops(this.Length);this.regs7 = (UnmanagedComponentsStorage.Item<T7>*)UnsafeUtility.AddressOf(ref regs7);
this.ops7 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops1.Length; ++i) {
    if (this.ops1.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T1>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1, this.ReadT1(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops2.Length; ++i) {
    if (this.ops2.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T2>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2, this.ReadT2(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops3.Length; ++i) {
    if (this.ops3.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T3>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3, this.ReadT3(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops4.Length; ++i) {
    if (this.ops4.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T4>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4, this.ReadT4(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops5.Length; ++i) {
    if (this.ops5.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T5>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs5, this.ReadT5(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs5);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops6.Length; ++i) {
    if (this.ops6.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T6>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs6, this.ReadT6(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs6);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops7.Length; ++i) {
    if (this.ops7.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T7>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs7, this.ReadT7(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs7);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  
this.ops1.Dispose();  
this.ops2.Dispose();  
this.ops3.Dispose();  
this.ops4.Dispose();  
this.ops5.Dispose();  
this.ops6.Dispose();  
this.ops7.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT1(int index) {
    if (this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T1 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs1->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T1 GetT1(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
    return ref this.regs1->components.GetPtr(this.allocator, entityId).data;
}
public T1 ReadT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT1(int index) => this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT2(int index) {
    if (this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T2 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs2->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T2 GetT2(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
    return ref this.regs2->components.GetPtr(this.allocator, entityId).data;
}
public T2 ReadT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT2(int index) => this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT3(int index) {
    if (this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T3 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs3->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T3 GetT3(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
    return ref this.regs3->components.GetPtr(this.allocator, entityId).data;
}
public T3 ReadT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT3(int index) => this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT4(int index) {
    if (this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T4 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs4->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T4 GetT4(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
    return ref this.regs4->components.GetPtr(this.allocator, entityId).data;
}
public T4 ReadT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT4(int index) => this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT5(int index) {
    if (this.regs5->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T5 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs5->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs5->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T5 GetT5(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs5->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 1, });
    return ref this.regs5->components.GetPtr(this.allocator, entityId).data;
}
public T5 ReadT5(int index) => this.regs5->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT5(int index) => this.regs5->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT5(int index) => this.regs5->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT6(int index) {
    if (this.regs6->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T6 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs6->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs6->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T6 GetT6(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs6->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 1, });
    return ref this.regs6->components.GetPtr(this.allocator, entityId).data;
}
public T6 ReadT6(int index) => this.regs6->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT6(int index) => this.regs6->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT6(int index) => this.regs6->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT7(int index) {
    if (this.regs7->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops7.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T7>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T7 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs7->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs7->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops7.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T7>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T7 GetT7(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs7->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops7.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T7>.burstTypeId.Data, code = 1, });
    return ref this.regs7->components.GetPtr(this.allocator, entityId).data;
}
public T7 ReadT7(int index) => this.regs7->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT7(int index) => this.regs7->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT7(int index) => this.regs7->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}

#if ECS_COMPILE_IL2CPP_OPTIONS
[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
public unsafe struct FilterBag<T0,T1,T2,T3,T4,T5,T6,T7,T8> : IFilterBag  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    public readonly int Length;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T0>* regs0;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops0;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T1>* regs1;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops1;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T2>* regs2;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops2;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T3>* regs3;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops3;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T4>* regs4;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops4;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T5>* regs5;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops5;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T6>* regs6;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops6;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T7>* regs7;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops7;
[NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private UnmanagedComponentsStorage.Item<T8>* regs8;
[Unity.Collections.NativeDisableParallelForRestriction] private Ops ops8;

    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private ME.ECS.Collections.V3.MemArrayAllocator<Entity> entities;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeList<int> indexes;
    [Unity.Collections.NativeDisableParallelForRestriction][Unity.Collections.ReadOnlyAttribute] private Unity.Collections.NativeArray<int> entityToIndex;
    [NativeDisableUnsafePtrRestriction][Unity.Collections.NativeDisableParallelForRestriction] private ME.ECS.Collections.V3.MemoryAllocator* allocator;
    [Unity.Collections.NativeDisableParallelForRestriction] private Ops entityOps;
    public int Count => this.Length;
    public Tick tick;
    private EntityVersions entityVersions;
    public FilterBag(Filter filter, Unity.Collections.Allocator allocator) {
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FilterBag::Create()");
        #endif
        this = default;
        var mode = Unity.Collections.NativeLeakDetection.Mode;
        Unity.Collections.NativeLeakDetection.Mode = Unity.Collections.NativeLeakDetectionMode.Disabled;
        var world = filter.world;
        this.tick = world.GetCurrentTick();
        this.entityVersions = world.currentState.storage.versions;
        this.entities = world.currentState.storage.cache;
        var filterArr = filter.ToList(allocator, out this.entityToIndex);
        this.indexes = filterArr;
        this.Length = filterArr.Length;
        if (this.Length > 0) {
            this.entityOps = new Ops(this.Length);
            ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
            ref var memAllocator = ref world.currentState.allocator;
            ref var regs0 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T0>>(allRegs.items[in memAllocator, AllComponentTypes<T0>.typeId]);
regs0.Merge(ref memAllocator);
ref var regs1 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T1>>(allRegs.items[in memAllocator, AllComponentTypes<T1>.typeId]);
regs1.Merge(ref memAllocator);
ref var regs2 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T2>>(allRegs.items[in memAllocator, AllComponentTypes<T2>.typeId]);
regs2.Merge(ref memAllocator);
ref var regs3 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T3>>(allRegs.items[in memAllocator, AllComponentTypes<T3>.typeId]);
regs3.Merge(ref memAllocator);
ref var regs4 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T4>>(allRegs.items[in memAllocator, AllComponentTypes<T4>.typeId]);
regs4.Merge(ref memAllocator);
ref var regs5 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T5>>(allRegs.items[in memAllocator, AllComponentTypes<T5>.typeId]);
regs5.Merge(ref memAllocator);
ref var regs6 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T6>>(allRegs.items[in memAllocator, AllComponentTypes<T6>.typeId]);
regs6.Merge(ref memAllocator);
ref var regs7 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T7>>(allRegs.items[in memAllocator, AllComponentTypes<T7>.typeId]);
regs7.Merge(ref memAllocator);
ref var regs8 = ref memAllocator.Ref<UnmanagedComponentsStorage.Item<T8>>(allRegs.items[in memAllocator, AllComponentTypes<T8>.typeId]);
regs8.Merge(ref memAllocator);

            for (int i = 0; i < this.indexes.Length; ++i) {
                var entityId = this.indexes[i];
                if (regs0.components.Has(ref memAllocator, entityId).state == 0) regs0.components.Set(ref memAllocator, entityId, default);
if (regs1.components.Has(ref memAllocator, entityId).state == 0) regs1.components.Set(ref memAllocator, entityId, default);
if (regs2.components.Has(ref memAllocator, entityId).state == 0) regs2.components.Set(ref memAllocator, entityId, default);
if (regs3.components.Has(ref memAllocator, entityId).state == 0) regs3.components.Set(ref memAllocator, entityId, default);
if (regs4.components.Has(ref memAllocator, entityId).state == 0) regs4.components.Set(ref memAllocator, entityId, default);
if (regs5.components.Has(ref memAllocator, entityId).state == 0) regs5.components.Set(ref memAllocator, entityId, default);
if (regs6.components.Has(ref memAllocator, entityId).state == 0) regs6.components.Set(ref memAllocator, entityId, default);
if (regs7.components.Has(ref memAllocator, entityId).state == 0) regs7.components.Set(ref memAllocator, entityId, default);
if (regs8.components.Has(ref memAllocator, entityId).state == 0) regs8.components.Set(ref memAllocator, entityId, default);

            }
            this.regs0 = (UnmanagedComponentsStorage.Item<T0>*)UnsafeUtility.AddressOf(ref regs0);
this.ops0 = new Ops(this.Length);this.regs1 = (UnmanagedComponentsStorage.Item<T1>*)UnsafeUtility.AddressOf(ref regs1);
this.ops1 = new Ops(this.Length);this.regs2 = (UnmanagedComponentsStorage.Item<T2>*)UnsafeUtility.AddressOf(ref regs2);
this.ops2 = new Ops(this.Length);this.regs3 = (UnmanagedComponentsStorage.Item<T3>*)UnsafeUtility.AddressOf(ref regs3);
this.ops3 = new Ops(this.Length);this.regs4 = (UnmanagedComponentsStorage.Item<T4>*)UnsafeUtility.AddressOf(ref regs4);
this.ops4 = new Ops(this.Length);this.regs5 = (UnmanagedComponentsStorage.Item<T5>*)UnsafeUtility.AddressOf(ref regs5);
this.ops5 = new Ops(this.Length);this.regs6 = (UnmanagedComponentsStorage.Item<T6>*)UnsafeUtility.AddressOf(ref regs6);
this.ops6 = new Ops(this.Length);this.regs7 = (UnmanagedComponentsStorage.Item<T7>*)UnsafeUtility.AddressOf(ref regs7);
this.ops7 = new Ops(this.Length);this.regs8 = (UnmanagedComponentsStorage.Item<T8>*)UnsafeUtility.AddressOf(ref regs8);
this.ops8 = new Ops(this.Length);
        }
        fixed (MemoryAllocator* f = &world.currentState.allocator) {
            this.allocator = f;
        }
        Unity.Collections.NativeLeakDetection.Mode = mode;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    public void BeginForEachIndex(int chunkIndex) => this.entityOps.BeginForEachIndex(chunkIndex);
    public void EndForEachIndex() => this.entityOps.EndForEachIndex();
    public void Push() {
        if (this.Length == 0) return;
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("Push");
        #endif
        var world = Worlds.currentWorld;
        ref var allRegs = ref world.currentState.structComponents.unmanagedComponentsStorage;
        ref var memAllocator = ref world.currentState.allocator;
        for (int i = 0; i < this.ops0.Length; ++i) {
    if (this.ops0.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T0>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0, this.ReadT0(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs0);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops1.Length; ++i) {
    if (this.ops1.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T1>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1, this.ReadT1(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs1);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops2.Length; ++i) {
    if (this.ops2.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T2>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2, this.ReadT2(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs2);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops3.Length; ++i) {
    if (this.ops3.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T3>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3, this.ReadT3(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs3);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops4.Length; ++i) {
    if (this.ops4.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T4>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4, this.ReadT4(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs4);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops5.Length; ++i) {
    if (this.ops5.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T5>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs5, this.ReadT5(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs5);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops6.Length; ++i) {
    if (this.ops6.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T6>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs6, this.ReadT6(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs6);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops7.Length; ++i) {
    if (this.ops7.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T7>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs7, this.ReadT7(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs7);
                }
            }
        }
    }
}
for (int i = 0; i < this.ops8.Length; ++i) {
    if (this.ops8.Read(i, out var op) == true) {
        var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
        if (op.code == 2 && op.componentId == -1) {
            world.RemoveEntity(in entity);
        } else {
            if (op.componentId == AllComponentTypes<T8>.typeId) {
                if (op.code == 1) {
                    DataUnmanagedBufferUtils.PushSet_INTERNAL(world, in entity, ref memAllocator, ref *this.regs8, this.ReadT8(op.entityIndex));
                } else if (op.code == 2) {
                    DataUnmanagedBufferUtils.PushRemove_INTERNAL(world, in entity, ref memAllocator, ref *this.regs8);
                }
            }
        }
    }
}

        for (int i = 0; i < this.entityOps.Length; ++i) {
            if (this.entityOps.Read(i, out var op) == true) {
                var entity = this.entities[in memAllocator, this.indexes[op.entityIndex]];
                if (op.code == 2 && op.componentId == -1) {
                    world.RemoveEntity(in entity);
                }
            }
        }
        this.Dispose();
        #if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
        #endif
    }
    
    public int GetIndexByEntityId(int id) => this.entityToIndex[id];
    public void DestroyEntity(int index) => this.entityOps.Write(new Op() { entityIndex = index, componentId = -1, code = 2, });
    public int GetEntityId(int index) => this.indexes[index];
    public ref readonly Entity GetEntity(int index) => ref this.entities[*this.allocator, this.indexes[index]];
    public void Revert() => this.Dispose();
    private void Dispose() {
        if (this.Length > 0) {
            this.entityOps.Dispose();
            this.ops0.Dispose();  
this.ops1.Dispose();  
this.ops2.Dispose();  
this.ops3.Dispose();  
this.ops4.Dispose();  
this.ops5.Dispose();  
this.ops6.Dispose();  
this.ops7.Dispose();  
this.ops8.Dispose();  

        }
        this.indexes.Dispose();
        this.entityToIndex.Dispose();
        this = default;
    }
    #region API
    public void RemoveT0(int index) {
    if (this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T0 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs0->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T0 GetT0(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs0->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops0.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T0>.burstTypeId.Data, code = 1, });
    return ref this.regs0->components.GetPtr(this.allocator, entityId).data;
}
public T0 ReadT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT0(int index) => this.regs0->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT0(int index) => this.regs0->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT1(int index) {
    if (this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T1 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs1->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T1 GetT1(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs1->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops1.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T1>.burstTypeId.Data, code = 1, });
    return ref this.regs1->components.GetPtr(this.allocator, entityId).data;
}
public T1 ReadT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT1(int index) => this.regs1->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT1(int index) => this.regs1->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT2(int index) {
    if (this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T2 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs2->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T2 GetT2(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs2->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops2.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T2>.burstTypeId.Data, code = 1, });
    return ref this.regs2->components.GetPtr(this.allocator, entityId).data;
}
public T2 ReadT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT2(int index) => this.regs2->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT2(int index) => this.regs2->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT3(int index) {
    if (this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T3 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs3->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T3 GetT3(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs3->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops3.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T3>.burstTypeId.Data, code = 1, });
    return ref this.regs3->components.GetPtr(this.allocator, entityId).data;
}
public T3 ReadT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT3(int index) => this.regs3->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT3(int index) => this.regs3->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT4(int index) {
    if (this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T4 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs4->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T4 GetT4(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs4->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops4.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T4>.burstTypeId.Data, code = 1, });
    return ref this.regs4->components.GetPtr(this.allocator, entityId).data;
}
public T4 ReadT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT4(int index) => this.regs4->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT4(int index) => this.regs4->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT5(int index) {
    if (this.regs5->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T5 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs5->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs5->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T5 GetT5(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs5->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops5.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T5>.burstTypeId.Data, code = 1, });
    return ref this.regs5->components.GetPtr(this.allocator, entityId).data;
}
public T5 ReadT5(int index) => this.regs5->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT5(int index) => this.regs5->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT5(int index) => this.regs5->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT6(int index) {
    if (this.regs6->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T6 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs6->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs6->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T6 GetT6(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs6->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops6.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T6>.burstTypeId.Data, code = 1, });
    return ref this.regs6->components.GetPtr(this.allocator, entityId).data;
}
public T6 ReadT6(int index) => this.regs6->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT6(int index) => this.regs6->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT6(int index) => this.regs6->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT7(int index) {
    if (this.regs7->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops7.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T7>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T7 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs7->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs7->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops7.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T7>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T7 GetT7(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs7->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops7.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T7>.burstTypeId.Data, code = 1, });
    return ref this.regs7->components.GetPtr(this.allocator, entityId).data;
}
public T7 ReadT7(int index) => this.regs7->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT7(int index) => this.regs7->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT7(int index) => this.regs7->components.ReadPtr(this.allocator, this.indexes[index]).version;
public void RemoveT8(int index) {
    if (this.regs8->components.HasPtr(this.allocator, this.indexes[index]).state > 0) {
        this.ops8.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T8>.burstTypeId.Data, code = 2, });
    }
}
public void Set(int index, in T8 component) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs8->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    ref var componentData = ref this.regs8->components.GetPtr(this.allocator, this.indexes[index]);
    if (DataBlittableBurstBufferUtils.NeedToPush(in *this.allocator, this.tick, ref this.entityVersions, entityId, ref componentData, in component) == true) {
        this.ops8.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T8>.burstTypeId.Data, code = 1, });
        componentData.data = component;
    }
}
public ref T8 GetT8(int index) {
    var entityId = this.indexes[index];
    #if WORLD_STATE_CHECK
    if (this.regs8->components.HasDataPtr(this.allocator, entityId) == false) {
        E.THROW_FILTER_BAG_COMPONENT_NOT_EXISTS();
    }
    #endif
    this.ops8.Write(new Op() { entityIndex = index, componentId = AllComponentTypes<T8>.burstTypeId.Data, code = 1, });
    return ref this.regs8->components.GetPtr(this.allocator, entityId).data;
}
public T8 ReadT8(int index) => this.regs8->components.ReadPtr(this.allocator, this.indexes[index]).data;
public bool HasT8(int index) => this.regs8->components.HasPtr(this.allocator, this.indexes[index]).state > 0;
public long GetVersionT8(int index) => this.regs8->components.ReadPtr(this.allocator, this.indexes[index]).version;

    #endregion
}



    }
    
    public static class FiltersForEachExtensions {

        internal static class JobsW<T0>  where T0:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDW<T0>> ForEach<T0>(this in Filter filter, ME.ECS.Filters.FDW<T0> onEach)  where T0:unmanaged,IComponentBase {
    
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

internal static class JobsR<T0>  where T0:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDR<T0>> ForEach<T0>(this in Filter filter, ME.ECS.Filters.FDR<T0> onEach)  where T0:unmanaged,IComponentBase {
    
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

internal static class JobsWW<T0,T1>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWW<T0,T1>> ForEach<T0,T1>(this in Filter filter, ME.ECS.Filters.FDWW<T0,T1> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase {
    
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

internal static class JobsRW<T0,T1>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRW<T0,T1>> ForEach<T0,T1>(this in Filter filter, ME.ECS.Filters.FDRW<T0,T1> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase {
    
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

internal static class JobsRR<T0,T1>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRR<T0,T1>> ForEach<T0,T1>(this in Filter filter, ME.ECS.Filters.FDRR<T0,T1> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase {
    
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

internal static class JobsWWW<T0,T1,T2>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWW<T0,T1,T2>> ForEach<T0,T1,T2>(this in Filter filter, ME.ECS.Filters.FDWWW<T0,T1,T2> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    
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

internal static class JobsRWW<T0,T1,T2>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWW<T0,T1,T2>> ForEach<T0,T1,T2>(this in Filter filter, ME.ECS.Filters.FDRWW<T0,T1,T2> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    
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

internal static class JobsRRW<T0,T1,T2>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRW<T0,T1,T2>> ForEach<T0,T1,T2>(this in Filter filter, ME.ECS.Filters.FDRRW<T0,T1,T2> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    
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

internal static class JobsRRR<T0,T1,T2>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRR<T0,T1,T2>> ForEach<T0,T1,T2>(this in Filter filter, ME.ECS.Filters.FDRRR<T0,T1,T2> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase {
    
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

internal static class JobsWWWW<T0,T1,T2,T3>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWW<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDWWWW<T0,T1,T2,T3> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

internal static class JobsRWWW<T0,T1,T2,T3>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWW<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDRWWW<T0,T1,T2,T3> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

internal static class JobsRRWW<T0,T1,T2,T3>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWW<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDRRWW<T0,T1,T2,T3> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

internal static class JobsRRRW<T0,T1,T2,T3>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRW<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDRRRW<T0,T1,T2,T3> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

internal static class JobsRRRR<T0,T1,T2,T3>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRR<T0,T1,T2,T3>> ForEach<T0,T1,T2,T3>(this in Filter filter, ME.ECS.Filters.FDRRRR<T0,T1,T2,T3> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase {
    
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

internal static class JobsWWWWW<T0,T1,T2,T3,T4>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDWWWWW<T0,T1,T2,T3,T4> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

internal static class JobsRWWWW<T0,T1,T2,T3,T4>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRWWWW<T0,T1,T2,T3,T4> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

internal static class JobsRRWWW<T0,T1,T2,T3,T4>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRRWWW<T0,T1,T2,T3,T4> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

internal static class JobsRRRWW<T0,T1,T2,T3,T4>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRRRWW<T0,T1,T2,T3,T4> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRW<T0,T1,T2,T3,T4>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRW<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRRRRW<T0,T1,T2,T3,T4> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRR<T0,T1,T2,T3,T4>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRR<T0,T1,T2,T3,T4>> ForEach<T0,T1,T2,T3,T4>(this in Filter filter, ME.ECS.Filters.FDRRRRR<T0,T1,T2,T3,T4> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase {
    
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

internal static class JobsWWWWWW<T0,T1,T2,T3,T4,T5>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDWWWWWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

internal static class JobsRWWWWW<T0,T1,T2,T3,T4,T5>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRWWWWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

internal static class JobsRRWWWW<T0,T1,T2,T3,T4,T5>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRWWWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

internal static class JobsRRRWWW<T0,T1,T2,T3,T4,T5>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRRWWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRWW<T0,T1,T2,T3,T4,T5>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRRRWW<T0,T1,T2,T3,T4,T5> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRW<T0,T1,T2,T3,T4,T5>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRW<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRRRRW<T0,T1,T2,T3,T4,T5> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRR<T0,T1,T2,T3,T4,T5>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRR<T0,T1,T2,T3,T4,T5>> ForEach<T0,T1,T2,T3,T4,T5>(this in Filter filter, ME.ECS.Filters.FDRRRRRR<T0,T1,T2,T3,T4,T5> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase {
    
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

internal static class JobsWWWWWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDWWWWWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

internal static class JobsRWWWWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRWWWWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

internal static class JobsRRWWWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRWWWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

internal static class JobsRRRWWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRWWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRWWW<T0,T1,T2,T3,T4,T5,T6>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRRWWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRWW<T0,T1,T2,T3,T4,T5,T6>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRRRWW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRW<T0,T1,T2,T3,T4,T5,T6>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRRRRW<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRR<T0,T1,T2,T3,T4,T5,T6>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6>> ForEach<T0,T1,T2,T3,T4,T5,T6>(this in Filter filter, ME.ECS.Filters.FDRRRRRRR<T0,T1,T2,T3,T4,T5,T6> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase {
    
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

internal static class JobsWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase {
    
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

internal static class JobsWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDWWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRWWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRWWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRWWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRWWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRWWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRRWWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRWW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRRW<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

internal static class JobsRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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

public static unsafe ForEachUtils.ForEachTask<ME.ECS.Filters.FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8>> ForEach<T0,T1,T2,T3,T4,T5,T6,T7,T8>(this in Filter filter, ME.ECS.Filters.FDRRRRRRRRR<T0,T1,T2,T3,T4,T5,T6,T7,T8> onEach)  where T0:unmanaged,IComponentBase where T1:unmanaged,IComponentBase where T2:unmanaged,IComponentBase where T3:unmanaged,IComponentBase where T4:unmanaged,IComponentBase where T5:unmanaged,IComponentBase where T6:unmanaged,IComponentBase where T7:unmanaged,IComponentBase where T8:unmanaged,IComponentBase {
    
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