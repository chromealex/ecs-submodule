using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Debug {

    [CreateAssetMenu(menuName = "ME.ECS/Debug/Statistics Object")]
    public class StatisticsObject : ScriptableObject {

        [System.Serializable]
        public struct Item {

            public string key;
            public EntityStatistic stat;

        }

        public EntityStatistic all;
        public EntityStatistic reset;
        public int startsCount;
        public List<Item> items = new List<Item>();

        public void OnWorldCreated() {
            
            ++this.startsCount;
            
        }

        public int GetRecommendedCapacity(string key = null) {

            if (key == null) {

                return this.GetRecommendedCapacityFor(this.all);
                
            }

            var item = this.FindItem(key);
            return this.GetRecommendedCapacityFor(item.stat);

        }

        private int GetRecommendedCapacityFor(EntityStatistic stat) {

            var max = stat.max;
            return Mathf.CeilToInt(max / 100f) * 100;

        }

        public void OnEntityCreate(string key, Entity entity) {

            if (Worlds.current.HasResetState() == false) {
                
                this.reset.OnCreateEntity(entity);
                this.all.min = this.reset.max;
                return;
                
            }
            
            this.all.OnCreateEntity(entity);

            if (string.IsNullOrEmpty(key) == false) {

                this.SetCustom(key, entity);
                
            }

        }

        private Item FindItem(string key) {
            
            for (int i = 0; i < this.items.Count; ++i) {

                var item = this.items[i];
                if (item.key == key) {
                    
                    return this.items[i];

                }
                
            }
            
            return default;

        }

        private void SetCustom(string key, Entity entity) {

            var found = false;
            for (int i = 0; i < this.items.Count; ++i) {

                var item = this.items[i];
                if (item.key == key) {
                    
                    item.stat.OnCreateEntity(entity);
                    this.items[i] = item;
                    found = true;
                    break;

                }
                
            }

            if (found == false) {

                var stat = new EntityStatistic() { min = this.reset.max, max = this.reset.max };
                stat.OnCreateEntity(entity);
                
                var item = new Item() {
                    key = key,
                    stat = stat,
                };
                this.items.Add(item);

            }
            
        }

    }

}