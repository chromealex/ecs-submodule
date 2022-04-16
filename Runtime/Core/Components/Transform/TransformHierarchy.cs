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

                // TODO: Possible stack overflow while using Clear(true) because of OnEntityDestroy call
                ref var nodes = ref entity.Get<Nodes>();
                foreach (var child in nodes.items) {
                    child.Remove<Container>();
                }
                nodes.items.Clear(destroyData: true);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void OnEntityVersionChanged(in Entity entity) {

            if (entity.Has<Nodes>() == true) {

                var world = Worlds.currentWorld;
                ref readonly var nodes = ref entity.Read<Nodes>();
                foreach (var item in nodes.items) {

                    world.IncrementEntityVersion(in item);
                    // TODO: Possible stack overflow while using OnEntityVersionChanged call
                    world.OnEntityVersionChanged(in item);

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

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        private static void SetParent_INTERNAL(in Entity child, in Entity root) {

            if (child == root) return;

            if (root == Entity.Empty) {

                var childContainer = child.Read<Container>();
                if (childContainer.entity.IsAlive() == false) return;
                
                ref var nodes = ref childContainer.entity.Get<Nodes>();
                child.Remove<Container>();
                nodes.items.Remove(child);
                return;

            }

            ref var container = ref child.Get<Container>();
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
            rootNodes.items.Add(child);

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
            if (childNodes.items.Contains(root) == true) {

                return true;

            }

            foreach (var cc in childNodes.items) {

                if (ECSTransformHierarchy.FindInHierarchy(in cc, in root) == true) return true;

            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool TryGetParent(this in Entity child, out Entity parent) {

            var r = child.TryRead<Container>(out var c);
            parent = c.entity;
            return r;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool HasParent(this in Entity child) {

            return child.Has<Container>();

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity GetParent(this in Entity child) {

            return child.Read<Container>().entity;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static bool TryReadParent(this in Entity child, out Entity parent) {

            return child.TryGetParent(out parent);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity ReadParent(this in Entity child) {

            return child.GetParent();

        }

    }

}
