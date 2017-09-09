using System;

namespace Business
{
    public interface BuySellItemViewInterface
    {
        void ShowError(PlayingCharacter buyer, string errorText);
        void ShowItemPrice(string priceText);
        void RefreshGold();
        void NotifySuccessfulOperation(Item item, PlayingCharacter buyerSeller);
    }
}

