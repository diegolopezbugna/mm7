using System;
using UnityEngine;
using Infrastructure;

namespace Business
{
    public class SellItemUseCase
    {
        private BuySellItemViewInterface View;
        private PlayingCharacterViewInterface PlayingCharacterView;

        public SellItemUseCase(BuySellItemViewInterface view, PlayingCharacterViewInterface playingCharacterView)
        {
            View = view;
            PlayingCharacterView = playingCharacterView;
        }

        private int GetMerchantPrice(Item item, int totalMerchantBonus, float shopValueMultiplier) {
            var merchantPrice = Mathf.CeilToInt(item.Value / (shopValueMultiplier + 2f)); // TODO: reduced price by merchant bonus
            return merchantPrice > item.Value ? item.Value : merchantPrice;
        }

        public void AskItemPrice(Item item, PlayingCharacter seller, float shopValueMultiplier) {
            var normalPrice = Mathf.CeilToInt(item.Value / (shopValueMultiplier + 2f));
            int totalMerchantBonus = seller.GetTotalSkillBonus(SkillCode.Merchant);
            var price = GetMerchantPrice(item, totalMerchantBonus, shopValueMultiplier);

            var priceText = "";
            if (totalMerchantBonus == 0)
                priceText = Localization.Instance.Get("MerchantBuyText1", price);
            else if (price == item.Value)
                priceText = Localization.Instance.Get("MerchantBuyText3", item.Name, normalPrice, price);
            else
                priceText = Localization.Instance.Get("MerchantBuyText2", item.Name, normalPrice, price);
            View.ShowItemPrice(priceText);
        }

        public void SellItem(Item item, PlayingCharacter seller, float shopValueMultiplier) {
            int price = GetMerchantPrice(item, seller.GetTotalSkillBonus(SkillCode.Merchant), shopValueMultiplier);

            seller.Inventory.RemoveItem(item);
            Game.Instance.PartyStats.Gold += price;
            PlayingCharacterView.RefreshGoldAndFood();
            View.NotifySuccessfulOperation(item, seller);
        }

    }
}

