using System.Collections.Generic;

namespace ME.ECS {

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
    public static class PoolRegistries {

		public struct Data {

		}

		public static StructComponents<T> Spawn<T>() where T : struct, IComponentBase {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponents<T>(), null);
			
		}

		public static StructComponentsOneShot<T> SpawnOneShot<T>() where T : struct, IComponentBase, IComponentOneShot {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponentsOneShot<T>(), null);
			
		}

		public static StructComponentsCopyable<T> SpawnCopyable<T>() where T : struct, IComponentBase, IStructCopyable<T> {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponentsCopyable<T>(), null);
			
		}

		public static StructComponentsDisposable<T> SpawnDisposable<T>() where T : struct, IComponentBase, IComponentDisposable {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponentsDisposable<T>(), null);
			
		}

		public static StructComponentsBlittable<T> SpawnBlittable<T>() where T : struct, IComponentBase {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponentsBlittable<T>(), null);
			
		}

		public static StructComponentsBlittableCopyable<T> SpawnBlittableCopyable<T>() where T : struct, IComponentBase, IStructCopyable<T> {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponentsBlittableCopyable<T>(), null);
			
		}

		public static void Recycle<T>(T dic) where T : StructRegistryBase {

			Pools.current.PoolRecycle(ref dic);
			
		}

    }

}
