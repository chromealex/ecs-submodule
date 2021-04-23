using System.Collections.Generic;

namespace ME.ECS {

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
    public static class PoolRegistries {

	    private static Dictionary<long, PoolInternalBase> pool = new Dictionary<long, PoolInternalBase>();
	    
	    public static StructRegistryBase Spawn<T>() where T : struct, IStructComponentBase {

		    var key = WorldUtilities.GetAllComponentTypeId<T>();
		    var obj = (StructComponents<T>)PoolRegistries.Spawn_INTERNAL(key, out var pool);
		    if (obj != null) return obj;

		    return PoolInternalBase.Create<StructComponents<T>>(pool);

	    }

	    public static StructRegistryBase SpawnCopyable<T>() where T : struct, IStructComponentBase, IStructCopyable<T> {

		    var key = WorldUtilities.GetAllComponentTypeId<T>();
		    var obj = (StructComponentsCopyable<T>)PoolRegistries.Spawn_INTERNAL(key, out var pool);
		    if (obj != null) return obj;

		    return PoolInternalBase.Create<StructComponentsCopyable<T>>(pool);

	    }

	    public static StructRegistryBase SpawnDisposable<T>() where T : struct, IStructComponentBase, IComponentDisposable {

		    var key = WorldUtilities.GetAllComponentTypeId<T>();
		    var obj = (StructComponentsDisposable<T>)PoolRegistries.Spawn_INTERNAL(key, out var pool);
		    if (obj != null) return obj;

		    return PoolInternalBase.Create<StructComponentsDisposable<T>>(pool);

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

}
