#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Transform;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class ECSTransformHierarchy {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void OnEntityDestroy(in Entity entity) {

            if (entity.Has<Container>() == true) {
                
                entity.SetParent(in Entity.Empty);
                
            }
            
            if (entity.Has<Nodes>() == true) {

                ref var nodes = ref entity.Get<Nodes>();
                if (nodes.items.IsCreated() == true) {
                    var list = nodes.items.Read();
                    for (int i = 0, cnt = list.Count; i < cnt; ++i) {

                        var child = list[i];
                        if (child.IsAlive() == false) continue;
                        child.Remove<Container>();
                        // TODO: Possible stack overflow while using Destroy because of OnEntityDestroy call
                        child.Destroy();

                    }

                    list.Clear();
                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void OnEntityVersionChanged(in Entity entity) {

            if (entity.Has<Nodes>() == true) {

                var world = Worlds.currentWorld;
                ref readonly var nodes = ref entity.Read<Nodes>();
                if (nodes.items.IsCreated() == true) {
                    var list = nodes.items.Read();
                    for (int i = 0, cnt = list.Count; i < cnt; ++i) {

                        var item = list[i];
                        world.IncrementEntityVersion(in item);
                        // TODO: Possible stack overflow while using OnEntityVersionChanged call
                        world.OnEntityVersionChanged(in item);

                    }
                }

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static uint GetVersionInHierarchy(this in Entity entity) {

            var v = entity.GetVersion();
            var ent = entity;
            while (ent.Has<Container>() == true) {

                ent = ent.Read<Container>().entity;
                v += ent.GetVersion();
                
            }
            return v;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetParent(this in Entity child, in Entity root) {

            child.SetParent(in root, worldPositionStays: true);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetParent(this in Entity child, in Entity root, bool worldPositionStays) {

            if (worldPositionStays == true) {

                var pos = child.GetPosition();
                var rot = child.GetRotation();
                ECSTransformHierarchy.SetParent_INTERNAL(in child, in root);
                child.SetPosition(pos);
                child.SetRotation(rot);

            } else {

                ECSTransformHierarchy.SetParent_INTERNAL(in child, in root);

            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void SetParent_INTERNAL(in Entity child, in Entity root) {

            if (child == root) return;

            ref var container = ref child.Get<Container>();
            if (root == Entity.Empty) {

                ref var nodes = ref container.entity.Get<Nodes>();
                child.Remove<Container>();
                if (nodes.items.IsCreated() == true) nodes.items.Get().Remove(child);
                return;

            }

            if (container.entity == root || root.IsAlive() == false) {

                return;

            }
            
            if (root.Has<Rotation>() == false) {
                root.SetLocalRotation(UnityEngine.Quaternion.identity);
            }

            if (ECSTransformHierarchy.FindInHierarchy(in child, in root) == true) return;

            if (container.entity.IsAlive() == true) {

                child.SetParent(Entity.Empty);

            }

            container.entity = root;
            ref var rootNodes = ref root.Get<Nodes>();
            if (rootNodes.items.IsCreated() == false) {
                var list = PoolListCopyable<Entity>.Spawn(4);
                list.Add(child);
                rootNodes.items = new ME.ECS.Collections.DataList<Entity>(list);
            } else {
                rootNodes.items.Get().Add(child);
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity GetRoot(this in Entity child) {

            Entity root;
            var container = child;
            do {

                root = container;
                container = container.Read<Container>().entity;

            } while (container.IsAlive() == true);

            return root;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static bool FindInHierarchy(in Entity child, in Entity root) {

            var childNodes = child.Read<Nodes>();
            if (childNodes.items.IsCreated() == false) return false;
            var list = childNodes.items.Read();
            if (list.Contains(root) == true) {

                return true;

            }

            for (int i = 0, cnt = list.Count; i < cnt; ++i) {

                var cc = list[i];

                if (ECSTransformHierarchy.FindInHierarchy(in cc, in root) == true) return true;

            }

            return false;

        }

    }

}
