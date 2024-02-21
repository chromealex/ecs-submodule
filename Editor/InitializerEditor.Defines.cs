using System.Linq;
using UnityEngine.UIElements;

namespace ME.ECSEditor {
    
    using UnityEngine;
    using UnityEditor;
    using ME.ECS;

    public partial class InitializerEditor {

        public static InitializerBase.DefineInfo[] GetDefines() {
            if (InitializerEditor.getAdditionalDefines != null) {
                var list = new System.Collections.Generic.List<InitializerBase.DefineInfo>();
                list.AddRange(InitializerEditor.defines);
                foreach (var item in InitializerEditor.getAdditionalDefines.GetInvocationList()) {
                    list.AddRange((InitializerBase.DefineInfo[])item.DynamicInvoke());
                }
                return list.ToArray();
            }

            return InitializerEditor.defines;
        }
        
        private InitializerBase.DefineInfo GetDefineInfo(string define) {

            foreach (var defineInfo in InitializerEditor.GetDefines()) {

                if (defineInfo.define == define) {
                    
                    return defineInfo;
                    
                }
                
            }

            return default;

        }
        
        private static readonly InitializerBase.DefineInfo[] defines = new[] {
            new InitializerBase.DefineInfo(true, "GAMEOBJECT_VIEWS_MODULE_SUPPORT", "Turn on/off GameObject View Provider.", () => {
                #if GAMEOBJECT_VIEWS_MODULE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Light),
            new InitializerBase.DefineInfo(true, "PARTICLES_VIEWS_MODULE_SUPPORT", "Turn on/off Particles View Provider.", () => {
                #if PARTICLES_VIEWS_MODULE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Light),
            new InitializerBase.DefineInfo(true, "DRAWMESH_VIEWS_MODULE_SUPPORT", "Turn on/off Graphics View Provider.", () => {
                #if DRAWMESH_VIEWS_MODULE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Light),
            new InitializerBase.DefineInfo(true, "WORLD_STATE_CHECK", "If turned on, ME.ECS will check that all write data methods are in right state. If you turn off this check, you'll be able to write data in any state, but it could cause out of sync state.", () => {
                #if WORLD_STATE_CHECK
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugOnly, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(true, "WORLD_THREAD_CHECK", "If turned on, ME.ECS will check random number usage from non-world thread. If you don't want to synchronize the game, you could turn this check off.", () => {
                #if WORLD_THREAD_CHECK
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugOnly, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(true, "WORLD_EXCEPTIONS", "If turned on, ME.ECS will throw exceptions on unexpected behaviour. Turn off this check in release builds.", () => {
                #if WORLD_EXCEPTIONS
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugOnly, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(true, "WORLD_TICK_THREADED", "If turned on, ME.ECS will run logic in another thread.", () => {
                #if WORLD_TICK_THREADED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Light),
            new InitializerBase.DefineInfo(true, "FPS_MODULE_SUPPORT", "FPS module support.", () => {
                #if FPS_MODULE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Light),
            new InitializerBase.DefineInfo(true, "ECS_COMPILE_IL2CPP_OPTIONS", "If turned on, ME.ECS will use IL2CPP options for the faster runtime, this flag removed unnecessary null-checks and bounds array checks.", () => {
                #if ECS_COMPILE_IL2CPP_OPTIONS
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Unknown, InitializerBase.RuntimeSpeed.Unknown),
            new InitializerBase.DefineInfo(true, "ECS_COMPILE_IL2CPP_OPTIONS_FILE_INCLUDE", "Turn off this option if you provide your own Il2CppSetOptionAttribute. Works with ECS_COMPILE_IL2CPP_OPTIONS.", () => {
                #if ECS_COMPILE_IL2CPP_OPTIONS_FILE_INCLUDE
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Unknown, InitializerBase.RuntimeSpeed.Unknown),
            new InitializerBase.DefineInfo(true, "MULTITHREAD_SUPPORT", "Turn on this option if you need to add/remove components inside jobs.", () => {
                #if MULTITHREAD_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Unknown, InitializerBase.RuntimeSpeed.Unknown),
            new InitializerBase.DefineInfo(true, "MESSAGE_PACK_SUPPORT", "MessagePack package support.", () => {
                #if MESSAGE_PACK_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Unknown, InitializerBase.RuntimeSpeed.Unknown),
            new InitializerBase.DefineInfo(true, "ENTITY_VERSION_INCREMENT_ACTIONS", "Turn on to raise events on entity version increments.", () => {
                #if ENTITY_VERSION_INCREMENT_ACTIONS
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Heavy),
            new InitializerBase.DefineInfo(false, "BUFFER_SLICED_DISABLED", "Turn on to use Sliced Buffers which allows to add entities in Get<> API.", () => {
                #if BUFFER_SLICED_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Light),
            new InitializerBase.DefineInfo(true, "VIEWS_REGISTER_VIEW_SOURCE_CHECK_STATE", "Forbid RegisterViewSource after world initialization.", () => {
                #if VIEWS_REGISTER_VIEW_SOURCE_CHECK_STATE
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Light),
            new InitializerBase.DefineInfo(true, "ME_ECS_COLLECT_WEAK_REFERENCES", "Collect weak references for ecs modules and provide public api (weak/unweak).", () => {
                #if ME_ECS_COLLECT_WEAK_REFERENCES
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugOnly, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Heavy),
            new InitializerBase.DefineInfo(false, "SHARED_COMPONENTS_DISABLED", "Disable shared components storage and entity shared API. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if SHARED_COMPONENTS_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Heavy, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(false, "ENTITIES_GROUP_DISABLED", "Disable entities group storage and entities group API. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if ENTITIES_GROUP_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(false, "FILTERS_LAMBDA_DISABLED", "Disable lambda in filters. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if FILTERS_LAMBDA_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(false, "STATIC_API_DISABLED", "Disable static API for entities. Use this if you don't use this feature at all to speed up your runtime.", () => {
                #if STATIC_API_DISABLED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Light),
            new InitializerBase.DefineInfo(true, "COMPONENTS_COPYABLE", "Enable custom Copyable components. Use this if you need to custom copy/recycle components.", () => {
                #if COMPONENTS_COPYABLE
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Normal, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(true, "SET_ENTITY_NAME", "Set name component to entities.", () => {
                #if SET_ENTITY_NAME
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Normal, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(true, "NETWORK_SYNC_QUEUE_SUPPORT", "If enabled, NetworkModule sends SYNC_RPC on each state, taken out from history (each 4 ticks)", () => {
                #if NETWORK_SYNC_QUEUE_SUPPORT
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Normal),
            new InitializerBase.DefineInfo(true, "SPARSESET_DENSE_SLICED", "If enabled, entity's components are stored in non movable memory arrays. Is required to use when aspects used", () => {
                #if SPARSESET_DENSE_SLICED
                return true;
                #else
                return false;
                #endif
            }, true, InitializerBase.ConfigurationType.DebugAndRelease, InitializerBase.CodeSize.Light, InitializerBase.RuntimeSpeed.Normal),
        };

    }

}