using UnityEngine;

namespace ME.ECS.Pathfinding {
    
    public static class BoundsCompressor  {
    
        private static Vector3[][] MinMaxCache = { new [] { Vector3.zero,  Vector3.zero },  new [] { Vector3.zero, Vector3.zero } };

        public static void CompressBounds(System.Collections.Generic.List<Bounds> bounds, float error = 0.0001f) {
            if (bounds != null && bounds.Count > 1) {
                var foundNeighbor = false;
                var index = 0;

                do {
                    for (var i = index; i < bounds.Count; i++) {
                        foundNeighbor = false;
                        var a = bounds[i];
                        
                        for (var j = i + 1; j < bounds.Count; j++) {
                            var b = bounds[j];
                            if (BoundsCompressor.HasSharedSide(a, b, error) == true) {
                                a.Encapsulate(b);
                                bounds[j] = a;
                                bounds[i] = bounds[index];

                                foundNeighbor = true;
                                ++index;
                                break;
                            }
                        }
                        
                        if (foundNeighbor == true) break;
                    }
                } while (foundNeighbor == true);
                
                
                if (index > 0) bounds.RemoveRange(0, index);
            }
        }

        private static bool HasSharedSide(Bounds a, Bounds b, float error) {

            MinMaxCache[0][0] = a.min;
            MinMaxCache[0][1] = a.max;
            MinMaxCache[1][0] = b.min;
            MinMaxCache[1][1] = b.max;

            for (var axis = 0; axis < 3; axis++) {
                var axis2 = (axis + 1) % 3;
                var axis3 = (axis + 2) % 3;
                
                for (var e = 0; e < 2; e++) {
                    var hasSharedSide = true;
                    var e_inv = (e + 1) % 2;
                    
                    for (var p = 0; p < 4; p++) {
                        var e2 = p & 1;
                        var e3 = (p >> 1) & 1;

                        if (Mathf.Abs(MinMaxCache[0][e][axis] - MinMaxCache[1][e_inv][axis]) > error ||
                            Mathf.Abs(MinMaxCache[0][e2][axis2] - MinMaxCache[1][e2][axis2]) > error ||
                            Mathf.Abs(MinMaxCache[0][e3][axis3] - MinMaxCache[1][e3][axis3]) > error)
                        {
                            hasSharedSide = false;
                            break;
                        }
     
                    }

                    if (hasSharedSide == true) return true;
                }
                
            }

            return false;
        }
    }

}

