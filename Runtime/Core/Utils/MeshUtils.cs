using UnityEngine;

namespace ME.ECS {

    public static class MeshUtils {

        public static UnityEngine.Mesh GetMeshFromTerrain(UnityEngine.Terrain terrain) {

            var resolution = terrain.terrainData.heightmapResolution;
            var l = resolution;
            var w = resolution;
            var width = terrain.terrainData.heightmapResolution * terrain.terrainData.heightmapScale.x;
            var length = terrain.terrainData.heightmapResolution * terrain.terrainData.heightmapScale.z;
            //Vector3 terrainPos = terrain.transform.position;

            var newVertices = new Vector3[l * w];
            var newTriangles = new int[l * w * 2 * 3];
            var tri = 0;

            var newUV = new Vector2[l * w];
            var uvStepL = 1f / (l - 1);
            var uvStepW = 1f / (w - 1);

            var heights = terrain.terrainData.GetHeights(0, 0, w, l);
            for (var i = 0; i < l; i++) {
                for (var j = 0; j < w; j++) {
                    var height = heights[i, j];//.SampleHeight(new Vector3(terrainPos.x + width / (w - 1) * j, 0f, terrainPos.z + length / (l - 1) * i));
                    newVertices[i * w + j] = new Vector3(width / (w - 1) * j, height, length / (l - 1) * i);
                    newUV[i * w + j] = new Vector2(uvStepL * i, uvStepW * j);
                    if (i > 0 && j > 0) {
                        newTriangles[tri + 0] = i * w + j - 1; // 3
                        newTriangles[tri + 1] = (i - 1) * w + j; // 2
                        newTriangles[tri + 2] = (i - 1) * w + j - 1; // 1

                        tri += 3;
                        newTriangles[tri + 0] = i * w + j; // 4
                        newTriangles[tri + 1] = (i - 1) * w + j; // 2
                        newTriangles[tri + 2] = i * w + j - 1; // 3
                        tri += 3;
                    }
                }
            }

            var mesh = new UnityEngine.Mesh();
            mesh.vertices = newVertices;
            mesh.triangles = newTriangles;
            mesh.uv = newUV;

            return mesh;

        }

    }

}