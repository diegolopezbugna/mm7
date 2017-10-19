using UnityEngine;
using UnityEditor;
using System.Collections;
using UMA;

namespace CrazyMinnow.SALSA.UMA
{	
	public static class CM_UmaSetup_Runtime
	{
		/// <summary>
		/// Configures a complete SALSA with RandomEyes enabled UMA character
		/// </summary>	
		[MenuItem("GameObject/Crazy Minnow Studio/UMA 2/SALSA 1-Click UMA Runtime Setup")]	
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

			CM_UmaBasic umaBasic = umaCharacter.AddComponent<CM_UmaBasic>();
			umaBasic.generator = umaConfig.GetComponentInChildren<UMAGenerator>();
			umaBasic.slotLibrary = umaConfig.GetComponentInChildren<SlotLibrary>();
			umaBasic.overlayLibrary = umaConfig.GetComponentInChildren<OverlayLibrary>();
			umaBasic.raceLibrary = umaConfig.GetComponentInChildren<RaceLibrary>();
			umaBasic.animController = 
				AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
					"Assets/UMA/Example/Animators/Locomotion.controller") as RuntimeAnimatorController;

			CM_UmaSync umaSync = umaCharacter.AddComponent<CM_UmaSync>();
			umaSync.mode = CM_UmaSync.Mode.Runtime;
			umaSync.salsaClip =
				AssetDatabase.LoadAssetAtPath<AudioClip>(
					"Assets/Crazy Minnow Studio/Examples/Audio/DemoScenes/MilitaryMan/mil.moves.wav") as AudioClip;

			umaCharacter.AddComponent<CM_UmaExpressions>();
		}
	}
}