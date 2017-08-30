using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    public Item Item { get; set; }

    private RawImage rawImage;
    private InventoryUI inventoryUI;

	// Use this for initialization
	void Start () {
        rawImage = GetComponent<RawImage>();
        inventoryUI = GetComponentInParent<InventoryUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
        
    #region POINTER DOWN/UP

    public void OnPointerDown(PointerEventData eventData)
    {
        inventoryUI.OnInventoryItemPointerDown(Item, eventData);
    }


    #endregion

    #region POINTER EXIT/ENTER

    public void OnPointerEnter(PointerEventData eventData)
    {
        rawImage.color = Color.gray;
    }
        
    public void OnPointerExit(PointerEventData eventData)
    {
        rawImage.color = Color.white;
    }

    #endregion
}
