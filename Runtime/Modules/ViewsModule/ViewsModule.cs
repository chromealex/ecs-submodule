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

    public enum DestroyViewBehaviour : byte {

        DestroyWithEntity = 0,
        LeaveOnScene = 1,

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

            if (entity.Has<ViewComponent>() == true) {
                ref var view = ref entity.Get<ViewComponent>();
                if (view.viewInfo.prefabSourceId != viewId) {
                    view.viewInfo = new ViewInfo(entity, viewId, Worlds.currentWorld.GetStateTick(), view.viewInfo.destroyViewBehaviour);
                }
            } else {
                Worlds.currentWorld.InstantiateView(viewId, entity);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void ReplaceView(this Entity entity, IView prefab) {

            if (entity.Has<ViewComponent>() == true) Worlds.currentWorld.DestroyAllViews(in entity);
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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void AssignView(this Entity entity, ViewId viewId, DestroyViewBehaviour destroyViewBehaviour) {

            Worlds.currentWorld.AssignView(viewId, entity, destroyViewBehaviour);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void AssignView(this Entity entity, IView prefab, DestroyViewBehaviour destroyViewBehaviour) {

            Worlds.currentWorld.AssignView(prefab, entity, destroyViewBehaviour);

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
        public ViewId RegisterViewSource<TProvider>(TProvider providerInitializer, IView prefab, ViewId customId) where TProvider : struct, IViewsProviderInitializer {

            var viewsModule = this.GetModule<ViewsModule>();
            return viewsModule.RegisterViewSource(providerInitializer, ViewsModule.ViewSourceObject.Create(prefab), customId);

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
        public void AssignView(IView prefab, Entity entity, DestroyViewBehaviour destroyViewBehaviour) {

            var viewsModule = this.GetModule<ViewsModule>();
            viewsModule.AssignView(prefab, entity, destroyViewBehaviour);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void AssignView(ViewId prefab, Entity entity, DestroyViewBehaviour destroyViewBehaviour) {

            var viewsModule = this.GetModule<ViewsModule>();
            viewsModule.AssignView(prefab, entity, destroyViewBehaviour);

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
        public ViewId RegisterViewSourceShared<TProvider>(TProvider providerInitializer, IView prefab, ViewId customId = default) where TProvider : struct, IViewsProviderInitializer {

            return this.RegisterViewSource<TProvider>(providerInitializer, prefab, customId);

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
        uint entityVersion { get; set; }
        Entity entity { get; }
        ViewInfo info { get; }

    }

    public interface IViewRespawnTime {

        float respawnTime { get; set; }
        float cacheTimeout { get; }
        bool useCache { get; }

    }
    
    public interface IViewDestroyTime {

        float despawnDelay { get; }
        
    }

    public interface IViewPool {

        uint customViewId { get; }

    }

    public interface IView : IViewBase, System.IComparable<IView> {

        void DoInitialize();
        void DoDeInitialize();
        void DoDestroy();
        void ApplyState(float deltaTime, bool immediately);
        void ApplyPhysicsState(float deltaTime);
        void DoUpdate(float deltaTime);

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

        void Register(IView instance, ViewInfo viewInfo);
        bool UnRegister(IView instance);

        ViewId RegisterViewSource<TProvider>(TProvider providerInitializer, ViewsModule.ViewSourceObject viewSourceObject, ViewId customId) where TProvider : struct, IViewsProviderInitializer;
        bool UnRegisterViewSource(IView prefab);

        void AssignView(IView prefab, in Entity entity, DestroyViewBehaviour destroyViewBehaviour = DestroyViewBehaviour.DestroyWithEntity);
        void AssignView(ViewId prefabSourceId, in Entity entity, DestroyViewBehaviour destroyViewBehaviour = DestroyViewBehaviour.DestroyWithEntity);
        void InstantiateView(IView prefab, in Entity entity);
        void InstantiateView(ViewId prefabSourceId, in Entity entity);
        void DestroyView(ref IView instance);

        IView GetViewByEntity(in Entity entity);

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
        public readonly byte destroyViewBehaviourStorage;
        public DestroyViewBehaviour destroyViewBehaviour => (DestroyViewBehaviour)this.destroyViewBehaviourStorage;
        
        public ViewInfo(Entity entity, ViewId prefabSourceId, Tick creationTick, DestroyViewBehaviour destroyViewBehaviour) {

            this.entity = entity;
            this.prefabSourceId = prefabSourceId;
            this.creationTick = creationTick;
            this.destroyViewBehaviourStorage = (byte)destroyViewBehaviour;

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
    /// Component to describe Views
    /// </summary>
    [ComponentGroup("Views", GroupColor.Magenta, -900)]
    [ComponentOrder(1)]
    public struct ViewComponent : IComponent, IComponentRuntime {

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
        private Dictionary<ViewSourceObject, ViewId> registryLoadingAwaitSourceToId;
        private Dictionary<ViewId, ViewSourceObject> registryLoadingAwaitIdToSource;
        private ViewId viewSourceIdRegistry;
        private bool isRequestsDirty;
        private bool forceUpdateState;
        private Tick previousTick;

        public World world { get; set; }

        void IModuleBase.OnConstruct() {

            this.viewSourceIdRegistry = ViewId.Zero;
            this.isRequestsDirty = false;
            this.forceUpdateState = false;
            this.list = PoolArray<Views>.Spawn(ViewsModule.VIEWS_CAPACITY);
            this.rendering = PoolHashSet<ViewInfo>.Spawn(ViewsModule.VIEWS_CAPACITY);
            this.registryLoadingAwaitSourceToId = PoolDictionary<ViewSourceObject, ViewId>.Spawn(ViewsModule.REGISTRY_CAPACITY);
            this.registryLoadingAwaitIdToSource = PoolDictionary<ViewId, ViewSourceObject>.Spawn(ViewsModule.REGISTRY_CAPACITY);
            this.registryPrefabToId = PoolDictionary<IView, ViewId>.Spawn(ViewsModule.REGISTRY_CAPACITY);
            this.registryIdToPrefab = PoolDictionary<ViewId, IView>.Spawn(ViewsModule.REGISTRY_CAPACITY);
            this.registryTypeToProviderInfo = PoolDictionary<System.Type, ProviderInfo>.Spawn(ViewsModule.REGISTRY_CAPACITY);

            this.registryPrefabToProvider = PoolDictionary<ViewId, IViewsProvider>.Spawn(ViewsModule.REGISTRY_PROVIDERS_CAPACITY);
            this.registryPrefabToProviderInitializer = PoolDictionary<ViewId, IViewsProviderInitializerBase>.Spawn(ViewsModule.REGISTRY_PROVIDERS_CAPACITY);
            
            this.previousTick = Tick.Zero;

            WorldUtilities.SetAllComponentInHash<ViewComponent>(false);

        }

        void IModuleBase.OnDeconstruct() {

            this.isRequestsDirty = true;
            this.forceUpdateState = true;
            //this.UpdateRequests();

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
            
            this.previousTick = Tick.Zero;

        }
        
        IView IViewModule.GetViewByEntity(in Entity entity) {

            var id = entity.id;
            if (id < 0 || id >= this.list.Length) return null;
            return this.list.arr[id].mainView;

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
        public void UnassignView(ref IView instance) {
            
            if (this.world.settings.turnOffViews == true) return;
            
            this.DeInitialize(instance);
            this.UnRegister(instance);

            ((IViewBaseInternal)instance).Setup(this.world, default);
            instance = null;
            this.isRequestsDirty = true;
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void AssignView(IView sceneSource, in Entity entity, DestroyViewBehaviour destroyViewBehaviour) {

            this.AssignView(this.GetViewSourceId(sceneSource), entity, destroyViewBehaviour);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void AssignView(ViewId sourceId, in Entity entity, DestroyViewBehaviour destroyViewBehaviour) {

            #if UNITY_EDITOR
            // [Editor-Only] Check if sourceId is scene object
            var editorSource = this.GetViewSource(sourceId);
            if (editorSource is UnityEngine.Object editorSourceObj) {
                var status = UnityEditor.PrefabUtility.IsPartOfAnyPrefab(editorSourceObj);
                if (status == true) {

                    // Source is a prefab
                    throw new System.Exception($"View {editorSource} must be a scene instance");

                }
            }
            #endif
            
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

            var viewInfo = new ViewInfo(entity, sourceId, this.world.GetStateTick(), destroyViewBehaviour);

            var view = new ViewComponent() {
                viewInfo = viewInfo,
                seed = this.world.GetSeed(),
            };
            this.world.SetData(in entity, view);
            var instance = this.GetViewSource(sourceId);
            this.Register(instance, viewInfo);
            
            if (this.world.HasResetState() == false) {

                this.UpdateView(instance, view.seed, in viewInfo);

            }

            this.isRequestsDirty = true;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(IView prefab, in Entity entity) {

            this.InstantiateView(this.GetViewSourceId(prefab), entity);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(ViewId sourceId, in Entity entity) {

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

            var viewInfo = new ViewInfo(entity, sourceId, this.world.GetStateTick(), DestroyViewBehaviour.DestroyWithEntity);
            var view = new ViewComponent() {
                viewInfo = viewInfo,
                seed = this.world.GetSeed(),
            };
            this.world.SetData(in entity, view);

            if (this.world.HasResetState() == false) {

                this.CreateVisualInstance(view.seed, in view.viewInfo);

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

            if (instance.info.entity.IsAlive() == true) {

                ref readonly var view = ref instance.info.entity.Read<ViewComponent>();
                if (view.viewInfo.creationTick == instance.info.creationTick &&
                    view.viewInfo.prefabSourceId == instance.info.prefabSourceId &&
                    view.viewInfo.entity == instance.info.entity) {

                    instance.info.entity.Remove<ViewComponent>();

                }

            }

            this.isRequestsDirty = true;

            instance = null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void DestroyAllViews(in Entity entity) {

            if (this.world.isActive == false) return;

            // Called by tick system
            if (this.world.HasStep(WorldStep.LogicTick) == false && this.world.HasResetState() == true) {

                throw new OutOfStateException();

            }

            if (this.world.HasResetState() == false && entity.Has<ViewComponent>() == true) {

                for (var id = this.list.Length - 1; id >= 0; --id) {

                    ref var views = ref this.list.arr[id];
                    var currentViewInstance = views.mainView;
                    if (currentViewInstance == null) continue;

                    if (currentViewInstance.info.entity == entity) {

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
        private IView SpawnView_INTERNAL(ViewInfo viewInfo) {

            if (this.registryPrefabToProvider.TryGetValue(viewInfo.prefabSourceId, out var provider) == true) {

                var instance = provider.Spawn(this.GetViewSource(viewInfo.prefabSourceId), viewInfo.prefabSourceId, in viewInfo.entity);
                ME.WeakRef.Reg(instance);
                this.Register(instance, viewInfo);

                return instance;

            }

            return null;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void RecycleView_INTERNAL(ref IView instance) {

            if (instance.info.destroyViewBehaviour == DestroyViewBehaviour.LeaveOnScene) {

                // Just unassign view
                this.UnassignView(ref instance);
                return;

            }

            var viewInstance = instance;
            if (this.registryPrefabToProvider.TryGetValue(viewInstance.info.prefabSourceId, out var provider) == true) {

                if (provider.Destroy(ref viewInstance) == true) {

                    // Immediately destroy
                    this.DoDestroy(instance);
                    this.DeInitialize(instance);
                    this.UnRegister(instance);

                } else {

                    // Delayed destroy - DeInitialize will be called manually later
                    this.DoDestroy(instance);
                    this.UnRegister(instance);

                }

            }

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

        private void RegisterViewSource_INTERNAL<TProvider>(TProvider providerInitializer, ViewId customId) where TProvider : struct, IViewsProviderInitializer {
            
            if (customId == 0) customId = ++this.viewSourceIdRegistry;
            while (this.registryPrefabToProvider.ContainsKey(customId) == true) {
                customId = ++this.viewSourceIdRegistry;
            }
            
            this.viewSourceIdRegistry = customId;

            if (this.registryTypeToProviderInfo.TryGetValue(typeof(TProvider), out var existProvider) == true) {
                
                this.registryPrefabToProvider.Add(customId, existProvider.provider);
                this.registryPrefabToProviderInitializer.Add(customId, existProvider.initializer);
                
            } else {

                var viewsProviderInitializer = (IViewsProviderInitializer)providerInitializer;
                var provider = viewsProviderInitializer.Create();
                provider.world = this.world;
                provider.OnConstruct();
                this.registryPrefabToProvider.Add(customId, provider);
                this.registryPrefabToProviderInitializer.Add(customId, viewsProviderInitializer);
                this.registryTypeToProviderInfo.Add(typeof(TProvider), new ProviderInfo() {
                    provider = provider,
                    initializer = viewsProviderInitializer,
                });

            }

        }

        public struct ViewSourceObject : System.IEquatable<ViewSourceObject> {

            private IView view;
            private System.Collections.IEnumerator op;

            public static ViewSourceObject Create(IView view) {
                
                return new ViewSourceObject() {
                    view = view,
                };
                
            }

            public static ViewSourceObject Create(System.Collections.IEnumerator op) {
                
                return new ViewSourceObject() {
                    op = op,
                };
                
            }

            public IView GetResult() => this.view;

            public bool IsDone() {
                
                this.Validate();
                return this.view != null || this.op == null || this.op.MoveNext() == false;
                
            }
            public bool IsValid() => this.view != null || this.op != null;

            private void Validate() {

                if (this.view != null) return;
                if (this.op != null && this.IsDone() == true) {

                    this.view = (IView)this.op.Current;
                    this.op = null;
                    
                }
                
            }

            public override int GetHashCode() {

                if (this.IsValid() == false) return 0;
                return this.view != null ? this.view.GetHashCode() : this.op.GetHashCode();
                
            }
            
            public bool Equals(ViewSourceObject other) {
                return Equals(this.view, other.view) && Equals(this.op, other.op);
            }

        }

        public ViewId RegisterViewSource<TProvider>(TProvider providerInitializer, ViewSourceObject prefabSource, ViewId customId) where TProvider : struct, IViewsProviderInitializer {

            if (prefabSource.IsValid() == false) {

                ViewSourceIsNullException.Throw();

            }

            if (prefabSource.IsDone() == true) {

                // Prefab is ready to use (already loaded)
                // so we can use prefab as IView to register
                var prefab = prefabSource.GetResult();
                if (this.registryPrefabToId.TryGetValue(prefab, out var viewId) == true) {

                    return viewId;

                }

                #if VIEWS_REGISTER_VIEW_SOURCE_CHECK_STATE
                E.IS_NOT_LOGIC_STEP(this.world);
                #endif

                this.RegisterViewSource_INTERNAL(providerInitializer, customId);
                this.registryPrefabToId.Add(prefab, this.viewSourceIdRegistry);
                this.registryIdToPrefab.Add(this.viewSourceIdRegistry, prefab);

            } else {
                
                // Prefab was not loaded yet
                // We need to register handler for now
                // And wait until it will be done
                if (this.registryLoadingAwaitSourceToId.TryGetValue(prefabSource, out var viewId) == true) {

                    return viewId;

                }
                
                #if VIEWS_REGISTER_VIEW_SOURCE_CHECK_STATE
                E.IS_NOT_LOGIC_STEP(this.world);
                #endif

                this.RegisterViewSource_INTERNAL(providerInitializer, customId);
                this.registryLoadingAwaitSourceToId.Add(prefabSource, this.viewSourceIdRegistry);
                this.registryLoadingAwaitIdToSource.Add(this.viewSourceIdRegistry, prefabSource);
                
            }
            
            return this.viewSourceIdRegistry;

        }

        public bool UnRegisterViewSource(IView prefab) {

            if (prefab == null) {

                ViewSourceIsNullException.Throw();
                return false;

            }

            E.IS_LOGIC_STEP(this.world);
            
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
        public void Register(IView instance, ViewInfo info) {
            
            var id = info.entity.id;
            ArrayUtils.Resize(id, ref this.list);

            this.list.arr[id].Add(instance);

            this.rendering.Add(info);

            var instanceInternal = (IViewBaseInternal)instance;
            instanceInternal.Setup(this.world, info);

            instance.DoInitialize();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public bool UnRegister(IView instance) {

            var viewInfo = new ViewInfo(instance.entity, instance.info.prefabSourceId, instance.info.creationTick, instance.info.destroyViewBehaviour);
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
        public void DeInitialize(IView instance) {

            if (instance != null) instance.DoDeInitialize();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void DoDestroy(IView instance) {

            if (instance != null) instance.DoDestroy();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void CreateVisualInstance(uint seed, in ViewInfo viewInfo) {

            if (viewInfo.entity.IsAlive() == false) return;

            var instance = this.SpawnView_INTERNAL(viewInfo);
            if (instance == null) {

                UnityEngine.Debug.LogError("CreateVisualInstance failed while viewInfo.prefabSourceId: " + viewInfo.prefabSourceId + " and contains: " +
                                           this.registryPrefabToProvider.ContainsKey(viewInfo.prefabSourceId));

            }

            this.UpdateView(instance, seed, in viewInfo);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private void UpdateView(IView instance, in uint seed, in ViewInfo viewInfo) {
            
            // Call ApplyState with deltaTime = current time offset
            var dt = UnityEngine.Mathf.Max(0f, (float)(long)(this.world.GetCurrentTick() - viewInfo.creationTick) * (float)this.world.GetTickTime());
            instance.entityVersion = viewInfo.entity.GetVersion();
            instance.ApplyState(dt, immediately: true);
            // Simulate particle systems
            instance.SimulateParticles(dt, seed);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private bool IsRenderingNow(in ViewInfo viewInfo) {

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
                    if (currentViewInstance.info.prefabSourceId != view.viewInfo.prefabSourceId) {

                        // Destroy current view
                        this.RecycleView_INTERNAL(ref currentViewInstance);
                        hasChanged = true;

                    }

                }

            }
            
            var allEntities = this.world.GetAliveEntities();
            if (allEntities.isCreated == true) {
                
                for (int j = 0; j < allEntities.Count; ++j) {

                    ref var entityId = ref allEntities[in this.world.currentState.allocator, j];

                    var ent = this.world.GetEntityById(entityId);
                    ref readonly var view = ref ent.Read<ViewComponent>();
                    if (view.viewInfo.entity != Entity.Empty) {

                        if (this.IsRenderingNow(in view.viewInfo) == true) {

                            // is rendering now - skip

                        } else {

                            // is not rendering now

                            if (this.registryLoadingAwaitIdToSource.TryGetValue(view.viewInfo.prefabSourceId, out var prefabSource) == true) {

                                if (prefabSource.IsDone() == false) {
                                    
                                    // Source is not loaded yet
                                    // Set requests as dirty
                                    this.isRequestsDirty = true;
                                    continue;

                                } else {
                                    
                                    // Source is just loaded
                                    
                                    // Add to registry
                                    this.registryIdToPrefab.Add(view.viewInfo.prefabSourceId, prefabSource.GetResult());
                                    this.registryPrefabToId.Add(prefabSource.GetResult(), view.viewInfo.prefabSourceId);
                                    
                                    // Remove from loading
                                    this.registryLoadingAwaitIdToSource.Remove(view.viewInfo.prefabSourceId);
                                    this.registryLoadingAwaitSourceToId.Remove(prefabSource);

                                }
                                
                            }
                            
                            // create required instance
                            this.CreateVisualInstance(view.seed, in view.viewInfo);
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
            //if (this.world.IsReverting() == true) return;

            var currentTick = this.world.GetCurrentTick();
            if (this.previousTick > currentTick) return;
            this.previousTick = currentTick;

            var hasChanged = this.UpdateRequests();
            if (this.world.currentState != null) {

                for (var id = this.list.Length - 1; id >= 0; --id) {

                    ref var views = ref this.list.arr[id];
                    var currentViewInstance = views.mainView;
                    if (currentViewInstance == null) continue;
                    if (currentViewInstance.entity.IsAliveWithBoundsCheck() == false) continue;

                    var version = WorldStaticCallbacks.GetEntityVersion(currentViewInstance.entity);
                    if (this.forceUpdateState == true || version != currentViewInstance.entityVersion) {

                        currentViewInstance.entityVersion = version;
                        currentViewInstance.ApplyState(deltaTime, immediately: false);

                    }

                    currentViewInstance.DoUpdate(deltaTime);
                    currentViewInstance.UpdateParticlesSimulation(deltaTime);

                }

                this.forceUpdateState = false;

            }

            // Update providers
            foreach (var providerKv in this.registryTypeToProviderInfo) {

                providerKv.Value.provider.Update(this, this.list, deltaTime, hasChanged);

            }

        }

    }

}
#endif
