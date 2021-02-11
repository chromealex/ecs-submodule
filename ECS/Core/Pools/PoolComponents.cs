using System.Collections.Generic;

namespace ME.ECS {

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
    public static class PoolRegistries {

	    private static Dictionary<long, PoolInternalBase> pool = new Dictionary<long, PoolInternalBase>();
	    
	    public static StructRegistryBase Spawn<T>() where T : struct, IStructComponent {

		    var key = WorldUtilities.GetAllComponentTypeId<T>();
		    var obj = (StructComponents<T>)PoolRegistries.Spawn_INTERNAL(key, out var pool);
		    if (obj != null) return obj;

		    return PoolInternalBase.Create<StructComponents<T>>(pool);

	    }

	    public static StructRegistryBase SpawnCopyable<T>() where T : struct, IStructComponent, IStructCopyable<T> {

		    var key = WorldUtilities.GetAllComponentTypeId<T>();
		    var obj = (StructComponentsCopyable<T>)PoolRegistries.Spawn_INTERNAL(key, out var pool);
		    if (obj != null) return obj;

		    return PoolInternalBase.Create<StructComponentsCopyable<T>>(pool);

	    }

	    private static object Spawn_INTERNAL(int key, out PoolInternalBase pool) {
		    
		    if (PoolRegistries.pool.TryGetValue(key, out pool) == true) {

			    var obj = pool.Spawn();
			    if (obj != null) return obj;

		    } else {
                
			    pool = new PoolInternalBase(typeof(StructRegistryBase), null, null);
			    var obj = pool.Spawn();
			    PoolRegistries.pool.Add(key, pool);
			    if (obj != null) return obj;

		    }

		    return null;

	    }

	    private static void Recycle_INTERNAL(int key, object system) {
		    
		    PoolInternalBase pool;
		    if (PoolRegistries.pool.TryGetValue(key, out pool) == true) {

			    pool.Recycle(system);
                
		    } else {
                
			    pool = new PoolInternalBase(typeof(StructRegistryBase), null, null);
			    pool.Recycle(system);
			    PoolRegistries.pool.Add(key, pool);
                
		    }
		    
	    }

	    public static void Recycle(StructRegistryBase system) {

		    if (system == null) return;
		    
		    var key = system.GetAllTypeBit();
		    PoolRegistries.Recycle_INTERNAL(key, system);
		    
	    }

    }

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	[System.ObsoleteAttribute("Managed components are deprecated, use struct components or struct copyable components instead.")]
    public static class PoolComponents {

	    private static Dictionary<int, PoolInternalBase> pool = new Dictionary<int, PoolInternalBase>();

	    public static T Spawn<T>() where T : class, IComponentBase, new() {

		    var key = WorldUtilities.GetKey<T>();
		    var obj = (T)PoolComponents.Spawn_INTERNAL(typeof(T), key, out var pool);
		    if (obj != null) return obj;

		    return PoolInternalBase.Create<T>(pool);

	    }
	    
	    public static object Spawn(System.Type type) {

		    var key = WorldUtilities.GetKey(type);
		    var instance = PoolComponents.Spawn_INTERNAL(type, key, out var pool);
		    if (instance == null) {

			    instance = (IComponent)System.Activator.CreateInstance(type);
			    PoolInternalBase.CallOnSpawn(instance, pool);

		    }

		    return instance;

	    }

	    private static object Spawn_INTERNAL(System.Type type, int key, out PoolInternalBase pool) {
		    
		    if (PoolComponents.pool.TryGetValue(key, out pool) == true) {

			    var obj = pool.Spawn();
			    if (obj != null) return obj;

		    } else {
                
			    pool = new PoolInternalBase(type, null, null);
			    var obj = pool.Spawn();
			    PoolComponents.pool.Add(key, pool);
			    if (obj != null) return obj;

		    }

		    return null;

	    }

	    public static void Recycle<T>(ref T system) where T : class, IComponentBase {

		    PoolComponents.Recycle(system);
		    system = null;

	    }

	    private static void Recycle_INTERNAL(System.Type type, int key, object system) {
		    
		    PoolInternalBase pool;
		    if (PoolComponents.pool.TryGetValue(key, out pool) == true) {

			    pool.Recycle(system);
                
		    } else {
                
			    pool = new PoolInternalBase(type, null, null);
			    pool.Recycle(system);
			    PoolComponents.pool.Add(key, pool);
                
		    }
		    
	    }

	    public static void Recycle<T>(T system) where T : class, IComponentBase {

		    var key = WorldUtilities.GetKey<T>();
		    PoolComponents.Recycle_INTERNAL(typeof(T), key, system);
		    
	    }

	    public static void Recycle<T>(T system, System.Type type) where T : class, IComponentBase {

		    var key = WorldUtilities.GetKey(type);
		    PoolComponents.Recycle_INTERNAL(type, key, system);

	    }

	    public static void Recycle<TComponent>(ref TComponent[] list) where TComponent : class, IComponentBase {

		    for (int i = 0; i < list.Length; ++i) {
			    
			    PoolComponents.Recycle(list[i]);
			    
		    }
		    PoolArray<TComponent>.Recycle(ref list);

	    }

	    public static void Recycle<TComponent>(ME.ECS.Collections.ListCopyable<TComponent> list) where TComponent : class, IComponentBase {

		    for (int i = 0; i < list.Count; ++i) {
			    
			    PoolComponents.Recycle(list[i], list[i].GetType());
			    
		    }
		    list.Clear();

	    }

	    public static void Recycle<TComponent>(IList<TComponent> list) where TComponent : class, IComponentBase {

		    for (int i = 0; i < list.Count; ++i) {
			    
			    PoolComponents.Recycle(list[i], list[i].GetType());
			    
		    }
		    list.Clear();

	    }

	    public static void Recycle<TComponent>(HashSet<TComponent> list) where TComponent : class, IComponentBase {

		    foreach (var item in list) {
			    
			    PoolComponents.Recycle(item);
			    
		    }
		    list.Clear();

	    }

    }

}
