#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

using System.Collections.Generic;

namespace ME.ECS {

    public class Pools {

        public static bool isActive = true;
        public static IPoolImplementation current = new PoolImplementation(isNull: true);

    }

    public struct DefaultPools {

        public System.Collections.Generic.Dictionary<System.Type, ME.ECS.PoolInternalBase> pool;

        public void Init() {
            
            this.pool = new System.Collections.Generic.Dictionary<System.Type, PoolInternalBase>();

        }

        public void Clear() {

            foreach (var item in this.pool) {

                item.Value.Clean();

            }
            this.pool.Clear();
            this.pool = null;

        }

        public T Spawn<T, TState>(TState state, System.Func<TState, T> constructor, System.Action<T> destructor = null) where T : class {

            if (Pools.isActive == false) {
                var instance = constructor.Invoke(state);
                PoolInternalBase.CallOnSpawn(instance, null);
                return instance;
            }

            var type = typeof(T);
            if (this.pool.TryGetValue(type, out var pool) == true) {

                return (T)pool.Spawn();

            }

            pool = new PoolInternal<T, TState>(type, state, constructor, destructor);
            this.pool.Add(type, pool);

            return (T)pool.Spawn();

        }

        public bool Recycle<T>(ref T obj) where T : class {
            
            if (obj == null) return false;
            
            if (Pools.isActive == false) {
                PoolInternalBase.CallOnDespawn(obj, null);
                obj = default;
                return false;
            }

            {
                var type = typeof(T);
                if (this.pool.TryGetValue(type, out var pool) == true) {

                    pool.Recycle(obj);
                    obj = default;
                    return true;

                }
            }

            {
                var type = obj.GetType();
                if (this.pool.TryGetValue(type, out var pool) == true) {

                    pool.Recycle(obj);
                    obj = default;
                    return true;

                }
            }

            return false;

        }

    }

    public class PoolImplementation : IPoolImplementation {

        private DefaultPools pools;
        public bool isNull;

        public PoolImplementation(bool isNull) {

            this.pools.Init();
            this.isNull = isNull;

        }

        void IPoolImplementation.Clear() {

            this.pools.Clear();

        }

        T IPoolImplementation.PoolSpawn<T>(System.Action<T> destructor) {

            if (this.isNull == true) {
                var res = new T();
                PoolInternalBase.CallOnSpawn(res, null);
                return res;
            }
            return this.pools.Spawn<T, byte>(0, (state) => new T(), destructor);
            
        }

        T IPoolImplementation.PoolSpawn<T, TState>(TState state, System.Func<TState, T> constructor, System.Action<T> destructor) {

            if (this.isNull == true) {
                var res = constructor.Invoke(state);
                PoolInternalBase.CallOnSpawn(res, null);
                //ME.WeakRef.Reg(res);
                return res;
            }
            var instance = this.pools.Spawn<T, TState>(state, constructor, destructor);
            //ME.WeakRef.Reg(instance);
            return instance;

        }

        void IPoolImplementation.PoolRecycle<T>(ref T obj) {

            if (this.isNull == true || this.pools.Recycle(ref obj) == false) {
                PoolInternalBase.CallOnDespawn(obj, null);
                obj = null;
            }
            
        }

    }
    
    public interface IPoolImplementation {

        T PoolSpawn<T>(System.Action<T> destructor) where T : class, new();
        T PoolSpawn<T, TState>(TState state, System.Func<TState, T> constructor, System.Action<T> destructor) where T : class;
        void PoolRecycle<T>(ref T obj) where T : class;
        void Clear();

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

            return "Allocated: " + this.poolAllocated + ", Deallocated: " + this.poolDeallocated + ", Used: " + this.poolUsed + ", cached: " + this.cache.Count + ", new: " +
                   this.poolNewAllocated;

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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual void Prewarm(int count) {

            for (int i = 0; i < count; ++i) {

                this.Recycle(this.Spawn());

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
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

    public class PoolInternal<T, TState> : PoolInternalBase where T : class {

        public TState state;
        protected System.Func<TState, T> constructor;
        protected System.Action<T> destructor;

        public PoolInternal(System.Type poolType, TState state, System.Func<TState, T> constructor, System.Action<T> destructor) : base(poolType) {

            this.state = state;
            this.constructor = constructor;
            this.destructor = destructor;

        }

        protected override void OnClear() {
            
            base.OnClear();

            this.state = default;
            this.constructor = default;
            this.destructor = default;

        }

        protected override void Construct(ref object item) {
            
            if (this.constructor != null && item == null) {

                item = this.constructor.Invoke(this.state);

            }
            
        }

        protected override void Destruct(object item) {
            
            if (this.destructor != null) {

                this.destructor.Invoke((T)item);

            }
            
        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class PoolInternalBase {

        protected Stack<object> cache = new Stack<object>();
        private HashSet<object> contains = new HashSet<object>();
        protected System.Type poolType;
        private int poolBytesSize;
        protected int poolAllocated;
        protected int poolDeallocated;
        protected int poolNewAllocated;
        protected int poolUsed;
        protected int poolBytesUsed;

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
        public static int bytesUsed;

        public static void Clear() {

            var pools = PoolInternalBase.list;
            for (int i = 0; i < pools.Count; ++i) {

                var pool = pools[i];
                pool.OnClear();

            }

            pools.Clear();

        }

        public void Clean() {
            
            this.OnClear();
            
        }

        protected virtual void OnClear() {

            if (PoolInternalBase.list.Count > 0) {
                PoolInternalBase.list.Remove(this);
            }
            this.cache.Clear();
            this.contains.Clear();
            
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

            try {

                var key = "ME.Pools.StackTraceEnabled";
                PoolInternalBase.isStackTraceEnabled = UnityEditor.EditorPrefs.GetBool(key, false);
                PoolInternalBase.isStackTraceEnabledSet = true;

            } catch (System.Exception) {
            
            }

            return PoolInternalBase.isStackTraceEnabled;

        }

        [UnityEditor.MenuItem("ME.ECS/Debug/Pools Info")]
        public static void Debug() {

            UnityEngine.Debug.Log($"Allocated: {PoolInternalBase.allocated}, Deallocated: {PoolInternalBase.deallocated}, Used: {PoolInternalBase.used}, cached: {(PoolInternalBase.deallocated - PoolInternalBase.allocated)}, new: {PoolInternalBase.newAllocated}, approx bytes used: {PoolInternalBase.bytesUsed}");

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

                UnityEngine.Debug.Log($"Max cache type: {maxCached.poolType}, Pool:\n{maxCached}");

            }

            if (maxAlloc != null) {

                UnityEngine.Debug.Log($"Max alloc type: {maxAlloc.poolType}, Pool:\n{maxAlloc}");

            }

            for (int i = 0; i < PoolInternalBase.list.Count; ++i) {

                var item = PoolInternalBase.list[i];
                if (item.poolAllocated != item.poolDeallocated) {

                    UnityEngine.Debug.LogWarning($"Memory leak: {item.poolType}, Pool:\n{item}");

                    if (PoolInternalBase.IsStackTraceEnabled() == true && item.stackTraces != null) {

                        var max = 10;
                        foreach (var stack in item.stackTraces) {

                            UnityEngine.Debug.Log($"{stack.Key.GetType()}\n{stack.Value}");
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
            PoolInternalBase.bytesUsed = 0;

        }
        #endif

        public void ResetStat() {

            this.poolAllocated = 0;
            this.poolDeallocated = 0;
            this.poolUsed = 0;
            this.poolNewAllocated = 0;
            this.poolBytesUsed = 0;

        }

        public override string ToString() {

            return $"Allocated: {this.poolAllocated}, Deallocated: {this.poolDeallocated}, Used: {this.poolUsed}, cached: {this.cache.Count}, new: {this.poolNewAllocated}, approx bytes used: {this.poolBytesUsed}";

        }

        public PoolInternalBase(System.Type poolType) {

            this.poolType = poolType;
            this.poolBytesSize = 0;
            #if UNITY_EDITOR
            if (poolType.IsValueType == true) {
                this.poolBytesSize = Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf(poolType);
            }
            #endif
            
            PoolInternalBase.list.Add(this);

        }

        public static int GetSizeOfObject(object obj, int avgStringSize = -1) {

            if (obj == null) return 0;

            var pointerSize = System.IntPtr.Size;
            var size = 0;
            var type = obj.GetType();
            var info = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            foreach (var field in info) {
                if (field.FieldType.IsEnum) {
                    size += sizeof(int);
                } else if (field.FieldType.IsValueType) {
                    try {
                        size += System.Runtime.InteropServices.Marshal.SizeOf(field.FieldType);
                    } catch (System.Exception) {
                    }
                } else {
                    size += pointerSize;
                    if (field.FieldType.IsArray) {
                        var array = field.GetValue(obj) as System.Array;
                        if (array != null) {
                            var elementType = array.GetType().GetElementType();
                            if (elementType.IsValueType) {
                                try {
                                    size += System.Runtime.InteropServices.Marshal.SizeOf(elementType) * array.Length;
                                } catch (System.Exception) {
                                }
                            } else {
                                size += pointerSize * array.Length;
                                if (elementType == typeof(string) && avgStringSize > 0) {
                                    size += avgStringSize * array.Length;
                                }
                            }
                        }
                    } else if (field.FieldType == typeof(string) && avgStringSize > 0) {
                        size += avgStringSize;
                    }
                }
            }

            return size;
        }

        public static T Create<T>(PoolInternalBase pool) where T : new() {

            var instance = new T();
            PoolInternalBase.CallOnSpawn(instance, pool);

            return instance;

        }

        public static void CallOnDespawn<T>(T instance, PoolInternalBase pool) {
            
            #if UNITY_EDITOR
            if (PoolInternalBase.IsStackTraceEnabled() == true) {

                pool?.RemoveStackTrace(instance);

            }
            #endif

            if (instance is IPoolableRecycle poolable) {

                poolable.OnRecycle();

            }
            
        }

        public static void CallOnSpawn<T>(T instance, PoolInternalBase pool) {

            if (instance is IPoolableSpawn poolable) {

                poolable.OnSpawn();

            }

            #if UNITY_EDITOR
            if (PoolInternalBase.IsStackTraceEnabled() == true) {

                pool?.WriteStackTrace(instance);

            }
            #endif

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual void Prewarm(int count) {

            for (int i = 0; i < count; ++i) {

                this.Recycle(this.Spawn());

            }

        }

        protected virtual void Construct(ref object obj) {
            
        }

        protected virtual void Destruct(object item) {
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual object Spawn() {

            var item = (this.cache.Count > 0 ? this.cache.Pop() : null);
            if (item == null) {

                ++PoolInternalBase.newAllocated;
                ++this.poolNewAllocated;

            } else {

                this.contains.Remove(item);
                ++PoolInternalBase.used;
                ++this.poolUsed;

            }

            this.Construct(ref item);
            
            if (item is IPoolableSpawn poolable) {

                poolable.OnSpawn();

            }

            ++this.poolAllocated;
            ++PoolInternalBase.allocated;

            #if UNITY_EDITOR
            var bytes = this.poolBytesSize;

            if (PoolInternalBase.IsStackTraceEnabled() == true) {

                bytes = PoolInternalBase.GetSizeOfObject(item, 8);
                this.poolBytesUsed += bytes;
                PoolInternalBase.bytesUsed += bytes;

                this.WriteStackTrace(item);

            } else {
                
                this.poolBytesUsed += bytes;
                PoolInternalBase.bytesUsed += bytes;

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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public virtual void Recycle(object instance) {

            #if UNITY_EDITOR
            if (PoolInternalBase.IsStackTraceEnabled() == true) {

                //var bytes = PoolInternalBase.GetSizeOfObject(instance, 8);
                //this.poolBytesUsed -= bytes;
                //PoolInternalBase.bytesUsed -= bytes;
                
                this.RemoveStackTrace(instance);

            } else {

                //var bytes = this.poolBytesUsed;
                //this.poolBytesUsed -= bytes;
                //PoolInternalBase.bytesUsed -= bytes;
                
            }

            if (instance != null) {

                this.poolType = instance.GetType();

            }
            #endif

            ++this.poolDeallocated;
            ++PoolInternalBase.deallocated;

            this.Destruct(instance);
            
            if (instance is IPoolableRecycle poolable) {

                poolable.OnRecycle();

            }

            if (this.contains.Contains(instance) == false) {

                this.contains.Add(instance);
                this.cache.Push(instance);

            } else {

                UnityEngine.Debug.LogError($"You are trying to push instance {instance} that already in pool!");

            }

        }

    }

}