
namespace ME.ECS.Pathfinding.Features {

    using Collections;

    using UnityEngine;

    public static class PathSmoothingUtils {

        public static ListCopyable<Vector3> Bezier(ListCopyable<Vector3> points, int subdivisions = 2, float tangentLength = 0.5f) {
            subdivisions = Mathf.Max(subdivisions, 0);

            var segmentsCount = 1 << subdivisions;
            var outputPoints = PoolListCopyable<Vector3>.Spawn(points.Count * segmentsCount + 1);

            for (int i = 0; i < points.Count - 1; i++) {
                var t1 = (points[i + 1] - points[i == 0 ? i : i - 1]) * tangentLength;
                var t2 = (points[i] - points[i == points.Count - 2 ? i + 1 : i + 2]) * tangentLength;

                var p0 = points[i];
                var p1 = p0 + t1;
                var p2 = points[i + 1];
                var p3 = p2 + t2;

                for (int j = 0; j < segmentsCount; j++) {
                    outputPoints.Add(CubicBezier(p0, p1, p3, p2, (float)j / segmentsCount));
                }
            }

            outputPoints.Add(points[points.Count - 1]);

            return outputPoints;
        }

        private static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            float t2 = 1 - t;
            return t2 * t2 * t2 * p0 + 3 * t2 * t2 * t * p1 + 3 * t2 * t * t * p2 + t * t * t * p3;
        }
    }

}
