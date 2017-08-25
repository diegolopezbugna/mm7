using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    private InventoryUI inventoryUI;

    public Item Item { get; set; }

    private RawImage rawImage;

	// Use this for initialization
	void Start () {
        rawImage = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region IPointerClickHandler implementation

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("item click: " + Item.Name);
    }

    #endregion

    #region IPointerDownHandler implementation

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("item down: " + Item.Name);
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
