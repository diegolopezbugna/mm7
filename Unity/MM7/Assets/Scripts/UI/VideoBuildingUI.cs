using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using Business;
using Infrastructure;

public class VideoBuildingUI : BaseUI<VideoBuildingUI>, BuyItemViewInterface {

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
    private Texture textureArmorWeapon;

    [SerializeField]
    private Texture textureMagicWeapon;

    [SerializeField]
    private GameObject itemTemplatePrefab;

    [SerializeField]
    private ItemInfoUI itemInfoUI;


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

    public override void Start()
    {
        base.Start();
        Show(Building.GetByLocationCode("1"), Npc.GetByLocationCode("1"));
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

        if (npcTopic.ShopActionType != ShopActionType.None)
        {
            if (npc.ShopType == ShopType.WeaponSmith)
                ShowWeaponShop(npcTopic.ShopActionType);
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
    public void ShowWeaponShop(ShopActionType shopActionType)
    {
        if (shopActionType == ShopActionType.BuyStandard || shopActionType == ShopActionType.BuySpecial)
        {
            videoImage.texture = textureWeaponShop;
            // TODO: don't refresh items each time you enter shop
            var items = Item.GetStandardItemsToBuyAt(ShopType.WeaponSmith, shopActionType == ShopActionType.BuySpecial ? 2 : 1); // TODO: treasure level??
            var x = 30;
            foreach (var i in items)
            {
                DrawItem(i, new Vector2(x, (textureWeaponShop.height - i.Texture.height) / 2));
                x += (textureWeaponShop.width - 30) / (items.Count);
            }
        }
        else if (shopActionType == ShopActionType.Sell)
        {
            
        }
    }

    private void DrawItem(Item item, Vector2 positionFromZero)
    {
        var itemGameObject = Instantiate(itemTemplatePrefab, videoImage.transform);
        var rawImage = itemGameObject.GetComponent<RawImage>();
        rawImage.texture = item.Texture;
        rawImage.SetNativeSize();
        rawImage.rectTransform.anchorMin = Vector2.zero;
        rawImage.rectTransform.anchorMax = Vector2.zero;
        rawImage.rectTransform.pivot = Vector2.zero;
        rawImage.rectTransform.anchoredPosition = positionFromZero;
        var inventoryItem = itemGameObject.GetComponent<InventoryItem>();
        inventoryItem.Item = item;
        inventoryItem.OnItemPointerEnter = ShowItemPrice;
        inventoryItem.OnItemPointerExit = HideItemPrice;
        inventoryItem.OnItemPointerDown = OnSellingItemPointerDown;
    }

    private void ShowItemPrice(Item item, PointerEventData eventData) {
        var buyItemUseCase = new BuyItemUseCase(this);
        buyItemUseCase.AskItemPrice(item, Game.Instance.PartyStats.Chars[0], 1.5f); // TODO: buying char, value multiplier
    }

    private void HideItemPrice(Item item, PointerEventData eventData) {
        dialogText.text = "";
    }

    private void OnSellingItemPointerDown(Item item, PointerEventData eventData) {
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

    #region BuyItemViewInterface implementation
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

    public void NotifySuccessfulBuy(Item item, PlayingCharacter buyer)
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
