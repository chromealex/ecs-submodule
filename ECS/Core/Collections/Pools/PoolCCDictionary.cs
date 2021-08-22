using System.Collections.Generic;

namespace ME.ECS {

	using Collections;
	
	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolCCDictionary<TKey, TValue> {

		public struct Data {

			public int capacity;
			public int threadsCount;

		}

		public static CCDictionary<TKey, TValue> Spawn(int capacity, int threadsCount = 8) {

			return Pools.current.PoolSpawn(new Data() {
				capacity = capacity,
				threadsCount = threadsCount,
			}, (data) => new CCDictionary<TKey, TValue>(data.threadsCount, data.capacity), (x) => x.Clear());
			
		}

		public static void Recycle(ref CCDictionary<TKey, TValue> dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

	}

}
