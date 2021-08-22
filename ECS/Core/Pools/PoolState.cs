using System.Collections.Generic;
using System.Linq;

namespace ME.ECS {

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolStates<T> where T : State, new() {

		public struct Data {

		}

		public static T Spawn() {

			return Pools.current.PoolSpawn(new Data(), (data) => new T(), null);
			
		}

		public static void Recycle(ref T system) {

			Pools.current.PoolRecycle(ref system);
			
		}

		public static void Recycle(T system) {

			Pools.current.PoolRecycle(ref system);
			
		}

	}

}
