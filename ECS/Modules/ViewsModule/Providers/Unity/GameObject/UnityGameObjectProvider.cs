#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if GAMEOBJECT_VIEWS_MODULE_SUPPORT
namespace ME.ECS {

    using ME.ECS.Views;
    using ME.ECS.Views.Providers;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial struct WorldViewsSettings {

        public bool unityGameObjectProviderDisableJobs;

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial struct WorldDebugViewsSettings {

        public bool unityGameObjectProviderShowOnScene;

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
        public ViewId RegisterViewSource(UnityEngine.GameObject prefab) {

            return this.RegisterViewSource(new UnityGameObjectProviderInitializer(), prefab);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId RegisterViewSource(UnityGameObjectProviderInitializer providerInitializer, UnityEngine.GameObject prefab) {

            if (prefab == null) {

                ViewSourceIsNullException.Throw();

            }

            if (prefab.TryGetComponent(out IView component) == true) {

                return this.RegisterViewSource(providerInitializer, component);

            }

            return ViewId.Zero;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId RegisterViewSource(MonoBehaviourViewBase prefab) {

            return this.RegisterViewSource(new UnityGameObjectProviderInitializer(), (IView)prefab);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(UnityEngine.GameObject prefab, Entity entity) {

            if (prefab.TryGetComponent(out IView component) == true) {

                this.InstantiateView(component, entity);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(MonoBehaviourViewBase prefab, Entity entity) {

            this.InstantiateView((IView)prefab, entity);

        }

    }

}

namespace ME.ECS.Views {

    using ME.ECS.Views.Providers;

    public partial interface IViewModule {

        ViewId RegisterViewSource(UnityEngine.GameObject prefab);
        void UnRegisterViewSource(UnityEngine.GameObject prefab);
        void InstantiateView(UnityEngine.GameObject prefab, Entity entity);

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public partial class ViewsModule {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId RegisterViewSource(UnityEngine.GameObject prefab) {

            return this.RegisterViewSource(new UnityGameObjectProviderInitializer(), prefab.GetComponent<MonoBehaviourView>());

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void UnRegisterViewSource(UnityEngine.GameObject prefab) {

            this.UnRegisterViewSource(prefab.GetComponent<MonoBehaviourView>());

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(UnityEngine.GameObject prefab, Entity entity) {

            var viewSource = prefab.GetComponent<MonoBehaviourView>();
            this.InstantiateView(this.GetViewSourceId(viewSource), entity);

        }

    }

}

namespace ME.ECS.Views.Providers {

    using ME.ECS;
    using ME.ECS.Views;
    using Unity.Jobs;
    using UnityEngine.Jobs;
    using Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class MonoBehaviourViewBase : ViewBase, IDoValidate {

        public ParticleSystemSimulation particleSystemSimulation;
        new protected UnityEngine.Transform transform;

        public virtual bool applyStateJob => true;

        public bool IsJobsEnabled() {

            var world = Worlds.currentWorld;
            if (world.settings.useJobsForViews == false || world.settings.viewsSettings.unityGameObjectProviderDisableJobs == true) return false;

            return this.applyStateJob;

        }

        public virtual void ApplyStateJob(TransformAccess transform, float deltaTime, bool immediately) { }

        internal void InitializeTransform() {

            this.transform = base.transform;

        }

        public void SimulateParticles(float time, uint seed) {

            this.particleSystemSimulation.SimulateParticles(time, seed);

        }

        public void UpdateParticlesSimulation(float deltaTime) {

            this.particleSystemSimulation.Update(deltaTime);

        }

        [UnityEngine.ContextMenu("Validate")]
        public void DoValidate() {

            this.particleSystemSimulation.OnValidate(this.GetComponentsInChildren<UnityEngine.ParticleSystem>(true));

        }

        public virtual void OnValidate() {

            this.DoValidate();

        }

        public override string ToString() {

            return this.particleSystemSimulation.ToString();

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class MonoBehaviourView : MonoBehaviourViewBase, IView, IViewRespawnTime, IViewPool, IViewBaseInternal {

        int System.IComparable<IView>.CompareTo(IView other) {
            return 0;
        }

        float IViewRespawnTime.respawnTime { get; set; }

        /// <summary>
        /// Do you want to use custom pool id?
        /// Useful if you want to store unique instance of the same prefab source in different storages.
        /// </summary>
        public virtual uint customViewId => 0u;
        /// <summary>
        /// Do you want to use cache pooling for this view?
        /// </summary>
        public virtual bool useCache => false;
        /// <summary>
        /// Time to get ready this instance to be used again after it has been despawned.
        /// </summary>
        public virtual float cacheTimeout => Worlds.currentWorld.GetModule<ME.ECS.StatesHistory.IStatesHistoryModuleBase>().GetCacheSize();

        public World world { get; private set; }
        public virtual Entity entity { get; private set; }
        public uint entityVersion { get; set; }
        public ViewId prefabSourceId { get; private set; }
        public Tick creationTick { get; private set; }

        void IViewBaseInternal.Setup(World world, ViewInfo viewInfo) {

            this.world = world;
            this.entity = viewInfo.entity;
            this.prefabSourceId = viewInfo.prefabSourceId;
            this.creationTick = viewInfo.creationTick;

        }

        void IView.DoInitialize() {

            this.InitializeTransform();
            this.OnInitialize();

        }

        void IView.DoDeInitialize() {

            this.OnDeInitialize();

        }

        public virtual void OnInitialize() { }
        public virtual void OnDeInitialize() { }
        public virtual void ApplyState(float deltaTime, bool immediately) { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void ApplyPhysicsState(float deltaTime) { }

        public override string ToString() {

            var info = string.Empty;
            info += "Entity: " + this.entity.ToString() + "\n";
            info += "Prefab Source Id: " + this.prefabSourceId + "\n";
            info += "Creation Tick: " + this.creationTick + "\n";
            return info + base.ToString();

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class UnityGameObjectProvider : ViewsProvider {

        private PoolGameObject<MonoBehaviourView> pool;

        public override void OnConstruct() {

            this.pool = new PoolGameObject<MonoBehaviourView>();
            ME.WeakRef.Reg(this.pool);

        }

        public override void OnDeconstruct() {

            this.pool.Clear();
            this.pool = null;
            if (this.currentTransformArray.isCreated == true) this.currentTransformArray.Dispose();
            //if (this.resultTransforms != null) PoolList<UnityEngine.Transform>.Recycle(ref this.resultTransforms);
            if (this.tempList != null) PoolListCopyable<MonoBehaviourView>.Recycle(ref this.tempList);

        }

        public override IView Spawn(IView prefab, ViewId prefabSourceId, in Entity targetEntity) {

            var sourceTyped = (MonoBehaviourView)prefab;
            var view = this.pool.Spawn(sourceTyped, prefabSourceId, sourceTyped.customViewId, in targetEntity);
            if (this.world.debugSettings.showViewsOnScene == false || this.world.debugSettings.viewsSettings.unityGameObjectProviderShowOnScene == false) {

                view.gameObject.hideFlags = UnityEngine.HideFlags.HideInHierarchy;

            }

            return view;

        }

        public override void Destroy(ref IView instance) {

            var instanceTyped = (MonoBehaviourView)instance;
            this.pool.Recycle(ref instanceTyped, instanceTyped.customViewId, instanceTyped.useCache == true ? instanceTyped.cacheTimeout : 0f);
            instance = null;

        }

        private struct Job : IJobParallelForTransform {

            public float deltaTime;
            public int length;

            public void Execute(int index, TransformAccess transform) {

                if (index >= this.length) return;

                UnityGameObjectProvider.resultList[index].ApplyStateJob(transform, this.deltaTime, immediately: false);

            }

        }

        private static ListCopyable<MonoBehaviourView> resultList;
        private static int resultCount;
        //private Unity.Collections.NativeArray<Unity.Mathematics.float3> burstPositions;
        //private Unity.Collections.NativeArray<Unity.Mathematics.quaternion> burstRotations;
        //private Unity.Collections.NativeArray<Unity.Mathematics.float3> burstScales;
        private BufferArray<UnityEngine.Transform> currentTransforms;
        private TransformAccessArray currentTransformArray;
        private ListCopyable<MonoBehaviourView> tempList;

        public override void Update(BufferArray<Views> list, float deltaTime, bool hasChanged) {

            if (this.world.settings.useJobsForViews == false || this.world.settings.viewsSettings.unityGameObjectProviderDisableJobs == true) return;

            if (list.isCreated == true) {

                if (hasChanged == true) {

                    if (this.tempList == null) this.tempList = PoolListCopyable<MonoBehaviourView>.Spawn(list.Length);

                    var changed = false; //ArrayUtils.Resize(list.Length - 1, ref this.currentTransforms);

                    var k = 0;
                    for (int i = 0, length = list.Length; i < length; ++i) {

                        var item = list.arr[i];
                        if (item.isNotEmpty == false) continue;

                        for (int j = 0, count = item.Length; j < count; ++j) {

                            var view = item[j] as MonoBehaviourView;
                            if (view == null) continue;
                            if (view.applyStateJob == true && view.entity.IsAlive() == true) {

                                changed |= ArrayUtils.Resize(k, ref this.currentTransforms);
                                var isNew = false;
                                if (k >= this.tempList.Count) {

                                    this.tempList.Add(view);
                                    this.currentTransforms.arr[k] = view.transform;
                                    isNew = true;

                                }

                                var tempItem = this.tempList[k];
                                if (isNew == true ||
                                    tempItem.prefabSourceId != view.prefabSourceId ||
                                    tempItem.creationTick != view.creationTick ||
                                    tempItem.entity != view.entity) {

                                    this.tempList[k] = view;
                                    this.currentTransforms.arr[k] = view.transform;
                                    changed = true;

                                }

                                ++k;

                            }

                        }

                    }

                    if (this.currentTransformArray.isCreated == false) this.currentTransformArray = new TransformAccessArray(k);

                    if (changed == true) {

                        this.currentTransformArray.SetTransforms(this.currentTransforms.arr);
                        //if (UnityGameObjectProvider.resultList != null) PoolList<MonoBehaviourView>.Recycle(ref UnityGameObjectProvider.resultList);
                        //var result = PoolList<MonoBehaviourView>.Spawn(this.tempList.Count);
                        //result.AddRange(this.tempList);
                        UnityGameObjectProvider.resultList = this.tempList;
                        UnityGameObjectProvider.resultCount = k;

                    }

                }

                if (UnityGameObjectProvider.resultCount > 0 && this.currentTransformArray.isCreated == true) {

                    var job = new Job() {
                        deltaTime = deltaTime,
                        length = UnityGameObjectProvider.resultCount
                    };

                    var handle = job.Schedule(this.currentTransformArray);
                    handle.Complete();

                }

            }

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct UnityGameObjectProviderInitializer : IViewsProviderInitializer {

        int System.IComparable<IViewsProviderInitializerBase>.CompareTo(IViewsProviderInitializerBase other) {
            return 0;
        }

        public IViewsProvider Create() {

            return PoolClass<UnityGameObjectProvider>.Spawn();

        }

        public void Destroy(IViewsProvider instance) {

            PoolClass<UnityGameObjectProvider>.Recycle((UnityGameObjectProvider)instance);

        }

    }

}
#endif