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
    private StatsUI[] stats;

    [SerializeField]
    private SkillsUI[] skills;

    [SerializeField]
    private InventoryUI[] inventories;

    [SerializeField]
    private EquippedItemsUI[] equippedItemsRightPanels;

    [SerializeField]
    private ItemInfoUI itemInfoUI;

    private RectTransform rectTransform;
    private Canvas canvas;

    public Item DraggingItem { get; private set; }
    public GameObject DraggingItemGameObject { get; private set; }

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
        {
            equippedItemsRightPanels[i].SetPlayingChar(Game.Instance.PartyStats.Chars[i]);
            inventories[i].OnInventoryItemPointerDown = OnInventoryItemPointerDown;
            inventories[i].OnInventorySlotPointerDown = OnInventorySlotPointerDown;
        }
    }

    public override void Update()
    {
        base.Update();

        if (DraggingItem != null && DraggingItemGameObject != null)
            PositionDraggingItem();
    }

    private void BeginDrag(Item item) {
        DraggingItem = item;
        DraggingItemGameObject = new GameObject("draggingItem");
        DraggingItemGameObject.transform.SetParent(canvas.transform, false);
        DraggingItemGameObject.transform.SetAsLastSibling();
        var image = DraggingItemGameObject.AddComponent<RawImage>();
        image.texture = item.Texture;
        image.SetNativeSize();
        image.rectTransform.pivot = new Vector2(0, 1);
        image.raycastTarget = false;
        PositionDraggingItem();
    }

    private void EndDragAndRefreshUI()
    {
        DestroyDraggingItem();
        RefreshUI();
    }

    private void RefreshUI() 
    {
        // TODO: dont redraw all
        if (stats[0].gameObject.activeSelf) {
            ShowStats();
        }

        // TODO: dont redraw all
        if (skills[0].gameObject.activeSelf) {
            ShowSkills();
        }

        // TODO: dont redraw all
        if (inventories[0].gameObject.activeSelf) {
            ShowInventory();
        }

        foreach (var e in equippedItemsRightPanels) {
            e.Draw(); // TODO: don't redraw all
        }
    }

    private void DestroyDraggingItem()
    {
        DraggingItem = null;
        Destroy(DraggingItemGameObject);
        DraggingItemGameObject = null;
    }

    private void PositionDraggingItem()
    {
        Vector2 globalMousePos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, null, out globalMousePos))
        {
            var offsetX = InventoryUI.GetOffsetForCenterItemInSlot(Game.Instance.PartyStats.Chars[0].Inventory.SlotWidth, DraggingItem.Texture.width);
            var offsetY = InventoryUI.GetOffsetForCenterItemInSlot(Game.Instance.PartyStats.Chars[0].Inventory.SlotHeight, DraggingItem.Texture.height);
            DraggingItemGameObject.transform.localPosition = new Vector2(
                globalMousePos.x - Game.Instance.PartyStats.Chars[0].Inventory.SlotWidth/2 + offsetX, 
                globalMousePos.y + Game.Instance.PartyStats.Chars[0].Inventory.SlotHeight/2 - offsetY);
        }
    }

    public void OnInventorySlotPointerDown(Inventory inventory, int slotX, int slotY)
    {
        if (DraggingItem != null)
        {
            if (inventory.TryInsertItemAt(DraggingItem, slotX, slotY))
                EndDragAndRefreshUI();
        }
    }

    public void OnInventoryItemPointerDown(Inventory inventory, Item item, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DraggingItem == null)
            {
                inventory.RemoveItem(item);
                RefreshUI();
                BeginDrag(item);
            }
            else
            {
                var pos = inventory.RemoveItem(item);
                if (inventory.TryInsertItemAt(DraggingItem, pos.x, pos.y))
                {
                    EndDragAndRefreshUI();
                    BeginDrag(item);
                }
                else
                {
                    inventory.TryInsertItemAt(item, pos.x, pos.y); // rollback
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            itemInfoUI.Show(item);
        }
    }

    public void OnRightPanelPointerDown(PlayingCharacter playingCharacter, PointerEventData eventData)
    {
        if (DraggingItem != null)
        {
            TryEquipDraggingItem(playingCharacter);
        }
    }

    public void OnEquippedItemPointerDown(PlayingCharacter playingCharacter, Item item, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DraggingItem != null)
            {
                TryEquipDraggingItem(playingCharacter);
            }
            else
            {
                playingCharacter.UnequipItem(item);
                RefreshUI();
                BeginDrag(item);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            itemInfoUI.Show(item);
        }
    }

    private void TryEquipDraggingItem(PlayingCharacter playingCharacter) 
    {
        if (playingCharacter.CanEquipItem(DraggingItem))
        {
            Item oldEquippedItem = null;
            if (playingCharacter.TryEquipItem(DraggingItem, out oldEquippedItem))
            {
                EndDragAndRefreshUI();
                if (oldEquippedItem != null)
                    BeginDrag(oldEquippedItem);
            }
        }
        else
        {
            // TODO: alert user, sound?
        }
    }

    #region SHOW PANELS

    public void ShowStats() {
        if (!IsShowing)
            Show();

        for (int i = 0; i < Game.Instance.PartyStats.Chars.Count; i++)
        {
            stats[i].gameObject.SetActive(true);
            stats[i].ShowStats(Game.Instance.PartyStats.Chars[i]);
            skills[i].gameObject.SetActive(false);
            inventories[i].gameObject.SetActive(false);
        }
    }

    public void ShowSkills() {
        if (!IsShowing)
            Show();

        for (int i = 0; i < Game.Instance.PartyStats.Chars.Count; i++)
        {
            stats[i].gameObject.SetActive(false);
            skills[i].gameObject.SetActive(true);
            skills[i].ShowSkills(Game.Instance.PartyStats.Chars[i]);
            inventories[i].gameObject.SetActive(false);
        }
    }

    public void ShowInventory() {
        if (!IsShowing)
            Show();

        for (int i = 0; i < Game.Instance.PartyStats.Chars.Count; i++)
        {
            stats[i].gameObject.SetActive(false);
            skills[i].gameObject.SetActive(false);
            inventories[i].gameObject.SetActive(true);
            inventories[i].Inventory = Game.Instance.PartyStats.Chars[i].Inventory;
            inventories[i].DrawInventory();
        }
    }

    #endregion

}
