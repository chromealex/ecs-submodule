#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
using Unity.Jobs;

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
        public void AssignView(UnityEngine.GameObject sceneSource, Entity entity, DestroyViewBehaviour destroyViewBehaviour) {

            if (sceneSource.TryGetComponent(out IView component) == true) {

                this.AssignView(component, entity, destroyViewBehaviour);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void AssignView(MonoBehaviourViewBase sceneSource, Entity entity, DestroyViewBehaviour destroyViewBehaviour) {
            
            this.AssignView((IView)sceneSource, entity, destroyViewBehaviour);
            
        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId RegisterViewSource(UnityEngine.GameObject prefab, ViewId customId = default) {

            return this.RegisterViewSource(new UnityGameObjectProviderInitializer(), prefab, customId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId RegisterViewSource(UnityGameObjectProviderInitializer providerInitializer, UnityEngine.GameObject prefab, ViewId customId = default) {

            if (prefab == null) {

                ViewSourceIsNullException.Throw();

            }

            if (prefab.TryGetComponent(out IView component) == true) {

                return this.RegisterViewSource(providerInitializer, component, customId);

            }

            return ViewId.Zero;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public ViewId RegisterViewSource(MonoBehaviourViewBase prefab, ViewId customId = default) {

            return this.RegisterViewSource(new UnityGameObjectProviderInitializer(), (IView)prefab, customId);

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

        ViewId RegisterViewSource(UnityEngine.GameObject prefab, ViewId customId = default);
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
        public ViewId RegisterViewSource(UnityEngine.GameObject prefab, ViewId customId = default) {

            return this.RegisterViewSource(new UnityGameObjectProviderInitializer(), ViewsModule.ViewSourceObject.Create(prefab.GetComponent<MonoBehaviourView>()), customId);

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
    using UnityEngine.Jobs;
    using Collections;

    public struct InterpolatedTransform : System.IEquatable<InterpolatedTransform> {

        public struct Transform : System.IEquatable<Transform> {

            public UnityEngine.Vector3 position;
            public UnityEngine.Quaternion rotation;
            public UnityEngine.Vector3 localScale;
            public bool isDirty;

            public Transform(UnityEngine.Transform transform) {

                this.position = transform.position;
                this.rotation = transform.rotation;
                this.localScale = transform.localScale;
                this.isDirty = true;

            }

            public bool IsEquals(UnityEngine.Transform transform) {

                return this.position == transform.position &&
                       this.rotation == transform.rotation &&
                       this.localScale == transform.localScale;

            }

            public bool Equals(Transform other) {
                return this.position.Equals(other.position) && this.rotation.Equals(other.rotation) && this.localScale.Equals(other.localScale) && this.isDirty == other.isDirty;
            }

            public override bool Equals(object obj) {
                return obj is Transform other && Equals(other);
            }

            public override int GetHashCode() {
                unchecked {
                    var hashCode = this.position.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.rotation.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.localScale.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.isDirty.GetHashCode();
                    return hashCode;
                }
            }

        }
        
        [System.Serializable]
        public struct Settings {

            public bool enabled;
            [UnityEngine.Space(8f)]
            public float movementSpeed;
            [UnityEngine.Space(8f)]
            public float rotationSpeed;

        }

        private readonly MonoBehaviourViewBase view;
        private readonly UnityEngine.Transform transform;
        private readonly Settings settings;
        private Transform targetTransform;
        private bool isInitialized;
        
        public UnityEngine.Vector3 interpolatedPosition { get; set; }
        public UnityEngine.Quaternion interpolatedRotation { get; set; }
        public UnityEngine.Vector3 interpolatedScale { get; set; }

        public UnityEngine.Vector3 position {
            get => this.targetTransform.position;
            set {
                this.targetTransform.position = value;
                this.targetTransform.isDirty = true;
            }
        }

        public UnityEngine.Quaternion rotation {
            get => this.targetTransform.rotation;
            set {
                this.targetTransform.rotation = value;
                this.targetTransform.isDirty = true;
            }
        }

        public UnityEngine.Vector3 localPosition {
            get => this.transform.localPosition;
            set => this.transform.localPosition = value;
        }

        public UnityEngine.Quaternion localRotation {
            get => this.transform.localRotation;
            set => this.transform.localRotation = value;
        }

        public UnityEngine.Vector3 localScale {
            get => this.targetTransform.localScale;
            set {
                this.targetTransform.localScale = value;
                this.targetTransform.isDirty = true;
            }
        }

        public bool interpolateNextImmediately { get; set; }

        public bool isValid => this.transform != null;

        public InterpolatedTransform(MonoBehaviourViewBase view, UnityEngine.Transform transform, InterpolatedTransform.Settings settings) {

            this.view = view;
            this.transform = transform;
            this.settings = settings;
            this.targetTransform = new Transform(transform);
            this.isInitialized = false;
            this.interpolateNextImmediately = true;
            this.interpolatedPosition = transform.position;
            this.interpolatedScale = transform.localScale;
            this.interpolatedRotation = transform.rotation;

        }

        private void ApplyStateInterpolation() {

            var prevTick = (long)this.view.world.GetInterpolationState().tick;
            var currentTick = (long)this.view.world.GetCurrentTick();
            var tickTime = this.view.world.tickTime;

            var prevTime = prevTick * tickTime;
            var currentTime = currentTick * tickTime;

            var currentWorldTime = this.view.world.timeSinceStart - tickTime;

            var factor = UnityEngine.Mathf.InverseLerp(prevTime, currentTime, (float)currentWorldTime);

            this.transform.position = UnityEngine.Vector3.Lerp(this.interpolatedPosition, this.targetTransform.position, factor);
            this.transform.localScale = UnityEngine.Vector3.Lerp(this.interpolatedScale, this.targetTransform.localScale, factor);
            this.transform.rotation = UnityEngine.Quaternion.Slerp(this.interpolatedRotation, this.targetTransform.rotation, factor);

        }
        
        public void Update(float dt) {

            if (this.settings.enabled == false || this.isInitialized == false) {

                if (this.targetTransform.isDirty == false) return;

                this.transform.position = this.targetTransform.position;
                this.transform.rotation = this.targetTransform.rotation;
                this.transform.localScale = this.targetTransform.localScale;

                this.targetTransform.isDirty = false;
                this.isInitialized = true;

            } else {
                
                if (this.view.world.settings.viewsSettings.interpolationState == true) {
                
                    this.ApplyStateInterpolation();
                    return;
                
                }

                if (this.targetTransform.isDirty == false) return;

                var maxMovementDelta = this.view.GetInterpolationMovementSpeed() * dt;
                if (maxMovementDelta > 0f && interpolateNextImmediately == false) {
                    this.transform.position = UnityEngine.Vector3.MoveTowards(this.transform.position, this.targetTransform.position, maxMovementDelta);
                } else {
                    this.transform.position = this.targetTransform.position;
                }

                var maxRotationDelta = this.view.GetInterpolationRotationSpeed() * dt;
                if (maxRotationDelta > 0f && interpolateNextImmediately == false) {
                    this.transform.rotation = UnityEngine.Quaternion.RotateTowards(this.transform.rotation, this.targetTransform.rotation, maxRotationDelta);
                } else {
                    this.transform.rotation = this.targetTransform.rotation;
                }

                this.transform.localScale = this.targetTransform.localScale;

                if (this.targetTransform.IsEquals(this.transform) == true) {
                    
                    this.targetTransform.isDirty = false;

                }

                this.interpolateNextImmediately = false;

            }

        }

        public static bool operator ==(UnityEngine.Transform transform, InterpolatedTransform other) {
            return transform == other.transform;
        }

        public static bool operator !=(UnityEngine.Transform transform, InterpolatedTransform other) {
            return !(transform == other);
        }

        public static bool operator ==(InterpolatedTransform transform, UnityEngine.Transform other) {
            return transform.transform == other;
        }

        public static bool operator !=(InterpolatedTransform transform, UnityEngine.Transform other) {
            return !(transform == other);
        }

        public static implicit operator UnityEngine.Transform(InterpolatedTransform transform) {
            return transform.transform;
        }

        public bool Equals(InterpolatedTransform other) {
            return Equals(this.view, other.view) && Equals(this.transform, other.transform) && this.settings.Equals(other.settings) && this.targetTransform.Equals(other.targetTransform);
        }

        public override bool Equals(object obj) {
            return obj is InterpolatedTransform other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (this.view != null ? this.view.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.transform != null ? this.transform.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.settings.GetHashCode();
                hashCode = (hashCode * 397) ^ this.targetTransform.GetHashCode();
                return hashCode;
            }
        }

    }
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class MonoBehaviourViewBase : ViewBase, IDoValidate {

        [System.Serializable]
        public struct DefaultParameters {

            public bool useDespawnTime;
            public float despawnTime;

            [UnityEngine.Space(8f)]
            public uint cacheCustomViewId;
            
            [UnityEngine.Space(8f)]
            public bool useCache;
        
            [UnityEngine.Space(8f)]
            public bool useCacheTimeout;
            public float cacheTimeout;

        }

        [System.Serializable]
        public struct ParentParameters {

            [System.Serializable]
            public struct Item {

                public UnityEngine.Transform transform;
                public int parentId;

            }

            public bool enabled;
            [UnityEngine.Space(8f)]
            public int parentId;
            [UnityEngine.Space(8f)]
            public Item[] roots;

            public UnityEngine.Transform GetRoot(int parentId) {

                for (int i = 0; i < this.roots.Length; ++i) {
                    if (this.roots[i].parentId == parentId) return this.roots[i].transform;
                }

                return null;

            }

        }
        
        public ParticleSystemSimulation particleSystemSimulation;
        public DefaultParameters defaultParameters;
        public InterpolatedTransform.Settings interpolationParameters;
        public ParentParameters parentParameters;
        
        new protected InterpolatedTransform transform;
        
        public World world { get; protected internal set; }

        public virtual bool applyStateJob => true;

        public bool IsJobsEnabled() {

            var world = Worlds.currentWorld;
            if (world.settings.useJobsForViews == false || world.settings.viewsSettings.unityGameObjectProviderDisableJobs == true) return false;

            return this.applyStateJob;

        }

        public virtual void ApplyStateJob(TransformAccess transform, float deltaTime, bool immediately) { }

        internal void InitializeTransform() {

            this.transform = new InterpolatedTransform(this, base.transform, this.interpolationParameters);

        }

        public virtual float GetInterpolationMovementSpeed() => this.interpolationParameters.movementSpeed;
        public virtual float GetInterpolationRotationSpeed() => this.interpolationParameters.rotationSpeed;

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

            this.InitializeTransform();
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
    public abstract class MonoBehaviourView : MonoBehaviourViewBase, IView, IViewRespawnTime, IViewDestroyTime, IViewPool, IViewBaseInternal {

        int System.IComparable<IView>.CompareTo(IView other) {
            return 0;
        }

        float IViewRespawnTime.respawnTime { get; set; }

        /// <summary>
        /// Do you want to use custom pool id?
        /// Useful if you want to store unique instance of the same prefab source in different storages.
        /// </summary>
        public virtual uint customViewId => (this.defaultParameters.useCache == true ? this.defaultParameters.cacheCustomViewId : 0u);
        /// <summary>
        /// Do you want to use cache pooling for this view?
        /// </summary>
        public virtual bool useCache => this.defaultParameters.useCache;
        /// <summary>
        /// Time to get ready this instance to be used again after it has been despawned.
        /// </summary>
        public virtual float cacheTimeout => this.defaultParameters.useCacheTimeout == true ? this.defaultParameters.cacheTimeout : (float)Worlds.currentWorld.GetTimeFromTick(Worlds.currentWorld.GetModule<ME.ECS.StatesHistory.IStatesHistoryModuleBase>().GetCacheSize());
        /// <summary>
        /// Time to despawn view before it has been pooled.
        /// </summary>
        public virtual float despawnDelay => (this.defaultParameters.useDespawnTime == true ? this.defaultParameters.despawnTime : 0f);
        
        public uint entityVersion { get; set; }
        public virtual Entity entity => this.info.entity;
        public ViewId prefabSourceId => this.info.prefabSourceId;
        public Tick creationTick => this.info.creationTick;
        public ViewInfo info { get; private set; }

        void IViewBaseInternal.Setup(World world, ViewInfo viewInfo) {

            this.world = world;
            this.info = viewInfo;

        }

        public UnityEngine.Transform GetParentTransform(int parentId) {

            var root = this.parentParameters.GetRoot(parentId);
            if (root != null) return root;
            
            return this.transform;

        }

        void IView.DoInitialize() {

            this.InitializeTransform();
            this.OnInitialize();

        }

        void IView.DoDeInitialize() {

            this.OnDeInitialize();

        }

        public virtual void DoDestroy() {

            this.OnDisconnect();

        }

        public virtual void OnInitialize() { }
        public virtual void OnDeInitialize() { }
        public virtual void OnDisconnect() { }
        public virtual void ApplyState(float deltaTime, bool immediately) { }
        public virtual void ApplyPhysicsState(float deltaTime) { }
        
        void IView.DoUpdate(float deltaTime) {

            this.OnUpdate(deltaTime);
            if (this.transform.isValid == true) {
                this.transform.Update(deltaTime);
            }
            
        }
        
        public virtual void OnUpdate(float deltaTime) { }

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
            if (view.parentParameters.enabled == true) WorldStaticCallbacks.RaiseCallbackOnViewCreated(in targetEntity, view, view.parentParameters.parentId);
            if (this.world.debugSettings.showViewsOnScene == false || this.world.debugSettings.viewsSettings.unityGameObjectProviderShowOnScene == false) {

                view.gameObject.hideFlags = UnityEngine.HideFlags.HideInHierarchy;

            }

            return view;

        }

        public override bool Destroy(ref IView instance) {

            var instanceTyped = (MonoBehaviourView)instance;
            if (instanceTyped.parentParameters.enabled == true) WorldStaticCallbacks.RaiseCallbackOnViewDestroy(instanceTyped.entity, instanceTyped);
            var immediately = this.pool.Recycle(ref instanceTyped, instanceTyped.customViewId, instanceTyped.useCache == true ? instanceTyped.cacheTimeout : 0f);
            instance = null;
            return immediately;

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

        public override void Update(ViewsModule module, BufferArray<Views> list, float deltaTime, bool hasChanged) {

            this.pool.Update(module, deltaTime);

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
                        length = UnityGameObjectProvider.resultCount,
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