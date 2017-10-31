using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class EquippedItemsUI : MonoBehaviour, IPointerDownHandler {

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
        var equippedItems = playingChar.EquippedItems;
        Clean(); // TODO: reuse gameobjects

        if (equippedItems.Missile != null)
            DrawEquippedItem(equippedItems.Missile, centeredPivot, new Vector2(11f, 28f));

        if (equippedItems.Cloak != null)
            DrawEquippedItem(equippedItems.Cloak, centeredBottomPivot, new Vector2(1.7f, 8f));

        DrawBodyPart("bod", centeredBottomPivot, GetBodyPosition());

        if (equippedItems.Armor != null)
            DrawEquippedItem(equippedItems.Armor, Vector2.one, GetArmorPosition(equippedItems.Armor));

        if (!equippedItems.IsDualHandWeaponEquipped)
            DrawBodyPart("lad", centeredPivot, GetLeftArmPosition());

        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            DrawBodyPart("brd", centeredTopPivot, new Vector2(-6.8f, playingChar.PortraitCode == "14" ? -99.5f : -91.1f));

        if (equippedItems.Belt != null)
            DrawEquippedItem(equippedItems.Belt, centeredPivot, GetBeltPosition());

        var rightHand = DrawBodyPart("rh", Vector2.up, GetRightHandPosition());

        if (equippedItems.WeaponRight != null)
            DrawEquippedItem(equippedItems.WeaponRight, Vector2.up, GetWeaponRightPosition(equippedItems.WeaponRight, rightHand.texture.width));

        if (equippedItems.IsDualWeaponsWielding)
            DrawEquippedItem(equippedItems.WeaponLeft, Vector2.up, GetWeaponRightPosition(equippedItems.WeaponLeft, rightHand.texture.width)); // TODO: dual wielding position

        rightHand.transform.SetAsLastSibling();

        if (equippedItems.IsDualWeaponsWielding)
            DrawBodyPart("lh", centeredPivot, GetLeftHandPosition());

        if (equippedItems.IsDualHandWeaponEquipped)
            DrawBodyPart("lau", centeredPivot, GetLeftArmDualHandWeaponPosition());

//        if (equippedItems.IsDualHandWeaponEquipped)
//            leftHandUsed = DrawBodyPart("lhu", centeredPivot, new Vector2(-44.83f, 37.1f)); // is it needed?

        if (equippedItems.Shield != null)
            DrawEquippedItem(equippedItems.Shield, centeredPivot, new Vector2(34.2f, -17.1f));

        if (equippedItems.Helm != null)
            DrawEquippedItem(equippedItems.Helm, centeredBottomPivot, GetHelmPosition(equippedItems.Helm));

        if (equippedItems.Boots != null)
            DrawEquippedItem(equippedItems.Boots, centeredBottomPivot, GetBootsPosition());

        if (equippedItems.Amulet != null)
            DrawEquippedItem(equippedItems.Amulet, Vector2.one, Vector2.zero);

        if (equippedItems.Gauntlets[0] != null)
            DrawEquippedItem(equippedItems.Gauntlets[0], Vector2.zero, new Vector2(0f,  90f * scale));
        if (equippedItems.Gauntlets[1] != null)
            DrawEquippedItem(equippedItems.Gauntlets[1], Vector2.right, new Vector2(0f,  90f * scale));

        if (equippedItems.Rings[0] != null)
            DrawEquippedItem(equippedItems.Rings[0], Vector2.zero, Vector2.zero);
        if (equippedItems.Rings[1] != null)
            DrawEquippedItem(equippedItems.Rings[1], Vector2.right, Vector2.zero);
        if (equippedItems.Rings[2] != null)
            DrawEquippedItem(equippedItems.Rings[2], Vector2.zero, new Vector2(0f,  30f * scale));
        if (equippedItems.Rings[3] != null)
            DrawEquippedItem(equippedItems.Rings[3], Vector2.right, new Vector2(0f, 30f * scale));
        if (equippedItems.Rings[4] != null)
            DrawEquippedItem(equippedItems.Rings[4], Vector2.zero, new Vector2(0f, 60f * scale));
        if (equippedItems.Rings[5] != null)
            DrawEquippedItem(equippedItems.Rings[5], Vector2.right, new Vector2(0f, 60f * scale));
    }

    private Vector2 GetBodyPosition()
    {
        var t = GetBodyPartTexture("bod");
        return new Vector2((t.width - 173f) / 2f, - (t.height - 353f));
    }

    private Vector2 GetRightHandPosition()
    {
        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            return new Vector2(19.25f, -146.95f);
        else if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Female)
            return new Vector2(25.4f, -143.54f);
        else if (playingChar.Gender == Gender.Female)
            return new Vector2(27.9f, -117.7f);
        else
            return new Vector2(23.85f, -106.75f);
    }

    private Vector2 GetLeftArmPosition()
    {
        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            return new Vector2(43.3f, 1.1f);
        else if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Female)
            return new Vector2(41.8f, -1.52f);
        else
            return new Vector2(34.8f, 21.9f);
    }

    private Vector2 GetLeftArmDualHandWeaponPosition()
    {
        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            return new Vector2(-8.58f, 5.78f);
        else if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Female)
            return new Vector2(-7.66f, 1.83f);
        else
            return new Vector2(-7.15f, 37.1f);
    }

    private Vector2 GetLeftHandPosition()
    {
        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            return new Vector2(49.6f, -33.4f);
        else if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Female)
            return new Vector2(43.7f, -32.9f);
        else if (playingChar.Gender == Gender.Female)
            return new Vector2(48.2f, -20.1f);
        else
            return new Vector2(44.2f, -22.5f);
    }

    private Vector2 GetArmorPosition(Item item)
    {
        Vector2 pos;
        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            pos = new Vector2(-GetLeftArmPosition().x + 5f, -99.8f);
        else if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Female)
            pos = new Vector2(-GetLeftArmPosition().x + 2f, -103f);
        else if (playingChar.Gender == Gender.Female)
            pos = new Vector2(-GetLeftArmPosition().x - 13f, -67f);
        else
            pos = new Vector2(-GetLeftArmPosition().x + 2f, -67f);
        pos.x += item.EquipX * scale;
        pos.y += item.EquipY * scale;
        return pos;
    }

    private Vector2 GetHelmPosition(Item item)
    {
        Vector2 pos;
        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            pos = new Vector2(-1f, 209.62f);
        else if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Female)
            pos = new Vector2(1.92f, 211.73f);
        else
            pos = new Vector2(-2f, 248f);
        pos.x += item.EquipX * scale;
        pos.y += item.EquipY * scale;
        return pos;
    }

    private Vector2 GetBeltPosition()
    {
        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            return new Vector2(-2.15f, -35.8f);
        else if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Female)
            return new Vector2(0.43f, -28.32f);
        else if (playingChar.Gender == Gender.Female)
            return new Vector2(-2.15f, 8.6f);
        else
            return new Vector2(1.12f, -6.85f);
    }

    private Vector2 GetWeaponRightPosition(Item item, float rightHandWidth)
    {
        var rightHandPos = GetRightHandPosition();
        return new Vector2(rightHandPos.x + rightHandWidth - 19f - item.EquipX * scale, rightHandPos.y + item.EquipY * scale);
    }

    private Vector2 GetBootsPosition()
    {
        if (playingChar.Race.RaceCode == RaceCode.Dwarf && playingChar.Gender == Gender.Male)
            return new Vector2(-3f, 0f);
        else
            return Vector2.zero;
    }

    private RawImage DrawBodyPart(string bodyPart, Vector2 pivotAnchor, Vector2 anchoredPosition) 
    {
        var go = new GameObject(bodyPart);
        go.transform.SetParent(rectTransform, false);
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
        // TODO: cache body parts
        return Resources.Load(string.Format("{0}/PC{1}{2}", resourcesDirectory, playingChar.PortraitCode, bodyPart)) as Texture;
    }

    private RawImage DrawEquippedItem(Item item, Vector2 pivotAnchor, Vector2 anchoredPosition) 
    {
        var go = new GameObject(item.PictureFilename);
        go.transform.SetParent(rectTransform, false);
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
        inventoryItem.OnItemPointerDown = OnInventoryItemPointerDown;
        return image;
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

    public void OnInventoryItemPointerDown(Item item, PointerEventData eventData) {
        charDetailsUI.OnEquippedItemPointerDown(playingChar, item, eventData);
    }

    #region IPointerDownHandler implementation

    public void OnPointerDown(PointerEventData eventData)
    {
        charDetailsUI.OnRightPanelPointerDown(playingChar, eventData);
    }

    #endregion

}
