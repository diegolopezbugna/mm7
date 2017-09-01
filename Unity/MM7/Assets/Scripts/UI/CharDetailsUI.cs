using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;
using UnityStandardAssets.Characters.FirstPerson;

public class CharDetailsUI : BaseUI<CharDetailsUI> {

    [SerializeField]
    private GameObject stats;

    [SerializeField]
    private GameObject skills;

    [SerializeField]
    private InventoryUI[] inventories;

    [SerializeField]
    private EquippedItemsUI[] equippedItemsRightPanels;

    private RectTransform rectTransform;
    private Canvas canvas;

    public Item DraggingItem { get; private set; }
    public GameObject DraggingItemGameObject { get; private set; }
    public object DraggingFrom { get; private set; }

    public override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public override void Start()
    {
        base.Start();
        for (int i = 0; i < equippedItemsRightPanels.Length; i++)
            equippedItemsRightPanels[i].SetPlayingChar(Game.Instance.PartyStats.Chars[i]);
    }

    public override void Update()
    {
        base.Update();

        if (DraggingItem != null && DraggingItemGameObject != null)
            PositionDraggingItem();
    }

    public override void Hide()
    {
        if (DraggingItem != null && DraggingItemGameObject != null)
            DestroyDraggingItem();

        base.Hide();
    }

    #region INVENTORIES

    private void PositionDraggingItem()
    {
        Vector2 globalMousePos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, null, out globalMousePos))
        {
            var offsetX = GetOffsetForCenterItemInSlot(Game.Instance.PartyStats.Chars[0].Inventory.SlotWidth, DraggingItem.Texture.width);
            var offsetY = GetOffsetForCenterItemInSlot(Game.Instance.PartyStats.Chars[0].Inventory.SlotHeight, DraggingItem.Texture.height);
            DraggingItemGameObject.transform.localPosition = new Vector2(
                globalMousePos.x - Game.Instance.PartyStats.Chars[0].Inventory.SlotWidth/2 + offsetX, 
                globalMousePos.y + Game.Instance.PartyStats.Chars[0].Inventory.SlotHeight/2 - offsetY);
        }
    }

    public float GetOffsetForCenterItemInSlot(float slotSize, float itemSize)
    {
        var slotsNeeded = Game.Instance.PartyStats.Chars[0].Inventory.GetSlotsNeeded(slotSize, itemSize);
        var extraSpace = slotsNeeded * slotSize - itemSize;
        return extraSpace / 2;
    }

    public void OnInventoryPointerDown(Inventory inventory, int slotX, int slotY)
    {
        if (DraggingItem != null)
        {
            bool didMove = false;
            if (DraggingFrom is Inventory)
                didMove = TryMoveItem(DraggingItem, slotX, slotY, DraggingFrom as Inventory, inventory);
            else if (DraggingFrom is PlayingCharacter)
                didMove = TryUnequipItem(DraggingItem, DraggingFrom as PlayingCharacter, inventory, slotX, slotY);

            if (didMove)
                EndDrag();
        }
    }

    public void OnInventoryItemPointerDown(Inventory inventory, Item item, PointerEventData eventData, InventoryItem inventoryItem)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DraggingItem == null)
            {
                // begin drag
                DraggingItem = item;
                DraggingFrom = inventory;
                DraggingItemGameObject = new GameObject("draggingItem");
                DraggingItemGameObject.transform.SetParent(canvas.transform, false);
                DraggingItemGameObject.transform.SetAsLastSibling();
                var image = DraggingItemGameObject.AddComponent<RawImage>();
                image.texture = item.Texture;
                image.SetNativeSize();
                image.rectTransform.pivot = new Vector2(0, 1);
                image.raycastTarget = false;
                PositionDraggingItem();
                inventoryItem.MakeImageTranslucent();
            }
            else
            {
                // TODO: y si viene de PlayingChar??
                // exchange items
                if (DraggingFrom is Inventory &&
                    TryExchangeItems(DraggingFrom as Inventory, DraggingItem, inventory, item))
                    EndDrag();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ItemInfoUI.Instance.Show(item);
        }
    }

    // TODO: move to business use case
    private bool TryMoveItem(Item item, int x, int y, Inventory inventoryFrom, Inventory inventoryTo) 
    {
        var originalPosition = inventoryFrom.RemoveItem(item);
        if (originalPosition == null)
            return false;
        var canMoveIt = inventoryTo.TryInsertItemAt(item, x, y);
        if (!canMoveIt)
            inventoryFrom.TryInsertItemAt(item, originalPosition.x, originalPosition.y); // rollback
        return canMoveIt;
    }

    // TODO: move to business use case
    private bool TryExchangeItems(Inventory inventory1, Item item1, Inventory inventory2, Item item2)
    {
        var originalPosItem1 = inventory1.RemoveItem(item1);
        var originalPosItem2 = inventory2.RemoveItem(item2);
        if (inventory1.TryInsertItemAt(item2, originalPosItem1.x, originalPosItem1.y))
        {
            if (inventory2.TryInsertItemAt(item1, originalPosItem2.x, originalPosItem2.y))
                return true;
            else
                inventory1.RemoveItem(item2);
        }
        // rollback
        inventory1.TryInsertItemAt(item1, originalPosItem1.x, originalPosItem1.y);
        inventory2.TryInsertItemAt(item2, originalPosItem2.x, originalPosItem2.y);
        return false;
    }

    #endregion

    private void EndDrag()
    {
        DestroyDraggingItem();
        foreach (var i in inventories)
            i.DrawInventory(); // TODO: don't redraw all
        foreach (var e in equippedItemsRightPanels) {
            e.DrawBody(); // TODO: don't redraw all
            e.DrawEqquipedItems();
        }
    }

    private void DestroyDraggingItem()
    {
        DraggingItem = null;
        Destroy(DraggingItemGameObject);
        DraggingItemGameObject = null;
        DraggingFrom = null;
    }

    #region EQUIPPED ITEMS

    public void OnRightPanelPointerDown(PlayingCharacter playingCharacter, PointerEventData eventData)
    {
        if (DraggingItem != null)
        {
            bool didMove = false;
            Item oldEquippedItem = null;
            if (DraggingFrom is Inventory)
                didMove = TryEquipItem(playingCharacter, DraggingItem, DraggingFrom as Inventory, out oldEquippedItem);
//            else if (DraggingFrom is PlayingCharacter)
//                didMove = TryUnequipItem(DraggingItem, DraggingFrom as PlayingCharacter, inventory, slotX, slotY);
            // TODO: move from another equipped player

            if (didMove)
            {
                EndDrag();
                if (oldEquippedItem != null)
                    BeginEquippedItemDrag(playingCharacter, oldEquippedItem, null);
            }
        }
    }

    public void OnEquippedItemPointerDown(PlayingCharacter playingCharacter, Item item, PointerEventData eventData, InventoryItem equippedItem)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DraggingItem == null)
            {
                // begin drag
                BeginEquippedItemDrag(playingCharacter, item, equippedItem);
            }
            else
            {
                // TODO: y si ya tenemos uno?
                // TODO: y si viene de otro player?
                Item oldEquippedItem = null;
                if (DraggingFrom is Inventory &&
                    TryEquipItem(playingCharacter, DraggingItem, DraggingFrom as Inventory, out oldEquippedItem))
                {
                    EndDrag();
                    if (oldEquippedItem != null)
                        BeginEquippedItemDrag(playingCharacter, oldEquippedItem, null);
                }

//                if (DraggingFrom is Inventory)
//                    TryEquipItem(DraggingItem, 
//                else if (DraggingFrom is PlayingCharacter)
                    
                // exchange items
//                if (TryExchangeItems(DraggingFrom, DraggingItem, inventory, item))
//                    EndDrag();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ItemInfoUI.Instance.Show(item);
        }
    }

    private void BeginEquippedItemDrag(PlayingCharacter playingCharacter, Item item, InventoryItem equippedItem) {
        DraggingItem = item;
        DraggingFrom = playingCharacter;
        DraggingItemGameObject = new GameObject("draggingItem");
        DraggingItemGameObject.transform.SetParent(canvas.transform, false);
        DraggingItemGameObject.transform.SetAsLastSibling();
        var image = DraggingItemGameObject.AddComponent<RawImage>();
        image.texture = item.Texture;
        image.SetNativeSize();
        image.rectTransform.pivot = new Vector2(0, 1);
        image.raycastTarget = false;
        PositionDraggingItem();
        if (equippedItem != null)
            equippedItem.MakeImageTranslucent();
    }

    // TODO: move to business use case
    private bool TryEquipItem(PlayingCharacter playingCharacter, Item item, Inventory inventoryFrom, out Item oldEquipItem) 
    {
        oldEquipItem = null;
        var canEquip = playingCharacter.CanEquipItem(item);
        if (canEquip)
        {
            oldEquipItem = playingCharacter.EquipItem(item);
            inventoryFrom.RemoveItem(item);
        }
        return canEquip;
    }

    // TODO: move to business use case
    private bool TryUnequipItem(Item item, PlayingCharacter playingCharacterFrom, Inventory inventoryTo, int slotX, int slotY) 
    {
        var canMoveIt = inventoryTo.TryInsertItemAt(item, slotX, slotY);
        if (canMoveIt)
            playingCharacterFrom.UnequipItem(item);
        return canMoveIt;
    }

    #endregion

    #region SHOW PANELS

    public void ShowStats(PlayingCharacter character) {
        if (!IsShowing)
            Show();
        stats.SetActive(true);
        skills.SetActive(false);
        foreach (var i in inventories)
            i.gameObject.SetActive(false);
    }

    public void ShowSkills(PlayingCharacter character) {
        if (!IsShowing)
            Show();
        stats.SetActive(false);
        skills.SetActive(true);
        foreach (var i in inventories)
            i.gameObject.SetActive(false);
    }

    public void ShowInventory() {
        if (!IsShowing)
            Show();
        stats.SetActive(false);
        skills.SetActive(false);
        for (int i = 0; i < inventories.Length; i++)
        {
            inventories[i].gameObject.SetActive(true);
            inventories[i].Inventory = Game.Instance.PartyStats.Chars[i].Inventory;
            inventories[i].DrawInventory();
        }
    }

    #endregion

}
