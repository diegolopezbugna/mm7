using System;
using Infrastructure;

namespace Business
{
    public class RentRoomUseCase
    {
        private BuySellItemViewInterface View;
        private PartyRestsViewInterface PartyRestsView;
        private PlayingCharacterViewInterface PlayingCharacterView;

        public RentRoomUseCase(BuySellItemViewInterface view, PartyRestsViewInterface partyRestsView, PlayingCharacterViewInterface playingCharacterView)
        {
            View = view;
            PartyRestsView = partyRestsView;
            PlayingCharacterView = playingCharacterView;
        }

        public void RentRoom(int shopMultiplier, PlayingCharacter buyer)
        {
            var rentCost = shopMultiplier / 2; // TODO: reduced price by merchant bonus
            if (Game.Instance.PartyStats.Gold >= rentCost)
            {
                Game.Instance.PartyStats.Gold -= shopMultiplier / 2;
                View.RefreshGoldAndFood();
                View.Hide();
                PartyRestsView.Show();
                var partyRestsUseCase = new PartyRestsUseCase(PartyRestsView, PlayingCharacterView);
                partyRestsUseCase.RestAndHeal(true);
            }
            else
            {
                View.ShowError(Localization.Instance.Get("ImSorryButYouDontHaveEnoughMoney", buyer.Name));
            }
        }

        public void BuyFood(int shopMultiplier, PlayingCharacter buyer)
        {
            var foodCost = shopMultiplier / 3; // TODO: reduced price by merchant bonus
            if (Game.Instance.PartyStats.Gold >= foodCost)
            {
                var foodToBuy = shopMultiplier;
                if (Game.Instance.PartyStats.Food < foodToBuy)
                {
                    Game.Instance.PartyStats.Food += foodToBuy;
                    Game.Instance.PartyStats.Gold -= foodCost; // TODO: reduced price by merchant bonus
                    View.RefreshGoldAndFood();
                }
                else
                {
                    View.ShowError(Localization.Instance.Get("YourPacksAreFull"));
                }
            }
            else
            {
                View.ShowError(Localization.Instance.Get("ImSorryButYouDontHaveEnoughMoney", buyer.Name));
            }
        }
    }
}

