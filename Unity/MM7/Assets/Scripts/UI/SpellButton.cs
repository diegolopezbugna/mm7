using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Business;
using Infrastructure;

public delegate void SpellButtonDelegate(SpellInfo spellInfo);

namespace Business
{
    public class SpellButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private RawImage image;
        
        private SpellInfo _spellInfo;
        public SpellInfo SpellInfo { 
            get { return _spellInfo; }
            set {
                _spellInfo = value;

                image = gameObject.AddComponent<RawImage>();
                image.texture = _spellInfo.TextureOff;
                image.SetNativeSize();
                image.rectTransform.pivot = Vector2.up;
                image.rectTransform.anchorMin = Vector2.up;
                image.rectTransform.anchorMax = Vector2.up;
                image.rectTransform.anchoredPosition = new Vector2(_spellInfo.SpellBookPosX, -_spellInfo.SpellBookPosY);
            }
        }

        public SpellButtonDelegate OnSpellButtonLeftClick { get; set; }
        public SpellButtonDelegate OnSpellButtonRightClick { get; set; }
        public SpellButtonDelegate OnSpellButtonPointerEnter { get; set; }
        public SpellButtonDelegate OnSpellButtonPointerExit { get; set; }

        public SpellButton()
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && OnSpellButtonLeftClick != null)
                OnSpellButtonLeftClick(SpellInfo);
            if (eventData.button == PointerEventData.InputButton.Right && OnSpellButtonRightClick != null)
                OnSpellButtonRightClick(SpellInfo);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            image.texture = SpellInfo.TextureOn;
            
            if (OnSpellButtonPointerEnter != null)
                OnSpellButtonPointerEnter(SpellInfo);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.texture = SpellInfo.TextureOff;

            if (OnSpellButtonPointerExit != null)
                OnSpellButtonPointerExit(SpellInfo);
        }
    }
}

