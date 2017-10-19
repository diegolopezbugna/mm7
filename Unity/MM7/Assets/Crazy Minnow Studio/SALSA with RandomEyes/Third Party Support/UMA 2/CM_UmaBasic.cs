using UnityEngine;
using UMA;
using UMA.PoseTools;
using CrazyMinnow.SALSA;

namespace CrazyMinnow.SALSA.UMA
{
	/// <summary>
	/// This is a basic demonstration UMA character creation script. 
	/// It's used to create a simple male or female UMA so that the CM_UmaSync.cs
	/// script can setup SALSA with RandomEyes on it.
	/// 
	/// If you implement your own character creation script, use this for reference, 
	/// you will need to implement a few lines of code. Declare the following public 
	/// string, delegate, and static event variables. These will be used to pass 
	/// necessary data from the character creation script to CM_UmaSync.cs.
	///
	/// Public characterName string to hold a unique character name for this UMA.
	/// CM_UmaSync.cs also has a public characterName string.
	/// Each character creation script needs a unique characterName
	/// and a corresponding CM_UmaSync.cs script with a matching
	/// charcterName string variable.
	/// 
	///		public string characterName = "UMA_SALSA_1";
	/// 
	/// Public delegate to hold the UMAData object instance and characterName parameters
	/// 
	///		public delegate void UMACreated(UMAData obj, string characterName);
	/// 
	/// Public event for CM_UmaSync.cs to subscribe to and receive the UMAData object 
	/// instance and characterName parameters
	/// 
	///		public static event UMACreated OnUMACreated;
	/// 
	/// After you initialize your UMADynamicAvatar, subscribe to the UMAData object instance 
	/// OnCharacterCreated event. See this line of code in the GenerateUMA() method below.
	/// 
	///		umaDynamicAvatar.umaData.OnCharacterCreated += UmaData_OnCharacterCreated;
	/// 
	/// This will create an UmaData_OnCharacterCreated event method stub. Remove the Excpetion 
	/// code and add a null check and event call for our OnUMACreated event. Now when our UMA 
	/// gets created, this event will be caught and we'll fire our own OnUMACreated event and 
	/// pass the UMAData object instance and characterName parameters to any 
	/// subscribers (CM_UmaSync.cs).
	/// 
	///		private void UmaData_OnCharacterCreated(UMAData obj)
	///		{
	/// 		// OnUMACreated will not be null when it has subscribers (CM_UmaSync.cs)
	///			if (OnUMACreated != null)
	///			{
	///				// Fire the event so that subscriber can retreive the UMAData object and characterName
	///				OnUMACreated(obj, characterName);
	///			}
	///		}
	/// 
	/// CM_UmaSync.cs can't subscribe to the OnCharacterCreated event directly because the 
	/// UMAData object instance doesn't exist until runtime.
	/// 
	/// 
	/// We heavily relied on [Secret Anorak]'s wonderful [A Practical Guide To UMA] 
	/// video series to learn the UMA basics and write this script.
	/// 
	/// https://www.youtube.com/watch?v=XN0E1G2ylbs&list=PLkDHFObfS19wRJ9vvaDTwCe-zRPv9jI25&index=1
	/// 
	/// Thanks Secret Anorak!
	/// </summary>
	[AddComponentMenu("Crazy Minnow Studio/UMA 2/CM_UmaBasic")]
	public class CM_UmaBasic : MonoBehaviour
	{
		public string characterName = "UMA_SALSA_1";
		public UMAGeneratorBase generator;
		public SlotLibrary slotLibrary;
		public OverlayLibrary overlayLibrary;
		public RaceLibrary raceLibrary;
		public RuntimeAnimatorController animController;

		public delegate void UMACreated(UMAData obj, string characterName); // Public delegate for CM_UmaSync.cs to receive the UMAData object
		public static event UMACreated OnUMACreated; // Public event for CM_UmaSync to subscribe to and receive the UMAData object and characterName string

		public enum Gender { Male, Female };
		public Gender gender = Gender.Male;

		private UMADynamicAvatar umaDynamicAvatar;
		private UMADnaHumanoid umaDna;
		private GameObject umaObj;
		private int numberOfSlots = 20;

		/// <summary>
		/// This event is was created by subscribing to the OnCharacterCreated
		/// UMAData instance found in the GenerateUMA() method below:
		///		umaData.OnCharacterCreated += UmaData_OnCharacterCreated;
		/// The event is caught here after the UMA has been created, and we
		/// then fire the OnUMACreated event and pass the UMAData object instance
		/// and the character name string to any subscribers of the OnUMACreated 
		/// event (CM_UmaSync.cs).
		/// </summary>
		/// <param name="obj"></param>
		private void UmaData_OnCharacterCreated(UMAData obj)
		{
			if (OnUMACreated != null) // OnUMACreated will not be null when it has subscribers
			{
				OnUMACreated(obj, characterName); // Fire the event so that subscriber can retreive the UMAData object
			}
		}

		/// <summary>
		/// Awake
		/// </summary>
		void Awake()
		{
			GenerateUMA();
		}

		/// <summary>
		/// Create the UMA character
		/// </summary>
		void GenerateUMA()
		{
			// Assign this gameObject to umaObj, this is where additional UMA components will be added
			umaObj = gameObject;
			umaDynamicAvatar = umaObj.AddComponent<UMADynamicAvatar>();

			// Initialize the avatar and get the UMAData object
			umaDynamicAvatar.Initialize();
			/* Subscribe to the OnCharacterCreated event in the umaData instance.
			This will trigger an event when the character creation is complete,
			and send this instance of the UMAData object as a parameter. 
			Inside that event, we'll fire the OnUMACreated event and forward 
			the UMAData object instance and the characterName parameters. 
			The CM_UmaSync.cs script subscribes to the OnUMACreated event and 
			uses the UMAData object and the characterName parameters to 
			perform the SALSA and eye control setup. */
			umaDynamicAvatar.umaData.OnCharacterCreated += UmaData_OnCharacterCreated;

			// Attach the generator to the UMADynamicAvitar
			umaDynamicAvatar.umaGenerator = generator;
			umaDynamicAvatar.umaData.umaGenerator = generator;

			// Setup slot array
			umaDynamicAvatar.umaData.umaRecipe.slotDataList = new SlotData[numberOfSlots];

			// Setup a morph reference
			umaDna = new UMADnaHumanoid();
			umaDynamicAvatar.umaData.umaRecipe.AddDna(umaDna);

			// Setup the race slots and overlays
			SetRace(gender);

			// Assign the animation controller to the UMADynamicAvatar
			umaDynamicAvatar.animationController = animController;

			// Generator the UMA
			umaDynamicAvatar.UpdateNewRace();

			// Set the parent and zero out local position and rotation
			umaObj.transform.parent = this.transform;
			umaObj.transform.localPosition = Vector3.zero;
			umaObj.transform.localRotation = Quaternion.identity;
		}

		/// <summary>
		/// Setup the race slots and overlays. These parameters 
		/// switch based on the Gender enum selector selection.
		/// </summary>
		void SetRace(Gender gender)
		{
			var umaRecipe = umaDynamicAvatar.umaData.umaRecipe;
			umaRecipe.SetRace(raceLibrary.GetRace("Human" + gender));

			InstantiateSlot(0, gender + "Eyes");
			AddOverlay(0, "EyeOverlay");

			InstantiateSlot(1, gender + "InnerMouth");
			AddOverlay(1, "InnerMouth");

			InstantiateSlot(2, gender + "Face");

			InstantiateSlot(3, gender + "Torso");
			AddOverlay(3, gender + "Body01");

			InstantiateSlot(4, gender + "Hands");
			LinkOverlay(4, 3);

			InstantiateSlot(5, gender + "Legs");
			LinkOverlay(5, 3);

			InstantiateSlot(6, gender + "Feet");
			LinkOverlay(6, 3);

			if (gender == Gender.Male)
			{
				AddOverlay(2, gender + "Head02");
				AddOverlay(2, gender + "Hair02", new Color(0.28f, 0.22f, 0.18f));
			}
			else // Female
			{
				AddOverlay(2, gender + "Head01");
				AddOverlay(2, gender + "ShortHair01", new Color(0.28f, 0.22f, 0.18f));
			}

			AddOverlay(2, gender + "Eyebrow01", new Color(0.28f, 0.22f, 0.18f));
			AddOverlay(3, gender + "Underwear01", new Color(0.1f, 0.1f, 0.1f));
			AddOverlay(3, gender + "Shirt01", new Color(0.35f, 0.35f, 0.35f));
		}

		/// <summary>
		/// Instanciate slots
		/// </summary>
		/// <param name="slot"></param>
		/// <param name="slotName"></param>
		void InstantiateSlot(int slot, string slotName)
		{
			umaDynamicAvatar.umaData.umaRecipe.slotDataList[slot] = slotLibrary.InstantiateSlot(slotName);
		}

		/// <summary>
		/// Add overlays
		/// </summary>
		/// <param name="slot"></param>
		/// <param name="overlayName"></param>
		void AddOverlay(int slot, string overlayName)
		{
			umaDynamicAvatar.umaData.umaRecipe.slotDataList[slot].AddOverlay(overlayLibrary.InstantiateOverlay(overlayName));
		}

		/// <summary>
		/// Add overlays with color
		/// </summary>
		/// <param name="slot"></param>
		/// <param name="overlayName"></param>
		/// <param name="color"></param>
		void AddOverlay(int slot, string overlayName, Color color)
		{
			umaDynamicAvatar.umaData.umaRecipe.slotDataList[slot].AddOverlay(overlayLibrary.InstantiateOverlay(overlayName, color));
		}

		/// <summary>
		/// Link to existing overlays
		/// </summary>
		/// <param name="slot"></param>
		/// <param name="slotToLink"></param>
		void LinkOverlay(int slot, int slotToLink)
		{
			umaDynamicAvatar.umaData.umaRecipe.slotDataList[slot].SetOverlayList(umaDynamicAvatar.umaData.umaRecipe.slotDataList[slotToLink].GetOverlayList());
		}
	}
}