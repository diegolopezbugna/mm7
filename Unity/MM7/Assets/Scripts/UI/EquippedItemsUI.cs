using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class EquippedItemsUI : MonoBehaviour, ItemsContainerUI, IPointerDownHandler {

    private RawImage body;
    private RawImage rightHand;
    private RawImage leftArm;
    private RawImage leftHand;
    private RawImage leftArmUsed;
    private RawImage leftHandUsed;

    private RawImage weapon1;
    private RawImage weapon2;
    private RawImage missile;
    private RawImage shield;
    private RawImage cloak;
    private RawImage armor;
    private RawImage belt;
    private RawImage boots;
    private RawImage helm;

    private PlayingCharacter playingChar;
    private string resourcesDirectory = "PcBodies";
    private float scale = 0.9f;
    private Vector2 centeredPivot = new Vector2(0.5f, 0.5f);
    private Vector2 centeredBottomPivot = new Vector2(0.5f, 0f);
    private Vector2 centeredTopPivot = new Vector2(0.5f, 1f);

    private CharDetailsUI charDetailsUI; 
    private RectTransform rectTransform;

    void Awake() {
        charDetailsUI = GetComponentInParent<CharDetailsUI>();
        rectTransform = GetComponent<RectTransform>();
    }

	void Start () {
	}
	
	void Update () {
	}

    public void SetPlayingChar(PlayingCharacter playingChar) 
    {
        this.playingChar = playingChar;
        Draw();
    }

    private void Clean()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    public void Draw() 
    {
        Clean(); // TODO: reuse gameobjects
        missile = CheckAndDrawEquippedItem(playingChar.EquippedItems.Missile, centeredPivot, new Vector2(11f, 28f));
        cloak = CheckAndDrawEquippedItem(playingChar.EquippedItems.Cloak, centeredBottomPivot, new Vector2(1.7f, 8f));
        body = DrawBodyPart("bod", centeredBottomPivot, Vector2.zero);
        leftArm = DrawBodyPart("lad", centeredPivot, new Vector2(34.8f, 21.9f));
        armor = CheckAndDrawEquippedItem(playingChar.EquippedItems.Armor, centeredTopPivot, new Vector2(2f, -67f));
        belt = CheckAndDrawEquippedItem(playingChar.EquippedItems.Belt, centeredPivot, new Vector2(3f, -6.85f));
        weapon1 = CheckAndDrawEquippedItem(playingChar.EquippedItems.Weapon1, new Vector2(0.5f, 0.2f), new Vector2(-44.3f, 125f));
        weapon2 = CheckAndDrawEquippedItem(playingChar.EquippedItems.Weapon2, new Vector2(0.5f, 0.2f), new Vector2(-44.3f, 120f));
        rightHand = DrawBodyPart("rh", centeredPivot, new Vector2(-44.3f, 30.5f));
        leftHand = DrawBodyPart("lh", centeredPivot, new Vector2(44.2f, -22.5f));
        leftArmUsed = DrawBodyPart("lau", centeredPivot, new Vector2(-7.15f, 37.1f));
        leftHandUsed = DrawBodyPart("lhu", centeredPivot, new Vector2(-44.83f, 37.1f));
        shield = CheckAndDrawEquippedItem(playingChar.EquippedItems.Shield, centeredPivot, new Vector2(34.2f, -17.1f));
        helm = CheckAndDrawEquippedItem(playingChar.EquippedItems.Helm, centeredTopPivot, new Vector2(-3.3f, -15.11f));
        boots = CheckAndDrawEquippedItem(playingChar.EquippedItems.Boots, centeredBottomPivot, new Vector2(0f, 0f));
    }

    private RawImage DrawBodyPart(string bodyPart, Vector2 pivotAnchor, Vector2 anchoredPosition) 
    {
        var go = new GameObject(bodyPart);
        go.transform.SetParent(rectTransform, false);
        //go.transform.SetAsLastSibling();
        var image = go.AddComponent<RawImage>();
        image.texture = GetBodyPartTexture(bodyPart);
        Scale(image);
        image.rectTransform.pivot = pivotAnchor;
        image.rectTransform.anchorMin = pivotAnchor;
        image.rectTransform.anchorMax = pivotAnchor;
        image.rectTransform.anchoredPosition = anchoredPosition;
        image.raycastTarget = false;
        return image;
    }

    private Texture GetBodyPartTexture(string bodyPart) 
    {
        return Resources.Load(string.Format("{0}/PC{1}{2}", resourcesDirectory, playingChar.PortraitCode, bodyPart)) as Texture;
    }

    private RawImage CheckAndDrawEquippedItem(Item item, Vector2 pivotAnchor, Vector2 anchoredPosition) 
    {
        if (item != null)
        {
            var go = new GameObject(item.PictureFilename);
            go.transform.SetParent(rectTransform, false);
            //go.transform.SetAsLastSibling();
            var image = go.AddComponent<RawImage>();
            image.texture = GetEquippmentTexture(item);
            image.raycastTarget = true;
            image.rectTransform.pivot = pivotAnchor;
            image.rectTransform.anchorMin = pivotAnchor;
            image.rectTransform.anchorMax = pivotAnchor;
            image.rectTransform.anchoredPosition = anchoredPosition;
            Scale(image);
            var inventoryItem = go.AddComponent<InventoryItem>();
            inventoryItem.Item = item;
            inventoryItem.ItemsContainerUI = this;
            return image;
        }
        return null;
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

    public void OnInventoryItemPointerDown(Item item, PointerEventData eventData, InventoryItem equippedItem) {
        charDetailsUI.OnEquippedItemPointerDown(playingChar, item, eventData, equippedItem);
    }

    #region IPointerDownHandler implementation

    public void OnPointerDown(PointerEventData eventData)
    {
        charDetailsUI.OnRightPanelPointerDown(playingChar, eventData);
    }

    #endregion

}
