namespace ME.ECS {

	using System.Collections.Generic;
	
	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolStack<TValue> {

		private static PoolInternalBase pool = new PoolInternalBase(typeof(Stack<TValue>), () => new Stack<TValue>(), (x) => ((Stack<TValue>)x).Clear());

		public static Stack<TValue> Spawn(int capacity = 0) {

			return (Stack<TValue>)PoolStack<TValue>.pool.Spawn();
		    
		}

		public static void Recycle(ref Stack<TValue> dic) {

			PoolStack<TValue>.pool.Recycle(dic);
			dic = null;

		}

		public static void Recycle(Stack<TValue> dic) {

			PoolStack<TValue>.pool.Recycle(dic);

		}

	}
	
	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolSortedSet<TValue> {

		private static PoolInternalBase pool = new PoolInternalBase(typeof(SortedSet<TValue>), () => new SortedSet<TValue>(), (x) => ((SortedSet<TValue>)x).Clear());

		public static SortedSet<TValue> Spawn(int capacity = 0) {

			return (SortedSet<TValue>)PoolSortedSet<TValue>.pool.Spawn();
		    
		}

		public static void Recycle(ref SortedSet<TValue> dic) {

			PoolSortedSet<TValue>.pool.Recycle(dic);
			dic = null;

		}

		public static void Recycle(SortedSet<TValue> dic) {

			PoolSortedSet<TValue>.pool.Recycle(dic);

		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolHashSet<TValue> {

		private static PoolInternalBase pool = new PoolInternalBase(typeof(HashSet<TValue>), () => new HashSet<TValue>(), (x) => ((HashSet<TValue>)x).Clear());

		public static HashSet<TValue> Spawn(int capacity = 0) {

			return (HashSet<TValue>)PoolHashSet<TValue>.pool.Spawn();
		    
		}

		public static void Recycle(ref HashSet<TValue> dic) {

			PoolHashSet<TValue>.pool.Recycle(dic);
			dic = null;

		}

		public static void Recycle(HashSet<TValue> dic) {

			PoolHashSet<TValue>.pool.Recycle(dic);

		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolQueue<TValue> {

		private static int capacity;
		private static PoolInternalBase pool = new PoolInternalBase(typeof(Queue<TValue>), () => new Queue<TValue>(PoolQueue<TValue>.capacity), (x) => ((Queue<TValue>)x).Clear());

		public static Queue<TValue> Spawn(int capacity) {

			PoolQueue<TValue>.capacity = capacity;
			return (Queue<TValue>)PoolQueue<TValue>.pool.Spawn();
		    
		}

		public static void Recycle(ref Queue<TValue> dic) {

			PoolQueue<TValue>.pool.Recycle(dic);
			dic = null;

		}

		public static void Recycle(Queue<TValue> dic) {

			PoolQueue<TValue>.pool.Recycle(dic);

		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolList<TValue> {

		private static int capacity;
		private static PoolInternalBase pool = new PoolInternalBase(typeof(List<TValue>), () => new List<TValue>(PoolList<TValue>.capacity), (x) => ((List<TValue>)x).Clear());

		public static List<TValue> Spawn(int capacity) {

			PoolList<TValue>.capacity = capacity;
			return (List<TValue>)PoolList<TValue>.pool.Spawn();
		    
		}

		public static void Recycle(ref List<TValue> dic) {

			PoolList<TValue>.pool.Recycle(dic);
			dic = null;

		}

		public static void Recycle(List<TValue> dic) {

			PoolList<TValue>.pool.Recycle(dic);

		}

	}

}
