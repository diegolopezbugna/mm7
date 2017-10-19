using UnityEngine;
using UMA;
using UMA.PoseTools;
using CrazyMinnow.SALSA;

namespace CrazyMinnow.SALSA.UMA
{
	/// <summary>
	/// CM_UmaSync.cs can be used with runtime characters as demonstrated in the 
	/// CM_UmaBasic.cs example script, or at design-time using saved UMA2.asset
	/// ScriptableObject files. 
	/// 
	/// For runtime usage, set the Mode to Runtime.
	/// CM_UmaSync.cs subscribes to the OnUMACreated event and 
	/// uses the event to detect when the UMA character is created so that the 
	/// UMAExpressionSet, Salsa3D, and RandomEyes3D can be added and configured.
	/// This setting also exposes a Character Name field. Make sure this field is
	/// unique, and matches the Character Name field in CM_UmaBasic or your own UMA
	/// creator script, when creating multiple SALSA UMA's. For your implementation, 
	/// use, modify, or replace CM_UmaBasic.cs with your own character creation setup. 
	/// But be sure to implement the OnUMACreated event so that CM_UmaSync.cs can 
	/// subscribe to it. The event passes the UMAData object instance and characterName 
	/// parameters so that CM_UmaSync.cs can add the UMAExpressionSet, UMAExpressionPlayer, 
	/// SALSA, and either RandomEyes or the ExpressionPlayer eye control to the UMA.
	/// See the notes in CM_UmaBasic.cs for more information if you plan to 
	/// implement your own UMA character creation script.
	/// 
	/// For design-time usage, set the Mode to DesignTime.
	/// Design-time mode is for using saved UMA.asset ScriptableObject files to create
	/// your characters. Add CM_UmaSync.cs to your UMA GameObject, add a new Character
	/// Created event, connect your UMA object to the even, and select the 
	/// CharacterCreated event from the CM_UmaSync script.
	/// </summary>
	[AddComponentMenu("Crazy Minnow Studio/UMA 2/CM_UmaSync")]
	public class CM_UmaSync : MonoBehaviour
	{
		public enum Mode { DesignTime, Runtime }
		public Mode mode = Mode.Runtime; // Switches between DesignTime and Runtime modes
		public UMADynamicAvatar umaDynamicAvatar;
		public string characterName = "UMA_SALSA_1";	
        public AudioClip salsaClip; // Audio clip to lipsync
		public Salsa3D salsa3D; // Salsa3D component to be added		
		[Range(0.0001f, 0.0118f)]
		public float saySmallTrigger = 0.0005f;
		[Range(0.0002f, 0.0119f)]
		public float sayMediumTrigger = 0.003f;
		[Range(0.0003f, 0.012f)]
		public float sayLargeTrigger = 0.006f;
		[Range(0f, 100f)]
		public float salsaRangeOfMotion = 60f;
		[Range(0f, 100f)]
		public float salsaBlendSpeed = 0.075f;
		public enum EyeControl { UseExpressionPlayer, UseRandomEyes3D } // Enum to select eye control
		public EyeControl eyeControl = EyeControl.UseRandomEyes3D;
		public RandomEyes3D randomEyes3D; // RandomEyes component to be added (if selected for eye control)
		[Range(0f, 100f)]
		public float reRangeOfMotion = 40f;
		[Range(0f, 100f)]
		public float reBlendSpeed = 8f;
		public bool initialize = true;

		public delegate void SalsaCompleted(Salsa3D salsa3D); // Public delegate to send the Salsa3D object
		public static event SalsaCompleted OnSalsaCompleted; // Public event for subscribers to receive the Salsa3D object

        private UMAData umaData;
		private UMAExpressionPlayer expressionPlayer; // UMAExpressionPlayer to be added
		private Transform leftEye; // Used to set the RandomEyes gizmo for eye tracking
		private Transform rightEye; // Used to set the RandomEyes gizmo for eye tracking		
		private Transform head; // Used to set the RandomEyes gizmo for eye tracking
		private bool lockShapes; // Used to allow access to shape group shapes when SALSA is not talking
        private Vector3 spawnPos; // The initial spawn position

        /// <summary>
        /// Subscribe to OnUMACreated event from CM_UmaBasic.cs
        /// </summary>
        private void OnEnable()
		{
			CM_UmaBasic.OnUMACreated += CM_UmaBasic_OnUMACreated;
        }

        /// <summary>
        /// Unsubscribe from OnUMACreated event from CM_UmaBasic.cs
        /// </summary>
        private void OnDisable()
		{
			CM_UmaBasic.OnUMACreated -= CM_UmaBasic_OnUMACreated;
		}

        /// <summary>
        /// Unsubscribe from OnUMACreated event from CM_UmaBasic.cs
        /// </summary>
        private void OnDestroy()
		{
			CM_UmaBasic.OnUMACreated -= CM_UmaBasic_OnUMACreated;
		}

        /// <summary>
        /// Grab the position during awake
        /// </summary>
        private void Awake()
        {
            if (mode == Mode.Runtime)
                spawnPos = transform.position;
        }

        /// <summary>
        /// Update Syncs lipsync from Salsa3D, and eye movement and facial animation from RandomEyes3D
        /// </summary>
        private void LateUpdate()
		{
			// Toggle shape lock to provide access to shape group shapes when SALSA is not talking
			if (salsa3D)
			{
				if (salsa3D.sayAmount.saySmall == 0f && salsa3D.sayAmount.sayMedium == 0f && salsa3D.sayAmount.sayLarge == 0f)
				{
					lockShapes = false;
				}
				else
				{
					lockShapes = true;
				}
			}

			if (salsa3D && lockShapes)
			{
				/* Here we use the Salsa3D.sayIndex to determine which mouth positions are active.
				 * Salsa3D.sayIndex retuns the following values:
				 * 0 = rest
				 * 1 = saySmall
				 * 2 = sayMedium
				 * 3 = sayLarge
				 *
				 * The SalsaUtility.LerpRangeOfMotion is a static method, included with SALSA, that adjusts 
				 * a value over time, using a specified blend speed, towards a maximum blend value, using linear interpolation.
				 * You can think of this code block as a set of four groups of shapes that map to SALSA's 
				 * rest, small, medium, and large mouth shapes. To adjust the range of motion, use the 
				 * [Range of Motion] slider in the Salsa3D inspector. Each of the shape groups below conform well
				 * to the mouth shape recommendations found in our manual.
				 * http://crazyminnowstudio.com/projects/salsa-with-randomeyes-lipsync/manuals/salsa-manual/
				 */
				switch (salsa3D.sayIndex)
				{
					case 0: // Rest position
						expressionPlayer.jawOpen_Close = SalsaUtility.LerpRangeOfMotion(expressionPlayer.jawOpen_Close, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.leftMouthSmile_Frown = SalsaUtility.LerpRangeOfMotion(expressionPlayer.leftMouthSmile_Frown, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.rightMouthSmile_Frown = SalsaUtility.LerpRangeOfMotion(expressionPlayer.rightMouthSmile_Frown, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.leftLowerLipUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.leftLowerLipUp_Down, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.rightLowerLipUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.rightLowerLipUp_Down, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.tongueUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.tongueUp_Down, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.mouthNarrow_Pucker = SalsaUtility.LerpRangeOfMotion(expressionPlayer.mouthNarrow_Pucker, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						break;
					case 1: // saySmall
						expressionPlayer.jawOpen_Close = SalsaUtility.LerpRangeOfMotion(expressionPlayer.jawOpen_Close, salsa3D.blendSpeed, 0.1f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Increment);
						expressionPlayer.leftMouthSmile_Frown = SalsaUtility.LerpRangeOfMotion(expressionPlayer.leftMouthSmile_Frown, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.rightMouthSmile_Frown = SalsaUtility.LerpRangeOfMotion(expressionPlayer.rightMouthSmile_Frown, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.leftLowerLipUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.leftLowerLipUp_Down, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.rightLowerLipUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.rightLowerLipUp_Down, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.tongueUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.tongueUp_Down, salsa3D.blendSpeed, 0.5f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Increment);
						expressionPlayer.mouthNarrow_Pucker = SalsaUtility.LerpRangeOfMotion(expressionPlayer.mouthNarrow_Pucker, salsa3D.blendSpeed, 0.6f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Increment);
						break;
					case 2: // sayMedium
						expressionPlayer.jawOpen_Close = SalsaUtility.LerpRangeOfMotion(expressionPlayer.jawOpen_Close, salsa3D.blendSpeed, 0.25f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Increment);
						expressionPlayer.leftMouthSmile_Frown = SalsaUtility.LerpRangeOfMotion(expressionPlayer.leftMouthSmile_Frown, salsa3D.blendSpeed, 1f, SalsaUtility.BlendDirection.Increment);
						expressionPlayer.rightMouthSmile_Frown = SalsaUtility.LerpRangeOfMotion(expressionPlayer.rightMouthSmile_Frown, salsa3D.blendSpeed, 1f, SalsaUtility.BlendDirection.Increment);
						expressionPlayer.leftLowerLipUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.leftLowerLipUp_Down, salsa3D.blendSpeed, -1f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.rightLowerLipUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.rightLowerLipUp_Down, salsa3D.blendSpeed, -1f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.tongueUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.tongueUp_Down, salsa3D.blendSpeed, 1f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Increment);
						expressionPlayer.mouthNarrow_Pucker = SalsaUtility.LerpRangeOfMotion(expressionPlayer.mouthNarrow_Pucker, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						break;
					case 3: // sayLarge
						expressionPlayer.jawOpen_Close = SalsaUtility.LerpRangeOfMotion(expressionPlayer.jawOpen_Close, salsa3D.blendSpeed, 0.7f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Increment);
						expressionPlayer.leftMouthSmile_Frown = SalsaUtility.LerpRangeOfMotion(expressionPlayer.leftMouthSmile_Frown, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.rightMouthSmile_Frown = SalsaUtility.LerpRangeOfMotion(expressionPlayer.rightMouthSmile_Frown, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.leftLowerLipUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.leftLowerLipUp_Down, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.rightLowerLipUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.rightLowerLipUp_Down, salsa3D.blendSpeed, 0f, SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.tongueUp_Down = SalsaUtility.LerpRangeOfMotion(expressionPlayer.tongueUp_Down, salsa3D.blendSpeed, -1f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Decrement);
						expressionPlayer.mouthNarrow_Pucker = SalsaUtility.LerpRangeOfMotion(expressionPlayer.mouthNarrow_Pucker, salsa3D.blendSpeed, 1f * (salsa3D.rangeOfMotion / 100), SalsaUtility.BlendDirection.Increment);
						break;
				}
			}

			/* Here we use the RandomEyes3D.lookAmount values to drive the eye look and blink sliders.
			 * You can also set a look target for eye tracking:
			 *		randomEyes3D.SetLookTarget(GameObject obj);
			 * or enable target affinity. This works like an attention span when tracking a target
			 *		randomEyes3D.SetTargetAffinity(bool status);
			 * and set the affinity percentage, or the percentage of time to track the target.
			 * The remainder of the time will be filled with random looking around.
			 *		randomEyes3D.SetAffinityPercentage(float percent);
			 */
			if (randomEyes3D != null)
			{ 
				// Blink
				expressionPlayer.leftEyeOpen_Close = randomEyes3D.lookAmount.blink / 100 * -1;
				expressionPlayer.rightEyeOpen_Close = expressionPlayer.leftEyeOpen_Close;
				// LookUp
				if (randomEyes3D.lookAmount.lookUp > 0)
				{
					expressionPlayer.leftEyeUp_Down = randomEyes3D.lookAmount.lookUp / 100;
					expressionPlayer.rightEyeUp_Down = randomEyes3D.lookAmount.lookUp / 100;
				}
				// LookDown
				if (randomEyes3D.lookAmount.lookDown > 0)
				{
					expressionPlayer.leftEyeUp_Down = randomEyes3D.lookAmount.lookDown / 100 * -1;
					expressionPlayer.rightEyeUp_Down = randomEyes3D.lookAmount.lookDown / 100 * -1;
				}
				// LookLeft
				if (randomEyes3D.lookAmount.lookLeft > 0)
				{
					expressionPlayer.leftEyeIn_Out = randomEyes3D.lookAmount.lookLeft / 100 * -1;
					expressionPlayer.rightEyeIn_Out = randomEyes3D.lookAmount.lookLeft / 100;
				}
				// LookRight
				if (randomEyes3D.lookAmount.lookRight > 0)
				{
					expressionPlayer.leftEyeIn_Out = randomEyes3D.lookAmount.lookRight / 100;
					expressionPlayer.rightEyeIn_Out = randomEyes3D.lookAmount.lookRight / 100 * -1;
				}
			}
		}
		
        /// <summary>
		/// OnUMACreated subscription event
		/// </summary>
		private void CM_UmaBasic_OnUMACreated(UMAData obj, string characterName)
		{
			if (characterName == this.characterName)
			{
				umaData = obj; // Set the UMAData object

				Initialize();
                if (mode == Mode.Runtime)
                    obj.transform.position = spawnPos;
            }
        }

        /// <summary>
        /// Initialize the UMAExpressionSet, Salsa3D, and eye control
        /// </summary>
        private void Initialize()
        {
            if (umaData != null)
            {
                // Add and initialize the UMAExpressionSet
                UMAExpressionSet expressionSet = umaData.umaRecipe.raceData.expressionSet;
                expressionPlayer = umaData.gameObject.AddComponent<UMAExpressionPlayer>();
                expressionPlayer.expressionSet = expressionSet;
                expressionPlayer.umaData = umaData;
                expressionPlayer.Initialize();

                AddSalsa(); // Add and configure SALSA lipsync

                if (eyeControl == EyeControl.UseExpressionPlayer) //Use UMAExpressionSet eye control
                {
                    expressionPlayer.enableBlinking = true;
                    expressionPlayer.enableSaccades = true;
                }
                else // Use RandomEyes3D eye control
                {
                    expressionPlayer.enableBlinking = false;
                    expressionPlayer.enableSaccades = false;

                    AddRandomEyes(); // Add and configure RandomEyes
                }

                /* Subscribe to this event to be notified once the Salsa and/or 
				 * RandomEyes setup are complete. It's Also required when using 
				 * the live mic or RT-Voice text-to-speech workflow. 
				 */
                if (OnSalsaCompleted != null)
                    OnSalsaCompleted(salsa3D);
            }
        }

        /// <summary>
        /// Add and configure the Salsa3D component
        /// </summary>
        private void AddSalsa()
        {
            salsa3D = umaData.gameObject.AddComponent<Salsa3D>(); // Add the Salsa3D component
            salsa3D.SetAudioClip(salsaClip); // Set the SALSA AudioClip
            salsa3D.saySmallTrigger = saySmallTrigger; // Amplitude level to trigger saySmall mouth shape
            salsa3D.sayMediumTrigger = sayMediumTrigger; // Amplitude level to trigger sayMedium mouth shape
            salsa3D.sayLargeTrigger = sayLargeTrigger; // Amplitude level to trigger sayLarge mouth shape
            salsa3D.SetRangeOfMotion(salsaRangeOfMotion); // Set the SALSA range of motion
            salsa3D.blendSpeed = salsaBlendSpeed; // Set the SALSA blend speed
            salsa3D.Play(); // Play the AudioClip to begin lipsync
        }

        /// <summary>
        /// Add and configure the RandomEyes3D component
        /// </summary>
        private void AddRandomEyes()
        {
            randomEyes3D = umaData.gameObject.AddComponent<RandomEyes3D>(); // Add the RandomEyes3D component
            randomEyes3D.SetRangeOfMotion(reRangeOfMotion); // Set the max range of motion
            randomEyes3D.SetBlendSpeed(reBlendSpeed); // Set the blend speed
            randomEyes3D.FindOrCreateEyePositionGizmo(); // Add the RandomEyes

            // Get child transforms
            Transform[] items = randomEyes3D.gameObject.GetComponentsInChildren<Transform>();
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].name == "LeftEye") leftEye = items[i];
                if (items[i].name == "RightEye") rightEye = items[i];
                if (items[i].name == "Head") head = items[i];
            }

            // Position the RandomEyes gizmo between the eyes
            randomEyes3D.eyePosition.transform.position =
                ((leftEye.transform.position - rightEye.transform.position) * 0.5f) + rightEye.transform.position;

            // Parent the RandomEyes gizmo to the head
            randomEyes3D.eyePosition.transform.parent = head;
        }

        /// <summary>
        /// Get reference to the local UMADynamicAvatar
        /// </summary>
        public void GetUMADynamicAvatar()
		{
			if (umaDynamicAvatar == null)
			{
				umaDynamicAvatar = GetComponent<UMADynamicAvatar>();
			}
		}

		/// <summary>
		/// When using CM_UmaSync in DesignTime mode, connect the 
		/// UMADynamicAvatar Character Created (UMAData) event to this method.
		/// </summary>
        public void CharacterCreated(UMAData umaData)
		{
//			GetUMADynamicAvatar();
//            umaData = umaDynamicAvatar.umaData;
            this.umaData = umaData;

			Initialize();
		}
	}
}