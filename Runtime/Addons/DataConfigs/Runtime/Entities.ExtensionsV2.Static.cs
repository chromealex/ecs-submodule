#if !STATIC_API_DISABLED
#if ENABLE_IL2CPP
#define INLINE_METHODS
#endif

#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif

namespace ME.ECS {

    using Collections;

    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public static partial class EntityExtensionsV2 {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        /// <summary>
        /// Try read TComponent from any data configs which were applied onto entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component"></param>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns>True if config found</returns>
        public static bool TryReadStatic<TComponent>(this in Entity entity, out TComponent component) where TComponent : struct, IComponentStatic {

            component = default;
            
            // check if this component in SourceConfig
            if (entity.TryRead<ME.ECS.DataConfigs.SourceConfig>(out var sourceConfig) == true) {

                if (sourceConfig.config.GetData().TryRead(out component) == true) return true;

            }
            
            // check if this component in SourceConfigs
            if (entity.TryRead<ME.ECS.DataConfigs.SourceConfigs>(out var sourceConfigs) == true) {

                var e = sourceConfigs.configs.GetEnumerator(Worlds.current.GetState());
                while (e.MoveNext() == true) {

                    var config = e.Current;
                    if (config.GetData().TryRead(out component) == true) return true;
                    
                }
                e.Dispose();
                
            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        /// <summary>
        /// Check if TComponent exists in any data configs which were applied onto entity
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns>True if found</returns>
        public static bool HasStatic<TComponent>(this in Entity entity) where TComponent : struct, IComponentStatic {

            // check if this component in SourceConfig
            if (entity.TryRead<ME.ECS.DataConfigs.SourceConfig>(out var sourceConfig) == true) {

                if (sourceConfig.config.GetData().Has<TComponent>() == true) return true;

            }
            
            // check if this component in SourceConfigs
            if (entity.TryRead<ME.ECS.DataConfigs.SourceConfigs>(out var sourceConfigs) == true) {

                var e = sourceConfigs.configs.GetEnumerator(Worlds.current.GetState());
                while (e.MoveNext() == true) {

                    var config = e.Current;
                    if (config.GetData().Has<TComponent>() == true) return true;
                    
                }
                e.Dispose();
                
            }

            return false;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        /// <summary>
        /// Reads TComponent from any data configs which were applied onto entity
        /// </summary>
        /// <param name="entity"></param>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns>Component or default if nothing found</returns>
        public static TComponent ReadStatic<TComponent>(this in Entity entity) where TComponent : struct, IComponentStatic {

            if (entity.TryReadStatic<TComponent>(out var component) == true) {

                return component;

            }
            
            return default;

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        /// <summary>
        /// Register config which has TComponent from source
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="source">Source entity which has static TComponent</param>
        /// <typeparam name="TComponent"></typeparam>
        public static void SetStatic<TComponent>(this in Entity entity, in Entity source) where TComponent : struct, IComponentStatic {

            if (entity.HasStatic<TComponent>() == true) return;
            
            // check if this component in SourceConfig
            if (source.TryRead<ME.ECS.DataConfigs.SourceConfig>(out var sourceConfig) == true) {

                if (sourceConfig.config.GetData().Has<TComponent>() == true) {

                    ME.ECS.DataConfigs.DataConfig.AddSource(in entity, sourceConfig.config);
                    return;
                    
                }

            }
            
            // check if this component in SourceConfigs
            if (source.TryRead<ME.ECS.DataConfigs.SourceConfigs>(out var sourceConfigs) == true) {

                var e = sourceConfigs.configs.GetEnumerator(Worlds.current.GetState());
                while (e.MoveNext() == true) {

                    var config = e.Current;
                    if (config.GetData().Has<TComponent>() == true) {

                        ME.ECS.DataConfigs.DataConfig.AddSource(in entity, config);
                        return;
                    
                    }

                }
                e.Dispose();
                
            }
            
        }

    }

}
#endif