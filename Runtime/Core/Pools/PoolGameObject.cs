#if VIEWS_MODULE_SUPPORT
using System.Collections.Generic;
using UnityEngine;

namespace ME.ECS.Views {

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public class PoolGameObject<T> where T : Component, IView, IViewRespawnTime, IViewDestroyTime {

        public struct DespawnInfo {

            public T instance;
            public float time;
            public float respawnTimeout;
            public ulong key;

        }
        
        private Dictionary<ulong, HashSet<T>> prefabToInstances = new Dictionary<ulong, HashSet<T>>();
        private ME.ECS.Collections.ListCopyable<DespawnInfo> despawnInstances = new ME.ECS.Collections.ListCopyable<DespawnInfo>();

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
            if (this.prefabToInstances.TryGetValue(key, out var list) == true) {

                if (list.Count > 0) {

                    var foundRespawned = false;
                    if (source.useCache == true) {

                        foreach (var item in list) {
                            
                            if (item.entity.id == targetEntity.id) {

                                instance = item;
                                list.Remove(instance);
                                found = true;
                                foundRespawned = true;
                                break;
                                
                            }
                            
                        }

                        if (found == false) {

                            foreach (var item in list) {

                                if (item.respawnTime <= PoolGameObject<T>.GetCurrentTime()) {

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

                @internal.Setup(instance.world, new ViewInfo(instance.entity, sourceId, instance.creationTick));

            }

            instance.gameObject.SetActive(true);
            return instance;

        }

        public bool Recycle(ref T instance, uint customViewId, float respawnTimeout) {

            var immediately = true;
            var key = this.GetKey(instance.prefabSourceId, customViewId);
            if (this.prefabToInstances.TryGetValue(key, out var list) == true) {

                if (instance != null) {

                    if (instance.despawnDelay > 0f) {

                        this.AddInstanceForDespawn(key, instance, respawnTimeout);
                        immediately = false;

                    } else {

                        this.Recycle_INTERNAL(list, instance, respawnTimeout);
                        
                    }

                }

            } else {

                UnityObjectUtils.Destroy(instance.gameObject);

            }

            instance = null;

            return immediately;

        }

        private void Recycle_INTERNAL(HashSet<T> list, T instance, float respawnTimeout) {
            
            if (instance.gameObject != null) instance.gameObject.SetActive(false);
            instance.respawnTime = PoolGameObject<T>.GetCurrentTime() + respawnTimeout;
            list.Add(instance);
            
        }

        private void AddInstanceForDespawn(ulong key, T instance, float respawnTimeout) {
        
            this.despawnInstances.Add(new DespawnInfo() {
                instance = instance,
                respawnTimeout = respawnTimeout,
                time = instance.despawnDelay,
                key = key,
            });
            
        }

        public void Update(ViewsModule module, float dt) {

            for (int i = this.despawnInstances.Count - 1; i >= 0; --i) {

                ref var data = ref this.despawnInstances[i];
                data.time -= dt;
                if (data.time <= 0f) {

                    if (this.prefabToInstances.TryGetValue(data.key, out var list) == true) {

                        module.DeInitialize(data.instance);
                        this.Recycle_INTERNAL(list, data.instance, data.respawnTimeout);

                    } else {

                        UnityObjectUtils.Destroy(data.instance.gameObject);

                    }
                    
                    this.despawnInstances.RemoveAt(i);
                    
                }

            }

        }
        
    }

}
#endif