using System.Collections;
using System.Collections.Generic;

namespace ME.ECS {

    using Collections;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class Components : IPoolableRecycle {

        public struct Bucket {

            public BufferArray<ListCopyable<IComponent>> components;
            public bool includeInHash;

        }

        [ME.ECS.Serializer.SerializeField]
        private BufferArray<Bucket> buckets; // arr by component type
        [ME.ECS.Serializer.SerializeField]
        private int capacity;
        
        private static int typeId;
        
        public void Initialize(int capacity) {

            this.buckets = PoolArray<Bucket>.Spawn(capacity);
            
        }

        public void SetFreeze(bool freeze) {

        }

        void IPoolableRecycle.OnRecycle() {

            this.CleanUp_INTERNAL();
            
            this.capacity = default;
            
        }

        public int GetHash() {

            var count = 0;
            for (int i = 0; i < this.buckets.Length; ++i) {

                ref var bucket = ref this.buckets.arr[i];
                if (bucket.includeInHash == true) {

                    if (bucket.components.arr != null) {

                        for (int j = 0; j < bucket.components.Length; ++j) {

                            var list = bucket.components.arr[j];
                            if (list != null) count += list.Count;

                        }

                    }

                }

            }

            return count;

        }

        public int RemoveAllPredicate<TComponent, TComponentPredicate>(int entityId, TComponentPredicate predicate) where TComponent : class, IComponent where TComponentPredicate : IComponentPredicate<TComponent> {

            var count = 0;
            var typeId = Components.GetTypeId<TComponent>();
            if (typeId < 0 || typeId >= this.buckets.Length) return count;

            ref var bucket = ref this.buckets.arr[typeId];
            if (bucket.components.arr == null || entityId < 0 || entityId >= bucket.components.Length) return count;

            ref var list = ref bucket.components.arr[entityId];
            if (list == null) return count;
            
            for (int i = list.Count - 1; i >= 0; --i) {

                var tComp = list[i] as TComponent;
                if (predicate.Execute(tComp) == false) continue;
                
                PoolComponents.Recycle(ref tComp);
                list.RemoveAt(i);
                ++count;

            }

            return count;

        }

        public int RemoveAll<TComponent>(int entityId) where TComponent : class, IComponent {
            
            return this.RemoveAll_INTERNAL<TComponent>(entityId);

        }

        public int RemoveAll<TComponent>() where TComponent : class, IComponent {

            return this.RemoveAll_INTERNAL<TComponent>();

        }
        
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int RemoveAll_INTERNAL<TComponent>() where TComponent : class, IComponentBase {

            var count = 0;
            for (int j = 0; j < this.buckets.Length; ++j) {

                ref var bucket = ref this.buckets.arr[j];
                if (bucket.components.arr == null) continue;
                
                for (int k = 0; k < bucket.components.Length; ++k) {

                    var list = bucket.components.arr[k];
                    if (list == null) continue;

                    for (int i = list.Count - 1; i >= 0; --i) {

                        if (list[i] is TComponent tComp) {

                            PoolComponents.Recycle(ref tComp);
                            list.RemoveAtFast(i);
                            ++count;

                        }

                    }

                }
                
            }
            
            return count;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int RemoveAll_INTERNAL<TComponent>(int entityId) where TComponent : class, IComponentBase {

            var count = 0;
            var typeId = Components.GetTypeId<TComponent>();
            if (typeId < 0 || typeId >= this.buckets.Length) return count;

            ref var bucket = ref this.buckets.arr[typeId];
            if (bucket.components.arr == null || entityId < 0 || entityId >= bucket.components.Length) return count;

            var list = bucket.components.arr[entityId];
            if (list == null) return 0;
                    
            for (int i = list.Count - 1; i >= 0; --i) {

                var tComp = list[i] as TComponent;
                PoolComponents.Recycle(ref tComp);
                list.RemoveAtFast(i);
                ++count;

            }

            return count;

        }

        public int RemoveAll(int entityId) {

            var count = 0;
            for (int i = 0; i < this.buckets.Length; ++i) {

                ref var bucket = ref this.buckets.arr[i];
                if (bucket.components.arr != null && entityId >= 0 && entityId < bucket.components.Length) {

                    ref var list = ref bucket.components.arr[entityId];
                    if (list == null) continue;
                    
                    count += list.Count;
                    PoolComponents.Recycle(list);
                    list.Clear();
                    
                }

            }

            return count;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static int GetTypeId<TComponent>() {

            return WorldUtilities.GetAllComponentTypeId<TComponent>();

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool IsTypeInHash<TComponent>() {

            return WorldUtilities.IsAllComponentInHash<TComponent>();

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetTypeInHash<TComponent>(bool state) {

            WorldUtilities.SetAllComponentInHash<TComponent>(state);

        }

        public void Add<TComponent>(int entityId, TComponent data) where TComponent : class, IComponent {

            var typeId = Components.GetTypeId<TComponent>();
            ArrayUtils.Resize(typeId, ref this.buckets);
            //ArrayUtils.RawResize(typeId, ref this.buckets);
            
            ref var bucket = ref this.buckets.arr[typeId];
            bucket.includeInHash = Components.IsTypeInHash<TComponent>();
            ArrayUtils.Resize(entityId, ref bucket.components);
            //ArrayUtils.RawResize(entityId, ref bucket.components);

            if (bucket.components.arr[entityId] == null) bucket.components.arr[entityId] = PoolListCopyable<IComponent>.Spawn(1);
            bucket.components.arr[entityId].Add(data);
            
        }
        
        public TComponent GetFirst<TComponent>(int entityId) where TComponent : class, IComponent {
            
            var typeId = Components.GetTypeId<TComponent>();
            if (typeId >= 0 && typeId < this.buckets.Length) {

                ref var bucket = ref this.buckets.arr[typeId];
                if (bucket.components.arr == null || entityId < 0 || entityId >= bucket.components.Length || bucket.components.arr[entityId] == null) return null;
                
                var list = bucket.components.arr[entityId];
                if (list.Count > 0) return list[0] as TComponent;

            }

            return null;

        }

        public BufferArray<Bucket> GetAllBuckets() {
            
            return this.buckets;

        }

        public ListCopyable<IComponent> ForEach<TComponent>(int entityId) where TComponent : class, IComponent {
            
            var typeId = Components.GetTypeId<TComponent>();
            if (typeId >= 0 && typeId < this.buckets.Length) {

                ref var bucket = ref this.buckets.arr[typeId];
                if (bucket.components.arr == null || entityId < 0 || entityId >= bucket.components.Length || bucket.components.arr[entityId] == null) return null;
                
                return bucket.components.arr[entityId];
                
            }

            return null;

        }

        public bool Contains<TComponent>(int entityId) where TComponent : class, IComponent {

            var typeId = Components.GetTypeId<TComponent>();
            if (typeId >= 0 && typeId < this.buckets.Length) {

                ref var bucket = ref this.buckets.arr[typeId];
                if (bucket.components.arr == null || entityId < 0 || entityId >= bucket.components.Length || bucket.components.arr[entityId] == null) return false;

                return bucket.components.arr[entityId].Count > 0;

            }
            
            return false;

        }

        public IList<IComponentBase> GetData(int entityId) {

            var list = new List<IComponentBase>();
            for (int i = 0; i < this.buckets.Length; ++i) {

                var bucket = this.buckets.arr[i];
                if (bucket.components.arr == null || entityId < 0 || entityId >= bucket.components.Length || bucket.components.arr[entityId] == null) continue;

                list.AddRange(bucket.components.arr[entityId]);

            }

            return list;

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void CleanUp_INTERNAL() {

            if (this.buckets.arr == null) return;
            
            ArrayUtils.Recycle(ref this.buckets, new CopyBucket());
            
        }

        private struct CopyComponent : IArrayElementCopy<IComponent> {

            public void Copy(IComponent from, ref IComponent to) {
                
                var comp = to;
                if (comp == null) {

                    var type = from.GetType();
                    comp = (IComponent)PoolComponents.Spawn(type);
                    
                }

                if (comp is IComponentCopyable compCopyable) compCopyable.CopyFrom((IComponentCopyable)from);
                to = comp;

            }

            public void Recycle(IComponent item) {
                
                if (item != null) PoolComponents.Recycle(item, item.GetType());
                
            }

        }

        private struct CopyComponentsList : IArrayElementCopy<ListCopyable<IComponent>> {

            public void Copy(ListCopyable<IComponent> from, ref ListCopyable<IComponent> to) {
                
                ArrayUtils.Copy(from, ref to, new CopyComponent());
                
            }
            
            public void Recycle(ListCopyable<IComponent> item) {

                ArrayUtils.Recycle(ref item, new CopyComponent());

            }

        }

        private struct CopyBucket : IArrayElementCopy<Bucket> {

            public void Copy(Bucket from, ref Bucket to) {
                
                ArrayUtils.Copy(from.components, ref to.components, new CopyComponentsList());
                
            }

            public void Recycle(Bucket item) {

                ArrayUtils.Recycle(ref item.components, new CopyComponentsList());

            }

        }

        public void CopyFrom(in Entity from, in Entity to) {

            for (int i = 0; i < this.buckets.Length; ++i) {

                var bucket = this.buckets.arr[i];
                if (bucket.components.arr == null || from.id >= bucket.components.Length) continue;
                
                var list = bucket.components.arr[from.id];
                if (list == null) continue;
                
                for (int j = 0; j < list.Count; ++j) {

                    var item = list[j];
                    if (item == null) continue;

                    //if (item is ME.ECS.Views.ViewComponent) continue;
                    
                    IComponent newItem = null;
                    var copyComponent = new CopyComponent();
                    copyComponent.Copy(item, ref newItem);

                    ArrayUtils.Resize(to.id, ref bucket.components);
                    ref var obj = ref bucket.components.arr[to.id];
                    if (obj == null) obj = PoolListCopyable<IComponent>.Spawn(4);
                    obj.Add(newItem);

                }

            }

            /*
            var allViews = from.ForEachComponent<ME.ECS.Views.ViewComponent>();
            if (allViews != null) {

                for (int index = 0, count = allViews.Count; index < count; ++index) {

                    var viewComponent = (ME.ECS.Views.ViewComponent)allViews[index];
                    to.InstantiateView(viewComponent.viewInfo.prefabSourceId);

                }

            }*/

        }
        
        public void CopyFrom(Components other) {
            
            this.capacity = other.capacity;
            
            // Clone other array
            ArrayUtils.Copy(other.buckets, ref this.buckets, new CopyBucket());
            
        }
        
    }

}