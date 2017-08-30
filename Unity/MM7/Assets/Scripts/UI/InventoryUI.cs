using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class InventoryUI : MonoBehaviour, IPointerDownHandler {

    [SerializeField]
    private GameObject itemTemplatePrefab;

    public Item DraggingItem { get; private set; }
    private GameObject DraggingItemGameObject { get; set; }

    private RectTransform rectTransform;
    private int totalSlotsH;
    private int totalSlotsV;
    private float slotWidth;
    private float slotHeight;
    private Canvas canvas;

    private Inventory Inventory { get; set; }

	void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
	}
	
	void Update() {
        if (DraggingItem != null && DraggingItemGameObject != null)
            PositionDraggingItem();
	}

    public void DrawInventory(Inventory inventory) {
        Inventory = inventory;
        totalSlotsH = inventory.GetTotalSlotsH();
        totalSlotsV = inventory.GetTotalSlotsV();
        slotWidth = inventory.SlotWidth;
        slotHeight = inventory.SlotHeight;

        CleanInventory();
        var alreadyDrawnItems = new List<Item>();

        // maybe it's more efficient to have the inventory as a list of (item, positions)
        for (int i = 0; i < totalSlotsH; i++)
        {
            for (int j = 0; j < totalSlotsV; j++)
            {
                var item = inventory.GetItemAt(i, j);
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
        var offsetX = GetOffsetForCenterItemInSlot(slotWidth, rawImage.texture.width);
        var offsetY = GetOffsetForCenterItemInSlot(slotHeight, rawImage.texture.height);
        rawImage.rectTransform.anchoredPosition = new Vector2((x * slotWidth) + offsetX, -((y * slotHeight) + offsetY));
        itemGameObject.GetComponent<InventoryItem>().Item = item;
    }

    private float GetOffsetForCenterItemInSlot(float slotSize, float itemSize)
    {
        var slotsNeeded = Inventory.GetSlotsNeeded(slotSize, itemSize);
        var extraSpace = slotsNeeded * slotSize - itemSize;
        return extraSpace / 2;
    }

    private void PositionDraggingItem()
    {
        Vector2 globalMousePos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, null, out globalMousePos))
        {
            var offsetX = GetOffsetForCenterItemInSlot(slotWidth, DraggingItem.Texture.width);
            var offsetY = GetOffsetForCenterItemInSlot(slotHeight, DraggingItem.Texture.height);
            DraggingItemGameObject.transform.localPosition = new Vector2(globalMousePos.x - slotWidth/2 + offsetX, globalMousePos.y + slotHeight/2 - offsetY);
        }
    }

    #region POINTER DOWN/UP

    public void OnInventoryItemPointerDown(Item item, PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DraggingItem == null)
            {
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
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ItemInfoUI.Instance.Show(item);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPointerDownPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPointerDownPosition); 
        var x = localPointerDownPosition.x;
        var y = -localPointerDownPosition.y;
        var slotX = Mathf.FloorToInt(x / slotWidth);
        var slotY = Mathf.FloorToInt(y / slotHeight);

        if (slotX < 0 || slotX >= totalSlotsH || slotY < 0 || slotY >= totalSlotsV)
            return;

        Debug.LogFormat("X: {0} - Y: {1} -> SLOT ({2}, {3})", x, y, slotX, slotY);

        if (DraggingItem != null)
        {
            if (Inventory.TryMoveItem(DraggingItem, slotX, slotY))
            {
                DraggingItem = null;
                Destroy(DraggingItemGameObject);
                DraggingItemGameObject = null;
                DrawInventory(Inventory); // TODO: don't redraw all
            }
        }
    }

    #endregion

}
