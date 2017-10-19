using UnityEngine;
using UnityEditor;
using System.Collections;
using UMA;

namespace CrazyMinnow.SALSA.UMA
{	
	public static class CM_UmaSetup_DesignTime
	{
		/// <summary>
		/// Configures a complete SALSA with RandomEyes enabled UMA character
		/// </summary>	
		[MenuItem("GameObject/Crazy Minnow Studio/UMA 2/SALSA UMA Design-Time Setup")]	
		static void Setup()
		{
			GameObject umaConfig = GameObject.Find("UMA_Config");
			if (!umaConfig)
			{
				umaConfig = PrefabUtility.InstantiatePrefab(
					AssetDatabase.LoadAssetAtPath<GameObject>(
					"Assets/Crazy Minnow Studio/SALSA with RandomEyes/Third Party Support/UMA 2/Prefabs/UMA_Config.prefab")) as GameObject;
				umaConfig.name = "UMA_Config";
			}

			GameObject umaCharacter = new GameObject("SALSA_UMA2");

			UMADynamicAvatar umaDynamicAvatar = umaCharacter.AddComponent<UMADynamicAvatar>();
			umaDynamicAvatar.umaRecipe = AssetDatabase.LoadAssetAtPath<UMATextRecipe>(
				"Assets/Crazy Minnow Studio/SALSA with RandomEyes/Third Party Support/UMA 2/Prefabs/UMA.asset") as UMATextRecipe;
			umaDynamicAvatar.animationController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
				"Assets/UMA/Example/Animators/Locomotion.controller") as RuntimeAnimatorController;
			umaDynamicAvatar.loadOnStart = true;

			CM_UmaSync umaSync = umaCharacter.AddComponent<CM_UmaSync>();
			umaSync.mode = CM_UmaSync.Mode.DesignTime;
			umaSync.salsaClip =
				AssetDatabase.LoadAssetAtPath<AudioClip>(
					"Assets/Crazy Minnow Studio/Examples/Audio/DemoScenes/MilitaryMan/mil.moves.wav") as AudioClip;

			umaCharacter.AddComponent<CM_UmaExpressions>();
        }
	}
}