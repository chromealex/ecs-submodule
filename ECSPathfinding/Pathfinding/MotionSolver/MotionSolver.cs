using System.Collections.Generic;

using UnityEngine;

namespace ME.ECS.Pathfinding {

    public static class MotionSolver {

        private static readonly List<CollisionManifold> bodyVsBodyCollisionsBuffer = new List<CollisionManifold>(100);
        private static readonly List<CollisionManifold> bodyVsObstacleCollisionsBuffer = new List<CollisionManifold>(100);

        public const float DEFAULT_BODY_MASS = 1f;
        public const float DEFAULT_COLLISION_DUMPING = 0.65f;
        public const float DEFAULT_DYNAMIC_DUMPING = 0.15f;

        public const int DEFAULT_COLLISION_LAYER = 1 << 0;
        public const int DEFAULT_COLLISION_MASK = -1;

        public static readonly SimulationConfig defaultSimulationConfig = new SimulationConfig {
            collisionDumping = DEFAULT_COLLISION_DUMPING,
            dynamicDumping = DEFAULT_DYNAMIC_DUMPING,
            substeps = 1
        };

        internal struct CollisionManifold {

            public int a;
            public int b;

            public Vector2 normal;
            public float depth;

        }

        [System.Serializable]
        public struct SimulationConfig {
            public float collisionDumping;
            public float dynamicDumping;

            public int substeps;
        }

        public struct Body : IStructComponent {

            public bool isStatic;
            public Vector2 position;
            public Vector2 velocity;

            public float radius;
            public float mass;

            public int layer;
            public int collisionMask;

            public uint pushLayer;

        }

        public struct Obstacle : IStructComponent {

            public Vector2 position;
            public Vector2 extents;

        }

        public static Body CreateBody(Vector2 position, Vector2 velocity, float radius = 1f, float mass = MotionSolver.DEFAULT_BODY_MASS) {
            return new Body { position = position, velocity = velocity, radius = radius, mass = mass, layer = MotionSolver.DEFAULT_COLLISION_LAYER, collisionMask = MotionSolver.DEFAULT_COLLISION_MASK };
        }

        public static void Step(Filter bodies, float deltaTime, in SimulationConfig config) {

            MotionSolver.Step(bodies, Filter.Empty, deltaTime, config);
        }

        public static void Step(Filter bodies, Filter obstacles, float deltaTime, in SimulationConfig config) {

            MotionSolver.bodyVsBodyCollisionsBuffer.Clear();
            MotionSolver.bodyVsObstacleCollisionsBuffer.Clear();

            //int bodyMin = 0, bodyMax = -1, obstacleMin = 0, obstacleMax = -1;

            var bagBodies = new ME.ECS.Buffers.FilterBag<Body>(bodies, Unity.Collections.Allocator.Persistent);
            var bagBodies2 = new ME.ECS.Buffers.FilterBag<Body>(bodies, Unity.Collections.Allocator.Persistent);
            var bagObstacles = new ME.ECS.Buffers.FilterBag<Obstacle>(obstacles, Unity.Collections.Allocator.Persistent);
            
            /*if (bodies.IsAlive() == true && bodies.Count > 0) {
                bodies.GetBounds(out bodyMin, out bodyMax);
            }

            if (obstacles.IsAlive() == true && obstacles.Count > 0) {
                obstacles.GetBounds(out obstacleMin, out obstacleMax);
            }*/

            var substepDeltaTime = deltaTime / (float)config.substeps;
            for (int substep = 0; substep < config.substeps; substep++) {
                
                var applyVelocity = false;
                var velocityDumping = 0f;
                if (config.dynamicDumping > 0f) {
                    velocityDumping = Mathf.Pow(config.dynamicDumping, substepDeltaTime);
                    applyVelocity = true;
                }

                bagBodies.Reset();
                for (int i = 0; i < bagBodies.Length; ++i) {

                    bagBodies.MoveNext();
                    ref var body = ref bagBodies.GetT0();
                    if (body.isStatic == false) {
                        body.position += body.velocity * substepDeltaTime;
                    }
                    if (applyVelocity == true) body.velocity *= velocityDumping;
                    
                }

                bagBodies.Reset();
                for (int i = 0; i < bagBodies.Length; ++i) {
                    
                    bagBodies.MoveNext();
                    ref readonly var a = ref bagBodies.ReadT0();
                    ref var entityA = ref bagBodies.Get();
                    bagObstacles.Reset();
                    for (int m = 0; m < bagObstacles.Length; ++m) {

                        bagObstacles.MoveNext();
                        ref readonly var b = ref bagObstacles.ReadT0();

                        var difference = a.position - b.position;
                        var closest = new Vector2(b.position.x + Mathf.Clamp(difference.x, -b.extents.x, b.extents.x), b.position.y + Mathf.Clamp(difference.y, -b.extents.y, b.extents.y));
                        difference = closest - a.position;

                        if (difference.sqrMagnitude < a.radius * a.radius) {
                            var distance = difference.magnitude;
                            var normal = distance > 0f ? difference / distance : Vector2.right;
                            var depth = Mathf.Abs(distance - a.radius);

                            MotionSolver.bodyVsObstacleCollisionsBuffer.Add(new CollisionManifold { a = bagBodies.index, b = bagObstacles.index, depth = depth, normal = normal });
                        }
                    }
                    
                    bagBodies2.Reset();
                    for (var j = 0; j < bagBodies2.Length; j++) {

                        bagBodies2.MoveNext();
                        ref readonly var b = ref bagBodies2.ReadT0();

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

                            MotionSolver.bodyVsBodyCollisionsBuffer.Add(new CollisionManifold { a = bagBodies.index, b = bagBodies2.index, depth = depth, normal = normal });
                        }
                    }
                    
                }

                for (var i = 0; i < MotionSolver.bodyVsBodyCollisionsBuffer.Count; i++) {
                    
                    var c = MotionSolver.bodyVsBodyCollisionsBuffer[i];
                    ref var a = ref bagBodies.GetT0(c.a);
                    ref var b = ref bagBodies.GetT0(c.b);
                    
                    MotionSolver.ResolveCollision(ref a, ref b, c.depth, c.normal, config.collisionDumping);
                    
                }

                for (var i = 0; i < MotionSolver.bodyVsObstacleCollisionsBuffer.Count; i++) {
                    
                    var c = MotionSolver.bodyVsObstacleCollisionsBuffer[i];
                    ref var a = ref bagBodies.GetT0(c.a);
                    ref var b = ref bagObstacles.GetT0(c.b);

                    MotionSolver.ResolveCollision(ref a, ref b, c.depth, c.normal, config.collisionDumping);
                    
                }

            }
            
            bagBodies.Push();
            bagObstacles.Revert();
            bagBodies2.Revert();
            
        }

        private static void ResolveCollision(ref Body a, ref Body b, float depth, Vector2 normal, float collisionDumping) {

            if (a.isStatic == true) {
                b.position += depth * normal;
            } else if (b.isStatic == true) {
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

            if (Vector2.Dot(b.velocity - a.velocity, normal) > 0) {
                return;
            }

            var tangent = new Vector2(-normal.y, normal.x);

            var dotTanA = Vector2.Dot(a.velocity, tangent);
            var dotTanB = Vector2.Dot(b.velocity, tangent);

            var dotNormalA = Vector2.Dot(a.velocity, normal);
            var dotNormalB = Vector2.Dot(b.velocity, normal);

            var mA = (dotNormalA * (a.mass - b.mass) + 2.0f * b.mass * dotNormalB) / (a.mass + b.mass);
            var mB = (dotNormalB * (b.mass - a.mass) + 2.0f * a.mass * dotNormalA) / (a.mass + b.mass);

            a.velocity = tangent * dotTanA + normal * (mA * collisionDumping);
            b.velocity = tangent * dotTanB + normal * (mB * collisionDumping);
        }

        private static void ResolveCollision(ref Body a, ref Obstacle b, float depth, Vector2 normal, float collisionDumping) {
            a.position -= depth * normal;
        }

    }

}
