using System;

namespace Business
{
    public interface BuySellItemViewInterface
    {
        void Hide();
        void ShowError(string errorText);
        void ShowItemPrice(string priceText);
        void NotifySuccessfulOperation(Item item, PlayingCharacter buyerSeller);
    }
}

