using System;
using UnityEngine;
using Infrastructure;

namespace Business
{
    public class SellItemUseCase
    {
        private BuySellItemViewInterface view;
        
        public SellItemUseCase(BuySellItemViewInterface view)
        {
            this.view = view;
        }

        private int GetMerchantPrice(Item item, int totalMerchantBonus, float shopValueMultiplier) {
            var merchantPrice = Mathf.CeilToInt(item.Value / (shopValueMultiplier + 2f)); // TODO: reduced price by merchant bonus
            return merchantPrice > item.Value ? item.Value : merchantPrice;
        }

        public void AskItemPrice(Item item, PlayingCharacter seller, float shopValueMultiplier) {
            var normalPrice = Mathf.CeilToInt(item.Value / (shopValueMultiplier + 2f));
            int totalMerchantBonus = seller.GetTotalMerchantBonus();
            var price = GetMerchantPrice(item, totalMerchantBonus, shopValueMultiplier);

            var priceText = "";
            if (totalMerchantBonus == 0)
                priceText = Localization.Instance.Get("MerchantBuyText1", price);
            else if (price == item.Value)
                priceText = Localization.Instance.Get("MerchantBuyText3", item.Name, normalPrice, price);
            else
                priceText = Localization.Instance.Get("MerchantBuyText2", item.Name, normalPrice, price);
            view.ShowItemPrice(priceText);
        }

        public void SellItem(Item item, PlayingCharacter seller, float shopValueMultiplier) {
            int price = GetMerchantPrice(item, seller.GetTotalMerchantBonus(), shopValueMultiplier);

            seller.Inventory.RemoveItem(item);
            Game.Instance.PartyStats.Gold += price;
            view.RefreshGold();
            view.NotifySuccessfulOperation(item, seller);
        }

    }
}

