using System;

namespace Business
{
    public interface BuySellItemViewInterface
    {
        void Hide();
        void ShowError(PlayingCharacter buyer, string errorText);
        void ShowItemPrice(string priceText);
        void RefreshGoldAndFood();
        void NotifySuccessfulOperation(Item item, PlayingCharacter buyerSeller);
    }
}

