using System.Collections.Generic;
using System.Linq;

namespace ME.ECS {

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolClass<T> where T : class, new() {

		private static PoolInternalBase pool = new PoolInternalBase(typeof(T), () => new T(), null);

		public static T Spawn() {
		    
			return (T)PoolClass<T>.pool.Spawn();
		    
		}

		public static void Recycle(ref T instance) {
		    
			PoolClass<T>.pool.Recycle(instance);
			instance = null;

		}

		public static void Recycle(T instance) {
		    
			PoolClass<T>.pool.Recycle(instance);
		    
		}

	}

	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class PoolClassMainThread<T> where T : class, new() {

		private static PoolInternalBaseNoStackPool pool = new PoolInternalBaseNoStackPool(() => new T(), null);

		public static T Spawn() {
		    
			return (T)PoolClassMainThread<T>.pool.Spawn();
		    
		}

		public static void Recycle(ref T instance) {
		    
			PoolClassMainThread<T>.pool.Recycle(instance);
			instance = null;

		}

		public static void Recycle(T instance) {
		    
			PoolClassMainThread<T>.pool.Recycle(instance);
		    
		}

	}

    public interface IPoolableSpawn {

	    void OnSpawn();

    }

    public interface IPoolableRecycle {

	    void OnRecycle();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class PoolInternalBaseNoStackPool {

	    protected Stack<object> cache = new Stack<object>();
	    protected System.Func<object> constructor;
	    protected System.Action<object> desctructor;
	    protected int poolAllocated;
	    protected int poolDeallocated;
	    protected int poolNewAllocated;
	    protected int poolUsed;

	    public static int newAllocated;
	    public static int allocated;
	    public static int deallocated;
	    public static int used;

	    private static List<PoolInternalBaseNoStackPool> list = new List<PoolInternalBaseNoStackPool>();

	    public override string ToString() {
		    
		    return "Allocated: " + this.poolAllocated + ", Deallocated: " + this.poolDeallocated + ", Used: " + this.poolUsed + ", cached: " + this.cache.Count + ", new: " + this.poolNewAllocated;
		    
	    }

	    public PoolInternalBaseNoStackPool(System.Func<object> constructor, System.Action<object> desctructor) {

		    this.constructor = constructor;
		    this.desctructor = desctructor;
		    
		    PoolInternalBaseNoStackPool.list.Add(this);

	    }

	    public static void Clear() {

		    var pools = PoolInternalBaseNoStackPool.list;
		    for (int i = 0; i < pools.Count; ++i) {
			    
			    var pool = pools[i];
			    pool.cache.Clear();
			    pool.constructor = null;
			    pool.desctructor = null;

		    }
		    pools.Clear();
		    
	    }
	    
	    public static T Create<T>() where T : new() {
		    
		    var instance = new T();
		    PoolInternalBaseNoStackPool.CallOnSpawn(instance);

		    return instance;

	    }

	    public static void CallOnSpawn<T>(T instance) {
		    
		    if (instance is IPoolableSpawn poolable) {
			    
			    poolable.OnSpawn();
			    
		    }
		    
	    }

	    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	    public virtual void Prewarm(int count) {

		    for (int i = 0; i < count; ++i) {

			    this.Recycle(this.Spawn());

		    }

	    }

	    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	    public virtual object Spawn() {

		    object item = null;
		    if (this.cache.Count > 0) {

			    item = this.cache.Pop();

		    }
		    
		    if (item == null) {

			    ++PoolInternalBaseNoStackPool.newAllocated;
			    ++this.poolNewAllocated;

		    } else {

			    ++PoolInternalBaseNoStackPool.used;
			    ++this.poolUsed;

		    }
		    
		    if (this.constructor != null && item == null) {
			    
			    item = this.constructor.Invoke();
			    
		    }

		    if (item is IPoolableSpawn poolable) {
			    
			    poolable.OnSpawn();
			    
		    }

		    ++this.poolAllocated;
		    ++PoolInternalBaseNoStackPool.allocated;
		    
		    return item;

	    }

	    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	    public virtual void Recycle(object instance) {
		    
		    ++this.poolDeallocated;
		    ++PoolInternalBaseNoStackPool.deallocated;

		    if (this.desctructor != null) {
			    
			    this.desctructor.Invoke(instance);
			    
		    }

		    if (instance is IPoolableRecycle poolable) {
			    
			    poolable.OnRecycle();
			    
		    }

		    this.cache.Push(instance);

	    }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class PoolInternalBase {

	    #if MULTITHREAD_SUPPORT
	    protected CCStack<object> cache = new CCStack<object>(usePool: true);
		#else
		protected Stack<object> cache = new Stack<object>();
		#endif
		private HashSet<object> contains = new HashSet<object>();
	    protected System.Func<object> constructor;
	    protected System.Action<object> desctructor;
	    protected System.Type poolType;
	    protected int poolAllocated;
	    protected int poolDeallocated;
	    protected int poolNewAllocated;
	    protected int poolUsed;

	    private static List<PoolInternalBase> list = new List<PoolInternalBase>();
	    
	    #if UNITY_EDITOR
	    private Dictionary<object, string> stackTraces;
	    private static bool isStackTraceEnabled;
	    private static bool isStackTraceEnabledSet;
	    #endif
	    
	    public static int newAllocated;
	    public static int allocated;
	    public static int deallocated;
	    public static int used;

	    public static void Clear() {

		    var pools = PoolInternalBase.list;
		    for (int i = 0; i < pools.Count; ++i) {
			    
			    var pool = pools[i];
			    pool.cache.Clear();
			    pool.constructor = null;
			    pool.desctructor = null;

		    }
		    pools.Clear();
		    
	    }

	    #if UNITY_EDITOR
	    [UnityEditor.MenuItem("ME.ECS/Debug/Enable Stack Trace", isValidateFunction: true)]
	    public static bool StackTraceValidation() {

		    var key = "ME.Pools.StackTraceEnabled";
		    var flag = UnityEditor.EditorPrefs.GetBool(key, false);
		    UnityEditor.Menu.SetChecked("ME.ECS/Debug/Enable Stack Trace", flag);
		    return true;

	    }

	    [UnityEditor.MenuItem("ME.ECS/Debug/Enable Stack Trace")]
	    public static void StackTrace() {

		    var key = "ME.Pools.StackTraceEnabled";
		    UnityEditor.EditorPrefs.SetBool(key, !UnityEditor.EditorPrefs.GetBool(key, false));
				
	    }

	    public static bool IsStackTraceEnabled() {

		    if (PoolInternalBase.isStackTraceEnabledSet == true) return PoolInternalBase.isStackTraceEnabled;
		    
		    var key = "ME.Pools.StackTraceEnabled";
		    PoolInternalBase.isStackTraceEnabled = UnityEditor.EditorPrefs.GetBool(key, false);
		    PoolInternalBase.isStackTraceEnabledSet = true;

		    return PoolInternalBase.isStackTraceEnabled;

	    }

	    [UnityEditor.MenuItem("ME.ECS/Debug/Pools Info")]
	    public static void Debug() {
		    
		    UnityEngine.Debug.Log("Allocated: " + PoolInternalBase.allocated + ", Deallocated: " + PoolInternalBase.deallocated + ", Used: " + PoolInternalBase.used + ", cached: " + (PoolInternalBase.deallocated - PoolInternalBase.allocated) + ", new: " + PoolInternalBase.newAllocated);

		    PoolInternalBase maxCached = null;
		    PoolInternalBase maxAlloc = null;
		    int maxCountCache = 0;
		    int maxCountAlloc = 0;
		    for (int i = 0; i < PoolInternalBase.list.Count; ++i) {

			    var item = PoolInternalBase.list[i];
			    if (maxCountCache < item.cache.Count) {

				    maxCountCache = item.cache.Count;
				    maxCached = item;

			    }
			    
			    if (maxCountAlloc < item.poolAllocated) {

				    maxCountAlloc = item.poolAllocated;
				    maxAlloc = item;

			    }

		    }

		    if (maxCached != null) {
			    
			    UnityEngine.Debug.Log("Max cache type: " + maxCached.poolType + ", Pool:\n" + maxCached);
			    
		    }

		    if (maxAlloc != null) {
			    
			    UnityEngine.Debug.Log("Max alloc type: " + maxAlloc.poolType + ", Pool:\n" + maxAlloc);
			    
		    }

		    for (int i = 0; i < PoolInternalBase.list.Count; ++i) {

			    var item = PoolInternalBase.list[i];
			    if (item.poolAllocated != item.poolDeallocated) {

				    UnityEngine.Debug.LogWarning("Memory leak: " + item.poolType + ", Pool:\n" + item);

				    if (PoolInternalBase.IsStackTraceEnabled() == true && item.stackTraces != null) {

					    var max = 10;
					    foreach (var stack in item.stackTraces) {

						    UnityEngine.Debug.Log(stack.Key.GetType() + "\n" + stack.Value);
						    --max;
						    if (max <= 0) break;
						    
					    }

				    }
				    
			    }
			    
		    }
		    
	    }
	    
	    [UnityEditor.MenuItem("ME.ECS/Debug/Pools Reset Stats")]
	    public static void DebugReset() {

		    for (int i = 0; i < PoolInternalBase.list.Count; ++i) {

			    PoolInternalBase.list[i].ResetStat();

		    }

		    PoolInternalBase.allocated = 0;
		    PoolInternalBase.deallocated = 0;
		    PoolInternalBase.used = 0;
		    PoolInternalBase.newAllocated = 0;

	    }
	    #endif

	    public void ResetStat() {

		    this.poolAllocated = 0;
		    this.poolDeallocated = 0;
		    this.poolUsed = 0;
		    this.poolNewAllocated = 0;

	    }

	    public override string ToString() {
		    
		    return "Allocated: " + this.poolAllocated + ", Deallocated: " + this.poolDeallocated + ", Used: " + this.poolUsed + ", cached: " + this.cache.Count + ", new: " + this.poolNewAllocated;
		    
	    }

	    public PoolInternalBase(System.Type poolType, System.Func<object> constructor, System.Action<object> desctructor) {

		    this.poolType = poolType;
		    this.constructor = constructor;
		    this.desctructor = desctructor;

		    PoolInternalBase.list.Add(this);
		    
	    }

	    public static T Create<T>(PoolInternalBase pool) where T : new() {
		    
		    var instance = new T();
		    PoolInternalBase.CallOnSpawn(instance, pool);

		    return instance;

	    }

	    public static void CallOnSpawn<T>(T instance, PoolInternalBase pool) {
		    
		    if (instance is IPoolableSpawn poolable) {
			    
			    poolable.OnSpawn();
			    
		    }

		    #if UNITY_EDITOR
		    if (PoolInternalBase.IsStackTraceEnabled() == true) {
			    
			    pool.WriteStackTrace(instance);
			    
		    }
		    #endif
		    
	    }

	    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	    public virtual void Prewarm(int count) {

		    for (int i = 0; i < count; ++i) {

			    this.Recycle(this.Spawn());

		    }

	    }

	    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	    public virtual object Spawn() {

			#if MULTITHREAD_SUPPORT
		    this.cache.TryPop(out object item);
			#else
			var item = (this.cache.Count > 0 ? this.cache.Pop() : null);
			#endif
		    if (item == null) {

			    ++PoolInternalBase.newAllocated;
			    ++this.poolNewAllocated;

		    } else {

			    this.contains.Remove(item);
			    ++PoolInternalBase.used;
			    ++this.poolUsed;

		    }
		    
		    if (this.constructor != null && item == null) {
			    
			    item = this.constructor.Invoke();
			    
		    }

		    if (item is IPoolableSpawn poolable) {
			    
			    poolable.OnSpawn();
			    
		    }

		    ++this.poolAllocated;
		    ++PoolInternalBase.allocated;

		    #if UNITY_EDITOR
		    if (PoolInternalBase.IsStackTraceEnabled() == true) {

			    this.WriteStackTrace(item);

		    }
		    
		    if (item != null) {
			    
			    this.poolType = item.GetType();
			    
		    }
		    #endif
		    
		    return item;

	    }

	    #if UNITY_EDITOR
	    private void RemoveStackTrace(object obj) {
		    
		    if (obj == null) return;
		    
		    if (this.stackTraces == null) this.stackTraces = new Dictionary<object, string>();
		    this.stackTraces.Remove(obj);

	    }
	    
	    private void WriteStackTrace(object obj) {

		    if (obj == null) return;
		    
		    var stack = System.Environment.StackTrace;
		    if (this.stackTraces == null) this.stackTraces = new Dictionary<object, string>();
		    if (this.stackTraces.ContainsKey(obj) == false) this.stackTraces.Add(obj, stack);
		    
	    }
	    #endif

	    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
	    public virtual void Recycle(object instance) {
		    
		    #if UNITY_EDITOR
		    if (PoolInternalBase.IsStackTraceEnabled() == true) {

			    this.RemoveStackTrace(instance);

		    }

		    if (instance != null) {
			    
			    this.poolType = instance.GetType();
			    
		    }
		    #endif

		    ++this.poolDeallocated;
		    ++PoolInternalBase.deallocated;

		    if (this.desctructor != null) {
			    
			    this.desctructor.Invoke(instance);
			    
		    }

		    if (instance is IPoolableRecycle poolable) {
			    
			    poolable.OnRecycle();
			    
		    }

		    if (this.contains.Contains(instance) == false) {

			    this.contains.Add(instance);
			    this.cache.Push(instance);

		    } else {
			    
			    UnityEngine.Debug.LogError("You are trying to push instance " + instance + " that already in pool!");
			    
		    }

	    }

    }

}
