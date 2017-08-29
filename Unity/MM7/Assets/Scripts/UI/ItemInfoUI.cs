using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Business;
using Infrastructure;

public class ItemInfoUI : Singleton<ItemInfoUI> {

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
        typeText.text = Localization.Instance.Get("Type: {0}", item.SkillGroup); // TODO: localization
        modificatorsText.text = GetModificatorsText(item);
        descriptionText.text = item.Description;
        valueText.text = Localization.Instance.Get("Value: {0}", item.Value); // TODO: localization
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

    private string GetModificatorsText(Item item)
    {
        return "Attack: +" + item.Mod2 + "  Damage: " + item.Mod1; // TODO: modificators 
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
