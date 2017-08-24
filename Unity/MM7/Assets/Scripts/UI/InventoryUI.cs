using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Business;

public class InventoryUI : MonoBehaviour, IPointerDownHandler {

    private RectTransform rectTransform;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPointerDownPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPointerDownPosition); 
        var marginLeft = 4;
        var marginTop = 7;
        var totalSlotsV = 9;
        var totalSlotsH = 14;
        var slotWidth = 450f / totalSlotsH;
        var slotHeight = 292f / totalSlotsV;
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


}
