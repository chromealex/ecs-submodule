#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if VIEWS_MODULE_SUPPORT
using System.Collections.Generic;
using Unity.Jobs;

namespace ME.ECS {

    using ME.ECS.Views;

    public partial interface IWorldBase {

        void InstantiateView(ViewId prefab, Entity entity);
        void InstantiateView(IView prefab, Entity entity);

        void InstantiateViewShared(ViewId prefab);
        void InstantiateViewShared(IView prefab);

        ViewId RegisterViewSource<TProvider>(TProvider providerInitializer, IView prefab) where TProvider : struct, IViewsProviderInitializer;
        bool UnRegisterViewSource(IView prefab);
        void DestroyView(ref IView instance);
        void DestroyAllViews(Entity entity);

        ViewId RegisterViewSourceShared<TProvider>(TProvider providerInitializer, IView prefab) where TProvider : struct, IViewsProviderInitializer;
        bool UnRegisterViewSourceShared(IView prefab);
        void DestroyViewShared(ref IView instance);
        void DestroyAllViewsShared();

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class EntityExtensions {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ReplaceView(this Entity entity, ViewId viewId) {

            Worlds.currentWorld.DestroyAllViews(in entity);
            Worlds.currentWorld.InstantiateView(viewId, entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ReplaceView(this Entity entity, IView prefab) {

            Worlds.currentWorld.DestroyAllViews(in entity);
            Worlds.currentWorld.InstantiateView(prefab, entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void InstantiateView(this Entity entity, ViewId viewId) {

            Worlds.currentWorld.InstantiateView(viewId, entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void InstantiateView(this Entity entity, IView prefab) {

            Worlds.currentWorld.InstantiateView(prefab, entity);

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed partial class World {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        partial void DestroyEntityPlugin2(Entity entity) {

            var viewsModule = this.GetModule<ViewsModule>();
            viewsModule.DestroyAllViews(entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        partial void RegisterPlugin1ModuleForEntity() {

            if (this.HasModule<ViewsModule>() == false) {

                this.AddModule<ViewsModule>();

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool UnRegisterViewSource(IView prefab) {

            var viewsModule = this.GetModule<ViewsModule>();
            return viewsModule.UnRegisterViewSource(prefab);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId RegisterViewSource<TProvider>(TProvider providerInitializer, IView prefab) where TProvider : struct, IViewsProviderInitializer {

            var viewsModule = this.GetModule<ViewsModule>();
            return viewsModule.RegisterViewSource(providerInitializer, prefab);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(IView prefab, Entity entity) {

            var viewsModule = this.GetModule<ViewsModule>();
            viewsModule.InstantiateView(prefab, entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(ViewId prefab, Entity entity) {

            var viewsModule = this.GetModule<ViewsModule>();
            viewsModule.InstantiateView(prefab, entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void DestroyView(ref IView instance) {

            var viewsModule = this.GetModule<ViewsModule>();
            viewsModule.DestroyView(ref instance);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void DestroyAllViews(in Entity entity) {

            var viewsModule = this.GetModule<ViewsModule>();
            viewsModule.DestroyAllViews(in entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool UnRegisterViewSourceShared(IView prefab) {

            return this.UnRegisterViewSource(prefab);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId RegisterViewSourceShared<TProvider>(TProvider providerInitializer, IView prefab) where TProvider : struct, IViewsProviderInitializer {

            return this.RegisterViewSource<TProvider>(providerInitializer, prefab);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateViewShared(IView prefab) {

            this.InstantiateView(prefab, this.sharedEntity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateViewShared(ViewId prefab) {

            this.InstantiateView(prefab, this.sharedEntity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void DestroyViewShared(ref IView instance) {

            this.DestroyView(ref instance);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void DestroyAllViewsShared() {

            this.DestroyAllViews(this.sharedEntity);

        }

    }

}

namespace ME.ECS.Views {

    using ME.ECS.Collections;

    internal interface IViewBaseInternal {

        void Setup(World world, ViewInfo viewInfo);

    }

    public interface IViewBase {

        World world { get; }
        Entity entity { get; }
        uint entityVersion { get; set; }
        ViewId prefabSourceId { get; }
        Tick creationTick { get; }

    }

    public interface IViewRespawnTime {

        float respawnTime { get; set; }
        float cacheTimeout { get; }
        bool useCache { get; }

    }

    public interface IViewPool {

        uint customViewId { get; }

    }

    public interface IView : IViewBase, System.IComparable<IView> {

        void DoInitialize();
        void DoDeInitialize();
        void ApplyState(float deltaTime, bool immediately);
        void ApplyPhysicsState(float deltaTime);
        void OnUpdate(float deltaTime);

        void UpdateParticlesSimulation(float deltaTime);
        void SimulateParticles(float time, uint seed);

    }

    public interface IViewModuleBase : IModuleBase {

        BufferArray<Views> GetData();
        HashSet<ViewInfo> GetRendering();

        System.Collections.IDictionary GetViewSourceData();
        IViewsProviderBase GetViewSourceProvider(ViewId viewSourceId);

        bool UpdateRequests();

    }

    public partial interface IViewModule : IViewModuleBase, IModule {

        void Register(IView instance);
        bool UnRegister(IView instance);

        ViewId RegisterViewSource<TProvider>(TProvider providerInitializer, IView prefab) where TProvider : struct, IViewsProviderInitializer;
        bool UnRegisterViewSource(IView prefab);

        void InstantiateView(IView prefab, Entity entity);
        void InstantiateView(ViewId prefabSourceId, Entity entity);
        void DestroyView(ref IView instance);

    }

    public class ViewRegistryNotFoundException : System.Exception {

        public ViewRegistryNotFoundException(ViewId sourceViewId) : base("[Views] View with id " + sourceViewId.ToString() +
                                                                         " not found in registry. Have you called RegisterViewSource()?") { }

    }

    public interface IViewComponent {

        ref ViewInfo GetViewInfo();

    }

    #if UNITY_ENABLED
    public abstract class ViewBase : UnityEngine.MonoBehaviour { }
    #endif

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    [System.Serializable]
    public readonly struct ViewInfo : System.IEquatable<ViewInfo> {

        public readonly Entity entity;
        public readonly ViewId prefabSourceId;
        public readonly Tick creationTick;

        public ViewInfo(Entity entity, ViewId prefabSourceId, Tick creationTick) {

            this.entity = entity;
            this.prefabSourceId = prefabSourceId;
            this.creationTick = creationTick;

        }

        public ulong GetKey() {

            return MathUtils.GetKey((uint)this.entity.id, this.prefabSourceId.v);

        }

        public override int GetHashCode() {

            return
                this.entity.id ^ this.prefabSourceId
                                     .GetHashCode(); //(int)MathUtils.GetKey(this.entity.id, this.prefabSourceId.GetHashCode()/* ^ this.creationTick.GetHashCode()*/);

        }

        public override bool Equals(object obj) {

            if (obj is ViewInfo ent) {
                
                return this.Equals(ent);
                
            }

            return false;
            
        }

        public bool Equals(ViewInfo p) {

            return /*this.creationTick == p.creationTick &&*/ this.entity.id == p.entity.id && this.prefabSourceId == p.prefabSourceId;

        }

        public override string ToString() {

            return this.entity.ToString() + "\nPrefab Source Id: " + this.prefabSourceId.ToString() + "\nCreation Tick: " + this.creationTick.ToString();

        }

    }

    /// <summary>
    /// Private component class to describe Views
    /// </summary>
    public struct ViewComponent : IStructComponent {

        public ViewInfo viewInfo;
        public uint seed;

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct Views {

        public IView mainView;
        public bool isNotEmpty;

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public int Length {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                var count = 0;
                if (this.mainView != null) ++count;
                return count;
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        public IView this[int i] {
            #if INLINE_METHODS
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            #endif
            get {
                return this.mainView;
            }
        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Add(IView view) {

            if (this.mainView == null) {

                this.mainView = view;

            }

            this.isNotEmpty = true;

        }

        #if ECS_COMPILE_IL2CPP_OPTIONS
        [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
         Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
        #endif
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool Remove(IView view) {

            if (this.mainView == view) {

                this.mainView = null;
                this.isNotEmpty = false;
                return true;

            }

            return false;

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial class ViewsModule : IViewModule, IUpdatePost, IModulePhysicsUpdate {

        internal struct ProviderInfo {

            public IViewsProvider provider;
            public IViewsProviderInitializer initializer;

        }

        private const int REGISTRY_PROVIDERS_CAPACITY = 100;
        private const int REGISTRY_CAPACITY = 100;
        private const int VIEWS_CAPACITY = 1000;

        private BufferArray<Views> list;
        private HashSet<ViewInfo> rendering;
        private Dictionary<ViewId, IViewsProviderInitializerBase> registryPrefabToProviderInitializer;
        private Dictionary<ViewId, IViewsProvider> registryPrefabToProvider;
        private Dictionary<System.Type, ProviderInfo> registryTypeToProviderInfo;
        private Dictionary<IView, ViewId> registryPrefabToId;
        private Dictionary<ViewId, IView> registryIdToPrefab;
        private ViewId viewSourceIdRegistry;
        private bool isRequestsDirty;
        private bool forceUpdateState;

        public World world { get; set; }

        void IModuleBase.OnConstruct() {

            this.viewSourceIdRegistry = ViewId.Zero;
            this.isRequestsDirty = false;
            this.forceUpdateState = false;
            this.list = PoolArray<Views>.Spawn(ViewsModule.VIEWS_CAPACITY);
            this.rendering = PoolHashSet<ViewInfo>.Spawn(ViewsModule.VIEWS_CAPACITY);
            this.registryPrefabToId = PoolDictionary<IView, ViewId>.Spawn(ViewsModule.REGISTRY_CAPACITY);
            this.registryIdToPrefab = PoolDictionary<ViewId, IView>.Spawn(ViewsModule.REGISTRY_CAPACITY);
            this.registryTypeToProviderInfo = PoolDictionary<System.Type, ProviderInfo>.Spawn(ViewsModule.REGISTRY_CAPACITY);

            this.registryPrefabToProvider = PoolDictionary<ViewId, IViewsProvider>.Spawn(ViewsModule.REGISTRY_PROVIDERS_CAPACITY);
            this.registryPrefabToProviderInitializer = PoolDictionary<ViewId, IViewsProviderInitializerBase>.Spawn(ViewsModule.REGISTRY_PROVIDERS_CAPACITY);

            WorldUtilities.SetAllComponentInHash<ViewComponent>(false);

        }

        void IModuleBase.OnDeconstruct() {

            this.isRequestsDirty = true;
            this.forceUpdateState = true;
            this.UpdateRequests();

            var temp = PoolListCopyable<IView>.Spawn(this.registryPrefabToId.Count);
            foreach (var prefab in this.registryIdToPrefab) {

                temp.Add(prefab.Value);

            }

            foreach (var prefab in temp) {

                this.UnRegisterViewSource(prefab);

            }

            PoolListCopyable<IView>.Recycle(ref temp);

            foreach (var item in this.registryTypeToProviderInfo) {

                var info = item.Value;
                info.provider.world = null;
                info.provider.OnDeconstruct();
                info.initializer.Destroy(info.provider);

            }

            PoolDictionary<System.Type, ProviderInfo>.Recycle(ref this.registryTypeToProviderInfo);
            PoolDictionary<ViewId, IViewsProvider>.Recycle(ref this.registryPrefabToProvider);
            PoolDictionary<ViewId, IViewsProviderInitializerBase>.Recycle(ref this.registryPrefabToProviderInitializer);

            PoolDictionary<ViewId, IView>.Recycle(ref this.registryIdToPrefab);
            PoolDictionary<IView, ViewId>.Recycle(ref this.registryPrefabToId);

            PoolHashSet<ViewInfo>.Recycle(ref this.rendering);
            PoolArray<Views>.Recycle(ref this.list);

            this.viewSourceIdRegistry = default;

        }

        BufferArray<Views> IViewModuleBase.GetData() {

            return this.list;

        }

        HashSet<ViewInfo> IViewModuleBase.GetRendering() {

            return this.rendering;

        }

        System.Collections.IDictionary IViewModuleBase.GetViewSourceData() {

            return this.registryIdToPrefab;

        }

        IViewsProviderBase IViewModuleBase.GetViewSourceProvider(ViewId viewSourceId) {

            IViewsProvider provider;
            if (this.registryPrefabToProvider.TryGetValue(viewSourceId, out provider) == true) {

                return provider;

            }

            return null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(IView prefab, Entity entity) {

            this.InstantiateView(this.GetViewSourceId(prefab), entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(ViewId sourceId, Entity entity) {

            if (this.world.settings.turnOffViews == true) return;

            // Called by tick system
            if (this.world.HasStep(WorldStep.LogicTick) == false && this.world.HasResetState() == true) {

                throw new OutOfStateException();

            }

            if (this.registryIdToPrefab.ContainsKey(sourceId) == false) {

                throw new ViewRegistryNotFoundException(sourceId);

            }

            if (this.world.HasData<ViewComponent>(in entity) == true) {

                throw new System.Exception($"View is already exist on entity {entity}");

            }

            var viewInfo = new ViewInfo(entity, sourceId, this.world.GetStateTick());
            var view = new ViewComponent() {
                viewInfo = viewInfo,
                seed = (uint)this.world.GetSeedValue(),
            };
            this.world.SetData(in entity, view);

            if (this.world.HasResetState() == false) {

                this.CreateVisualInstance(in view.seed, in view.viewInfo);

            }

            this.isRequestsDirty = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void DestroyView(ref IView instance) {

            if (this.world.settings.turnOffViews == true) return;

            // Called by tick system
            if (this.world.HasStep(WorldStep.LogicTick) == false && this.world.HasResetState() == true) {

                throw new OutOfStateException();

            }

            if (instance.entity.IsAlive() == true) {

                ref readonly var view = ref instance.entity.Read<ViewComponent>();
                if (view.viewInfo.creationTick == instance.creationTick &&
                    view.viewInfo.prefabSourceId == instance.prefabSourceId &&
                    view.viewInfo.entity == instance.entity) {

                    instance.entity.Remove<ViewComponent>();

                }

            }

            this.isRequestsDirty = true;

            instance = null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private IView SpawnView_INTERNAL(ViewInfo viewInfo) {

            IViewsProvider provider;
            if (this.registryPrefabToProvider.TryGetValue(viewInfo.prefabSourceId, out provider) == true) {

                var instance = provider.Spawn(this.GetViewSource(viewInfo.prefabSourceId), viewInfo.prefabSourceId, in viewInfo.entity);
                ME.WeakRef.Reg(instance);
                var instanceInternal = (IViewBaseInternal)instance;
                instanceInternal.Setup(this.world, viewInfo);
                this.Register(instance);

                return instance;

            }

            return null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void RecycleView_INTERNAL(ref IView instance) {

            var viewInstance = instance;
            this.UnRegister(instance);

            if (this.registryPrefabToProvider.TryGetValue(viewInstance.prefabSourceId, out var provider) == true) {

                provider.Destroy(ref viewInstance);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void DestroyAllViews(in Entity entity) {

            if (this.world.isActive == false) return;

            if (this.world.HasResetState() == false && entity.Has<ViewComponent>() == true) {

                for (var id = this.list.Length - 1; id >= 0; --id) {

                    ref var views = ref this.list.arr[id];
                    var currentViewInstance = views.mainView;
                    if (currentViewInstance == null) continue;

                    if (currentViewInstance.entity == entity) {

                        // Entity has dead
                        this.RecycleView_INTERNAL(ref currentViewInstance);
                        break;

                    }

                }

            }
            
            entity.Remove<ViewComponent>();
            this.isRequestsDirty = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public IView GetViewSource(ViewId viewSourceId) {

            if (this.registryIdToPrefab.TryGetValue(viewSourceId, out var prefab) == true) {

                return prefab;

            }

            return null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId GetViewSourceId(IView prefab) {

            if (this.registryPrefabToId.TryGetValue(prefab, out var viewId) == true) {

                return viewId;

            }

            return ViewId.Zero;

        }

        private void RegisterViewSource_INTERNAL<TProvider>(TProvider providerInitializer) where TProvider : struct, IViewsProviderInitializer {
            
            ++this.viewSourceIdRegistry;

            if (this.registryTypeToProviderInfo.TryGetValue(typeof(TProvider), out var existProvider) == true) {
                
                this.registryPrefabToProvider.Add(this.viewSourceIdRegistry, existProvider.provider);
                this.registryPrefabToProviderInitializer.Add(this.viewSourceIdRegistry, existProvider.initializer);
                
            } else {

                var viewsProviderInitializer = (IViewsProviderInitializer)providerInitializer;
                var provider = viewsProviderInitializer.Create();
                provider.world = this.world;
                provider.OnConstruct();
                this.registryPrefabToProvider.Add(this.viewSourceIdRegistry, provider);
                this.registryPrefabToProviderInitializer.Add(this.viewSourceIdRegistry, viewsProviderInitializer);
                this.registryTypeToProviderInfo.Add(typeof(TProvider), new ProviderInfo() {
                    provider = provider,
                    initializer = viewsProviderInitializer,
                });

            }

        }

        public ViewId RegisterViewSource<TProvider>(TProvider providerInitializer, IView prefab) where TProvider : struct, IViewsProviderInitializer {

            if (prefab == null) {

                ViewSourceIsNullException.Throw();

            }

            if (this.registryPrefabToId.TryGetValue(prefab, out var viewId) == true) {

                return viewId;

            }

            #if VIEWS_REGISTER_VIEW_SOURCE_CHECK_STATE
            if (this.world.HasStep(WorldStep.LogicTick) == true) {

                throw new InStateException();

            }
            #endif

            this.RegisterViewSource_INTERNAL(providerInitializer);
            this.registryPrefabToId.Add(prefab, this.viewSourceIdRegistry);
            this.registryIdToPrefab.Add(this.viewSourceIdRegistry, prefab);

            return this.viewSourceIdRegistry;

        }

        public bool UnRegisterViewSource(IView prefab) {

            if (prefab == null) {

                ViewSourceIsNullException.Throw();

            }

            if (this.world.isActive == true && this.world.HasStep(WorldStep.LogicTick) == true) {

                throw new InStateException();

            }

            if (this.registryPrefabToId.TryGetValue(prefab, out var viewId) == true) {

                this.registryPrefabToProviderInitializer.Remove(viewId);
                this.registryPrefabToProvider.Remove(viewId);
                this.registryPrefabToId.Remove(prefab);
                return this.registryIdToPrefab.Remove(viewId);

            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void Register(IView instance) {

            var id = instance.entity.id;
            ArrayUtils.Resize(id, ref this.list);

            this.list.arr[id].Add(instance);

            var viewInfo = new ViewInfo(instance.entity, instance.prefabSourceId, instance.creationTick);
            this.rendering.Add(viewInfo);

            instance.DoInitialize();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool UnRegister(IView instance) {

            if (instance != null) instance.DoDeInitialize();

            var viewInfo = new ViewInfo(instance.entity, instance.prefabSourceId, instance.creationTick);
            if (this.rendering.Remove(viewInfo) == true) {

                var id = instance.entity.id;
                this.list.arr[id].Remove(instance);
                return true;

            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void CreateVisualInstance(in uint seed, in ViewInfo viewInfo) {

            if (viewInfo.entity.IsAlive() == false) return;

            var instance = this.SpawnView_INTERNAL(viewInfo);
            if (instance == null) {

                UnityEngine.Debug.LogError("CreateVisualInstance failed while viewInfo.prefabSourceId: " + viewInfo.prefabSourceId + " and contains: " +
                                           this.registryPrefabToProvider.ContainsKey(viewInfo.prefabSourceId));

            }

            // Call ApplyState with deltaTime = current time offset
            var dt = UnityEngine.Mathf.Max(0f, (this.world.GetCurrentTick() - viewInfo.creationTick) * this.world.GetTickTime());
            instance.entityVersion = viewInfo.entity.GetVersion();
            instance.ApplyState(dt, immediately: true);
            // Simulate particle systems
            instance.SimulateParticles(dt, seed);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool IsRenderingNow(in ViewInfo viewInfo) {

            /*foreach (var item in this.rendering) {

                if (item.Equals(viewInfo) == true) {

                    return true;

                }
                
            }*/

            return this.rendering.Contains(viewInfo);

        }

        public void SetRequestsAsDirty() {

            this.isRequestsDirty = true;

        }

        public void SetUpdateStateAsDirty() {

            this.forceUpdateState = true;

        }

        //private HashSet<ViewInfo> prevList = new HashSet<ViewInfo>();
        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool UpdateRequests() {

            if (this.isRequestsDirty == false) return false;
            this.isRequestsDirty = false;

            var hasChanged = false;

            if (this.world.currentState == null) {
                
                // Recycle all views
                for (var id = this.list.Length - 1; id >= 0; --id) {
                    
                    ref var views = ref this.list.arr[id];
                    var currentViewInstance = views.mainView;
                    if (currentViewInstance == null) continue;
                    
                    this.RecycleView_INTERNAL(ref currentViewInstance);
                    
                }

                return true;

            }

            // Recycle all views that doesn't required
            for (var id = this.list.Length - 1; id >= 0; --id) {

                ref var views = ref this.list.arr[id];
                var currentViewInstance = views.mainView;
                if (currentViewInstance == null) continue;

                var entity = currentViewInstance.entity;
                if (entity.IsAliveWithBoundsCheck() == false) {

                    // Entity has dead
                    this.RecycleView_INTERNAL(ref currentViewInstance);
                    hasChanged = true;

                } else {

                    // If entity is alive - check if view has changed
                    ref readonly var view = ref currentViewInstance.entity.Read<ViewComponent>();
                    if (currentViewInstance.prefabSourceId != view.viewInfo.prefabSourceId) {

                        // Destroy current view
                        this.RecycleView_INTERNAL(ref currentViewInstance);
                        hasChanged = true;

                    }

                }

            }


            var allEntities = this.world.GetAliveEntities();
            if (allEntities != null) {
                
                for (int j = 0; j < allEntities.Count; ++j) {

                    ref var entityId = ref allEntities[j];

                    var ent = this.world.GetEntityById(entityId);
                    ref readonly var view = ref ent.Read<ViewComponent>();
                    if (view.viewInfo.entity != Entity.Empty) {

                        if (this.IsRenderingNow(in view.viewInfo) == true) {

                            // is rendering now - skip

                        } else {

                            // is not rendering now
                            // create required instance
                            this.CreateVisualInstance(in view.seed, in view.viewInfo);
                            hasChanged = true;

                        }

                    }

                }
                
            }

            return hasChanged;

        }

        void IModulePhysicsUpdate.UpdatePhysics(float deltaTime) {

            for (var id = 0; id < this.list.Length; ++id) {

                ref var views = ref this.list.arr[id];
                var currentViewInstance = views.mainView;
                if (currentViewInstance == null) continue;
                if (currentViewInstance.entity.IsAliveWithBoundsCheck() == false) continue;

                currentViewInstance.ApplyPhysicsState(deltaTime);

            }

        }

        public void UpdatePost(in float deltaTime) {

            if (this.world.settings.turnOffViews == true) return;

            var hasChanged = this.UpdateRequests();
            if (this.world.currentState != null) {

                for (var id = this.list.Length - 1; id >= 0; --id) {

                    ref var views = ref this.list.arr[id];
                    var currentViewInstance = views.mainView;
                    if (currentViewInstance == null) continue;
                    if (currentViewInstance.entity.IsAliveWithBoundsCheck() == false) continue;

                    var version = currentViewInstance.entity.GetVersion();
                    if (this.forceUpdateState == true || version != currentViewInstance.entityVersion) {

                        currentViewInstance.entityVersion = version;
                        currentViewInstance.ApplyState(deltaTime, immediately: false);

                    }

                    currentViewInstance.OnUpdate(deltaTime);
                    currentViewInstance.UpdateParticlesSimulation(deltaTime);

                }

                this.forceUpdateState = false;

            }

            // Update providers
            foreach (var providerKv in this.registryPrefabToProvider) {

                providerKv.Value.Update(this.list, deltaTime, hasChanged);

            }

        }

    }

}
#endif