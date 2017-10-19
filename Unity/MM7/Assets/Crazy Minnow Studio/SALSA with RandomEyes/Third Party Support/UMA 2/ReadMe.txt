**********************************************
UMA 2 add-on for SALSA
version 1.4
https://crazyminnowstudio.com/posts/uma-lipsync-using-salsa-with-randomeyes/
				
Copyright ©2017 Crazy Minnow Studio, LLC
http://crazyminnowstudio.com/projects/salsa-with-randomeyes-lipsync/
**********************************************

Package Contents
----------------
Crazy Minnow Studio/SALSA with RandomEyes/Third Party Support/
	UMA 2
		Editor
			CM_UmaSetup_DesignTime.cs
				This script provides fast setup for testing SALSA with RandomEyes on an UMA asset character.
			CM_UmaSetup_Runtime.cs
				This script provides 1-click setup for testing SALSA with RandomEyes on a code created UMA character.	
			CM_UmaSyncEditor.cs
				Custom inspector for CM_UmaSync.cs.
		Examples
			Scenes
				DesignTime_Multiple
					A saved UMA.asset demo that creates two different UMA characters with SALSA lipsync and RandomEyes eye control.		
				DesignTime_Single
					A saved UMA.asset demo scene that creates an UMA character with SALSA lipsync and RandomEyes eye control.
				Runtime_Multiple
					A code-scene demo that creates two different UMA characters with SALSA lipsync and RandomEyes eye control.	
				Runtime_Single
					A code-based demo scene that creates an UMA character with SALSA lipsync and RandomEyes eye control.
			Scripts
				CM_UmaExpressionsTester
					A simple tester class to demonstration the expression functions from CM_UmaExpressions
		Prefabs
			UMA_Config
				A prefab that contains the UMA building blocks.
			UMA
				A saved UMA.asset ScriptableObject (DNA).
		CM_UmaBasic.cs
			A simple character creator script that creates basic male or female UMA's.		
		CM_UmaExpressions
			Provides key RandomEyes3D Custom Shapes like functionality to UMAExpressionPlayer.
		CM_UmaSync.cs
			Helper script to apply Salsa and RandomEyes configuration settings and BlendShape data to the UMAExpressionPlayer.
		ReadMe.txt
			This readme file.


Installation Instructions
-------------------------
1. Install SALSA with RandomEyes into your project.
	Select [Window] -> [Asset Store]
	Once the Asset Store window opens, select the download icon, and download and import [SALSA with RandomEyes].

2. Install UMA 2 into your project.
	Select [Window] -> [Asset Store]
	Once the Asset Store window opens, select the download icon, and download and import [UMA 2 - Unity Multipurpose Avatar].

3. Import the SALSA with RandomEyes UMA Character support package.
	Select [Assets] -> [Import Package] -> [Custom Package...]
	Browse to the [SALSA_3rdPartySupport_UMA.unitypackage] file and [Open].


Quick Start Instructions 
------------------------
(Using the included UMA character creator script / Runtime)
	1. In a new scene, use the included 1-click setup from the GameObject menu to add all necessary objects and components.
		[GameObject] -> [Crazy Minnow Studio] -> [UMA 2] -> [SALSA 1-Click UMA Setup]

	2. Play the scene.

(Using a saved UMA.asset file / Design-Time)
	1. In a new scene, use the included design-time setup from the GameObject menu to add all necessary objects and components.
		[GameObject] -> [Crazy Minnow Studio] -> [UMA 2] -> [SALSA UMA Design-Time Setup]

	2. On the SALSA_UMA2 object, add a [Character Created] event to the [UMADynamicAvatar] component,
		then drag SALSA_UMA2 to the Object field and select [CM_UmaSync] -> [CharacterCreated()].

	3. Play the scene.


Implementation Instructions
---------------------------
Runtime
	1. Use, modify, or replace CM_UmaBasic.cs with your own character creation setup.

	2. If you implement your own character creation script, use CM_UmaBasic.cs for reference, you will need to implement a few lines of code.
		Declare the following public string, delegate, and static event. These will be used to pass necessary data from the character creation script to CM_UmaSync.cs.

			// A string to hold a unique character name for this UMA. 
			// CM_UmaSync.cs also has a public characterName string. Each character creation script needs a unique characterName and a corresponding CM_UmaSync.cs script with a matching charcterName string variable.
			public string characterName = "UMA_SALSA_1";

			// Delegate to hold the UMAData object instance and characterName parameters
			public delegate void UMACreated(UMAData obj, string characterName); 

			// Event for CM_UmaSync.cs to subscribe to and receive the UMAData object instance and characterName parameters
			public static event UMACreated OnUMACreated;

		After you initialize your UMADynamicAvatar, subscribe to the UMAData object instance OnCharacterCreated event. See this line of code in CM_UmaBasic.cs for an example.

			umaDynamicAvatar.umaData.OnCharacterCreated += UmaData_OnCharacterCreated;

		This will create an UmaData_OnCharacterCreated event method stub. Remove the Excpetion code and add a null check and event call for our OnUMACreated event. Now when our UMA gets created, this event will be caught and we'll fire our own OnUMACreated event and pass the UMAData object instance and characterName parameters to any subscribers (CM_UmaSync.cs).

			private void UmaData_OnCharacterCreated(UMAData obj)
			{			
				// OnUMACreated will not be null when it has subscribers (CM_UmaSync.cs)
				if (OnUMACreated != null)
				{
					// Fire the event so that subscriber can retreive the UMAData object and characterName
					OnUMACreated(obj, characterName);
				}
			}

		CM_UmaSync.cs can't subscribe to the OnCharacterCreated event directly because the UMAData object instance doesn't exist until runtime.

Design-time
	1. Create your UMA and save the Avatar asset using the [UMA] -> [Load and Save] -> [Save Selected Avatar(s) asset]
	2. Drag this asset file to the scene.
	3. Add CM_UmaSync.cs to the new hierarchy object.
	4. Set the CM_UmaSync Mode to DesignTime.
	5. Add an event listenter to the UMA Dynamic Avatar [Character Created (UMAData)] event.
	6. Add this object to the event object field.
	7. Select the CharacterCreated event from CM_UmaSync.


What [SALSA 1-Click UMA Runtime Setup] does
-------------------------------------------
1. It instanciates an instance of the [UMA_Config] prefab into the scene.

2. It instanciates an empty GameObject [SALSA_UMA2] into the scene and adds the following components:
	[Component] -> [Crazy Minnow Studio] -> [UMA 2] -> [CM_UmaBasic] (UMA character creator)
	[Component] -> [Crazy Minnow Studio] -> [UMA 2] -> [CM_UmaSync] (Adds UMAExpressionSet, Salsa3D, and optionally RandomEyes3D to the UMA)
	[Component] -> [Crazy Minnow Studio] -> [UMA 2] -> [CM_UmaExpressions]

3. On the CM_UmaBasic component, it links the Generator, Slot Library, Overylay Library, Race Library, and the Anim Controller.

4. On the CM_UmaSync component, it links the mil.moves wav file from the Crazy Minnow Studio examples.

What [SALSA Design-Time UMA Setup] does
---------------------------------------
1. It instanciates an instance of the [UMA_Config] prefab into the scene.

2. It instanciates an empty GameObject [SALSA_UMA2] into the scene and adds the following components:
	[UMADynamicAvatar]
	[Component] -> [Crazy Minnow Studio] -> [UMA 2] -> [CM_UmaSync] (Adds UMAExpressionSet, Salsa3D, and optionally RandomEyes3D to the UMA)
	[Component] -> [Crazy Minnow Studio] -> [UMA 2] -> [CM_UmaExpressions]

3. It links the UMA.asset file to the UMA Recipe filed on the UMADynamicAvatar component.
	Assets/Crazy Minnow Studio/SALSA with RandomEyes/Third Party Support/UMA 2/Prefabs/UMA.asset

4. It links the Locomotion animation controller to the UMADynamicAvatar component.
	Assets/UMA/Example/Animators

5. It set the Load On Start property to true on the UMADynamicAvatar component.

6. On the CM_UmaSync component, it sets the Mode to DesignTime, links the UMADynamicAvatar, and adds an AudioClip.

7. After this:
	You will need to add an event listenter to the UMA Dynamic Avatar Character Created (UMAData) event. Link this object to the event object field, then select CharacterCreated from CM_UmaSync.