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

public class ShopUI : MonoBehaviour, BuySellItemViewInterface {

    [SerializeField]
    private RawImage videoImage;

    [SerializeField]
    private Text dialogText;

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

    private VideoBuildingUI videoBuildingUI;

    void Awake() {
        Party.Instance.PlayingCharacterSelectedChanged += Party_Instance_PlayingCharacterSelectedChanged;
        videoBuildingUI = VideoBuildingUI.Instance;
    }

    void Party_Instance_PlayingCharacterSelectedChanged (object sender, EventArgs e)
    {
        if (videoBuildingUI.IsShowing)
        {
            var seller = GetSellerNpc();
            videoBuildingUI.ShowTopics(seller != null ? seller : videoBuildingUI.Npcs[0]);
            if (GetComponentInChildren<InventoryUI>() != null)
                ShowShop(ShopActionType.Sell, seller.Shop, null, null);
        }
    }

    public void Clean()
    {
        foreach (Transform child in videoImage.transform)
            Destroy(child.gameObject);
    }

    public void OnTopicClicked(Npc npc, NpcTopic npcTopic) 
    {
        foreach (Transform child in videoImage.transform)
            Destroy(child.gameObject);
        var centeredBottomPivot = new Vector2(0.5f, 0f);
        var centeredTopPivot = new Vector2(0.5f, 1f);

        if (npcTopic.ShopActionType != ShopActionType.None && npc.Shop != null)
        {
            if (npc.Shop.ShopType == ShopType.Inn)
            {
                if (npcTopic.ShopActionType == ShopActionType.RentRoom)
                {
                    var rentRoomUseCase = new RentRoomUseCase(this, RestUI.Instance, Party.Instance);
                    rentRoomUseCase.RentRoom(npc.Shop.ShopMultiplier, Party.Instance.GetPlayingCharacterSelectedOrDefault());
                }
                else if (npcTopic.ShopActionType == ShopActionType.BuyFood)
                {
                    var rentRoomUseCase = new RentRoomUseCase(this, RestUI.Instance, Party.Instance);
                    rentRoomUseCase.BuyFood(npc.Shop.ShopMultiplier, Party.Instance.GetPlayingCharacterSelectedOrDefault());
                }
            }
            else if (npc.Shop.ShopType == ShopType.Healer)
            {
                if (npcTopic.ShopActionType == ShopActionType.Heal)
                {
                    var playingCharacterHealsUseCase = new PlayingCharacterHealsUseCase(Party.Instance, this);
                    playingCharacterHealsUseCase.HealAtHealer(npc.Shop.ShopMultiplier, Party.Instance.GetPlayingCharacterSelectedOrDefault());
                }
                if (npcTopic.ShopActionType == ShopActionType.Donate)
                {
                    // TODO: move to use case, do something
                    Game.Instance.PartyStats.Gold -= Mathf.CeilToInt(npc.Shop.ShopMultiplier);
                    if (Game.Instance.PartyStats.Gold < 0)
                        Game.Instance.PartyStats.Gold = 0;
                    Party.Instance.RefreshGoldAndFood();
                }
            }
            else if (npc.Shop.ShopType == ShopType.WeaponSmith)
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
            inventoryUI.Inventory = Party.Instance.GetPlayingCharacterSelectedOrDefault().Inventory;
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
        var buyItemUseCase = new BuyItemUseCase(this, Party.Instance);
        buyItemUseCase.AskItemPrice(item, Party.Instance.GetPlayingCharacterSelectedOrDefault(), GetSellerNpc().Shop.ShopMultiplier);
    }

    private void HideItemPrice(Item item, PointerEventData eventData) {
        dialogText.text = "";
    }

    private void OnBuyingItemPointerDown(Item item, PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var buyItemUseCase = new BuyItemUseCase(this, Party.Instance);
            buyItemUseCase.BuyItem(item, Party.Instance.GetPlayingCharacterSelectedOrDefault(), GetSellerNpc().Shop.ShopMultiplier);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item.EquipSlot == EquipSlot.Book)
            {
                var spellInfo = item.GetSpellInfoAssociated();
                itemInfoUI.Show(spellInfo);
            }
            else
                itemInfoUI.Show(item);
        }
    }

    private void OnSellingItemFromInventoryPointerDown(Inventory inventory, Item item, PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var sellItemUseCase = new SellItemUseCase(this, Party.Instance);
            sellItemUseCase.SellItem(item, Party.Instance.GetPlayingCharacterSelectedOrDefault(), GetSellerNpc().Shop.ShopMultiplier);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            itemInfoUI.Show(item);
        }
    }

    private void OnSellingItemFromInventoryPointerEnter(Inventory inventory, Item item, PointerEventData eventData) {
        var sellItemUseCase = new SellItemUseCase(this, Party.Instance);
        sellItemUseCase.AskItemPrice(item, Party.Instance.GetPlayingCharacterSelectedOrDefault(), GetSellerNpc().Shop.ShopMultiplier);
    }

    private void OnSellingItemFromInventoryPointerExit(Inventory inventory, Item item, PointerEventData eventData) {
        dialogText.text = "";
    }

    private Npc GetSellerNpc() {
        foreach (var npc in videoBuildingUI.Npcs)
            if (npc.Shop != null)
                return npc;
        return null;
    }

    #region BuySellItemViewInterface implementation

    public void Hide()
    {
        videoBuildingUI.Hide();
    }

    public void ShowError(string errorText)
    {
        dialogText.text = errorText;
    }

    public void ShowItemPrice(string priceText)
    {
        dialogText.text = priceText;
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
