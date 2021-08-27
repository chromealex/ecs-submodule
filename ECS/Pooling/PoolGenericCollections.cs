namespace ME.ECS {

	using System.Collections.Generic;

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolDictionary<TKey, TValue> {

        public struct Data {

	        public IEqualityComparer<TKey> customComparer;
	        public int capacity;
	        
        }

        public static Dictionary<TKey, TValue> Spawn(int capacity, IEqualityComparer<TKey> customComparer = null) {

            return Pools.current.PoolSpawn(new Data() {
	            capacity = capacity,
	            customComparer = customComparer,
            }, (data) => new Dictionary<TKey, TValue>(data.capacity, data.customComparer), x => x.Clear());
			
        }

        public static void Recycle(ref Dictionary<TKey, TValue> system) {

            Pools.current.PoolRecycle(ref system);
			
        }

        public static void Recycle(Dictionary<TKey, TValue> system) {

            Pools.current.PoolRecycle(ref system);
			
        }

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolStack<TValue> {

		public struct Data {

			public int capacity;
	        
		}

		public static Stack<TValue> Spawn(int capacity) {

			return Pools.current.PoolSpawn(new Data() {
				capacity = capacity,
			}, (data) => new Stack<TValue>(data.capacity), x => x.Clear());
			
		}

		public static void Recycle(ref Stack<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

		public static void Recycle(Stack<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

	}
	
	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolSortedSet<TValue> {

		public struct Data {

		}

		public static SortedSet<TValue> Spawn() {

			return Pools.current.PoolSpawn(new Data() {
			}, (data) => new SortedSet<TValue>(), x => x.Clear());
			
		}

		public static void Recycle(ref SortedSet<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

		public static void Recycle(SortedSet<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolHashSet<TValue> {

		public struct Data {

			public int capacity;

		}

		public static HashSet<TValue> Spawn(int capacity = 8) {

			return Pools.current.PoolSpawn(new Data() {
				capacity = capacity,
			}, (data) => new System.Collections.Generic.HashSet<TValue>(), x => x.Clear());
			
		}

		public static void Recycle(ref HashSet<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

		public static void Recycle(HashSet<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolQueue<TValue> {

		public struct Data {

			public int capacity;
	        
		}

		public static Queue<TValue> Spawn(int capacity) {

			return Pools.current.PoolSpawn(new Data() {
				capacity = capacity,
			}, (data) => new Queue<TValue>(data.capacity), x => x.Clear());
			
		}

		public static void Recycle(ref Queue<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

		public static void Recycle(Queue<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolList<TValue> {

		public struct Data {

			public int capacity;
	        
		}

		public static List<TValue> Spawn(int capacity) {

			return Pools.current.PoolSpawn(new Data() {
				capacity = capacity,
			}, (data) => new List<TValue>(data.capacity), x => x.Clear());
			
		}

		public static void Recycle(ref List<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

		public static void Recycle(List<TValue> system) {

			Pools.current.PoolRecycle(ref system);
			
		}

	}

}