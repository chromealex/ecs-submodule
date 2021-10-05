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

		public static StructComponents<T> Spawn<T>() where T : struct, IStructComponentBase {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponents<T>(), null);
			
		}

		public static StructComponentsOneShot<T> SpawnOneShot<T>() where T : struct, IStructComponentBase, IComponentOneShot {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponentsOneShot<T>(), null);
			
		}

		public static StructComponentsCopyable<T> SpawnCopyable<T>() where T : struct, IStructComponentBase, IStructCopyable<T> {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponentsCopyable<T>(), null);
			
		}

		public static StructComponentsDisposable<T> SpawnDisposable<T>() where T : struct, IStructComponentBase, IComponentDisposable {

			return Pools.current.PoolSpawn(new Data(), (data) => new StructComponentsDisposable<T>(), null);
			
		}

		public static void Recycle(StructRegistryBase dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

		public static void Recycle(ref StructRegistryBase dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

    }

}
