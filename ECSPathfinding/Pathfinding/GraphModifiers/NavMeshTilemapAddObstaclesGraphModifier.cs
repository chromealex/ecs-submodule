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

            var cache = PoolDictionary<int, Item>.Spawn(100);
            foreach (var item in this.items) {

                cache.Add(item.requiredTile == null ? 0 : item.requiredTile.GetInstanceID(), item);

            }
            
            var navMeshGraph = graph as NavMeshGraph;
            foreach (var pos in this.bounds.allPositionsWithin) {

                UnityEngine.Vector3Int localPlace = new UnityEngine.Vector3Int(pos.x, pos.y, pos.z);
                var tile = this.tilemap.GetTile(localPlace);
                var id = tile == null ? 0 : tile.GetInstanceID();
                if (cache.TryGetValue(id, out var item) == true) {
                    
                    var source = new UnityEngine.AI.NavMeshBuildSource {
                        area = item.tag,
                        shape = UnityEngine.AI.NavMeshBuildSourceShape.Box,
                        transform = Matrix4x4.TRS(this.tilemap.GetCellCenterWorld(pos) - this.tilemap.layoutGrid.cellGap,
                                                  this.tilemap.transform.rotation,
                                                  this.tilemap.transform.lossyScale) * this.tilemap.orientationMatrix * this.tilemap.GetTransformMatrix(pos),
                        size = new Vector3(item.customSize == true ? item.size.x : this.tilemap.layoutGrid.cellSize.x, item.customSize == true ? item.size.y : this.tilemap.layoutGrid.cellSize.y, item.height),
                    };
                    
                    if (item.height < navMeshGraph.minHeight) navMeshGraph.SetMinMaxHeight(item.height, navMeshGraph.maxHeight);
                    if (item.height > navMeshGraph.maxHeight) navMeshGraph.SetMinMaxHeight(navMeshGraph.minHeight, item.height);

                    navMeshGraph.AddBuildSource(source);
                    
                }
                
            }
            
            PoolDictionary<int, Item>.Recycle(ref cache);

        }

        public override void ApplyAfterConnections(Graph graph) {
            
        }

        public bool GetHeight(NavMeshGraph graph, Vector3 worldPosition, out float height) {

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

        public void GetMinMaxHeight(out float min, out float max) {

            min = float.MaxValue;
            max = float.MinValue;

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
