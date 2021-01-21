using System.Collections.Generic;
using System.Linq;

namespace ME.ECS {

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolStates<T> where T : State, new() {

		private static PoolInternalBase pool = new PoolInternalBase(() => new T(), null);

		public static T Spawn() {
		    
			return (T)PoolStates<T>.pool.Spawn();
		    
		}

		public static void Recycle(ref T instance) {
		    
			PoolStates<T>.pool.Recycle(instance);
			instance = null;

		}

		public static void Recycle(T instance) {
		    
			PoolStates<T>.pool.Recycle(instance);
		    
		}

	}

}
