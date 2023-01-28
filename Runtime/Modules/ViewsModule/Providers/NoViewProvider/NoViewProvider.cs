#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif
using Unity.Jobs;

namespace ME.ECS {

    using ME.ECS.Views;
    using ME.ECS.Views.Providers;

    public partial struct WorldViewsSettings {

        public bool unityNoViewProviderDisableJobs;

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
        public ViewId RegisterViewSource(NoViewBase prefab, ViewId customId = default) {

            return this.RegisterViewSource(new NoViewProviderInitializer(), (IView)prefab, customId);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public void InstantiateView(NoViewBase prefab, Entity entity) {

            this.InstantiateView((IView)prefab, entity);

        }

    }

}

namespace ME.ECS.Views.Providers {

    using ME.ECS;
    using ME.ECS.Views;
    using ME.ECS.Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class NoViewBase {

        public void SimulateParticles(float time, uint seed) { }

        public void UpdateParticlesSimulation(float deltaTime) { }

        public override string ToString() {

            return "NoView";

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public abstract class NoView : NoViewBase, IView, IViewBaseInternal {

        int System.IComparable<IView>.CompareTo(IView other) {
            return 0;
        }

        public World world { get; private set; }
        public uint entityVersion { get; set; }
        public Entity entity => this.info.entity;
        public ViewId prefabSourceId => this.info.prefabSourceId;
        public Tick creationTick => this.info.creationTick;
        public ViewInfo info { get; private set; }

        void IViewBaseInternal.Setup(World world, ViewInfo viewInfo) {

            this.world = world;
            this.info = viewInfo;

        }

        void IView.DoInitialize() {

            this.OnInitialize();

        }

        void IView.DoDeInitialize() {

            this.OnDeInitialize();

        }

        void IView.DoDestroy() {

            this.OnDisconnect();

        }

        void IView.DoUpdate(float dt) {
            
            this.OnUpdate(dt);

        }

        public virtual void OnInitialize() { }
        public virtual void OnDeInitialize() { }
        public virtual void OnDisconnect() { }
        public virtual void ApplyStateJob(float deltaTime, bool immediately) { }
        public virtual void ApplyState(float deltaTime, bool immediately) { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void ApplyPhysicsState(float deltaTime) { }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class NoViewProvider : ViewsProvider {

        internal struct NullState {}
        
        private DictionaryCopyable<ViewId, PoolInternalBase> pools;
        private static ME.ECS.Collections.BufferArray<Views> currentList;

        public override void OnConstruct() {

            this.pools = PoolDictionaryCopyable<ViewId, PoolInternalBase>.Spawn(100);

        }

        public override void OnDeconstruct() {

            PoolDictionaryCopyable<ViewId, PoolInternalBase>.Recycle(ref this.pools);

        }

        public override IView Spawn(IView prefab, ViewId prefabSourceId, in Entity targetEntity) {

            var prefabSource = (NoView)prefab;

            if (this.pools.TryGetValue(prefabSourceId, out var pool) == false) {
                
                pool = new PoolInternalBase(typeof(NoView));
                this.pools.Add(prefabSourceId, pool);
                
            }

            var obj = pool.Spawn(new NullState());
            if (obj == null) {

                obj = System.Activator.CreateInstance(prefab.GetType());

            }

            var particleViewBase = (IViewBaseInternal)obj;
            particleViewBase.Setup(this.world, prefabSource.info);

            return (IView)obj;

        }

        public override bool Destroy(ref IView instance) {

            var prefabSourceId = instance.info.prefabSourceId;
            if (this.pools.TryGetValue(prefabSourceId, out var pool) == false) {
                
                pool = new PoolInternalBase(typeof(DrawMeshViewBase));
                this.pools.Add(prefabSourceId, pool);
                
            }

            pool.Recycle(instance);
            instance = null;

            return true;

        }

        private struct Job : IJobParallelFor {

            public float deltaTime;

            public void Execute(int index) {

                var list = NoViewProvider.currentList.arr[index];
                if (list.mainView == null) return;

                for (int i = 0, count = list.Length; i < count; ++i) {

                    var instance = list[i] as NoView;
                    if (instance == null) continue;

                    instance.ApplyStateJob(this.deltaTime, immediately: false);

                }

            }

        }

        public override void Update(ViewsModule module, ME.ECS.Collections.BufferArray<Views> list, float deltaTime, bool hasChanged) {

            if (this.world.settings.useJobsForViews == true && this.world.settings.viewsSettings.unityNoViewProviderDisableJobs == false) {

                NoViewProvider.currentList = list;

                var job = new Job() {
                    deltaTime = deltaTime,
                };
                var handle = job.Schedule(list.Length, 16);
                handle.Complete();

            } else {

                for (int j = 0; j < list.Length; ++j) {

                    var item = list.arr[j];
                    for (int i = 0, count = item.Length; i < count; ++i) {

                        var instance = item[i] as NoView;
                        if (instance == null) continue;

                        instance.ApplyStateJob(deltaTime, immediately: false);

                    }

                }

            }

        }

    }

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public struct NoViewProviderInitializer : IViewsProviderInitializer {

        int System.IComparable<IViewsProviderInitializerBase>.CompareTo(IViewsProviderInitializerBase other) {
            return 0;
        }

        public IViewsProvider Create() {

            return PoolClass<NoViewProvider>.Spawn();

        }

        public void Destroy(IViewsProvider instance) {

            PoolClass<NoViewProvider>.Recycle((NoViewProvider)instance);

        }

    }

}