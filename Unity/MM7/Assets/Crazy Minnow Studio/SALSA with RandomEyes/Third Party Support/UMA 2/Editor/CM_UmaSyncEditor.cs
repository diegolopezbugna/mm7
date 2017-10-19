using UnityEngine;
using UnityEditor;
using System.Collections;
using CrazyMinnow.SALSA;
using UMA;

namespace CrazyMinnow.SALSA.UMA
{
	/// <summary>
	/// This is the custom inspector for CM_UmaSync, a script that acts as a proxy between 
	/// SALSA with RandomEyes and UMA characters, and allows users to link SALSA with 
	/// RandomEyes to UMA characters without any model modifications.
	/// 
	/// Crazy Minnow Studio, LLC
	/// CrazyMinnowStudio.com
	/// 
	/// NOTE:While every attempt has been made to ensure the safe content and operation of 
	/// these files, they are provided as-is, without warranty or guarantee of any kind. 
	/// By downloading and using these files you are accepting any and all risks associated 
	/// and release Crazy Minnow Studio, LLC of any and all liability.
	[CustomEditor(typeof(CM_UmaSync)), CanEditMultipleObjects]
	public class CM_UmaSyncEditor : Editor
	{
		private CM_UmaSync umaSync; // CM_UmaSync reference

		public void OnEnable()
		{			
			umaSync = target as CM_UmaSync; // Get reference
		}

		public override void OnInspectorGUI()
		{
			GUILayout.Space(10);
			EditorGUILayout.BeginVertical();
			{
				umaSync.mode = (CM_UmaSync.Mode)EditorGUILayout.EnumPopup("Mode", umaSync.mode);
				if (umaSync.mode == CM_UmaSync.Mode.DesignTime)
				{
                    umaSync.characterName = string.Empty;
                    umaSync.umaDynamicAvatar = (UMADynamicAvatar)EditorGUILayout.ObjectField("UMADynamicAvatar", umaSync.umaDynamicAvatar, typeof(UMADynamicAvatar), true);
					umaSync.GetUMADynamicAvatar();
				}
				else // Runtime
				{
					umaSync.characterName = EditorGUILayout.TextField("Character Name", umaSync.characterName);
					umaSync.umaDynamicAvatar = null;
					umaSync.initialize = true;
				}
				umaSync.salsaClip = EditorGUILayout.ObjectField("Salsa Clip", umaSync.salsaClip, typeof(AudioClip), true) as AudioClip;
				umaSync.salsa3D = EditorGUILayout.ObjectField("Salsa3D", umaSync.salsa3D, typeof(Salsa3D), true) as Salsa3D;
				umaSync.saySmallTrigger = EditorGUILayout.Slider("saySmall Trigger", umaSync.saySmallTrigger, 0.0001f, 0.0118f);
				umaSync.sayMediumTrigger = EditorGUILayout.Slider("sayMedium Trigger", umaSync.sayMediumTrigger, 0.0002f, 0.0119f);
				umaSync.sayLargeTrigger = EditorGUILayout.Slider("sayLarge Trigger", umaSync.sayLargeTrigger, 0.0003f, 0.012f);
				umaSync.salsaRangeOfMotion = EditorGUILayout.Slider("Range Of Motion", umaSync.salsaRangeOfMotion, 0f, 100f);
				umaSync.salsaBlendSpeed = EditorGUILayout.Slider("Blend Speed", umaSync.salsaBlendSpeed, 0f, 100f);

				GUILayout.Space(10);
				GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) }); // Horizontal rule
				GUILayout.Space(10);

				umaSync.eyeControl = (CM_UmaSync.EyeControl)EditorGUILayout.EnumPopup("Eye Control", umaSync.eyeControl);
				if (umaSync.eyeControl == CM_UmaSync.EyeControl.UseRandomEyes3D)
				{
					umaSync.randomEyes3D = EditorGUILayout.ObjectField("RandomEyes3D", umaSync.randomEyes3D, typeof(RandomEyes3D), true) as RandomEyes3D;
					umaSync.reRangeOfMotion = EditorGUILayout.Slider("Range Of Motion", umaSync.reRangeOfMotion, 0f, 100f);
					umaSync.reBlendSpeed = EditorGUILayout.Slider("Blend Speed", umaSync.reBlendSpeed, 0f, 100f);
				}

			}
			EditorGUILayout.EndVertical();
		}
	}
}
