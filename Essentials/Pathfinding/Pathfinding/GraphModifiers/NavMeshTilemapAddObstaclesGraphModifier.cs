using UnityEngine;

namespace ME.ECS.Pathfinding {

    public class NavMeshTilemapAddObstaclesGraphModifier : GraphModifierBase {

        [System.Serializable]
        public struct Item {

            public UnityEngine.Tilemaps.TileBase requiredTile;
            
            [NavMeshArea]
            public int tag;
            public float height;
            public bool customSize;
            public Vector2 size;

        }

        public UnityEngine.Tilemaps.Tilemap tilemap;
        public Item[] items;
        public BoundsInt bounds;
        
        public override void ApplyBeforeConnections(Graph graph) {

            var navMeshGraph = graph as NavMeshGraph;
            var graphBounds = new Bounds((Vector3)navMeshGraph.graphCenter, navMeshGraph.size);
            var graphHeightMin = navMeshGraph.minHeight;
            var graphHeightMax = navMeshGraph.maxHeight;
            var cache = PoolDictionary<int, Item>.Spawn(100);
            var obstacles = PoolDictionary<int, System.Collections.Generic.List<Bounds>>.Spawn(3);
            
            foreach (var item in this.items) {

                cache.Add(item.requiredTile == null ? 0 : item.requiredTile.GetInstanceID(), item);

            }
            

            foreach (var pos in this.bounds.allPositionsWithin) {

                var worldPosition = this.tilemap.GetCellCenterWorld(pos) - this.tilemap.layoutGrid.cellGap;
                
                if (graphBounds.Contains(worldPosition) == false) continue;

                var tile = this.tilemap.GetTile(pos);
                var id = tile == null ? 0 : tile.GetInstanceID();

                if (cache.TryGetValue(id, out var item) == true) {

                    if (obstacles.TryGetValue(item.tag, out var list) == false) {
                        list = new System.Collections.Generic.List<Bounds>();
                        obstacles[item.tag] = PoolList<Bounds>.Spawn(100);
                    }
                
                    list.Add(new Bounds(worldPosition,
                                        new Vector3(item.customSize == true ? item.size.x : this.tilemap.layoutGrid.cellSize.x,
                                                    item.customSize == true ? item.size.y : this.tilemap.layoutGrid.cellSize.y, item.height)));
                    

                    if (item.height < graphHeightMin) graphHeightMin = item.height;
                    if (item.height > graphHeightMax) graphHeightMax = item.height;
                    
                }

            }

            foreach (var kv in obstacles) {
                var count = kv.Value.Count;
                BoundsCompressor.CompressBounds(kv.Value, 0.05f);

                UnityEngine.Debug.Log($"Obstacles bounds compress from {count} to {kv.Value.Count}");
                
                foreach (var obstacle in kv.Value) {
                    var source = new UnityEngine.AI.NavMeshBuildSource {
                        area = kv.Key,
                        shape = UnityEngine.AI.NavMeshBuildSourceShape.Box,
                        transform = Matrix4x4.TRS(obstacle.center,
                                                  this.tilemap.transform.rotation,
                                                  this.tilemap.transform.lossyScale) * this.tilemap.orientationMatrix * this.tilemap.GetTransformMatrix(this.tilemap.WorldToCell(obstacle.center)),
                        size = obstacle.size,
                    };
                    
                    navMeshGraph.AddBuildSource(source);
                }
                
                PoolList<Bounds>.Recycle(kv.Value);
            }

            navMeshGraph.SetMinMaxHeight(graphHeightMin, graphHeightMax);
            
            PoolDictionary<int, Item>.Recycle(ref cache);
            PoolDictionary<int, System.Collections.Generic.List<Bounds>>.Recycle(ref obstacles);

        }

        public override void ApplyAfterConnections(Graph graph) {
            
        }

        public bool GetHeight(NavMeshGraph graph, Vector3 worldPosition, out sfloat height) {

            height = 0f;

            var tilePosition = this.tilemap.WorldToCell(worldPosition);
            var tile = this.tilemap.GetTile(tilePosition);

            if (tile != null) {

                for (int i = 0; i < this.items.Length; ++i) {

                    var item = this.items[i];
                    if (item.requiredTile == null || item.requiredTile == tile) {

                        height = item.height;
                        return true;
                        
                    }

                }

            }

            return false;
            
        }

        public void GetMinMaxHeight(out sfloat min, out sfloat max) {

            min = sfloat.MaxValue;
            max = sfloat.MinValue;

            for (int i = 0; i < this.items.Length; ++i) {

                var item = this.items[i];

                if (item.height < min) min = item.height;
                if (item.height > max) max = item.height;

            }
            
        }

        public override void OnDrawGizmos() {

            var bounds = new Bounds(this.bounds.center, this.bounds.size);
            
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

        }

    }

}
