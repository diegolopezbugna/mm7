using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using Infrastructure;

public class ItemInfoUI : MonoBehaviour {

    [SerializeField]
    private HorizontalLayoutGroup mainHorizontalLayoutGroup;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private RawImage itemImage;

    [SerializeField]
    private RectTransform rightInfo;

    [SerializeField]
    private Text typeText;

    [SerializeField]
    private Text modificatorsText;

    [SerializeField]
    private Text specialBonusText;

    [SerializeField]
    private Text descriptionText;

    [SerializeField]
    private Text valueText;

    private RectTransform rectTransform;
    //private CanvasGroup canvasGroup;

	// Use this for initialization
	void Awake () {
        rectTransform = GetComponent<RectTransform>();
        //canvasGroup = GetComponent<CanvasGroup>();
	}
	
    // Use this for initialization
    void Start () {
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(1))
        {
            Hide();
        }
	}

    public void Show(Item item)
    {
        // TODO: don't show data of unidentified items
        gameObject.SetActive(true);
        //StartCoroutine(canvasGroup.Fade(0f, 1f, 0.2f));

        titleText.text = item.Name;
        typeText.text = string.Format("{0}: {1}", Localization.Instance.Get("type"), item.NotIdentifiedName);
        ShowHideText(modificatorsText, GetModificatorsText(item));
        ShowHideText(specialBonusText, GetSpecialBonusText(item));
        descriptionText.text = item.Description;
        valueText.text = string.Format("{0}: {1}", Localization.Instance.Get("value"), item.Value);
        itemImage.rectTransform.sizeDelta = new Vector2(item.Texture.width, item.Texture.height);
        itemImage.texture = item.Texture;
        RedimensionPanel();
    }

    public void Hide()
    {
//        StartCoroutine(canvasGroup.Fade(1f, 0f, 0.2f, () => {
//            gameObject.SetActive(false);
//        }));
        gameObject.SetActive(false);
    }

    private void ShowHideText(Text textObject, string textToShow) {
        if (!string.IsNullOrEmpty(textToShow))
        {
            textObject.gameObject.SetActive(true);
            textObject.text = textToShow;
        }
        else
        {
            textObject.gameObject.SetActive(false);
        }
    }

    private string GetModificatorsText(Item item)
    {
        if (item.IsHandToHandWeapon)
            return string.Format("{0}: +{1}  {2}: {3}", Localization.Instance.Get("attack"), item.GetAttackBonus(), Localization.Instance.Get("damage"), item.Mod1);
        else if (item.IsLongRangeWeapon)
            return string.Format("{0}: +{1}  {2}: {3}", Localization.Instance.Get("shoot"), item.GetShootBonus(), Localization.Instance.Get("damage"), item.Mod1);
        else if (item.IsWandWeapon)
            return string.Format("{0}: {1}", Localization.Instance.Get("charges"), item.GetChargesLeft());
        else if (item.EquipSlot == EquipSlot.Reagent || item.EquipSlot == EquipSlot.Bottle)
            return string.Format("{0}: {1}", Localization.Instance.Get("power"), 0);  //TODO: alchemy potions
        else if (item.IsArmor)
            return string.Format("{0}: {1}", Localization.Instance.Get("armor"), item.GetArmorBonus());
        return null;
    }

    private string GetSpecialBonusText(Item item)
    {
        return null; // TODO: special item bonus
    }

    private void RedimensionPanel()
    {
        var pictureHeight = itemImage.texture.height;
        LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionText.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rightInfo);
        var rightInfoHeight = rightInfo.sizeDelta.y;
        var maxHeight = Mathf.Max(pictureHeight, rightInfoHeight);

        var newPanelHeight = maxHeight + mainHorizontalLayoutGroup.padding.top + mainHorizontalLayoutGroup.padding.bottom;
        var newPanelWidth = mainHorizontalLayoutGroup.padding.left + itemImage.texture.width + mainHorizontalLayoutGroup.spacing + rightInfo.sizeDelta.x + mainHorizontalLayoutGroup.padding.right;
        rectTransform.sizeDelta = new Vector2(newPanelWidth, newPanelHeight);
    }
}
