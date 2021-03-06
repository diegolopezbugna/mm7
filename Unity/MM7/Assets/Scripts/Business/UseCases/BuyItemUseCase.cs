﻿using System;
using UnityEngine;
using Infrastructure;

namespace Business
{
    public class BuyItemUseCase
    {
        private BuySellItemViewInterface View;
        private PlayingCharacterViewInterface PlayingCharacterView;

        public BuyItemUseCase(BuySellItemViewInterface view, PlayingCharacterViewInterface playingCharacterView)
        {
            View = view;
            PlayingCharacterView = playingCharacterView;
        }

        private int GetMerchantPrice(Item item, int totalMerchantBonus, float shopValueMultiplier) {
            var merchantPrice = Mathf.CeilToInt(item.Value * shopValueMultiplier); // TODO: reduced price by merchant bonus
            return merchantPrice < item.Value ? item.Value : merchantPrice;
        }

        public void AskItemPrice(Item item, PlayingCharacter buyer, float shopValueMultiplier) {
            var normalPrice = Mathf.CeilToInt(item.Value * shopValueMultiplier);
            int totalMerchantBonus = buyer.GetTotalSkillBonus(SkillCode.Merchant);
            var price = GetMerchantPrice(item, totalMerchantBonus, shopValueMultiplier);

            var priceText = "";
            if (totalMerchantBonus == 0)
                priceText = Localization.Instance.Get("MerchantSellText1", item.Name, price);
            else if (price == item.Value)
                priceText = Localization.Instance.Get("MerchantSellText3", item.Name, normalPrice, price);
            else
                priceText = Localization.Instance.Get("MerchantSellText2", item.Name, normalPrice, price);
            View.ShowItemPrice(priceText);
        }

        public void BuyItem(Item item, PlayingCharacter buyer, float shopValueMultiplier) {
            int price = GetMerchantPrice(item, buyer.GetTotalSkillBonus(SkillCode.Merchant), shopValueMultiplier);

            if (Game.Instance.PartyStats.Gold < price)
            {
                View.ShowError(Localization.Instance.Get("ImSorryButYouDontHaveEnoughMoney", buyer.Name));
                return;
            }

            if (!buyer.Inventory.TryInsertItem(item))
            {
                View.ShowError(Localization.Instance.Get("YourPacksAreFull"));
                return;
            }

            Game.Instance.PartyStats.Gold -= price;
            PlayingCharacterView.RefreshGoldAndFood();
            View.NotifySuccessfulOperation(item, buyer);
        }

    }
}

