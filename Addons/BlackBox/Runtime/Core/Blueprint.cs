using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.BlackBox {

    public interface IBlueprintContainerOutput {

        ref Blueprint.Item GetOutputItem(Box box, out Vector2 position);

    }

    [System.Serializable]
    public struct BlueprintInfo {

        public Blueprint link;
        public Vector2 outputPosition;

    }
    
    [CreateAssetMenu(menuName = "ME.ECS/BlackBox/Blueprint")]
    public class Blueprint : ScriptableObject {

        [System.Serializable]
        public struct Item {

            public Vector2 position;
            public Rect rect;
            public Box box;

        }

        [HideInInspector]
        public Item[] boxes;
        [HideInInspector][System.NonSerialized]
        public static Item defaultItem;

        [HideInInspector]
        public Item outputItem;
        
        [BoxLink]
        public Box root;

        public void Add(Box box, Vector2 position) {
            
            for (int i = 0; i < this.boxes.Length; ++i) {

                if (this.boxes[i].box == box) {

                    return;

                }
                
            }
            
            System.Array.Resize(ref this.boxes, this.boxes.Length + 1);
            this.boxes[this.boxes.Length - 1] = new Item() {
                box = box,
                position = position,
            };

        }

        public void Remove(Box box) {
            
            for (int i = 0; i < this.boxes.Length; ++i) {

                if (this.boxes[i].box == box) {

                    if (i < this.boxes.Length - 1) System.Array.Copy(this.boxes, i + 1, this.boxes, i, this.boxes.Length - i - 1);
                    System.Array.Resize(ref this.boxes, this.boxes.Length - 1);
                    return;

                }
                
            }
            
        }
        
        public void UpdatePosition(Box box, float x, float y) {

            if (this.outputItem.box == box) {
                
                this.outputItem.position = new Vector2(x, y);
                return;
                
            }
            
            for (int i = 0; i < this.boxes.Length; ++i) {

                if (this.boxes[i].box == box) {

                    ref var item = ref this.boxes[i];
                    item.position = new Vector2(x, y);
                    break;

                }
                
            }
            
        }

        private Item tempInnerItem;
        public ref Item GetItem(Box box) {

            if (this.outputItem.box == box) {

                return ref this.outputItem;

            }
            
            for (int i = 0; i < this.boxes.Length; ++i) {

                if (this.boxes[i].box == box) {

                    return ref this.boxes[i];

                }

                if (this.boxes[i].box is IBlueprintContainerOutput blueprintContainer) {
                    
                    var inner = blueprintContainer.GetOutputItem(box, out var pos);
                    if (inner.box == box) {
                        
                        this.tempInnerItem = inner;
                        this.tempInnerItem.position = pos;
                        return ref this.tempInnerItem;
                        
                    }

                }
                
            }

            return ref Blueprint.defaultItem;

        }

        public void OnCreate() {

            for (int i = 0; i < this.boxes.Length; ++i) {
                
                this.boxes[i].box.OnCreate();
                
            }
            
        }
        
        public void Execute(in Entity entity, float deltaTime) {

            var box = this.root;
            while (box != null) {

                box = box.Execute(in entity, deltaTime);

            }

        }
        
    }
    
}