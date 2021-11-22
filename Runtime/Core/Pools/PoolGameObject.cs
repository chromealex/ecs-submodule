#if VIEWS_MODULE_SUPPORT
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Views {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class PoolGameObject<T> where T : Component, IViewBase, IViewRespawnTime {

        private Dictionary<ulong, HashSet<T>> prefabToInstances = new Dictionary<ulong, HashSet<T>>();

        private static float GetCurrentTime() {

            //return (float)Worlds.currentWorld.GetTimeSinceStart();
            return Time.realtimeSinceStartup;

        }
        
        public void Clear() {

            foreach (var instance in this.prefabToInstances) {

                foreach (var view in instance.Value) {

                    if (view != null) UnityObjectUtils.Destroy(view.gameObject);

                }
                
            }
            this.prefabToInstances.Clear();
            
        }

        private ulong GetKey(uint key1, uint key2) {

            return MathUtils.GetKey(key1, key2);

        }
        
        public T Spawn(T source, ViewId sourceId, uint customViewId, in Entity targetEntity) {

            T instance = default;
            var found = false;
            var key = this.GetKey(sourceId, customViewId);
            HashSet<T> list;
            if (this.prefabToInstances.TryGetValue(key, out list) == true) {

                if (list.Count > 0) {

                    var foundRespawned = false;
                    if (source is IViewRespawnTime sourceRespawn && sourceRespawn.useCache == true) {

                        foreach (var item in list) {
                            
                            if (item is IViewRespawnTime itemRespawn && item.entity.id == targetEntity.id) {

                                instance = item;
                                list.Remove(instance);
                                found = true;
                                foundRespawned = true;
                                break;
                                
                            }
                            
                        }

                        if (found == false) {

                            foreach (var item in list) {

                                if (item is IViewRespawnTime itemRespawn && itemRespawn.respawnTime <= PoolGameObject<T>.GetCurrentTime()) {

                                    instance = item;
                                    list.Remove(instance);
                                    found = true;
                                    foundRespawned = true;
                                    break;

                                }

                            }

                        }

                    }
                    
                    if (foundRespawned == false) {

                        foreach (var item in list) {

                            instance = item;
                            list.Remove(instance);
                            found = true;
                            break;

                        }

                    }

                }

            } else {
                
                list = new HashSet<T>();
                this.prefabToInstances.Add(key, list);
                
            }

            if (found == false) {

                var go = GameObject.Instantiate(source);
                instance = go.GetComponent<T>();

            }

            if (instance is IViewBaseInternal @internal) {

                var instanceInternal = @internal;
                instanceInternal.Setup(instance.world, new ViewInfo(instance.entity, sourceId, instance.creationTick));

            }

            instance.gameObject.SetActive(true);
            return instance;

        }

        public void Recycle(ref T instance, uint customViewId, float timeout) {
            
            var key = this.GetKey(instance.prefabSourceId, customViewId);
            HashSet<T> list;
            if (this.prefabToInstances.TryGetValue(key, out list) == true) {

                if (instance != null) {

                    if (instance.gameObject != null) instance.gameObject.SetActive(false);
                    if (instance is IViewRespawnTime respawnTimeInstance) {
                    
                        respawnTimeInstance.respawnTime = PoolGameObject<T>.GetCurrentTime() + timeout;

                    }
                    list.Add(instance);

                }

            } else {

                UnityObjectUtils.Destroy(instance.gameObject);

            }

            instance = null;

        }
        
    }

}
#endif