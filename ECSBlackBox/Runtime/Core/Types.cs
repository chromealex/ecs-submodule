namespace ME.ECS.BlackBox {

    public enum RefType {
        Undefined = -2,
        Generic = -1, // 0xFFFFFFFF
        /// <summary>
        ///   <para>Integer property.</para>
        /// </summary>
        Integer = 0,
        /// <summary>
        ///   <para>Boolean property.</para>
        /// </summary>
        Boolean = 1,
        /// <summary>
        ///   <para>Float property.</para>
        /// </summary>
        Float = 2,
        /// <summary>
        ///   <para>String property.</para>
        /// </summary>
        String = 3,
        /// <summary>
        ///   <para>Color property.</para>
        /// </summary>
        Color = 4,
        /// <summary>
        ///   <para>Reference to another object.</para>
        /// </summary>
        ObjectReference = 5,
        /// <summary>
        ///   <para>LayerMask property.</para>
        /// </summary>
        LayerMask = 6,
        /// <summary>
        ///   <para>Enumeration property.</para>
        /// </summary>
        Enum = 7,
        /// <summary>
        ///   <para>2D vector property.</para>
        /// </summary>
        Vector2 = 8,
        /// <summary>
        ///   <para>3D vector property.</para>
        /// </summary>
        Vector3 = 9,
        /// <summary>
        ///   <para>4D vector property.</para>
        /// </summary>
        Vector4 = 10, // 0x0000000A
        /// <summary>
        ///   <para>Rectangle property.</para>
        /// </summary>
        Rect = 11, // 0x0000000B
        /// <summary>
        ///   <para>Array size property.</para>
        /// </summary>
        ArraySize = 12, // 0x0000000C
        /// <summary>
        ///   <para>Character property.</para>
        /// </summary>
        Character = 13, // 0x0000000D
        /// <summary>
        ///   <para>AnimationCurve property.</para>
        /// </summary>
        AnimationCurve = 14, // 0x0000000E
        /// <summary>
        ///   <para>Bounds property.</para>
        /// </summary>
        Bounds = 15, // 0x0000000F
        /// <summary>
        ///   <para>Gradient property.</para>
        /// </summary>
        Gradient = 16, // 0x00000010
        /// <summary>
        ///   <para>Quaternion property.</para>
        /// </summary>
        Quaternion = 17, // 0x00000011
        /// <summary>
        ///   <para>A reference to another Object in the Scene. This is done via an ExposedReference type and resolves to a reference to an Object that exists in the context of the SerializedObject containing the SerializedProperty.</para>
        /// </summary>
        ExposedReference = 18, // 0x00000012
        /// <summary>
        ///   <para>Fixed buffer size property.</para>
        /// </summary>
        FixedBufferSize = 19, // 0x00000013
        /// <summary>
        ///   <para>2D integer vector property.</para>
        /// </summary>
        Vector2Int = 20, // 0x00000014
        /// <summary>
        ///   <para>3D integer vector property.</para>
        /// </summary>
        Vector3Int = 21, // 0x00000015
        /// <summary>
        ///   <para>Rectangle with Integer values property.</para>
        /// </summary>
        RectInt = 22, // 0x00000016
        /// <summary>
        ///   <para>Bounds with Integer values property.</para>
        /// </summary>
        BoundsInt = 23, // 0x00000017
        /// <summary>
        ///   <para>Managed reference property.</para>
        /// </summary>
        ManagedReference = 24, // 0x00000018
    }

}