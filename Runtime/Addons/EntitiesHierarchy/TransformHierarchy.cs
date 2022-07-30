#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

namespace ME.ECS {

    using Transform;
    using Collections.V3;
    using Collections.MemoryAllocator;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static class ECSTransformHierarchy {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void OnEntityDestroy(ref MemoryAllocator allocator, in Entity entity) {

            if (entity.Has<Container>() == true) {
                
                entity.SetParent(in Entity.Empty);
                
            }
            
            if (entity.Has<Nodes>() == true) {

                // TODO: Possible stack overflow while using Clear(true) because of OnEntityDestroy call
                ref var nodes = ref entity.Get<Nodes>();
                var e = nodes.items.GetEnumerator(in allocator);
                while (e.MoveNext() == true) {
                    e.Current.Remove<Container>();
                    e.Current.Destroy();
                }
                e.Dispose();
                nodes.items.Dispose(ref allocator);

            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void OnEntityVersionChanged(ref MemoryAllocator allocator, in Entity entity) {

            if (entity.TryRead<Nodes>(out var nodes) == true) {

                var world = Worlds.current;
                var e = nodes.items.GetEnumerator(in allocator);
                while (e.MoveNext() == true) {
                    var item = e.Current;
                    world.IncrementEntityVersion(in item);
                    // TODO: Possible stack overflow while using OnEntityVersionChanged call
                    world.OnEntityVersionChanged(in item);
                }
                e.Dispose();
                nodes.items.Dispose(ref allocator);
                
            }

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static uint GetVersionInHierarchy(this in Entity entity) {

            var v = entity.GetVersion();
            var ent = entity;
            while (ent.TryRead<Container>(out var container) == true) {

                ent = container.entity;
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
        public static void SetParent2D(this in Entity child, in Entity root) {

            child.SetParent2D(in root, worldPositionStays: true);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static void SetParent2D(this in Entity child, in Entity root, bool worldPositionStays) {

            if (worldPositionStays == true) {

                var pos = child.GetPosition2D();
                var rot = child.GetRotation2D();
                ECSTransformHierarchy.SetParent_INTERNAL(in child, in root);
                child.SetPosition2D(pos);
                child.SetRotation2D(rot);

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
                nodes.items.Remove(ref Worlds.current.currentState.allocator, child);
                return;

            }

            ref var container = ref child.Get<Container>();
            if (container.entity == root || root.IsAlive() == false) {

                return;

            }
            
            if (ECSTransformHierarchy.FindInHierarchy(in child, in root) == true) return;

            if (container.entity.IsAlive() == true) {

                child.SetParent(Entity.Empty);

            }

            container.entity = root;
            ref var rootNodes = ref root.Get<Nodes>();
            if (rootNodes.items.isCreated == false) rootNodes.items = new List<Entity>(ref Worlds.current.currentState.allocator, 1);
            rootNodes.items.Add(ref Worlds.current.currentState.allocator, child);

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
            if (childNodes.items.Contains(in Worlds.current.currentState.allocator, root) == true) {

                return true;

            }

            var e = childNodes.items.GetEnumerator(in Worlds.current.currentState.allocator);
            while (e.MoveNext() == true) {
                if (ECSTransformHierarchy.FindInHierarchy(e.Current, in root) == true) return true;
            }
            e.Dispose();

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
