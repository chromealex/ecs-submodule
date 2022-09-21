namespace ME.ECS {

    public static class UnityVectorExt {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2 X(this UnityEngine.Vector2 v, float value = 0f) {

            return new UnityEngine.Vector2(value, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2 Y(this UnityEngine.Vector2 v, float value = 0f) {

            return new UnityEngine.Vector2(v.x, value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 X(this UnityEngine.Vector3 v, float value = 0f) {

            return new UnityEngine.Vector3(value, v.y, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 Y(this UnityEngine.Vector3 v, float value = 0f) {

            return new UnityEngine.Vector3(v.x, value, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 Z(this UnityEngine.Vector3 v, float value = 0f) {

            return new UnityEngine.Vector3(v.x, v.y, value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 XY(this UnityEngine.Vector2 v, float z = 0f) {

            return new UnityEngine.Vector3(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3 XZ(this UnityEngine.Vector2 v, float y = 0f) {

            return new UnityEngine.Vector3(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2 XY(this UnityEngine.Vector3 v) {

            return new UnityEngine.Vector2(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2 YZ(this UnityEngine.Vector3 v) {

            return new UnityEngine.Vector2(v.y, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2 XZ(this UnityEngine.Vector3 v) {

            return new UnityEngine.Vector2(v.x, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3Int XY(this UnityEngine.Vector2Int v, int z = 0) {

            return new UnityEngine.Vector3Int(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector3Int XZ(this UnityEngine.Vector2Int v, int y = 0) {

            return new UnityEngine.Vector3Int(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2Int XY(this UnityEngine.Vector3Int v) {

            return new UnityEngine.Vector2Int(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static UnityEngine.Vector2Int XZ(this UnityEngine.Vector3Int v) {

            return new UnityEngine.Vector2Int(v.x, v.z);

        }

    }

    public static class UnityMathExt {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float2 X(this Unity.Mathematics.float2 v, float value = 0f) {

            return new Unity.Mathematics.float2(value, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float2 Y(this Unity.Mathematics.float2 v, float value = 0f) {

            return new Unity.Mathematics.float2(v.x, value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float3 X(this Unity.Mathematics.float3 v, float value = 0f) {

            return new Unity.Mathematics.float3(value, v.y, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float3 Y(this Unity.Mathematics.float3 v, float value = 0f) {

            return new Unity.Mathematics.float3(v.x, value, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float3 Z(this Unity.Mathematics.float3 v, float value = 0f) {

            return new Unity.Mathematics.float3(v.x, v.y, value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float3 XY(this Unity.Mathematics.float2 v, float z = 0f) {

            return new Unity.Mathematics.float3(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float3 XZ(this Unity.Mathematics.float2 v, float y = 0f) {

            return new Unity.Mathematics.float3(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float2 XY(this Unity.Mathematics.float3 v) {

            return new Unity.Mathematics.float2(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.float2 XZ(this Unity.Mathematics.float3 v) {

            return new Unity.Mathematics.float2(v.x, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.int3 XY(this Unity.Mathematics.int3 v, int z = 0) {

            return new Unity.Mathematics.int3(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.int3 XZ(this Unity.Mathematics.int2 v, int y = 0) {

            return new Unity.Mathematics.int3(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.int2 XY(this Unity.Mathematics.int3 v) {

            return new Unity.Mathematics.int2(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static Unity.Mathematics.int2 XZ(this Unity.Mathematics.int3 v) {

            return new Unity.Mathematics.int2(v.x, v.z);

        }

    }

    #if FIXED_POINT_MATH
    public static class MEECSMathExt {

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float2 X(this ME.ECS.Mathematics.float2 v, float value = 0f) {

            return new ME.ECS.Mathematics.float2(value, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float2 Y(this ME.ECS.Mathematics.float2 v, float value = 0f) {

            return new ME.ECS.Mathematics.float2(v.x, value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float3 X(this ME.ECS.Mathematics.float3 v, float value = 0f) {

            return new ME.ECS.Mathematics.float3(value, v.y, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float3 Y(this ME.ECS.Mathematics.float3 v, float value = 0f) {

            return new ME.ECS.Mathematics.float3(v.x, value, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float3 Z(this ME.ECS.Mathematics.float3 v, float value = 0f) {

            return new ME.ECS.Mathematics.float3(v.x, v.y, value);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float3 XY(this ME.ECS.Mathematics.float2 v, float z = 0f) {

            return new ME.ECS.Mathematics.float3(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float3 XZ(this ME.ECS.Mathematics.float2 v, float y = 0f) {

            return new ME.ECS.Mathematics.float3(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float2 XY(this ME.ECS.Mathematics.float3 v) {

            return new ME.ECS.Mathematics.float2(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.float2 XZ(this ME.ECS.Mathematics.float3 v) {

            return new ME.ECS.Mathematics.float2(v.x, v.z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.int3 XY(this ME.ECS.Mathematics.int3 v, int z = 0) {

            return new ME.ECS.Mathematics.int3(v.x, v.y, z);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.int3 XZ(this ME.ECS.Mathematics.int2 v, int y = 0) {

            return new ME.ECS.Mathematics.int3(v.x, y, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.int2 XY(this ME.ECS.Mathematics.int3 v) {

            return new ME.ECS.Mathematics.int2(v.x, v.y);

        }

        #if INLINE_METHODS
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        #endif
        public static ME.ECS.Mathematics.int2 XZ(this ME.ECS.Mathematics.int3 v) {

            return new ME.ECS.Mathematics.int2(v.x, v.z);

        }

    }
    #endif

}