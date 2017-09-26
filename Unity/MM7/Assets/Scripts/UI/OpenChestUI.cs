using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Business;
using Infrastructure;

public class OpenChestUI : BaseUI<OpenChestUI> {

    [SerializeField]
    private ItemInfoUI itemInfoUI;

    private InventoryUI inventoryUI;
    private List<Item> items;

    public void Show(List<Item> items)
    {
        base.Show();

        this.items = items;

        inventoryUI = GetComponentInChildren<InventoryUI>();
        inventoryUI.OnInventoryItemPointerDown = OnItemPointerDown;

        var inventory = new Inventory(9, 8, 30, 30);

        foreach (var item in items)
            inventory.TryInsertItem(item);

        inventoryUI.Inventory = inventory;
        inventoryUI.DrawInventory();
    }

    private void OnItemPointerDown(Inventory inventory, Item item, PointerEventData eventData) 
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item.EquipSlot == EquipSlot.gold)
            {
                Game.Instance.PartyStats.Gold += item.Value;
                Party.Instance.RefreshGoldAndFood();
                Party.Instance.PlayGoldSound();
                Party.Instance.ShowMessage(Localization.Instance.Get("YouFoundXGold", item.Value));
                inventory.RemoveItem(item);
                items.Remove(item);
                inventoryUI.DrawInventory();
            }
            else
            {
                if (Party.Instance.GetPlayingCharacterSelectedOrDefault().Inventory.TryInsertItem(item))
                {
                    inventory.RemoveItem(item);
                    items.Remove(item);
                    inventoryUI.DrawInventory();
                }
                else
                {
                    Party.Instance.ShowMessage(Localization.Instance.Get("YourPacksAreFull"));
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            itemInfoUI.Show(item);
        }
    }



}
