using System;
using UnityEngine;
using Infrastructure;

namespace Business
{
    public class BuyItemUseCase
    {
        private BuySellItemViewInterface view;
        
        public BuyItemUseCase(BuySellItemViewInterface view)
        {
            this.view = view;
        }

        private int GetMerchantPrice(Item item, int totalMerchantBonus, float shopValueMultiplier) {
            var merchantPrice = Mathf.CeilToInt(item.Value * shopValueMultiplier); // TODO: reduced price by merchant bonus
            return merchantPrice < item.Value ? item.Value : merchantPrice;
        }

        public void AskItemPrice(Item item, PlayingCharacter buyer, float shopValueMultiplier) {
            var normalPrice = Mathf.CeilToInt(item.Value * shopValueMultiplier);
            int totalMerchantBonus = buyer.GetTotalMerchantBonus();
            var price = GetMerchantPrice(item, totalMerchantBonus, shopValueMultiplier);

            var priceText = "";
            if (totalMerchantBonus == 0)
                priceText = Localization.Instance.Get("MerchantSellText1", item.NotIdentifiedName, price);
            else if (price == item.Value)
                priceText = Localization.Instance.Get("MerchantSellText3", item.NotIdentifiedName, normalPrice, price);
            else
                priceText = Localization.Instance.Get("MerchantSellText2", item.NotIdentifiedName, normalPrice, price);
            view.ShowItemPrice(priceText);
        }

        public void BuyItem(Item item, PlayingCharacter buyer, float shopValueMultiplier) {
            int price = GetMerchantPrice(item, buyer.GetTotalMerchantBonus(), shopValueMultiplier);

            if (Game.Instance.PartyStats.Gold < price)
            {
                view.ShowError(buyer, Localization.Instance.Get("ImSorryButYouDontHaveEnoughMoney", buyer.Name));
                return;
            }

            if (!buyer.Inventory.TryInsertItem(item))
            {
                view.ShowError(buyer, Localization.Instance.Get("YourPacksAreFull"));
                return;
            }

            Game.Instance.PartyStats.Gold -= price;
            view.RefreshGold();
            view.NotifySuccessfulOperation(item, buyer);
        }

    }
}

