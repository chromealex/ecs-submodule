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

            public FPVector2 position;
            public FPVector2 velocity;
            public FPVector2 targetPos;

            public pfloat speed;
            public pfloat radius;
            public pfloat mass;

            public byte isStatic;
            public int layer;
            public int collisionMask;
            public uint pushLayer;

            //public ME.ECS.Collections.StackArray10<float> sensorsLength;

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

                    var entityIdA = i;
                    ref readonly var a = ref this.bodies.ReadT0(entityIdA);
                    for (var m = 0; m < this.obstacles.Length; ++m) {

                        if (this.output[0] >= this.bodyVsObstacleCollisionsBuffer.Length) break;

                        var entityIdB = m;
                        ref readonly var b = ref this.obstacles.ReadT0(entityIdB);

                        var difference = a.position - b.position;
                        var closest = new FPVector2(b.position.x + FPMath.Clamp(difference.x, -b.extents.x, b.extents.x),
                                                    b.position.y + FPMath.Clamp(difference.y, -b.extents.y, b.extents.y));
                        difference = closest - a.position;

                        if (difference.sqrMagnitude < a.radius * a.radius) {
                            var distance = difference.magnitude;
                            var normal = distance > 0f ? difference / distance : new FPVector2(1f, 0f);
                            var depth = FPMath.Abs(distance - a.radius);
                            
                            var idx = this.output[0];
                            var found = false;
                            for (int k = 0; k < idx; ++k) {
                                
                                var item = this.bodyVsObstacleCollisionsBuffer[k];
                                if ((item.a == entityIdA && item.b == entityIdB) ||
                                    (item.a == entityIdB && item.b == entityIdA)) {
                                    
                                    found = true;
                                    break;
                                    
                                }
                                
                            }

                            if (found == false) {

                                this.bodyVsObstacleCollisionsBuffer[idx] = new CollisionManifoldBodyVsObstacle
                                    { a = entityIdA, b = entityIdB, depth = depth, normal = normal };
                                this.output[0]++;

                            }

                            break;
                        }

                    }

                    for (var j = 0; j < this.bodies.Length; ++j) {

                        if (j == i) continue;
                        if (this.output[1] >= this.bodyVsBodyCollisionsBuffer.Length) break;

                        var entityIdB = j;
                        ref readonly var b = ref this.bodies.ReadT0(entityIdB);

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

                            var idx = this.output[1];
                            var found = false;
                            for (int k = 0; k < idx; ++k) {
                                
                                var item = this.bodyVsBodyCollisionsBuffer[k];
                                if ((item.a == entityIdA && item.b == entityIdB) ||
                                    (item.a == entityIdB && item.b == entityIdA)) {
                                    
                                    found = true;
                                    break;
                                    
                                }
                                
                            }

                            if (found == false) {

                                this.bodyVsBodyCollisionsBuffer[idx] = new CollisionManifoldBodyVsBody
                                    { a = entityIdA, b = entityIdB, depth = depth, normal = normal };
                                this.output[1]++;

                            }
                            
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
            
            [Unity.Collections.NativeDisableParallelForRestriction] public Unity.Collections.NativeArray<CollisionManifoldBodyVsObstacle> items;
            
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
            
            [Unity.Collections.NativeDisableParallelForRestriction] public Unity.Collections.NativeArray<CollisionManifoldBodyVsBody> items;
            
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
                
                var entityId = index;
                ref var body = ref this.bodies.GetT0(entityId);
                body.velocity *= this.velocityDumping;
                
            }

        }

        [Unity.Burst.BurstCompileAttribute(Unity.Burst.FloatPrecision.High, Unity.Burst.FloatMode.Deterministic, CompileSynchronously = true, Debug = false)]
        public struct MoveBodies : Unity.Jobs.IJobParallelFor {

            public pfloat substepDeltaTime;
            public ME.ECS.Buffers.FilterBag<Body> bodies;
            
            public void Execute(int index) {

                ref var body = ref this.bodies.GetT0(index);
                if (body.isStatic == 0) {
                    
                    body.position += body.velocity * this.substepDeltaTime;
                    
                }
                
            }

        }

        public static Body CreateBody(Vector2 position, Vector2 velocity, float radius = 1f, float mass = MotionSolver.DEFAULT_BODY_MASS) {
            
            return new Body {
                position = position, velocity = velocity, radius = radius, mass = mass, layer = MotionSolver.DEFAULT_COLLISION_LAYER, collisionMask = MotionSolver.DEFAULT_COLLISION_MASK,
                //sensorsLength = new ME.ECS.Collections.StackArray10<float>(8),
            };
            
        }

        /*
        public static void SimulateRVO(Filter bodies) {

            const float step = 45f;
            const float stepSize = 45f;
            
            foreach (var agent in bodies) {

                ref var body = ref agent.Get<Body>();
                var targetPos = body.targetPos;
                var pos = body.position;
                var forwardDir = body.velocity;
                var sensorLength = body.radius * 4f;

                { // sensor length per dir

                    var sensorLengthSqr = sensorLength * sensorLength;
                    for (int i = 0; i < body.sensorsLength.Length; ++i) {

                        var sensorDir = FPVector2.Rotate(forwardDir, step * i);
                        var angle = FPMath.Abs(Vector2.SignedAngle(sensorDir, forwardDir));
                        body.sensorsLength[i] = (360f - angle) / 360f * sensorLength;

                    }

                    foreach (var other in bodies) {

                        if (agent == other) continue;

                        ref readonly var otherBody = ref other.Read<Body>();
                        // TODO: Calc more accurate iterative prediction
                        var agentPos = otherBody.position + otherBody.velocity * ((otherBody.position - pos).magnitude / body.speed);
                        var agentNearestPos = agentPos + (pos - agentPos).normalized * otherBody.radius;
                        var dir = (agentNearestPos - pos);
                        var dist = dir.sqrMagnitude;
                        if (dist <= sensorLengthSqr) {

                            for (int i = 0; i < body.sensorsLength.Length; ++i) {

                                var d = FPVector2.Rotate(forwardDir, step * i);
                                var angle = FPMath.Abs(Vector2.SignedAngle(dir, d));
                                if (angle <= stepSize) {

                                    var len = angle / step * sensorLength;
                                    len = FPMath.Min(len, FPMath.Sqrt(dist));
                                    body.sensorsLength[i] = FPMath.Min(body.sensorsLength[i], len);

                                }

                            }

                        }

                    }

                }

                { // make decision
                    
                    var targetDir = targetPos - pos;
                    var dirLenIdx = -1;
                    var dirAngleIdx = -1;
                    var max = 0f;
                    var nearestAngle = float.MaxValue;
                    for (int i = 0; i < body.sensorsLength.Length; ++i) {

                        var len = body.sensorsLength[i];
                        if (len >= max) {

                            max = len;
                            dirLenIdx = i;

                        }

                        var sensorDir = FPVector2.Rotate(forwardDir, step * i);
                        var angle = FPMath.Repeat(Vector2.SignedAngle(sensorDir, targetDir), 360f);
                        if (angle <= nearestAngle && len >= sensorLength * 0.3f) {

                            nearestAngle = angle;
                            dirAngleIdx = i;

                        }

                    }

                    var dirIdx = -1;
                    if (dirAngleIdx == dirLenIdx) {

                        dirIdx = dirLenIdx;

                    } else {

                        dirIdx = (dirAngleIdx >= 0 ? dirAngleIdx : dirLenIdx);

                    }

                    var d = FPQuaternion.Euler(0f, 0f, step * dirIdx) * forwardDir;
                    body.velocity = d * body.speed;
                    
                }

            }

        }
*/

        public static void Step(Filter bodies, float deltaTime, in SimulationConfig config) {
            
            MotionSolver.Step(bodies, Filter.Empty, deltaTime, config);
            
        }

        public static void Step(Filter bodies, Filter obstacles, float deltaTime, in SimulationConfig config) {

            //MotionSolver.SimulateRVO(bodies);

            //UnityEngine.Debug.Log(Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable<Body>());
            //UnityEngine.Debug.Log(Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable<ME.ECS.Collections.StackArray10<float>>());
            //UnityEngine.Debug.Log(Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable<MoveBodies>());
            //UnityEngine.Debug.Log(Unity.Collections.LowLevel.Unsafe.UnsafeUtility.IsBlittable<Obstacle>());

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

        }

        private static void ResolveCollision(ref Body a, ref Body b, pfloat depth, FPVector2 normal, pfloat collisionDumping, int aIsStatic, int bIsStatic) {

            if (a.isStatic == 1 || aIsStatic == 1) {
                b.position += depth * normal;
            } else if (b.isStatic == 1 || bIsStatic == 1) {
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
