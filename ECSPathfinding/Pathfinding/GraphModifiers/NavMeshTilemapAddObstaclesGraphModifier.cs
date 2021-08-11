using UnityEngine;

namespace ME.ECS.Pathfinding {

    public class NavMeshTilemapAddObstaclesGraphModifier : GraphModifierBase {

        [System.Serializable]
        public struct Item {

            public UnityEngine.Tilemaps.TileBase requiredTile;
            public bool checkSprite;
            public Sprite[] spriteOneOf;
            
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
            foreach (var pos in this.bounds.allPositionsWithin) {

                UnityEngine.Vector3Int localPlace = new UnityEngine.Vector3Int(pos.x, pos.y, pos.z);
                var tile = this.tilemap.GetTile(localPlace);

                UnityEngine.AI.NavMeshBuildSource source = default;
                bool sourceFound = false;
                for (int i = 0; i < this.items.Length; ++i) {

                    var item = this.items[i];
                    if ((item.checkSprite == false && (item.requiredTile == null || item.requiredTile == tile)) ||
                        (item.checkSprite == true && System.Array.IndexOf(item.spriteOneOf, this.tilemap.GetSprite(localPlace)) >= 0)) {

                        sourceFound = true;
                        source = new UnityEngine.AI.NavMeshBuildSource {
                            area = item.tag,
                            shape = UnityEngine.AI.NavMeshBuildSourceShape.Box,
                            transform = Matrix4x4.TRS(this.tilemap.GetCellCenterWorld(pos) - this.tilemap.layoutGrid.cellGap,
                                                      this.tilemap.transform.rotation,
                                                      this.tilemap.transform.lossyScale) * this.tilemap.orientationMatrix * this.tilemap.GetTransformMatrix(pos),
                            size = new Vector3(item.customSize == true ? item.size.x : this.tilemap.layoutGrid.cellSize.x, item.customSize == true ? item.size.y : this.tilemap.layoutGrid.cellSize.y, item.height),
                        };

                        if (item.height < navMeshGraph.minHeight) navMeshGraph.SetMinMaxHeight(item.height, navMeshGraph.maxHeight);
                        if (item.height > navMeshGraph.maxHeight) navMeshGraph.SetMinMaxHeight(navMeshGraph.minHeight, item.height);

                    }

                }
                if (sourceFound == true) navMeshGraph.AddBuildSource(source);
                
            }

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
                    if ((item.checkSprite == false && (item.requiredTile == null || item.requiredTile == tile)) ||
                        (item.checkSprite == true && System.Array.IndexOf(item.spriteOneOf, this.tilemap.GetSprite(tilePosition)) >= 0)) {

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
