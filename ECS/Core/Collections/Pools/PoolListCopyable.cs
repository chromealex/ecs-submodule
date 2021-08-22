namespace ME.ECS {

	using System.Collections.Generic;
	using ME.ECS.Collections;

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolCCList<TValue> {

		public struct Data {

		}

		public static CCList<TValue> Spawn() {

			return Pools.current.PoolSpawn(new Data(), (data) => new CCList<TValue>(), x => x.ClearNoCC());
			
		}

		public static void Recycle(ref CCList<TValue> dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolSortedSetCopyable<TValue> {

		public static SortedSetCopyable<TValue> Spawn() {

			return Pools.current.PoolSpawn<SortedSetCopyable<TValue>>(x => x.Clear());
			
		}

		public static void Recycle(ref SortedSetCopyable<TValue> dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolHashSetCopyable<TValue> {

		public static HashSetCopyable<TValue> Spawn() {

			return Pools.current.PoolSpawn<HashSetCopyable<TValue>>((x) => ((HashSetCopyable<TValue>)x).Clear());
			
		}

		public static void Recycle(ref HashSetCopyable<TValue> dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolQueueCopyable<TValue> where TValue : struct {

		public static QueueCopyable<TValue> Spawn(int capacity) {

			return Pools.current.PoolSpawn(capacity, c => new QueueCopyable<TValue>(c), x => x.Clear());
			
		}

		public static void Recycle(ref QueueCopyable<TValue> dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolListCopyable<TValue> {

		public static ListCopyable<TValue> Spawn(int capacity) {

			return Pools.current.PoolSpawn(capacity, c => new ListCopyable<TValue>(c), x => x.Clear());
			
		}

		public static void Recycle(ref ListCopyable<TValue> dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolSortedList<TKey, TValue> where TKey : struct, System.IComparable {
		
		public struct Data {

			public int capacity;

		}

		public static ME.ECS.Collections.SortedList<TKey, TValue> Spawn(int capacity) {

			return Pools.current.PoolSpawn(new Data() {
				capacity = capacity,
			}, (data) => new ME.ECS.Collections.SortedList<TKey, TValue>(data.capacity), x => x.Clear());
			
		}

		public static void Recycle(ME.ECS.Collections.SortedList<TKey, TValue> dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

		public static void Recycle(ref ME.ECS.Collections.SortedList<TKey, TValue> dic) {

			Pools.current.PoolRecycle(ref dic);
			
		}

	}

}
