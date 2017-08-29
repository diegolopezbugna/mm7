using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

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
        
    #region IPointerDownHandler implementation

    public void OnPointerDown(PointerEventData eventData)
    {
        inventoryUI.OnItemSlotPointerDown(Item, eventData);
    }

    #endregion

    #region IPointerUpHandler implementation

    public void OnPointerUp(PointerEventData eventData)
    {
        inventoryUI.OnItemSlotPointerUp(Item, eventData);
    }

    #endregion

    #region IPointerEnterHandler implementation

    public void OnPointerEnter(PointerEventData eventData)
    {
        rawImage.color = Color.gray;
    }

    #endregion

    #region IPointerExitHandler implementation

    public void OnPointerExit(PointerEventData eventData)
    {
        rawImage.color = Color.white;
    }

    #endregion
}
