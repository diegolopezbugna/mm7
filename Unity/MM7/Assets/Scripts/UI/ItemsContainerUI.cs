using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Business;

public interface ItemsContainerUI
{
    void OnInventoryItemPointerDown(Item item, PointerEventData eventData, InventoryItem inventoryItem);
}

