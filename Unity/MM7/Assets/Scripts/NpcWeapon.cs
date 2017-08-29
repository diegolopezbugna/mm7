using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class NpcWeapon : MonoBehaviour {

    private UMAData umaData;

    [SerializeField]
    private GameObject weaponPrefab;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCharacterCreated()
    {
        var umaDynamicAvatar = GetComponent<DynamicCharacterAvatar>();
        umaData = umaDynamicAvatar.umaData;
        LoadRightHand();
    }

    void LoadRightHand()
    {
        var weapon = Instantiate(weaponPrefab);
        var handGameObject = umaData.GetBoneGameObject("RightHand");
        weapon.transform.SetParent(handGameObject.transform);
        weapon.transform.localPosition = new Vector3(-0.09f, 0.04f, -0.02f);
        weapon.transform.localRotation = Quaternion.identity;
    }

}
