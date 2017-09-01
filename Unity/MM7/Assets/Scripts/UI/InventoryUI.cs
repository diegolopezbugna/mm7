using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class InventoryUI : MonoBehaviour, ItemsContainerUI, IPointerDownHandler {

    [SerializeField]
    private GameObject itemTemplatePrefab;

    public Inventory Inventory { get; set; }

    private RectTransform rectTransform;
    private CharDetailsUI charDetailsUI; 

	void Awake() {
        rectTransform = GetComponent<RectTransform>();
        charDetailsUI = GetComponentInParent<CharDetailsUI>();
	}
	
	void Update() {
	}

    public void DrawInventory() {
        CleanInventory();
        var alreadyDrawnItems = new List<Item>();

        // maybe it's more efficient to have the inventory as a list of (item, positions)
        for (int i = 0; i < Inventory.GetTotalSlotsH(); i++)
        {
            for (int j = 0; j < Inventory.GetTotalSlotsV(); j++)
            {
                var item = Inventory.GetItemAt(i, j);
                if (item != null && !alreadyDrawnItems.Contains(item))
                {
                    DrawItem(item, i, j);
                    alreadyDrawnItems.Add(item);
                }
            }
        }
    }

    private void CleanInventory()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    private void DrawItem(Item item, int x, int y) 
    {
        var itemGameObject = Instantiate(itemTemplatePrefab, transform);
        var rawImage = itemGameObject.GetComponent<RawImage>();
        rawImage.texture = item.Texture;
        rawImage.SetNativeSize();
        var offsetX = charDetailsUI.GetOffsetForCenterItemInSlot(Inventory.SlotWidth, rawImage.texture.width);
        var offsetY = charDetailsUI.GetOffsetForCenterItemInSlot(Inventory.SlotHeight, rawImage.texture.height);
        rawImage.rectTransform.anchoredPosition = new Vector2((x * Inventory.SlotWidth) + offsetX, -((y * Inventory.SlotHeight) + offsetY));
        var inventoryItem = itemGameObject.GetComponent<InventoryItem>();
        inventoryItem.Item = item;
        inventoryItem.ItemsContainerUI = this;
    }

    #region POINTER DOWN/UP

    public void OnInventoryItemPointerDown(Item item, PointerEventData eventData, InventoryItem inventoryItem)
    {
        charDetailsUI.OnInventoryItemPointerDown(Inventory, item, eventData, inventoryItem);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPointerDownPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPointerDownPosition); 
        var x = localPointerDownPosition.x;
        var y = -localPointerDownPosition.y;
        var slotX = Mathf.FloorToInt(x / Inventory.SlotWidth);
        var slotY = Mathf.FloorToInt(y / Inventory.SlotHeight);

        if (slotX < 0 || slotX >= Inventory.GetTotalSlotsH() || slotY < 0 || slotY >= Inventory.GetTotalSlotsV())
            return;

        charDetailsUI.OnInventoryPointerDown(Inventory, slotX, slotY);
    }

    #endregion

}
