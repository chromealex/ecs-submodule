using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace ME.ECS.Pathfinding {

    public static class MotionSolver {

        //private static readonly List<CollisionManifold> bodyVsBodyCollisionsBuffer = new List<CollisionManifold>(100);
        //private static readonly List<CollisionManifold> bodyVsObstacleCollisionsBuffer = new List<CollisionManifold>(100);

        public const float DEFAULT_BODY_MASS = 1f;
        public const float DEFAULT_COLLISION_DUMPING = 0.65f;
        public const float DEFAULT_DYNAMIC_DUMPING = 0.15f;

        public const int DEFAULT_COLLISION_LAYER = 1 << 0;
        public const int DEFAULT_COLLISION_MASK = -1;

        public static readonly SimulationConfig defaultSimulationConfig = new SimulationConfig {
            collisionDumping = DEFAULT_COLLISION_DUMPING,
            dynamicDumping = DEFAULT_DYNAMIC_DUMPING,
            substeps = 1,
            maxBodyVsBodyCollisions = 100,
            maxBodyVsObstaclesCollisions = 100,
        };

        public struct CollisionManifoldBodyVsObstacle {

            public int a;
            public int b;

            public FPVector2 normal;
            public pfloat depth;

        }

        public struct CollisionManifoldBodyVsBody {

            public int a;
            public int b;

            public FPVector2 normal;
            public pfloat depth;

        }

        [System.Serializable]
        public struct SimulationConfig {
        
            public pfloat collisionDumping;
            public pfloat dynamicDumping;

            public int substeps;
            
            public int maxBodyVsBodyCollisions;
            public int maxBodyVsObstaclesCollisions;
            
        }

        public struct Body : IStructComponent {

            public bool isStatic;
            public FPVector2 position;
            public FPVector2 velocity;

            public pfloat radius;
            public pfloat mass;

            public int layer;
            public int collisionMask;

            public uint pushLayer;

        }

        public struct Obstacle : IStructComponent {

            public FPVector2 position;
            public FPVector2 extents;

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        public struct ResolverJob : IJob {

            public ME.ECS.Buffers.FilterBag<Body> bodies;
            public ME.ECS.Buffers.FilterBag<Obstacle> obstacles;

            [Unity.Collections.NativeDisableParallelForRestriction] public Unity.Collections.NativeArray<CollisionManifoldBodyVsObstacle> bodyVsObstacleCollisionsBuffer;
            [Unity.Collections.NativeDisableParallelForRestriction] public Unity.Collections.NativeArray<CollisionManifoldBodyVsBody> bodyVsBodyCollisionsBuffer;
            [Unity.Collections.NativeDisableParallelForRestriction] public Unity.Collections.NativeArray<int> output;
            
            public void Execute() {

                for (var i = 0; i < this.bodies.Length; ++i) {

                    ref readonly var a = ref this.bodies.ReadT0(i);
                    for (var m = 0; m < this.obstacles.Length; ++m) {

                        if (this.output[0] >= this.bodyVsObstacleCollisionsBuffer.Length) break;

                        ref readonly var b = ref this.obstacles.ReadT0(m);

                        var difference = a.position - b.position;
                        var closest = new FPVector2(b.position.x + FPMath.Clamp(difference.x, -b.extents.x, b.extents.x),
                                                    b.position.y + FPMath.Clamp(difference.y, -b.extents.y, b.extents.y));
                        difference = closest - a.position;

                        if (difference.sqrMagnitude < a.radius * a.radius) {
                            var distance = difference.magnitude;
                            var normal = distance > 0f ? difference / distance : new FPVector2(1f, 0f);
                            var depth = FPMath.Abs(distance - a.radius);

                            this.bodyVsObstacleCollisionsBuffer[this.output[0]++] = new CollisionManifoldBodyVsObstacle
                                { a = i, b = m, depth = depth, normal = normal };
                            break;
                        }

                    }

                    for (var j = 0; j < this.bodies.Length; ++j) {

                        if (j == i) continue;
                        if (this.output[1] >= this.bodyVsBodyCollisionsBuffer.Length) break;

                        ref readonly var b = ref this.bodies.ReadT0(j);

                        if ((a.collisionMask & b.layer) == 0 && (b.layer & a.collisionMask) == 0) {
                            continue;
                        }

                        var collisionDistance = a.radius + b.radius;
                        var collisionDistanceSqr = collisionDistance * collisionDistance;
                        var difference = b.position - a.position;

                        if (difference.sqrMagnitude < collisionDistanceSqr) {

                            var distance = difference.magnitude;
                            var normal = distance > 0f ? difference / distance : new FPVector2(1f, 0f);
                            var depth = FPMath.Abs(distance - collisionDistance);

                            this.bodyVsBodyCollisionsBuffer[this.output[1]++] =
                                new CollisionManifoldBodyVsBody { a = i, b = j, depth = depth, normal = normal };

                        }

                    }

                }

            }

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        public struct ResolveCollisionsBodyVsObstacleJob : Unity.Jobs.IJobParallelFor {

            public pfloat collisionDumping;
            public ME.ECS.Buffers.FilterBag<Body> bodies;
            public ME.ECS.Buffers.FilterBag<Obstacle> obstacles;
            
            public Unity.Collections.NativeArray<CollisionManifoldBodyVsObstacle> items;
            
            public void Execute(int index) {
                
                var c = this.items[index];
                ref var a = ref this.bodies.GetT0(c.a);
                ref var b = ref this.obstacles.GetT0(c.b);

                MotionSolver.ResolveCollision(ref a, ref b, c.depth, c.normal, this.collisionDumping);
                
            }

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        public struct ResolveCollisionsBodyVsBodyJob : Unity.Jobs.IJobParallelFor {

            public pfloat collisionDumping;
            public ME.ECS.Buffers.FilterBag<Body> bodies;
            
            public Unity.Collections.NativeArray<CollisionManifoldBodyVsBody> items;
            
            public void Execute(int index) {
                
                var c = this.items[index];
                ref var a = ref this.bodies.GetT0(c.a);
                ref var b = ref this.bodies.GetT0(c.b);
                    
                MotionSolver.ResolveCollision(ref a, ref b, c.depth, c.normal, this.collisionDumping, a.isStatic, b.isStatic);

            }

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        public struct ResolveDynamicDumping : Unity.Jobs.IJobParallelFor {

            public pfloat velocityDumping;
            public ME.ECS.Buffers.FilterBag<Body> bodies;
            
            public void Execute(int index) {
                
                ref var body = ref this.bodies.GetT0(index);
                body.velocity *= this.velocityDumping;
                
            }

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        public struct MoveBodies : Unity.Jobs.IJobParallelFor {

            public pfloat substepDeltaTime;
            public ME.ECS.Buffers.FilterBag<Body> bodies;
            
            public void Execute(int index) {
                   
                ref var body = ref this.bodies.GetT0(index);
                if (body.isStatic == false) {
                    
                    body.position += body.velocity * this.substepDeltaTime;
                    
                }
                
            }

        }

        public static Body CreateBody(Vector2 position, Vector2 velocity, float radius = 1f, float mass = MotionSolver.DEFAULT_BODY_MASS) {
            return new Body { position = position, velocity = velocity, radius = radius, mass = mass, layer = MotionSolver.DEFAULT_COLLISION_LAYER, collisionMask = MotionSolver.DEFAULT_COLLISION_MASK };
        }

        public static void Step(Filter bodies, float deltaTime, in SimulationConfig config) {

            MotionSolver.Step(bodies, Filter.Empty, deltaTime, config);
        }

        public static void Step(Filter bodies, Filter obstacles, float deltaTime, in SimulationConfig config) {

            var bodiesBag = new ME.ECS.Buffers.FilterBag<Body>(bodies, Unity.Collections.Allocator.TempJob);
            var obstaclesBag = new ME.ECS.Buffers.FilterBag<Obstacle>(obstacles, Unity.Collections.Allocator.TempJob);
            var bodyVsBody = new Unity.Collections.NativeArray<CollisionManifoldBodyVsBody>(config.maxBodyVsBodyCollisions, Unity.Collections.Allocator.TempJob);
            var bodyVsObstacle = new Unity.Collections.NativeArray<CollisionManifoldBodyVsObstacle>(config.maxBodyVsObstaclesCollisions, Unity.Collections.Allocator.TempJob);
            var results = new Unity.Collections.NativeArray<int>(2, Unity.Collections.Allocator.TempJob);
            var substepDeltaTime = deltaTime / config.substeps;

            {
                
                var jobMoveBodies = new MoveBodies() {
                    bodies = bodiesBag,
                    substepDeltaTime = substepDeltaTime,
                };
                var handleJobMoveBodies = jobMoveBodies.Schedule(bodiesBag.Length, 64);
                handleJobMoveBodies.Complete();
                
            }

            var jobResolver = new ResolverJob() {
                bodies = bodiesBag,
                obstacles = obstaclesBag,
                bodyVsBodyCollisionsBuffer = bodyVsBody,
                bodyVsObstacleCollisionsBuffer = bodyVsObstacle,
                output = results,
            };
            var handleResolver = jobResolver.Schedule();
            handleResolver.Complete();
            
            {
                
                var jobCollisionBodyVsObstacle = new ResolveCollisionsBodyVsObstacleJob() {
                    collisionDumping = config.collisionDumping,
                    bodies = bodiesBag,
                    obstacles = obstaclesBag,
                    items = bodyVsObstacle,
                };
                var handleJobCollisionBodyVsObstacle = jobCollisionBodyVsObstacle.Schedule(jobResolver.output[0], 64);
                handleJobCollisionBodyVsObstacle.Complete();
                
                var jobCollisionBodyVsBody = new ResolveCollisionsBodyVsBodyJob() {
                    collisionDumping = config.collisionDumping,
                    bodies = bodiesBag,
                    items = bodyVsBody,
                };
                var handleJobCollisionBodyVsBody = jobCollisionBodyVsBody.Schedule(jobResolver.output[1], 64);
                handleJobCollisionBodyVsBody.Complete();
                
                //JobHandle.CompleteAll(ref handleJobCollisionBodyVsBody, ref handleJobCollisionBodyVsObstacle);
                
            }

            if (config.dynamicDumping > 0f) {
                
                var jobCollisionBodyVsObstacle = new ResolveDynamicDumping() {
                    bodies = bodiesBag,
                    velocityDumping = FPMath.Pow(config.dynamicDumping, substepDeltaTime),
                };
                var handle = jobCollisionBodyVsObstacle.Schedule(bodiesBag.Length, 64);
                handle.Complete();
                
            }

            bodyVsBody.Dispose();
            bodyVsObstacle.Dispose();
            results.Dispose();

            bodiesBag.Push();
            obstaclesBag.Push();

            /*return;

            MotionSolver.bodyVsBodyCollisionsBuffer.Clear();
            MotionSolver.bodyVsObstacleCollisionsBuffer.Clear();

            int bodyMin = 0, bodyMax = -1, obstacleMin = 0, obstacleMax = -1;

            if (bodies.IsAlive() == true && bodies.Count > 0) {
                bodies.GetBounds(out bodyMin, out bodyMax);
            }

            if (obstacles.IsAlive() == true && obstacles.Count > 0) {
                obstacles.GetBounds(out obstacleMin, out obstacleMax);
            }

            var world = Worlds.currentWorld;

            for (int substep = 0; substep < config.substeps; substep++) {
                for (var i = bodyMin; i <= bodyMax; i++) {
                    ref var entity = ref world.GetEntityById(i);

                    if (entity.IsEmpty() == false) {
                        ref var body = ref entity.GetData<Body>(createIfNotExists: false);
                        body.collidedWithObstacle = false;
                        if (body.isStatic == false) {
                            body.position += body.velocity * substepDeltaTime;
                        }
                    }
                }

                for (var i = bodyMin; i <= bodyMax; i++) {
                    ref var entityA = ref world.GetEntityById(i);

                    if (entityA.IsEmpty() == true) {
                        continue;
                    }

                    ref var a = ref entityA.GetData<Body>(createIfNotExists: false);

                    if (a.collidedWithObstacle == false) {

                        for (int m = obstacleMin; m <= obstacleMax; m++) {
                            ref var entityB = ref world.GetEntityById(m);

                            if (entityB.IsEmpty() == true) {
                                continue;
                            }

                            ref var b = ref entityB.GetData<Obstacle>(createIfNotExists: false);

                            var difference = a.position - b.position;
                            var closest = new Vector2(b.position.x + Mathf.Clamp(difference.x, -b.extents.x, b.extents.x),
                                                      b.position.y + Mathf.Clamp(difference.y, -b.extents.y, b.extents.y));
                            difference = closest - a.position;

                            if (difference.sqrMagnitude < a.radius * a.radius) {
                                var distance = difference.magnitude;
                                var normal = distance > 0f ? difference / distance : Vector2.right;
                                var depth = Mathf.Abs(distance - a.radius);

                                a.collidedWithObstacle = true;
                                MotionSolver.bodyVsObstacleCollisionsBuffer.Add(new CollisionManifold { a = entityA, b = entityB, depth = depth, normal = normal });
                                break;
                            }
                        }

                    }

                    for (var j = i + 1; j <= bodyMax; j++) {

                        ref var entityB = ref world.GetEntityById(j);

                        if (entityB.IsEmpty() == true) {
                            continue;
                        }

                        ref var b = ref entityB.GetData<Body>(createIfNotExists: false);
                        
                        if ((a.collisionMask & b.layer) == 0 && (b.layer & a.collisionMask) == 0) {
                            continue;
                        }

                        var collisionDistance = a.radius + b.radius;
                        var collisionDistanceSqr = collisionDistance * collisionDistance;
                        var difference = b.position - a.position;

                        if (difference.sqrMagnitude < collisionDistanceSqr) {
                            var distance = difference.magnitude;
                            var normal = distance > 0f ? difference / distance : Vector2.right;
                            var depth = Mathf.Abs(distance - collisionDistance);

                            MotionSolver.bodyVsBodyCollisionsBuffer.Add(new CollisionManifold { a = entityA, b = entityB, depth = depth, normal = normal });
                            
                        }
                    }
                }

                for (var i = 0; i < MotionSolver.bodyVsObstacleCollisionsBuffer.Count; i++) {
                    var c = MotionSolver.bodyVsObstacleCollisionsBuffer[i];
                    ref var a = ref c.a.GetData<Body>(createIfNotExists: false);
                    ref var b = ref c.b.GetData<Obstacle>(createIfNotExists: false);
                    a.collidedWithObstacle = true;

                    MotionSolver.ResolveCollision(ref a, ref b, c.depth, c.normal, config.collisionDumping);
                }

                for (var i = 0; i < MotionSolver.bodyVsBodyCollisionsBuffer.Count; i++) {
                    var c = MotionSolver.bodyVsBodyCollisionsBuffer[i];
                    ref var a = ref c.a.GetData<Body>(createIfNotExists: false);
                    ref var b = ref c.b.GetData<Body>(createIfNotExists: false);
                    
                    MotionSolver.ResolveCollision(ref a, ref b, c.depth, c.normal, config.collisionDumping, a.collidedWithObstacle, b.collidedWithObstacle);
                }

                if (config.dynamicDumping > 0f) {
                    var velocityDumping = Mathf.Pow(config.dynamicDumping, substepDeltaTime);

                    for (var i = bodyMin; i <= bodyMax; i++) {
                        ref var entity = ref world.GetEntityById(i);

                        if (entity.IsEmpty() == false) {
                            ref var body = ref entity.GetData<Body>(createIfNotExists: false);
                            body.velocity *= velocityDumping;
                        }
                    }
                }
            } */
        }

        private static void ResolveCollision(ref Body a, ref Body b, pfloat depth, FPVector2 normal, pfloat collisionDumping, bool aIsStatic, bool bIsStatic) {

            if (a.isStatic == true || aIsStatic == true) {
                b.position += depth * normal;
            } else if (b.isStatic == true || bIsStatic == true) {
                a.position -= depth * normal;
            } else if (a.pushLayer != b.pushLayer) {
                if (a.velocity.sqrMagnitude > b.velocity.sqrMagnitude) {
                    a.position -= depth * normal;
                } else {
                    b.position += depth * normal;
                }
                return;
            } else {
                var displace = 0.5f * depth * normal;

                a.position -= displace;
                b.position += displace;
            }

            if (FPVector2.Dot(b.velocity - a.velocity, normal) > 0) {
                return;
            }

            var tangent = new FPVector2(-normal.y, normal.x);

            var dotTanA = FPVector2.Dot(a.velocity, tangent);
            var dotTanB = FPVector2.Dot(b.velocity, tangent);

            var dotNormalA = FPVector2.Dot(a.velocity, normal);
            var dotNormalB = FPVector2.Dot(b.velocity, normal);

            var mA = (dotNormalA * (a.mass - b.mass) + 2.0f * b.mass * dotNormalB) / (a.mass + b.mass);
            var mB = (dotNormalB * (b.mass - a.mass) + 2.0f * a.mass * dotNormalA) / (a.mass + b.mass);

            a.velocity = tangent * dotTanA + normal * (mA * collisionDumping);
            b.velocity = tangent * dotTanB + normal * (mB * collisionDumping);
        }

        private static void ResolveCollision(ref Body a, ref Obstacle b, pfloat depth, FPVector2 normal, pfloat collisionDumping) {

            a.position -= depth * normal;
            
        }

    }

}
