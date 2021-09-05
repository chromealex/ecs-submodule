#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Transform;

    public static class ECSTransformHierarchy {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void OnEntityDestroy(in Entity entity) {

            if (entity.Has<Container>() == true) {
                
                entity.SetParent(in Entity.Empty);
                
            }
            
            if (entity.Has<Childs>() == true) {

                // TODO: Possible stack overflow while using Clear(true) because of OnEntityDestroy call
                ref var childs = ref entity.Get<Childs>();
                foreach (var child in childs.childs) {
                    child.Remove<Container>();
                }
                childs.childs.Clear(destroyData: true);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void OnEntityVersionChanged(in Entity entity) {

            if (entity.Has<Childs>() == true) {

                var world = Worlds.currentWorld;
                ref readonly var childs = ref entity.Read<Childs>();
                foreach (var item in childs.childs) {

                    world.IncrementEntityVersion(in item);
                    // TODO: Possible stack overflow while using OnEntityVersionChanged call
                    world.OnEntityVersionChanged(in item);

                }
                
            }

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

            ref var container = ref child.Get<Container>();
            if (root == Entity.Empty) {

                ref var childs = ref container.entity.Get<Childs>();
                child.Remove<Container>();
                childs.childs.Remove(child);
                return;

            }

            if (container.entity == root || root.IsAlive() == false) {

                return;

            }

            if (ECSTransformHierarchy.FindInHierarchy(in child, in root) == true) return;

            if (container.entity.IsAlive() == true) {

                child.SetParent(Entity.Empty);

            }

            container.entity = root;
            ref var rootChilds = ref root.Get<Childs>();
            rootChilds.childs.Add(child);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Entity GetRoot(this in Entity child) {

            var root = child;
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

            var childChilds = child.Read<Childs>();
            if (childChilds.childs.Contains(root) == true) {

                return true;

            }

            foreach (var cc in childChilds.childs) {

                if (ECSTransformHierarchy.FindInHierarchy(in cc, in root) == true) return true;

            }

            return false;

        }

    }

}