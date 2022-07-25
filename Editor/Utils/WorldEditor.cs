using ME.ECS;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ME.ECSEditor {
    
    public class WorldEditor {

        public static WorldEditor current;
        
        public World world;
        public bool foldout;
        public bool foldoutSystems;
        public bool foldoutModules;
        public bool foldoutEntitiesStorage;
        public bool foldoutFilters;
        public int stateSize;
        public double stateSizeLastUpdateTime;

        private List<ME.ECS.IStorage> foldoutStorages = new List<ME.ECS.IStorage>();
        private Dictionary<object, int> pageObjects = new Dictionary<object, int>();
        private Dictionary<object, int> onPageCountObjects = new Dictionary<object, int>();
        private Dictionary<object, string> searchObjects = new Dictionary<object, string>();
        private HashSet<int> foldoutCustoms = new HashSet<int>();
        private Dictionary<object, List<int>> foldoutStorageFilters = new Dictionary<object, List<int>>();
        private Dictionary<object, List<int>> foldoutStorageData = new Dictionary<object, List<int>>();
        private Dictionary<object, List<int>> foldoutStorageStructComponents = new Dictionary<object, List<int>>();
        private Dictionary<object, List<int>> foldoutStorageViews = new Dictionary<object, List<int>>();

        public bool IsAlive() {

            return this.world != null && this.world.currentState != null;

        }
        
        public void UpdateStateSize() {

            if (EditorApplication.timeSinceStartup > this.stateSizeLastUpdateTime + 15d) {

                this.stateSizeLastUpdateTime = EditorApplication.timeSinceStartup;
                System.Threading.ThreadPool.QueueUserWorkItem((_) => {

                    try {

                        this.stateSize = UnityObjectUtils.GetObjectSize(this.world.GetState());

                    } catch (System.Exception ex) {
                        
                        Debug.LogException(ex);
                        
                    }

                });
                
            }

        }

        public bool IsFoldOutCustom(object instance) {

            var hc = (instance != null ? instance.GetHashCode() : 0);
            return this.foldoutCustoms.Contains(hc);

        }

        public void SetFoldOutCustom(object instance, bool state) {

            var hc = (instance != null ? instance.GetHashCode() : 0);
            if (state == true) {

                if (this.foldoutCustoms.Contains(hc) == false) this.foldoutCustoms.Add(hc);

            } else {

                this.foldoutCustoms.Remove(hc);

            }

        }

        public bool IsFoldOutFilters(object key, int entityId) {

            List<int> list;
            if (this.foldoutStorageFilters.TryGetValue(key, out list) == true) {

                return list.Contains(entityId);

            }

            return false;

        }

        public void SetFoldOutFilters(object key, int entityId, bool state) {

            List<int> list;
            if (this.foldoutStorageFilters.TryGetValue(key, out list) == true) {

                if (state == true) {

                    if (list.Contains(entityId) == false) list.Add(entityId);

                } else {

                    list.Remove(entityId);

                }

            } else {

                if (state == true) {

                    list = new List<int>();
                    list.Add(entityId);
                    this.foldoutStorageFilters.Add(key, list);

                }

            }

        }

        public bool IsFoldOutStructComponents(object storage, int entityId) {

            List<int> list;
            if (this.foldoutStorageStructComponents.TryGetValue(storage, out list) == true) {

                return list.Contains(entityId);

            }

            return true;

        }

        public void SetFoldOutStructComponents(object storage, int entityId, bool state) {

            List<int> list;
            if (this.foldoutStorageStructComponents.TryGetValue(storage, out list) == true) {

                if (state == true) {

                    if (list.Contains(entityId) == false) list.Add(entityId);

                } else {

                    list.Remove(entityId);

                }

            } else {

                if (state == true) {

                    list = new List<int>();
                    list.Add(entityId);
                    this.foldoutStorageStructComponents.Add(storage, list);

                }

            }

        }

        public bool IsFoldOutData(object storage, int entityId) {

            List<int> list;
            if (this.foldoutStorageData.TryGetValue(storage, out list) == true) {

                return list.Contains(entityId);

            }

            return true;

        }

        public void SetFoldOutData(object storage, int entityId, bool state) {

            List<int> list;
            if (this.foldoutStorageData.TryGetValue(storage, out list) == true) {

                if (state == true) {

                    if (list.Contains(entityId) == false) list.Add(entityId);

                } else {

                    list.Remove(entityId);

                }

            } else {

                if (state == true) {

                    list = new List<int>();
                    list.Add(entityId);
                    this.foldoutStorageData.Add(storage, list);

                }

            }

        }

        public bool IsFoldOutViews(object storage, int entityId) {

            List<int> list;
            if (this.foldoutStorageViews.TryGetValue(storage, out list) == true) {

                return list.Contains(entityId);

            }

            return true;

        }

        public void SetFoldOutViews(object storage, int entityId, bool state) {

            List<int> list;
            if (this.foldoutStorageViews.TryGetValue(storage, out list) == true) {

                if (state == true) {

                    if (list.Contains(entityId) == false) list.Add(entityId);

                } else {

                    list.Remove(entityId);

                }

            } else {

                if (state == true) {

                    list = new List<int>();
                    list.Add(entityId);
                    this.foldoutStorageViews.Add(storage, list);

                }

            }

        }

        public bool IsFoldOut(IStorage storage) {

            return this.foldoutStorages.Contains(storage);

        }

        public void SetFoldOut(IStorage storage, bool state) {

            if (state == true) {

                if (this.foldoutStorages.Contains(storage) == false) this.foldoutStorages.Add(storage);

            } else {

                this.foldoutStorages.Remove(storage);

            }

        }

        public int GetPage(object storage) {

            if (this.pageObjects.ContainsKey(storage) == false) {

                return 0;

            }

            return this.pageObjects[storage];

        }

        public void SetPage(object storage, int page) {

            if (this.pageObjects.ContainsKey(storage) == true) {
            
                this.pageObjects[storage] = page;

            } else {

                this.pageObjects.Add(storage, page);
                
            }
            
        }

        public int GetOnPageCount(object storage) {

            if (this.onPageCountObjects.ContainsKey(storage) == false) {

                return 10;

            }

            return this.onPageCountObjects[storage];

        }

        public void SetOnPageCount(object storage, int page) {

            if (this.onPageCountObjects.ContainsKey(storage) == true) {
            
                this.onPageCountObjects[storage] = page;

            } else {

                this.onPageCountObjects.Add(storage, page);
                
            }
            
        }

        public string GetSearch(object storage) {

            if (this.searchObjects.ContainsKey(storage) == false) {
                
                return string.Empty;

            }

            return this.searchObjects[storage];

        }

        public void SetSearch(object storage, string search) {

            if (this.searchObjects.ContainsKey(storage) == true) {
            
                this.searchObjects[storage] = search;

            } else {

                this.searchObjects.Add(storage, search);
                
            }
            
        }
        
        public ME.ECS.FiltersArchetype.FiltersArchetypeStorage GetFilters() {

            return WorldHelper.GetFilters(this.world);

        }
        
        public ME.ECS.FiltersArchetype.FiltersArchetypeStorage GetEntitiesStorage() {

            return WorldHelper.GetEntitiesStorage(this.world);

        }

        public ME.ECS.Collections.V3.MemoryAllocator GetAllocator() {

            return WorldHelper.GetAllocator(this.world);

        }

        public IStructComponentsContainer GetStructComponentsStorage() {

            return WorldHelper.GetStructComponentsStorage(this.world);

        }
        
        public ME.ECS.Collections.BufferArray<SystemGroup> GetSystems() {

            return WorldHelper.GetSystems(this.world);

        }

        public ME.ECS.Collections.ListCopyable<ME.ECS.IModuleBase> GetModules() {

            return WorldHelper.GetModules(this.world);

        }

        public bool HasMethod(object instance, string methodName) {

            return WorldHelper.HasMethod(instance, methodName);

        }

        public override string ToString() {

            return "World " + this.world.id.ToString() + " (Tick: " + this.world.GetCurrentTick().ToString() + ")";

        }

    }

}