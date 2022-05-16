using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

namespace ME.ECS.Collections {

    /// <summary>
    /// Editor drawing of the NativeQuadTree
    /// </summary>
    public unsafe partial struct NativeQuadTree<T> where T : unmanaged {

        public static void Draw(NativeQuadTree<T> tree, NativeList<QuadElement<T>> results, AABB2D range,
                                Color[][] texture) {
            var widthMult = texture.Length / tree.bounds.extents.x * 2 / 2 / 2;
            var heightMult = texture[0].Length / tree.bounds.extents.y * 2 / 2 / 2;

            var widthAdd = tree.bounds.center.x + tree.bounds.extents.x;
            var heightAdd = tree.bounds.center.y + tree.bounds.extents.y;

            for (var i = 0; i < tree.nodes.Length; i++) {

                var node = tree.nodes[i]; //UnsafeUtility.ReadArrayElement<QuadNode>(tree.nodes->Ptr, i);

                if (node.count > 0) {
                    for (var k = 0; k < node.count; k++) {
                        //var element = UnsafeUtility.ReadArrayElement<QuadElement<T>>(tree.elements->Ptr, node.firstChildIndex + k);
                        var element = tree.elements[node.firstChildIndex + k];

                        texture[(int)((element.pos.x + widthAdd) * widthMult)]
                            [(int)((element.pos.y + heightAdd) * heightMult)] = Color.red;
                    }
                }
            }

            foreach (var element in results) {
                texture[(int)((element.pos.x + widthAdd) * widthMult)]
                    [(int)((element.pos.y + heightAdd) * heightMult)] = Color.green;
            }

            NativeQuadTree<T>.DrawBounds(texture, range, tree);
        }

        private static void DrawBounds(Color[][] texture, AABB2D bounds, NativeQuadTree<T> tree) {
            var widthMult = texture.Length / tree.bounds.extents.x * 2 / 2 / 2;
            var heightMult = texture[0].Length / tree.bounds.extents.y * 2 / 2 / 2;

            var widthAdd = tree.bounds.center.x + tree.bounds.extents.x;
            var heightAdd = tree.bounds.center.y + tree.bounds.extents.y;

            var top = new float2(bounds.center.x, bounds.center.y - bounds.extents.y);
            var left = new float2(bounds.center.x - bounds.extents.x, bounds.center.y);

            for (var leftToRight = 0; leftToRight < bounds.extents.x * 2; leftToRight++) {
                var poxX = left.x + leftToRight;
                texture[(int)((poxX + widthAdd) * widthMult)][(int)((bounds.center.y + heightAdd + bounds.extents.y) * heightMult)] = Color.blue;
                texture[(int)((poxX + widthAdd) * widthMult)][(int)((bounds.center.y + heightAdd - bounds.extents.y) * heightMult)] = Color.blue;
            }

            for (var topToBottom = 0; topToBottom < bounds.extents.y * 2; topToBottom++) {
                var posY = top.y + topToBottom;
                texture[(int)((bounds.center.x + widthAdd + bounds.extents.x) * widthMult)][(int)((posY + heightAdd) * heightMult)] = Color.blue;
                texture[(int)((bounds.center.x + widthAdd - bounds.extents.x) * widthMult)][(int)((posY + heightAdd) * heightMult)] = Color.blue;
            }
        }

    }

}