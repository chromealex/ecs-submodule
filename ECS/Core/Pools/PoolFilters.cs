using System.Collections.Generic;

namespace ME.ECS {

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
    public static class PoolFilters {

		public struct Data {

		}

		public static T Spawn<T>() where T : class, new() {

			return Pools.current.PoolSpawn(new Data(), (data) => new T(), null);
			
		}

		public static void Recycle<T>(T system) where T : class {

			Pools.current.PoolRecycle(ref system);
			
		}

    }

}
