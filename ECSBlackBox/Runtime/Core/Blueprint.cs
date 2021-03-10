using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.BlackBox {
    
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
        private Item defaultItem;
        
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
            
            for (int i = 0; i < this.boxes.Length; ++i) {

                if (this.boxes[i].box == box) {

                    ref var item = ref this.boxes[i];
                    item.position = new Vector2(x, y);
                    break;

                }
                
            }
            
        }

        public ref Item GetItem(Box box) {

            for (int i = 0; i < this.boxes.Length; ++i) {

                if (this.boxes[i].box == box) {

                    return ref this.boxes[i];

                }
                
            }

            return ref this.defaultItem;

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