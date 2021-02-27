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

        internal struct CollisionManifold {

            public int a;
            public int b;

            public Vector2 normal;
            public float depth;

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

        public static void Step(ME.ECS.Collections.BufferArray<Body> bodies, float deltaTime,
                                float collisionDumping = MotionSolver.DEFAULT_COLLISION_DUMPING,
                                float dynamicDumping = MotionSolver.DEFAULT_DYNAMIC_DUMPING) {

            MotionSolver.Step(bodies, ME.ECS.Collections.BufferArray<Obstacle>.Empty, deltaTime, collisionDumping, dynamicDumping);
        }

        public static void Step(ME.ECS.Collections.BufferArray<Body> bodies, ME.ECS.Collections.BufferArray<Obstacle> obstacles, float deltaTime,
                                float collisionDumping = MotionSolver.DEFAULT_COLLISION_DUMPING,
                                float dynamicDumping = MotionSolver.DEFAULT_DYNAMIC_DUMPING) {
            MotionSolver.bodyVsBodyCollisionsBuffer.Clear();
            MotionSolver.bodyVsObstacleCollisionsBuffer.Clear();

            for (var i = 0; i < bodies.Length; i++) {
                ref var body = ref bodies.arr[i];
                if (body.isStatic == false) {
                    body.position += body.velocity * deltaTime;
                }
            }

            for (var i = 0; i < bodies.Length; i++) {
                ref var a = ref bodies.arr[i];

                for (int m = 0; m < obstacles.Length; m++) {
                    ref var b = ref obstacles.arr[m];

                    var difference = a.position - b.position;
                    var closest = new Vector2(b.position.x + Mathf.Clamp(difference.x, -b.extents.x, b.extents.x), b.position.y + Mathf.Clamp(difference.y, -b.extents.y, b.extents.y));
                    difference = closest - a.position;

                    if (difference.sqrMagnitude < a.radius * a.radius) {
                        var distance = difference.magnitude;
                        var normal = distance > 0f ? difference / distance : Vector2.right;
                        var depth = Mathf.Abs(distance - a.radius);

                        MotionSolver.bodyVsObstacleCollisionsBuffer.Add(new CollisionManifold { a = i, b = m, depth = depth, normal = normal });
                    }
                }

                for (var j = i + 1; j < bodies.Length; j++) {
                    ref var b = ref bodies.arr[j];

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

                        MotionSolver.bodyVsBodyCollisionsBuffer.Add(new CollisionManifold { a = i, b = j, depth = depth, normal = normal });
                    }
                }
            }

            for (var i = 0; i < MotionSolver.bodyVsBodyCollisionsBuffer.Count; i++) {
                var c = MotionSolver.bodyVsBodyCollisionsBuffer[i];
                ref var a = ref bodies.arr[c.a];
                ref var b = ref bodies.arr[c.b];

                MotionSolver.ResolveCollision(ref a, ref b, c.depth, c.normal, collisionDumping);
            }

            for (var i = 0; i < MotionSolver.bodyVsObstacleCollisionsBuffer.Count; i++) {
                var c = MotionSolver.bodyVsObstacleCollisionsBuffer[i];
                ref var a = ref bodies.arr[c.a];
                ref var b = ref obstacles.arr[c.b];

                MotionSolver.ResolveCollision(ref a, ref b, c.depth, c.normal, collisionDumping);
            }

            if (dynamicDumping > 0f) {
                var velocityDumping = Mathf.Pow(dynamicDumping, deltaTime);

                for (var i = 0; i < bodies.Length; i++) {
                    ref var body = ref bodies.arr[i];
                    body.velocity *= velocityDumping;
                }
            }
        }

        private static void ResolveCollision(ref Body a, ref Body b, float depth, Vector2 normal, float collisionDumping) {
            var pushAllow = (a.pushLayer == b.pushLayer);
            if (pushAllow == false) {
                return;
            } else if (a.isStatic == true) {
                b.position += depth * normal;
            } else if (b.isStatic == true) {
                a.position -= depth * normal;
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
