using ME.ECS;
using Unity.Jobs;
using Unity.Burst;
using ME.ECS.Essentials.Physics.Components;
using ME.ECS.Mathematics;
using ME.ECS.Essentials.Physics;

namespace ME.ECS.Essentials.Physics.Core.Collisions.Systems {

    #pragma warning disable
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    using Unity.Collections;
    
    using Bag = ME.ECS.Buffers.FilterBag<
        PhysicsCollider,
        PhysicsVelocity,
        ME.ECS.Transform.Container,
        PhysicsCustomTags,
        ME.ECS.Transform.Position,
        ME.ECS.Transform.Rotation>;

    using BagMotion = ME.ECS.Buffers.FilterBag<
        PhysicsMass,
        PhysicsGravityFactor,
        PhysicsDamping,
        PhysicsMassOverride>;

    using BagJoints = ME.ECS.Buffers.FilterBag<
        PhysicsJoint,
        PhysicsConstrainedBodyPair>;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class BuildPhysicsWorldSystem : ISystem, IAdvanceTick {
        
        private UnityPhysicsFeature feature;

        private ME.ECS.Essentials.Physics.SimulationContext simulationContext;
        private Filter staticBodies;
        private Filter dynamicBodies;
        private Filter joints;

        // Scheduler has static data which will never changed
        // So it could be stored inside this system
        private ME.ECS.Essentials.Physics.DispatchPairSequencer scheduler = new ME.ECS.Essentials.Physics.DispatchPairSequencer();
        
        private ref ME.ECS.Essentials.Physics.PhysicsWorld physicsWorld => ref this.feature.physicsWorldInternal;
        
        public World world { get; set; }
        
        void ISystemBase.OnConstruct() {
            
            this.GetFeature(out this.feature);

            Filter.Create("BurstFilter-Static")
                  .With<ME.ECS.Transform.Position>()
                  .With<ME.ECS.Transform.Rotation>()
                  .With<IsPhysicsStatic>()
                  .Push(ref this.staticBodies);

            Filter.Create("BurstFilter-Dynamic")
                  .With<ME.ECS.Transform.Position>()
                  .With<ME.ECS.Transform.Rotation>()
                  .With<PhysicsMass>()
                  .Without<IsPhysicsStatic>()
                  .Push(ref this.dynamicBodies);

            Filter.Create("BurstFilter-Joints")
                .With<PhysicsJoint>()
                .With<PhysicsConstrainedBodyPair>()
                .Push(ref this.joints);

        }

        void ISystemBase.OnDeconstruct() {
            
            this.simulationContext.Dispose();
            
            this.scheduler.Dispose();
            
        }
        
        [BurstCompile]
        internal struct CreateJoints : IJobParallelForFilterBag<BagJoints> {

            [ReadOnly] public int NumDynamicBodies;
            [ReadOnly] public NativeHashMap<Entity, int> EntityBodyIndexMap;

            [NativeDisableParallelForRestriction] public NativeArray<Joint> Joints;
            [NativeDisableParallelForRestriction] public NativeHashMap<Entity, int>.ParallelWriter EntityJointIndexMap;

            public int DefaultStaticBodyIndex;

            public void Execute(ref BagJoints bag, int index) {

                var joint = bag.ReadT0(index);
                var bodyPair = bag.ReadT1(index);
                var entity = bag.GetEntity(index);
                
                var entityA = bodyPair.EntityA;
                var entityB = bodyPair.EntityB;
                
                // TODO find a reasonable way to look up the constraint body indices
                // - stash body index in a component on the entity? But we don't have random access to Entity data in a job
                // - make a map from entity to rigid body index? Sounds bad and I don't think there is any NativeArray-based map data structure yet

                // If one of the entities is null, use the default static entity
                var pair = new BodyIndexPair
                {
                    BodyIndexA = entityA == Entity.Null ? DefaultStaticBodyIndex : -1,
                    BodyIndexB = entityB == Entity.Null ? DefaultStaticBodyIndex : -1,
                };

                // Find the body indices
                pair.BodyIndexA = EntityBodyIndexMap.TryGetValue(entityA, out var idxA) ? idxA : -1;
                pair.BodyIndexB = EntityBodyIndexMap.TryGetValue(entityB, out var idxB) ? idxB : -1;

                bool isInvalid = false;
                // Invalid if we have not found the body indices...
                isInvalid |= (pair.BodyIndexA == -1 || pair.BodyIndexB == -1);
                // ... or if we are constraining two static bodies
                // Mark static-static invalid since they are not going to affect simulation in any way.
                isInvalid |= (pair.BodyIndexA >= NumDynamicBodies && pair.BodyIndexB >= NumDynamicBodies);
                if (isInvalid)
                {
                    pair = BodyIndexPair.Invalid;
                }

                Joints[index] = new Joint
                {
                    BodyPair = pair,
                    Entity = entity,
                    EnableCollision = (byte)bodyPair.EnableCollision,
                    AFromJoint = joint.BodyAFromJoint.AsMTransform(),
                    BFromJoint = joint.BodyBFromJoint.AsMTransform(),
                    Version = joint.Version,
                    Constraints = joint.GetConstraints(),
                };
                EntityJointIndexMap.TryAdd(entity, index);

            }
            
        }

        [BurstCompile]
        private struct FillBodiesJob : IJobParallelForFilterBag<Bag> {

            public NativeArray<float3> bagParentPositions;
            public NativeArray<quaternion> bagParentRotations;
            [ReadOnly] public int FirstBodyIndex;
            [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestrictionAttribute] public Unity.Collections.NativeArray<ME.ECS.Essentials.Physics.RigidBody> bodies;
            [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestrictionAttribute] public NativeHashMap<Entity, int>.ParallelWriter EntityBodyIndexMap;

            public void Execute(ref Bag bag, int index) {
                
                int rbIndex = this.FirstBodyIndex + index;
                var collider = bag.ReadT0(index);
                var hasChunkPhysicsColliderType = bag.HasT0(index);
                var hasChunkPhysicsCustomTagsType = bag.HasT3(index);
                var entity = bag.GetEntity(index);
                
                var worldFromBody = new RigidTransform(this.bagParentRotations[index], this.bagParentPositions[index]);
                this.bodies[rbIndex] = new RigidBody
                {
                    WorldFromBody = new RigidTransform(worldFromBody.rot, worldFromBody.pos),
                    Collider = hasChunkPhysicsColliderType ? collider.value : default,
                    Entity = entity,
                    CustomTags = hasChunkPhysicsCustomTagsType ? bag.ReadT3(index).value : (byte)0
                };
                EntityBodyIndexMap.TryAdd(entity, rbIndex);
                
            }

        }

        [BurstCompile]
        private struct FillDefaultBodiesJob : IJob {
            
            [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestrictionAttribute]
            public NativeArray<RigidBody> NativeBodies;
            public int BodyIndex;

            [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestrictionAttribute]
            public NativeHashMap<Entity, int>.ParallelWriter EntityBodyIndexMap;

            public void Execute() {

                NativeBodies[BodyIndex] = new RigidBody
                {
                    WorldFromBody = new RigidTransform(quaternion.identity, float3.zero),
                    Collider = default,
                    Entity = Entity.Null,
                    CustomTags = 0
                };
                EntityBodyIndexMap.TryAdd(Entity.Null, BodyIndex);

            }

        }

        [BurstCompile]
        private struct FillMotionJob : IJobParallelForFilterBag<Bag> {

            public Unity.Collections.NativeArray<ME.ECS.Essentials.Physics.MotionData> motionDatas;
            public Unity.Collections.NativeArray<ME.ECS.Essentials.Physics.MotionVelocity> motionVelocities;
            public BagMotion bagMotion;
            public PhysicsMass defaultPhysicsMass;

            public void Execute(ref Bag bag, int index) {
    
                var pos = (float3)bag.ReadT4(index).value;
                var rot = (quaternion)bag.ReadT5(index).value;
                
                var chunkVelocity = bag.ReadT1(index);
                var chunkMass = this.bagMotion.ReadT0(index);
                var chunkGravityFactor = this.bagMotion.ReadT1(index);
                var chunkDamping = this.bagMotion.ReadT2(index);
                var chunkMassOverride = this.bagMotion.ReadT3(index);
                var hasChunkPhysicsMassType = this.bagMotion.HasT0(index);
                var hasChunkPhysicsGravityFactorType = this.bagMotion.HasT1(index);
                var hasChunkPhysicsDampingType = this.bagMotion.HasT2(index);
                var hasChunkPhysicsMassOverrideType = this.bagMotion.HasT3(index);
                
                // Note: if a dynamic body infinite mass then assume no gravity should be applied
                sfloat defaultGravityFactor = hasChunkPhysicsMassType ? sfloat.One : sfloat.Zero;

                var isKinematic = !hasChunkPhysicsMassType || hasChunkPhysicsMassOverrideType && chunkMassOverride.IsKinematic != 0;
                this.motionVelocities[index] = new ME.ECS.Essentials.Physics.MotionVelocity {
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
                this.motionDatas[index] = new ME.ECS.Essentials.Physics.MotionData {
                    WorldFromMotion = new RigidTransform(a, b),
                    BodyFromMotion = new RigidTransform(mass.InertiaOrientation, mass.CenterOfMass),
                    LinearDamping = damping.Linear,
                    AngularDamping = damping.Angular,
                };

            }

        }

        [BurstCompile]
        private struct ApplyPhysicsJob : IJobParallelForFilterBag<Bag> {

            //public Unity.Collections.NativeArray<ME.ECS.Essentials.Physics.RigidBody> bodies;
            public Unity.Collections.NativeArray<ME.ECS.Essentials.Physics.MotionVelocity> motionVelocities;
            
            public void Execute(ref Bag bag, int index) {

                //var data = this.bodies[index].WorldFromBody;
                //this.bag.GetT0(index).value = data.pos;
                //this.bag.GetT1(index).value = data.rot;
                ref var vel = ref bag.GetT1(index);
                vel.Angular = this.motionVelocities[index].AngularVelocity;
                vel.Linear = this.motionVelocities[index].LinearVelocity;
                
            }

        }
        
        [BurstCompile]
        private struct CheckStaticBodyChangesJob : IJobParallelForFilterBag<Bag> {

            public Tick currentTick;
            public Tick prevTick;
            [Unity.Collections.NativeDisableParallelForRestrictionAttribute]
            public Unity.Collections.NativeArray<int> result;
            
            public void Execute(ref Bag bag, int index) {
                
                bool didBatchChange = (bag.GetVersionT2(index) != this.currentTick ||
                                       bag.GetVersionT2(index) != this.prevTick ||
                                       bag.GetVersionT4(index) != this.currentTick ||
                                       bag.GetVersionT4(index) != this.prevTick ||
                                       bag.GetVersionT5(index) != this.currentTick ||
                                       bag.GetVersionT5(index) != this.prevTick);
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

            var marker = new Unity.Profiling.ProfilerMarker("[Physics] Reset World");
            marker.Begin();
            
            this.physicsWorld.Reset(this.staticBodies.Count + 1 , this.dynamicBodies.Count, this.joints.Count);

            marker.End();

            if (this.physicsWorld.Bodies.Length == 0) {
                
                return;
                
            }

            var simulationParameters = new ME.ECS.Essentials.Physics.SimulationStepInput() {
                Gravity = this.feature.gravity,
                NumSolverIterations = 4,
                SolverStabilizationHeuristicSettings = ME.ECS.Essentials.Physics.Solver.StabilizationHeuristicSettings.Default,
                SynchronizeCollisionWorld = true,
                TimeStep = deltaTime,
                World = this.physicsWorld,
            };

            ref var internalData = ref this.world.GetSharedData<PhysicsInternal>();

            var bagJoints = new BagJoints(this.joints, Unity.Collections.Allocator.TempJob);
            var bag = new Bag(this.dynamicBodies, Unity.Collections.Allocator.TempJob);
            var bagStatic = new Bag(this.staticBodies, Unity.Collections.Allocator.TempJob);
            var bagMotion = new BagMotion(this.dynamicBodies, Unity.Collections.Allocator.TempJob);
            var buildStaticTree = new Unity.Collections.NativeArray<int>(1, Unity.Collections.Allocator.TempJob);
            
            var rootPositionsDynamic = this.GetParentPositionsFromBag(bag);
            var rootRotationsDynamic = this.GetParentRotationsFromBag(bag);
            var rootPositionsStatic = this.GetParentPositionsFromBag(bagStatic);
            var rootRotationsStatic = this.GetParentRotationsFromBag(bagStatic);

            {
                
                marker = new Unity.Profiling.ProfilerMarker("[Physics] Build World");
                marker.Begin();

                JobHandle staticBodiesCheckHandle = default;
                buildStaticTree[0] = 0;
                {
                    // Check static has changed
                    if (internalData.prevStaticCount != bagStatic.Length) {
                        buildStaticTree[0] = 1;
                    } else {
                        staticBodiesCheckHandle = new CheckStaticBodyChangesJob {
                            result = buildStaticTree,
                            currentTick = this.world.GetCurrentTick(),
                            prevTick = this.world.GetCurrentTick() - 1,
                        }.Schedule(bagStatic);
                    }

                    internalData.prevStaticCount = bagStatic.Length;
                }

                var motionDatas = this.physicsWorld.MotionDatas;
                var motionVelocities = this.physicsWorld.MotionVelocities;

                JobHandle outputDependency;
                using (var jobHandles = new NativeList<JobHandle>(4, Allocator.Temp)) {

                    jobHandles.Add(staticBodiesCheckHandle);

                    jobHandles.Add(new FillDefaultBodiesJob {
                        NativeBodies = this.physicsWorld.Bodies,
                        BodyIndex = this.physicsWorld.Bodies.Length - 1,
                        EntityBodyIndexMap = this.physicsWorld.CollisionWorld.EntityBodyIndexMap.AsParallelWriter(),
                    }.Schedule());

                    if (bag.Length > 0) {

                        jobHandles.Add(new FillBodiesJob() {
                            bodies = this.physicsWorld.Bodies,
                            bagParentPositions = rootPositionsDynamic,
                            bagParentRotations = rootRotationsDynamic,
                            FirstBodyIndex = 0,
                            EntityBodyIndexMap = this.physicsWorld.CollisionWorld.EntityBodyIndexMap.AsParallelWriter(),
                        }.Schedule(bag));

                        jobHandles.Add(new FillMotionJob() {
                            bagMotion = bagMotion,
                            defaultPhysicsMass = PhysicsMass.CreateDynamic(ME.ECS.Essentials.Physics.MassProperties.UnitSphere, 1f),
                            motionDatas = motionDatas,
                            motionVelocities = motionVelocities,
                        }.Schedule(bag));

                    }

                    if (bagStatic.Length > 0) {

                        jobHandles.Add(new FillBodiesJob() {
                            bodies = this.physicsWorld.Bodies,
                            bagParentPositions = rootPositionsStatic,
                            bagParentRotations = rootRotationsStatic,
                            FirstBodyIndex = bag.Length,
                            EntityBodyIndexMap = this.physicsWorld.CollisionWorld.EntityBodyIndexMap.AsParallelWriter(),
                        }.Schedule(bagStatic));

                    }

                    var handle = JobHandle.CombineDependencies(jobHandles);
                    jobHandles.Clear();

                    if (bagJoints.Length > 0) {
                        jobHandles.Add(new CreateJoints {
                            Joints = this.physicsWorld.Joints,
                            DefaultStaticBodyIndex = this.physicsWorld.Bodies.Length - 1,
                            NumDynamicBodies = bag.Length,
                            EntityBodyIndexMap = this.physicsWorld.CollisionWorld.EntityBodyIndexMap,
                            EntityJointIndexMap = this.physicsWorld.DynamicsWorld.EntityJointIndexMap.AsParallelWriter(),
                        }.Schedule(bagJoints, handle));
                    }

                    var buildBroadphaseHandle = this.physicsWorld.CollisionWorld.ScheduleBuildBroadphaseJobs(
                        ref this.physicsWorld,
                        simulationParameters.TimeStep,
                        simulationParameters.Gravity,
                        buildStaticTree,
                        handle,
                        true);
                    jobHandles.Add(buildBroadphaseHandle);

                    outputDependency = JobHandle.CombineDependencies(jobHandles);

                }

                marker.End();

                {

                    marker = new Unity.Profiling.ProfilerMarker("[Physics] Step World");
                    marker.Begin();

                    var jobs = ME.ECS.Essentials.Physics.Simulation.ScheduleStepJobs(ref simulationParameters,
                                                                                     ref this.simulationContext,
                                                                                     this.scheduler,
                                                                                     outputDependency,
                                                                                     true);
                    jobs.FinalExecutionHandle.Complete();

                    marker.End();

                    marker = new Unity.Profiling.ProfilerMarker("[Physics] Apply World");
                    marker.Begin();

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
                        var dynamicBodies = this.physicsWorld.DynamicBodies;
                        new ApplyPhysicsJob() {
                            //bodies = dynamicBodies,
                            motionVelocities = motionVelocities,
                        }.Schedule(bag).Complete();

                        for (int i = 0; i < bag.Length; ++i) {

                            var data = dynamicBodies[i].WorldFromBody;
                            var entity = bag.GetEntity(i);
                            entity.SetPosition(data.pos);
                            entity.SetRotation(data.rot);

                        }

                    }

                    marker.End();

                }
                
            }
            
            marker = new Unity.Profiling.ProfilerMarker("[Physics] Clean up");
            marker.Begin();
            
            bag.Push();
            bagStatic.Revert();
            bagMotion.Revert();
            bagJoints.Revert();

            rootPositionsDynamic.Dispose();
            rootRotationsDynamic.Dispose();
            rootPositionsStatic.Dispose();
            rootRotationsStatic.Dispose();

            buildStaticTree.Dispose();

            marker.End();

        }

        private NativeArray<quaternion> GetParentRotationsFromBag(Bag bag) {
            
            var arr = new NativeArray<quaternion>(bag.Length, Allocator.TempJob);
            for (int i = 0; i < bag.Length; ++i) {
                arr[i] = bag.GetEntity(i).GetRotation();
            }
            return arr;

        }

        private NativeArray<float3> GetParentPositionsFromBag(Bag bag) {
            
            var arr = new NativeArray<float3>(bag.Length, Allocator.TempJob);
            for (int i = 0; i < bag.Length; ++i) {
                arr[i] = bag.GetEntity(i).GetPosition();
            }
            return arr;
            
        }

    }
    
}