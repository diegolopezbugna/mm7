using System;

namespace Business
{
    public interface BuyItemViewInterface
    {
        void ShowError(PlayingCharacter buyer, string errorText);
        void ShowItemPrice(string priceText);
        void RefreshGold();
        void NotifySuccessfulBuy(Item item, PlayingCharacter buyer);
    }
}

