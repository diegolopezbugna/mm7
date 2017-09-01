using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    public Item Item { get; set; }
    public ItemsContainerUI ItemsContainerUI { get; set; }

    private RawImage rawImage;

    [SerializeField]
    private Color highlightedColor = new Color(0.8f, 0.8f, 0.8f);

	// Use this for initialization
	void Start () {
        rawImage = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MakeImageTranslucent()
    {
        rawImage.raycastTarget = false;
        rawImage.CrossFadeAlpha(0.5f, 0.1f, true);
    }
        
    public void MakeImageSolid()
    {
        rawImage.raycastTarget = true;
        rawImage.CrossFadeAlpha(1f, 0.1f, true);
    }

    #region POINTER DOWN/UP

    public void OnPointerDown(PointerEventData eventData)
    {
        ItemsContainerUI.OnInventoryItemPointerDown(Item, eventData, this);
    }


    #endregion

    #region POINTER EXIT/ENTER

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rawImage.raycastTarget)
            rawImage.CrossFadeColor(highlightedColor, 0.1f, true, false);
    }
        
    public void OnPointerExit(PointerEventData eventData)
    {
        if (rawImage.raycastTarget)
            rawImage.CrossFadeColor(Color.white, 0.1f, true, false);
    }

    #endregion
}
