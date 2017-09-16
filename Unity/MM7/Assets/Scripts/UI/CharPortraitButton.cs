using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Business;

public delegate void CharPortraitButtonDelegate();

public class CharPortraitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public CharPortraitButtonDelegate OnCharPortraitButtonLeftUp { get; set; }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && OnCharPortraitButtonLeftUp != null)
            OnCharPortraitButtonLeftUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
