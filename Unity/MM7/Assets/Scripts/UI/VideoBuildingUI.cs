using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using Business;
using Infrastructure;

public class VideoBuildingUI : BaseUI<VideoBuildingUI>, BuySellItemViewInterface {

    [SerializeField]
    private RawImage videoImage;

    [SerializeField]
    private Text dialogText;

    [SerializeField]
    private Text buildingNameText;

    [SerializeField]
    private GameObject portraitsContainer;

    [SerializeField]
    private GameObject portraitTopicsContainer;

    [SerializeField]
    private RawImage portraitTopicsPortraitImage;

    [SerializeField]
    private Text portraitTopicsPortraitText;

    [SerializeField]
    private GameObject topicsContainer;

    [SerializeField]
    private Texture textureWeaponShop;

    [SerializeField]
    private Texture textureArmorShop;

    [SerializeField]
    private Texture textureMagicShop;

    [SerializeField]
    private GameObject itemTemplatePrefab;

    [SerializeField]
    private ItemInfoUI itemInfoUI;

    [SerializeField]
    private GameObject inventoryPrefab;

    //public Building Building { get; set; }
    //public List<Npc> Npcs { get; set; }

    private Dictionary<string, int> CurrentGreeting = new Dictionary<string, int>();
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private Canvas canvas;

    public override void Awake() {
        base.Awake();
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        canvas = GetComponent<Canvas>();
    }

	public override void Update () {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            canvas.enabled = false;
        }
	}

    public void Show(Building building, List<Npc> npcs) {
//        Building = building;
//        Npcs = npcs;
        base.Show();
        StartCoroutine(PlayVideo("Assets/Resources/Videos/" + building.VideoFilename + ".mp4"));
        buildingNameText.text = building.Name;
        var npc = npcs[0]; // TODO: more than 1
        dialogText.text = npc.NextGreeting();
        portraitTopicsPortraitImage.texture = Resources.Load(string.Format("NPC Pictures/NPC{0:D3}", npc.PictureCode)) as Texture;
        portraitTopicsPortraitText.text = npc.Name;
        ShowTopics(npc);
    }

    public override void Hide()
    {
        foreach (Transform child in videoImage.transform)
            Destroy(child.gameObject);
        base.Hide();
    }

    private void OnVideoPrepared() {
        canvas.enabled = true;
    }

    private void ShowTopics(Npc npc)
    {
        // remove all but first
        var texts = topicsContainer.GetComponentsInChildren<Text>();
        for (int i = 1; i < texts.Length; i++)
        {
            texts[i].GetComponent<TopicData>().Npc = null;   // destroy is not inmediate
            texts[i].GetComponent<TopicData>().NpcTopic = null;   // destroy is not inmediate
            Destroy(texts[i].gameObject);
        }

        var topicText = topicsContainer.GetComponentInChildren<Text>();
        for (int i = 0; i < npc.Topics.Count; i++)
        {
            if (i > 0)
                topicText = Instantiate(topicText, topicText.transform.parent);

            topicText.text = npc.Topics[i].Title;
            topicText.GetComponent<TopicData>().Npc = npc;
            topicText.GetComponent<TopicData>().NpcTopic = npc.Topics[i];
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)topicsContainer.transform);
    }

    public void OnTopicClicked(Npc npc, NpcTopic npcTopic) 
    {
        foreach (Transform child in videoImage.transform)
            Destroy(child.gameObject);
        var centeredBottomPivot = new Vector2(0.5f, 0f);
        var centeredTopPivot = new Vector2(0.5f, 1f);

        if (npcTopic.ShopActionType != ShopActionType.None && npc.Shop != null)
        {
            if (npc.Shop.ShopType == ShopType.WeaponSmith)
                ShowShop(npcTopic.ShopActionType, npc.Shop, textureWeaponShop, (List<Item> items) => {
                    for (int i = 0; i < items.Count; i++)
                        DrawItem(items[i], new Vector2(0, 0.5f), new Vector2(30f + i * ((textureWeaponShop.width - 30f) / (items.Count)), 0f));
                });
            else if (npc.Shop.ShopType == ShopType.Armory)
                ShowShop(npcTopic.ShopActionType, npc.Shop, textureArmorShop, (List<Item> items) => {
                    DrawItem(items[0], centeredBottomPivot, new Vector2(-145f, 255f)); 
                    DrawItem(items[1], centeredBottomPivot, new Vector2(-42f, 255f)); 
                    DrawItem(items[2], centeredBottomPivot, new Vector2(62f, 255f)); 
                    DrawItem(items[3], centeredBottomPivot, new Vector2(163f, 255f)); 
                    DrawItem(items[4], centeredTopPivot, new Vector2(-145f, -120f)); 
                    DrawItem(items[5], centeredTopPivot, new Vector2(-42f, -120f)); 
                    DrawItem(items[6], centeredTopPivot, new Vector2(62f, -120f)); 
                    DrawItem(items[7], centeredTopPivot, new Vector2(163f, -120f)); 
                });
            else
                ShowShop(npcTopic.ShopActionType, npc.Shop, textureMagicShop, (List<Item> items) => {
                    for (int i = 0; i < 6; i++)
                        DrawItem(items[i], centeredBottomPivot, new Vector2(-170f + i * 70, 199f)); 
                    for (int i = 6; i < 12; i++)
                        DrawItem(items[i], centeredBottomPivot, new Vector2(-170f + (i-6) * 70, 46f)); 
                });
        }
        else
        {
            dialogText.text = npcTopic.Description;
        }
    }
        
    IEnumerator PlayVideo(string url)
    {
        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        videoPlayer.url = url;
        videoPlayer.isLooping = false;

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        //videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        //Wait until video is prepared
        var waitTime = new WaitForSeconds(0.01f);
        while (!videoPlayer.isPrepared)
            yield return waitTime;
        yield return waitTime;

        //Assign the Texture from Video to RawImage to be displayed
        videoImage.texture = videoPlayer.texture;

        Debug.Log("Done Preparing Video");
        OnVideoPrepared();

        //Play Video & sound
        videoPlayer.Play();
        audioSource.Play();
    }

    // TODO: move all the shopping functionality to another class
    // TODO: qué significan los valores de A y C???
    private void ShowShop(ShopActionType shopActionType, Shop shop, Texture bgTexture, Action<List<Item>> drawItems) 
    {
        if (shopActionType == ShopActionType.BuyStandard || shopActionType == ShopActionType.BuySpecial)
        {
            videoImage.texture = bgTexture;
            // TODO: don't refresh items each time you enter shop
            var items = Item.GetItemsToBuyAt(shop.ShopType, 
                            shopActionType == ShopActionType.BuySpecial ? shop.TreasureLevelSpecial : shop.TreasureLevelStandard);
            
            drawItems(items);
        }
        else if (shopActionType == ShopActionType.Sell)
        {
            var inventoryGameObject = Instantiate(inventoryPrefab, videoImage.transform);
            inventoryGameObject.SetActive(true);
            ((RectTransform)inventoryGameObject.transform).anchoredPosition = new Vector2(-227f, 147f);
            var inventoryUI = inventoryGameObject.GetComponent<InventoryUI>();
            inventoryUI.OnInventoryItemPointerDown = OnSellingItemFromInventoryPointerDown;
            inventoryUI.OnInventoryItemPointerEnter = OnSellingItemFromInventoryPointerEnter;
            inventoryUI.OnInventoryItemPointerExit = OnSellingItemFromInventoryPointerExit;
            inventoryUI.Inventory = Game.Instance.PartyStats.Chars[0].Inventory; // TODO: selected char
            inventoryUI.DrawInventory();
        }
        else if (shopActionType == ShopActionType.BuySpells)
        {
            videoImage.texture = bgTexture;

            // TODO: don't refresh items each time you enter shop
            var items = Item.GetBooksToBuyAt(shop.ShopType, shop.GuildLevel);

            drawItems(items);
        }
    }

    private void DrawItem(Item item, Vector2 anchorPivot, Vector2 anchoredPosition)
    {
        var itemGameObject = Instantiate(itemTemplatePrefab, videoImage.transform);
        var rawImage = itemGameObject.GetComponent<RawImage>();
        rawImage.texture = item.Texture;
        rawImage.SetNativeSize();
        rawImage.rectTransform.anchorMin = anchorPivot;
        rawImage.rectTransform.anchorMax = anchorPivot;
        rawImage.rectTransform.pivot = anchorPivot;
        rawImage.rectTransform.anchoredPosition = anchoredPosition;
        var inventoryItem = itemGameObject.GetComponent<InventoryItem>();
        inventoryItem.Item = item;
        inventoryItem.OnItemPointerEnter = ShowItemPrice;
        inventoryItem.OnItemPointerExit = HideItemPrice;
        inventoryItem.OnItemPointerDown = OnBuyingItemPointerDown;
    }

    private void ShowItemPrice(Item item, PointerEventData eventData) {
        var buyItemUseCase = new BuyItemUseCase(this);
        buyItemUseCase.AskItemPrice(item, Game.Instance.PartyStats.Chars[0], 1.5f); // TODO: buying char, value multiplier
    }

    private void HideItemPrice(Item item, PointerEventData eventData) {
        dialogText.text = "";
    }

    private void OnBuyingItemPointerDown(Item item, PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var buyItemUseCase = new BuyItemUseCase(this);
            buyItemUseCase.BuyItem(item, Game.Instance.PartyStats.Chars[0], 1.5f); // TODO: buying char, value multiplier
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            itemInfoUI.Show(item);
        }
    }

    private void OnSellingItemFromInventoryPointerDown(Inventory inventory, Item item, PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var sellItemUseCase = new SellItemUseCase(this);
            sellItemUseCase.SellItem(item, Game.Instance.PartyStats.Chars[0], 1.5f); // TODO: selling char, value multiplier
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // TODO: spell info
            itemInfoUI.Show(item);
        }
    }

    private void OnSellingItemFromInventoryPointerEnter(Inventory inventory, Item item, PointerEventData eventData) {
        var sellItemUseCase = new SellItemUseCase(this);
        sellItemUseCase.AskItemPrice(item, Game.Instance.PartyStats.Chars[0], 1.5f); // TODO: selling char, value multiplier
    }

    private void OnSellingItemFromInventoryPointerExit(Inventory inventory, Item item, PointerEventData eventData) {
        dialogText.text = "";
    }

    #region BuySellItemViewInterface implementation
    public void ShowError(PlayingCharacter buyer, string errorText)
    {
        // TODO: portrait sad face
        dialogText.text = errorText;
    }

    public void ShowItemPrice(string priceText)
    {
        dialogText.text = priceText;
    }

    public void RefreshGold()
    {
        // TODO: refresh gold
    }

    public void NotifySuccessfulOperation(Item item, PlayingCharacter buyerSeller)
    {
        // TODO: gold sound
        // TODO: portrait happy face
        dialogText.text = "";
        foreach (var ii in videoImage.transform.GetComponentsInChildren<InventoryItem>())
            if (ii.Item == item)
                Destroy(ii.gameObject);
    }
    #endregion
}
