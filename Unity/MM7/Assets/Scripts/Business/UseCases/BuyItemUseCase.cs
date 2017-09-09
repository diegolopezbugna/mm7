using System;
using UnityEngine;
using Infrastructure;

namespace Business
{
    public class BuyItemUseCase
    {
        private BuyItemViewInterface view;
        
        public BuyItemUseCase(BuyItemViewInterface view)
        {
            this.view = view;
        }

        private int GetReducedPrice(Item item, PlayingCharacter buyer, float shopValueMultiplier) {
            return Mathf.CeilToInt(item.Value * shopValueMultiplier); // TODO: reduced price by merchant bonus
        }

        public void AskItemPrice(Item item, PlayingCharacter buyer, float shopValueMultiplier) {
            var normalPrice = Mathf.CeilToInt(item.Value * shopValueMultiplier);
            int totalMerchantBonus = buyer.GetTotalMerchantBonus();
            var price = GetReducedPrice(item, buyer, shopValueMultiplier);

            var priceText = "";
            if (totalMerchantBonus == 0)
                priceText = Localization.Instance.Get("MerchantSellText1", item.NotIdentifiedName, price);
            else if (totalMerchantBonus < 10) // TODO: totalMerchantBonus < 10 ??
                priceText = Localization.Instance.Get("MerchantSellText2", item.NotIdentifiedName, normalPrice, price);
            else
                priceText = Localization.Instance.Get("MerchantSellText3", item.NotIdentifiedName, normalPrice, price);
            view.ShowItemPrice(priceText);
        }

        public void BuyItem(Item item, PlayingCharacter buyer, float shopValueMultiplier) {
            int price = GetReducedPrice(item, buyer, shopValueMultiplier);

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
            view.NotifySuccessfulBuy(item, buyer);
        }

    }
}

