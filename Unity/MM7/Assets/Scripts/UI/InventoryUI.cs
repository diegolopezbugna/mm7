using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class InventoryUI : MonoBehaviour, IPointerDownHandler {

    [SerializeField]
    private GameObject itemTemplatePrefab;

    private RectTransform rectTransform;
    private int totalSlotsH;
    private int totalSlotsV;
    private float slotWidth;
    private float slotHeight;
    private float marginLeft = 7f;
    private float marginTop = 7f;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPointerDownPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPointerDownPosition); 
        var x = localPointerDownPosition.x;
        var y = -localPointerDownPosition.y;
        var slotX = Mathf.FloorToInt((x - marginLeft) / slotWidth);
        var slotY = Mathf.FloorToInt((y - marginTop) / slotHeight);

        if (slotX < 0 || slotX >= totalSlotsH || slotY < 0 || slotY >= totalSlotsV)
            return;

        Debug.LogFormat("X: {0} - Y: {1} -> SLOT ({2}, {3})", x, y, slotX, slotY);
    }

	// Use this for initialization
	void Start() {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update() {
	}

    public void DrawInventory(Inventory inventory) {
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
        var texture = Resources.Load("Items/" + item.PictureFilename) as Texture;
        var rawImage = itemGameObject.GetComponent<RawImage>();
        rawImage.rectTransform.anchoredPosition = new Vector2(marginLeft + x * slotWidth, -(marginTop + 2f + y * slotHeight)); // TODO: center items???
        rawImage.rectTransform.sizeDelta = new Vector2(texture.width, texture.height);
        rawImage.texture = texture;
    }
}
