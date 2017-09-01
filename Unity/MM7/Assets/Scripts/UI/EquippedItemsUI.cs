using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;

public class EquippedItemsUI : MonoBehaviour {

    [SerializeField]
    private RawImage body;

    [SerializeField]
    private RawImage rightHand;

    [SerializeField]
    private RawImage leftArm;

    [SerializeField]
    private RawImage leftHand;

    [SerializeField]
    private RawImage leftArmUsed;

    [SerializeField]
    private RawImage leftHandUsed;

    [SerializeField]
    private RawImage weapon1;

    [SerializeField]
    private RawImage weapon2;

    [SerializeField]
    private RawImage missile;

    [SerializeField]
    private RawImage shield;

    [SerializeField]
    private RawImage cloak;

    [SerializeField]
    private RawImage armor;

    [SerializeField]
    private RawImage belt;

    [SerializeField]
    private RawImage boots;

    [SerializeField]
    private RawImage helm;

    private PlayingCharacter playingChar;
    private string resourcesDirectory = "PcBodies";
    private float scale = 0.9f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayingChar(PlayingCharacter playingChar) {
        this.playingChar = playingChar;
        DrawBody();
        DrawEqquipedItems();
    }

    public void DrawBody() {
        body.texture = GetBodyPartTexture("bod");
        Scale(body);
        rightHand.texture = GetBodyPartTexture("rh");
        Scale(rightHand);
        leftArm.texture = GetBodyPartTexture("lad");
        Scale(leftArm);
        leftHand.texture = GetBodyPartTexture("lh");
        Scale(leftHand);
        leftArmUsed.texture = GetBodyPartTexture("lau");
        Scale(leftArmUsed);
        leftHandUsed.texture = GetBodyPartTexture("lhu");
        Scale(leftHandUsed);
    }

    public void DrawEqquipedItems()
    {
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Weapon1, weapon1);
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Weapon2, weapon2);
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Missile, missile);
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Shield, shield);
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Helm, helm);
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Boots, boots);
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Armor, armor);
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Belt, belt);
        CheckAndDrawEquippedItem(playingChar.EquippedItems.Cloak, cloak);
    }

    private Texture GetBodyPartTexture(string bodyPart) {
        return Resources.Load(string.Format("{0}/PC{1}{2}", resourcesDirectory, playingChar.PortraitCode, bodyPart)) as Texture;
    }

    private void CheckAndDrawEquippedItem(Item item, RawImage image) {
        if (item != null)
        {
            image.gameObject.SetActive(true);
            image.texture = GetEquippmentTexture(item);
            Scale(image);
        }
        else
            image.gameObject.SetActive(false);
    }

    private void Scale(RawImage image) {
        image.rectTransform.sizeDelta = new Vector2(image.texture.width * scale, image.texture.height * scale);
    }

    private Texture GetEquippmentTexture(Item item) {
        if (item.IsEquipmentTextureVariant)
            return item.GetEquipmentTexture(playingChar);
        else
            return item.Texture;
    }

}
