using ME.ECS;
using Unity.Jobs;
using Unity.Burst;
using ME.ECS.Essentials.Physics.Components;
using ME.ECS.Mathematics;

namespace ME.ECS.Essentials.Physics.Core.Collisions.Systems {

    #pragma warning disable
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    using Bag = ME.ECS.Buffers.FilterBag<
        ME.ECS.Transform.Position,
        ME.ECS.Transform.Rotation,
        ME.ECS.Transform.Scale,
        PhysicsCollider,
        PhysicsVelocity>;

    using BagMotion = ME.ECS.Buffers.FilterBag<
        PhysicsMass,
        PhysicsGravityFactor,
        PhysicsDamping,
        PhysicsMassOverride>;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class BuildPhysicsWorldSystem : ISystem, IAdvanceTick {
        
        private UnityPhysicsFeature feature;

        private UnityS.Physics.SimulationContext simulationContext;
        private Filter staticBodies;
        private Filter dynamicBodies;

        // Scheduler has static data which will never changed
        // So it could be stored inside this system
        private UnityS.Physics.DispatchPairSequencer scheduler = new UnityS.Physics.DispatchPairSequencer();
        
        private ref UnityS.Physics.PhysicsWorld physicsWorld => ref this.feature.physicsWorldInternal;
        
        public World world { get; set; }
        
        void ISystemBase.OnConstruct() {
            
            this.GetFeature(out this.feature);

            Filter.Create("BurstFilter-Static")
                  .With<ME.ECS.Transform.Position>()
                  .With<ME.ECS.Transform.Rotation>()
                  .With<ME.ECS.Transform.Scale>()
                  .With<PhysicsBody>()
                  .With<IsPhysicsStatic>()
                  .Push(ref this.staticBodies);

            Filter.Create("BurstFilter-Dynamic")
                  .With<ME.ECS.Transform.Position>()
                  .With<ME.ECS.Transform.Rotation>()
                  .With<ME.ECS.Transform.Scale>()
                  .With<PhysicsBody>()
                  .Without<IsPhysicsStatic>()
                  .Push(ref this.dynamicBodies);

        }

        void ISystemBase.OnDeconstruct() {
            
            this.simulationContext.Dispose();
            this.physicsWorld.Dispose();
            
            this.scheduler.Dispose();
            
        }

        [BurstCompile]
        private struct FillBodiesJob : IJobParallelFor {

            public Unity.Collections.NativeArray<UnityS.Physics.RigidBody> bodies;
            public Bag bag;
            
            public void Execute(int index) {

                var pos = this.bag.ReadT0(index).value;
                var rot = this.bag.ReadT1(index).value;
                var collider = this.bag.ReadT3(index);
                var entity = this.bag.GetEntity(index);
                this.bodies[index] = new UnityS.Physics.RigidBody {
                    WorldFromBody = new RigidTransform(rot, pos),
                    Collider = collider.IsValid == true ? collider.value : default,
                    Entity = entity,
                    CustomTags = (byte)0,
                };

            }

        }
        
        [BurstCompile]
        private struct FillMotionJob : IJobParallelFor {

            public Unity.Collections.NativeArray<UnityS.Physics.MotionData> motionDatas;
            public Unity.Collections.NativeArray<UnityS.Physics.MotionVelocity> motionVelocities;
            public Bag bag;
            public BagMotion bagMotion;
            public PhysicsMass defaultPhysicsMass;

            public void Execute(int index) {

                var pos = (float3)this.bag.ReadT0(index).value;
                var rot = (quaternion)this.bag.ReadT1(index).value;
                
                var chunkVelocity = this.bag.ReadT4(index);
                var chunkMass = this.bagMotion.ReadT0(index);
                var chunkGravityFactor = this.bagMotion.ReadT1(index);
                var chunkDamping = this.bagMotion.ReadT2(index);
                var chunkMassOverride = this.bagMotion.ReadT3(index);
                var hasChunkPhysicsMassType = this.bagMotion.HasT0(index);
                var hasChunkPhysicsDampingType = this.bagMotion.HasT1(index);
                var hasChunkPhysicsGravityFactorType = this.bagMotion.HasT2(index);
                var hasChunkPhysicsMassOverrideType = this.bagMotion.HasT3(index);
                
                // Note: if a dynamic body infinite mass then assume no gravity should be applied
                sfloat defaultGravityFactor = hasChunkPhysicsMassType ? sfloat.One : sfloat.Zero;

                var isKinematic = !hasChunkPhysicsMassType || hasChunkPhysicsMassOverrideType && chunkMassOverride.IsKinematic != 0;
                this.motionVelocities[index] = new UnityS.Physics.MotionVelocity {
                    LinearVelocity = chunkVelocity.Linear,
                    AngularVelocity = chunkVelocity.Angular,
                    InverseInertia = isKinematic ? this.defaultPhysicsMass.InverseInertia : chunkMass.InverseInertia,
                    InverseMass = isKinematic ? this.defaultPhysicsMass.InverseMass : chunkMass.InverseMass,
                    AngularExpansionFactor = hasChunkPhysicsMassType ? chunkMass.AngularExpansionFactor : this.defaultPhysicsMass.AngularExpansionFactor,
                    GravityFactor = isKinematic ? sfloat.Zero : hasChunkPhysicsGravityFactorType ? chunkGravityFactor.value : defaultGravityFactor,
                };

                // Note: these defaults assume a dynamic body with infinite mass, hence no damping
                var defaultPhysicsDamping = new PhysicsDamping {
                    Linear = sfloat.Zero,
                    Angular = sfloat.Zero,
                };

                // Create motion datas
                PhysicsMass mass = hasChunkPhysicsMassType ? chunkMass : this.defaultPhysicsMass;
                PhysicsDamping damping = hasChunkPhysicsDampingType ? chunkDamping : defaultPhysicsDamping;

                var a = math.mul(rot, mass.InertiaOrientation);
                var b = math.rotate(rot, mass.CenterOfMass) + pos;
                this.motionDatas[index] = new UnityS.Physics.MotionData {
                    WorldFromMotion = new RigidTransform(a, b),
                    BodyFromMotion = new RigidTransform(mass.InertiaOrientation, mass.CenterOfMass),
                    LinearDamping = damping.Linear,
                    AngularDamping = damping.Angular,
                };
                
            }

        }

        [BurstCompile]
        private struct ApplyPhysicsJob : IJobParallelFor {

            public Unity.Collections.NativeArray<UnityS.Physics.RigidBody> bodies;
            public Unity.Collections.NativeArray<UnityS.Physics.MotionVelocity> motionVelocities;
            public Bag bag;

            public void Execute(int index) {
                
                var data = this.bodies[index].WorldFromBody;
                this.bag.GetT0(index).value = data.pos;
                this.bag.GetT1(index).value = data.rot;
                ref var vel = ref this.bag.GetT4(index);
                vel.Angular = this.motionVelocities[index].AngularVelocity;
                vel.Linear = this.motionVelocities[index].LinearVelocity;
                
            }

        }
        
        [BurstCompile]
        private struct CheckStaticBodyChangesJob : IJobParallelFor {

            public Bag bag;
            public Tick currentTick;
            public Tick prevTick;
            [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
            public Unity.Collections.NativeArray<int> result;
            
            public void Execute(int index) {
                
                bool didBatchChange = (this.bag.GetVersionT0(index) != this.currentTick ||
                                       this.bag.GetVersionT0(index) != this.prevTick ||
                                       this.bag.GetVersionT1(index) != this.currentTick ||
                                       this.bag.GetVersionT1(index) != this.prevTick ||
                                       this.bag.GetVersionT2(index) != this.currentTick ||
                                       this.bag.GetVersionT2(index) != this.prevTick);
                if (didBatchChange == true) {
                    // Note that multiple worker threads may be running at the same time.
                    // They either write 1 to Result[0] or not write at all.  In case multiple
                    // threads are writing 1 to this variable, in C#, reads or writes of int
                    // data type are atomic, which guarantees that Result[0] is 1.
                    this.result[0] = 1;
                }
            }
        }

        void IAdvanceTick.AdvanceTick(in float deltaTime) {

            this.physicsWorld.Reset(this.staticBodies.Count, this.dynamicBodies.Count, 0);

            var simulationParameters = new UnityS.Physics.SimulationStepInput() {
                Gravity = new float3(0f, -9.8f, 0f),
                NumSolverIterations = 4,
                SolverStabilizationHeuristicSettings = UnityS.Physics.Solver.StabilizationHeuristicSettings.Default,
                SynchronizeCollisionWorld = true,
                TimeStep = deltaTime,
                World = this.physicsWorld,
            };

            ref var internalData = ref this.world.GetSharedData<PhysicsInternal>();

            var bag = new Bag(this.dynamicBodies, Unity.Collections.Allocator.TempJob);
            var bagStatic = new Bag(this.staticBodies, Unity.Collections.Allocator.TempJob);
            var bagMotion = new BagMotion(this.dynamicBodies, Unity.Collections.Allocator.TempJob);
            var buildStaticTree = new Unity.Collections.NativeArray<int>(1, Unity.Collections.Allocator.TempJob);
            JobHandle staticBodiesCheckHandle = default;
            buildStaticTree[0] = 0;
            {
                // Check static has changed
                if (internalData.prevStaticCount != bagStatic.Length) {
                    buildStaticTree[0] = 1;
                } else {
                    staticBodiesCheckHandle = new CheckStaticBodyChangesJob {
                        result = buildStaticTree,
                        bag = bagStatic,
                        currentTick = this.world.GetCurrentTick(),
                        prevTick = this.world.GetCurrentTick() - 1,
                    }.Schedule(bagStatic.Length, MathUtils.GetScheduleBatchCount(bagStatic.Length));
                }
                internalData.prevStaticCount = bagStatic.Length;
            }

            var dynamicBodies = this.physicsWorld.DynamicBodies;
            var staticBodies = this.physicsWorld.StaticBodies;
            var motionDatas = this.physicsWorld.MotionDatas;
            var motionVelocities = this.physicsWorld.MotionVelocities;
            
            var fillBodiesDynamicJob = new FillBodiesJob() {
                bag = bag,
                bodies = dynamicBodies,
            }.Schedule(bag.Length, MathUtils.GetScheduleBatchCount(bag.Length));
            var fillBodiesStaticJob = new FillBodiesJob() {
                bag = bagStatic,
                bodies = staticBodies,
            }.Schedule(bagStatic.Length, MathUtils.GetScheduleBatchCount(bagStatic.Length));
            var fillMotionJob = new FillMotionJob() {
                bag = bag,
                bagMotion = bagMotion,
                defaultPhysicsMass = PhysicsMass.CreateDynamic(UnityS.Physics.MassProperties.UnitSphere, 1f),
                motionDatas = motionDatas,
                motionVelocities = motionVelocities,
            }.Schedule(bag.Length, MathUtils.GetScheduleBatchCount(bag.Length));
            
            var deps = JobHandle.CombineDependencies(fillBodiesDynamicJob, fillBodiesStaticJob, fillMotionJob);
            deps = JobHandle.CombineDependencies(deps, staticBodiesCheckHandle);
            var broadphaseJob = this.physicsWorld.CollisionWorld.ScheduleBuildBroadphaseJobs(in this.physicsWorld, simulationParameters.TimeStep, simulationParameters.Gravity, buildStaticTree, deps, true);
            
            var jobs = UnityS.Physics.Simulation.ScheduleStepJobs(ref simulationParameters, ref this.simulationContext, this.scheduler, broadphaseJob, true/*ref this.simulationContext*/);
            jobs.FinalExecutionHandle.Complete();

            this.world.SetSharedDataOneShot(new PhysicsOneShotInternal() {
                collisionEvents = this.simulationContext.CollisionEvents,
                triggerEvents = this.simulationContext.TriggerEvents,
            });
            
            if (this.feature.sendCollisionEvents == true) {

                foreach (var evt in this.simulationContext.CollisionEvents) {

                    var entityAEvt = new Entity(EntityFlag.OneShot);
                    entityAEvt.SetOneShot(new PhysicsEventOnCollision() {
                        data = evt,
                    });

                    var entityBEvt = new Entity(EntityFlag.OneShot);
                    entityBEvt.SetOneShot(new PhysicsEventOnCollision() {
                        data = evt,
                    });

                }

            }

            if (this.feature.sendTriggerEvents == true) {

                foreach (var evt in this.simulationContext.TriggerEvents) {

                    var entityAEvt = new Entity(EntityFlag.OneShot);
                    entityAEvt.SetOneShot(new PhysicsEventOnTrigger() {
                        data = evt,
                    });

                    var entityBEvt = new Entity(EntityFlag.OneShot);
                    entityBEvt.SetOneShot(new PhysicsEventOnTrigger() {
                        data = evt,
                    });

                }

            }

            {
                // Sync physics result with entities
                new ApplyPhysicsJob() {
                    bag = bag,
                    bodies = dynamicBodies,
                    motionVelocities = motionVelocities,
                }.Schedule(bag.Length, MathUtils.GetScheduleBatchCount(bag.Length)).Complete();
            }

            bag.Push();
            bagStatic.Revert();
            bagMotion.Revert();

            buildStaticTree.Dispose();

        }

    }
    
}