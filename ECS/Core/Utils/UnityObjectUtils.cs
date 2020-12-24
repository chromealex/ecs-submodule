namespace ME.ECS {

	using UnityEngine;
	
	#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
	#endif
	public static class UnityObjectUtils {

		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static void Destroy(Object obj) {
			
			#if UNITY_EDITOR
            
			if (Application.isEditor) {
                
				Object.DestroyImmediate(obj);
				return;
                
			}
            
			#endif
                        
			Object.Destroy(obj);
			
		}

	}

}