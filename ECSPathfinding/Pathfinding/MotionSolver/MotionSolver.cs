using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Pathfinding {

    public static class MotionSolver {

        private static readonly List<CollisionManifold> collisionsBuffer = new List<CollisionManifold>(100);

        internal const float DEFAULT_BODY_MASS = 1f;
        internal const float DEFAULT_COLLISION_DUMPING = 0.65f;
        internal const float DEFAULT_DYNAMIC_DUMPING = 0.15f;

        internal const int DEFAULT_COLLISION_LAYER = 1 << 0;
        internal const int DEFAULT_COLLISION_MASK = -1;

        internal struct CollisionManifold {

            public int a;
            public int b;

            public Vector2 normal;
            public float depth;

        }

        public struct Body {

            public bool isStatic;
            public Vector2 position;
            public Vector2 velocity;

            public float radius;
            public float mass;

            public int layer;
            public int collisionMask;
        }

        public static Body CreateBody(Vector2 position, Vector2 velocity, float radius = 1f, float mass = MotionSolver.DEFAULT_BODY_MASS) {
            return new Body { position = position, velocity = velocity, radius = radius, mass = mass, layer = DEFAULT_COLLISION_LAYER, collisionMask = DEFAULT_COLLISION_MASK };
        }

        public static void Step(ME.ECS.Collections.BufferArray<Body> bodies, float deltaTime, float collisionDumping = MotionSolver.DEFAULT_COLLISION_DUMPING, float dynamicDumping = MotionSolver.DEFAULT_DYNAMIC_DUMPING) {
            MotionSolver.collisionsBuffer.Clear();

            for (int i = 0; i < bodies.Length; i++) {
                ref var body = ref bodies.arr[i];
                if (body.isStatic == false) body.position += body.velocity * deltaTime;
            }

            for (int i = 0; i < bodies.Length; i++) {
                ref var a = ref bodies.arr[i];
                for (int j = i + 1; j < bodies.Length; j++) {
                    ref var b = ref bodies.arr[j];

                    if ((a.collisionMask & b.layer) == 0 && (b.layer & a.collisionMask) == 0) {
                        continue;
                    }

                    var collisionDistance = a.radius + b.radius;
                    var collisionDistanceSqr = collisionDistance * collisionDistance;

                    if ((b.position - a.position).sqrMagnitude <= collisionDistanceSqr) {
                        var distance = Vector2.Distance(a.position, b.position);
                        var normal = distance > 0f ? (b.position - a.position) / distance : Vector2.right;
                        var depth = Mathf.Abs(distance - collisionDistance) * 0.5f;

                        MotionSolver.collisionsBuffer.Add(new CollisionManifold { a = i, b = j, depth = depth, normal = normal });
                    }
                }
            }

            for (int i = 0; i < MotionSolver.collisionsBuffer.Count; i++) {
                var c = MotionSolver.collisionsBuffer[i];
                ref var a = ref bodies.arr[c.a];
                ref var b = ref bodies.arr[c.b];

                var displace = c.depth * c.normal;

                if (a.isStatic == true) {
                    b.position += displace * 2f;
                } else if (b.isStatic == true) {
                    a.position -= displace * 2f;
                } else {
                    a.position -= displace;
                    b.position += displace;
                }

                if (Vector2.Dot(b.velocity - a.velocity, c.normal) > 0) {
                    continue;
                }

                var tangent = new Vector2(-c.normal.y, c.normal.x);

                var dotTanA = Vector2.Dot(a.velocity, tangent);
                var dotTanB = Vector2.Dot(b.velocity, tangent);

                var dotNormalA = Vector2.Dot(a.velocity, c.normal);
                var dotNormalB = Vector2.Dot(b.velocity, c.normal);

                var mA = (dotNormalA * (a.mass - b.mass) + 2.0f * b.mass * dotNormalB) / (a.mass + b.mass);
                var mB = (dotNormalB * (b.mass - a.mass) + 2.0f * a.mass * dotNormalA) / (a.mass + b.mass);

                a.velocity = tangent * dotTanA + c.normal * mA * collisionDumping;
                b.velocity = tangent * dotTanB + c.normal * mB * collisionDumping;
            }

            if (dynamicDumping > 0f) {
                var velocityDumping = Mathf.Pow(dynamicDumping, deltaTime);

                for (int i = 0; i < bodies.Length; i++) {
                    ref var body = ref bodies.arr[i];
                    body.velocity *= velocityDumping;
                }
            }
        }

    }

}
